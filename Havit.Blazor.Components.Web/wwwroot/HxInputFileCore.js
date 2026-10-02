// Input elements with an upload started, by element id.
// Used by dispose() as the element may already be removed from the DOM when the component is being disposed.
const uploadingInputElements = new Map();

export function upload(inputElementId, hxInputFileDotnetObjectReference, uploadEndpointUrl, accessToken, maxFileSize, maxParallelUploads, uploadHttpMethod, antiforgeryHeaderName, antiforgeryToken) {
	const inputElement = document.getElementById(inputElementId);
	const files = inputElement.files;
	let totalSize = 0;

	inputElement.requests = new Array();
	inputElement.cancelled = false;
	inputElement.disposed = false;
	uploadingInputElements.set(inputElementId, inputElement);

	// Once disposed, the DotNetObjectReference is (being) disposed and must not be used anymore.
	const dotnetReference = {
		invokeMethodAsync: function (methodName, ...args) {
			if (inputElement.disposed) {
				return Promise.resolve();
			}
			return hxInputFileDotnetObjectReference.invokeMethodAsync(methodName, ...args);
		}
	};

	let nextFile = maxParallelUploads;

	let completedUploads = 0;

	if (files.length === 0) {
		dotnetReference.invokeMethodAsync('HxInputFileCore_HandleUploadCompleted', 0, 0);
		return;
	}

	for (let i = 0; i < Math.min(files.length, maxParallelUploads); i++) {
		(function (curr) {
			uploadFile(curr);
		}(i));
	}

	function uploadFile(index) {

		if (inputElement && inputElement.cancelled && inputElement.cancelled === true) {
			return;
		}

		const file = files[index];

		if (maxFileSize && (file.size > maxFileSize)) {
			const msg = `[${index}]${file.name} client pre-check: File size ${file.size} bytes exceeds MaxFileSize limit ${maxFileSize} bytes.`;
			console.warn(msg);

			dotnetReference.invokeMethodAsync('HxInputFileCore_HandleFileUploaded', index, file.name, file.size, file.type, file.lastModified, 413, msg);

			if (nextFile < files.length) {
				uploadFile(nextFile);
				nextFile++;
			}

			return;
		}

		if (file && file.size) {
			totalSize = totalSize + file.size;
		}

		const data = new FormData();
		data.append('file', file, file.name);

		const request = new XMLHttpRequest();
		inputElement.requests.push(request);

		request.open(uploadHttpMethod, uploadEndpointUrl, true);

		if (accessToken) {
			request.setRequestHeader('Authorization', 'Bearer ' + accessToken);
		}

        if (antiforgeryToken) {
            request.setRequestHeader(antiforgeryHeaderName, antiforgeryToken);
        }

		request.upload.onprogress = function (e) {
			dotnetReference.invokeMethodAsync('HxInputFileCore_HandleUploadProgress', index, file.name, e.loaded, e.total);
		};
		request.onreadystatechange = function () {
			if (request.readyState === 4) {
				completedUploads++;
				dotnetReference.invokeMethodAsync('HxInputFileCore_HandleFileUploaded', index, file.name, file.size, file.type, file.lastModified, request.status, request.responseText);

				if (nextFile < files.length) {
					uploadFile(nextFile);
					nextFile++;
				}
			};
			if (completedUploads === files.length) {
				dotnetReference.invokeMethodAsync('HxInputFileCore_HandleUploadCompleted', files.length, totalSize);
			}
		}

		request.send(data);
    }
}

export function getFiles(inputElementId) {
	const inputElement = document.getElementById(inputElementId);
	inputElement.hxInputFileNextFileIndex = 0;
	return Array.from(inputElement.files).map(e => { return { index: inputElement.hxInputFileNextFileIndex++, name: e.name, lastModified: e.lastModified, size: e.size, type: e.type }; });
}

export function reset(inputElementId) {
	const inputElement = document.getElementById(inputElementId);
	if (!inputElement) {
		return;
	}

	inputElement.cancelled = true;

	if (inputElement.requests) {
		for (const request of inputElement.requests) {
			request.abort();
		}
	}

	inputElement.value = '';
	inputElement.dispatchEvent(new Event('change'));
}

export function dispose(inputElementId) {
	const inputElement = uploadingInputElements.get(inputElementId) ?? document.getElementById(inputElementId);
	uploadingInputElements.delete(inputElementId);
	if (!inputElement) {
		return;
	}

	// Set the flags before aborting the requests - abort() fires onreadystatechange synchronously.
	inputElement.disposed = true;
	inputElement.cancelled = true;

	if (inputElement.requests) {
		for (const request of inputElement.requests) {
			request.abort();
		}
	}
}
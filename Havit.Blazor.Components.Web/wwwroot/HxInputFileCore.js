export function upload(inputElementId, hxInputFileDotnetObjectReference, uploadEndpointUrl, accessToken, maxFileSize, maxParallelUploads) {
	var inputElement = document.getElementById(inputElementId);
	var dotnetReference = hxInputFileDotnetObjectReference;
	var files = inputElement.files;
	var totalSize = 0;

	inputElement.requests = new Array();
	inputElement.cancelled = false;

	var nextFile = maxParallelUploads;

	var completedUploads = 0;

	if (files.length === 0) {
		dotnetReference.invokeMethodAsync('HxInputFileCore_HandleUploadCompleted', 0, 0);
		return;
	}

	for (var i = 0; i < Math.min(files.length, maxParallelUploads); i++) {
		(function (curr) {
			uploadFile(curr);
		}(i));
	}

	function uploadFile(index) {

		if (inputElement && inputElement.cancelled && inputElement.cancelled === true) {
			return;
		}

		var file = files[index];

		if (maxFileSize && (file.size > maxFileSize)) {
			let msg = `[${index}]${file.name} client pre-check: File size ${file.size} bytes exceeds MaxFileSize limit ${maxFileSize} bytes.`;
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

		var data = new FormData();
		data.append('file', file, file.name);

		var request = new XMLHttpRequest();
		inputElement.requests.push(request);

		request.open('POST', uploadEndpointUrl, true);

		if (accessToken) {
			request.setRequestHeader('Authorization', 'Bearer ' + accessToken);
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
	var inputElement = document.getElementById(inputElementId);
	inputElement.hxInputFileNextFileIndex = 0;
	return Array.from(inputElement.files).map(e => { return { index: inputElement.hxInputFileNextFileIndex++, name: e.name, lastModified: e.lastModified, size: e.size, type: e.type }; });
}

export function reset(inputElementId) {
	var inputElement = document.getElementById(inputElementId);
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
	var inputElement = document.getElementById(inputElementId);
	inputElement.hxInputFileDotnetObjectReference = null;
}
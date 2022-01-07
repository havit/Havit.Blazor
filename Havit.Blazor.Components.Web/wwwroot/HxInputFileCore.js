var nextFile = 0;

export function upload(inputElementId, hxInputFileDotnetObjectReference, uploadEndpointUrl, accessToken, maxFileSize, maxParallelUploads) {
	var inputElement = document.getElementById(inputElementId);
	var dotnetReference = hxInputFileDotnetObjectReference;
	var files = inputElement.files;
	var totalSize = 0;

	if (maxParallelUploads > files.length) {
		maxParallelUploads = files.length;
		nextFile = files.length;
	}
	else {
		nextFile = maxParallelUploads;
    }

	uploadFiles(files, accessToken, maxFileSize, dotnetReference, totalSize, maxParallelUploads, 0, uploadEndpointUrl);
}

function uploadFiles(files, accessToken, maxFileSize, dotnetReference, totalSize, endIndex, startIndex, uploadEndpointUrl) {
	if (files.length === 0) {
		dotnetReference.invokeMethodAsync('HxInputFileCore_HandleUploadCompleted', 0, 0);
		return;
	}

	for (var i = startIndex; i < endIndex; i++) {
		(function (curr) {
			var index = curr;
			var file = files[curr];

			if (maxFileSize && (file.size > maxFileSize)) {
				let msg = `[${index}]${file.name} client pre-check: File size ${file.size} bytes exceeds MaxFileSize limit ${maxFileSize} bytes.`;
				console.warn(msg);
				dotnetReference.invokeMethodAsync('HxInputFileCore_HandleFileUploaded', index, file.name, file.size, file.type, file.lastModified, 413, msg);
				return;
			}

			if (file && file.size) {
				totalSize = totalSize + file.size;
            }

			var data = new FormData();
			data.append('file', file, file.name);

			var request = new XMLHttpRequest();
			request.open('POST', uploadEndpointUrl, true);

			if (accessToken) {
				request.setRequestHeader('Authorization', 'Bearer ' + accessToken);
			}

			request.upload.onprogress = function (e) {
				dotnetReference.invokeMethodAsync('HxInputFileCore_HandleUploadProgress', index, file.name, e.loaded, e.total);
			};
			request.onreadystatechange = function () {
				if (request.readyState === 4) {
					dotnetReference.invokeMethodAsync('HxInputFileCore_HandleFileUploaded', index, file.name, file.size, file.type, file.lastModified, request.status, request.responseText);

					if (nextFile < files.length) {
						uploadFiles(files, accessToken, maxFileSize, dotnetReference, totalSize, nextFile + 1, nextFile, uploadEndpointUrl);
						nextFile++;
                    }
				};
				if (nextFile >= files.length) {
					dotnetReference.invokeMethodAsync('HxInputFileCore_HandleUploadCompleted', files.length, totalSize);
				}
			}

			request.send(data);
		}(i));
	}
}

export function getFiles(inputElementId) {
	var inputElement = document.getElementById(inputElementId);
	inputElement.hxInputFileNextFileIndex = 0;
	return Array.from(inputElement.files).map(e => { return { index: inputElement.hxInputFileNextFileIndex++, name: e.name, lastModified: e.lastModified, size: e.size, type: e.type }; });
}

export function reset(inputElementId) {
	var inputElement = document.getElementById(inputElementId);
	inputElement.value = '';
	inputElement.dispatchEvent(new Event('change'));
}

export function dispose(inputElementId) {
	var inputElement = document.getElementById(inputElementId);
	inputElement.hxInputFileDotnetObjectReference = null;
}
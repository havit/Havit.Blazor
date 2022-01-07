var nextFile = new Array(); // Stores the next files to upload for different uploads.
var requests =
{
	"id": new Array()
};
var cancelled =
{
	"id": false
};

export function upload(inputElementId, hxInputFileDotnetObjectReference, uploadEndpointUrl, accessToken, maxFileSize, maxParallelUploads) {
	var inputElement = document.getElementById(inputElementId);
	var dotnetReference = hxInputFileDotnetObjectReference;
	var files = inputElement.files;
	var totalSize = 0;

	requests[inputElementId] = new Array();
	cancelled[inputElementId] = false;

	let uploadIndex = nextFile.length;

	if (maxParallelUploads > files.length) {
		maxParallelUploads = files.length;
		nextFile.push(files.length);
	}
	else {
		nextFile.push(maxParallelUploads);
    }

	uploadFiles(files, accessToken, maxFileSize, dotnetReference, totalSize, maxParallelUploads, 0, uploadEndpointUrl, uploadIndex, inputElementId);
}

function uploadFiles(files, accessToken, maxFileSize, dotnetReference, totalSize, endIndex, startIndex, uploadEndpointUrl, uploadIndex, inputElementId) {
	console.log(inputElementId);
	console.log(cancelled[inputElementId]);

	if (cancelled[inputElementId] === true) {
		console.log("cancelled");
		return;
    }

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

			var requestId = requests[inputElementId].length;

			requests[inputElementId].push(new XMLHttpRequest());

			requests[inputElementId][requestId].open('POST', uploadEndpointUrl, true);

			if (accessToken) {
				requests[inputElementId][requestId].setRequestHeader('Authorization', 'Bearer ' + accessToken);
			}

			requests[inputElementId][requestId].upload.onprogress = function (e) {
				dotnetReference.invokeMethodAsync('HxInputFileCore_HandleUploadProgress', index, file.name, e.loaded, e.total);
			};
			requests[inputElementId][requestId].onreadystatechange = function () {
				if (requests[inputElementId][requestId].readyState === 4) {
					dotnetReference.invokeMethodAsync('HxInputFileCore_HandleFileUploaded', index, file.name, file.size, file.type, file.lastModified, requests[inputElementId][requestId].status, requests[inputElementId][requestId].responseText);

					if (nextFile[uploadIndex] < files.length) {
						console.log(nextFile[uploadIndex]);

						uploadFiles(files, accessToken, maxFileSize, dotnetReference, totalSize, nextFile[uploadIndex] + 1, nextFile[uploadIndex], uploadEndpointUrl, uploadIndex, inputElementId);
						nextFile[uploadIndex] = nextFile[uploadIndex] + 1;
                    }
				};
				if (nextFile[uploadIndex] >= files.length) {
					dotnetReference.invokeMethodAsync('HxInputFileCore_HandleUploadCompleted', files.length, totalSize);
				}
			}

			requests[inputElementId][requestId].send(data);
		}(i));
	}
}

export function getFiles(inputElementId) {
	var inputElement = document.getElementById(inputElementId);
	inputElement.hxInputFileNextFileIndex = 0;
	return Array.from(inputElement.files).map(e => { return { index: inputElement.hxInputFileNextFileIndex++, name: e.name, lastModified: e.lastModified, size: e.size, type: e.type }; });
}

export function reset(inputElementId) {
	console.log("cancel button pressed " + inputElementId);

	cancelled[inputElementId] = true;
	for (const request of requests[inputElementId]) {
		request.abort();
    }
}

export function dispose(inputElementId) {
	var inputElement = document.getElementById(inputElementId);
	inputElement.hxInputFileDotnetObjectReference = null;
}
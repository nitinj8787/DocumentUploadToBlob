using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using System.IO;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using testDocUpload.Response;

namespace testDocUpload.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly string CONTAINER_NAME = "documents";

        private readonly BlobContainerClient _blobContainerClient;

        // Initialize this controller with dev emulator storage account for local tseting and blob container
        public DocumentsController()
        {
            _blobContainerClient = new BlobContainerClient("UseDevelopmentStorage=true", CONTAINER_NAME);
            _blobContainerClient.CreateIfNotExists();
        }

        /// <summary>
        /// returns the list of documnets from blob storage, response is of type DocumentResult 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/documents")]
        public async Task<ActionResult<IEnumerable<DocumentResult>>> ListFiles()
        {
            try
            {
                var blobsInfoList = new List<DocumentResult>();

                // List all blobs in the container
                await foreach (BlobItem blobItem in _blobContainerClient.GetBlobsAsync())
                {
                    var dr = new DocumentResult { Name = blobItem.Name, Location = CONTAINER_NAME, FileSize = blobItem.Properties.ContentLength };

                    blobsInfoList.Add(dr);
                }

                return blobsInfoList;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// upload the blob to storage account
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/documents")]
        public async Task<ActionResult> UploadFile([FromForm] string fileName,  string Path)
        {
            try
            {
                string uploadfile = HelperMethod.GetFileNamePath(fileName, Path);

                // only file which has permitted extension is allowed to upload
                if (!HelperMethod.IsPermittedFile(fileName))
                    return StatusCode(415, $"Only ( {string.Join(",", HelperMethod.PERMITTED_FILE_EXTENSION)} ) extension supported");

                BlobClient _blobClient = _blobContainerClient.GetBlobClient(fileName);

                // if file size to upload is greater than 5MB, discard and return 
                long fileLength = new System.IO.FileInfo(uploadfile).Length;
                if ((fileLength / 1024) / 1024 > 5)
                    return StatusCode(413, "file size more than 5 MB is not allowed to upload");

                using var fileStream = System.IO.File.OpenRead(uploadfile);
                await _blobClient.UploadAsync(fileStream, true);

                fileStream.Close();

                // successfully created blob hence 201
                return StatusCode(201, $"{fileName} file successfully uploaded");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// download the file to the given user download path, filename is the blob name
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/documents/download")]
        public async Task<ActionResult> DownloadFile(string fileName, string Path)
        {
            try
            {
                string downloadfile = HelperMethod.GetFileNamePath(fileName, Path);

                var _blobClient = _blobContainerClient.GetBlobClient(fileName);

                // Download the blob's contents and save it to a file
                BlobDownloadInfo download = await _blobClient.DownloadAsync();

                using FileStream downloadFileStream = System.IO.File.OpenWrite(downloadfile);
                await download.Content.CopyToAsync(downloadFileStream);

                downloadFileStream.Close();

                return StatusCode(200, $"{fileName} downloaded successfully at {Path}");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        /// <summary>
        /// delete the blob from container as per filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/documents")]
        public async Task<ActionResult> Delete(string fileName)
        {
            var _blobClient = _blobContainerClient.GetBlobClient(fileName);

            var fileExist = await _blobClient.ExistsAsync();

            if (!fileExist)
                return StatusCode(200, $"No '{fileName}' file eixst for deletion");
            
            await _blobClient.DeleteIfExistsAsync();

            return StatusCode(200, $"{fileName} file deleted successfully");
        }

    }
}
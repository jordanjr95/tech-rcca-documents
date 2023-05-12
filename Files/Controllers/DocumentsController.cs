using Files.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Files.Models;
using Azure.Storage.Blobs;
using System.IO;
using Files.Services;

namespace Files.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {

        private readonly DocumentsContext _context;
        private readonly DocumentsService _documentsService;

        public DocumentsController(DocumentsService documentsService) =>
        _documentsService = documentsService;

        [HttpGet]
        public async Task<List<Documents>> Get() =>
            await _documentsService.GetAsync();

        [HttpGet("{id}")]   
        public async Task<ActionResult<Documents>> Get(string id)
        {
            var document = await _documentsService.GetAsync(id);

            if (document is null)
            {
                return NotFound();
            }

            return document;
        }

        [HttpGet("listOfDocumentsWaiting")]
        public async Task<List<Documents>> ListOfDocumentsWaiting() =>
            await _documentsService.GetWaitingApprovalAsync();

        [HttpGet("listOfDocumentsApproved")]
        public async Task<List<Documents>> ListOfDocumentsApproved() =>
            await _documentsService.GetApprovedAsync();

        [HttpPost("approveDocument/{id}")]
        public async Task<IActionResult> ApproveDocument(string id)
        {
            var document = await _documentsService.GetAsync(id);

            if (document is null)
            {
                return NotFound();
            }

            document.waitingAdminApproval = false;

            await _documentsService.ApproveAsync(id, document);

            return NoContent();
        }

        [HttpPost("rejectDocument/{id}")]
        public async Task<IActionResult> RejectDocument(string id)
        {
            var document = await _documentsService.GetAsync(id);

            if (document is null)
            {
                return NotFound();
            }

            await _documentsService.RejectAsync(id);

            return NoContent();
        }

        [HttpPost("updateDocument/{id}")]
        public async Task<IActionResult> UpdateDocument(string id, Documents updatedDocument)
        {
            var document = await _documentsService.GetAsync(id);

            if (document is null)
            { 
                return NotFound();
            }

            updatedDocument.documentID = document.documentID;

            await _documentsService.UpdateAsync(id, updatedDocument);

            return NoContent();
        }

        [HttpPost("uploadDocument")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile([FromForm] FileElements fileElements)
        {
            try
            {
                // Upload file to Azure Blob Storage
                BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=projectcars;AccountKey=X8rno/SKhFS96VGDj1bQn9zls4QzY7FUNGZ3inErG+aQTvAY/3RgMlFDBxaK0oIfuh8qhvw3DYnh+AStAmXouQ==;BlobEndpoint=https://projectcars.blob.core.windows.net/;TableEndpoint=https://projectcars.table.core.windows.net/;QueueEndpoint=https://projectcars.queue.core.windows.net/;FileEndpoint=https://projectcars.file.core.windows.net/");
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("documents");

                string fileName = Path.GetFileNameWithoutExtension(fileElements.File.FileName);
                string fileExtension = Path.GetExtension(fileElements.File.FileName);
                string blobName = $"{fileName}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}{fileExtension}";

                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                using (Stream stream = fileElements.File.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream);
                }

                // Insert link to uploaded file in SQL Server database
                Documents documentModel = new Documents
                {
                    templateID = fileElements.templateID,
                    documentReference = blobClient.Uri.AbsoluteUri,
                    waitingAdminApproval = true,
                    formElements = fileElements.elements
                };

                await _documentsService.CreateAsync(documentModel);

                return Ok("File uploaded successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading file: {ex.Message}");
            }
        }

    }
}

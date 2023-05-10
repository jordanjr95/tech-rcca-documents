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
        public async Task<ActionResult<Documents>> Get(int id)
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
        public async Task<IActionResult> ApproveDocument(int id)
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
        public async Task<IActionResult> RejectDocument(int id)
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
        public async Task<IActionResult> UpdateDocument(int id, Documents updatedDocument)
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

        //[HttpPost("uploadDocument")]
        //public async Task<IActionResult> UploadFile(IFormFile file, int templateID)
        //{
        //    try
        //    {
        //        //string connectionString = _configuration.GetConnectionString("DefaultConnection"); //documentsconnection

        //        // Upload file to Azure Blob Storage
        //        BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=projectcars;AccountKey=X8rno/SKhFS96VGDj1bQn9zls4QzY7FUNGZ3inErG+aQTvAY/3RgMlFDBxaK0oIfuh8qhvw3DYnh+AStAmXouQ==;BlobEndpoint=https://projectcars.blob.core.windows.net/;TableEndpoint=https://projectcars.table.core.windows.net/;QueueEndpoint=https://projectcars.queue.core.windows.net/;FileEndpoint=https://projectcars.file.core.windows.net/");
        //        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("documents");

        //        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
        //        string fileExtension = Path.GetExtension(file.FileName);
        //        string blobName = $"{fileName}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}{fileExtension}";

        //        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        //        using (Stream stream = file.OpenReadStream())
        //        {
        //            await blobClient.UploadAsync(stream);
        //        }

        //        // Insert link to uploaded file in SQL Server database
        //        Documents documentModel = new Documents
        //        {
        //            templateID = templateID,
        //            documentReference = blobClient.Uri.AbsoluteUri,
        //            waitingAdminApproval = true

        //        };
        //        _context.Documents.Add(documentModel);
        //        await _context.SaveChangesAsync();

        //        return Ok("File uploaded successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading file: {ex.Message}");
        //    }
        //}

    }
}

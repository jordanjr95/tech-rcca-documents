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

        //private readonly IConfiguration _configuration;
        private readonly DocumentsContext _context;
        private readonly DocumentsService _documentsService;

        public DocumentsController(DocumentsService documentsService) =>
        _documentsService = documentsService;

        [HttpGet]
        public async Task<List<Documents>> Get() =>
            await _documentsService.GetAsync();

        //[HttpPost("uploadfile")]
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

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Documents>> GetDocument(int id)
        //{
        //    if (_context.Documents == null)
        //    {
        //        return NotFound();
        //    }
        //    var template = await _context.Documents.FindAsync(id);

        //    if (template == null)
        //    {
        //        return NotFound();
        //    }

        //    return template;
        //}

        //[HttpGet("ListOfDocumentsWaiting")]
        //public async Task<ActionResult<List<Documents>>> ListOfDocumentsWaiting()
        //{
        //    if (_context.Documents == null)
        //    {
        //        return NotFound();
        //    }
        //    var template = _context.Documents.Where(m => m.waitingAdminApproval).ToList();


        //    if (template == null)
        //    {
        //        return NotFound();
        //    }

        //    return template;
        //}
    }
}

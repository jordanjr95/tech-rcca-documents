using Files.Context;
using Files.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Files.Services
{
    public class DocumentsService
    {
        private readonly IMongoCollection<Documents> _documentsCollection;

        public DocumentsService(
        IOptions<DocumentsDatabaseSettings> documentDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                documentDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                documentDatabaseSettings.Value.DatabaseName);

            _documentsCollection = mongoDatabase.GetCollection<Documents>(
                documentDatabaseSettings.Value.DocumentsCollectionName);
        }

        public async Task<List<Documents>> GetAsync() =>
            await _documentsCollection.Find(_ => true).ToListAsync();
    }
}
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

        public async Task<Documents?> GetAsync(string id) =>
            await _documentsCollection.Find(x => x.documentID == id).FirstOrDefaultAsync();

        public async Task<List<Documents>> GetWaitingApprovalAsync() =>
            await _documentsCollection.Find(x => x.waitingAdminApproval == true).ToListAsync();

        public async Task<List<Documents>> GetApprovedAsync() =>
            await _documentsCollection.Find(x => x.waitingAdminApproval == false).ToListAsync();

        public async Task ApproveAsync(string id, Documents approvedDocument) =>
            await _documentsCollection.ReplaceOneAsync(x => x.documentID == id, approvedDocument);

        public async Task RejectAsync(string id) =>
            await _documentsCollection.DeleteOneAsync(x => x.documentID == id);

        public async Task UpdateAsync(string id, Documents updatedDocument) =>
            await _documentsCollection.ReplaceOneAsync(x => x.documentID == id, updatedDocument);

        public async Task CreateAsync(Documents newDocument) =>
            await _documentsCollection.InsertOneAsync(newDocument);
    }
}
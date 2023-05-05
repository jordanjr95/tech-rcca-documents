using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Files.Models
{
    public class Documents
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("documentID")]
        public int documentID { get; set; }

        [BsonElement("templateID")]
        public int templateID { get; set; }

        [BsonElement("documentReference")]
        public string documentReference { get; set; }

        [BsonElement("waitingforAdminApproval")]
        public bool waitingAdminApproval { get; set; }

        [BsonElement("formElements")]
        public FormElements formElements { get; set; }
    }
}

using MongoDB.Bson.Serialization.Attributes;

namespace Files.Models
{
    public class FileElements
    {
        [BsonElement("templateID")]
        public string templateID { get; set; }
        public IFormFile File { get; set; }
        public FormElements elements { get; set; }
    }
}

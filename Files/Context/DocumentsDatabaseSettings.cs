using Files.Models;
using Microsoft.EntityFrameworkCore;

namespace Files.Context
{
    public class DocumentsDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string DocumentsCollectionName { get; set; } = null!;

    }
}

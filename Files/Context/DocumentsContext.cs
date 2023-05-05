using Files.Models;
using Microsoft.EntityFrameworkCore;

namespace Files.Context
{
    public class DocumentsContext : DbContext
    {
        public DocumentsContext(DbContextOptions<DocumentsContext> options)
            : base(options) 
        {
            
        }

        public DbSet<Documents> Documents { get; set; }
            
    }
}

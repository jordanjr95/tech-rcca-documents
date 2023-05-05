using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;

namespace Files.Models
{
    public class Documents
    {
        [Key]
        public int documentID { get; set; }
        public int templateID { get; set; }
        public string documentReference { get; set; }
        public bool waitingAdminApproval { get; set; }
    }
}

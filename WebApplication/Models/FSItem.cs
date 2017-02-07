using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class FSItem
    {
        [Required]
        [DisplayName("Name")]
        public string FileName { get; set; }

        [Required]
        [DisplayName("Attribute")]
        public string Attribute { get; set; }

        public bool isDirectory { get; set; }
    }
}
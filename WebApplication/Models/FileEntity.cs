using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class FileEntity
    {
        public int FileId { get; set; }

        [Required]
        [DisplayName("Name")]
        public string FileName { get; set; }

        [Required]
        [DisplayName("Size")]       
        public string FileSize { get; set; }
    }
}
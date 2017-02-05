using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class File
    {
        public int CustomerId { get; set; }

        [Required]
        [DisplayName("Name")]
        public string CustomerName { get; set; }

        [Required]
        [DisplayName("Address")]       
        public string CustomerAddress { get; set; }
    }
}
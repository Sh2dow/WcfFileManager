using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public abstract class FSItem
    {
        [Required]
        [DisplayName("Name")]
        public string name { get; set; }
        [Required]
        [DisplayName("path")]
        public string path { get; set; }
        public bool isDirectory { get; set; }
    }

    public class FItem : FSItem
    {
        public FItem()
        {
            isDirectory = false;
        }
    }

    public class DItem : FSItem
    {
        public DItem()
        {
            isDirectory = true;
        }
    }

    public class PItem : FSItem
    {
        //public List<Int64> NestedItems { get; set; }
        public PItem()
        {
            isDirectory = false;
        }
    }
}
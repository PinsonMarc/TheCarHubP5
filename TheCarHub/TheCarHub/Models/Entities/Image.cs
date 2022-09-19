using System.ComponentModel.DataAnnotations.Schema;

namespace TheCarHub.Models
{
    public class Image
    {
        public int? Id { get; set; }
        [NotMapped]
        public bool Selected { get; set; }        
        public string FileName { get; set; }
        
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}

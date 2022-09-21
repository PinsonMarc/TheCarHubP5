using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheCarHub.Models
{
    public class Make
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<Model> Models { get; set; }
    }
}

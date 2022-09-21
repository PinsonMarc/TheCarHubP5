using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheCarHub.Models
{
    public class Model
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }


        public List<Car> Cars { get; set; }

        [Required]
        public int MakeId { get; set; }

        public Make Make { get; set; }
    }
}

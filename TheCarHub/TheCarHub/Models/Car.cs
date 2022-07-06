using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheCarHub.Models
{
    public class Car
    {
        public string Id { get; set; }  
        public string VIN { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public DateTime PurchaseDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PurchasePrice { get; set; }
        public string Repairs { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal RepairCost { get; set; }
        public DateTime LotDate { get; set; }
        public DateTime? SaleDate { get; set; }
    }
}

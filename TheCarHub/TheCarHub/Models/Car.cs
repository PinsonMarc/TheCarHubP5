using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheCarHub.Models
{
    public class Car
    {
        [BindNever]
        public string Id { get; set; }

        [StringLength(100)]
        public string VIN { get; set; }

        [Range(1990, 2050)]
        public int Year { get; set; }

        [StringLength(50)]
        public string Make { get; set; }

        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(10)]
        public string Trim { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal PurchasePrice { get; set; }
        public string Repairs { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal RepairCost { get; set; }

        [DataType(DataType.Date)]
        public DateTime LotDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SaleDate { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Available,
        NotAvailable,
        Sold
    }
}

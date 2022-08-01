using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheCarHub.Models
{
    public class Car
    {
        [BindNever]
        public int Id { get; set; }

        [StringLength(100)]
        public string VIN { get; set; }

        [Range(1990, 2050)]
        public int Year { get; set; }

        public Model Model { get; set; }
        
        public int? ModelId { get; set; }

        [StringLength(10)]
        public string Trim { get; set; }

        [DisplayName("Purchase Date")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [DisplayName("Purchase Price")]
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal PurchasePrice { get; set; }
        public string Repairs { get; set; }

        [DisplayName("Repair Cost")]
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal RepairCost { get; set; }

        [DisplayName("Lot Date")]
        [DataType(DataType.Date)]
        public DateTime LotDate { get; set; }

        [DisplayName("Sale Date")]
        [DataType(DataType.Date)]
        public DateTime? SaleDate { get; set; }

        public List<Image> Images { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Available,
        NotAvailable,
        Sold
    }
}

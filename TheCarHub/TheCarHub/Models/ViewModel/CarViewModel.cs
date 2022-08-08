using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace TheCarHub.Models
{
    public class CarViewModel
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

        [DisplayName("Purchase Price ($)")]
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal PurchasePrice { get; set; }
        public string Repairs { get; set; }

        [DisplayName("Repair Cost ($)")]
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

        [Display(Name = "Images")]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        [DataType(DataType.Upload)]
        public List<IFormFile> ImageFiles { get; set; }
    }

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This photo extension is not allowed!";
        }
    }
}

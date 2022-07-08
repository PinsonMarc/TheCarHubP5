using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheCarHub.Models
{
    public class Image
    {
        public int? Id { get; set; }

        public string FileName { get; set; }
        
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}

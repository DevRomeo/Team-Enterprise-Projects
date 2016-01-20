using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ML.Web.Models
{
    public class MobileModel
    {
        [Display(Name="Wallet Number")]
        [RegularExpression(@"/^([0-9])$/", ErrorMessage = "The Wallet No. field accept only numbers.")]
        [Required(ErrorMessage = "Wallet Number is required")]
        public string Wallet { get; set; }
        
        [Display(Name = "Customer ID")]
        [RegularExpression(@"/^([a-zA-Z0-9])$/", ErrorMessage = "MLKP Customer ID  field accept Alphanumeric only")]
        [Required(ErrorMessage = "MLKP Customer ID is required")]
        public string Kyc { get; set; }
        
        [Display(Name = "Device ID")]
        [RegularExpression(@"/^([a-zA-Z0-9])$/", ErrorMessage = "ML Wallet Device ID field accept Alphanumeric only")]
        [Required(ErrorMessage = "ML Wallet Device ID is required")]
        public string Device { get; set; }
    }
}
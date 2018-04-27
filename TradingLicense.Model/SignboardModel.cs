using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class SignboardModel
    {
        
        public int SignboardID { get; set; }

        [Required(ErrorMessage = "Please Enter Length")]
        [Display(Name= "Length")]
        public float Length { get; set; }

        [Required(ErrorMessage = "Please Enter Width")]
        [Display(Name = "Width")]
        public float Width { get; set; }

        [Required(ErrorMessage = "Please Enter WithLamp")]
        [Display(Name = "WithLamp")]
        public int WithLamp { get; set; }

        [Required(ErrorMessage = "Please Enter Quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please Enter Face Quantity")]
        [Display(Name = "Face Quantitys")]
        public int FaceQty { get; set; }

        [Required(ErrorMessage = "Please Enter Display Method")]
        [Display(Name = "Display Method")]
        [StringLength(15)]
        public string DisplayMethod { get; set; }

        [Required(ErrorMessage = "Please Enter Location")]
        [Display(Name = "Location")]
        [StringLength(10)]
        public string Location { get; set; }
    }
}

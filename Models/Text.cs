using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace mvc.Models
{
     public class Text
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string Image64
        {
            get
            {
                string a = null;
                if(Image != null){
                a =String.Format("data:image/png;base64,{0}", Convert.ToBase64String(Image));
                }
                else{a = null;}
                return a;
            }    
        }

        [NotMapped]
        public IFormFile IImage { get; set; }
    }
}
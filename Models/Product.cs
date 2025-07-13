using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MWBAYLY.Models
{
    public class Product
    {
       
            public int Id { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="Minium must be 3")]
        [MaxLength(50, ErrorMessage = "Minium must be 50")]
        public string Name { get; set; }
        [Required]
            public string Description { get; set; }
      //  [Range(0, 10)]
            public decimal Price { get; set; }
        [ValidateNever]
            public string ImgUrl { get; set; }
        [Range(0, 10)]
           public double Rate { get; set; }

            public int categoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        public int? CompanyId { get; set; }
        [ValidateNever]
        public Company Company { get; set; }

    }
}

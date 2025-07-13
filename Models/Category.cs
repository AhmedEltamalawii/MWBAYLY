using System.ComponentModel.DataAnnotations;

namespace MWBAYLY.Models
{
    public class Category
    {
        
            public int Id { get; set; }
        [Required]
        [MinLength(3,ErrorMessage = "THE MIN Length IS 3 Character")]
        [MaxLength(50,ErrorMessage ="THE MAX Length IS 50 Character")]
       
        public string Name { get; set; }

            public ICollection<Product> Products { get; } = new List<Product>();
        
    }
}

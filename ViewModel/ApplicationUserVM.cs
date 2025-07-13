using System.ComponentModel.DataAnnotations;

namespace MWBAYLY.ViewModel
{
    public class ApplicationUserVM
    {
        public int Id { get; set; }
        public string userName{ get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Address{ get; set; }
    }
}

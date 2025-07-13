using Microsoft.AspNetCore.Identity;

namespace MWBAYLY.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Address { get; set; }
    }
}

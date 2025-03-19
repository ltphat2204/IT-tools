using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Entities
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public required string Name { get; set; }
        [DefaultValue("https://res.cloudinary.com/ltphat2204/image/upload/q_auto:best/f_auto/default_profile")]
        public string Photo { get; set; } = "https://res.cloudinary.com/ltphat2204/image/upload/q_auto:best/f_auto/default_profile";
    }
}

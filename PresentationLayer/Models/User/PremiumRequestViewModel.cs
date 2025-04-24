using BusinessLayer.Entities;

namespace PresentationLayer.Models.User
{
    public class PremiumRequestViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public int TotalUsers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}

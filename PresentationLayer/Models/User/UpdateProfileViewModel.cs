namespace PresentationLayer.Models.User
{
    public class UpdateProfileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IFormFile? NewImage { get; set; } 
        public string? ImageUrl { get; set; }
        public string? PremiumRequest { get; set; }
        public bool IsPremium { get; set; }
    }
}

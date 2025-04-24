using BusinessLayer.Entities;

namespace PresentationLayer.Models.User
{
    public class UserListViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public int TotalUsers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchQuery { get; set; }
        public string RoleFilter { get; set; }
        public string[] Roles { get; set; }
    }
}

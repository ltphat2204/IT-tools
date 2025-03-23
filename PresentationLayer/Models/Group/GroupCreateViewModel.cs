using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.Group
{
    public class GroupCreateViewModel
    {
        [Required]
        [Display(Name = "Group name")]
        public required string GroupName { get; set; }
    }
}

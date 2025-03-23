using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.Group
{
    public class GroupEditViewModel
    {
        public int GroupId { get; set; }

        [Required]
        [Display(Name = "Group name")]
        public required string GroupName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.Tool
{
    public class ToolEditViewModel
    {
        public int ToolId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "UI is required.")]
        public string? UIStringHtml { get; set; }

        public string? Libraries { get; set; }

        public string? Process { get; set; }

        public bool IsPremium { get; set; } = false;

        [Required(ErrorMessage = "Group is required.")]
        public int GroupId { get; set; }

        public IEnumerable<BusinessLayer.Entities.Group>? Groups { get; set; }
    }
}

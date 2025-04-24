namespace PresentationLayer.Models.Tool
{
    public class ToolDetailViewModel
    {
        public BusinessLayer.Entities.Tool Tool { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsBlock { get; set; } = false;
    }
}

namespace PresentationLayer.Models.Tool
{
    public class ToolsViewModel
    {
        public List<BusinessLayer.Entities.Tool> Tools { get; set; }
        public List<BusinessLayer.Entities.Group> Groups { get; set; }
        public string CurrentSearchTerm { get; set; }
        public int CurrentGroupId { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Tab { get; set; }
    }
}

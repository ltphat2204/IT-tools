using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Entities
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [Required]
        public required string GroupName { get; set; }
        public ICollection<Tool>? Tools { get; set; }
    }
}

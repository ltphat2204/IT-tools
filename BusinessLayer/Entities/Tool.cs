using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Entities
{
    public class Tool
    {
        [Key]
        public int ToolId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required string UIStringHtml { get; set; }
        public string? Libraries { get; set; }
        public string? Process { get; set; }
        
        public bool IsPremium { get; set; } = false;

        [Required]
        public int GroupId { get; set; }
        public required Group Group { get; set; }
    }
}

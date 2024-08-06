using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [Range(1, 100,ErrorMessage ="Hey Boyy, the display order must be in between 1-100")]
        public int DisplayOrder { get; set; }
    }
}

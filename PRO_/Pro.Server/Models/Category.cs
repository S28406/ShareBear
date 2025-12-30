using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Category
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }

    public ICollection<Tool> Tools { get; set; }
}
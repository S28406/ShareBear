using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class ToolAccessory
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Quantity_Available { get; set; }

    [Required]
    public Guid Tool_ID { get; set; }
    public Tool Tool { get; set; }
}
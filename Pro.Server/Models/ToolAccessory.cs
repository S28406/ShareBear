using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class ToolAccessory
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int QuantityAvailable { get; set; }

    [Required]
    public Guid ToolId { get; set; }
    public Tool Tool { get; set; }
}
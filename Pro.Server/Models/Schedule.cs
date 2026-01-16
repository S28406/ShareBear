using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Schedule
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public DateTime AvailableFrom { get; set; }
    [Required]
    public DateTime AvailableTo { get; set; }

    [Required]
    public Guid ToolsId { get; set; }
    public Tool Tool { get; set; }
}
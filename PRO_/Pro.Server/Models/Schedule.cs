using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Schedule
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    public DateTime Available_from { get; set; }
    [Required]
    public DateTime Available_to { get; set; }

    [Required]
    public Guid Tools_ID { get; set; }
    public Tool Tool { get; set; }
}
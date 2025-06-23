using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Event
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public string Description { get; set; }

    [Required]
    public Guid Organizers_ID { get; set; }
    public User Organizer { get; set; }
}
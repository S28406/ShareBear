using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Event
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public string Description { get; set; }

    [Required]
    public Guid OrganizersId { get; set; }
    public User Organizer { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Notification
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Sent_At { get; set; }
    [Required]
    public int Read { get; set; }

    [Required]
    public Guid Users_ID { get; set; }
    public User User { get; set; }
}
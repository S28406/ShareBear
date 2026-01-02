using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Notification
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime SentAt { get; set; }
    [Required]
    public int Read { get; set; }

    [Required]
    public Guid UsersId { get; set; }
    public User User { get; set; }
}
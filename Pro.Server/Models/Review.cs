using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Review
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public int Rating { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }

    [Required]
    public Guid ToolId { get; set; }
    public Tool Tool { get; set; }

    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; }
}
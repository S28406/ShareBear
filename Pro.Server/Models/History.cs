using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class History
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime AddedAt { get; set; }

    [Required]
    public Guid ToolId { get; set; }
    public Tool Tool { get; set; }

    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; }
}
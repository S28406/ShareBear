using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class SecurityDeposit
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public float Ammount { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public DateTime RefundDate { get; set; }

    [Required]
    public Guid ToolsId { get; set; }
    public Tool Tool { get; set; }

    [Required]
    public Guid UsersId { get; set; }
    public User User { get; set; }
}
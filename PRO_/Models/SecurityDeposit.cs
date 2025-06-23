using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class SecurityDeposit
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    public float Ammount { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public DateTime Refund_Date { get; set; }

    [Required]
    public Guid Tools_ID { get; set; }
    public Tool Tool { get; set; }

    [Required]
    public Guid Users_ID { get; set; }
    public User User { get; set; }
}
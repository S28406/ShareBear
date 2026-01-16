using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRO.Models;

public class User
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Role { get; set; }
    [Required]
    public byte[] PasswordHash { get; set; }
    [Required]
    public byte[] PasswordSalt { get; set; }
    

    public ICollection<Tool> Tools { get; set; }
    public ICollection<Borrow> Borrows { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<SecurityDeposit> SecurityDeposits { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<History> Histories { get; set; }
}
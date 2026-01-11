using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Borrow
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Required]
    public string Status { get; set; } = "Pending";

    [Required]
    public float Price { get; set; }

    [Required]
    public Guid UsersId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Return> Returns { get; set; } = new List<Return>();
    public ICollection<ProductBorrow> ProductBorrows { get; set; } = new List<ProductBorrow>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
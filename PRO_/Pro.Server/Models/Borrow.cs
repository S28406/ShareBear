using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Borrow
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public float Price { get; set; }

    [Required]
    public Guid Users_ID { get; set; }
    public User User { get; set; }

    public ICollection<Return> Returns { get; set; }
    public ICollection<ProductBorrow> ProductBorrows { get; set; }
    
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
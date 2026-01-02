using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Return
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }
    [Required]
    public string Condition { get; set; }
    [Required]
    public string Damage { get; set; }

    [Required]
    public Guid BorrowsId { get; set; }
    public Borrow Borrow { get; set; }
}
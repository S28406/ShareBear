using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class ProductBorrow
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid ToolId { get; set; }
    public Tool Tool { get; set; } = null!;

    [Required]
    public Guid BorrowId { get; set; }
    public Borrow Borrow { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }
}
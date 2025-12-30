using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class ProductBorrow
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    public Guid Tools_ID { get; set; }
    public Tool Tool { get; set; }

    [Required]
    public Guid Orders_ID { get; set; }
    public Borrow Order { get; set; }
    [Required]
    public int Quantity { get; set; }
}
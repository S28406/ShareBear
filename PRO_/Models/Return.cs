using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Return
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }
    [Required]
    public string Condition { get; set; }
    [Required]
    public string Damage { get; set; }

    [Required]
    public Guid Borrows_ID { get; set; }
    public Borrow Borrow { get; set; }
}
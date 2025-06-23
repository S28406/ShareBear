using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Payment
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }
    [Required]
    public float Ammount { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public string Method { get; set; }

    [Required]
    public Guid Orders_ID { get; set; }
    public Borrow Borrow { get; set; }
}
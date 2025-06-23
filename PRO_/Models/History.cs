using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class History
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Added_at { get; set; }

    [Required]
    public Guid Tool_ID { get; set; }
    public Tool Tool { get; set; }

    [Required]
    public Guid Users_ID { get; set; }
    public User User { get; set; }
}
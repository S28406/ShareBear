using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class LendingPartner
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    public string Partnership_Type { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime Start_Date { get; set; }
    [Required]
    public string Partner_Contract { get; set; }

    [Required]
    public Guid Users_Id { get; set; }
    public User User { get; set; }
    
    [Required]
    public Guid Partners_Id { get; set; }
    public User Partner { get; set; }
}
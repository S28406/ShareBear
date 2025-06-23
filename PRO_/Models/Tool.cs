using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Tool
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public float Price { get; set; }
    [Required]
    public int Quantity { get; set; }

    [Required]
    public Guid Users_ID { get; set; }
    public User User { get; set; }

    [Required]
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<ToolAccessory> Accessories { get; set; }
    public ICollection<Schedule> Schedules { get; set; }
    public ICollection<History> Histories { get; set; }
    public ICollection<SecurityDeposit> SecurityDeposits { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
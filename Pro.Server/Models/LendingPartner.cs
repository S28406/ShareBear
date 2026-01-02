using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class LendingPartner
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string PartnershipType { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [Required]
    public string PartnerContract { get; set; }

    [Required]
    public Guid UsersId { get; set; }
    public User User { get; set; }
    
    [Required]
    public Guid PartnersId { get; set; }
    public User Partner { get; set; }
}
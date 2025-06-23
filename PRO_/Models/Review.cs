﻿using System.ComponentModel.DataAnnotations;

namespace PRO.Models;

public class Review
{
    [Key]
    [Required]
    public Guid ID { get; set; }
    [Required]
    public int Rating { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }

    [Required]
    public Guid Tools_ID { get; set; }
    public Tool Tool { get; set; }

    [Required]
    public Guid Users_ID { get; set; }
    public User User { get; set; }
}
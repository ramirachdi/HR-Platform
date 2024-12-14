using ProjetNET.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PasswordRecovery
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public Guid UserId { get; set; }

    public User User { get; set; } // Assuming you have a User model

    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string RecoveryCode { get; set; }

    [Required]
    public DateTime ExpirationDateTime { get; set; }
}

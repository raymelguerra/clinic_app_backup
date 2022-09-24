using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicApp.AuthorizationAndUserManager.Models;

public class RegisterViewModel
{
    [Required]
    [StringLength(50)]
    public string? Username { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string? Password { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string? ConfirmPassword { get; set; }
    
    [Required]
    public List<string>? Rol { get; set; }

    public string? Email { get; set; }
}

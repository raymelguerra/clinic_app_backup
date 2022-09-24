using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicApp.AuthorizationAndUserManager.Models;

public class ResetPasswordViewModel
{
    [Required]
    [StringLength(50)]
    public string? Username { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 5)]
    public string? NewPassword { get; set; }
}



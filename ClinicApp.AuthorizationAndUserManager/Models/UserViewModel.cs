using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicApp.AuthorizationAndUserManager.Models;

public class UserViewModel
{
    public string? id { get; set; }
    public string? username { get; set; }
    public List<string>? roles { get; set; }
    public string? email { get; set; }
}

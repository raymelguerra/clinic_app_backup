using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class ReleaseInformation
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

}

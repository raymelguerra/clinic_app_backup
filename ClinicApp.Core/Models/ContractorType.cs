using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class ContractorType
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

}

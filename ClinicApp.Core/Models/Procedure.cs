using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Core.Models;

public class Procedure
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public int ContractorTypeId { get; set; }

    [ForeignKey("ContractorTypeId")]
    public ContractorType? ContractorType { get; set; }

}

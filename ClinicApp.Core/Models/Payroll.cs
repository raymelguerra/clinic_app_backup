using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class Payroll
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ContractorId { get; set; }

    [Required]
    public int ContractorTypeId { get; set; }

    [Required]
    public int InsuranceProcedureId { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [ForeignKey("ContractorId")]
    public Contractor Contractor { get; set; } = null!;

    [ForeignKey("ContractorTypeId")]
    public ContractorType ContractorType { get; set; } = null!;

    [ForeignKey("InsuranceProcedureId")]
    public InsuranceProcedure InsuranceProcedure { get; set; } = null!;

    [ForeignKey("CompanyId")]
    public Company Company { get; set; } = null!;

}

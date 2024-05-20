using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class PlaceOfService
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Value { get; set; }

}

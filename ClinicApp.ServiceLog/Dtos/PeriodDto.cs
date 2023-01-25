using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class PeriodDto
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

}

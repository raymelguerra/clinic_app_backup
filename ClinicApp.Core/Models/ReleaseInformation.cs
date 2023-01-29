using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class ReleaseInformation
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Client> Clients { get; } = new List<Client>();
}

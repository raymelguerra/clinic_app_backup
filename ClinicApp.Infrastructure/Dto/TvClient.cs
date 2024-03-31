using System;

namespace ClinicApp.Infrastructure.Dto;

public class TvClient: TvObject
{
    public string Id { get; set; }
    public string Name { get; set; }

    public string Status
    {
        get
        {
            if (Contractors.All(x => x.Status == "billed")) return "billed";
            else if (Contractors.Any(x => x.Status == "billed")) return "billed_gray";
            return "empty";
        }
        set { }
    }

    public List<TvContractor> Contractors { get; set; }
    public TvClient()
    {
        Contractors = new List<TvContractor>();
    }
}

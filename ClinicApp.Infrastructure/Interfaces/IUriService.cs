using ClinicApp.Infrastructure.Data;

namespace ClinicApp.Infrastructure.Interfaces;
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }

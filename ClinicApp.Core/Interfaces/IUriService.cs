using ClinicApp.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicApp.Core.Interfaces;
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.Core.Models
{
    public enum LogEvent
    {
        // INFO LOGS
        CREATED = 1000,
        UPDATED = 1001,
        DELETED = 1002,
        GET_BY_ID = 1003,
        GET_ALL = 1004,

        // ERRORS LOGS
        NOT_FOUND = 0404,
        BAD_REQUEST = 0400,
        FORBIDDEN = 0403,
        UNAUTHORIZED = 0401,
        INTERNAL_SERVER_ERROR = 0500

    }
}

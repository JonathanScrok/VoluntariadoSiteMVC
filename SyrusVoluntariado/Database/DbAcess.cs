using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Database
{
    public class DbAcess
    {
        public static IConfiguration Configuration;

        public static string GetConnection()
        {
            var connString = DbAcess.Configuration["ConnectionString"];
            return connString;
        }

    }
}

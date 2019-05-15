using AlertReport.Web.Infrastructure;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertReport.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new PasswordHasher();


            var s1 = x.HashPassword("komp123");
            var s2 = x.HashPassword("komp123");

            var r1 = x.VerifyHashedPassword(s1, "komp123");
            var r2 = x.VerifyHashedPassword(s2, "komp123");
        }
    }
}

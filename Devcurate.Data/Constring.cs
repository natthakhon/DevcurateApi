using System;
using System.Collections.Generic;
using System.Text;

namespace Devcurate.Data
{
    public static class Constring
    {
        public static string ConnectionString()
        {
            string mdfLocation = @"C:\Users\Natthakhon.la\source\repos\Devcurate.Api\Devcurate.Data\Database1.mdf";

            return String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0};Integrated Security=True;Connect Timeout=30", mdfLocation);
        }
    }
}

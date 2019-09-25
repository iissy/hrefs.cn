using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ASY.Hrefs.Util.MiscHelpers
{
    public static class SqlHelpers
    {
        public static DbConnection CreateDbConnection(string connectionStrings)
        {
            DbConnection conn = new MySqlConnection(connectionStrings);
            return conn;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BaiTestDongTienQuang
{
    public class Connection
    {
        public static SqlConnection getConnetion()
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-CAV8Q06;Initial Catalog=BAITEST;Integrated Security=True");
            if (conn != null)
            {

                return conn;

            }
            return null;
        }
    }
}

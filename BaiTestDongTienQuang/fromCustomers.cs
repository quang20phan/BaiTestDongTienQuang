using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BaiTestDongTienQuang
{
    public partial class fromCustomers : Form
    {
        
        public fromCustomers()
        {   
            InitializeComponent();
            int pageSum = getAllCustomers().Rows.Count;
            lbPageNumber.Text = string.Format("Page {0}/{1}", pageNumber, pageSum/pageSize);
        }

        int pageNumber = 1;
        int pageSize = 10;
        private void fromCustomers_Load(object sender, EventArgs e)
        {
            LoadDataTable(pageNumber, pageSize);
        }
        private async void LoadDataTable(int pageNumber, int pageSize)
        {
            SqlConnection conn = Connection.getConnetion();

            string sql = @"DECLARE @PageNumber AS INT
                            DECLARE @PageSize AS INT
                            SET @PageNumber=" + pageNumber + "SET @PageSize=" + pageSize + "SELECT ID, Code, Name as [Tên], Address as [Địa chỉ] FROM [tbl__Customers]ORDER BY ID OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            DataTable dttb = new DataTable();
            if(sqlDataReader.HasRows)
            {
                dttb.Load(sqlDataReader);
            }
            dgv__Customers.DataSource = dttb;
            conn.Close();        
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            int pageSum = getAllCustomers().Rows.Count;
            if (pageNumber - 1  > 0)
            {
                pageNumber--;
                LoadDataTable(pageNumber, pageSize);
                lbPageNumber.Text = string.Format("Page {0}/{1}", pageNumber, pageSum / pageSize);
            }
        }

        private void btnNext_Click( object sender, EventArgs e)
        {
            int pageSum = getAllCustomers().Rows.Count;
            if (pageNumber < pageSum / pageSize)
            { 
                pageNumber++;
                LoadDataTable(pageNumber, pageSize);
                lbPageNumber.Text = string.Format("Page {0}/{1}", pageNumber, pageSum / pageSize);
            }
        }

        private DataTable getAllCustomers()
        {
            SqlConnection conn = Connection.getConnetion();

            string sql = "SELECT ID, Code, Name, Address FROM [tbl__Customers]";
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dttb = new DataTable();
            if (sqlDataReader.HasRows)
            {
                dttb.Load(sqlDataReader);
            }
            conn.Close();

            return dttb;
        }
    }
}

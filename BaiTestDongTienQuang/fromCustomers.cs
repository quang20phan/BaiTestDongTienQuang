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
using PagedList;

namespace BaiTestDongTienQuang
{
    public partial class fromCustomers : Form
    {
        int pageNumber = 1;
        IPagedList<tbl__Customers> lst;
        public fromCustomers()
        {   
            InitializeComponent();
            LoadDataTable();
        }    

        public async Task<PagedList<tbl__Customers>> getPage(int pageNumber = 1, int pageSize = 10)
        {
            return (PagedList<tbl__Customers>)await Task.Factory.StartNew(() => { 
                using(BAITESTEntities db = new BAITESTEntities())
                {
                    return db.tbl__Customers.OrderBy(p => p.ID).ToPagedList(pageNumber, pageSize);
                }
            });
            
        }

        private async void fromCustomers_Load(object sender, EventArgs e)
        {
            lst = await getPage();
           
            btnPrev.Enabled = lst.HasPreviousPage;
            btnNext.Enabled = lst.HasNextPage;
            dgv__Customers.DataSource = lst.ToList();
            lbPageNumber.Text = string.Format("Page {0}/{1}", pageNumber, lst.PageCount);
        }
        private void LoadDataTable()
        {
            SqlConnection conn = Connection.getConnetion();

            string sql = "SELECT ID, Code, Name, Address FROM [tbl__Customers]";
            SqlCommand sqlCommand = new SqlCommand(sql, conn);

            SqlDataAdapter adapt = new SqlDataAdapter(sqlCommand);
            DataSet dtset = new DataSet();
            adapt.Fill(dtset);
            dgv__Customers.DataSource = dtset.Tables[0];

        }

        private async void btnPrev_Click(object sender, EventArgs e)
        {
            if (lst.HasPreviousPage)
            {
                lst = await getPage(--pageNumber);
                btnPrev.Enabled = lst.HasPreviousPage;
                btnNext.Enabled = lst.HasNextPage;
                dgv__Customers.DataSource = lst.ToList();
                lbPageNumber.Text = string.Format("Page {0}/{1}", pageNumber, lst.PageCount);
            }
        }

        private async void btnNext_Click( object sender, EventArgs e)
        {
            if (lst.HasNextPage)
            {
                lst = await getPage(++pageNumber);
                btnPrev.Enabled = lst.HasPreviousPage;
                btnNext.Enabled = lst.HasNextPage;
                dgv__Customers.DataSource = lst.ToList();
                lbPageNumber.Text = string.Format("Page {0}/{1}", pageNumber, lst.PageCount);
            }
        }
    }
}

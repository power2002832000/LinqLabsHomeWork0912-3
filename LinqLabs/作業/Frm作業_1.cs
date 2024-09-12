using LinqLabs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_1 : Form
    {
        public Frm作業_1()
        {
            InitializeComponent();
        


        }
        private int currentPage = 1;              // 當前頁數
        private int pageSize = 10;                // 每頁顯示的筆數（由 textBox1 控制）


        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    this.lblDetails.Text = "Order details";

            //    NOTE
            //    var OrderID = ((NWDataSet.OrdersRow)this.bindingSource1.Current).OrderID;

            //    var q = from o in this.nwDataSet1.Order_Details
            //            where o.OrderID == OrderID
            //            select o;

            //    this.dataGridView2.DataSource = q.ToList();

            //}
            //catch
            //{

            //}
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //this.nwDataSet1.Products.Take(10);//Top 10 Skip(10)

            //Distinct()
            int totalRecords = this.nwDataSet1.Orders.Rows.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            
            if (currentPage < totalPages)
            {
                currentPage++;
                DisplayPage(currentPage);
            }


        }

        private void button14_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files =  dir.GetFiles();

            //files[0].CreationTime
            this.dataGridView1.DataSource = files;

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            this.order_DetailsTableAdapter1.Fill(this.nwDataSet1.Order_Details);
            
            SetPageSize();

            
            currentPage = 1; 
            DisplayPage(currentPage);
        }

        private void Frm作業_1_Load(object sender, EventArgs e)
        {

            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            var q1 = from o in this.nwDataSet1.Orders
                     select o.OrderDate.Year;
            this.comboBox1.DataSource = q1.Distinct().ToList();
            this.order_DetailsTableAdapter1.Fill(nwDataSet1.Order_Details);
            

        }


        private void SetPageSize()
        {
            if (int.TryParse(textBox1.Text, out int result) && result > 0)
            {
                pageSize = result;
            }
            else
            {
                // 若輸入無效，設為預設值 10
                pageSize = 10;
                textBox1.Text = pageSize.ToString(); // 更新 textBox1 顯示為 10
            }
        }
        private void DisplayPage(int pageNumber)
        {
            if (this.nwDataSet1.Orders.Rows.Count == 0)
            {
                dataGridView1.DataSource = null;
                MessageBox.Show("No records to display.");
                return;
            }
            int totalRecords = this.nwDataSet1.Orders.Rows.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var pagedData = this.nwDataSet1.Orders.AsEnumerable()
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize);

            if (pagedData.Any())
                dataGridView1.DataSource = pagedData.CopyToDataTable();
            else
                dataGridView1.DataSource = null;

            //MessageBox.Show($"Page {currentPage} of {totalPages}");


        }

        private void button12_Click(object sender, EventArgs e)
        {
            //上一頁
            if (currentPage > 1)
            {
                currentPage--;
                DisplayPage(currentPage);
            }
            else
            { MessageBox.Show("沒有資料了"); }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int year;// = 1997;
            int.TryParse(this.comboBox1.Text, out year);

            var q2 = from o in this.nwDataSet1.Orders
                     where o.OrderDate.Year == year
                     select o;

            //NOTE  BindingSource
            this.bindingSource2.DataSource = q2.ToList();
            this.dataGridView1.DataSource = this.bindingSource2;



        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.lblMaster.Text = "2017 Files";
            System.IO.DirectoryInfo dirs = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dirs.GetFiles();

            var q = from f in files
                    where f.CreationTime.Year == 2017
                    orderby f.CreationTime descending
                    select f;
            this.dataGridView1.DataSource = q.ToList();
        }
    }
        }

    


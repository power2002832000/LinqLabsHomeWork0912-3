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
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            this.order_DetailsTableAdapter1.Fill(this.nwDataSet1.Order_Details);



            var q1 = from o in this.nwDataSet1.Orders
                     select o.OrderDate.Year;

            //NOTE Distinct()
            this.comboBox1.DataSource = q1.Distinct().ToList();


        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblDetails.Text = "Order details";
                var OrderID = ((NWDataSet.OrdersRow)this.bindingSource1.Current).OrderID;

                var q = from o in this.nwDataSet1.Order_Details
                        where o.OrderID == OrderID
                        select o;

                this.dataGridView2.DataSource = q.ToList();

            }
            catch
            {

                
            }

        }
        private void button13_Click(object sender, EventArgs e)
        {
            //this.nwDataSet1.Products.Take(10);//Top 10 Skip(10)

            //Distinct()
            int.TryParse(this.textBox1.Text, out countPerPage);

            page += 1;
            this.dataGridView1.DataSource = this.nwDataSet1.Products.Skip(countPerPage * page).Take(countPerPage).ToList();


        }

        private void button14_Click(object sender, EventArgs e)
        {
            //System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            //System.IO.FileInfo[] files =  dir.GetFiles();

            ////files[0].CreationTime
            //this.dataGridView1.DataSource = files;


            this.lblMaster.Text = "LOG Files";
            System.IO.DirectoryInfo dirs = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dirs.GetFiles();

            var q = from f in files
                    where f.Extension.ToUpper() == ".LOG"
                    select f;
            this.dataGridView1.DataSource = q.ToList();


        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        int page = -1;
        int countPerPage = 10;


        private void button6_Click(object sender, EventArgs e)
        {
            this.lblMaster.Text = "Orders";

            //var q1 = from o in this.northwindDataSet1.Orders
            //             select o.OrderDate.Year;

            ////NOTE Distinct()
            //this.comboBox1.DataSource = q1.Distinct().ToList();

            this.dataGridView1.DataSource = this.nwDataSet1.Orders.ToList();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            int year;// = 1997;
            int.TryParse(this.comboBox1.Text, out year);

            var q2 = from o in this.nwDataSet1.Orders
                     where o.OrderDate.Year == year
                     select o;

            //NOTE  BindingSource
            this.bindingSource1.DataSource = q2.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.lblMaster.Text = "Large Files";
            System.IO.DirectoryInfo dirs = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dirs.GetFiles();

            var q = from f in files
                    where f.Length > 2000
                    select f;
            this.dataGridView1.DataSource = q.ToList();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblMaster.Text = "Orders";

            int year = 1997;
            int.TryParse(this.comboBox1.Text, out year);

            var q = from o in this.nwDataSet1.Orders
                    where o.OrderDate.Year == year
                    select o;

            //NOTE - BindingSource
            this.bindingSource1.DataSource = q.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;

        }

        private void button12_Click(object sender, EventArgs e)
        {
            int.TryParse(this.textBox1.Text, out countPerPage);

            page -= 1;
            this.dataGridView1.DataSource = this.nwDataSet1.Products.Skip(countPerPage * page).Take(countPerPage).ToList();

        }
    }
}

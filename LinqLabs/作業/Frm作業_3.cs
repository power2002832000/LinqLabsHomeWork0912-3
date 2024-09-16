using LinqLabs.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinqLabs.作業
{
    public partial class Frm作業_3 : Form
    {
        private List<Student> students_scores;
        public Frm作業_3()
        {
            InitializeComponent();
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            this.order_DetailsTableAdapter1.Fill(this.nwDataSet1.Order_Details);
            students_scores = new List<Student>()
            {
                new Student{ Name = "aaa", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Male" },
                new Student{ Name = "bbb", Class = "CS_102", Chi = 80, Eng = 80, Math = 100, Gender = "Male" },
                new Student{ Name = "ccc", Class = "CS_101", Chi = 60, Eng = 50, Math = 75, Gender = "Female" },
                new Student{ Name = "ddd", Class = "CS_102", Chi = 80, Eng = 70, Math = 85, Gender = "Female" },
                new Student{ Name = "eee", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Female" },
                new Student{ Name = "fff", Class = "CS_102", Chi = 80, Eng = 80, Math = 80, Gender = "Female" },
            };

        }

        private void button36_Click(object sender, EventArgs e)
        {
            #region 搜尋 班級學生成績

            // 
            // 共幾個 學員成績 ?						

            // 找出 前面三個 的學員所有科目成績					
            // 找出 後面兩個 的學員所有科目成績					

            // 找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績						

            // 找出學員 'bbb' 的成績	                          

            // 找出除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)	

            // 找出 'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績  |				
            // 數學不及格 ... 是誰 
            #endregion
            listBox1.Items.Clear();

            // 顯示所有學生的成績
            var query = students_scores.Select(s => s);

            foreach (var student in query)
            {
                listBox1.Items.Add($"姓名: {student.Name}, 班級: {student.Class}, 國文: {student.Chi}, 英文: {student.Eng}, 數學: {student.Math}, 性別: {student.Gender}");
            }


        }

        private void button37_Click(object sender, EventArgs e)
        {
            //個人 sum, min, max, avg
            // 統計 每個學生個人成績 並排序
            listBox1.Items.Clear();

            // 計算各科的最大值、最小值、平均值
            var chiStats = new
            {
                Max = students_scores.Max(s => s.Chi),
                Min = students_scores.Min(s => s.Chi),
                Avg = students_scores.Average(s => s.Chi)
            };

            var engStats = new
            {
                Max = students_scores.Max(s => s.Eng),
                Min = students_scores.Min(s => s.Eng),
                Avg = students_scores.Average(s => s.Eng)
            };

            var mathStats = new
            {
                Max = students_scores.Max(s => s.Math),
                Min = students_scores.Min(s => s.Math),
                Avg = students_scores.Average(s => s.Math)
            };

            // 顯示統計結果
            listBox1.Items.Add("各科統計結果:");
            listBox1.Items.Add($"國文 - 最高分: {chiStats.Max}, 最低分: {chiStats.Min}, 平均分: {chiStats.Avg:F2}");
            listBox1.Items.Add($"英文 - 最高分: {engStats.Max}, 最低分: {engStats.Min}, 平均分: {engStats.Avg:F2}");
            listBox1.Items.Add($"數學 - 最高分: {mathStats.Max}, 最低分: {mathStats.Min}, 平均分: {mathStats.Avg:F2}");
        
    }

        private void button33_Click(object sender, EventArgs e)
        {
            // split=> 分成 三群 '待加強'(60~69) '佳'(70~89) '優良'(90~100) 
            // print 每一群是哪幾個 ? (每一群 sort by 分數 descending)

            listBox1.Items.Clear();

            // 定義分群的分數範圍
            const int minScore = 60;
            const int midScore = 70;
            const int highScore = 90;

            // 根據每科分數來決定群組
            var grouped = students_scores.GroupBy(s =>
            {
                if (s.Chi >= highScore && s.Eng >= highScore && s.Math >= highScore)
                    return "優良";
                else if (s.Chi >= midScore && s.Eng >= midScore && s.Math >= midScore)
                    return "佳";
                else
                    return "待加強";
            });

   
            foreach (var group in grouped)
            {
                listBox1.Items.Add($"群組: {group.Key}");

                
                var sortedGroup = group.OrderByDescending(s => s.Chi + s.Eng + s.Math);

                foreach (var student in sortedGroup)
                {
                    int total = student.Chi + student.Eng + student.Math;
                    listBox1.Items.Add($"  姓名: {student.Name}, 總分: {total} (國文: {student.Chi}, 英文: {student.Eng}, 數學: {student.Math})");
                }
            }
        

    }

        private void button10_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            try
            {
                // 確保 Orders 表已經填充數據
                if (this.nwDataSet1.Orders.Rows.Count == 0)
                {
                    this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
                }

                // 使用 LINQ to Objects 從 DataTable 中獲取 Orders 列表
                var orders = this.nwDataSet1.Orders.AsEnumerable();

                // 使用 LINQ 按年份和月份分組 Orders
                var ordersByYearMonth = from order in orders
                                        where order.Field<DateTime?>("OrderDate") != null
                                        group order by new
                                        {
                                            Year = order.Field<DateTime>("OrderDate").Year,
                                            Month = order.Field<DateTime>("OrderDate").Month
                                        } into yearMonthGroup
                                        orderby yearMonthGroup.Key.Year descending, yearMonthGroup.Key.Month descending
                                        select new
                                        {
                                            Year = yearMonthGroup.Key.Year,
                                            Month = yearMonthGroup.Key.Month,
                                            OrderCount = yearMonthGroup.Count()
                                        };

                // 顯示分組結果
                listBox1.Items.Add("按年份和月份分組的訂單資料：");
                listBox1.Items.Add("");

                foreach (var group in ordersByYearMonth)
                {
                    string monthName = new DateTime(group.Year, group.Month, 1).ToString("MMMM"); // 獲取月份名稱
                    listBox1.Items.Add($"年份: {group.Year}, 月份: {group.Month} ({monthName})");
                    listBox1.Items.Add($"訂單數量: {group.OrderCount}");
                    listBox1.Items.Add("---------------------------");
                }
            }
            catch (Exception ex)
            {
                // 處理異常情況
                MessageBox.Show($"發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
    }

        private void Frm作業_3_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (this.nwDataSet1.Products.Rows.Count == 0)
            {
                this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            }
            var products = this.nwDataSet1.Products.AsEnumerable();
            var lowPrice = products
    .Where(p => p.Field<decimal?>("UnitPrice") < 10)
    .OrderBy(p => p.Field<decimal?>("UnitPrice"))
    .Select(p => new
    {
        ProductID = p.Field<int>("ProductID"),
        ProductName = p.Field<string>("ProductName"),
        UnitPrice = p.Field<decimal?>("UnitPrice") ?? 0
    })
    .ToList();

            var mediumPrice = products
                .Where(p => p.Field<decimal?>("UnitPrice") >= 10 && p.Field<decimal?>("UnitPrice") <= 40)
                .OrderBy(p => p.Field<decimal?>("UnitPrice"))
                .Select(p => new
                {
                    ProductID = p.Field<int>("ProductID"),
                    ProductName = p.Field<string>("ProductName"),
                    UnitPrice = p.Field<decimal?>("UnitPrice") ?? 0
                })
                .ToList();

            var highPrice = products
                .Where(p => p.Field<decimal?>("UnitPrice") > 40)
                .OrderBy(p => p.Field<decimal?>("UnitPrice"))
                .Select(p => new
                {
                    ProductID = p.Field<int>("ProductID"),
                    ProductName = p.Field<string>("ProductName"),
                    UnitPrice = p.Field<decimal?>("UnitPrice") ?? 0
                })
                .ToList();

            listBox1.Items.Add("低價產品（UnitPrice < 10）：");
            if (lowPrice.Any())
            {
                foreach (var product in lowPrice)
                {
                    listBox1.Items.Add($"ID: {product.ProductID}, 名稱: {product.ProductName}, 價格: {product.UnitPrice:C}");
                }
            }
            else
            {
                listBox1.Items.Add("無低價產品。");
            }
            listBox1.Items.Add(""); 

            listBox1.Items.Add("中價產品（10 ≤ UnitPrice ≤ 40）：");
            if (mediumPrice.Any())
            {
                foreach (var product in mediumPrice)
                {
                    listBox1.Items.Add($"ID: {product.ProductID}, 名稱: {product.ProductName}, 價格: {product.UnitPrice:C}");
                }
            }
            else
            {
                listBox1.Items.Add("無中價產品。");
            }
            listBox1.Items.Add(""); 

            listBox1.Items.Add("高價產品（UnitPrice > 40）：");
            if (highPrice.Any())
            {
                foreach (var product in highPrice)
                {
                    listBox1.Items.Add($"ID: {product.ProductID}, 名稱: {product.ProductName}, 價格: {product.UnitPrice:C}");
                }
            }
            else
            {
                listBox1.Items.Add("無高價產品。");
            }

        }

        private void button15_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            try
            {
                // 確保 Orders 表已經填充數據
                if (this.nwDataSet1.Orders.Rows.Count == 0)
                {
                    this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
                }

                // 使用 LINQ to Objects 從 DataTable 中獲取 Orders 列表
                var orders = this.nwDataSet1.Orders.AsEnumerable();

                // 使用 LINQ 按年份分組 Orders
                var ordersByYear = from order in orders
                                   where order.Field<DateTime?>("OrderDate") != null
                                   group order by order.Field<DateTime>("OrderDate").Year into yearGroup
                                   orderby yearGroup.Key descending 
                                   select new
                                   {
                                       Year = yearGroup.Key,
                                       OrderCount = yearGroup.Count(),
                                       TotalSales = yearGroup.Sum(o => o.Field<decimal?>("Freight") ?? 0) 
                                   };

           
                listBox1.Items.Add("按年份分組的訂單資料：");
                listBox1.Items.Add("");

                foreach (var group in ordersByYear)
                {
                    listBox1.Items.Add($"年份: {group.Year}");
                    listBox1.Items.Add($"訂單數量: {group.OrderCount}");
                    listBox1.Items.Add("---------------------------");
                }
            }
            catch (Exception ex)
            {
                // 處理異常情況
                MessageBox.Show($"發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            try
            {
                // 確保 Order_Details 表已經填充數據
                if (this.nwDataSet1.Order_Details.Rows.Count == 0)
                {
                    this.order_DetailsTableAdapter1.Fill(this.nwDataSet1.Order_Details);
                }

                // 使用 LINQ to Objects 從 DataTable 中獲取 Order_Details 列表
                var orderDetails = this.nwDataSet1.Order_Details.AsEnumerable();

                // 計算總銷售額：Σ (Quantity × UnitPrice)
                var totalSales = orderDetails.Sum(od => od.Field<short>("Quantity") * od.Field<decimal>("UnitPrice"));

                // 顯示總銷售額在 Label
                listBox1.Items.Add($"總銷售額: {totalSales:C}");
            }
            catch (Exception ex)
            {
                // 處理異常情況
                MessageBox.Show($"發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        


    }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}

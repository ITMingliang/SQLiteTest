using SQLliteTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLiteTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // 创建数据库文件
            SQLiteHelper.CreateDBFile("mydb.db");
            // 消息提示
            MessageBox.Show("数据库创建成功.");
        }
        /// <summary>
        /// 创建报表：mytab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            // 创建表：mytab，包含一个ID和一个text字段，ID为键值
            SQLiteHelper.CreateTable("mydb.db", "create table mytab (ID INT PRIMARY KEY NOT NULL, text TEXT)");
            // 消息提示
            MessageBox.Show("报表创建成功.");
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SQLiteHelper.DeleteDBFile("mydb.db");
            MessageBox.Show("数据库删除成功.");
        }
        /// <summary>
        /// 删除报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            SQLiteHelper.DeleteTable("mydb.db", "mytab");
            MessageBox.Show("报表删除成功.");
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            string strSql = $"select * from mytab where ID={textBox1.Text}";
#if true

            List<string> ds = SQLiteHelper.SqlRow("mydb.db", strSql);
            int addr = 0;
            dataGridView2.Rows.Clear();
            int index = dataGridView2.Rows.Add();
            foreach (string d in ds)
            {
                dataGridView2.Rows[index].Cells[addr++].Value = d;
            }
#endif
#if false
            DataTable ds = SQLiteHelper.SqlTable("mydb.db", strSql);
            dataGridView1.DataSource = ds;
#endif
        }
        /// <summary>
        /// 查询整张表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            string strSql = "select * from mytab";
            DataTable ds = SQLiteHelper.SqlTable("mydb.db", strSql);
            dataGridView1.DataSource = ds;
        }
        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            int id = 0;
            if (dataGridView1.Rows.Count > 0 || !string.IsNullOrEmpty(dataGridView1.Rows[0].Cells[0].Value.ToString()))
            {
                id = int.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[0].Value.ToString()) + 1;
            }
            // INSERT INTO TABLE_NAME VALUES (value1,value2,value3,...valueN);
            string sql = $"insert into mytab (ID, text) values({id}, '{textBox2.Text}')";
            SQLiteHelper.ExecuteNonQuery("mydb.db", sql);
            button6_Click(null, null);
        }
      

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            string sql = $"DELETE FROM mytab where ID='{textBox1.Text}'";
            SQLiteHelper.ExecuteNonQuery("mydb.db", sql);
            button6_Click(null, null);
        }

        /// <summary>
        /// 修改一条记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            string sql = $"update mytab set text='{textBox2.Text}' where ID='{textBox1.Text}'";
            SQLiteHelper.ExecuteNonQuery("mydb.db", sql);
            button6_Click(null, null);
        }

        /// <summary>
        /// 清空表显示纪录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != null)
                dataGridView1.DataSource = null;
            dataGridView2.Rows.Clear();
        }

        /// <summary>
        /// 登录测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            //登录检测
            try
            {
                string dbPath = "Data Source=" + Directory.GetCurrentDirectory() + @"\data\orders.db;Version=3;Password=07762828216;";
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    //查询是否有相应的数据
                    string uname = "";
                    string upwd = "";
                    string sql_select = "select count(0) from users where username =@uname and password =@upwd ";
                    //连接数据库
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    SQLiteCommand cmd = new SQLiteCommand(sql_select, conn);
                    //cmd.Parameters.Add(new SQLiteParameter("@uname", uname));
                    //cmd.Parameters.Add(new SQLiteParameter("@upwd", upwd));
                    SQLiteParameter[] paras = new SQLiteParameter[] { new SQLiteParameter("@uname", uname), new SQLiteParameter("@upwd", upwd) };
                    cmd.Parameters.AddRange(paras);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int result = Convert.ToInt16(reader.GetValue(0));
                        if (result == 0)
                        {
                            MessageBox.Show("用户名或者密码错误，请重新输入！");
                        }
                        else if (result == 1)
                        {
                            MessageBox.Show("登录成功！");
                        }
                        else
                        {
                            MessageBox.Show("程序出错，请不要输入奇怪的的内容！");
                            //退出软件
                            System.Environment.Exit(0);
                        }
                    }
                    //关闭数据库连接
                    reader.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接数据库失败：" + ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //base.Close();
            System.Environment.Exit(0);
        }
    }
}

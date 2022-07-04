using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_Asynchronous_Lesson
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Task と context を使うとPromise.thenのような非同期処理の書き方が出来る
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Run(() => GetData()).ContinueWith(x=>
            {
                // UIスレッド上で行う処理
                dataGridView1.DataSource = x.Result; //x.Result はGetData()の返り値
                MessageBox.Show("完了");

            }, context);
        }

        private List<DTO> GetData()
        {
            var result = new List<DTO>();
            for (int i = 0; i < 5; i++)
            {
                System.Threading.Thread.Sleep(1000);
                result.Add(new DTO(i.ToString(), "Name" + i));
            }
            return result;
        }
    }
}

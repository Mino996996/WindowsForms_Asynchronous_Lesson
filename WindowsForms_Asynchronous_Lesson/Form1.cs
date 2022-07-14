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
        static List<DTO> m_result = new List<DTO>();
        static int m_DoWorkerFlag = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //// Task と context を使うとPromise.thenのような非同期処理の書き方が出来る
            //var context = TaskScheduler.FromCurrentSynchronizationContext();
            //Task.Run(() => GetData()).ContinueWith(x=>
            //{
            //    // UIスレッド上で行う処理
            //    dataGridView1.DataSource = x.Result; //x.Result はGetData()の返り値
            //    MessageBox.Show("完了");

            //}, context);
            if (backgroundWorker1.IsBusy) {
                MessageBox.Show("起動中です");
            }
            else {
                m_DoWorkerFlag = 1;
                backgroundWorker1.RunWorkerAsync();
            }

        }

        private List<DTO> GetData(out bool flag)
        {
            flag = false;
            var result = new List<DTO>();
            for (int i = 0; i < 5; i++)
            {
                System.Threading.Thread.Sleep(1000);
                if (backgroundWorker1.CancellationPending)
                {
                    return result;
                }
                result.Add(new DTO(i.ToString(), "Name" + i));
            }
            flag = true;
            return result;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (m_DoWorkerFlag == 1)
            {
                m_result = GetData(out bool flag);
                if (flag)
                {
                    backgroundWorker1.ReportProgress(0, "");
                }
                else
                {
                    backgroundWorker1.ReportProgress(1, "");
                }
            }
            else
            {
                backgroundWorker1.ReportProgress(-1, "");
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                dataGridView1.DataSource = m_result;
                MessageBox.Show("完了");
            }
            else if (e.ProgressPercentage == 1)
            {
                MessageBox.Show("キャンセル");
            }
            else
            {
                MessageBox.Show("完了");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }
    }
}

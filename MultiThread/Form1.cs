using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiThread
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            List<string> numList = new List<string>();
            for (int i = 1; i < 33; i++)
            {
                numList.Add(i.ToString()); ;
            }

            redNumArray = numList.ToArray();

            numList.Clear();
            for (int j = 1; j < 17; j++)
            {
                numList.Add(j.ToString()); ;
            }

            blueNumArray = numList.ToArray();
        }

        /*需求：
         * 写一个双色球的生成器，双色球有6和红球，1个绿球组成
         * 6个红球有1-32数字产生，不能重复
         * 1个绿球有1-16数字产生
         */
        private string[] blueNumArray = new string[] { };
        private string[] redNumArray = new string[] { };

        private bool isGoOn = true;

        private void btnStart_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                int index2 = GetRandNum(1, 16);
                lbRed2.Text = redNumArray[index2];

            }

            return;

            foreach (var item in gb1.Controls)
            {
                if (item is Label)
                {
                    Label label = (Label)item;
                    if (label.Name.Contains("Red11"))
                    {
                        //Task.Run(() =>
                        //{
                        while (isGoOn)
                        {
                            int index = GetRandNum(1, 32);
                            string num = redNumArray[index];
                            //label.Text = num;  
                            /*重要知识1： label控件是在主线程里面初始化的，所以只能主线程来操作，否则会报错;
                             * 要用委托，把操作动作委托给主线程
                             */
                            this.Invoke(new Action(() =>
                            {
                                label.Text = num;
                            }));
                        }
                        //});
                    }
                    else if (label.Name.Contains("Blue"))
                    {
                        Task.Run(() =>
                        {
                            while (isGoOn)
                            {
                                int index = GetRandNum(1, 16);
                                string num = redNumArray[index];
                                // label.Text = num;
                                this.Invoke(new Action(() =>
                                {
                                    label.Text = num;
                                }));
                            }
                        });
                    }
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isGoOn = false;

            MessageBox.Show($"号码：{lbRed1.Text}-{lbRed2.Text}-{lbRed3.Text}-{lbRed4.Text}-{lbRed5.Text}-{lbRed6.Text}   {lbBlue.Text}");
        }

        int seed = 0;
        private int GetRandNum(int min, int max)
        {
             Thread.Sleep(1000);
            //TimeSpan ts = DateTime.Now - new DateTime(1900, 1, 1); 

            Random random = new Random(seed++);

            return random.Next(min - 1, max);
        }


    }
}

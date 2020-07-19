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

                numList.Add(i < 10 ? $"0{i}" : i.ToString());
            }

            redNumArray = numList.ToArray();

            numList.Clear();
            for (int j = 1; j < 17; j++)
            {
                numList.Add(j < 10 ? $"0{j}" : j.ToString());
            }

            blueNumArray = numList.ToArray();

            btnStop.Enabled = false;
        }

        /*需求：
         * 写一个双色球的生成器，双色球有6和红球，1个绿球组成
         * 6个红球有1-32数字产生，不能重复
         * 1个绿球有1-16数字产生
         */
        private string[] blueNumArray = new string[] { };
        private string[] redNumArray = new string[] { };

        private bool isGoOn = true;
        private readonly static object obj_lock = new object();

        public List<Task> taskList = new List<Task>();
        private void btnStart_Click(object sender, EventArgs e)
        {
            isGoOn = true;
            btnStart.Enabled = false;

            foreach (var item in gb1.Controls)
            {
                if (item is Label)
                {
                    Label label = (Label)item;
                    if (label.Name.Contains("Red"))
                    {
                        taskList.Add(Task.Run(() =>
                        {
                            while (isGoOn)
                            {
                                int index = GetRandNum(1, 32);
                                string num = redNumArray[index];
                                //label.Text = num;  
                                /*重要知识1： label控件是在主线程里面初始化的，所以只能主线程来操作，子线程不能操作它，否则会报错;
                                 * 要在主线程里面用委托，把操作动作委托给主线程
                                 */

                                lock (obj_lock)
                                {
                                    //isChongfu(num);
                                    if (!isChongfu(num))
                                    {
                                        this.Invoke(new Action(() =>
                                        {
                                            label.Text = num;
                                        }));
                                    }
                                }
                            }
                        }));
                    }
                    else if (label.Name.Contains("Blue"))
                    {
                       taskList.Add(Task.Run(() =>
                       {
                           while (isGoOn)
                           {
                               int index = GetRandNum(1, 16);
                               string num = redNumArray[index];

                               lock (obj_lock)
                               {
                                   // label.Text = num;
                                   this.Invoke(new Action(() =>
                                      {
                                          label.Text = num;
                                      }));
                               }
                           }
                       }));
                    }
                }
            }

            btnStop.Enabled = true;
        }

        public bool isChongfu(string num)
        {
            List<string> numList = new List<string>();
            foreach (var item in gb1.Controls)
            {
                if (item is Label)
                {
                    Label label = (Label)item;
                    if (label.Name.Contains("Red") && label.Text != "00")
                    {
                        numList.Add(label.Text);
                    }
                }
            }
            bool isChongFu = numList.Contains(num);

            //if (isChongFu)
            //{
            //    MessageBox.Show("重复");
            //}
            return isChongFu;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isGoOn = false;
            // Task.WaitAll(taskList.ToArray());
            //MessageBox.Show($"号码：{lbRed1.Text}-{lbRed2.Text}-{lbRed3.Text}-{lbRed4.Text}-{lbRed5.Text}-{lbRed6.Text}   {lbBlue.Text}");

            /*
                重要知识点：Task.WaitAll 在主线程（假设称为线程1）里面等待所有子线程结束，但是子线程（假设线程2）里面有委托主线程显示num.
                就会造成主线程等待子线程结束，而子线程等待主线程显示num,造成互相等待死锁；
                分析：线程1等待线程2结束；线程2登录线程1显示num；造成死锁，解决方案是把Task.WaitAll(taskList.ToArray())放到另一个
                子线程3里面，这样就会线程3等待线程2结束，线程2等待线程1显示num，就不会造成死锁；
                最终代码如下
             */

            Task.Run(() =>
            {
                Task.WaitAll(taskList.ToArray());
                MessageBox.Show($"您选择的号码：{lbRed1.Text}-{lbRed2.Text}-{lbRed3.Text}-{lbRed4.Text}-{lbRed5.Text}-{lbRed6.Text}   {lbBlue.Text}");

                //委托主线修改主线程下的按钮状态，因为按钮是主线程初始化的，在子线程里面修改会报错
                this.Invoke(new Action(() =>
                {
                    btnStop.Enabled = false;
                    btnStart.Enabled = true;
                }));

            });


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

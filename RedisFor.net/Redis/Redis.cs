using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Redis
{
    public partial class Redis : Form
    {
        public Redis()
        {
            InitializeComponent();
        }

        RedisClient client = new RedisClient("127.0.0.1", 6379);

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
            {
                MessageBox.Show("请输入键值对!");
                return;
            }
            
            client.Set<string>(textBox1.Text, textBox2.Text);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGet_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            List<string> list=client.GetAllKeys();
            foreach (string item in list)
            {
               string val= client.Get<string>(item);
                listBox1.Items.Add(string.Format("键:{0} 值:{1}",item,val));
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.Text.Split(' ')[0].Split(':')[1];
            textBox2.Text = listBox1.Text.Split(' ')[1].Split(':')[1];
        }

        /// <summary>
        /// 正常执行事物
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommit_Click(object sender, EventArgs e)
        {
            using (IRedisTransaction IRT = client.CreateTransaction())
            {
                //设置一个key=key  value=20的数据
                IRT.QueueCommand(r => r.Set("key", 20));
                //然后让该数据++1
                IRT.QueueCommand(r => r.Increment("key", 1));

                IRT.Commit(); // 提交事务
                //最终该变量的值是21
            }
            btnGet_Click(null, null);
        }

        private void btnRoleBack_Click(object sender, EventArgs e)
        {
            //先检测key=key的变量
            client.Watch("key");
            //然后再事物之前修改该变量，
            client.Set("key", "123");
            using (IRedisTransaction IRT = client.CreateTransaction())
            {
                //设置一个key=key  value=20的数据
                IRT.QueueCommand(r => r.Set("key", 20));
                //然后让该数据++1
                IRT.QueueCommand(r => r.Increment("key", 1));

                IRT.Commit(); // 提交事务
                //事物在操作该变量的时候，发现key这个变量已经发生改变了，所以就自动回滚了,值还是之前设置的123
            }
            btnGet_Click(null, null);
        }

        private void btnErrorCommit_Click(object sender, EventArgs e)
        {
            try
            {
                using (IRedisTransaction IRT = client.CreateTransaction())
                {
                    //设置一个key=key  value=aaa的数据
                    IRT.QueueCommand(r => r.Set("key", "aaa"));
                    //然后让该数据++1,但是字符串++1是会报错的，所以这一句是没有执行成功的，但是第一句执行成功了
                    IRT.QueueCommand(r => r.Increment("key", 1));

                    IRT.Commit(); // 提交事务
                    //最终该变量的值还是aaa
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnGet_Click(null, null);
            }
        }

        
    }
}

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

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
            {
                MessageBox.Show("请输入键值对!");
                return;
            }
            RedisClient client = new RedisClient("127.0.0.1", 6379);
            client.Set<string>(textBox1.Text, textBox2.Text);
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            RedisClient client = new RedisClient("127.0.0.1", 6379);
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

        
    }
}

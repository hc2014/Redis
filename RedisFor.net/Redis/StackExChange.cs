using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redis
{
    public partial class StackExChange : Form
    {
        //https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/Basics.md
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379,server2:6379");
        public StackExChange()
        {
            InitializeComponent();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            IDatabase db = redis.GetDatabase();
            db.StringSet(txtKey.Text, txtVal.Text);
        }

        private void StackExChange_Load(object sender, EventArgs e)
        {
            //订阅消息
            ISubscriber sub = redis.GetSubscriber();
            sub.Subscribe("messages", (channel, message) =>
            {
                MessageBox.Show("\r\n订阅到message的信息是:" + (string)message);
            });
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            ISubscriber sub = redis.GetSubscriber();
            sub.Publish("messages", "hello");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AntdUI;
using System.Threading;

namespace Demo.Controls
{
    public partial class Spin : UserControl
    {
        Form form;
        public Spin(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            AntdUI.Spin.open(this, config =>
            {

                // 配置 config 的参数
                config.Text = "数据正在加载中";
                Thread.Sleep(1000);
            }, () =>
            {



            });
        }

        private void Spin_Load(object sender, EventArgs e)
        {
            AntdUI.Spin.open(this, config =>
            {

                // 配置 config 的参数
                config.Text = "页面正在加载中";
                Thread.Sleep(1000);
            }, () =>
            {



            });
        }
    }
}

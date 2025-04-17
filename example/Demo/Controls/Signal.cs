using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Signal : UserControl
    {
        Form form;
        public Signal(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            signal1.Value += 1;
            signal2.Value += 1;
            signal3.Value += 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            signal1.Value -= 1;
            signal2.Value -= 1;
            signal3.Value -= 1;
        }

       
    }
}

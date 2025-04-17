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
    public partial class Battery : UserControl
    {
        Form form;

        public Battery(Form _form)
        {
            form = _form;
            InitializeComponent();
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            battery1.Value += 5;
            battery4.Value += 5;
            battery5.Value += 5;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            battery1.Value -= 5;
            battery4.Value -= 5;
            battery5.Value -= 5;
        }
    }
}

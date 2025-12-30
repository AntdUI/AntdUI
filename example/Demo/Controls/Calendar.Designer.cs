using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Calendar
    {
        /// <summary> 
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            header1 = new AntdUI.PageHeader();
            calendar1 = new AntdUI.Calendar();
            flowPanel1 = new AntdUI.FlowPanel();
            switch_showchinese = new AntdUI.Switch();
            label4 = new AntdUI.Label();
            switch_showtoday = new AntdUI.Switch();
            label6 = new AntdUI.Label();
            divider1 = new AntdUI.Divider();
            flowPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "按照日历形式展示数据的容器。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Calendar.Description";
            header1.LocalizationText = "Calendar.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(744, 74);
            header1.TabIndex = 0;
            header1.Text = "Calendar 日历";
            header1.UseTitleFont = true;
            // 
            // calendar1
            // 
            calendar1.Dock = DockStyle.Fill;
            calendar1.Full = true;
            calendar1.Location = new Point(0, 134);
            calendar1.Name = "calendar1";
            calendar1.ShowChinese = true;
            calendar1.Size = new Size(744, 416);
            calendar1.TabIndex = 2;
            // 
            // flowPanel1
            // 
            flowPanel1.Controls.Add(switch_showchinese);
            flowPanel1.Controls.Add(label4);
            flowPanel1.Controls.Add(switch_showtoday);
            flowPanel1.Controls.Add(label6);
            flowPanel1.Dock = DockStyle.Top;
            flowPanel1.Location = new Point(0, 102);
            flowPanel1.Name = "flowPanel1";
            flowPanel1.Size = new Size(744, 32);
            flowPanel1.TabIndex = 1;
            // 
            // switch_showchinese
            // 
            switch_showchinese.Checked = true;
            switch_showchinese.Location = new Point(381, 3);
            switch_showchinese.Name = "switch_showchinese";
            switch_showchinese.Size = new Size(50, 26);
            switch_showchinese.TabIndex = 13;
            // 
            // label4
            // 
            label4.Location = new Point(220, 3);
            label4.Name = "label4";
            label4.Size = new Size(155, 26);
            label4.TabIndex = 12;
            label4.Text = "ShowChinese";
            // 
            // switch_showtoday
            // 
            switch_showtoday.Checked = true;
            switch_showtoday.Location = new Point(164, 3);
            switch_showtoday.Name = "switch_showtoday";
            switch_showtoday.Size = new Size(50, 26);
            switch_showtoday.TabIndex = 11;
            // 
            // label6
            // 
            label6.Location = new Point(3, 3);
            label6.Name = "label6";
            label6.Size = new Size(155, 26);
            label6.TabIndex = 10;
            label6.Text = "ShowButtonToday";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Calendar.{id}";
            divider1.Location = new Point(0, 74);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(744, 28);
            divider1.TabIndex = 3;
            divider1.Text = "基本用法";
            // 
            // Calendar
            // 
            Controls.Add(calendar1);
            Controls.Add(flowPanel1);
            Controls.Add(divider1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Calendar";
            Size = new Size(744, 550);
            flowPanel1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Calendar calendar1;
        private AntdUI.FlowPanel flowPanel1;
        private AntdUI.Switch switch_showtoday;
        private AntdUI.Label label6;
        private AntdUI.Switch switch_showchinese;
        private AntdUI.Label label4;
        private AntdUI.Divider divider1;
    }
}
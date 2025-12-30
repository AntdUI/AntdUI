using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Badge
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
            tBadge6 = new AntdUI.Badge();
            tBadge7 = new AntdUI.Badge();
            tBadge8 = new AntdUI.Badge();
            tBadge9 = new AntdUI.Badge();
            tBadge10 = new AntdUI.Badge();
            divider1 = new AntdUI.Divider();
            label2 = new Label();
            badge1 = new AntdUI.Badge();
            badge2 = new AntdUI.Badge();
            badge3 = new AntdUI.Badge();
            badge4 = new AntdUI.Badge();
            badge5 = new AntdUI.Badge();
            badge6 = new AntdUI.Badge();
            flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel2 = new FlowLayoutPanel();
            divider2 = new AntdUI.Divider();
            flowLayoutPanel3 = new FlowLayoutPanel();
            button1 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            button3 = new AntdUI.Button();
            button4 = new AntdUI.Button();
            button5 = new AntdUI.Button();
            button6 = new AntdUI.Button();
            tag1 = new AntdUI.Tag();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            button4.SuspendLayout();
            button6.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "图标右上角的圆形徽标数字。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Badge.Description";
            header1.LocalizationText = "Badge.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(616, 74);
            header1.TabIndex = 0;
            header1.Text = "Badge 徽标数";
            header1.UseTitleFont = true;
            // 
            // tBadge6
            // 
            tBadge6.Location = new Point(141, 3);
            tBadge6.Name = "tBadge6";
            tBadge6.Size = new Size(40, 40);
            tBadge6.State = AntdUI.TState.Warn;
            tBadge6.TabIndex = 3;
            // 
            // tBadge7
            // 
            tBadge7.Location = new Point(95, 3);
            tBadge7.Name = "tBadge7";
            tBadge7.Size = new Size(40, 40);
            tBadge7.State = AntdUI.TState.Processing;
            tBadge7.TabIndex = 2;
            // 
            // tBadge8
            // 
            tBadge8.Location = new Point(3, 3);
            tBadge8.Name = "tBadge8";
            tBadge8.Size = new Size(40, 40);
            tBadge8.TabIndex = 0;
            // 
            // tBadge9
            // 
            tBadge9.Location = new Point(187, 3);
            tBadge9.Name = "tBadge9";
            tBadge9.Size = new Size(40, 40);
            tBadge9.State = AntdUI.TState.Error;
            tBadge9.TabIndex = 4;
            // 
            // tBadge10
            // 
            tBadge10.Location = new Point(49, 3);
            tBadge10.Name = "tBadge10";
            tBadge10.Size = new Size(40, 40);
            tBadge10.State = AntdUI.TState.Success;
            tBadge10.TabIndex = 1;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Badge.{id}";
            divider1.Location = new Point(0, 74);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(616, 28);
            divider1.TabIndex = 0;
            divider1.Text = "基本";
            // 
            // label2
            // 
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(6, 6);
            label2.Name = "label2";
            label2.Size = new Size(202, 40);
            label2.TabIndex = 1;
            label2.Text = "Light";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // badge1
            // 
            badge1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            badge1.Location = new Point(3, 3);
            badge1.Name = "badge1";
            badge1.Size = new Size(87, 23);
            badge1.TabIndex = 0;
            badge1.Text = "Default";
            // 
            // badge2
            // 
            badge2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            badge2.Location = new Point(96, 3);
            badge2.Name = "badge2";
            badge2.Size = new Size(91, 23);
            badge2.State = AntdUI.TState.Success;
            badge2.TabIndex = 1;
            badge2.Text = "Success";
            // 
            // badge3
            // 
            badge3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            badge3.Location = new Point(193, 3);
            badge3.Name = "badge3";
            badge3.Size = new Size(90, 23);
            badge3.State = AntdUI.TState.Primary;
            badge3.TabIndex = 2;
            badge3.Text = "Primary";
            // 
            // badge4
            // 
            badge4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            badge4.Location = new Point(289, 3);
            badge4.Name = "badge4";
            badge4.Size = new Size(117, 23);
            badge4.State = AntdUI.TState.Processing;
            badge4.TabIndex = 3;
            badge4.Text = "Processing";
            // 
            // badge5
            // 
            badge5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            badge5.Location = new Point(412, 3);
            badge5.Name = "badge5";
            badge5.Size = new Size(98, 23);
            badge5.State = AntdUI.TState.Warn;
            badge5.TabIndex = 4;
            badge5.Text = "Warning";
            // 
            // badge6
            // 
            badge6.AutoSizeMode = AntdUI.TAutoSize.Auto;
            badge6.Location = new Point(516, 3);
            badge6.Name = "badge6";
            badge6.Size = new Size(66, 23);
            badge6.State = AntdUI.TState.Error;
            badge6.TabIndex = 5;
            badge6.Text = "Error";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(badge1);
            flowLayoutPanel1.Controls.Add(badge2);
            flowLayoutPanel1.Controls.Add(badge3);
            flowLayoutPanel1.Controls.Add(badge4);
            flowLayoutPanel1.Controls.Add(badge5);
            flowLayoutPanel1.Controls.Add(badge6);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 102);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(616, 44);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(tBadge8);
            flowLayoutPanel2.Controls.Add(tBadge10);
            flowLayoutPanel2.Controls.Add(tBadge7);
            flowLayoutPanel2.Controls.Add(tBadge6);
            flowLayoutPanel2.Controls.Add(tBadge9);
            flowLayoutPanel2.Dock = DockStyle.Top;
            flowLayoutPanel2.Location = new Point(0, 146);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(616, 46);
            flowLayoutPanel2.TabIndex = 2;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Badge.{id}";
            divider2.Location = new Point(0, 192);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(616, 28);
            divider2.TabIndex = 0;
            divider2.Text = "更多";
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Controls.Add(button1);
            flowLayoutPanel3.Controls.Add(button2);
            flowLayoutPanel3.Controls.Add(button3);
            flowLayoutPanel3.Controls.Add(button4);
            flowLayoutPanel3.Controls.Add(button6);
            flowLayoutPanel3.Dock = DockStyle.Top;
            flowLayoutPanel3.Location = new Point(0, 220);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(616, 94);
            flowLayoutPanel3.TabIndex = 3;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button1.Badge = "9";
            button1.BorderWidth = 1F;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(81, 46);
            button1.TabIndex = 0;
            button1.Text = "Button";
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button2.Badge = " ";
            button2.BorderWidth = 1F;
            button2.Location = new Point(90, 3);
            button2.Name = "button2";
            button2.Size = new Size(58, 46);
            button2.TabIndex = 1;
            button2.Text = "Dot";
            // 
            // button3
            // 
            button3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button3.Badge = " ";
            button3.BadgeAlign = AntdUI.TAlign.TL;
            button3.BadgeBack = Color.Green;
            button3.BadgeSize = 0.8F;
            button3.BadgeSvg = "CheckSquareFilled";
            button3.BorderWidth = 1F;
            button3.Location = new Point(154, 3);
            button3.Name = "button3";
            button3.Size = new Size(61, 46);
            button3.TabIndex = 2;
            button3.Text = "SVG";
            // 
            // button4
            // 
            button4.BorderWidth = 1F;
            button4.Controls.Add(button5);
            button4.Location = new Point(221, 3);
            button4.Name = "button4";
            button4.Padding = new Padding(10);
            button4.Size = new Size(107, 69);
            button4.TabIndex = 3;
            button4.Text = "Button";
            // 
            // button5
            // 
            button5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button5.IconSvg = "DeleteFilled";
            button5.Location = new Point(70, 0);
            button5.Name = "button5";
            button5.Shape = AntdUI.TShape.Circle;
            button5.Size = new Size(37, 37);
            button5.TabIndex = 1;
            button5.Type = AntdUI.TTypeMini.Error;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.BorderWidth = 1F;
            button6.Controls.Add(tag1);
            button6.Location = new Point(334, 3);
            button6.Name = "button6";
            button6.Padding = new Padding(10);
            button6.Size = new Size(130, 69);
            button6.TabIndex = 4;
            button6.Text = "Button";
            // 
            // tag1
            // 
            tag1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tag1.Font = new Font("Microsoft YaHei UI", 8F);
            tag1.LocalizationText = "Badge.{id}";
            tag1.Location = new Point(71, 3);
            tag1.Name = "tag1";
            tag1.Size = new Size(56, 25);
            tag1.TabIndex = 1;
            tag1.Text = "猥琐发育";
            tag1.Type = AntdUI.TTypeMini.Primary;
            // 
            // Badge
            // 
            Controls.Add(flowLayoutPanel3);
            Controls.Add(divider2);
            Controls.Add(flowLayoutPanel2);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(divider1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Badge";
            Size = new Size(616, 585);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            button4.ResumeLayout(false);
            button6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Badge tBadge6;
        private AntdUI.Badge tBadge7;
        private AntdUI.Badge tBadge8;
        private AntdUI.Badge tBadge9;
        private AntdUI.Badge tBadge10;
        private AntdUI.Divider divider1;
        private Label label2;
        private AntdUI.Badge badge1;
        private AntdUI.Badge badge2;
        private AntdUI.Badge badge3;
        private AntdUI.Badge badge4;
        private AntdUI.Badge badge5;
        private AntdUI.Badge badge6;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel2;
        private AntdUI.Divider divider2;
        private FlowLayoutPanel flowLayoutPanel3;
        private AntdUI.Button button1;
        private AntdUI.Button button2;
        private AntdUI.Button button3;
        private AntdUI.Button button4;
        private AntdUI.Button button5;
        private AntdUI.Button button6;
        private AntdUI.Tag tag1;
    }
}
using System.Drawing;

namespace Demo.Controls
{
    partial class HyperlinkLabel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            hyperlinkLabel5 = new AntdUI.HyperlinkLabel();
            divider5 = new AntdUI.Divider();
            hyperlinkLabel4 = new AntdUI.HyperlinkLabel();
            divider4 = new AntdUI.Divider();
            hyperlinkLabel3 = new AntdUI.HyperlinkLabel();
            divider3 = new AntdUI.Divider();
            hyperlinkLabel2 = new AntdUI.HyperlinkLabel();
            divider2 = new AntdUI.Divider();
            hyperlinkLabel1 = new AntdUI.HyperlinkLabel();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "超链接文本 <a>";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "HyperlinkLabel.Description";
            header1.LocalizationText = "HyperlinkLabel.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new Size(592, 74);
            header1.TabIndex = 1;
            header1.Text = "HyperlinkLabel 超链接文本";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(hyperlinkLabel5);
            panel1.Controls.Add(divider5);
            panel1.Controls.Add(hyperlinkLabel4);
            panel1.Controls.Add(divider4);
            panel1.Controls.Add(hyperlinkLabel3);
            panel1.Controls.Add(divider3);
            panel1.Controls.Add(hyperlinkLabel2);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(hyperlinkLabel1);
            panel1.Controls.Add(divider1);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(592, 402);
            panel1.TabIndex = 2;
            // 
            // hyperlinkLabel5
            // 
            hyperlinkLabel5.Dock = System.Windows.Forms.DockStyle.Top;
            hyperlinkLabel5.Location = new Point(0, 260);
            hyperlinkLabel5.Name = "hyperlinkLabel5";
            hyperlinkLabel5.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            hyperlinkLabel5.Size = new Size(592, 30);
            hyperlinkLabel5.TabIndex = 5;
            hyperlinkLabel5.Text = "多个链接: <a href=one>链接1</a> 和 <a href=two>链接2</a>";
            hyperlinkLabel5.LinkClicked += hyperlinkLabel_LinkClicked;
            // 
            // divider5
            // 
            divider5.Dock = System.Windows.Forms.DockStyle.Top;
            divider5.Font = new Font("Microsoft YaHei UI", 10F);
            divider5.LocalizationText = "HyperlinkLabel.{id}";
            divider5.Location = new Point(0, 232);
            divider5.Name = "divider5";
            divider5.Orientation = AntdUI.TOrientation.Left;
            divider5.Size = new Size(592, 28);
            divider5.TabIndex = 0;
            divider5.TabStop = false;
            divider5.Text = "多个链接";
            // 
            // hyperlinkLabel4
            // 
            hyperlinkLabel4.Dock = System.Windows.Forms.DockStyle.Top;
            hyperlinkLabel4.Location = new Point(0, 202);
            hyperlinkLabel4.Name = "hyperlinkLabel4";
            hyperlinkLabel4.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            hyperlinkLabel4.Size = new Size(592, 30);
            hyperlinkLabel4.TabIndex = 4;
            hyperlinkLabel4.Text = "自定义样式 <a href=none>链接</a>";
            hyperlinkLabel4.LinkClicked += hyperlinkLabel_LinkClicked;
            // 
            // divider4
            // 
            divider4.Dock = System.Windows.Forms.DockStyle.Top;
            divider4.Font = new Font("Microsoft YaHei UI", 10F);
            divider4.LocalizationText = "HyperlinkLabel.{id}";
            divider4.Location = new Point(0, 174);
            divider4.Name = "divider4";
            divider4.Orientation = AntdUI.TOrientation.Left;
            divider4.Size = new Size(592, 28);
            divider4.TabIndex = 0;
            divider4.TabStop = false;
            divider4.Text = "自定义样式";
            // 
            // hyperlinkLabel3
            // 
            hyperlinkLabel3.Badge = "New";
            hyperlinkLabel3.BadgeAlign = AntdUI.TAlign.RT;
            hyperlinkLabel3.Dock = System.Windows.Forms.DockStyle.Top;
            hyperlinkLabel3.Location = new Point(0, 144);
            hyperlinkLabel3.Name = "hyperlinkLabel3";
            hyperlinkLabel3.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            hyperlinkLabel3.Size = new Size(592, 30);
            hyperlinkLabel3.TabIndex = 3;
            hyperlinkLabel3.Text = "带徽章的 <a href=none>超链接</a>";
            hyperlinkLabel3.LinkClicked += hyperlinkLabel_LinkClicked;
            // 
            // divider3
            // 
            divider3.Dock = System.Windows.Forms.DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "HyperlinkLabel.{id}";
            divider3.Location = new Point(0, 116);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(592, 28);
            divider3.TabIndex = 0;
            divider3.TabStop = false;
            divider3.Text = "带徽章的";
            // 
            // hyperlinkLabel2
            // 
            hyperlinkLabel2.Dock = System.Windows.Forms.DockStyle.Top;
            hyperlinkLabel2.Location = new Point(0, 86);
            hyperlinkLabel2.Name = "hyperlinkLabel2";
            hyperlinkLabel2.Size = new Size(592, 30);
            hyperlinkLabel2.TabIndex = 2;
            hyperlinkLabel2.Text = "居中对齐的 <a href=none>超链接</a>";
            hyperlinkLabel2.TextAlign = ContentAlignment.MiddleCenter;
            hyperlinkLabel2.LinkClicked += hyperlinkLabel_LinkClicked;
            // 
            // divider2
            // 
            divider2.Dock = System.Windows.Forms.DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "HyperlinkLabel.{id}";
            divider2.Location = new Point(0, 58);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(592, 28);
            divider2.TabIndex = 0;
            divider2.TabStop = false;
            divider2.Text = "居中对齐";
            // 
            // hyperlinkLabel1
            // 
            hyperlinkLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            hyperlinkLabel1.Location = new Point(0, 28);
            hyperlinkLabel1.Name = "hyperlinkLabel1";
            hyperlinkLabel1.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            hyperlinkLabel1.Size = new Size(592, 30);
            hyperlinkLabel1.TabIndex = 1;
            hyperlinkLabel1.Text = "这是一个 <a href=none>超链接</a> 测试";
            hyperlinkLabel1.LinkClicked += hyperlinkLabel_LinkClicked;
            // 
            // divider1
            // 
            divider1.Dock = System.Windows.Forms.DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "HyperlinkLabel.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(592, 28);
            divider1.TabIndex = 0;
            divider1.TabStop = false;
            divider1.Text = "常规";
            // 
            // HyperlinkLabel
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Name = "HyperlinkLabel";
            Size = new Size(592, 476);
            panel1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.HyperlinkLabel hyperlinkLabel1;
        private AntdUI.HyperlinkLabel hyperlinkLabel2;
        private AntdUI.HyperlinkLabel hyperlinkLabel3;
        private AntdUI.HyperlinkLabel hyperlinkLabel4;
        private AntdUI.HyperlinkLabel hyperlinkLabel5;
        private AntdUI.Divider divider1;
        private AntdUI.Divider divider2;
        private AntdUI.Divider divider5;
        private AntdUI.Divider divider4;
        private AntdUI.Divider divider3;
    }
}
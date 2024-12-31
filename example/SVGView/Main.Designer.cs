// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

namespace SVGView
{
    partial class Main
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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            windowBar1 = new AntdUI.PageHeader();
            button2 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            input1 = new AntdUI.Input();
            pictureBox1 = new PictureBox();
            windowBar1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // windowBar1
            // 
            windowBar1.Controls.Add(button2);
            windowBar1.Controls.Add(button1);
            windowBar1.Dock = DockStyle.Top;
            windowBar1.LocalizationText = "Title";
            windowBar1.Location = new Point(0, 0);
            windowBar1.Name = "windowBar1";
            windowBar1.ShowButton = true;
            windowBar1.ShowIcon = true;
            windowBar1.Size = new Size(800, 36);
            windowBar1.TabIndex = 0;
            windowBar1.Text = "SVG视图";
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Width;
            button2.Dock = DockStyle.Right;
            button2.IconSvg = resources.GetString("button2.IconSvg");
            button2.LocalizationText = "Auto";
            button2.Location = new Point(521, 0);
            button2.Name = "button2";
            button2.Size = new Size(78, 36);
            button2.TabIndex = 1;
            button2.Text = "一键";
            button2.Type = AntdUI.TTypeMini.Success;
            button2.MouseClick += button2_Click;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Width;
            button1.Dock = DockStyle.Right;
            button1.LocalizationText = "Zip";
            button1.Location = new Point(599, 0);
            button1.Name = "button1";
            button1.Size = new Size(57, 36);
            button1.TabIndex = 0;
            button1.Text = "剔除";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.MouseClick += button1_Click;
            // 
            // input1
            // 
            input1.Dock = DockStyle.Left;
            input1.Location = new Point(0, 36);
            input1.Multiline = true;
            input1.Name = "input1";
            input1.Size = new Size(390, 414);
            input1.TabIndex = 1;
            input1.TextChanged += input1_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(390, 36);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(410, 414);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // Main
            // 
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBox1);
            Controls.Add(input1);
            Controls.Add(windowBar1);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SVG视图";
            windowBar1.ResumeLayout(false);
            windowBar1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader windowBar1;
        private AntdUI.Input input1;
        private AntdUI.Button button1;
        private PictureBox pictureBox1;
        private AntdUI.Button button2;
    }
}

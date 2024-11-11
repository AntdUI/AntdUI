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

namespace Demo
{
    partial class Setting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setting));
            tablePanel = new System.Windows.Forms.TableLayoutPanel();
            label1 = new AntdUI.Label();
            label2 = new AntdUI.Switch();
            label3 = new AntdUI.Label();
            label4 = new AntdUI.Label();
            label5 = new AntdUI.Label();
            switch1 = new AntdUI.Switch();
            switch2 = new AntdUI.Switch();
            switch3 = new AntdUI.Switch();
            tablePanel.SuspendLayout();
            SuspendLayout();
            // 
            // tablePanel
            // 
            resources.ApplyResources(tablePanel, "tablePanel");
            tablePanel.Controls.Add(label1, 0, 0);
            tablePanel.Controls.Add(label2, 1, 0);
            tablePanel.Controls.Add(label3, 0, 1);
            tablePanel.Controls.Add(label4, 0, 2);
            tablePanel.Controls.Add(label5, 0, 3);
            tablePanel.Controls.Add(switch1, 1, 1);
            tablePanel.Controls.Add(switch2, 1, 2);
            tablePanel.Controls.Add(switch3, 1, 3);
            tablePanel.Name = "tablePanel";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.AutoCheck = true;
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // switch1
            // 
            resources.ApplyResources(switch1, "switch1");
            switch1.AutoCheck = true;
            switch1.Name = "switch1";
            // 
            // switch2
            // 
            resources.ApplyResources(switch2, "switch2");
            switch2.AutoCheck = true;
            switch2.Name = "switch2";
            // 
            // switch3
            // 
            resources.ApplyResources(switch3, "switch3");
            switch3.AutoCheck = true;
            switch3.Name = "switch3";
            // 
            // Setting
            // 
            resources.ApplyResources(this, "$this");
            Controls.Add(tablePanel);
            Name = "Setting";
            tablePanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tablePanel;
        private AntdUI.Label label1;
        private AntdUI.Switch label2;
        private AntdUI.Label label3;
        private AntdUI.Label label4;
        private AntdUI.Label label5;
        private AntdUI.Switch switch1;
        private AntdUI.Switch switch2;
        private AntdUI.Switch switch3;
    }
}
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

using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class DatePicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatePicker));
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            datePickerRange3 = new AntdUI.DatePickerRange();
            datePickerRange4 = new AntdUI.DatePickerRange();
            datePicker4 = new AntdUI.DatePicker();
            datePickerRange2 = new AntdUI.DatePicker();
            divider3 = new AntdUI.Divider();
            panel3 = new System.Windows.Forms.Panel();
            datePickerRange1 = new AntdUI.DatePickerRange();
            inputRange1 = new AntdUI.DatePickerRange();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            datePicker3 = new AntdUI.DatePicker();
            datePicker2 = new AntdUI.DatePicker();
            datePicker1 = new AntdUI.DatePicker();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            resources.ApplyResources(header1, "header1");
            header1.Name = "header1";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(divider3);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Name = "panel1";
            // 
            // panel4
            // 
            panel4.Controls.Add(datePickerRange3);
            panel4.Controls.Add(datePickerRange4);
            panel4.Controls.Add(datePicker4);
            panel4.Controls.Add(datePickerRange2);
            resources.ApplyResources(panel4, "panel4");
            panel4.Name = "panel4";
            // 
            // datePickerRange3
            // 
            resources.ApplyResources(datePickerRange3, "datePickerRange3");
            datePickerRange3.Name = "datePickerRange3";
            datePickerRange3.PresetsClickChanged += datePickerRange4_PresetsClickChanged;
            // 
            // datePickerRange4
            // 
            datePickerRange4.AllowClear = true;
            resources.ApplyResources(datePickerRange4, "datePickerRange4");
            datePickerRange4.Name = "datePickerRange4";
            datePickerRange4.Presets.AddRange(new object[] { "今天", "昨天", "过去7天", "过去39天", "本周", "上周", "本月", "上月", "本季", "上季", "本年", "去年" });
            datePickerRange4.PresetsClickChanged += datePickerRange4_PresetsClickChanged;
            // 
            // datePicker4
            // 
            datePicker4.Format = "yyyy-MM-dd HH:mm:ss";
            resources.ApplyResources(datePicker4, "datePicker4");
            datePicker4.Name = "datePicker4";
            datePicker4.Presets.AddRange(new object[] { "今天", "昨天", "过去7天", "过去39天", "本周", "上周", "本月", "上月", "本季", "上季", "本年", "去年", "去年1", "去年2" });
            datePicker4.PresetsClickChanged += datePickerRange4_PresetsClickChanged;
            // 
            // datePickerRange2
            // 
            datePickerRange2.Format = "yyyy-MM-dd HH:mm:ss";
            resources.ApplyResources(datePickerRange2, "datePickerRange2");
            datePickerRange2.Name = "datePickerRange2";
            datePickerRange2.PresetsClickChanged += datePickerRange4_PresetsClickChanged;
            // 
            // divider3
            // 
            resources.ApplyResources(divider3, "divider3");
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            // 
            // panel3
            // 
            panel3.Controls.Add(datePickerRange1);
            panel3.Controls.Add(inputRange1);
            resources.ApplyResources(panel3, "panel3");
            panel3.Name = "panel3";
            // 
            // datePickerRange1
            // 
            resources.ApplyResources(datePickerRange1, "datePickerRange1");
            datePickerRange1.Name = "datePickerRange1";
            // 
            // inputRange1
            // 
            resources.ApplyResources(inputRange1, "inputRange1");
            inputRange1.Name = "inputRange1";
            // 
            // divider2
            // 
            resources.ApplyResources(divider2, "divider2");
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            // 
            // panel2
            // 
            panel2.Controls.Add(datePicker3);
            panel2.Controls.Add(datePicker2);
            panel2.Controls.Add(datePicker1);
            resources.ApplyResources(panel2, "panel2");
            panel2.Name = "panel2";
            // 
            // datePicker3
            // 
            datePicker3.AllowClear = true;
            datePicker3.DropDownArrow = true;
            resources.ApplyResources(datePicker3, "datePicker3");
            datePicker3.Name = "datePicker3";
            datePicker3.Value = new System.DateTime(2013, 11, 11, 0, 0, 0, 0);
            // 
            // datePicker2
            // 
            datePicker2.AllowClear = true;
            resources.ApplyResources(datePicker2, "datePicker2");
            datePicker2.Name = "datePicker2";
            datePicker2.Placement = AntdUI.TAlignFrom.BR;
            // 
            // datePicker1
            // 
            resources.ApplyResources(datePicker1, "datePicker1");
            datePicker1.Name = "datePicker1";
            // 
            // divider1
            // 
            resources.ApplyResources(divider1, "divider1");
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            // 
            // DatePicker
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            resources.ApplyResources(this, "$this");
            Name = "DatePicker";
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.DatePicker datePicker2;
        private AntdUI.DatePicker datePicker1;
        private AntdUI.DatePicker datePicker3;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Divider divider2;
        private AntdUI.DatePickerRange datePickerRange1;
        private AntdUI.DatePickerRange inputRange1;
        private System.Windows.Forms.Panel panel4;
        private AntdUI.Divider divider3;
        private AntdUI.DatePicker datePicker4;
        private AntdUI.DatePicker datePickerRange2;
        private AntdUI.DatePickerRange datePickerRange3;
        private AntdUI.DatePickerRange datePickerRange4;
    }
}
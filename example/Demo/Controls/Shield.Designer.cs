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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

namespace Demo.Controls
{
    partial class Shield
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
            shield1 = new AntdUI.Shield();
            shield2 = new AntdUI.Shield();
            shield3 = new AntdUI.Shield();
            shield4 = new AntdUI.Shield();
            SuspendLayout();
            // 
            // shield1
            // 
            shield1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            shield1.Location = new System.Drawing.Point(436, 18);
            shield1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            shield1.Name = "shield1";
            shield1.Padding = new System.Windows.Forms.Padding(5);
            shield1.Radius = 0;
            shield1.Size = new System.Drawing.Size(160, 55);
            shield1.StatusColor = System.Drawing.Color.FromArgb(93, 176, 186);
            shield1.StatusText = "Apache 2.0";
            shield1.SubjectColor = System.Drawing.Color.FromArgb(84, 84, 84);
            shield1.SubjectText = "Licenses";
            shield1.TabIndex = 0;
            shield1.Text = "shield1";
            // 
            // shield2
            // 
            shield2.Location = new System.Drawing.Point(13, 26);
            shield2.Name = "shield2";
            shield2.Padding = new System.Windows.Forms.Padding(5);
            shield2.Size = new System.Drawing.Size(119, 38);
            shield2.StatusColor = System.Drawing.Color.FromArgb(17, 130, 195);
            shield2.StatusText = "V2.0.6";
            shield2.SubjectColor = System.Drawing.Color.FromArgb(84, 84, 84);
            shield2.SubjectText = "AntdUI";
            shield2.TabIndex = 1;
            shield2.Text = "shield2";
            // 
            // shield3
            // 
            shield3.Location = new System.Drawing.Point(138, 26);
            shield3.Name = "shield3";
            shield3.Padding = new System.Windows.Forms.Padding(5);
            shield3.Size = new System.Drawing.Size(154, 39);
            shield3.StatusColor = System.Drawing.Color.FromArgb(76, 205, 27);
            shield3.StatusText = "Apache 2.0";
            shield3.SubjectColor = System.Drawing.Color.FromArgb(84, 84, 84);
            shield3.SubjectText = "Licenses";
            shield3.TabIndex = 2;
            shield3.Text = "shield3";
            // 
            // shield4
            // 
            shield4.Location = new System.Drawing.Point(309, 31);
            shield4.Name = "shield4";
            shield4.Padding = new System.Windows.Forms.Padding(5);
            shield4.Radius = 0;
            shield4.Size = new System.Drawing.Size(120, 29);
            shield4.StatusColor = System.Drawing.Color.FromArgb(17, 130, 195);
            shield4.StatusText = "V2.0.6";
            shield4.SubjectColor = System.Drawing.Color.FromArgb(84, 84, 84);
            shield4.SubjectText = "AntdUI";
            shield4.TabIndex = 3;
            shield4.Text = "shield4";
            // 
            // Shield
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(shield4);
            Controls.Add(shield3);
            Controls.Add(shield2);
            Controls.Add(shield1);
            Font = new System.Drawing.Font("Microsoft YaHei UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "Shield";
            Size = new System.Drawing.Size(653, 231);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Shield shield1;
        private AntdUI.Shield shield2;
        private AntdUI.Shield shield3;
        private AntdUI.Shield shield4;
    }
}

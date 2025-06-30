using System.Drawing;

namespace Demo.Controls
{
    partial class Transfer
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
            this.panel1 = new AntdUI.Panel();
            this.transfer1 = new AntdUI.Transfer();
            this.panel2 = new AntdUI.Panel();
            this.btn_reload = new AntdUI.Button();
            this.label_target = new AntdUI.Label();
            this.label_source = new AntdUI.Label();
            //this.label2 = new AntdUI.Label();
            //this.switch_search = new AntdUI.Switch();
            this.label1 = new AntdUI.Label();
            this.switch_oneWay = new AntdUI.Switch();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.transfer1);
            this.panel1.Location = new System.Drawing.Point(20, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(760, 380);
            this.panel1.TabIndex = 0;
            // 
            // transfer1
            // 
            this.transfer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.transfer1.Location = new System.Drawing.Point(20, 50);
            this.transfer1.Name = "transfer1";
            this.transfer1.Size = new System.Drawing.Size(720, 310);
            this.transfer1.SourceTitle = "源列表";
            this.transfer1.TabIndex = 0;
            this.transfer1.TargetTitle = "目标列表";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.btn_reload);
            this.panel2.Controls.Add(this.label_target);
            this.panel2.Controls.Add(this.label_source);
            //this.panel2.Controls.Add(this.label2);
            //this.panel2.Controls.Add(this.switch_search);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.switch_oneWay);
            this.panel2.Location = new System.Drawing.Point(20, 20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(760, 70);
            this.panel2.TabIndex = 1;
            // 
            // btn_reload
            // 
            this.btn_reload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_reload.Location = new System.Drawing.Point(650, 30);
            this.btn_reload.Name = "btn_reload";
            this.btn_reload.Size = new System.Drawing.Size(90, 30);
            this.btn_reload.TabIndex = 6;
            this.btn_reload.Text = "重新加载";
            this.btn_reload.Click += new System.EventHandler(this.btn_reload_Click);
            // 
            // label_target
            // 
            this.label_target.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_target.Location = new System.Drawing.Point(520, 30);
            this.label_target.Name = "label_target";
            this.label_target.Size = new System.Drawing.Size(120, 30);
            this.label_target.TabIndex = 5;
            this.label_target.Text = "目标列表: 0项";
            this.label_target.TextAlign = ContentAlignment.TopRight;
            // 
            // label_source
            // 
            this.label_source.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_source.Location = new System.Drawing.Point(400, 30);
            this.label_source.Name = "label_source";
            this.label_source.Size = new System.Drawing.Size(120, 30);
            this.label_source.TabIndex = 4;
            this.label_source.Text = "源列表: 10项";
            this.label_source.TextAlign = ContentAlignment.TopRight;
            // 
            // label2
            // 
            //this.label2.Location = new System.Drawing.Point(180, 30);
            //this.label2.Name = "label2";
            //this.label2.Size = new System.Drawing.Size(80, 30);
            //this.label2.TabIndex = 3;
            //this.label2.Text = "显示搜索框:";
            // 
            // switch_search
            // 
            //this.switch_search.Location = new System.Drawing.Point(260, 30);
            //this.switch_search.Name = "switch_search";
            //this.switch_search.Size = new System.Drawing.Size(50, 30);
            //this.switch_search.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "单向模式:";
            // 
            // switch_oneWay
            // 
            this.switch_oneWay.Location = new System.Drawing.Point(100, 30);
            this.switch_oneWay.Name = "switch_oneWay";
            this.switch_oneWay.Size = new System.Drawing.Size(50, 30);
            this.switch_oneWay.TabIndex = 0;
            // 
            // Transfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Transfer";
            this.Size = new System.Drawing.Size(800, 500);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panel1;
        private AntdUI.Transfer transfer1;
        private AntdUI.Panel panel2;
        private AntdUI.Label label1;
        private AntdUI.Switch switch_oneWay;
        //private AntdUI.Label label2;
        //private AntdUI.Switch switch_search;
        private AntdUI.Label label_source;
        private AntdUI.Label label_target;
        private AntdUI.Button btn_reload;
    }
}
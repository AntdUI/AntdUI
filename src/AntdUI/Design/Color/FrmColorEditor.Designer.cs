namespace AntdUI.Design
{
    partial class FrmColorEditor
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
            this.input1 = new AntdUI.Input();
            this.SuspendLayout();
            // 
            // input1
            // 
            this.input1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.input1.Location = new System.Drawing.Point(43, 174);
            this.input1.Name = "input1";
            this.input1.Size = new System.Drawing.Size(144, 50);
            this.input1.TabIndex = 1;
            // 
            // FrmColorEditor
            // 
            this.Controls.Add(this.input1);
            this.MinimumSize = new System.Drawing.Size(240, 280);
            this.Name = "FrmColorEditor";
            this.Size = new System.Drawing.Size(240, 280);
            this.ResumeLayout(false);

        }

        #endregion
        private Input input1;
    }
}
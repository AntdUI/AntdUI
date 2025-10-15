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
            input1 = new Input();
            SuspendLayout();
            // 
            // input1
            // 
            input1.Anchor = System.Windows.Forms.AnchorStyles.None;
            input1.Location = new System.Drawing.Point(43, 174);
            input1.Name = "input1";
            input1.Size = new System.Drawing.Size(144, 50);
            input1.TabIndex = 1;
            // 
            // FrmColorEditor
            // 
            Controls.Add(input1);
            Name = "FrmColorEditor";
            MinimumSize = new System.Drawing.Size(240, 280);
            Size = new System.Drawing.Size(240, 280);
            ResumeLayout(false);

        }

        #endregion

        private Input input1;
    }
}
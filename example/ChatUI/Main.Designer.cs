namespace ChatUI
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
            AntdUI.Chat.MsgItem msgItem1 = new AntdUI.Chat.MsgItem();
            AntdUI.Chat.MsgItem msgItem2 = new AntdUI.Chat.MsgItem();
            AntdUI.Chat.MsgItem msgItem3 = new AntdUI.Chat.MsgItem();
            AntdUI.Chat.MsgItem msgItem4 = new AntdUI.Chat.MsgItem();
            win = new AntdUI.WindowBar();
            msgList = new AntdUI.Chat.MsgList();
            chatList = new AntdUI.Chat.ChatList();
            SuspendLayout();
            // 
            // win
            // 
            win.Dock = DockStyle.Top;
            win.Location = new Point(0, 0);
            win.Name = "win";
            win.Size = new Size(745, 36);
            win.SubText = "聊天气泡展示";
            win.TabIndex = 0;
            win.Text = "AntdUI GPT";
            // 
            // msgList
            // 
            msgList.BackColor = Color.White;
            msgList.Dock = DockStyle.Left;
            msgItem1.Icon = Properties.Resources.aduskin;
            msgItem1.Name = "WPF / Flutter UI";
            msgItem1.Text = "AduSkin：我初三就上班啦";
            msgItem1.Time = "10:24";
            msgItem2.Count = 999;
            msgItem2.Icon = Properties.Resources.av2;
            msgItem2.Name = "宝宝";
            msgItem2.Text = "今天给你买一束花";
            msgItem2.Time = "13:14";
            msgItem3.Count = 5;
            msgItem3.Icon = Properties.Resources.antd;
            msgItem3.Name = "AntdUI";
            msgItem3.Select = true;
            msgItem3.Text = "Tom：❤AntDesign设计语言的Winform界面库";
            msgItem3.Time = "前天";
            msgItem4.Count = 1;
            msgItem4.Icon = Properties.Resources.av1;
            msgItem4.Name = "Tom";
            msgItem4.Text = "晚上一起 Crazy Day 4";
            msgItem4.Time = "疯狂星期四";
            msgList.Items.AddRange(new AntdUI.Chat.MsgItem[] { msgItem1, msgItem2, msgItem3, msgItem4 });
            msgList.Location = new Point(0, 36);
            msgList.Margin = new Padding(2);
            msgList.Name = "msgList";
            msgList.Size = new Size(246, 392);
            msgList.TabIndex = 0;
            // 
            // chatList
            // 
            chatList.Dock = DockStyle.Fill;
            chatList.Location = new Point(246, 36);
            chatList.Name = "chatList";
            chatList.Size = new Size(499, 392);
            chatList.TabIndex = 0;
            // 
            // Main
            // 
            ClientSize = new Size(745, 428);
            Controls.Add(chatList);
            Controls.Add(msgList);
            Controls.Add(win);
            Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Margin = new Padding(2);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.WindowBar win;
        private AntdUI.Chat.MsgList msgList;
        private AntdUI.Chat.ChatList chatList;
    }
}

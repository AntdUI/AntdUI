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

        private void InitializeComponent()
        {
            AntdUI.Chat.MsgItem msgItem1 = new AntdUI.Chat.MsgItem();
            AntdUI.Chat.MsgItem msgItem2 = new AntdUI.Chat.MsgItem();
            AntdUI.Chat.MsgItem msgItem3 = new AntdUI.Chat.MsgItem();
            AntdUI.Chat.MsgItem msgItem4 = new AntdUI.Chat.MsgItem();
            win = new AntdUI.PageHeader();
            colorTheme = new AntdUI.ColorPicker();
            btn_mode = new AntdUI.Button();
            msgList = new AntdUI.Chat.MsgList();
            chatList = new AntdUI.Chat.ChatList();
            win.SuspendLayout();
            SuspendLayout();
            // 
            // win
            // 
            win.Controls.Add(colorTheme);
            win.Controls.Add(btn_mode);
            win.Dock = DockStyle.Top;
            win.Location = new Point(0, 0);
            win.Name = "win";
            win.ShowButton = true;
            win.ShowIcon = true;
            win.Size = new Size(745, 36);
            win.SubText = "聊天气泡展示";
            win.TabIndex = 0;
            win.Text = "AntdUI GPT";
            // 
            // colorTheme
            // 
            colorTheme.Dock = DockStyle.Right;
            colorTheme.Location = new Point(529, 0);
            colorTheme.Name = "colorTheme";
            colorTheme.Padding = new Padding(4);
            colorTheme.Size = new Size(36, 36);
            colorTheme.TabIndex = 10;
            colorTheme.ValueChanged += colorTheme_ValueChanged;
            // 
            // btn_mode
            // 
            btn_mode.Dock = DockStyle.Right;
            btn_mode.Ghost = true;
            btn_mode.IconSvg = "SunOutlined";
            btn_mode.Location = new Point(565, 0);
            btn_mode.Name = "btn_mode";
            btn_mode.Radius = 0;
            btn_mode.Size = new Size(36, 36);
            btn_mode.TabIndex = 9;
            btn_mode.ToggleIconSvg = "MoonOutlined";
            btn_mode.WaveSize = 0;
            btn_mode.Click += btn_mode_Click;
            // 
            // msgList
            // 
            msgList.Dock = DockStyle.Left;
            msgItem1.Icon = Properties.Resources.aduskin;
            msgItem1.Name = "WPF / Flutter UI";
            msgItem1.Text = "AduSkin：我初三就上班啦";
            msgItem1.Time = "10:24";
            msgItem2.Badge = "99+";
            msgItem2.Count = 999;
            msgItem2.Icon = Properties.Resources.av2;
            msgItem2.Name = "宝宝";
            msgItem2.Text = "今天给你买一束花";
            msgItem2.Time = "13:14";
            msgItem3.Badge = "5";
            msgItem3.Count = 5;
            msgItem3.Icon = Properties.Resources.antd;
            msgItem3.Name = "AntdUI";
            msgItem3.Select = true;
            msgItem3.Text = "Tom：❤️AntDesign设计语言的Winform界面库";
            msgItem3.Time = "前天";
            msgItem4.Badge = "";
            msgItem4.Count = 1;
            msgItem4.Icon = Properties.Resources.av1;
            msgItem4.Name = "Tom";
            msgItem4.Text = "晚上一起 🍔 Crazy Day 4";
            msgItem4.Time = "疯狂星期四";
            msgList.Items.Add(msgItem1);
            msgList.Items.Add(msgItem2);
            msgList.Items.Add(msgItem3);
            msgList.Items.Add(msgItem4);
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
            Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            Margin = new Padding(2);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ChatUI";
            win.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader win;
        private AntdUI.Chat.MsgList msgList;
        private AntdUI.Chat.ChatList chatList;
        private AntdUI.ColorPicker colorTheme;
        private AntdUI.Button btn_mode;
    }
}

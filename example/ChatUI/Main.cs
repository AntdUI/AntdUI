// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using AntdUI;
using System.Text;

namespace ChatUI
{
    public partial class Main : AntdUI.Window
    {
        public Main()
        {
            InitializeComponent();
            Theme().Call(dark => msgList.BackColor = AntdUI.Style.Db.BgBase);
            msgList.ItemClick += msgList_ItemClick;
            colorTheme.Presets = new Color[] {
                "#f44336".ToColor(),
                "#e91e63".ToColor(),
                "#9c27b0".ToColor(),
                "#673ab7".ToColor(),
                "#3f51b5".ToColor(),
                "#2196f3".ToColor(),
                "#03a9f4".ToColor(),
                "#00bcd4".ToColor(),
                "#009688".ToColor(),
                "#4caf50".ToColor(),
                "#cddc39".ToColor(),
                "#ffeb3b".ToColor(),
                "#ffc107".ToColor(),
                "#ff9800".ToColor(),
                "#ff5722".ToColor(),
                "#795548".ToColor(),
                "#9e9e9e".ToColor(),
                "#607d8b".ToColor()
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("阿威十八式 🙌🖖🤘👋", Properties.Resources.aduskin, "AduSkin"));
                Thread.Sleep(700);

                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("完美主义 🥣💲🐖👚", Properties.Resources.av1, "Tom") { Me = true });
                Thread.Sleep(700);

                if (File.Exists(BasePath + "doc/pre/banner.png") && File.Exists(BasePath + "src/logo.png"))
                {
                    chatList.AddToBottom(new AntdUI.Chat.TextChatItem("🦄 Winform interface library based on the Ant Design\n" + "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(BasePath + "doc/pre/banner.png")), (Bitmap)Image.FromFile(BasePath + "src/logo.png"), "AntdUI"));
                    Thread.Sleep(700);
                }

                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("搭配 Nuget Tom.HttpLib 可以轻松实现GPT流式传输\n\nhttps://gitee.com/EVA-SS/HttpLib", Properties.Resources.av1, "Tom") { Me = true });
                Thread.Sleep(700);

                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("99+个需求已通过许愿表单进入许愿池⛲啦~", Properties.Resources.av1, "Tom") { Me = true });
                Thread.Sleep(700);

                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("城区这车油耗就是高，没啥说的，suv本来就不省油", Properties.Resources.aduskin, "AduSkin"));
                Thread.Sleep(700);

                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("咱这车换个轮眉得多少钱啊", Properties.Resources.av2, "2.0T 银"));
                Thread.Sleep(1000);

                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("百来块吧", Properties.Resources.av1, "Tom") { Me = true });
                Thread.Sleep(2000);

                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("我看一般都前端做的，winform还没见过", Properties.Resources.av3, "阿枫"));
                Thread.Sleep(700);

                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("别聊这些\r\n------------------------------", Properties.Resources.av2, "2.0T 银"));
                Thread.Sleep(1200);

                #region 模拟GPT

                var msg = new AntdUI.Chat.TextChatItem("", Properties.Resources.av2, "2.0T 银");
                chatList.AddToBottom(msg);
                msg.Loading = true;
                Thread.Sleep(1200);

                var strs = new List<string>() {
                    "It seems like your question",
                    " contains phrases that are taken from religious or biblical texts, ",
                    "particularly from the Book of Genesis in the Bible. ",
                    "To interpret your question, it appears you are asking ",
                    "about the initial reign and the significance of ",
                    "the firmament, dominion, and the first tree in the creation ",
                    "story.\r\n\r\nIn the Book of Genesis, God is depicted as creating the universe and everything in it. ",
                    "The firmament",
                    " refers to the sky or ",
                    "the space that separates the",
                    " Earth from ",
                    "the waters above. On ",
                    "the second day of creation, according to Genesis 1:6-8, God created the firmament",
                    " to divide the waters below (the seas) from the waters above.\r\n\r\nThe concept of ",
                    "dominion comes into play on the sixth day of creation when God created humans,",
                    " as described in Genesis 1:26-28.",
                    " God gave humans dominion over the fish,",
                    " birds, and every living",
                    " creature",
                    " on the",
                    " Earth, as well as over the plants and ",
                    "trees.\r\n\r\nThe first tree mentioned in the Bible is the Tree of ",
                    "the Knowledge of Good and Evil, which appears in the Garden of Eden story",
                    " in Genesis 2:9. God commanded Adam, the first human, not to ",
                    "eat from this tree, warning that doing so would result in death.",
                    " The Tree of Life is another significant tree mentioned in the Garden of",
                    " Eden.\r\n\r\nAs for the phrase \"the seas he i were cattle Under living.",
                    " It may beast every forth place,\" it is unclear what you are asking.",
                    " However, if you are referring to the creation of living creatures, Genesis",
                    " 1:20-23 describes how God created aquatic creatures and birds on",
                    " the fifth day, and land animals and humans on the sixth day." };
                string text = string.Join("", strs);
                var ran = new Random();
                bool run = true;
                int i = 0;
                var _sb = new StringBuilder();
                while (run)
                {
                    Thread.Sleep(ran.Next(2, 100));
                    int len = ran.Next(1, 16);

                    if (text.Length < i + len)
                    {
                        len = text.Length - i;
                        run = false;
                    }
                    if (len > 0)
                    {
                        bool isbut = chatList.IsBottom;
                        _sb.Append(text.Substring(i, len));
                        msg.Text = _sb.ToString();
                        if (isbut) chatList.ToBottom();
                        i += len;
                    }
                }
                msg.Loading = false;

                #endregion
            });
        }

        private void msgList_ItemClick(object sender, AntdUI.MsgItemClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                e.Item.Select = true;
                AntdUI.ContextMenuStrip.open(this, it =>
                {
                    AntdUI.Message.info(this, "点击内容：" + it.Text);
                }, new AntdUI.IContextMenuStripItem[]
                {
                    new AntdUI.ContextMenuStripItem("标记已读").SetIcon("CheckOutlined"),
                    new AntdUI.ContextMenuStripItem("删除此聊天").SetIcon("DeleteOutlined"),
                });
            }
        }

        static string BasePath = "../../../../";

        private void colorTheme_ValueChanged(object sender, AntdUI.ColorEventArgs e)
        {
            AntdUI.Style.SetPrimary(e.Value);
            Refresh();
        }

        private void btn_mode_Click(object sender, EventArgs e)
        {
            AntdUI.Config.IsDark = !AntdUI.Config.IsDark;
        }
    }
}
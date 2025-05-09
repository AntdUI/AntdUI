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

namespace ChatUI
{
    public partial class Main : AntdUI.Window
    {
        public Main()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Task.Run(() =>
            {
                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("阿威十八式 🙌🖖🤘👋", Properties.Resources.aduskin, "AduSkin"));
                Thread.Sleep(700);

                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("完美主义 🥣💲🐖👚", Properties.Resources.av1, "Tom") { Me = true });
                Thread.Sleep(700);

                if (File.Exists("../../../../../doc/pre/banner.png") && File.Exists("../../../../../src/logo.png"))
                {
                    chatList.AddToBottom(new AntdUI.Chat.TextChatItem("🦄 Winform UI library use Ant Design 5.0\n" + "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes("../../../../../doc/pre/banner.png")), (Bitmap)Image.FromFile("../../../../../src/logo.png"), "AntdUI"));
                    Thread.Sleep(700);
                }

                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("搭配 Nuget Tom.HttpLib 可以轻松实现GPT流式传输\n\nhttps://gitee.com/EVA-SS/HttpLib", Properties.Resources.av1, "Tom") { Me = true });
                Thread.Sleep(700);

                chatList.AddToBottom(new AntdUI.Chat.TextChatItem("估计还是因为长得帅把", Properties.Resources.av1, "Tom") { Me = true });
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
                while (run)
                {
                    Thread.Sleep(ran.Next(10, 200));
                    int len = ran.Next(1, 16);

                    if (text.Length < i + len)
                    {
                        len = text.Length - i;
                        run = false;
                    }
                    if (len > 0)
                    {
                        bool isbut = chatList.IsBottom;
                        msg.Text += text.Substring(i, len);
                        if (isbut) chatList.ToBottom();
                        i += len;
                    }
                }
                msg.Loading = false;

                #endregion
            });
        }
    }
}

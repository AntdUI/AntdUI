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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class ContextMenuStrip : UserControl
    {
        Form form;
        public ContextMenuStrip(Form _form)
        {
            form = _form;
            InitializeComponent();

            string svg_back = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M793 242H366v-74c0-6.7-7.7-10.4-12.9-6.3l-142 112c-4.1 3.2-4.1 9.4 0 12.6l142 112c5.2 4.1 12.9 0.4 12.9-6.3v-74h415v470H175c-4.4 0-8 3.6-8 8v60c0 4.4 3.6 8 8 8h618c35.3 0 64-28.7 64-64V306c0-35.3-28.7-64-64-64z\"></path></svg>", svg_refresh = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M758.2 839.1C851.8 765.9 912 651.9 912 523.9 912 303 733.5 124.3 512.6 124 291.4 123.7 112 302.8 112 523.9c0 125.2 57.5 236.9 147.6 310.2 3.5 2.8 8.6 2.2 11.4-1.3l39.4-50.5c2.7-3.4 2.1-8.3-1.2-11.1-8.1-6.6-15.9-13.7-23.4-21.2-29.4-29.4-52.5-63.6-68.6-101.7C200.4 609 192 567.1 192 523.9s8.4-85.1 25.1-124.5c16.1-38.1 39.2-72.3 68.6-101.7 29.4-29.4 63.6-52.5 101.7-68.6C426.9 212.4 468.8 204 512 204s85.1 8.4 124.5 25.1c38.1 16.1 72.3 39.2 101.7 68.6 29.4 29.4 52.5 63.6 68.6 101.7 16.7 39.4 25.1 81.3 25.1 124.5s-8.4 85.1-25.1 124.5c-16.1 38.1-39.2 72.3-68.6 101.7-9.3 9.3-19.1 18-29.3 26L668.2 724c-4.1-5.3-12.5-3.5-14.1 3l-39.6 162.2c-1.2 5 2.6 9.9 7.7 9.9l167 0.8c6.7 0 10.5-7.7 6.3-12.9l-37.3-47.9z\"></path></svg>", svg_save = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M893.3 293.3L730.7 130.7c-7.5-7.5-16.7-13-26.7-16V112H144c-17.7 0-32 14.3-32 32v736c0 17.7 14.3 32 32 32h736c17.7 0 32-14.3 32-32V338.5c0-17-6.7-33.2-18.7-45.2zM384 184h256v104H384V184z m456 656H184V184h136v136c0 17.7 14.3 32 32 32h320c17.7 0 32-14.3 32-32V205.8l136 136V840z\" ></path><path d=\"M512 442c-79.5 0-144 64.5-144 144s64.5 144 144 144 144-64.5 144-144-64.5-144-144-144z m0 224c-44.2 0-80-35.8-80-80s35.8-80 80-80 80 35.8 80 80-35.8 80-80 80z\"></path></svg>", svg_print = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M820 436h-40c-4.4 0-8 3.6-8 8v40c0 4.4 3.6 8 8 8h40c4.4 0 8-3.6 8-8v-40c0-4.4-3.6-8-8-8z\"></path><path d=\"M852 332H732V120c0-4.4-3.6-8-8-8H300c-4.4 0-8 3.6-8 8v212H172c-44.2 0-80 35.8-80 80v328c0 17.7 14.3 32 32 32h168v132c0 4.4 3.6 8 8 8h424c4.4 0 8-3.6 8-8V772h168c17.7 0 32-14.3 32-32V412c0-44.2-35.8-80-80-80zM360 180h304v152H360V180z m304 664H360V568h304v276z m200-140H732V500H292v204H160V412c0-6.6 5.4-12 12-12h680c6.6 0 12 5.4 12 12v292z\"></path></svg>", svg_laptop = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M956.9 845.1L896.4 632V168c0-17.7-14.3-32-32-32h-704c-17.7 0-32 14.3-32 32v464L67.9 845.1C60.4 866 75.8 888 98 888h828.8c22.2 0 37.6-22 30.1-42.9zM200.4 208h624v395h-624V208z m228.3 608l8.1-37h150.3l8.1 37H428.7z m224 0l-19.1-86.7c-0.8-3.7-4.1-6.3-7.8-6.3H398.2c-3.8 0-7 2.6-7.8 6.3L371.3 816H151l42.3-149h638.2l42.3 149H652.7z\"></path></svg>", svg_qr = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M468 128H160c-17.7 0-32 14.3-32 32v308c0 4.4 3.6 8 8 8h332c4.4 0 8-3.6 8-8V136c0-4.4-3.6-8-8-8z m-56 284H192V192h220v220z\"></path><path d=\"M274 338h56c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8h-56c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8zM468 548H136c-4.4 0-8 3.6-8 8v308c0 17.7 14.3 32 32 32h308c4.4 0 8-3.6 8-8V556c0-4.4-3.6-8-8-8z m-56 284H192V612h220v220z\"></path><path d=\"M274 758h56c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8h-56c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8zM864 128H556c-4.4 0-8 3.6-8 8v332c0 4.4 3.6 8 8 8h332c4.4 0 8-3.6 8-8V160c0-17.7-14.3-32-32-32z m-32 284H612V192h220v220z\"></path><path d=\"M694 338h56c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8h-56c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8zM888 548h-48c-4.4 0-8 3.6-8 8v134h-78V556c0-4.4-3.6-8-8-8H556c-4.4 0-8 3.6-8 8v332c0 4.4 3.6 8 8 8h48c4.4 0 8-3.6 8-8V644h78v102c0 4.4 3.6 8 8 8h190c4.4 0 8-3.6 8-8V556c0-4.4-3.6-8-8-8z\"></path><path d=\"M746 832h-48c-4.4 0-8 3.6-8 8v48c0 4.4 3.6 8 8 8h48c4.4 0 8-3.6 8-8v-48c0-4.4-3.6-8-8-8zM888 832h-48c-4.4 0-8 3.6-8 8v48c0 4.4 3.6 8 8 8h48c4.4 0 8-3.6 8-8v-48c0-4.4-3.6-8-8-8z\"></path></svg>", svg_a = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M904 816H120c-4.4 0-8 3.6-8 8v80c0 4.4 3.6 8 8 8h784c4.4 0 8-3.6 8-8v-80c0-4.4-3.6-8-8-8zM253.7 736h85c4.2 0 8-2.7 9.3-6.8l53.7-166h219.2l53.2 166c1.3 4 5 6.8 9.3 6.8h89.1c1.1 0 2.2-0.2 3.2-0.5 5.1-1.8 7.8-7.3 6-12.4L573.6 118.6c-1.4-3.9-5.1-6.6-9.2-6.6H462.1c-4.2 0-7.9 2.6-9.2 6.6L244.5 723.1c-0.4 1-0.5 2.1-0.5 3.2-0.1 5.3 4.3 9.7 9.7 9.7z m255.9-516.1h4.1l83.8 263.8H424.9l84.7-263.8z\"></path></svg>", svg_fy = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M140 188h584v164h76V144c0-17.7-14.3-32-32-32H96c-17.7 0-32 14.3-32 32v736c0 17.7 14.3 32 32 32h544v-76H140V188z\"></path><path d=\"M414.3 256h-60.6c-3.4 0-6.4 2.2-7.6 5.4L219 629.4c-0.3 0.8-0.4 1.7-0.4 2.6 0 4.4 3.6 8 8 8h55.1c3.4 0 6.4-2.2 7.6-5.4L322 540h196.2L422 261.4c-1.3-3.2-4.3-5.4-7.7-5.4z m12.4 228h-85.5L384 360.2 426.7 484zM936 528H800v-93c0-4.4-3.6-8-8-8h-56c-4.4 0-8 3.6-8 8v93H592c-13.3 0-24 10.7-24 24v176c0 13.3 10.7 24 24 24h136v152c0 4.4 3.6 8 8 8h56c4.4 0 8-3.6 8-8V752h136c13.3 0 24-10.7 24-24V552c0-13.3-10.7-24-24-24zM728 680h-88v-80h88v80z m160 0h-88v-80h88v80z\"></path></svg>", svg_add = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M464 144H160c-8.8 0-16 7.2-16 16v304c0 8.8 7.2 16 16 16h304c8.8 0 16-7.2 16-16V160c0-8.8-7.2-16-16-16z m-52 268H212V212h200v200zM864 144H560c-8.8 0-16 7.2-16 16v304c0 8.8 7.2 16 16 16h304c8.8 0 16-7.2 16-16V160c0-8.8-7.2-16-16-16z m-52 268H612V212h200v200zM864 544H560c-8.8 0-16 7.2-16 16v304c0 8.8 7.2 16 16 16h304c8.8 0 16-7.2 16-16V560c0-8.8-7.2-16-16-16z m-52 268H612V612h200v200zM424 712H296V584c0-4.4-3.6-8-8-8h-48c-4.4 0-8 3.6-8 8v128H104c-4.4 0-8 3.6-8 8v48c0 4.4 3.6 8 8 8h128v128c0 4.4 3.6 8 8 8h48c4.4 0 8-3.6 8-8V776h128c4.4 0 8-3.6 8-8v-48c0-4.4-3.6-8-8-8z\"></path></svg>", svg_share = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M752 664c-28.5 0-54.8 10-75.4 26.7L469.4 540.8c1.7-9.3 2.6-19 2.6-28.8s-0.9-19.4-2.6-28.8l207.2-149.9C697.2 350 723.5 360 752 360c66.2 0 120-53.8 120-120s-53.8-120-120-120-120 53.8-120 120c0 11.6 1.6 22.7 4.7 33.3L439.9 415.8C410.7 377.1 364.3 352 312 352c-88.4 0-160 71.6-160 160s71.6 160 160 160c52.3 0 98.7-25.1 127.9-63.8l196.8 142.5c-3.1 10.6-4.7 21.8-4.7 33.3 0 66.2 53.8 120 120 120s120-53.8 120-120-53.8-120-120-120z m0-476c28.7 0 52 23.3 52 52s-23.3 52-52 52-52-23.3-52-52 23.3-52 52-52zM312 600c-48.5 0-88-39.5-88-88s39.5-88 88-88 88 39.5 88 88-39.5 88-88 88z m440 236c-28.7 0-52-23.3-52-52s23.3-52 52-52 52 23.3 52 52-23.3 52-52 52z\"></path></svg>", svg_about = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M716.3 313.8c19-18.9 19-49.7 0-68.6l-69.9-69.9 0.1 0.1c-18.5-18.5-50.3-50.3-95.3-95.2-21.2-20.7-55.5-20.5-76.5 0.5L80.9 474.2c-21.2 21.1-21.2 55.3 0 76.4L474.6 944c21.2 21.1 55.4 21.1 76.5 0l165.1-165c19-18.9 19-49.7 0-68.6-19-18.9-49.7-18.9-68.7 0l-125 125.2c-5.2 5.2-13.3 5.2-18.5 0L189.5 521.4c-5.2-5.2-5.2-13.3 0-18.5l314.4-314.2c0.4-0.4 0.9-0.7 1.3-1.1 5.2-4.1 12.4-3.7 17.2 1.1l125.2 125.1c19 19 49.8 19 68.7 0z\"></path><path d=\"M408.6 514.4a106.3 106.2 0 1 0 212.6 0 106.3 106.2 0 1 0-212.6 0Z\"></path><path d=\"M944.8 475.8L821.9 353.5c-19-18.9-49.8-18.9-68.7 0.1-19 18.9-19 49.7 0 68.6l83 82.9c5.2 5.2 5.2 13.3 0 18.5l-81.8 81.7c-19 18.9-19 49.7 0 68.6 19 18.9 49.7 18.9 68.7 0l121.8-121.7c21.1-21.1 21.1-55.2-0.1-76.4z\"></path></svg>";

            menulist = new AntdUI.IContextMenuStripItem[]
            {
                new AntdUI.ContextMenuStripItem("返回", "Alt+向左键").SetIcon(svg_back),
                new AntdUI.ContextMenuStripItem("刷新", "Ctrl+R").SetIcon(svg_refresh),
                new AntdUI.ContextMenuStripItemDivider(),
                new AntdUI.ContextMenuStripItem("另存为", "Ctrl+S").SetIcon(svg_save),
                new AntdUI.ContextMenuStripItem("打印", "Ctrl+P").SetIcon(svg_print),
                new AntdUI.ContextMenuStripItemDivider(),
                new AntdUI.ContextMenuStripItem("发送标签页到你的设备").SetIcon(svg_laptop),
                new AntdUI.ContextMenuStripItem("为此页面创建QR代码").SetIcon(svg_qr),
                new AntdUI.ContextMenuStripItem("大声朗读", "Ctrl+Shift+U").SetIcon(svg_a),
                new AntdUI.ContextMenuStripItem("翻译为 中文(简体)").SetIcon(svg_fy),
                new AntdUI.ContextMenuStripItem("将页面添加到集锦").SetIcon(svg_add).SetSub(
                    new AntdUI.ContextMenuStripItem("返回", "Alt+向左键").SetIcon(svg_back).SetSub(
                        new AntdUI.ContextMenuStripItem("返回", "Alt+向左键").SetIcon(svg_back),
                        new AntdUI.ContextMenuStripItem("刷新", "Ctrl+R").SetIcon(svg_refresh),
                        new AntdUI.ContextMenuStripItemDivider(),
                        new AntdUI.ContextMenuStripItem("另存为", "Ctrl+S").SetIcon(svg_save),
                        new AntdUI.ContextMenuStripItem("打印", "Ctrl+P").SetIcon(svg_print)
                        ),
                    new AntdUI.ContextMenuStripItem("刷新", "Ctrl+R").SetIcon(svg_refresh),
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("另存为", "Ctrl+S").SetIcon(svg_save),
                    new AntdUI.ContextMenuStripItem("打印", "Ctrl+P").SetIcon(svg_print)
                    ),
                new AntdUI.ContextMenuStripItemDivider(),
                new AntdUI.ContextMenuStripItem("共享").SetIcon(svg_share),
                new AntdUI.ContextMenuStripItem("关于").SetIcon(svg_about)
            };
        }

        AntdUI.IContextMenuStripItem[] menulist = { };

        private void ContextMenuStrip_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                AntdUI.ContextMenuStrip.open(this, it =>
                {
                    Debug.WriteLine("点击内容：" + it.Text);

                    AntdUI.Message.info(form, "点击内容：" + it.Text);
                }, menulist);
            }
        }

        #region 回调案例

        private void Control_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                AntdUI.ContextMenuStrip.Config config = new AntdUI.ContextMenuStrip.Config(this, RightKey, menulist);
                config.Font = new Font("Microsoft YaHei UI", 10f, FontStyle.Bold);
                AntdUI.ContextMenuStrip.open(config);
            }
        }

        private void RightKey(AntdUI.ContextMenuStripItem it)
        {
            if (it.Text.Equals("新增"))
            {
                // 执行对应的操作
            }
            if (it.Text.Equals("修改"))
            {

            }
            if (it.Text.Equals("删除"))
            {

            }
            if (it.Text.Equals("刷新"))
            {

            }
            if (it.Text.Equals("重置"))
            {

            }
        }

        #endregion

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            AntdUI.ContextMenuStrip.open(this, it =>
            {
                Debug.WriteLine("点击内容：" + it.Text);

                AntdUI.Message.info(form, "点击内容：" + it.Text);
            }, menulist);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.StopAnimationBlink();
            notifyIcon1.Icon = form.Icon;
            notifyIcon1.Visible = !notifyIcon1.Visible;
            if (notifyIcon1.Visible) notifyIcon1.ShowBalloonTip(2000, "Hello, Ant Design!", header1.Description, ToolTipIcon.Info);
        }

        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            AntdUI.ContextMenuStrip.open(form, notifyIcon1, it =>
            {
                Debug.WriteLine("托盘点击内容：" + it.Text);
                AntdUI.Message.info(form, "托盘点击内容：" + it.Text);
            }, menulist);
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            button2.AnimationBlinkTransition(200, AntdUI.Style.Db.Primary, Color.FromArgb(255, 0, 0), Color.FromArgb(255, 128, 0),
                Color.FromArgb(255, 255, 0), Color.FromArgb(0, 255, 0), Color.FromArgb(0, 255, 255), Color.FromArgb(0, 0, 255), Color.FromArgb(0, 0, 255));
        }
    }
}
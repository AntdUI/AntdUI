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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Overview.Controls
{
    public partial class Icon : UserControl
    {
        Form form;
        public Icon(Form _form)
        {
            form = _form;
            InitializeComponent();
            LoadData();
        }

        #region 数据

        private void segmented1_SelectIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            LoadData();
        }

        void LoadData()
        {
            var data = GetData();
            var svgs = new List<AntdUI.VirtualItem>(data.Count);
            foreach (var it in data)
            {
                if (it.Value == null) svgs.Add(new TItem(it.Key));
                else svgs.Add(new VItem(it.Key, it.Value));
            }
            vpanel.Items.Clear();
            txt_search.Text = "";
            vpanel.Items.AddRange(svgs);
        }

        Dictionary<string, string> GetData()
        {
            var svgs = new Dictionary<string, string>(AntdUI.SvgDb.Custom.Count);
            if (segmented1.SelectIndex == 0)
            {
                svgs.Add("方向性图标", null);
                foreach (var it in AntdUI.SvgDb.Custom)
                {
                    if (it.Key == "StepBackwardFilled") return svgs;
                    else if (it.Key == "QuestionOutlined") svgs.Add("提示建议性图标", null);
                    else if (it.Key == "EditOutlined") svgs.Add("编辑类图标", null);
                    else if (it.Key == "AreaChartOutlined") svgs.Add("数据类图标", null);
                    else if (it.Key == "AndroidOutlined") svgs.Add("品牌和标识", null);
                    else if (it.Key == "AccountBookOutlined") svgs.Add("网站通用图标", null);
                    svgs.Add(it.Key, it.Value);
                }
            }
            else
            {
                svgs.Add("方向性图标", null);
                bool isadd = false;
                foreach (var it in AntdUI.SvgDb.Custom)
                {
                    if (it.Key == "UpCircleTwoTone") return svgs;
                    else if (it.Key == "StepBackwardFilled") isadd = true;
                    else if (it.Key == "QuestionCircleFilled") svgs.Add("提示建议性图标", null);
                    else if (it.Key == "EditFilled") svgs.Add("编辑类图标", null);
                    else if (it.Key == "PieChartFilled") svgs.Add("数据类图标", null);
                    else if (it.Key == "AndroidFilled") svgs.Add("品牌和标识", null);
                    else if (it.Key == "AccountBookFilled") svgs.Add("网站通用图标", null);
                    if (isadd) svgs.Add(it.Key, it.Value);
                }
            }
            return svgs;
        }

        #endregion

        #region 渲染

        class TItem : AntdUI.VirtualItem
        {
            string Key;
            public TItem(string key) { Tag = Key = key; }

            StringFormat s_f = AntdUI.Helper.SF_NoWrap(lr: StringAlignment.Near);
            public override void Paint(Graphics g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                using (var fore = new SolidBrush(AntdUI.Style.Db.TextBase))
                {
                    using (var font_title = new Font(e.Panel.Font.FontFamily, 12F, FontStyle.Bold))
                    {
                        AntdUI.CorrectionTextRendering.DrawStr(g, Key, font_title, fore, new Rectangle(e.Rect.X, e.Rect.Y + y, e.Rect.Width, e.Rect.Height - y), s_f);
                    }
                }
            }
            int y = 0;
            public override Size Size(Graphics g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                y = (int)(8 * dpi);
                return new Size(e.Rect.Width, (int)(36 * dpi) + y);
            }
        }
        class VItem : AntdUI.VirtualItem
        {
            public string Key, Value;
            public VItem(string key, string value) { Tag = Key = key; Value = value; }

            StringFormat s_f = AntdUI.Helper.SF_NoWrap();
            Bitmap bmp = null, bmp_ac = null;
            public override void Paint(Graphics g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                int icon_size = (int)(36 * dpi), text_size = (int)(24 * dpi), y = e.Rect.Y + (e.Rect.Height - (icon_size + text_size)) / 2;
                var rect_icon = new Rectangle(e.Rect.X + (e.Rect.Width - icon_size) / 2, y, icon_size, icon_size);
                var rect_text = new Rectangle(e.Rect.X, y + icon_size / 2 + text_size, e.Rect.Width, text_size);
                if (Hover)
                {
                    using (var path = AntdUI.Helper.RoundPath(e.Rect, e.Radius))
                    {
                        using (var brush = new SolidBrush(AntdUI.Style.Db.Primary))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    if (bmp_ac == null) bmp_ac = AntdUI.SvgExtend.SvgToBmp(Value, icon_size, icon_size, AntdUI.Style.Db.PrimaryColor);
                    g.DrawImage(bmp_ac, rect_icon);

                    using (var fore = new SolidBrush(AntdUI.Style.Db.PrimaryColor))
                    {
                        AntdUI.CorrectionTextRendering.DrawStr(g, Key, e.Panel.Font, fore, rect_text, s_f);
                    }
                }
                else
                {
                    if (bmp == null) bmp = AntdUI.SvgExtend.SvgToBmp(Value, icon_size, icon_size, AntdUI.Style.Db.Text);
                    g.DrawImage(bmp, rect_icon);
                    using (var fore = new SolidBrush(AntdUI.Style.Db.Text))
                    {
                        AntdUI.CorrectionTextRendering.DrawStr(g, Key, e.Panel.Font, fore, rect_text, s_f);
                    }
                }

            }
            public override Size Size(Graphics g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                return new Size((int)(200 * dpi), (int)(100 * dpi));
            }
        }

        #endregion

        private void vpanel_ItemClick(object sender, AntdUI.VirtualItemEventArgs e)
        {
            if (e.Item is VItem item)
            {
                if (AntdUI.Helper.ClipboardSetText(this, item.Key)) AntdUI.Message.success(form, item.Key + " 复制成功");
                else AntdUI.Message.error(form, item.Key + " 复制失败");
            }
        }

        #region Win32操作剪贴板

        private static bool SetClipBoardText(string Text)
        {
            try
            {
                uint uformat = 1;
                if (!OpenClipboard(IntPtr.Zero)) return false;
                if (!EmptyClipboard()) return false;
                var r = SetClipboardData(uformat, System.Runtime.InteropServices.Marshal.StringToCoTaskMemAnsi(Text));
                if (r == IntPtr.Zero) return false;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseClipboard();
            }
        }

        /// <summary>
        /// 打开剪切板
        /// </summary>
        /// <param name="hWndNewOwner"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        extern static bool OpenClipboard(IntPtr hWndNewOwner);

        /// <summary>
        /// 关闭剪切板
        /// </summary>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        extern static bool CloseClipboard();

        // 清空剪贴板
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        extern static bool EmptyClipboard();

        /// <summary>
        /// 设置剪切板内容
        /// </summary>
        /// <param name="uFormat"></param>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        extern static IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        #endregion

        #region 搜索

        private void txt_search_TextChanged(object sender, System.EventArgs e) => LoadSearchList();
        private void txt_search_SuffixClick(object sender, MouseEventArgs e) => LoadSearchList();

        void LoadSearchList()
        {
            string search = txt_search.Text;
            BeginInvoke(new Action(() =>
            {
                vpanel.PauseLayout = true;
                if (string.IsNullOrEmpty(search))
                {
                    foreach (var it in vpanel.Items) it.Visible = true;
                }
                else
                {
                    string searchLower = search.ToLower();
                    foreach (var it in vpanel.Items)
                    {
                        if (it is VItem item) it.Visible = item.Key.ToLower().Contains(search);
                    }
                }
                vpanel.PauseLayout = false;
            }));
        }

        #endregion
    }
}
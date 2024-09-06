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
                svgs.Add(new TItem(it.Key, it.Value));
                svgs.AddRange(it.Value);
            }
            vpanel.Items.Clear();
            txt_search.Text = "";
            vpanel.Items.AddRange(svgs);
        }

        Dictionary<string, List<VItem>> GetData()
        {
            var dir = new Dictionary<string, List<VItem>>(AntdUI.SvgDb.Custom.Count);
            var tmp = new List<VItem>(AntdUI.SvgDb.Custom.Count);
            if (segmented1.SelectIndex == 0)
            {
                foreach (var it in AntdUI.SvgDb.Custom)
                {
                    if (it.Key == "QuestionOutlined")
                    {
                        dir.Add("方向性图标", new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "EditOutlined")
                    {
                        dir.Add("提示建议性图标", new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AreaChartOutlined")
                    {
                        dir.Add("编辑类图标", new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AndroidOutlined")
                    {
                        dir.Add("数据类图标", new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AccountBookOutlined")
                    {
                        dir.Add("品牌和标识", new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "StepBackwardFilled")
                    {
                        dir.Add("网站通用图标", new List<VItem>(tmp));
                        tmp.Clear();
                        return dir;
                    }
                    tmp.Add(new VItem(it.Key, it.Value));
                }
                dir.Add("网站通用图标", new List<VItem>(tmp));
                tmp.Clear();
            }
            else
            {
                bool isadd = false;
                foreach (var it in AntdUI.SvgDb.Custom)
                {
                    if (it.Key == "StepBackwardFilled") isadd = true;
                    else if (it.Key == "QuestionCircleFilled")
                    {
                        dir.Add("方向性图标", new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "EditFilled")
                    {
                        dir.Add("提示建议性图标", new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "PieChartFilled")
                    {
                        dir.Add("编辑类图标", new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AndroidFilled")
                    {
                        dir.Add("数据类图标", new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AccountBookFilled")
                    {
                        dir.Add("品牌和标识", new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    if (isadd) tmp.Add(new VItem(it.Key, it.Value));
                }
                dir.Add("网站通用图标", new List<VItem>(tmp));
                tmp.Clear();
            }
            return dir;
        }

        #endregion

        #region 渲染

        class TItem : AntdUI.VirtualItem
        {
            string title, count;
            public List<VItem> data;
            public TItem(string t, List<VItem> d)
            {
                CanClick = false;
                data = d;
                title = t;
                count = d.Count.ToString();
            }

            StringFormat s_f = AntdUI.Helper.SF_NoWrap(lr: StringAlignment.Near);
            StringFormat s_c = AntdUI.Helper.SF_NoWrap();
            public override void Paint(Graphics g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;

                using (var fore = new SolidBrush(AntdUI.Style.Db.Text))
                {
                    using (var font_title = new Font(e.Panel.Font, FontStyle.Bold))
                    using (var font_count = new Font(e.Panel.Font.FontFamily, e.Panel.Font.Size * .74F, e.Panel.Font.Style))
                    {
                        var size = AntdUI.Helper.Size(g.MeasureString(title, font_title));
                        AntdUI.CorrectionTextRendering.DrawStr(g, title, font_title, fore, e.Rect, s_f);

                        var rect_count = new Rectangle(e.Rect.X + size.Width, e.Rect.Y + (e.Rect.Height - size.Height) / 2, size.Height, size.Height);
                        using (var path = AntdUI.Helper.RoundPath(rect_count, e.Radius))
                        {
                            using (var brush = new SolidBrush(AntdUI.Style.Db.TagDefaultBg))
                            {
                                g.FillPath(brush, path);
                            }
                            using (var pen = new Pen(AntdUI.Style.Db.DefaultBorder, 1 * dpi))
                            {
                                g.DrawPath(pen, path);
                            }
                        }
                        AntdUI.CorrectionTextRendering.DrawStr(g, count, font_count, fore, rect_count, s_c);
                    }
                }
            }

            public override Size Size(Graphics g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = AntdUI.Config.Dpi;
                return new Size(e.Rect.Width, (int)(44 * dpi));
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
                    vpanel.Empty = false;
                }
                else
                {
                    vpanel.Empty = true;
                    string searchLower = search.ToLower();
                    var titles = new List<TItem>(vpanel.Items.Count);
                    foreach (var it in vpanel.Items)
                    {
                        if (it is VItem item) it.Visible = item.Key.ToLower().Contains(search);
                        else if (it is TItem itemTitle) titles.Add(itemTitle);
                    }
                    foreach (var it in titles)
                    {
                        int count = 0;
                        foreach (var item in it.data)
                        {
                            if (item.Visible) count++;
                        }
                        it.Visible = count > 0;
                    }
                }
                vpanel.PauseLayout = false;
            }));
        }

        #endregion
    }
}
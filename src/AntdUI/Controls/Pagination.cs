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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Pagination 分页
    /// </summary>
    /// <remarks>采用分页的形式分隔长列表，每次只加载一个页面。</remarks>
    [Description("Pagination 分页")]
    [ToolboxItem(true)]
    [DefaultProperty("Current")]
    [DefaultEvent("ValueChanged")]
    public class Pagination : IControl
    {
        #region 属性

        int current = 1;
        /// <summary>
        /// 当前页数
        /// </summary>
        [Description("当前页数"), Category("数据"), DefaultValue(1)]
        public int Current
        {
            get => current;
            set
            {
                if (value < 1) value = 1;
                else if (value > PageTotal) value = PageTotal;
                if (current == value) return;
                current = value;
                ValueChanged?.Invoke(current, total, pageSize, PageTotal);
                ButtonLayout();
                Invalidate();
            }
        }

        int total = 0;
        /// <summary>
        /// 数据总数
        /// </summary>
        [Description("数据总数"), Category("数据"), DefaultValue(0)]
        public int Total
        {
            get => total;
            set
            {
                if (total == value) return;
                total = value;
                ButtonLayout();
                Invalidate();
            }
        }

        int pageSize = 10;
        /// <summary>
        /// 每页条数
        /// </summary>
        [Description("每页条数"), Category("数据"), DefaultValue(10)]
        public int PageSize
        {
            get => pageSize;
            set
            {
                if (pageSize == value) return;
                pageSize = value;
                if (Math.Ceiling(total * 1.0 / pageSize) < current) current = (int)Math.Ceiling(total * 1.0 / pageSize);
                ValueChanged?.Invoke(current, total, pageSize, PageTotal);
                if (input_SizeChanger != null)
                {
                    string tips = Localization.Provider?.GetLocalizedString("ItemsPerPage") ?? "条/页";
                    input_SizeChanger.Clear();
                    input_SizeChanger.PlaceholderText = value.ToString() + " " + tips;
                }
                ButtonLayout();
                Invalidate();
            }
        }

        /// <summary>
        /// 最大显示总页数
        /// </summary>
        [Description("最大显示总页数"), Category("行为"), DefaultValue(0)]
        public int MaxPageTotal { get; set; } = 0;

        /// <summary>
        /// 总页数
        /// </summary>
        [Description("总页数"), Category("数据")]
        public int PageTotal { get; private set; } = 1;

        int _gap = 8;
        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(8)]
        public int Gap
        {
            get => _gap;
            set
            {
                if (_gap == value) return;
                _gap = value;
                ButtonLayout();
                Invalidate();
            }
        }

        /// <summary>
        /// Value 属性值更改时发生
        /// </summary>
        [Description("Value 属性值更改时发生"), Category("行为")]
        public event ValueEventHandler? ValueChanged;

        /// <summary>
        /// 显示数据总量
        /// </summary>
        /// <param name="current">当前页数</param>
        /// <param name="total">数据总数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageTotal">总页数</param>
        /// <returns></returns>
        public delegate void ValueEventHandler(int current, int total, int pageSize, int pageTotal);

        /// <summary>
        /// 显示数据总量
        /// </summary>
        /// <param name="current">当前页数</param>
        /// <param name="total">数据总数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageTotal">总页数</param>
        /// <returns></returns>
        public delegate string RtValueEventHandler(int current, int total, int pageSize, int pageTotal);

        /// <summary>
        /// 显示数据总量
        /// </summary>
        [Description("用于显示数据总量"), Category("行为")]
        public event RtValueEventHandler? ShowTotalChanged;

        bool showSizeChanger = false;
        /// <summary>
        /// 是否展示 PageSize 切换器
        /// </summary>
        [Description("是否展示 PageSize 切换器"), Category("行为"), DefaultValue(false)]
        public bool ShowSizeChanger
        {
            get => showSizeChanger;
            set
            {
                if (showSizeChanger == value) return;
                showSizeChanger = value;
                if (!value) InputSizeChangerDispose();
                ButtonLayout();
                Invalidate();
            }
        }

        int[]? pageSizeOptions = null;
        /// <summary>
        /// 指定每页可以显示多少条
        /// </summary>
        [Description("指定每页可以显示多少条"), Category("行为"), DefaultValue(null)]
        public int[]? PageSizeOptions
        {
            get => pageSizeOptions;
            set
            {
                if (pageSizeOptions == value) return;
                pageSizeOptions = value;
                InputSizeChangerDispose();
                if (showSizeChanger)
                {
                    ButtonLayout();
                    Invalidate();
                }
            }
        }

        int sizeChangerWidth = 0;
        [Description("SizeChanger 宽度"), Category("行为"), DefaultValue(0)]
        public int SizeChangerWidth
        {
            get => sizeChangerWidth;
            set
            {
                if (sizeChangerWidth == value) return;
                sizeChangerWidth = value;
                if (showSizeChanger)
                {
                    InputSizeChangerDispose();
                    ButtonLayout();
                    Invalidate();
                }
            }
        }

        int pyr = 0;
        public override Rectangle DisplayRectangle
        {
            get => ClientRectangle.PaddingRect(Padding, 0, 0, pyr, 0, borderWidth * Config.Dpi);
        }

        Color? fill;
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fill
        {
            get => fill;
            set
            {
                fill = value;
                if (input_SizeChanger != null) input_SizeChanger.BorderColor = value;
                Invalidate();
            }
        }

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
            }
        }

        float borderWidth = 1F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(1F)]
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                Invalidate();
            }
        }

        RightToLeft rightToLeft = RightToLeft.No;
        [Description("反向"), Category("外观"), DefaultValue(RightToLeft.No)]
        public override RightToLeft RightToLeft
        {
            get => rightToLeft;
            set
            {
                if (rightToLeft == value) return;
                rightToLeft = value;
                ButtonLayout();
                Invalidate();
            }
        }

        string? textdesc;
        [Description("主动显示内容"), Category("外观"), DefaultValue(null)]
        public string? TextDesc
        {
            get => textdesc;
            set
            {
                if (textdesc == value) return;
                textdesc = value;
                ButtonLayout();
                Invalidate();
            }
        }

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            if (buttons.Length < 2) return;
            var g = e.Graphics.High();
            float border = borderWidth * Config.Dpi, _radius = radius * Config.Dpi;
            if (Enabled)
            {
                Color fore = Style.Db.Text, color = fill ?? Style.Db.Primary;
                using (var brush_hover = new SolidBrush(Style.Db.FillSecondary))
                {
                    #region 渲染上下

                    var btn_previous = buttons[0];
                    if (btn_previous.hover)
                    {
                        using (var path_previous = btn_previous.rect.RoundPath(_radius))
                        {
                            g.FillPath(brush_hover, path_previous);
                        }
                    }
                    using (var pen_arrow = new Pen(btn_previous.enabled ? fore : Style.Db.TextQuaternary, border))
                    {
                        g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(btn_previous.rect));
                    }

                    var btn_next = buttons[1];

                    if (btn_next.hover)
                    {
                        using (var path_next = btn_next.rect.RoundPath(_radius))
                        {
                            g.FillPath(brush_hover, path_next);
                        }
                    }
                    using (var pen_arrow = new Pen(btn_next.enabled ? fore : Style.Db.TextQuaternary, border))
                    {
                        g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(btn_next.rect));
                    }

                    #endregion

                    using (var brush = new SolidBrush(fore))
                    {
                        if (showTotal != null) g.DrawString(showTotal, Font, brush, rect_text, Helper.stringFormatCenter2);
                        for (int i = 2; i < buttons.Length; i++)
                        {
                            var btn = buttons[i];
                            if (btn.hover)
                            {
                                using (var path = btn.rect.RoundPath(_radius))
                                {
                                    g.FillPath(brush_hover, path);
                                }
                            }
                            if (btn.prog > 0)
                            {
                                using (var brush_prog = new SolidBrush(Style.Db.TextQuaternary))
                                {
                                    g.DrawString("•••", Font, brush_prog, btn.rect, Helper.stringFormatCenter2);
                                }
                            }
                            else
                            {
                                if (current == btn.num)
                                {
                                    using (var path = btn.rect.RoundPath(_radius))
                                    {
                                        using (var pen = new Pen(color, border))
                                        {
                                            g.DrawPath(pen, path);
                                        }
                                    }
                                }
                                g.DrawString(btn.key, Font, brush, btn.rect, Helper.stringFormatCenter2);
                            }
                        }
                    }
                }
            }
            else
            {
                #region 渲染上下

                var btn_previous = buttons[0];
                using (var pen_arrow = new Pen(Style.Db.TextQuaternary, border))
                {
                    g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(btn_previous.rect));
                }

                var btn_next = buttons[1];
                using (var pen_arrow = new Pen(Style.Db.TextQuaternary, border))
                {
                    g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(btn_next.rect));
                }

                #endregion

                using (var brush = new SolidBrush(Style.Db.TextQuaternary))
                {
                    if (showTotal != null) g.DrawString(showTotal, Font, brush, rect_text, Helper.stringFormatCenter2);
                    for (int i = 2; i < buttons.Length; i++)
                    {
                        var btn = buttons[i];
                        if (btn.prog > 0)
                        {
                            g.DrawString("•••", Font, brush, btn.rect, Helper.stringFormatCenter2);
                        }
                        else
                        {
                            if (current == btn.num)
                            {
                                using (var path = btn.rect.RoundPath(_radius))
                                {
                                    using (var brush_2 = new SolidBrush(Style.Db.Fill))
                                    {
                                        g.FillPath(brush_2, path);
                                    }
                                }
                            }
                            g.DrawString(btn.key, Font, brush, btn.rect, Helper.stringFormatCenter2);
                        }
                    }
                }
            }
        }

        #endregion

        #region 鼠标

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (buttons.Length > 0)
            {
                int count = 0, count_no = 0, count_hand = 0;
                for (int i = 0; i < buttons.Length; i++)
                {
                    var btn = buttons[i];
                    var hover = false;
                    if (btn.enabled) hover = btn.rect.Contains(e.Location);
                    else if ((i == 0 || i == 1) && btn.rect.Contains(e.Location))
                    {
                        count_no++;
                    }
                    if (btn.hover != hover)
                    {
                        btn.hover = hover;
                        count++;
                    }
                    if (btn.hover) count_hand++;
                }
                if (count_no > 0) Cursor = Cursors.No;
                else SetCursor(count_hand > 0);
                if (count > 0) Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            SetCursor(false);
            if (buttons.Length > 0)
            {
                int count = 0;
                foreach (var btn in buttons)
                {
                    if (btn.hover)
                    {
                        btn.hover = false;
                        count++;
                    }
                }
                if (count > 0) Invalidate();
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (buttons.Length > 0)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    var btn = buttons[i];
                    if (btn.enabled && btn.rect.Contains(e.Location))
                    {
                        if (i == 0) Current = current - 1;
                        else if (i == 1) Current = current + 1;
                        else if (btn.prog > 0)
                        {
                            if (btn.prog == 2) Current = current - 5;
                            else Current = current + 5;
                        }
                        else Current = btn.num;
                        Focus();
                        return;
                    }
                }
            }
            base.OnMouseDown(e);
        }

        #endregion

        #region 布局

        protected override void OnSizeChanged(EventArgs e)
        {
            ButtonLayout();
            base.OnSizeChanged(e);
        }

        ButtonLoad[] buttons = new ButtonLoad[0];
        internal string? showTotal = null;
        internal Rectangle rect_text;

        Input? input_SizeChanger = null;
        void InputSizeChangerDispose()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(InputSizeChangerDispose));
                return;
            }
            input_SizeChanger?.Dispose();
            input_SizeChanger = null;
        }
        void ButtonLayout()
        {
            var rect = ClientRectangle.PaddingRect(Padding, borderWidth * Config.Dpi);
            if (showSizeChanger)
            {
                int wsize = (int)(4 * Config.Dpi);
                rect.Y += wsize;
                rect.Height -= wsize * 2;
            }
            bool sizeChanger = ShowSizeChanger;
            int t_Width = rect.Width, _SizeChangerWidth = 0;
            if (t_Width > 1)
            {
                if (sizeChanger)
                {
                    _SizeChangerWidth = InitSizeChanger(rect);
                    t_Width -= _SizeChangerWidth;
                }
                int gap = (int)(_gap * Config.Dpi);
                int total_button = t_Width / (rect.Height + gap);//总共多少按钮
                if (total_button < 3)
                {
                    buttons = new ButtonLoad[0];
                    return;
                }
                int total_page = (int)Math.Ceiling((total * 1.0) / pageSize);//总页数
                if (total_page == 0) total_page = 1;

                if (textdesc == null) showTotal = ShowTotalChanged?.Invoke(current, total, pageSize, total_page);
                else showTotal = textdesc;

                int pyrn = Helper.GDI(g =>
                {
                    var dir = new Dictionary<int, int>();
                    int min = 100, max_size = rect.Height;
                    for (int i = 0; i <= total_page; i++)
                    {
                        if (i == min)
                        {
                            min = min * 10;
                            var size_font = g.MeasureString((i + 1).ToString(), Font).Size();
                            if (size_font.Width > rect.Height)
                            {
                                max_size = size_font.Width;
                                dir.Add(i.ToString().Length, size_font.Width);
                            }
                        }
                    }
                    int x = 0;
                    if (showTotal != null)
                    {
                        var size_font = g.MeasureString(showTotal, Font);
                        x = (int)Math.Ceiling(size_font.Width);
                        rect_text = new Rectangle(rect.X, rect.Y, x, rect.Height);
                    }

                    total_button = (int)Math.Floor((t_Width - x) / ((max_size * 1.0) + gap));//总共多少按钮
                    if (total_button < 3)
                    {
                        buttons = new ButtonLoad[0];
                        return 0;
                    }

                    if (MaxPageTotal > 0 && total_button > MaxPageTotal + 2) total_button = MaxPageTotal + 2;

                    int total_page_button = total_button - 2;

                    PageTotal = total_page;
                    bool has_previous = current > 1, has_next = total_page > current;//是否还有下一页
                    var _buttons = new List<ButtonLoad>(total_button) {
                        new ButtonLoad(new Rectangle(x + rect.X, rect.Y, rect.Height, rect.Height),has_previous)
                     };
                    int n_x = _buttons[0].rect.Right + gap;
                    if (total_page > total_page_button)
                    {
                        //大于
                        int _tol = total_page_button;
                        int center_page_button = (int)Math.Ceiling(total_page_button / 2F);
                        if (current <= center_page_button)
                        {
                            for (int i = 0; i < _tol - 2; i++) n_x += AddButs(ref _buttons, dir, new ButtonLoad(i + 1, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true)) + gap;
                            n_x += AddButs(ref _buttons, dir, new ButtonLoad(_tol - 2, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true, 1)) + gap;
                            //添加最后一页
                            n_x += AddButs(ref _buttons, dir, new ButtonLoad(total_page, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true));
                        }
                        else if (current > total_page - center_page_button)
                        {
                            n_x += AddButs(ref _buttons, dir, new ButtonLoad(1, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true)) + gap;
                            int last_i = total_page - (_tol - 3);
                            n_x += AddButs(ref _buttons, dir, new ButtonLoad(last_i, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true, 2)) + gap;
                            for (int i = 0; i < _tol - 2; i++) n_x += AddButs(ref _buttons, dir, new ButtonLoad(last_i + i, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true)) + gap;
                        }
                        else
                        {
                            int last_i = total_page - (_tol - 3);

                            #region 前

                            n_x += AddButs(ref _buttons, dir, new ButtonLoad(1, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true)) + gap;
                            n_x += AddButs(ref _buttons, dir, new ButtonLoad(1, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true, 2)) + gap;

                            #endregion

                            int center_len = (_tol - 4);
                            var start = current - center_len / 2;
                            for (int i = 0; i < center_len; i++) n_x += AddButs(ref _buttons, dir, new ButtonLoad(start + i, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true)) + gap;

                            #region 后

                            n_x += AddButs(ref _buttons, dir, new ButtonLoad(last_i, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true, 1)) + gap;
                            n_x += AddButs(ref _buttons, dir, new ButtonLoad(total_page, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true));

                            #endregion
                        }
                    }
                    else
                    {
                        //不够
                        for (int i = 0; i < total_page; i++) n_x += AddButs(ref _buttons, dir, new ButtonLoad(i + 1, new Rectangle(n_x, rect.Y, rect.Height, rect.Height), true)) + gap;
                    }
                    _buttons.Insert(1, new ButtonLoad(new Rectangle(_buttons[_buttons.Count - 1].rect.Right + gap, rect.Y, rect.Height, rect.Height), has_next));
                    int valr = 0;
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        var py = rect.Right - _buttons[1].rect.Right;
                        if (sizeChanger) py -= _SizeChangerWidth;
                        foreach (var btn in _buttons) btn._rect.Offset(py, 0);
                        rect_text.Offset(py, 0);
                    }
                    else if (sizeChanger) valr = rect.Right - _buttons[1].rect.Right - _SizeChangerWidth;
                    buttons = _buttons.ToArray();
                    return valr;
                });
                if (pyr == pyrn) return;
                pyr = pyrn;
                OnSizeChanged(EventArgs.Empty);
            }
        }
        int InitSizeChanger(Rectangle rect)
        {
            if (input_SizeChanger == null)
            {
                string tips = Localization.Provider?.GetLocalizedString("ItemsPerPage") ?? "条/页";
                var placeholder = pageSize.ToString() + " " + tips;
                bool r = rightToLeft == RightToLeft.Yes;
                int width = GetSizeChangerWidth(placeholder);
                if (pageSizeOptions == null || pageSizeOptions.Length == 0)
                {
                    var input = new Input
                    {
                        PlaceholderText = placeholder,
                        Size = new Size(width, rect.Height),
                        Dock = DockStyle.Right,
                        Font = Font,
                        BorderColor = fill
                    };
                    input_SizeChanger = input;
                }
                else
                {
                    var input = new Select
                    {
                        PlaceholderText = placeholder,
                        ListAutoWidth = true,
                        DropDownArrow = true,
                        Placement = TAlignFrom.Top,
                        Size = new Size(width, rect.Height),
                        Dock = DockStyle.Right,
                        Font = Font,
                        BorderColor = fill
                    };
                    foreach (var it in pageSizeOptions) input.Items.Add(it);
                    input.SelectedValue = pageSize;
                    input.Text = "";
                    input.SelectedValueChanged += (a, b) =>
                    {
                        if (b is int pageSize) PageSize = pageSize;
                    };
                    input_SizeChanger = input;
                }
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        Controls.Add(input_SizeChanger);
                    }));
                }
                else Controls.Add(input_SizeChanger);
                input_SizeChanger.KeyPress += Input_SizeChanger_KeyPress;
                return width;
            }
            else
            {
                if (sizeChangerWidth <= 0)
                {
                    string tips = Localization.Provider?.GetLocalizedString("ItemsPerPage") ?? "条/页";
                    var placeholder = pageSize.ToString() + " " + tips;
                    int width = GetSizeChangerWidth(placeholder);
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            input_SizeChanger.Width = width;
                        }));
                    }
                    else input_SizeChanger.Width = width;
                    return width;
                }
                return input_SizeChanger.Width;
            }
        }

        int GetSizeChangerWidth(string placeholder)
        {
            if (sizeChangerWidth > 0) return (int)(sizeChangerWidth * Config.Dpi);
            int wsize = (int)(4 * Config.Dpi) * 2;
            if (pageSizeOptions == null || pageSizeOptions.Length == 0)
            {
                return Helper.GDI(g =>
                {
                    var size = g.MeasureString(placeholder, Font).Size();
                    return size.Width + wsize + (int)Math.Ceiling(size.Height * 0.6F);
                });
            }
            else
            {
                return Helper.GDI(g =>
                {
                    var size = g.MeasureString(placeholder, Font).Size();
                    return size.Width + wsize + (int)Math.Ceiling(size.Height * 1.32F);
                });
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            if (input_SizeChanger != null) input_SizeChanger.Font = Font;
        }

        int AddButs(ref List<ButtonLoad> buttons, Dictionary<int, int> dir, ButtonLoad button)
        {
            if (button.key.Length > 1)
            {
                if (dir.TryGetValue(button.key.Length, out var it))
                {
                    button._rect.Width = it;
                }
            }
            buttons.Add(button);
            return button.rect.Width;
        }
        private void Input_SizeChanger_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && sender is Input input)
            {
                e.Handled = true;
                if (int.TryParse(input.Text, out var num)) PageSize = num;
            }
        }

        class ButtonLoad
        {
            public ButtonLoad(Rectangle _rect, bool enable)
            {
                rect = _rect;
                key = "";
                enabled = enable;
            }

            public ButtonLoad(int _num, Rectangle _rect, bool enable)
            {
                num = _num;
                rect = _rect;
                key = _num.ToString();
                enabled = enable;
            }
            public ButtonLoad(int _num, Rectangle _rect, bool enable, int p)
            {
                num = _num;
                rect = _rect;
                key = _num.ToString();
                enabled = enable;
                prog = p;
            }
            internal Rectangle _rect;
            public Rectangle rect { get => _rect; set => _rect = value; }

            public string key { get; set; }
            public int num { get; set; }
            public bool enabled { get; set; }
            public int prog { get; set; }
            public bool hover { get; set; }
        }

        #endregion

        public bool ProcessCmdKey(Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                var val = current - 1;
                if (val < 1 || val > PageTotal) return false;
                Current = val;
                return true;
            }
            else if (keyData == Keys.Right)
            {
                var val = current + 1;
                if (val < 1 || val > PageTotal) return false;
                Current = val;
                return true;
            }
            return false;
        }
    }
}
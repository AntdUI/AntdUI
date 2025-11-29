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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AntdUI
{
    /// <summary>
    /// Collapse 折叠面板
    /// </summary>
    /// <remarks>可以折叠/展开的内容区域。</remarks>
    [Description("Collapse 折叠面板")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    [DefaultEvent("ExpandChanged")]
    [Designer(typeof(CollapseDesigner))]
    public class Collapse : IControl, ICollapse
    {
        #region 属性

        /// <summary>
        /// 自动大小
        /// </summary>
        [Browsable(true)]
        [Description("自动大小"), Category("外观"), DefaultValue(false)]
        public override bool AutoSize
        {
            get => base.AutoSize;
            set
            {
                if (base.AutoSize == value) return;
                base.AutoSize = value;
                LoadLayout(false);
            }
        }

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
            }
        }

        Color? foreActive;
        /// <summary>
        /// 文字激活颜色
        /// </summary>
        [Description("文字激活颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ForeActive
        {
            get => foreActive;
            set
            {
                if (foreActive == value) return;
                foreActive = value;
            }
        }

        Color? headerBg;
        /// <summary>
        /// 折叠面板头部背景
        /// </summary>
        [Description("折叠面板头部背景"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? HeaderBg
        {
            get => headerBg;
            set
            {
                if (headerBg == value) return;
                headerBg = value;
                Invalidate();
            }
        }

        Size headerPadding { get; set; } = new Size(16, 12);
        /// <summary>
        /// 折叠面板头部内边距
        /// </summary>
        [Description("折叠面板头部内边距"), Category("外观"), DefaultValue(typeof(Size), "16, 12")]
        public Size HeaderPadding
        {
            get => headerPadding;
            set
            {
                if (headerPadding == value) return;
                headerPadding = value;
                LoadLayout();
            }
        }

        Size contentPadding { get; set; } = new Size(16, 16);
        /// <summary>
        /// 折叠面板内容内边距
        /// </summary>
        [Description("折叠面板内容内边距"), Category("外观"), DefaultValue(typeof(Size), "16, 16")]
        public Size ContentPadding
        {
            get => contentPadding;
            set
            {
                if (contentPadding == value) return;
                contentPadding = value;
                LoadLayout();
            }
        }

        #region 边框

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

        Color? borderColor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                Invalidate();
            }
        }

        #endregion

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

        int _gap = 0;
        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(0)]
        public int Gap
        {
            get => _gap;
            set
            {
                if (_gap == value) return;
                _gap = value;
                LoadLayout();
            }
        }

        /// <summary>
        /// 只保持一个展开
        /// </summary>
        [Description("只保持一个展开"), Category("外观"), DefaultValue(false)]
        public bool Unique { get; set; }

        /// <summary>
        /// 一个展开铺满
        /// </summary>
        [Description("一个展开铺满"), Category("外观"), DefaultValue(false)]
        public bool UniqueFull { get; set; }

        /// <summary>
        /// 展开/折叠的动画速度
        /// </summary>
        [Description("展开/折叠的动画速度"), Category("行为"), DefaultValue(100)]
        public int AnimationSpeed { get; set; } = 100;

        public override Rectangle DisplayRectangle => ClientRectangle.PaddingRect(Margin, Padding);

        #region 数据

        CollapseItemCollection? items;
        /// <summary>
        /// 获取列表中所有列表项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据"), DefaultValue(null)]
        public CollapseItemCollection Items
        {
            get
            {
                items ??= new CollapseItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ControlCollection Controls => base.Controls;

        #endregion

        /// <summary>
        /// 超出文字提示配置
        /// </summary>
        [Browsable(false)]
        [Description("超出文字提示配置"), Category("行为"), DefaultValue(null)]
        public TooltipConfig? TooltipConfig { get; set; }

        Font? fontExpand;
        /// <summary>
        /// 展开后的标题字体
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("展开后的标题字体"), Category("外观"), DefaultValue(null)]
        public Font? FontExpand
        {
            get => fontExpand;
            set
            {
                if (fontExpand == value) return;
                fontExpand = value;
                Invalidate();
            }
        }

        #endregion

        #region 布局

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadLayout(false);
        }

        internal bool canset = true;
        protected override void OnSizeChanged(EventArgs e)
        {
            if (canset) LoadLayout(false);
            base.OnSizeChanged(e);
        }

        internal void UniqueOne(CollapseItem item)
        {
            if (Unique)
            {
                if (items == null) return;
                foreach (var it in items)
                {
                    if (it == item) continue;
                    it.Expand = false;
                }
            }
        }

        public void LoadLayout(bool r = true)
        {
            if (IsHandleCreated)
            {
                if (items == null) return;
                var rect = ClientRectangle;
                if (rect.Width > 0 && rect.Height > 0)
                {
                    var rect_t = rect.DeflateRect(Margin);
                    LoadLayout(rect_t, items);
                    if (r) Invalidate(rect_t);
                }
            }
        }

        internal void LoadLayout(Rectangle rect, CollapseItemCollection items)
        {
            Helper.GDI(g =>
            {
                var size = g.MeasureString(Config.NullText, Font);
                int gap = (int)(_gap * Config.Dpi), gap_x = (int)(HeaderPadding.Width * Config.Dpi), gap_y = (int)(HeaderPadding.Height * Config.Dpi),
                    content_x = (int)(ContentPadding.Width * Config.Dpi), content_y = (int)(ContentPadding.Height * Config.Dpi), use_y = 0;
                int title_height = size.Height + gap_y * 2;
                int full_count = 0, useh = 0, full_h = 0;
                if (Unique && UniqueFull)
                {
                    foreach (var it in items)
                    {
                        if (it.Expand) full_count++;
                        else
                        {
                            useh += title_height + gap;
                            if (it.ExpandThread) useh += (int)((content_y * 2 + it.Height) * it.ExpandProg);
                            else if (it.Expand) useh += content_y * 2 + it.Height;
                        }
                    }
                    if (full_count > 0)
                    {
                        full_h = rect.Height - useh;
                        foreach (var it in items)
                        {
                            int y = rect.Y + use_y;
                            use_y += LoadLayout(g, it, rect, size, title_height, gap, gap_x, gap_y, content_x, content_y, full_h, it.Expand, y);
                        }
                    }
                    else
                    {
                        foreach (var it in items)
                        {
                            int y = rect.Y + use_y;
                            use_y += LoadLayout(g, it, rect, size, title_height, gap, gap_x, gap_y, content_x, content_y, full_h, it.Full, y);
                        }
                    }
                }
                else
                {
                    foreach (var it in items)
                    {
                        if (it.Full) full_count++;
                    }
                    if (full_count > 0)
                    {
                        foreach (var it in items)
                        {
                            if (!it.Full)
                            {
                                useh += title_height + gap;
                                if (it.ExpandThread) useh += (int)((content_y * 2 + it.Height) * it.ExpandProg);
                                else if (it.Expand) useh += content_y * 2 + it.Height;
                            }
                        }
                        full_h = (rect.Height - useh) / full_count;
                    }
                    foreach (var it in items)
                    {
                        int y = rect.Y + use_y;
                        use_y += LoadLayout(g, it, rect, size, title_height, gap, gap_x, gap_y, content_x, content_y, full_h, it.Full, y);
                    }
                }

                if (AutoSize)
                {
                    int rh = use_y + Margin.Vertical;
                    if (InvokeRequired) BeginInvoke(() => Height = rh);
                    else Height = rh;
                }
            });
        }
        int LoadLayout(Canvas g, CollapseItem it, Rectangle rect, Size size, int title_height, int gap, int gap_x, int gap_y, int content_x, int content_y, int full_h, bool full, int y)
        {
            Rectangle rectButtons = it.RectTitle = new Rectangle(rect.X, y, rect.Width, title_height);
            it.RectArrow = new Rectangle(rect.X + gap_x, y + gap_y, size.Height, size.Height);
            it.RectText = new Rectangle(rect.X + gap_x + size.Height + gap_y / 2, y + gap_y, rect.Width - (gap_x * 2 - size.Height - gap_y / 2), size.Height);
            OnExpandingChanged(it, it.Expand, rectButtons.Location);
            if (it.buttons != null && it.buttons.Count > 0)
            {
                int bx = rectButtons.Right;
                int gapW = (int)(4 * Config.Dpi);
                foreach (var btn in it.buttons)
                {
                    btn.PARENT = this;
                    btn.PARENTITEM = btn.ParentItem = it;
                    int height = rectButtons.Height - (btn.SwitchMode ? 12 : gapW);
                    int space = (rectButtons.Height - height) / 2;
                    bx -= space;
                    int? width = btn.Width;
                    if (width == null)
                    {
                        bool emptyIcon = string.IsNullOrEmpty(btn.IconSvg) && btn.Icon == null;
                        string? text = btn.SwitchMode ? (btn.Checked ? btn.CheckedText : btn.UnCheckedText) : btn.Text;

                        var size_btn = string.IsNullOrEmpty(text) ? new Size(0, 0) : g.MeasureString(text, Font);
                        width = btn.SwitchMode ? size_btn.Width * ((int)(3 * Config.Dpi)) : (emptyIcon ? gapW : height) + (size_btn.Width > 0 ? size_btn.Width + gapW : 0);
                    }

                    if (width < height) width = btn.SwitchMode ? height * 4 : height;
                    Rectangle rectItem = new Rectangle(bx - width.Value, rectButtons.Top + ((rectButtons.Height - height) / 2), width.Value, height);
                    btn.SetRect(g, rectItem, Font.Height, 0, GetIconSize(rectItem.Height));
                    bx -= (width.Value + space);
                }
            }
            Rectangle Rect;
            if (full)
            {
                if (it.ExpandThread) it.Rect = Rect = new Rectangle(rect.X, y, rect.Width, title_height + (int)((full_h - title_height) * it.ExpandProg));
                else if (it.Expand)
                {
                    it.Rect = Rect = new Rectangle(rect.X, y, rect.Width, full_h);
                    it.RectControl = new Rectangle(rect.X + content_x, y + title_height + content_y, rect.Width - content_x * 2, full_h - (title_height + content_y * 2));
                    it.SetSize();
                }
                else it.Rect = Rect = it.RectTitle;
            }
            else
            {
                if (it.ExpandThread) it.Rect = Rect = new Rectangle(rect.X, y, rect.Width, title_height + (int)((content_y * 2 + it.Height) * it.ExpandProg));
                else if (it.Expand)
                {
                    it.RectControl = new Rectangle(rect.X + content_x, y + title_height + content_y, rect.Width - content_x * 2, it.Height);
                    it.Rect = Rect = new Rectangle(rect.X, y, rect.Width, title_height + content_y * 2 + it.Height);
                    it.SetSize();
                }
                else it.Rect = Rect = it.RectTitle;
            }
            return Rect.Height + gap;
        }

        readonly FormatFlags s_c = FormatFlags.Top | FormatFlags.HorizontalCenter;

        #endregion

        #region 事件

        /// <summary>
        /// Expand 属性值更改时发生
        /// </summary>
        [Description("Expand 属性值更改时发生"), Category("行为")]
        public event CollapseExpandEventHandler? ExpandChanged;

        /// <summary>
        /// Expanding 属性值更改时发生
        /// </summary>
        [Description("Expanding 属性值更改时发生"), Category("行为")]
        public event CollapseExpandingEventHandler? ExpandingChanged;

        [Obsolete("请使用ButtonClick")]
        /// <summary>
        /// CollapseItem上的按件单击时发生
        /// </summary>
        [Description("CollapseItem上的按件单击时发生"), Category("行为")]
        public event CollapseButtonClickEventHandler? ButtonClickChanged;

        /// <summary>
        /// CollapseItem上的按件单击时发生
        /// </summary>
        [Description("CollapseItem上的按件单击时发生"), Category("行为")]
        public event CollapseButtonClickEventHandler? ButtonClick;

        internal void OnExpandChanged2(CollapseItem value, bool expand) => OnExpandChanged(value, expand);
        protected virtual void OnExpandChanged(CollapseItem value, bool expand) => ExpandChanged?.Invoke(this, new CollapseExpandEventArgs(value, expand, value.RectTitle, value.RectControl));

        protected virtual void OnExpandingChanged(CollapseItem value, bool expand, Point location) => ExpandingChanged?.Invoke(this, new CollapseExpandingEventArgs(value, expand, value.RectTitle, value.RectControl, location));

        protected virtual void OnButtonClick(CollapseItem value, CollapseGroupButton button) => (ButtonClick ?? ButtonClickChanged)?.Invoke(this, new CollapseButtonClickEventArgs(button, value));

        #endregion

        #region 渲染

        FormatFlags s_l = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis;
        protected override void OnDraw(DrawEventArgs e)
        {
            if (items == null || items.Count == 0)
            {
                base.OnDraw(e);
                return;
            }
            var g = e.Canvas;
            float r = radius * Config.Dpi;
            using (var forebrush = new SolidBrush(fore ?? Colour.Text.Get(nameof(Collapse), ColorScheme)))
            using (var brush = new SolidBrush(headerBg ?? Colour.FillQuaternary.Get(nameof(Collapse), ColorScheme)))
            {
                if (borderWidth > 0)
                {
                    using (var pen = new Pen(borderColor ?? Colour.BorderColor.Get(nameof(Collapse), ColorScheme), borderWidth * Config.Dpi))
                    using (var pen_arr = new Pen(forebrush.Color, 1.2F * Config.Dpi))
                    {
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        if (items.Count == 1 || _gap > 0)
                        {
                            foreach (var item in items)
                            {
                                if (item.Expand)
                                {
                                    using (var path = item.Rect.RoundPath(r))
                                    {
                                        g.Draw(pen, path);
                                    }
                                    using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                    {
                                        g.Fill(brush, path);
                                        g.Draw(pen, path);
                                    }
                                }
                                else
                                {
                                    using (var path = item.RectTitle.RoundPath(r))
                                    {
                                        g.Fill(brush, path);
                                        g.Draw(pen, path);
                                    }
                                }
                                PaintItem(g, item, forebrush, pen_arr);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                var item = items[i];
                                if (i == 0)
                                {
                                    if (item.Expand)
                                    {
                                        using (var path = item.Rect.RoundPath(r, true, true, false, false))
                                        {
                                            g.Draw(pen, path);
                                        }
                                        using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                        {
                                            g.Fill(brush, path);
                                            g.Draw(pen, path);
                                        }
                                    }
                                    else
                                    {
                                        using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                        {
                                            g.Fill(brush, path);
                                            g.Draw(pen, path);
                                        }
                                    }
                                    PaintItem(g, item, forebrush, pen_arr);
                                }
                                else if (i == items.Count - 1)
                                {
                                    if (item.Expand)
                                    {
                                        using (var path = item.Rect.RoundPath(r, false, false, true, true))
                                        {
                                            g.Draw(pen, path);
                                        }
                                        g.Fill(brush, item.RectTitle);
                                        g.Draw(pen, item.RectTitle);
                                    }
                                    else
                                    {
                                        using (var path = item.RectTitle.RoundPath(r, false, false, true, true))
                                        {
                                            g.Fill(brush, path);
                                            g.Draw(pen, path);
                                        }
                                    }
                                    PaintItem(g, item, forebrush, pen_arr);
                                }
                                else
                                {
                                    if (item.Expand)
                                    {
                                        g.Draw(pen, item.Rect);
                                        g.Fill(brush, item.RectTitle);
                                        g.Draw(pen, item.RectTitle);
                                    }
                                    else
                                    {
                                        using (var path = item.RectTitle.RoundPath(r, false, false, true, true))
                                        {
                                            g.Fill(brush, item.RectTitle);
                                            g.Draw(pen, item.RectTitle);
                                        }
                                    }
                                    PaintItem(g, item, forebrush, pen_arr);
                                }

                            }
                        }
                    }
                }
                else
                {
                    if (items.Count == 1 || _gap > 0)
                    {
                        foreach (var item in items)
                        {
                            if (item.Expand)
                            {
                                using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else
                            {
                                using (var path = item.RectTitle.RoundPath(r))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            PaintItem(g, item, forebrush);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            var item = items[i];
                            if (i == 0)
                            {
                                if (item.Expand)
                                {
                                    using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                    {
                                        g.Fill(brush, path);
                                    }
                                }
                                else
                                {
                                    using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                    {
                                        g.Fill(brush, path);
                                    }
                                }
                                PaintItem(g, item, forebrush);
                            }
                            else if (i == items.Count - 1)
                            {
                                if (item.Expand) g.Fill(brush, item.RectTitle);
                                else
                                {
                                    using (var path = item.RectTitle.RoundPath(r, false, false, true, true))
                                    {
                                        g.Fill(brush, path);
                                    }
                                }
                                PaintItem(g, item, forebrush);
                            }
                            else
                            {
                                if (item.Expand) g.Fill(brush, item.RectTitle);
                                else
                                {
                                    using (var path = item.RectTitle.RoundPath(r, false, false, true, true))
                                    {
                                        g.Fill(brush, item.RectTitle);
                                    }
                                }
                                PaintItem(g, item, forebrush);
                            }
                        }
                    }
                }
            }
            base.OnDraw(e);
        }
        private int GetIconSize(int titleHeight) { return titleHeight - (int)(8 * Config.Dpi); }

        void PaintItemIconText(Canvas g, CollapseItem item, SolidBrush fore)
        {
            Rectangle rect = item.RectText;
            if (item.HasIcon)
            {
                int height = rect.Height * 2;
                int iconSize = GetIconSize(height);
                int gap = (int)(6 * Config.Dpi);
                Rectangle ico_rect = new Rectangle(rect.X, rect.Y - gap, iconSize, iconSize);

                if (item.Icon != null) g.Image(item.Icon, ico_rect);
                else if (item.IconSvg != null) g.GetImgExtend(item.IconSvg, ico_rect, fore.Color);
                rect.X += iconSize + gap;
            }
            Font fnt = Font;
            if (item.Expand && FontExpand != null)
            {
                fnt = FontExpand;
                int fs = fnt.Size > Font.Size ? (int)(fnt.Size - Font.Size) : 0;
                rect.Inflate(fs, fs);
            }
            if (item.Expand && foreActive.HasValue) g.String(item.Text, fnt, foreActive.Value, rect, s_l);
            else g.String(item.Text, fnt, fore, rect, s_l);
        }
        void PaintItem(Canvas g, CollapseItem item, SolidBrush fore, Pen pen_arr)
        {
            if (item.ExpandThread) PaintArrow(g, item, pen_arr, -90 + (90F * item.ExpandProg));
            else if (item.Expand) g.DrawLines(pen_arr, item.RectArrow.TriangleLines(-1, .56F));
            else PaintArrow(g, item, pen_arr, -90F);

            //g.String(item.Text, Font, fore, item.RectText, s_l);
            PaintItemIconText(g, item, fore);
            PaintButtons(g, item, fore);
        }

        void PaintItem(Canvas g, CollapseItem item, SolidBrush fore)
        {
            if (item.ExpandThread) PaintArrow(g, item, fore, -90 + (90F * item.ExpandProg));
            else if (item.Expand) g.FillPolygon(fore, item.RectArrow.TriangleLines(-1, .56F));
            else PaintArrow(g, item, fore, -90F);

            PaintItemIconText(g, item, fore);

            PaintButtons(g, item, fore);
        }

        internal void PaintClick(Canvas g, GraphicsPath path, Rectangle rect, RectangleF rect_read, Color color, CollapseGroupButton btn)
        {
            if (btn.AnimationClick || true)
            {
                float alpha = 100 * (1F - btn.AnimationClickValue),
                    maxw = rect_read.Width + ((rect.Width - rect_read.Width) * btn.AnimationClickValue), maxh = rect_read.Height + ((rect.Height - rect_read.Height) * btn.AnimationClickValue);
                using (var path_click = new RectangleF(rect.X + (rect.Width - maxw) / 2F, rect.Y + (rect.Height - maxh) / 2F, maxw, maxh).RoundPath(maxh))
                {
                    path_click.AddPath(path, false);
                    g.Fill(Helper.ToColor(alpha, color), path_click);
                }
            }
        }
        void PaintButtons(Canvas g, CollapseItem item, SolidBrush fore)
        {
            if (item.buttons == null) return;
            using (var fore_active = new SolidBrush(Colour.Primary.Get(nameof(Button), ColorScheme)))
            using (var hover = new SolidBrush(Colour.FillSecondary.Get(nameof(Button), ColorScheme)))
            using (var brush_TextQuaternary = new SolidBrush(Colour.TextQuaternary.Get(nameof(Button), "foreDisabled", ColorScheme)))
            using (var active = new SolidBrush(Colour.PrimaryBg.Get(nameof(Button), ColorScheme)))
            {
                foreach (var btn in item.buttons)
                {
                    if (btn.Show && btn.Visible)
                    {
                        if (btn.EditType != EButtonEditTypes.Switch)
                        {
                            if (btn.EditType == EButtonEditTypes.Input || btn.EditType == EButtonEditTypes.Custom) continue;

                            if (btn.Enabled)
                            {
                                if (btn.Select || btn.AnimationClick) CollapseGroup.PaintBack(g, btn, active, radius);
                                if (btn.AnimationHover)
                                {
                                    using (var brush = new SolidBrush(Helper.ToColorN(btn.AnimationHoverValue, hover.Color)))
                                    {
                                        CollapseGroup.PaintBack(g, btn, brush, radius);
                                    }
                                }
                                else if (btn.Hover) CollapseGroup.PaintBack(g, btn, hover, radius);
                                else if (btn.AnimationClick)
                                {
                                    var rect_read = btn.rect;
                                    using (var path = rect_read.RoundPath(rect_read.Height))
                                    {
                                        Color _color = btn.Back ?? active.Color;
                                        PaintClick(g, path, rect_read, rect_read, _color, btn);
                                    }
                                }
                                if (btn.Icon != null) g.Image(btn.Icon, btn.ico_rect);

                                if (btn.IconSvg != null) g.GetImgExtend(btn.IconSvg, btn.ico_rect, btn.Select || btn.AnimationClick ? fore_active.Color : btn.Fore ?? fore.Color);
                                g.String(btn.Text, Font, btn.Select || btn.AnimationClick ? fore_active : fore, btn.txt_rect, s_c);
                            }
                            else
                            {
                                if (btn.Icon != null) g.Image(btn.Icon, btn.ico_rect);
                                if (btn.IconSvg != null) g.GetImgExtend(btn.IconSvg, btn.ico_rect, brush_TextQuaternary.Color);
                                g.String(btn.Text, Font, brush_TextQuaternary, btn.txt_rect, s_c);
                            }
                        }
                        else
                        {
                            var rect_read = btn.rect;
                            bool enabled = btn.Enabled;
                            using (var path = rect_read.RoundPath(rect_read.Height))
                            {
                                Color _color = btn.Back ?? Colour.Primary.Get(nameof(Switch), ColorScheme);
                                PaintClick(g, path, rect_read, rect_read, _color, btn);
                                if (enabled && (btn.hasFocus && Config.FocusBorderEnabled) && btn.WaveSize > 0)
                                {
                                    float wave = (btn.WaveSize * Config.Dpi / 2), wave2 = wave * 2;
                                    using (var path_focus = new RectangleF(rect_read.X - wave, rect_read.Y - wave, rect_read.Width + wave2, rect_read.Height + wave2).RoundPath(0, TShape.Round))
                                    {
                                        g.Draw(Colour.PrimaryBorder.Get(nameof(Switch), ColorScheme), wave, path_focus);
                                    }
                                }
                                using (var brush = new SolidBrush(Colour.TextQuaternary.Get(nameof(Switch), ColorScheme)))
                                {
                                    g.Fill(brush, path);
                                    if (btn.AnimationHover) g.Fill(Helper.ToColorN(btn.AnimationHoverValue, brush.Color), path);
                                    else if (btn.ExtraMouseHover) g.Fill(brush, path);
                                }
                                int gap = (int)(3 * Config.Dpi), gap2 = gap * 2;
                                if (btn.AnimationCheck)
                                {
                                    var alpha = 255 * btn.AnimationCheckValue;
                                    g.Fill(Helper.ToColor(alpha, _color), path);
                                    var dot_rect = new RectangleF(rect_read.X + gap + (rect_read.Width - rect_read.Height) * btn.AnimationCheckValue, rect_read.Y + gap, rect_read.Height - gap2, rect_read.Height - gap2);
                                    g.FillEllipse(enabled ? Colour.BgBase.Get(nameof(Switch), ColorScheme) : Color.FromArgb(200, Colour.BgBase.Get(nameof(Switch), ColorScheme)), dot_rect);

                                }
                                else if (btn.Checked)
                                {
                                    var colorhover = Colour.PrimaryHover.Get(nameof(Switch), ColorScheme);
                                    g.Fill(enabled ? _color : Color.FromArgb(200, _color), path);
                                    if (btn.AnimationHover) g.Fill(Helper.ToColorN(btn.AnimationHoverValue, colorhover), path);
                                    else if (btn.ExtraMouseHover) g.Fill(colorhover, path);
                                    var dot_rect = new RectangleF(rect_read.X + gap + rect_read.Width - rect_read.Height, rect_read.Y + gap, rect_read.Height - gap2, rect_read.Height - gap2);
                                    g.FillEllipse(enabled ? Colour.BgBase.Get(nameof(Switch), ColorScheme) : Color.FromArgb(200, Colour.BgBase.Get(nameof(Switch), ColorScheme)), dot_rect);
                                }
                                else
                                {
                                    var dot_rect = new RectangleF(rect_read.X + gap, rect_read.Y + gap, rect_read.Height - gap2, rect_read.Height - gap2);
                                    g.FillEllipse(enabled ? Colour.BgBase.Get(nameof(Switch), ColorScheme) : Color.FromArgb(200, Colour.BgBase.Get(nameof(Switch), ColorScheme)), dot_rect);
                                }

                                // 绘制文本
                                string? textToRender = btn.Checked ? btn.CheckedText : btn.UnCheckedText;
                                if (textToRender != null)
                                {
                                    Color _fore = btn.Fore ?? Colour.PrimaryColor.Get(nameof(Switch), ColorScheme);
                                    using (var brush = new SolidBrush(_fore))
                                    {
                                        var textSize = g.MeasureString(textToRender, Font);
                                        var textRect = btn.Checked
                                            ? new Rectangle(rect_read.X + (rect_read.Width - rect_read.Height + gap2) / 2 - textSize.Width / 2, rect_read.Y + rect_read.Height / 2 - textSize.Height / 2, textSize.Width, textSize.Height)
                                            : new Rectangle(rect_read.X + (rect_read.Height - gap + (rect_read.Width - rect_read.Height + gap) / 2 - textSize.Width / 2), rect_read.Y + rect_read.Height / 2 - textSize.Height / 2, textSize.Width, textSize.Height);
                                        g.String(textToRender, Font, brush, textRect);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        void PaintArrow(Canvas g, CollapseItem item, Pen pen, float rotate)
        {
            var rect_arr = item.RectArrow;
            int size_arrow = rect_arr.Width / 2;
            g.TranslateTransform(rect_arr.X + size_arrow, rect_arr.Y + size_arrow);
            g.RotateTransform(rotate);
            g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, rect_arr.Width, rect_arr.Height).TriangleLines(-1, .56F));
            g.ResetTransform();
        }
        void PaintArrow(Canvas g, CollapseItem item, SolidBrush brush, float rotate)
        {
            var rect_arr = item.RectArrow;
            int size_arrow = rect_arr.Width / 2;
            g.TranslateTransform(rect_arr.X + size_arrow, rect_arr.Y + size_arrow);
            g.RotateTransform(rotate);
            g.FillPolygon(brush, new Rectangle(-size_arrow, -size_arrow, rect_arr.Width, rect_arr.Height).TriangleLines(-1, .56F));
            g.ResetTransform();
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            foreach (var item in items)
            {
                if (item.Contains(e.X, e.Y))
                {
                    item.MDown = true;
                    return;
                }
                if (item.buttons == null) continue;
                foreach (var btn in item.buttons)
                {
                    if (!btn.Visible || !btn.Enabled) continue;
                    if (btn.Contains(e.X, e.Y))
                    {
                        item.MDown = true;
                        btn.AnimationClick = true;
                        Invalidate(btn.rect);
                        return;
                    }
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            foreach (var item in items)
            {
                if (item.MDown)
                {
                    if (item.Contains(e.X, e.Y)) item.Expand = !item.Expand;
                    else
                    {
                        if (item.buttons == null) continue;
                        foreach (var btn in item.buttons)
                        {
                            if (!btn.Visible || !btn.Enabled) continue;
                            if (btn.Contains(e.X, e.Y))
                            {
                                if (btn.SwitchMode)
                                {
                                    btn.Checked = !btn.Checked;
                                    OnButtonClick(item, btn);
                                    Invalidate(btn.rect);
                                    item.MDown = false;
                                    return;
                                }
                                btn.AnimationClick = false;
                                if (btn.EditType != EButtonEditTypes.Button) btn.Select = true;

                                OnButtonClick(item, btn);

                                item.MDown = false;
                                return;
                            }
                        }
                    }
                    item.MDown = false;
                    break;
                }
            }
            base.OnMouseUp(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            CloseTip();

        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (items == null || items.Count == 0) return;
            foreach (var item in items)
            {
                if (item.Contains(e.X, e.Y))
                {
                    SetCursor(true);
                    return;
                }
                if (item.buttons == null) continue;
                foreach (var btn in item.buttons)
                {
                    if (!btn.Visible || !btn.Enabled) continue;
                    if (btn.Contains(e.X, e.Y))
                    {
                        btn.hasFocus = btn.AnimationHover = true;
                        if (btn.SwitchMode) btn.ExtraMouseHover = true;
                        else
                        {
                            SetCursor(true);
                            Invalidate(btn.rect);
                        }
                        return;
                    }
                    else
                    {
                        if (btn.AnimationHover)
                        {
                            btn.AnimationHover = false;
                            if (btn.SwitchMode) btn.ExtraMouseHover = false;
                        }
                        btn.AnimationClick = false;
                    }
                }
            }
            SetCursor(false);
        }

        #region 鼠标悬浮

        protected override bool CanMouseMove { get; set; } = true;
        protected override void OnMouseHover(int x, int y)
        {
            CloseTip();
            if (x == -1 || y == -1 || items == null || items.Count == 0) return;
            foreach (var item in items)
            {
                if (item.buttons == null || item.buttons.Count == 0) continue;
                foreach (var btn in item.buttons)
                {
                    if (!btn.Visible || !btn.Enabled) continue;
                    if (btn.rect.Contains(x, y))
                    {
                        OpenTip(btn);
                        return;
                    }
                }
            }
        }

        #region Tip

        TooltipForm? toolTip;

        public void CloseTip()
        {
            toolTip?.IClose();
            toolTip = null;
        }

        bool OpenTip(CollapseGroupButton btn)
        {
            if (btn.Tooltip == null) CloseTip();
            else if (toolTip == null)
            {
                toolTip = new TooltipForm(this, btn.rect, btn.Tooltip, TooltipConfig ?? new TooltipConfig
                {
                    Font = Font,
                    ArrowAlign = TAlign.Bottom,
                });
                toolTip.Show(this);
            }
            else if (toolTip.SetText(btn.rect, btn.Tooltip))
            {
                CloseTip();
                OpenTip(btn);
            }
            return false;
        }

        #endregion

        #endregion

        #endregion

        #region 方法

        public void IUSelect()
        {
            if (items == null || items.Count == 0) return;
            foreach (var it in items) IUSelect(it);
        }
        public void IUSelect(CollapseItem item)
        {
            if (item.buttons == null || item.buttons.Count == 0) return;
            foreach (var btn in item.buttons)
            {
                if (!btn.Visible || !btn.Enabled) continue;
                btn.Select = false;
            }
        }
        #endregion

        #region 设计器

        internal class CollapseDesigner : ParentControlDesigner
        {
            public new Collapse Control => (Collapse)base.Control;

            protected override bool GetHitTest(Point point)
            {
                var point_ = Control.PointToClient(point);
                foreach (var tab in Control.Items)
                {
                    if (tab.Contains(point_.X, point_.Y)) return true;
                }
                return base.GetHitTest(point);
            }
        }

        #endregion
    }

    public class CollapseItemCollection : iCollection<CollapseItem>
    {
        public CollapseItemCollection(Collapse it)
        {
            BindData(it);
        }

        internal CollapseItemCollection BindData(Collapse it)
        {
            action = render =>
            {
                if (render) it.LoadLayout(false);
                it.Invalidate();
            };
            action_add = item =>
            {
                item.PARENT = it;
                item.Location = new Point(-item.Width, -item.Height);
                it.Controls.Add(item);
            };
            action_del = (item, index) =>
            {
                it.Controls.Remove(item);
            };
            return this;
        }
    }

    [ToolboxItem(false)]
    [Designer(typeof(IControlDesigner))]
    public class CollapseItem : ScrollableControl, ICollapseItem
    {
        public CollapseItem()
        {
            SetStyle(
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.ResizeRedraw |
               ControlStyles.DoubleBuffer |
               ControlStyles.SupportsTransparentBackColor |
               ControlStyles.ContainerControl |
               ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        protected override Size DefaultSize => new Size(100, 60);

        #region 属性

        #region 展开

        AnimationTask? ThreadExpand;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("外观"), Description("展开进度"), DefaultValue(0F)]
        internal float ExpandProg { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("外观"), Description("展开状态"), DefaultValue(false)]
        internal bool ExpandThread { get; set; }

        bool expand = false;
        /// <summary>
        /// 是否展开
        /// </summary>
        [Category("外观"), Description("是否展开"), DefaultValue(false)]
        public bool Expand
        {
            get => expand;
            set
            {
                if (expand == value) return;
                expand = value;
                PARENT?.OnExpandChanged2(this, expand);
                if (value) PARENT?.UniqueOne(this);
                if (PARENT != null && PARENT.IsHandleCreated && Config.HasAnimation(nameof(Collapse)))
                {
                    if (PARENT.AutoSize) PARENT.canset = false;
                    Location = new Point(-Width, -Height);
                    ThreadExpand?.Dispose();
                    float oldval = -1;
                    if (ThreadExpand?.Tag is float oldv) oldval = oldv;
                    ExpandThread = true;
                    var t = Animation.TotalFrames(10, PARENT.AnimationSpeed < 10 ? 10 : PARENT.AnimationSpeed);
                    ThreadExpand = new AnimationTask(new AnimationFixed2Config((i, val) =>
                    {
                        ExpandProg = val;
                        PARENT.LoadLayout();
                    }, 10, t, oldval, () =>
                    {
                        if (PARENT.AutoSize) PARENT.canset = true;
                        ExpandProg = 1F;
                        ExpandThread = false;
                        PARENT.LoadLayout();
                    }, value));
                }
                else
                {
                    PARENT?.LoadLayout();
                    if (!value) Location = new Point(-Width, -Height);
                }
            }
        }

        bool full = false;
        /// <summary>
        /// 是否铺满剩下空间
        /// </summary>
        [Category("外观"), Description("是否铺满剩下空间"), DefaultValue(false)]
        public bool Full
        {
            get => full;
            set
            {
                if (full == value) return;
                full = value;
                PARENT?.LoadLayout();
            }
        }

        #region Buttons

        internal CollapseGroupButtonCollection? buttons;
        /// <summary>
        /// 获取折叠项中所有按钮项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("按钮集合"), Category("外观")]
        public CollapseGroupButtonCollection Buttons
        {
            get
            {
                buttons ??= new CollapseGroupButtonCollection(this);
                return buttons;
            }
            set => buttons = value.BindData(this);
        }

        #endregion

        #endregion

        #region 国际化

        string text = "";
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue("")]
        public override string Text
        {
            get => this.GetLangIN(LocalizationText, text);
#pragma warning disable CS8765, CS8767
            set
#pragma warning restore CS8765, CS8767
            {
                if (text == value) return;
                base.Text = text = value;
                PARENT?.LoadLayout();
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        #endregion

        #region Icon

        Image? icon;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                Invalidate();
            }
        }

        string? iconSvg;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 是否包含图片
        /// </summary>
        internal bool HasIcon => iconSvg != null || Icon != null;

        #endregion

        #endregion

        #region 坐标

        internal bool MDown = false;
        public Rectangle Rect = new Rectangle(-10, -10, 0, 0);
        public Rectangle RectArrow, RectControl, RectTitle, RectText;
        internal bool Contains(int x, int y)
        {
            if (buttons == null || buttons.Count == 0) return RectTitle.Contains(x, y);
            foreach (var btn in buttons)
            {
                if (btn.Contains(x, y)) return false;
            }
            return RectTitle.Contains(x, y);
        }

        #endregion

        #region 变更

        internal Collapse? PARENT;
        protected override void OnTextChanged(EventArgs e)
        {
            PARENT?.LoadLayout();
            base.OnTextChanged(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            PARENT?.LoadLayout();
            base.OnVisibleChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (canset) PARENT?.LoadLayout();
            base.OnSizeChanged(e);
        }

        bool canset = true;
        public void SetSize()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(SetSize));
                return;
            }
            canset = false;
            Size = RectControl.Size;
            Location = RectControl.Location;
            canset = true;
        }

        #endregion

        public override string ToString() => Text;
    }
}
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Segmented 分段控制器
    /// </summary>
    /// <remarks>分段控制器。</remarks>
    [Description("Segmented 分段控制器")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    [DefaultEvent("SelectIndexChanged")]
    public class Segmented : IControl
    {
        public Segmented()
        {
            base.BackColor = Color.Transparent;
        }

        #region 属性

        bool vertical = false;
        /// <summary>
        /// 是否竖向
        /// </summary>
        [Description("是否竖向"), Category("外观"), DefaultValue(false)]
        public bool Vertical
        {
            get => vertical;
            set
            {
                if (vertical == value) return;
                vertical = value;
                ChangeItems();
                Invalidate();
            }
        }

        bool full = false;
        /// <summary>
        /// 是否铺满
        /// </summary>
        [Description("是否铺满"), Category("外观"), DefaultValue(false)]
        public bool Full
        {
            get => full;
            set
            {
                if (full == value) return;
                full = value;
                ChangeItems();
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

        bool round = false;
        /// <summary>
        /// 圆角样式
        /// </summary>
        [Description("圆角样式"), Category("外观"), DefaultValue(false)]
        public bool Round
        {
            get => round;
            set
            {
                if (round == value) return;
                round = value;
                Invalidate();
            }
        }

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        public new Color? BackColor
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 悬停背景颜色
        /// </summary>
        [Description("悬停背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? BackHover { get; set; }

        Color? backactive;
        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? BackActive
        {
            get => backactive;
            set
            {
                if (backactive == value) return;
                backactive = value;
                Invalidate();
            }
        }

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 悬停文字颜色
        /// </summary>
        [Description("悬停文字颜色"), Category("外观"), DefaultValue(null)]
        public Color? ForeHover { get; set; }

        Color? foreactive;
        /// <summary>
        /// 激活文字颜色
        /// </summary>
        [Description("激活文字颜色"), Category("外观"), DefaultValue(null)]
        public Color? ForeActive
        {
            get => foreactive;
            set
            {
                if (foreactive == value) return;
                foreactive = value;
                Invalidate();
            }
        }

        SegmentedItemCollection? items;
        /// <summary>
        /// 获取列表中所有列表项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据"), DefaultValue(null)]
        public SegmentedItemCollection Items
        {
            get
            {
                items ??= new SegmentedItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        int _select = -1;
        /// <summary>
        /// 选择序号
        /// </summary>
        [Description("选择序号"), Category("数据"), DefaultValue(-1)]
        public int SelectIndex
        {
            get => _select;
            set
            {
                if (_select == value) return;
                var old = _select;
                _select = value;
                SelectIndexChanged?.Invoke(this, value);
                SetRect(old, _select);
            }
        }

        protected override void Dispose(bool disposing)
        {
            ThreadBar?.Dispose();
            base.Dispose(disposing);
        }
        bool AnimationBar = false;
        RectangleF AnimationBarValue;
        ITask? ThreadBar = null;

        RectangleF TabSelectRect;

        void SetRect(int old, int value)
        {
            if (items == null || items.Count == 0) return;
            var _new = Items[value];
            if (_new == null) return;
            if (old > -1)
            {
                var _old = Items[old];
                if (_old == null) AnimationBarValue = TabSelectRect = _new.Rect;
                else
                {
                    ThreadBar?.Dispose();
                    RectangleF OldValue = _old.Rect, NewValue = _new.Rect;
                    if (Config.Animation)
                    {
                        if (vertical)
                        {
                            if (OldValue.X == NewValue.X)
                            {
                                AnimationBar = true;
                                TabSelectRect = NewValue;
                                float p_val = Math.Abs(NewValue.Y - AnimationBarValue.Y) * 0.09F, p_w_val = Math.Abs(NewValue.Height - AnimationBarValue.Height) * 0.1F, p_val2 = (NewValue.Y - AnimationBarValue.Y) * 0.5F;
                                ThreadBar = new ITask(this, () =>
                                {
                                    if (AnimationBarValue.Height != NewValue.Height)
                                    {
                                        if (NewValue.Height > OldValue.Height)
                                        {
                                            AnimationBarValue.Height += p_w_val;
                                            if (AnimationBarValue.Height > NewValue.Height) AnimationBarValue.Height = NewValue.Height;
                                        }
                                        else
                                        {
                                            AnimationBarValue.Height -= p_w_val;
                                            if (AnimationBarValue.Height < NewValue.Height) AnimationBarValue.Height = NewValue.Height;
                                        }
                                    }
                                    if (NewValue.Y > OldValue.Y)
                                    {
                                        if (AnimationBarValue.Y > p_val2) AnimationBarValue.Y += p_val / 2F;
                                        else AnimationBarValue.Y += p_val;
                                        if (AnimationBarValue.Y > NewValue.Y)
                                        {
                                            AnimationBarValue.Y = NewValue.Y;
                                            Invalidate();
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        AnimationBarValue.Y -= p_val;
                                        if (AnimationBarValue.Y < NewValue.Y)
                                        {
                                            AnimationBarValue.Y = NewValue.Y;
                                            Invalidate();
                                            return false;
                                        }
                                    }
                                    Invalidate();
                                    return true;
                                }, 10, () =>
                                {
                                    AnimationBarValue = NewValue;
                                    AnimationBar = false;
                                    Invalidate();
                                });
                            }
                        }
                        else
                        {
                            if (OldValue.Y == NewValue.Y)
                            {
                                AnimationBar = true;
                                TabSelectRect = NewValue;
                                float p_val = Math.Abs(NewValue.X - AnimationBarValue.X) * 0.09F, p_w_val = Math.Abs(NewValue.Width - AnimationBarValue.Width) * 0.1F, p_val2 = (NewValue.X - AnimationBarValue.X) * 0.5F;
                                ThreadBar = new ITask(this, () =>
                                {
                                    if (AnimationBarValue.Width != NewValue.Width)
                                    {
                                        if (NewValue.Width > OldValue.Width)
                                        {
                                            AnimationBarValue.Width += p_w_val;
                                            if (AnimationBarValue.Width > NewValue.Width) AnimationBarValue.Width = NewValue.Width;
                                        }
                                        else
                                        {
                                            AnimationBarValue.Width -= p_w_val;
                                            if (AnimationBarValue.Width < NewValue.Width) AnimationBarValue.Width = NewValue.Width;
                                        }
                                    }
                                    if (NewValue.X > OldValue.X)
                                    {
                                        if (AnimationBarValue.X > p_val2) AnimationBarValue.X += p_val / 2F;
                                        else AnimationBarValue.X += p_val;
                                        if (AnimationBarValue.X > NewValue.X)
                                        {
                                            AnimationBarValue.X = NewValue.X;
                                            Invalidate();
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        AnimationBarValue.X -= p_val;
                                        if (AnimationBarValue.X < NewValue.X)
                                        {
                                            AnimationBarValue.X = NewValue.X;
                                            Invalidate();
                                            return false;
                                        }
                                    }
                                    Invalidate();
                                    return true;
                                }, 10, () =>
                                {
                                    AnimationBarValue = NewValue;
                                    AnimationBar = false;
                                    Invalidate();
                                });
                            }
                        }
                        return;
                    }
                    else
                    {
                        TabSelectRect = AnimationBarValue = NewValue;
                        Invalidate();
                        return;
                    }
                }
            }
            else
            {
                AnimationBarValue = TabSelectRect = _new.Rect;
                Invalidate();
            }
        }

        /// <summary>
        /// SelectIndex 属性值更改时发生
        /// </summary>
        [Description("SelectIndex 属性值更改时发生"), Category("行为")]
        public event IntEventHandler? SelectIndexChanged = null;

        #region Change

        protected override void OnSizeChanged(EventArgs e)
        {
            ChangeItems();
            base.OnSizeChanged(e);
        }

        protected override void OnMarginChanged(EventArgs e)
        {
            ChangeItems();
            base.OnMarginChanged(e);
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            ChangeItems();
            base.OnPaddingChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            ChangeItems();
            base.OnFontChanged(e);
        }

        #endregion

        #region 布局

        bool pauseLayout = false;
        [Browsable(false), Description("暂停布局"), Category("行为"), DefaultValue(false)]
        public bool PauseLayout
        {
            get => pauseLayout;
            set
            {
                if (pauseLayout == value) return;
                pauseLayout = value;
                if (!value)
                {
                    ChangeItems();
                    Invalidate();
                }
            }
        }

        internal void ChangeItems()
        {
            if (pauseLayout || items == null || items.Count == 0) return;
            var _rect = ClientRectangle.PaddingRect(Padding);
            if (_rect.Width == 0 || _rect.Height == 0) return;
            var rect = _rect.PaddingRect(Margin);

            Helper.GDI(g =>
            {
                var size_t = g.MeasureString(Config.NullText, Font);
                int imgsize = (int)(size_t.Height * 1.8F), text_heigth = (int)Math.Ceiling(size_t.Height), sp = (int)(4 * Config.Dpi), gap = (int)(size_t.Height * 0.6F), gap2 = gap * 2;

                if (Full)
                {
                    int len = Items.Count;
                    if (Vertical)
                    {
                        float heightone = rect.Height * 1F / len, y = 0;
                        foreach (SegmentedItem it in Items)
                        {
                            it.SetRect(new RectangleF(rect.X, rect.Y + y, rect.Width, heightone), imgsize, text_heigth, sp);
                            y += heightone;
                        }
                    }
                    else
                    {
                        float widthone = rect.Width * 1F / len, x = 0;
                        foreach (SegmentedItem it in Items)
                        {
                            it.SetRect(new RectangleF(rect.X + x, rect.Y, widthone, rect.Height), imgsize, text_heigth, sp);
                            x += widthone;
                        }
                    }
                    Rect = _rect;
                }
                else
                {
                    if (Vertical)
                    {
                        int heigth = (int)Math.Ceiling(size_t.Height * 2.4F + gap2);
                        float y = 0;
                        foreach (SegmentedItem it in Items)
                        {
                            it.SetRect(new RectangleF(rect.X, rect.Y + y, rect.Width, heigth), imgsize, text_heigth, sp);
                            y += it.Rect.Height;
                        }
                        Rect = new RectangleF(_rect.X, _rect.Y, _rect.Height, y + Margin.Vertical);
                    }
                    else
                    {
                        float x = 0;
                        foreach (SegmentedItem it in Items)
                        {
                            var size = g.MeasureString(it.Text, Font);
                            it.SetRect(new RectangleF(rect.X + x, rect.Y, size.Width + gap2, rect.Height), imgsize, text_heigth, sp);
                            x += it.Rect.Width;
                        }
                        Rect = new RectangleF(_rect.X, _rect.Y, x + Margin.Horizontal, _rect.Height);
                    }
                }
            });
            if (_select > -1)
            {
                var _new = Items[_select];
                if (_new == null) return;
                AnimationBarValue = TabSelectRect = _new.Rect;
            }
        }

        #endregion

        RectangleF Rect;

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;

            var g = e.Graphics.High();
            float _radius = radius * Config.Dpi;
            using (var path = Rect.RoundPath(_radius, Round))
            {
                using (var brush = new SolidBrush(back.HasValue ? back.Value : Style.Db.BgLayout))
                {
                    g.FillPath(brush, path);
                }
            }
            var item_text = new System.Collections.Generic.List<SegmentedItem>();
            int _hover = -1;
            for (int i = 0; i < Items.Count; i++)
            {
                var it = Items[i];
                if (it == null) continue;
                if (i == _select && !AnimationBar)
                {
                    using (var path = TabSelectRect.RoundPath(_radius, Round))
                    {
                        using (var brush = new SolidBrush(backactive.HasValue ? backactive.Value : Style.Db.BgElevated))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }
                else if (it.Hover)
                {
                    _hover = i;
                    using (var path = it.Rect.RoundPath(_radius, Round))
                    {
                        using (var brush = new SolidBrush(BackHover.HasValue ? BackHover.Value : Style.Db.HoverBg))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }
                item_text.Add(it);
            }
            if (AnimationBar)
            {
                using (var path = AnimationBarValue.RoundPath(_radius, Round))
                {
                    using (var brush = new SolidBrush(backactive.HasValue ? backactive.Value : Style.Db.BgElevated))
                    {
                        g.FillPath(brush, path);
                    }
                }
            }
            using (var brush = new SolidBrush(fore.HasValue ? fore.Value : Style.Db.TextSecondary))
            {
                for (int i = 0; i < item_text.Count; i++)
                {
                    var it = item_text[i];
                    if (i == _select)
                    {
                        using (var brush_active = new SolidBrush(foreactive.HasValue ? foreactive.Value : Style.Db.Text))
                        {
                            if (PaintImg(g, it, brush_active.Color, it.ImgActiveSvg, it.ImgActive)) PaintImg(g, it, brush_active.Color, it.ImgSvg, it.Img);
                            g.DrawString(it.Text, Font, brush_active, it.RectText, Helper.stringFormatCenter);
                        }
                    }
                    else
                    {
                        if (i == _hover)
                        {
                            using (var brush_active = new SolidBrush(ForeHover.HasValue ? ForeHover.Value : Style.Db.HoverColor))
                            {
                                PaintImg(g, it, brush_active.Color, it.ImgSvg, it.Img);
                                g.DrawString(it.Text, Font, brush_active, it.RectText, Helper.stringFormatCenter);
                            }
                        }
                        else
                        {
                            PaintImg(g, it, brush.Color, it.ImgSvg, it.Img);
                            g.DrawString(it.Text, Font, brush, it.RectText, Helper.stringFormatCenter);
                        }
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        bool PaintImg(Graphics g, SegmentedItem it, Color color, string? svg, Image? bmp)
        {
            if (svg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(svg, it.RectImg, color))
                {
                    if (_bmp != null) { g.DrawImage(_bmp, it.RectImg); return false; }
                }
            }
            else if (bmp != null) { g.DrawImage(bmp, it.RectImg); return false; }
            return true;
        }

        #endregion

        #region 鼠标

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (items == null || items.Count == 0) return;
            int hand = 0, change = 0;
            foreach (SegmentedItem it in Items)
            {
                bool hover = it.Rect.Contains(e.Location);
                if (it.Hover != hover)
                {
                    it.Hover = hover;
                    change++;
                }
                if (it.Hover) hand++;
            }
            SetCursor(hand > 0);
            if (change > 0) Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int change = 0;
            foreach (SegmentedItem it in Items)
            {
                if (it.Hover)
                {
                    it.Hover = false;
                    change++;
                }
            }
            if (change > 0) Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int change = 0;
            foreach (SegmentedItem it in Items)
            {
                if (it.Hover)
                {
                    it.Hover = false;
                    change++;
                }
            }
            if (change > 0) Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (items == null || items.Count == 0) return;
            for (int i = 0; i < Items.Count; i++)
            {
                var it = Items[i];
                if (it != null && it.Rect.Contains(e.Location))
                {
                    SelectIndex = i;
                    return;
                }
            }
        }

        #endregion
    }

    public class SegmentedItemCollection : iCollection<SegmentedItem>
    {
        public SegmentedItemCollection(Segmented it)
        {
            BindData(it);
        }

        internal SegmentedItemCollection BindData(Segmented it)
        {
            action = render =>
            {
                if (render) it.ChangeItems();
                it.Invalidate();
            };
            return this;
        }
    }

    public class SegmentedItem
    {
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片"), Category("外观"), DefaultValue(null)]
        public Image? Img { get; set; }

        /// <summary>
        /// 图片SVG
        /// </summary>
        [Description("图片SVG"), Category("外观"), DefaultValue(null)]
        public string? ImgSvg { get; set; }

        /// <summary>
        /// 是否包含图片
        /// </summary>
        public bool HasImg
        {
            get => ImgSvg != null || Img != null;
        }

        /// <summary>
        /// 图片激活
        /// </summary>
        [Description("图片激活"), Category("外观"), DefaultValue(null)]
        public Image? ImgActive { get; set; }

        /// <summary>
        /// 图片激活SVG
        /// </summary>
        [Description("图片激活SVG"), Category("外观"), DefaultValue(null)]
        public string? ImgActiveSvg { get; set; }

        /// <summary>
        /// 文字
        /// </summary>
        [Description("文字"), Category("外观"), DefaultValue(null)]
        public string? Text { get; set; }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        internal bool Hover { get; set; }
        internal void SetRect(RectangleF rect, int imgsize, int text_heigth, int gap)
        {
            Rect = rect;
            if (HasImg)
            {
                float y = (rect.Height - (imgsize + text_heigth + gap)) / 2F;
                RectImg = new RectangleF(rect.X + (rect.Width - imgsize) / 2F, rect.Y + y, imgsize, imgsize);
                RectText = new RectangleF(rect.X, RectImg.Bottom + gap, rect.Width, text_heigth);
            }
            else RectText = rect;
        }
        internal RectangleF Rect { get; set; }
        internal RectangleF RectImg { get; set; }
        internal RectangleF RectText { get; set; }
    }
}
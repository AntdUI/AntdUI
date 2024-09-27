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
using System.Drawing.Design;
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

        #region 线条

        bool barstyle = false;
        [Description("线条样式"), Category("外观"), DefaultValue(false)]
        public bool BarStyle
        {
            get => barstyle;
            set
            {
                if (barstyle == value) return;
                barstyle = value;
                Invalidate();
            }
        }

        float barsize = 3F;
        /// <summary>
        /// 条大小
        /// </summary>
        [Description("条大小"), Category("条"), DefaultValue(3F)]
        public float BarSize
        {
            get => barsize;
            set
            {
                if (barsize == value) return;
                barsize = value;
                if (barstyle) Invalidate();
            }
        }

        int barpadding = 0;
        /// <summary>
        /// 条边距
        /// </summary>
        [Description("条边距"), Category("条"), DefaultValue(0)]
        public int BarPadding
        {
            get => barpadding;
            set
            {
                if (barpadding == value) return;
                barpadding = value;
                if (barstyle) Invalidate();
            }
        }

        /// <summary>
        /// 条圆角
        /// </summary>
        [Description("条圆角"), Category("条"), DefaultValue(0)]
        public int BarRadius { get; set; }

        #endregion

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

        TAlignMini iconalign = TAlignMini.Top;
        [Description("图标对齐方向"), Category("外观"), DefaultValue(TAlignMini.Top)]
        public TAlignMini IconAlign
        {
            get => iconalign;
            set
            {
                if (iconalign == value) return;
                iconalign = value;
                ChangeItems();
                Invalidate();
            }
        }

        int igap = 0;
        [Description("间距"), Category("外观"), DefaultValue(0)]
        public int Gap
        {
            get => igap;
            set
            {
                if (igap == value) return;
                igap = value;
                ChangeItems();
                Invalidate();
            }
        }

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackHover { get; set; }

        Color? backactive;
        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ForeHover { get; set; }

        Color? foreactive;
        /// <summary>
        /// 激活文字颜色
        /// </summary>
        [Description("激活文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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
                SelectIndexChanged?.Invoke(this, new IntEventArgs(value));
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
            if (items.ListExceed(value)) { Invalidate(); return; }
            var _new = items[value];
            if (items.ListExceed(old))
            {
                AnimationBarValue = TabSelectRect = _new.Rect;
                Invalidate();
                return;
            }
            var _old = items[old];
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

        /// <summary>
        /// SelectIndex 属性值更改时发生
        /// </summary>
        [Description("SelectIndex 属性值更改时发生"), Category("行为")]
        public event IntEventHandler? SelectIndexChanged = null;

        /// <summary>
        /// 点击项时发生
        /// </summary>
        [Description("点击项时发生"), Category("行为")]
        public event SegmentedItemEventHandler? ItemClick = null;

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
            if (items == null || items.Count == 0)
            {
                _select = -1;
                return;
            }
            else if (_select >= items.Count) _select = items.Count - 1;
            if (pauseLayout) return;
            var _rect = ClientRectangle.PaddingRect(Padding);
            if (_rect.Width == 0 || _rect.Height == 0) return;
            var rect = _rect.PaddingRect(Margin);

            Helper.GDI(g =>
            {
                var size_t = g.MeasureString(Config.NullText, Font);
                int text_heigth = (int)Math.Ceiling(size_t.Height), sp = (int)(4 * Config.Dpi), _igap = (int)(igap * Config.Dpi), gap = (int)(size_t.Height * 0.6F), gap2 = gap * 2;
                if (Full)
                {
                    int imgsize = (int)(size_t.Height * 1.8F);
                    int len = items.Count;
                    if (Vertical)
                    {
                        int heightone = (rect.Height * 1 - (_igap * (len - 1))) / len, y = 0;
                        switch (iconalign)
                        {
                            case TAlignMini.Top:
                                int imgsize_t = (int)(size_t.Height * 1.8F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X, rect.Y + y, rect.Width, heightone), imgsize_t);
                                    else it.SetRectTop(new Rectangle(rect.X, rect.Y + y, rect.Width, heightone), imgsize_t, text_heigth, sp);
                                    y += heightone + _igap;
                                }
                                break;
                            case TAlignMini.Bottom:
                                int imgsize_b = (int)(size_t.Height * 1.8F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X, rect.Y + y, rect.Width, heightone), imgsize_b);
                                    else it.SetRectBottom(new Rectangle(rect.X, rect.Y + y, rect.Width, heightone), imgsize_b, text_heigth, sp);
                                    y += heightone + _igap;
                                }
                                break;
                            case TAlignMini.Left:
                                int imgsize_l = (int)(size_t.Height * 1.2F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X, rect.Y + y, rect.Width, heightone), imgsize_l);
                                    else it.SetRectLeft(new Rectangle(rect.X, rect.Y + y, rect.Width, heightone), imgsize_l, sp, gap);
                                    y += heightone + _igap;
                                }
                                break;
                            case TAlignMini.Right:
                                int imgsize_r = (int)(size_t.Height * 1.2F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X, rect.Y + y, rect.Width, heightone), imgsize_r);
                                    else it.SetRectRight(new Rectangle(rect.X, rect.Y + y, rect.Width, heightone), imgsize_r, sp, gap);
                                    y += heightone + _igap;
                                }
                                break;
                            default:
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    it.SetRectNone(new Rectangle(rect.X, rect.Y + y, rect.Width, heightone));
                                    y += heightone + _igap;
                                }
                                break;
                        }
                    }
                    else
                    {
                        int widthone = (rect.Width * 1 - (_igap * (len - 1))) / len, x = 0;
                        switch (iconalign)
                        {
                            case TAlignMini.Top:
                                int imgsize_t = (int)(size_t.Height * 1.8F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X + x, rect.Y, widthone, rect.Height), imgsize_t);
                                    else it.SetRectTop(new Rectangle(rect.X + x, rect.Y, widthone, rect.Height), imgsize_t, text_heigth, sp);
                                    x += widthone + _igap;
                                }
                                break;
                            case TAlignMini.Bottom:
                                int imgsize_b = (int)(size_t.Height * 1.8F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X + x, rect.Y, widthone, rect.Height), imgsize_b);
                                    else it.SetRectBottom(new Rectangle(rect.X + x, rect.Y, widthone, rect.Height), imgsize_b, text_heigth, sp);
                                    x += widthone + _igap;
                                }
                                break;
                            case TAlignMini.Left:
                                int imgsize_l = (int)(size_t.Height * 1.2F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X + x, rect.Y, widthone, rect.Height), imgsize_l);
                                    else it.SetRectLeft(new Rectangle(rect.X + x, rect.Y, widthone, rect.Height), imgsize_l, sp, gap);
                                    x += widthone + _igap;
                                }
                                break;
                            case TAlignMini.Right:
                                int imgsize_r = (int)(size_t.Height * 1.2F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X + x, rect.Y, widthone, rect.Height), imgsize_r);
                                    else it.SetRectRight(new Rectangle(rect.X + x, rect.Y, widthone, rect.Height), imgsize_r, sp, gap);
                                    x += widthone + _igap;
                                }
                                break;
                            default:
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    it.SetRectNone(new Rectangle(rect.X + x, rect.Y, widthone, rect.Height));
                                    x += widthone + _igap;
                                }
                                break;
                        }
                    }
                    Rect = _rect;
                }
                else
                {
                    if (Vertical)
                    {
                        int y = 0;
                        switch (iconalign)
                        {
                            case TAlignMini.Top:
                                int imgsize_t = (int)(size_t.Height * 1.8F), heigth_t = (int)Math.Ceiling(size_t.Height * 2.4F + gap2);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X, rect.Y + y, rect.Width, heigth_t), imgsize_t);
                                    else it.SetRectTop(new Rectangle(rect.X, rect.Y + y, rect.Width, heigth_t), imgsize_t, text_heigth, sp);
                                    y += it.Rect.Height + _igap;
                                }
                                break;
                            case TAlignMini.Bottom:
                                int imgsize_b = (int)(size_t.Height * 1.8F), heigth_b = (int)Math.Ceiling(size_t.Height * 2.4F + gap2);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X, rect.Y + y, rect.Width, heigth_b), imgsize_b);
                                    else it.SetRectBottom(new Rectangle(rect.X, rect.Y + y, rect.Width, heigth_b), imgsize_b, text_heigth, sp);
                                    y += it.Rect.Height + _igap;
                                }
                                break;
                            case TAlignMini.Left:
                                int imgsize_l = (int)(size_t.Height * 1.2F), heigth_l = (int)Math.Ceiling(size_t.Height + gap2);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X, rect.Y + y, rect.Width, heigth_l), imgsize_l);
                                    else it.SetRectLeft(new Rectangle(rect.X, rect.Y + y, rect.Width, heigth_l), imgsize_l, sp, gap);
                                    y += it.Rect.Height + _igap;
                                }
                                break;
                            case TAlignMini.Right:
                                int imgsize_r = (int)(size_t.Height * 1.2F), heigth_r = (int)Math.Ceiling(size_t.Height + gap2);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X, rect.Y + y, rect.Width, heigth_r), imgsize_r);
                                    else it.SetRectRight(new Rectangle(rect.X, rect.Y + y, rect.Width, heigth_r), imgsize_r, sp, gap);
                                    y += it.Rect.Height + _igap;
                                }
                                break;
                            default:
                                int heigth = (int)Math.Ceiling(size_t.Height + gap);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    it.SetRectNone(new Rectangle(rect.X, rect.Y + y, rect.Width, heigth));
                                    y += it.Rect.Height + _igap;
                                }
                                break;
                        }
                        Rect = new RectangleF(_rect.X, _rect.Y, _rect.Width, y - _igap + Margin.Vertical);
                    }
                    else
                    {
                        int x = 0;
                        switch (iconalign)
                        {
                            case TAlignMini.Top:
                                int imgsize_t = (int)(size_t.Height * 1.8F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X + x, rect.Y, imgsize_t + gap2, rect.Height), imgsize_t);
                                    else
                                    {
                                        var size = g.MeasureString(it.Text, Font).Size();
                                        it.SetRectTop(new Rectangle(rect.X + x, rect.Y, size.Width + gap2, rect.Height), imgsize_t, text_heigth, sp);
                                    }
                                    x += it.Rect.Width + _igap;
                                }
                                break;
                            case TAlignMini.Bottom:
                                int imgsize_b = (int)(size_t.Height * 1.8F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X + x, rect.Y, imgsize_b + gap2, rect.Height), imgsize_b);
                                    else
                                    {
                                        var size = g.MeasureString(it.Text, Font).Size();
                                        it.SetRectBottom(new Rectangle(rect.X + x, rect.Y, size.Width + gap2, rect.Height), imgsize_b, text_heigth, sp);
                                    }
                                    x += it.Rect.Width + _igap;
                                }
                                break;
                            case TAlignMini.Left:
                                int imgsize_l = (int)(size_t.Height * 1.2F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X + x, rect.Y, imgsize_l + gap2, rect.Height), imgsize_l);
                                    else
                                    {
                                        var size = g.MeasureString(it.Text, Font).Size();
                                        it.SetRectLeft(new Rectangle(rect.X + x, rect.Y, size.Width + imgsize_l + sp + gap2, rect.Height), imgsize_l, sp, gap);
                                    }
                                    x += it.Rect.Width + _igap;
                                }
                                break;
                            case TAlignMini.Right:
                                int imgsize_r = (int)(size_t.Height * 1.2F);
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    if (it.HasIcon && it.HasEmptyText) it.SetIconNoText(new Rectangle(rect.X + x, rect.Y, imgsize_r + gap2, rect.Height), imgsize_r);
                                    else
                                    {
                                        var size = g.MeasureString(it.Text, Font).Size();
                                        it.SetRectRight(new Rectangle(rect.X + x, rect.Y, size.Width + imgsize_r + sp + gap2, rect.Height), imgsize_r, sp, gap);
                                    }
                                    x += it.Rect.Width + _igap;
                                }
                                break;
                            default:
                                foreach (var it in items)
                                {
                                    it.PARENT = this;
                                    var size = g.MeasureString(it.Text, Font).Size();
                                    it.SetRectNone(new Rectangle(rect.X + x, rect.Y, size.Width + gap2, rect.Height));
                                    x += it.Rect.Width + _igap;
                                }
                                break;
                        }
                        Rect = new RectangleF(_rect.X, _rect.Y, x - _igap + Margin.Horizontal, _rect.Height);
                    }
                }
            });
            if (_select > -1)
            {
                var _new = items[_select];
                AnimationBarValue = TabSelectRect = _new.Rect;
            }
        }

        #endregion

        RectangleF Rect;

        #endregion

        #region 渲染

        readonly StringFormat s_f = Helper.SF_ALL();
        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;

            var g = e.Graphics.High();
            float _radius = radius * Config.Dpi;
            bool enabled = Enabled;
            using (var path = Rect.RoundPath(_radius, Round))
            {
                using (var brush = new SolidBrush(back ?? Style.Db.BgLayout))
                {
                    g.FillPath(brush, path);
                }
            }
            var item_text = new System.Collections.Generic.List<SegmentedItem>(items.Count);
            int _hover = -1;
            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                if (it == null) continue;
                if (i == _select && !AnimationBar)
                {
                    if (BarStyle)
                    {
                        float barSize = BarSize * Config.Dpi, barPadding = BarPadding * Config.Dpi, barPadding2 = barPadding * 2;
                        var rect = new RectangleF(TabSelectRect.X + barPadding, TabSelectRect.Bottom - barSize, TabSelectRect.Width - barPadding2, barSize);
                        if (BarRadius > 0)
                        {
                            using (var path = rect.RoundPath(BarRadius * Config.Dpi))
                            {
                                using (var brush = new SolidBrush(backactive ?? Style.Db.BgElevated))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                        }
                        else
                        {
                            using (var brush = new SolidBrush(backactive ?? Style.Db.BgElevated))
                            {
                                g.FillRectangle(brush, rect);
                            }
                        }
                    }
                    else
                    {
                        using (var path = TabSelectRect.RoundPath(_radius, Round))
                        {
                            using (var brush = new SolidBrush(backactive ?? Style.Db.BgElevated))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                    }
                }
                else if (it.Hover)
                {
                    _hover = i;
                    using (var path = it.Rect.RoundPath(_radius, Round))
                    {
                        using (var brush = new SolidBrush(BackHover ?? Style.Db.HoverBg))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }
                item_text.Add(it);
            }
            if (AnimationBar)
            {
                if (BarStyle)
                {
                    float barSize = BarSize * Config.Dpi, barPadding = BarPadding * Config.Dpi, barPadding2 = barPadding * 2;
                    var rect = new RectangleF(AnimationBarValue.X + barPadding, AnimationBarValue.Bottom - barSize, AnimationBarValue.Width - barPadding2, barSize);
                    if (BarRadius > 0)
                    {
                        using (var path = rect.RoundPath(BarRadius * Config.Dpi))
                        {
                            using (var brush = new SolidBrush(backactive ?? Style.Db.BgElevated))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                    }
                    else
                    {
                        using (var brush = new SolidBrush(backactive ?? Style.Db.BgElevated))
                        {
                            g.FillRectangle(brush, rect);
                        }
                    }
                }
                else
                {
                    using (var path = AnimationBarValue.RoundPath(_radius, Round))
                    {
                        using (var brush = new SolidBrush(backactive ?? Style.Db.BgElevated))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }
            }
            using (var brush = new SolidBrush(enabled ? (fore ?? Style.Db.TextSecondary) : Style.Db.TextQuaternary))
            {
                for (int i = 0; i < item_text.Count; i++)
                {
                    var it = item_text[i];
                    if (i == _select)
                    {
                        using (var brush_active = new SolidBrush(enabled ? (foreactive ?? Style.Db.Text) : Style.Db.TextQuaternary))
                        {
                            if (PaintImg(g, it, brush_active.Color, it.IconActiveSvg, it.IconActive)) PaintImg(g, it, brush_active.Color, it.IconSvg, it.Icon);
                            g.DrawStr(it.Text, Font, brush_active, it.RectText, s_f);
                        }
                    }
                    else
                    {
                        if (i == _hover)
                        {
                            using (var brush_active = new SolidBrush(ForeHover ?? Style.Db.HoverColor))
                            {
                                PaintImg(g, it, brush_active.Color, it.IconSvg, it.Icon);
                                g.DrawStr(it.Text, Font, brush_active, it.RectText, s_f);
                            }
                        }
                        else
                        {
                            PaintImg(g, it, brush.Color, it.IconSvg, it.Icon);
                            g.DrawStr(it.Text, Font, brush, it.RectText, s_f);
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
            foreach (var it in items)
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
            foreach (var it in items)
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
            foreach (var it in items)
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
            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                if (it != null && it.Rect.Contains(e.Location))
                {
                    SelectIndex = i;
                    ItemClick?.Invoke(this, new SegmentedItemEventArgs(it, e));
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
        Image? icon = null;
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
                Invalidates();
            }
        }

        string? iconsvg = null;
        /// <summary>
        /// 图标SVG
        /// </summary>
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconsvg;
            set
            {
                if (iconsvg == value) return;
                iconsvg = value;
                Invalidates();
            }
        }

        /// <summary>
        /// 是否包含图标
        /// </summary>
        public bool HasIcon
        {
            get => IconSvg != null || Icon != null;
        }

        /// <summary>
        /// 图标激活
        /// </summary>
        [Description("图标激活"), Category("外观"), DefaultValue(null)]
        public Image? IconActive { get; set; }

        /// <summary>
        /// 图标激活SVG
        /// </summary>
        [Description("图标激活SVG"), Category("外观"), DefaultValue(null)]
        public string? IconActiveSvg { get; set; }

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                Invalidates();
            }
        }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        internal bool Hover { get; set; }

        internal bool HasEmptyText => text == null || string.IsNullOrEmpty(text);

        internal void SetIconNoText(Rectangle rect, int imgsize)
        {
            Rect = rect;
            RectImg = RectText = new Rectangle(rect.X + (rect.Width - imgsize) / 2, rect.Y + (rect.Height - imgsize) / 2, imgsize, imgsize);
        }
        internal void SetRectTop(Rectangle rect, int imgsize, int text_heigth, int gap)
        {
            Rect = rect;
            if (HasIcon)
            {
                int y = (rect.Height - (imgsize + text_heigth + gap)) / 2;
                RectImg = new Rectangle(rect.X + (rect.Width - imgsize) / 2, rect.Y + y, imgsize, imgsize);
                RectText = new Rectangle(rect.X, RectImg.Bottom + gap, rect.Width, text_heigth);
            }
            else RectText = rect;
        }
        internal void SetRectBottom(Rectangle rect, int imgsize, int text_heigth, int gap)
        {
            Rect = rect;
            if (HasIcon)
            {
                int y = (rect.Height - (imgsize + text_heigth + gap)) / 2;
                RectText = new Rectangle(rect.X, rect.Y + y, rect.Width, text_heigth);
                RectImg = new Rectangle(rect.X + (rect.Width - imgsize) / 2, RectText.Bottom + gap, imgsize, imgsize);
            }
            else RectText = rect;
        }
        internal void SetRectLeft(Rectangle rect, int imgsize, int gap, int sp)
        {
            Rect = rect;
            if (HasIcon)
            {
                RectImg = new Rectangle(rect.X + sp, rect.Y + (rect.Height - imgsize) / 2, imgsize, imgsize);
                RectText = new Rectangle(RectImg.Right + gap, rect.Y, rect.Width - sp - imgsize - gap, rect.Height);
            }
            else RectText = rect;
        }
        internal void SetRectRight(Rectangle rect, int imgsize, int gap, int sp)
        {
            Rect = rect;
            if (HasIcon)
            {
                RectText = new Rectangle(rect.X, rect.Y, rect.Width - sp - imgsize - gap, rect.Height);
                RectImg = new Rectangle(RectText.Right + gap, rect.Y + (rect.Height - imgsize) / 2, imgsize, imgsize);
            }
            else RectText = rect;
        }

        internal void SetRectNone(Rectangle rect)
        {
            Rect = rect;
            RectText = rect;
        }
        internal Rectangle Rect { get; set; }
        internal Rectangle RectImg { get; set; }
        internal Rectangle RectText { get; set; }

        internal Segmented? PARENT { get; set; }

        void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.ChangeItems();
            PARENT.Invalidate();
        }

        public override string? ToString() => text;
    }
}
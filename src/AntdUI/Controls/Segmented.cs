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
        #region 属性

        /// <summary>
        /// 是否铺满
        /// </summary>
        [Description("是否铺满"), Category("外观"), DefaultValue(false)]
        public bool Full { get; set; } = false;


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
        public Color? Back
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

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? BackActive { get; set; }

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

                    if (Config.Animation && OldValue.Y == NewValue.Y)
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
                                if (AnimationBarValue.X > p_val2)
                                    AnimationBarValue.X += p_val / 2F;
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
            else AnimationBarValue = TabSelectRect = _new.Rect;
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
        [Description("暂停布局"), Category("行为"), DefaultValue(false)]
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

            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    var size_t = g.MeasureString(Config.NullText, Font);
                    float imgsize = size_t.Height * 1.8F, text_heigth = size_t.Height * 1.642F, gap = size_t.Height * 0.6F, gap2 = gap * 2F;

                    if (Full)
                    {
                        int len = Items.Count;

                        float widthone = rect.Width * 1F / len;

                        float x = 0;
                        foreach (SegmentedItem it in Items)
                        {
                            it.SetRect(new RectangleF(rect.X + x, rect.Y, widthone, rect.Height), imgsize, text_heigth, size_t.Height);
                            x += widthone;
                        }
                        Rect = _rect;
                    }
                    else
                    {
                        float x = 0;
                        foreach (SegmentedItem it in Items)
                        {
                            var size = g.MeasureString(it.Text, Font);
                            it.SetRect(new RectangleF(rect.X + x, rect.Y, size.Width + gap2, rect.Height), imgsize, text_heigth, size_t.Height);
                            x += it.Rect.Width;
                        }
                        Rect = new RectangleF(_rect.X, _rect.Y, x + Margin.Horizontal, _rect.Height);
                        int width = (int)Math.Ceiling(Rect.Width + Padding.Horizontal);
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                Width = width;
                            }));
                        }
                        else Width = width;
                    }
                }
            }

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
                        using (var brush = new SolidBrush(BackActive.HasValue ? BackActive.Value : Style.Db.BgElevated))
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
                    using (var brush = new SolidBrush(BackActive.HasValue ? BackActive.Value : Style.Db.BgElevated))
                    {
                        g.FillPath(brush, path);
                    }
                }
            }
            using (var brush = new SolidBrush(Style.Db.TextSecondary))
            {
                for (int i = 0; i < item_text.Count; i++)
                {
                    var it = item_text[i];
                    if (it.Img != null) g.DrawImage(it.Img, it.RectImg);
                    if (i == _select)
                    {
                        using (var brush_active = new SolidBrush(Style.Db.Text))
                        {
                            g.DrawString(it.Text, Font, brush_active, it.RectText, Helper.stringFormatCenter);
                        }
                    }
                    else if (i == _hover)
                    {
                        using (var brush_active = new SolidBrush(Style.Db.HoverColor))
                        {
                            g.DrawString(it.Text, Font, brush_active, it.RectText, Helper.stringFormatCenter);
                        }
                    }
                    else g.DrawString(it.Text, Font, brush, it.RectText, Helper.stringFormatCenter);
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
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
        /// 文字
        /// </summary>
        [Description("文字"), Category("外观"), DefaultValue(null)]
        public string? Text { get; set; }

        /// <summary>
        /// 与对象关联的用户定义数据
        /// </summary>
        [Description("与对象关联的用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        internal bool Hover { get; set; }
        internal void SetRect(RectangleF rect, float imgsize, float text_heigth, float text_read_heigth)
        {
            Rect = rect;
            if (Img == null) RectText = rect;
            else
            {
                float h = imgsize + text_read_heigth, y = (rect.Height - h) / 2F;
                RectImg = new RectangleF(rect.X + (rect.Width - imgsize) / 2F, rect.Y + y, imgsize, imgsize);
                RectText = new RectangleF(rect.X, rect.Y + y + imgsize, rect.Width, text_heigth);
            }
        }
        internal RectangleF Rect { get; set; }
        internal RectangleF RectImg { get; set; }
        internal RectangleF RectText { get; set; }
    }
}
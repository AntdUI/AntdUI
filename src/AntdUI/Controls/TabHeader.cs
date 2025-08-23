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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// TabHeader 多标签页头
    /// </summary>
    [Description("TabHeader 多标签页头")]
    [DefaultProperty("SelectedIndex")]
    [DefaultEvent("TabChanged")]
    public class TabHeader : PageHeader
    {
        #region 属性

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
                OnPropertyChanged(nameof(Radius));
            }
        }

        /// <summary>
        /// 内容圆角
        /// </summary>
        [Description("内容圆角"), Category("外观"), DefaultValue(4)]
        public int RadiusContent { get; set; } = 4;

        int offsetY = 0;
        /// <summary>
        /// Y偏移量
        /// </summary>
        [Description("Y偏移量"), Category("外观"), DefaultValue(0)]
        public int OffsetY
        {
            get => offsetY;
            set
            {
                if (offsetY == value) return;
                offsetY = value;
                LoadLayout(true);
                OnPropertyChanged(nameof(OffsetY));
            }
        }

        bool showAdd = false;
        /// <summary>
        /// 是否显示添加
        /// </summary>
        [Description("是否显示添加"), Category("外观"), DefaultValue(false)]
        public bool ShowAdd
        {
            get => showAdd;
            set
            {
                if (showAdd == value) return;
                showAdd = value;
                LoadLayout(true);
                OnPropertyChanged(nameof(ShowAdd));
            }
        }

        /// <summary>
        /// 拖拽排序
        /// </summary>
        [Description("拖拽排序"), Category("行为"), DefaultValue(false)]
        public bool DragSort { get; set; }


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
                OnPropertyChanged(nameof(ForeColor));
            }
        }

        /// <summary>
        /// 悬浮文本颜色
        /// </summary>
        [Description("悬浮文本颜色"), Category("外观"), DefaultValue(null)]
        public Color? ForeHover { get; set; }

        /// <summary>
        /// 悬浮背景颜色
        /// </summary>
        [Description("悬浮背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? BackHover { get; set; }

        /// <summary>
        /// 激活文本颜色
        /// </summary>
        [Description("激活文本颜色"), Category("外观"), DefaultValue(null)]
        public Color? ForeActive { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? BackActive { get; set; }

        #region 边框

        float borderWidth = 0;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(0F)]
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

        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        public Color? BorderColor { get; set; }

        #endregion

        #region 边距

        float tabIconRatio = 1.34F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(1.34F)]
        public float TabIconRatio
        {
            get => tabIconRatio;
            set
            {
                if (tabIconRatio == value) return;
                tabIconRatio = value;
                LoadLayout(true);
                OnPropertyChanged(nameof(IconRatio));
            }
        }

        float tabCloseRatio = 1.408F;
        /// <summary>
        /// 关闭按钮比例
        /// </summary>
        [Description("关闭按钮比例"), Category("外观"), DefaultValue(1.408F)]
        public float TabCloseRatio
        {
            get => tabCloseRatio;
            set
            {
                if (tabCloseRatio == value) return;
                tabCloseRatio = value;
                LoadLayout(true);
                OnPropertyChanged(nameof(TabCloseRatio));
            }
        }

        /// <summary>
        /// 关闭图标比例
        /// </summary>
        [Description("关闭图标比例"), Category("外观"), DefaultValue(.74F)]
        public float TabCloseIconRatio { get; set; } = .74F;

        float tabGapRatio = .6F;
        /// <summary>
        /// 边距比例
        /// </summary>
        [Description("边距比例"), Category("外观"), DefaultValue(.6F)]
        public float TabGapRatio
        {
            get => tabGapRatio;
            set
            {
                if (tabGapRatio == value) return;
                tabGapRatio = value;
                LoadLayout(true);
                OnPropertyChanged(nameof(TabGapRatio));
            }
        }

        float tabIconGapRatio = .74F;
        /// <summary>
        /// 图标与文字间距比例
        /// </summary>
        [Description("图标与文字间距比例"), Category("外观"), DefaultValue(.74F)]
        public float TabIconGapRatio
        {
            get => tabIconGapRatio;
            set
            {
                if (tabIconGapRatio == value) return;
                tabIconGapRatio = value;
                LoadLayout(true);
                OnPropertyChanged(nameof(TabIconGapRatio));
            }
        }

        float tabAddIconRatio = 1.18F;
        /// <summary>
        /// 新增按钮图标比例
        /// </summary>
        [Description("新增按钮图标比例"), Category("外观"), DefaultValue(1.18F)]
        public float TabAddIconRatio
        {
            get => tabAddIconRatio;
            set
            {
                if (tabAddIconRatio == value) return;
                tabAddIconRatio = value;
                if (showAdd) LoadLayout(true);
                OnPropertyChanged(nameof(TabAddIconRatio));
            }
        }

        float tabAddGapRatio = .148F;
        /// <summary>
        /// 新增按钮边距比例
        /// </summary>
        [Description("新增按钮边距比例"), Category("外观"), DefaultValue(.148F)]
        public float TabAddGapRatio
        {
            get => tabAddGapRatio;
            set
            {
                if (tabAddGapRatio == value) return;
                tabAddGapRatio = value;
                if (showAdd) LoadLayout(true);
                OnPropertyChanged(nameof(TabAddGapRatio));
            }
        }

        int rightGap = 0;
        /// <summary>
        /// 右侧边距
        /// </summary>
        [Description("右侧边距"), Category("外观"), DefaultValue(0)]
        public int RightGap
        {
            get => rightGap;
            set
            {
                if (rightGap == value) return;
                rightGap = value;
                LoadLayout(true);
                OnPropertyChanged(nameof(RightGap));
            }
        }

        #endregion

        #region 数据

        TagTabCollection? items;
        /// <summary>
        /// 数据
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据")]
        public TagTabCollection Items
        {
            get
            {
                items ??= new TagTabCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        int _select = 0;
        [Description("选中序号"), Category("数据"), DefaultValue(0)]
        public int SelectedIndex
        {
            get => _select;
            set
            {
                if (_select == value) return;
                int old = _select;
                _select = value;
                Invalidate();
                if (items == null) return;
                SelectedItem = items[value];
                TabChanged?.Invoke(this, new TabChangedEventArgs(_selectItem!, value));
            }
        }

        TagTabItem? _selectItem = null;
        [Description("选中选项"), Category("数据"), DefaultValue(0)]
        public TagTabItem? SelectedItem
        {
            get => _selectItem;
            set
            {
                if (_selectItem == value || value is null) return;
                _selectItem = value;
                Invalidate();
                if (items == null) return;
                //获取Index
                var index = items.IndexOf(value);
                TabSelectedItemChanged?.Invoke(this, new TabChangedEventArgs(value, index));
            }
        }

        #endregion

        #endregion

        #region 布局

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            LoadLayout();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadLayout();
        }

        bool CanLayout()
        {
            if (IsHandleCreated)
            {
                var rect = ClientRectangle;
                if (items == null || items.Count == 0 || rect.Width == 0 || rect.Height == 0) return false;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 计算所有标签的布局和尺寸
        /// </summary>
        internal void LoadLayout(bool print = false)
        {
            if (CanLayout())
            {
                Helper.GDI(g =>
                {
                    var dir = new Dictionary<int, int[]>(items!.Count);
                    int txtHeight = g.MeasureString(Config.NullText, Font).Height, txtTW = 0, border = (int)(borderWidth * Config.Dpi), border2 = border * 2, offset = (int)(offsetY * Config.Dpi);
                    Rectangle crect = ClientRectangle.PaddingRect(Padding), rect = new Rectangle(crect.X, crect.Y + offset, crect.Width - rightGap, crect.Height - offset);
                    if (showAdd) rect.Width -= rect.Height;
                    int paddx = (int)(txtHeight * tabGapRatio), paddx2 = paddx * 2, gap = (int)(txtHeight * tabIconGapRatio), gap2 = gap * 2,
                    ico_size = (int)(txtHeight * tabIconRatio), close_size = (int)(txtHeight * tabCloseRatio), close_i_size = (int)(txtHeight * TabCloseIconRatio),
                    ico_y = rect.Y + (rect.Height - ico_size) / 2, close_y = rect.Y + (rect.Height - close_size) / 2, close_ico_y = (close_size - close_i_size) / 2;
                    int use_x = rect.X, count_loading = 0;
                    for (int i = 0; i < items.Count; i++)
                    {
                        var it = items[i];
                        it.PARENT = this;
                        if (it.Visible)
                        {
                            var size = g.MeasureText(it.Text, Font, 0, sf);
                            int tabWidth;
                            if (it.HasIcon)
                            {
                                if (it.ShowClose) tabWidth = size.Width + paddx2 + gap2 + ico_size + close_size;
                                else tabWidth = size.Width + paddx2 + gap2 + ico_size;
                            }
                            else
                            {
                                if (it.ShowClose) tabWidth = size.Width + paddx2 + gap + close_size;
                                else tabWidth = size.Width + paddx2;
                            }
                            dir.Add(i, new[] { size.Width, tabWidth, size.Width });
                            txtTW += tabWidth;
                        }
                    }

                    #region 判断超出缩进

                    int mw = rect.Width - hasr - _hasl;
                    if (txtTW > mw)
                    {
                        var dirb = new Dictionary<int, float>(items.Count);
                        foreach (var it in dir) dirb.Add(it.Key, it.Value[1] * 1F / txtTW);
                        for (int i = 0; i < items.Count; i++)
                        {
                            var it = items[i];
                            if (it.Visible)
                            {
                                int max = (int)Math.Round(mw * dirb[i]);
                                if (dir[i][1] > max)
                                {
                                    if (it.HasIcon)
                                    {
                                        if (it.ShowClose) dir[i][0] = max - (paddx2 + gap2 + ico_size + close_size);
                                        else dir[i][0] = max - (paddx2 + gap2 + ico_size);
                                    }
                                    else
                                    {
                                        if (it.ShowClose) dir[i][0] = max - (paddx2 + gap + close_size);
                                        else dir[i][0] = max - paddx2;
                                    }
                                    dir[i][1] = max;
                                }
                            }
                        }
                    }

                    #endregion

                    for (int i = 0; i < items.Count; i++)
                    {
                        var it = items[i];
                        if (it.Visible)
                        {
                            var textSize = dir[i];
                            int tabWidth = textSize[1];
                            it.Ellipsis = textSize[2] > textSize[0];
                            it.ShowText = textSize[0] > 0;
                            if (it.HasIcon || it.Loading)
                            {
                                var _rect = new Rectangle(use_x, rect.Y, tabWidth, rect.Height);
                                it.Rect = new Rectangle(_rect.X, _rect.Y, _rect.Width, _rect.Height + border2);
                                int x = _rect.X + paddx;
                                if (it.ShowText)
                                {
                                    it.RectIcon = new Rectangle(x, ico_y, ico_size, ico_size);
                                    var _rect_text = new Rectangle(x + ico_size + gap, _rect.Y, textSize[0], _rect.Height);
                                    it.RectText = _rect_text;
                                    it.RectTextFull = new Rectangle(_rect_text.X, _rect_text.Y, textSize[2], _rect_text.Height);

                                    if (it.ShowClose)
                                    {
                                        var _rect_close = new Rectangle(x + ico_size + gap2 + textSize[0], close_y, close_size, close_size);
                                        it.RectClose = _rect_close;
                                        it.RectCloseIco = new Rectangle(_rect_close.X + close_ico_y, _rect_close.Y + close_ico_y, close_i_size, close_i_size);
                                    }
                                    it.ShowIcon = true;
                                }
                                else
                                {
                                    var _rect_text = new Rectangle(x + ico_size + gap, _rect.Y, textSize[0], _rect.Height);

                                    if (it.ShowClose)
                                    {
                                        var _rect_close = new Rectangle(x + ico_size + gap2 + textSize[0], close_y, close_size, close_size);
                                        if (_rect_close.X < _rect_text.X - gap)
                                        {
                                            it.ShowIcon = false;
                                            _rect_close.X = _rect.X + (_rect.Width - close_size) / 2;
                                            it.RectClose = _rect_close;
                                            it.RectCloseIco = new Rectangle(_rect_close.X + close_ico_y, _rect_close.Y + close_ico_y, close_i_size, close_i_size);
                                        }
                                        else
                                        {
                                            it.RectIcon = new Rectangle(x, ico_y, ico_size, ico_size);
                                            it.RectText = _rect_text;
                                            it.RectTextFull = new Rectangle(_rect_text.X, _rect_text.Y, textSize[2], _rect_text.Height);
                                            it.RectClose = _rect_close;
                                            it.RectCloseIco = new Rectangle(_rect_close.X + close_ico_y, _rect_close.Y + close_ico_y, close_i_size, close_i_size);
                                            it.ShowIcon = true;
                                        }
                                    }
                                    else
                                    {
                                        it.RectIcon = new Rectangle(x, ico_y, ico_size, ico_size);
                                        it.RectText = _rect_text;
                                        it.RectTextFull = new Rectangle(_rect_text.X, _rect_text.Y, textSize[2], _rect_text.Height);
                                        it.ShowIcon = true;
                                    }
                                }
                                if (it.ShowIcon && it.Loading) count_loading++;
                            }
                            else
                            {
                                var _rect = new Rectangle(use_x, rect.Y, tabWidth, rect.Height);
                                it.Rect = new Rectangle(_rect.X, _rect.Y, _rect.Width, _rect.Height + border2);
                                if (it.ShowText)
                                {
                                    var _rect_text = new Rectangle(_rect.X + paddx, _rect.Y, textSize[0], _rect.Height);
                                    it.RectText = _rect_text;
                                    it.RectTextFull = new Rectangle(_rect_text.X, _rect_text.Y, textSize[2], _rect_text.Height);

                                    if (it.ShowClose)
                                    {
                                        var _rect_close = new Rectangle(_rect.X + paddx + gap + textSize[0], close_y, close_size, close_size);
                                        it.RectClose = _rect_close;
                                        it.RectCloseIco = new Rectangle(_rect_close.X + close_ico_y, _rect_close.Y + close_ico_y, close_i_size, close_i_size);
                                    }
                                }
                                else if (it.ShowClose)
                                {
                                    var _rect_close = new Rectangle(_rect.X + (_rect.Width - close_size) / 2, close_y, close_size, close_size);
                                    it.RectClose = _rect_close;
                                    it.RectCloseIco = new Rectangle(_rect_close.X + close_ico_y, _rect_close.Y + close_ico_y, close_i_size, close_i_size);
                                }
                            }
                            use_x += tabWidth;
                        }
                    }

                    if (showAdd)
                    {
                        int ico_add_size = (int)(txtHeight * tabAddIconRatio), gap_add = (int)(txtHeight * tabAddGapRatio), gap_add2 = gap_add * 2, h = rect.Height - gap_add2, iy = (h - ico_add_size) / 2;
                        RectAdd = new Rectangle(use_x + gap_add, rect.Y + gap_add, h, h);
                        RectAddIco = new Rectangle(RectAdd.X + iy, RectAdd.Y + iy, ico_add_size, ico_add_size);
                    }

                    if (count_loading > 0)
                    {
                        if (ThreadLoading == null)
                        {
                            ThreadLoading = new ITask(this, () =>
                            {
                                AnimationLoadingValue += 6;
                                if (AnimationLoadingValue > 360) AnimationLoadingValue = 0;
                                Invalidate();
                                return true;
                            }, 10, () => Invalidate());
                        }
                    }
                    else
                    {
                        ThreadLoading?.Dispose();
                        ThreadLoading = null;
                    }
                    return use_x;
                });
            }
            if (print) Invalidate();
        }

        Rectangle RectAdd, RectAddIco;
        bool HoverAdd = false;

        #endregion

        #region 渲染

        protected override void PaintContent(Canvas g, Rectangle rect, int left, int rigth)
        {
            if (items == null) return;
            var state = g.Save();
            var rect_real = new Rectangle(rect.X + left, rect.Y, rect.Width - rigth - left, rect.Height);
            // 设置新的剪裁区域
            g.SetClip(rect_real);
            g.TranslateTransform(left, 0);
            int _radius = (int)(radius * Config.Dpi), radiusContent = (int)(RadiusContent * Config.Dpi), border = (int)(borderWidth * Config.Dpi);
            TagTabItem? tabselect = null;
            var color = Colour.Text.Get("TabHeader", ColorScheme);
            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                if (it.Visible)
                {
                    if (i == _select)
                    {
                        tabselect = items[i];
                        continue;
                    }
                    DrawTab(g, items[i], color, i, _radius, radiusContent, border);
                }
            }
            if (dragHeader != null && dragHeader.im > -1)
            {
                g.Restore(state);
                state = g.Save();
                var it = items[dragHeader.im];
                using (var brush_split = new SolidBrush(Colour.BorderColor.Get("TabHeader", ColorScheme)))
                {
                    int sp = (int)(2 * Config.Dpi);
                    if (dragHeader.last) g.Fill(brush_split, new Rectangle(left + it.Rect.Right - sp, it.Rect.Y, sp * 2, it.Rect.Height));
                    else g.Fill(brush_split, new Rectangle(left + it.Rect.X - sp, it.Rect.Y, sp * 2, it.Rect.Height));
                }
                g.SetClip(rect_real);
                g.TranslateTransform(left, 0);
            }
            if (tabselect != null)
            {
                using (var path = tabselect.Rect.RoundPath(radius, true, true, false, false))
                {
                    g.Fill(BackActive ?? Colour.BgBase.Get("TabHeader", ColorScheme), path);
                    if (border > 0)
                    {
                        using (var pen = new Pen(BorderColor ?? Colour.BorderColor.Get("TabHeader", ColorScheme), border))
                        {
                            g.Draw(pen, path);
                        }
                    }
                }
                DrawText(g, tabselect, ForeActive ?? fore ?? color);
                DrawCloseButton(g, tabselect, color, radiusContent);
            }
            if (showAdd)
            {
                if (HoverAdd)
                {
                    using (var path = RectAdd.RoundPath(radiusContent))
                    {
                        g.Fill(Colour.FillSecondary.Get("TabHeader"), path);
                    }
                }
                g.PaintIconCore(RectAddIco, "PlusOutlined", color);
            }
            g.Restore(state);
        }

        StringFormat sf = Helper.SF_NoWrap(lr: StringAlignment.Near);
        /// <summary>
        /// 绘制单个标签（包含圆角）
        /// </summary>
        void DrawTab(Canvas g, TagTabItem tab, Color color, int index, int radius, int radiusContent, int border)
        {
            if (tab.Hover)
            {
                using (var path = tab.Rect.RoundPath(radius, true, true, false, false))
                {
                    g.Fill(BackHover ?? Colour.FillTertiary.Get("TabHeader", ColorScheme), path);
                }
                DrawText(g, tab, ForeHover ?? fore ?? color);
            }
            else DrawText(g, tab, fore ?? color);

            // 绘制关闭按钮
            DrawCloseButton(g, tab, color, radiusContent);
        }
        void DrawText(Canvas g, TagTabItem tab, Color color)
        {
            if (tab.ShowText)
            {
                if (tab.Ellipsis)
                {
                    float prog = tab.RectText.Width * 1F / tab.RectTextFull.Width;
                    using (var brush = new LinearGradientBrush(tab.RectTextFull, Color.Transparent, Color.Transparent, 0F))
                    {
                        brush.InterpolationColors = new ColorBlend(4)
                        {
                            Colors = new Color[] { color, color, Color.Transparent, Color.Transparent },
                            Positions = new float[] { 0, prog / 2F, prog, 1F }
                        };
                        g.DrawText(tab.Text, Font, brush, tab.RectTextFull, sf);
                    }
                }
                else g.DrawText(tab.Text, Font, color, tab.RectText, sf);
            }
            if (tab.ShowIcon)
            {
                if (tab.Loading)
                {
                    using (var pen = new Pen(Colour.Fill.Get("PageHeader", ColorScheme), tab.RectIcon.Height * .14F))
                    using (var brush = new Pen(Color.FromArgb(170, color), pen.Width))
                    {
                        g.DrawEllipse(pen, tab.RectIcon);
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, tab.RectIcon, AnimationLoadingValue, 100);
                    }
                }
                else
                {
                    // 绘制图标
                    if (tab.Icon != null) g.Image(tab.Icon, tab.RectIcon);
                    if (tab.IconSvg != null) g.GetImgExtend(tab.IconSvg, tab.RectIcon, color);
                }
            }
        }

        /// <summary>
        /// 绘制关闭按钮
        /// </summary>
        /// <param name="g"></param>
        /// <param name="tab"></param>
        void DrawCloseButton(Canvas g, TagTabItem tab, Color color, int radius)
        {
            if (!tab.ShowClose) return;

            if (tab.HoverClose)
            {
                using (var path = tab.RectClose.RoundPath(radius))
                {
                    g.Fill(Colour.FillSecondary.Get("TabHeader"), path);
                }
            }
            g.PaintIconClose(tab.RectCloseIco, color);
        }

        #region Loading

        int AnimationLoadingValue = 0;
        ITask? ThreadLoading;

        protected override void Dispose(bool disposing)
        {
            ThreadLoading?.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #endregion

        #region 鼠标

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (items == null) return;
            int count = 0, hand = 0;
            int x = e.X - _hasl, y = e.Y;
            if (dragHeader != null)
            {
                SetCursor(CursorType.SizeAll);
                dragHeader.hand = true;
                dragHeader.xr = x - dragHeader.x;
                int xr = dragHeader.x + dragHeader.xr;
                dragHeader.last = x > dragHeader.x;

                var cel_sel = HitTestCore(x, y, out int i);
                if (cel_sel != null)
                {
                    if (i == dragHeader.i) dragHeader.im = -1;
                    else dragHeader.im = i;
                }
                else
                {
                    var last = items.Count - 1;
                    if (last == dragHeader.i) dragHeader.im = -1;
                    else dragHeader.im = last;
                }
                Invalidate();
                return;
            }
            foreach (var it in items)
            {
                if (it.Visible && it.Enabled && it.Rect.Contains(x, y))
                {
                    bool hoveClose = it.ShowClose && it.RectClose.Contains(x, y);
                    if (hoveClose) hand++;
                    if (it.Hover == true && it.HoverClose == hoveClose) continue;
                    it.Hover = true;
                    it.HoverClose = hoveClose;
                    count++;
                }
                else
                {
                    if (it.Hover)
                    {
                        count++;
                        it.Hover = false;
                    }
                    if (it.HoverClose)
                    {
                        count++;
                        it.HoverClose = false;
                    }
                }
            }
            if (showAdd && RectAdd.Contains(x, y))
            {
                hand++;
                if (!HoverAdd)
                {
                    HoverAdd = true;
                    count++;
                }
            }
            else if (HoverAdd)
            {
                HoverAdd = false;
                count++;
            }
            SetCursor(hand > 0);
            if (count > 0) Invalidate();
        }

        TagTabItem? mdown;
        int mdownindex;
        Table.DragHeader? dragHeader;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            mdown = null;
            if (items == null)
            {
                int x = e.X - _hasl, y = e.Y;
                if (showAdd && RectAdd.Contains(x, y)) return;
                base.OnMouseDown(e);
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                int x = e.X - _hasl, y = e.Y;
                if (showAdd && RectAdd.Contains(x, y)) return;
                for (int i = 0; i < items.Count; i++)
                {
                    var it = items[i];
                    if (it.Visible && it.Enabled && it.Rect.Contains(x, y))
                    {
                        mdownindex = i;
                        mdown = it;
                        SelectedIndex = i;
                        SelectedItem = it;
                        if (DragSort)
                        {
                            dragHeader = new Table.DragHeader(e.X, e.Y, i, x);
                            return;
                        }
                        return;
                    }
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            int x = e.X - _hasl, y = e.Y;
            if (dragHeader != null)
            {
                bool hand = dragHeader.hand;
                if (hand && dragHeader.im != -1)
                {
                    _select = dragHeader.im;
                    var source = items![dragHeader.i];
                    items.RemoveAt(dragHeader.i);
                    items.Insert(dragHeader.im, source);
                    LoadLayout(true);
                }
                dragHeader = null;
                if (hand)
                {
                    Invalidate();
                    OnTouchCancel();
                    return;
                }
            }
            if (showAdd && RectAdd.Contains(x, y))
            {
                dragHeader = null;
                mdown = null;
                AddClick?.Invoke(this, EventArgs.Empty);
                return;
            }
            if (items == null) return;
            if (mdown == null)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        var it = items[i];
                        if (it.Visible && it.Enabled && it.ShowClose && it.Rect.Contains(x, y))
                        {
                            // 触发标签关闭事件
                            var args = new TabCloseEventArgs(it, mdownindex);
                            TabClosing?.Invoke(this, args);
                            if (args.Cancel) return;
                            items.Remove(it);
                            // 如果关闭的是当前选中标签，自动选择下一个标签
                            if (mdownindex == items.Count) SelectedIndex = Math.Max(0, items.Count - 1);
                            SelectedItem = items[SelectedIndex];
                            return;
                        }
                    }
                }
                return;
            }
            if (mdown.ShowClose && mdown.RectClose.Contains(x, y))
            {
                // 触发标签关闭事件
                var args = new TabCloseEventArgs(mdown, mdownindex);
                TabClosing?.Invoke(this, args);
                if (args.Cancel) return;
                items.Remove(mdown);
                if (mdown == _selectItem)
                {
                    if (_select > 0 && items.Count > 0) SelectedIndex--;
                }
                else
                {
                    if (_select > 0) _select--;
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            int count = 0;
            if (HoverAdd)
            {
                HoverAdd = false;
                count++;
            }
            if (items == null)
            {
                if (count > 0) Invalidate();
                return;
            }
            // 重置所有标签的悬停状态
            foreach (var it in items)
            {
                if (it.Hover || it.HoverClose)
                {
                    it.Hover = false;
                    it.HoverClose = false;
                    count++;
                }
            }
            if (count > 0) Invalidate();
        }

        #endregion

        #region 方法

        public TagTabItem? HitTest(int x, int y, out int i) => HitTestCore(x - _hasl, y, out i);
        public TagTabItem? HitTestCore(int x, int y, out int i)
        {
            i = -1;
            if (items == null) return null;
            for (int j = 0; j < items.Count; j++)
            {
                var it = items[j];
                if (it.Visible && it.Enabled && it.Rect.Contains(x, y))
                {
                    i = j;
                    return it;
                }
            }
            return null;
        }

        public void Select(TagTabItem item)
        {
            if (items == null) return;
            SelectedIndex = items.IndexOf(item);
            SelectedItem = item;
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="item"></param>
        public void AddTab(TagTabItem item, bool select = false)
        {
            if (select)
            {
                if (items == null) _select = 0;
                else _select = items.Count;
            }
            Items.Add(item);
        }

        /// <summary>
        /// 插入标签
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void InsertTab(int index, TagTabItem item, bool select = false)
        {
            if (select) _select = index;
            Items.Insert(index, item);
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        public void AddTab(string text, Image? icon = null) => Items.Add(new TagTabItem(text, icon));

        /// <summary>
        /// 添加标签
        /// </summary>
        public void AddTab(string text, string? iconsvg = null) => Items.Add(new TagTabItem(text, iconsvg));

        /// <summary>
        /// 插入标签
        /// </summary>
        /// <param name="index"></param>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        public void InsertTab(int index, string text, Image? icon = null) => Items.Insert(index, new TagTabItem(text, icon));

        /// <summary>
        /// 移除标签
        /// </summary>
        /// <param name="index"></param>
        public void RemoveTab(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                Items.RemoveAt(index);
                LoadLayout(true);
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 点击添加按钮
        /// </summary>
        [Description("添加选项事件")]
        public event EventHandler? AddClick;
        /// <summary>
        /// 序列号变动事件
        /// </summary>
        [Description("序列号变动事件")]
        public event EventHandler<TabChangedEventArgs>? TabChanged;
        /// <summary>
        /// 选项选中事件
        /// </summary>
        [Description("选项选中事件")]
        public event EventHandler<TabChangedEventArgs>? TabSelectedItemChanged;
        /// <summary>
        /// 选项关闭事件
        /// </summary>
        [Description("选项关闭事件")]
        public event EventHandler<TabCloseEventArgs>? TabClosing;

        #endregion
    }

    public class TagTabCollection : iCollection<TagTabItem>
    {
        public TagTabCollection(TabHeader it)
        {
            BindData(it);
        }

        internal TagTabCollection BindData(TabHeader it)
        {
            action = render =>
            {
                if (render) it.LoadLayout(true);
                else it.Invalidate();
            };
            return this;
        }
    }

    /// <summary>
    /// 标签页数据结构
    /// </summary>
    public class TagTabItem
    {
        public TagTabItem() : this("Text")
        { }

        public TagTabItem(string text)
        {
            _text = text;
        }

        public TagTabItem(string text, Image? icon)
        {
            _text = text;
            Icon = icon;
        }
        public TagTabItem(string text, string? iconSvg)
        {
            _text = text;
            IconSvg = iconSvg;
        }

        /// <summary>
        /// ID
        /// </summary>
        [Description("ID"), Category("数据"), DefaultValue(null)]
        public string? ID { get; set; }

        string _text;
        /// <summary>
        /// 文本
        /// </summary>
        [Category("外观"), Description("文本"), Localizable(true)]
        public string Text
        {
            get => Localization.GetLangI(LocalizationText, _text, new string?[] { "{id}", ID }) ?? _text;
            set
            {
                if (_text == value) return;
                _text = value;
                PARENT?.LoadLayout(true);
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        Image? icon;
        /// <summary>
        /// 图标
        /// </summary>
        [Category("外观"), Description("图标"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                PARENT?.LoadLayout(true);
            }
        }

        string? iconSvg;
        /// <summary>
        /// 图标
        /// </summary>
        [Category("外观"), Description("图标SVG"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                PARENT?.LoadLayout(true);
            }
        }

        /// <summary>
        /// 是否包含图片
        /// </summary>
        public bool HasIcon => iconSvg != null || icon != null;

        public bool Hover { get; set; }
        public bool HoverClose { get; set; }

        bool showClose = true;
        /// <summary>
        /// 是否显示
        /// </summary>
        [Description("是否显示关闭"), Category("外观"), DefaultValue(true)]
        public bool ShowClose
        {
            get => showClose;
            set
            {
                if (showClose == value) return;
                showClose = value;
                PARENT?.LoadLayout(true);
            }
        }

        bool visible = true;
        /// <summary>
        /// 是否显示
        /// </summary>
        [Description("是否显示"), Category("外观"), DefaultValue(true)]
        public bool Visible
        {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;
                PARENT?.LoadLayout(true);
            }
        }

        #region 禁用

        bool enabled = true;
        /// <summary>
        /// 禁用状态
        /// </summary>
        [Description("禁用状态"), Category("行为"), DefaultValue(true)]
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value) return;
                enabled = value;
                PARENT?.Invalidate();
            }
        }

        #endregion

        #region 加载

        /// <summary>
        /// 加载状态
        /// </summary>
        [Description("加载状态"), Category("外观"), DefaultValue(false)]
        public bool Loading { get; set; }

        #endregion

        public object? Tag { get; set; }

        #region 内部

        #region 变更

        internal TabHeader? PARENT;

        #endregion

        internal bool Ellipsis { get; set; }
        internal bool ShowText { get; set; }
        internal bool ShowIcon { get; set; }
        internal Rectangle Rect { get; set; }
        internal Rectangle RectText { get; set; }
        internal Rectangle RectTextFull { get; set; }
        internal Rectangle RectIcon { get; set; }
        internal Rectangle RectClose { get; set; }
        internal Rectangle RectCloseIco { get; set; }

        #endregion

        #region 设置

        #region 图标

        public TagTabItem SetIcon(Image? img)
        {
            icon = img;
            return this;
        }

        public TagTabItem SetIcon(string? svg)
        {
            iconSvg = svg;
            return this;
        }

        #endregion

        public TagTabItem SetID(string? value)
        {
            ID = value;
            return this;
        }

        public TagTabItem SetText(string value, string? localization = null)
        {
            _text = value;
            LocalizationText = localization;
            return this;
        }

        public TagTabItem SetVisible(bool value = false)
        {
            visible = value;
            return this;
        }

        public TagTabItem SetEnabled(bool value = false)
        {
            enabled = value;
            return this;
        }

        public TagTabItem SetLoading(bool value = true)
        {
            Loading = value;
            return this;
        }

        public TagTabItem SetShowClose(bool value = false)
        {
            showClose = value;
            return this;
        }

        public TagTabItem SetTag(object? value)
        {
            Tag = value;
            return this;
        }

        #endregion
    }
}

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
    /// Tree 树形控件
    /// </summary>
    /// <remarks>多层次的结构列表。</remarks>
    [Description("Tree 树形控件")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    [DefaultEvent("SelectChanged")]
    public class Tree : IControl
    {
        #region 属性

        /// <summary>
        /// 悬停背景颜色
        /// </summary>
        [Description("悬停背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackHover { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackActive { get; set; }

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

        /// <summary>
        /// 激活字体颜色
        /// </summary>
        [Description("激活字体颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ForeActive { get; set; }

        float iconratio = 1F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(1F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                if (IsHandleCreated)
                {
                    ChangeList();
                    Invalidate();
                }
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
                ChangeList();
                Invalidate();
            }
        }

        /// <summary>
        /// 间距缩进
        /// </summary>
        [Description("间距缩进"), Category("外观"), DefaultValue(null)]
        public int? GapIndent { get; set; }

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

        bool checkable = false;
        /// <summary>
        /// 节点前添加 Checkbox 复选框
        /// </summary>
        [Description("节点前添加 Checkbox 复选框"), Category("外观"), DefaultValue(false)]
        public bool Checkable
        {
            get => checkable;
            set
            {
                if (checkable == value) return;
                checkable = value;
                ChangeList();
                Invalidate();
            }
        }

        /// <summary>
        /// Checkable 状态下节点选择完全受控（父子节点选中状态不再关联）
        /// </summary>
        [Description("Checkable 状态下节点选择完全受控（父子节点选中状态不再关联）"), Category("行为"), DefaultValue(true)]
        public bool CheckStrictly { get; set; } = true;

        bool blockNode = false;
        /// <summary>
        /// 节点占据一行
        /// </summary>
        [Description("节点占据一行"), Category("外观"), DefaultValue(false)]
        public bool BlockNode
        {
            get => blockNode;
            set
            {
                if (blockNode == value) return;
                blockNode = value;
                ChangeList();
                Invalidate();
            }
        }

        /// <summary>
        /// 支持点选多个节点
        /// </summary>
        [Description("支持点选多个节点"), Category("行为"), DefaultValue(false)]
        public bool Multiple { get; set; }

        TreeItemCollection? items;
        /// <summary>
        /// 集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据")]
        public TreeItemCollection Items
        {
            get
            {
                items ??= new TreeItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        TreeItem? selectItem;
        /// <summary>
        /// 选择项
        /// </summary>
        [Browsable(false), Description("选择项"), Category("数据"), DefaultValue(null)]
        public TreeItem? SelectItem
        {
            get => selectItem;
            set
            {
                if (selectItem == value) return;
                selectItem = value;
                if (value == null) USelect(false);
                else Select(value);
            }
        }

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
                    ChangeList();
                    Invalidate();
                }
                OnPropertyChanged(nameof(PauseLayout));
            }
        }

        /// <summary>
        /// 滚动条
        /// </summary>
        [Browsable(false)]
        public ScrollBar ScrollBar;

        bool empty = true;
        [Description("是否显示空样式"), Category("外观"), DefaultValue(true)]
        public bool Empty
        {
            get => empty;
            set
            {
                if (empty == value) return;
                empty = value;
                Invalidate();
                OnPropertyChanged(nameof(Empty));
            }
        }

        string? emptyText;
        [Description("数据为空显示文字"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? EmptyText
        {
            get => emptyText;
            set
            {
                if (emptyText == value) return;
                emptyText = value;
                Invalidate();
                OnPropertyChanged(nameof(EmptyText));
            }
        }

        [Description("数据为空显示图片"), Category("外观"), DefaultValue(null)]
        public Image? EmptyImage { get; set; }

        #endregion

        #region 事件

        /// <summary>
        /// Select 属性值更改时发生
        /// </summary>
        [Description("Select 属性值更改时发生"), Category("行为")]
        public event TreeSelectEventHandler? SelectChanged;

        /// <summary>
        /// Expand 更改前发生
        /// </summary>
        [Description("Expand 更改前发生"), Category("行为")]
        public event TreeExpandEventHandler? BeforeExpand;

        /// <summary>
        /// Expand 更改后发生
        /// </summary>
        [Description("Expand 更改后发生"), Category("行为")]
        public event TreeCheckedEventHandler? AfterExpand;

        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category("行为")]
        public event TreeCheckedEventHandler? CheckedChanged;

        /// <summary>
        /// 点击项事件
        /// </summary>
        [Description("点击项事件"), Category("行为")]
        public event TreeSelectEventHandler? NodeMouseClick;

        /// <summary>
        /// 双击项事件
        /// </summary>
        [Description("双击项事件"), Category("行为")]
        public event TreeSelectEventHandler? NodeMouseDoubleClick;

        /// <summary>
        /// 移动项事件
        /// </summary>
        [Description("移动项事件"), Category("行为")]
        public event TreeHoverEventHandler? NodeMouseMove;
        internal void OnNodeMouseMove(TreeItem item, bool hover)
        {
            if (NodeMouseMove == null) return;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            NodeMouseMove(this, new TreeHoverEventArgs(item, item.Rect("Text", sx, sy), hover));
        }

        internal void OnSelectChanged(TreeItem item, TreeCType type, MouseEventArgs args)
        {
            if (SelectChanged == null) return;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            SelectChanged(this, new TreeSelectEventArgs(item, item.Rect("Text", sx, sy), type, args));
        }
        internal void OnNodeMouseClick(TreeItem item, TreeCType type, MouseEventArgs args)
        {
            if (NodeMouseClick == null) return;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            NodeMouseClick(this, new TreeSelectEventArgs(item, item.Rect("Text", sx, sy), type, args));
        }
        internal void OnNodeMouseDoubleClick(TreeItem item, TreeCType type, MouseEventArgs args)
        {
            if (NodeMouseDoubleClick == null) return;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            NodeMouseDoubleClick(this, new TreeSelectEventArgs(item, item.Rect("Text", sx, sy), type, args));
        }
        internal void OnCheckedChanged(TreeItem item, bool value) => CheckedChanged?.Invoke(this, new TreeCheckedEventArgs(item, value));

        internal void OnAfterExpand(TreeItem item, bool value) => AfterExpand?.Invoke(this, new TreeCheckedEventArgs(item, value));

        #endregion

        #region 布局

        protected override void OnSizeChanged(EventArgs e)
        {
            ChangeList();
            base.OnSizeChanged(e);
        }

        internal void ChangeList()
        {
            var rect = ClientRectangle;
            if (pauseLayout || items == null || items.Count == 0 || (rect.Width == 0 || rect.Height == 0)) return;
            int x = 0, y = 0;
            bool has = HasSub(items);
            Helper.GDI(g =>
            {
                var size = g.MeasureString(Config.NullText, Font);
                int icon_size = (int)(size.Height * iconratio), depth_gap = GapIndent.HasValue ? (int)(GapIndent.Value * Config.Dpi) : icon_size, gap = (int)(_gap * Config.Dpi), gapI = gap / 2, height = icon_size + gap * 2;
                check_radius = icon_size * .2F;
                if (CheckStrictly && has && items[0].PARENT == null && items[0].PARENTITEM == null)
                {
                    //新数据
                    var dir = new List<TreeItem>();
                    TestSub(ref dir, items);
                    foreach (var item in dir)
                    {
                        int check_count = 0;
                        foreach (var sub in item.Sub)
                        { if (sub.CheckState == CheckState.Checked || sub.CheckState == CheckState.Indeterminate) check_count++; }
                        if (check_count > 0) item.CheckState = check_count == item.Sub.Count ? CheckState.Checked : CheckState.Indeterminate;
                        else item.CheckState = CheckState.Unchecked;
                    }
                }
                ChangeList(g, rect, null, items, has, ref x, ref y, height, depth_gap, icon_size, gap, gapI, 0, true);
            });
            ScrollBar.SetVrSize(x, y);
            ScrollBar.SizeChange(rect);
        }

        bool HasSub(TreeItemCollection items)
        {
            foreach (var it in items)
            {
                if (it.ICanExpand) return true;
            }
            return false;
        }
        void TestSub(ref List<TreeItem> dir, TreeItemCollection items)
        {
            foreach (var it in items)
            {
                if (it.ICanExpand)
                {
                    dir.Insert(0, it);
                    TestSub(ref dir, it.Sub);
                }
            }
        }

        void ChangeList(Canvas g, Rectangle rect, TreeItem? Parent, TreeItemCollection items, bool has_sub, ref int x, ref int y, int height, int depth_gap, int icon_size, int gap, int gapI, int depth, bool expand)
        {
            int i = 0;
            foreach (var it in items)
            {
                it.Index = i;
                i++;
                it.PARENT = this;
                it.PARENTITEM = Parent;
                if (it.Visible)
                {
                    it.SetRect(g, Font, depth, checkable, blockNode, has_sub, new Rectangle(0, y, rect.Width, height), depth_gap, icon_size, gap);
                    if (expand)
                    {
                        if (it.subtxt_rect.Right > x) x = it.subtxt_rect.Right;
                        else if (it.txt_rect.Right > x) x = it.txt_rect.Right;
                    }
                    y += height + gapI;
                    if (it.ICanExpand)
                    {
                        int y_item = y;
                        ChangeList(g, rect, it, it.Sub, has_sub, ref x, ref y, height, depth_gap, icon_size, gap, gapI, depth + 1, expand ? it.Expand : false);
                        it.SubY = y_item - gapI / 2;
                        it.SubHeight = y - y_item;
                        if ((it.Expand || it.ExpandThread) && it.ExpandProg > 0)
                        {
                            it.ExpandHeight = y - y_item;
                            y = y_item + (int)(it.ExpandHeight * it.ExpandProg);
                        }
                        else if (!it.Expand) y = y_item;
                    }
                }
            }
        }

        #endregion

        #region 渲染

        float check_radius = 0F;
        public Tree()
        {
            ScrollBar = new ScrollBar(this, true, true);
        }

        protected override void OnDraw(DrawEventArgs e)
        {
            if (items == null || items.Count == 0)
            {
                if (Empty) e.Canvas.PaintEmpty(e.Rect, Font, fore ?? Colour.Text.Get("Tree", ColorScheme), EmptyText, EmptyImage, 0);
                base.OnDraw(e);
                return;
            }
            var g = e.Canvas;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            g.TranslateTransform(-sx, -sy);
            float _radius = radius * Config.Dpi;
            using (var brush_fore = new SolidBrush(fore ?? Colour.TextBase.Get("Tree", ColorScheme)))
            using (var brush_fore_active = new SolidBrush(ForeActive ?? Colour.Primary.Get("Tree", ColorScheme)))
            using (var brush_hover = new SolidBrush(BackHover ?? Colour.FillSecondary.Get("Tree", ColorScheme)))
            using (var brush_active = new SolidBrush(BackActive ?? Colour.PrimaryBg.Get("Tree", ColorScheme)))
            using (var brush_TextTertiary = new SolidBrush(Colour.TextTertiary.Get("Tree", ColorScheme)))
            {
                PaintItem(g, e.Rect, sx, sy, items, brush_fore, brush_fore_active, brush_hover, brush_active, brush_TextTertiary, _radius);
            }
            g.ResetTransform();
            ScrollBar.Paint(g);
            base.OnDraw(e);
        }
        void PaintItem(Canvas g, Rectangle rect, int sx, int sy, TreeItemCollection items, SolidBrush fore, SolidBrush fore_active, SolidBrush hover, SolidBrush active, SolidBrush brushTextTertiary, float radius)
        {
            foreach (var it in items)
            {
                it.show = IsShowRect(rect, sx, sy, it);
                if (it.show)
                {
                    PaintItem(g, it, fore, fore_active, hover, active, brushTextTertiary, radius, sx, sy);
                    if ((it.Expand || it.ExpandThread) && it.items != null && it.items.Count > 0)
                    {
                        var state = g.Save();
                        if (it.ExpandThread) g.SetClip(new RectangleF(rect.X, it.rect.Bottom, rect.Width, it.ExpandHeight * it.ExpandProg));
                        PaintItem(g, rect, sx, sy, it.items, fore, fore_active, hover, active, brushTextTertiary, radius);
                        g.Restore(state);
                    }
                }
                else ShowFalse(it.items);
            }
        }
        bool IsShowRect(Rectangle rect, int sx, int sy, TreeItem it)
        {
            if (it.Show && it.Visible)
            {
                bool inVisibleX = (it.rect.X <= (sx + rect.Width)) && (it.rect.Right >= sx),
                    inVisibleY = (it.rect.Y <= (sy + rect.Height)) && ((it.Expand ? it.rect.Bottom + (int)Math.Ceiling(it.SubHeight) : it.rect.Bottom) >= sy);
                if (inVisibleY && inVisibleX) return true;
            }
            return false;
        }
        void ShowFalse(TreeItemCollection? items)
        {
            if (items == null || items.Count == 0) return;
            foreach (var it in items)
            {
                it.show = false;
                ShowFalse(it.items);
            }
        }

        readonly StringFormat s_c = Helper.SF_Ellipsis(), s_l = Helper.SF_ALL(lr: StringAlignment.Near);
        void PaintItem(Canvas g, TreeItem item, SolidBrush fore, SolidBrush fore_active, SolidBrush hover, SolidBrush active, SolidBrush brushTextTertiary, float radius, int sx, int sy)
        {
            if (item.Select)
            {
                PaintBack(g, active, item.rect, radius);
                if (item.ICanExpand) PaintArrow(g, item, fore_active, sx, sy);
                PaintItemText(g, item, fore_active, brushTextTertiary);
            }
            else
            {
                if (item.Back.HasValue)
                {
                    using (var brush = new SolidBrush(item.Back.Value))
                    {
                        PaintBack(g, brush, item.rect, radius);
                    }
                }

                if (item.AnimationHover)
                {
                    using (var brush = new SolidBrush(Helper.ToColorN(item.AnimationHoverValue, hover.Color)))
                    {
                        PaintBack(g, brush, item.rect, radius);
                    }
                }
                else if (item.Hover) PaintBack(g, hover, item.rect, radius);
                if (item.ICanExpand) PaintArrow(g, item, fore, sx, sy);
                if (item.Enabled) PaintItemText(g, item, fore, brushTextTertiary);
                else
                {
                    using (var brush = new SolidBrush(Colour.TextQuaternary.Get("Tree", ColorScheme)))
                    {
                        PaintItemText(g, item, brush, brushTextTertiary);
                    }
                }
            }
            if (checkable)
            {
                using (var path_check = Helper.RoundPath(item.check_rect, check_radius, false))
                {
                    var bor2 = 2F * Config.Dpi;
                    if (item.Enabled)
                    {
                        if (item.AnimationCheck)
                        {
                            var alpha = 255 * item.AnimationCheckValue;

                            if (item.CheckState == CheckState.Indeterminate || (item.checkStateOld == CheckState.Indeterminate && !item.Checked))
                            {
                                g.Draw(Colour.BorderColor.Get("Tree", ColorScheme), bor2, path_check);
                                g.Fill(Helper.ToColor(alpha, Colour.Primary.Get("Tree", ColorScheme)), PaintBlock(item.check_rect));
                            }
                            else
                            {
                                float dot = item.check_rect.Width * 0.3F;

                                g.Fill(Helper.ToColor(alpha, Colour.Primary.Get("Tree", ColorScheme)), path_check);
                                g.DrawLines(Helper.ToColor(alpha, Colour.BgBase.Get("Tree", ColorScheme)), 3F * Config.Dpi, PaintArrow(item.check_rect));

                                if (item.Checked)
                                {
                                    float max = item.check_rect.Height + item.check_rect.Height * item.AnimationCheckValue, alpha2 = 100 * (1F - item.AnimationCheckValue);
                                    using (var brush = new SolidBrush(Helper.ToColor(alpha2, Colour.Primary.Get("Tree", ColorScheme))))
                                    {
                                        g.FillEllipse(brush, new RectangleF(item.check_rect.X + (item.check_rect.Width - max) / 2F, item.check_rect.Y + (item.check_rect.Height - max) / 2F, max, max));
                                    }
                                }
                                g.Draw(Colour.Primary.Get("Tree", ColorScheme), 2F * Config.Dpi, path_check);
                            }
                        }
                        else if (item.CheckState == CheckState.Indeterminate)
                        {
                            g.Draw(Colour.BorderColor.Get("Tree", ColorScheme), bor2, path_check);
                            g.Fill(Colour.Primary.Get("Tree", ColorScheme), PaintBlock(item.check_rect));
                        }
                        else if (item.Checked)
                        {
                            g.Fill(Colour.Primary.Get("Tree", ColorScheme), path_check);
                            g.DrawLines(Colour.BgBase.Get("Tree", ColorScheme), bor2, PaintArrow(item.check_rect));
                        }
                        else g.Draw(Colour.BorderColor.Get("Tree", ColorScheme), bor2, path_check);
                    }
                    else
                    {
                        g.Fill(Colour.FillQuaternary.Get("Tree", ColorScheme), path_check);
                        if (item.CheckState == CheckState.Indeterminate) g.Fill(Colour.TextQuaternary.Get("Tree", ColorScheme), PaintBlock(item.check_rect));
                        else if (item.Checked) g.DrawLines(Colour.TextQuaternary.Get("Tree", ColorScheme), bor2, PaintArrow(item.check_rect));
                        g.Draw(Colour.BorderColorDisable.Get("Tree", ColorScheme), bor2, path_check);
                    }
                }
            }
        }
        void PaintItemText(Canvas g, TreeItem item, SolidBrush fore, SolidBrush brushTextTertiary)
        {
            Color color = fore.Color;
            if (item.Fore.HasValue)
            {
                color = item.Fore.Value;
                using (var brush = new SolidBrush(color))
                {
                    g.DrawText(item.Text, Font, brush, item.txt_rect, s_c);
                }
            }
            else g.DrawText(item.Text, Font, fore, item.txt_rect, s_c);
            if (item.SubTitle != null) g.DrawText(item.SubTitle, Font, brushTextTertiary, item.subtxt_rect, s_l);
            if (item.Loading)
            {
                float loading_size = item.ico_rect.Height * .14F;
                var bor3 = 3F * Config.Dpi;
                g.DrawEllipse(Colour.Fill.Get("Tree"), bor3, item.ico_rect);
                using (var pen = new Pen(color, loading_size))
                {
                    pen.StartCap = pen.EndCap = LineCap.Round;
                    g.DrawArc(pen, item.ico_rect, item.AnimationLoadingValue, 100);
                }
            }
            else
            {
                if (item.Icon != null) g.Image(item.Icon, item.ico_rect);
                if (item.IconSvg != null) g.GetImgExtend(item.IconSvg, item.ico_rect, color);
            }

        }

        internal RectangleF PaintBlock(RectangleF rect)
        {
            float size = rect.Height * 0.2F, size2 = size * 2F;
            return new RectangleF(rect.X + size, rect.Y + size, rect.Width - size2, rect.Height - size2);
        }
        internal PointF[] PaintArrow(RectangleF rect)
        {
            float size = rect.Height * 0.15F, size2 = rect.Height * 0.2F, size3 = rect.Height * 0.26F;
            return new PointF[] {
                new PointF(rect.X+size,rect.Y+rect.Height/2),
                new PointF(rect.X+rect.Width*0.4F,rect.Y+(rect.Height-size3)),
                new PointF(rect.X+rect.Width-size2,rect.Y+size2),
            };
        }

        void PaintArrow(Canvas g, TreeItem item, SolidBrush color, int sx, int sy)
        {
            using (var pen = new Pen(color, 2F))
            {
                pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

                if (item.ExpandThread) PaintArrow(g, item, pen, sx, sy, -90F + (90F * item.ExpandProg));
                else if (item.Expand) g.DrawLines(pen, item.arrow_rect.TriangleLines(-1, .4F));
                else PaintArrow(g, item, pen, sx, sy, -90F);
            }
        }

        void PaintArrow(Canvas g, TreeItem item, Pen pen, int sx, int sy, float rotate)
        {
            int size_arrow = item.arrow_rect.Width / 2;
            g.TranslateTransform(item.arrow_rect.X + size_arrow, item.arrow_rect.Y + size_arrow);
            g.RotateTransform(rotate);
            g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, item.arrow_rect.Width, item.arrow_rect.Height).TriangleLines(-1, .4F));
            g.ResetTransform();
            g.TranslateTransform(-sx, -sy);
        }

        void PaintBack(Canvas g, SolidBrush brush, Rectangle rect, float radius)
        {
            if (round || radius > 0)
            {
                using (var path = rect.RoundPath(radius, round))
                {
                    g.Fill(brush, path);
                }
            }
            else g.Fill(brush, rect);
        }

        #endregion

        #region 鼠标

        TreeItem? MDown;
        bool doubleClick = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            doubleClick = e.Clicks > 1;
            MDown = null;
            if (ScrollBar.MouseDownY(e.Location) && ScrollBar.MouseDownX(e.Location))
            {
                if (items == null || items.Count == 0) return;
                OnTouchDown(e.X, e.Y);
                foreach (var it in items)
                {
                    if (IMouseDown(e, it)) return;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (ScrollBar.MouseUpY() && ScrollBar.MouseUpX() && OnTouchUp())
            {
                if (items == null || items.Count == 0 || MDown == null) return;
                foreach (var it in items)
                {
                    if (IMouseUp(e, it, MDown)) return;
                }
            }
        }
        bool IMouseDown(MouseEventArgs e, TreeItem item)
        {
            var down = item.Contains(e.X, e.Y, ScrollBar.ValueX, ScrollBar.ValueY, checkable, blockNode);
            if (down > 0)
            {
                MDown = item;
                return true;
            }
            if (item.ICanExpand && item.Expand)
            {
                foreach (var sub in item.Sub)
                {
                    if (IMouseDown(e, sub)) return true;
                }
            }
            return false;
        }

        bool _multiple = false;
        TreeItem? shift_index;
        bool IMouseUp(MouseEventArgs e, TreeItem item, TreeItem MDown)
        {
            bool can = item.ICanExpand;
            if (MDown == item)
            {
                var down = item.Contains(e.X, e.Y, ScrollBar.ValueX, ScrollBar.ValueY, checkable, blockNode);
                if (down > 0)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (down == TreeCType.Check && item.Enabled)
                        {
                            item.Checked = !item.Checked;
                            if (CheckStrictly)
                            {
                                SetCheck(item, item.Checked);
                                SetCheckStrictly(item.PARENTITEM);
                            }
                        }
                        else if (down == TreeCType.Arrow && can)
                        {
                            bool value = !item.Expand;
                            if (BeforeExpand == null) item.Expand = value;
                            else
                            {
                                var arge = new TreeExpandEventArgs(item, value);
                                BeforeExpand(this, arge);
                                if (arge.CanExpand) item.Expand = value;
                            }
                        }
                        else
                        {
                            if (doubleClick && can)
                            {
                                bool value = !item.Expand;
                                if (BeforeExpand == null) item.Expand = value;
                                else
                                {
                                    var arge = new TreeExpandEventArgs(item, value);
                                    BeforeExpand(this, arge);
                                    if (arge.CanExpand) item.Expand = value;
                                }
                            }
                            else
                            {
                                selectItem = item;
                                if (Multiple && ModifierKeys.HasFlag(Keys.Shift))
                                {
                                    _multiple = true;
                                    if (shift_index == null) item.SetSelect();
                                    else
                                    {
                                        if (item == shift_index) item.SetSelect();
                                        else if (shift_index.rect.Y > item.rect.Y) SetSelects(items!, item, shift_index);
                                        else SetSelects(items!, shift_index, item);
                                    }
                                }
                                else if (Multiple && ModifierKeys.HasFlag(Keys.Control))
                                {
                                    _multiple = true;
                                    item.SetSelect();
                                }
                                else
                                {
                                    if (_multiple)
                                    {
                                        _multiple = false;
                                        if (item.Select) USelect(false);
                                        item.Select = true;
                                    }
                                    else item.Select = true;
                                }
                                shift_index = item;
                                OnSelectChanged(item, down, e);
                                Invalidate();
                            }
                        }
                    }
                    if (doubleClick) OnNodeMouseDoubleClick(item, down, e);
                    else OnNodeMouseClick(item, down, e);
                }
                return true;
            }
            if (can && item.Expand)
            {
                foreach (var sub in item.Sub)
                {
                    if (IMouseUp(e, sub, MDown)) return true;
                }
            }
            return false;
        }

        bool SetSelects(TreeItemCollection items, TreeItem first, TreeItem last)
        {
            bool start = false;
            return SetSelects(items, first, last, ref start);
        }
        bool SetSelects(TreeItemCollection items, TreeItem first, TreeItem last, ref bool start)
        {
            foreach (var it in items)
            {
                if (last == it)
                {
                    it.SetSelect();
                    return true;
                }
                else if (first == it) start = true;
                if (start) it.SetSelect();
                if (it.items != null && it.items.Count > 0)
                {
                    if (SetSelects(it.items, first, last, ref start)) return true;
                }
            }
            return false;
        }

        void SetCheck(TreeItem item, bool value)
        {
            if (item.items != null && item.items.Count > 0)
            {
                foreach (var it in item.items)
                {
                    it.Checked = value;
                    SetCheck(it, value);
                }
            }
        }

        void SetCheckStrictly(TreeItem? item)
        {
            if (item == null) return;
            int check_all_count = 0, check_count = 0;
            foreach (var sub in item.Sub)
            {
                if (sub.CheckState == CheckState.Checked)
                {
                    check_count++;
                    check_all_count++;
                }
                else if (sub.CheckState == CheckState.Indeterminate) check_all_count++;
            }
            if (check_all_count > 0) item.CheckState = check_count == item.Sub.Count ? CheckState.Checked : CheckState.Indeterminate;
            else item.CheckState = CheckState.Unchecked;
            SetCheckStrictly(item.PARENTITEM);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ScrollBar.MouseMoveY(e.Location) && ScrollBar.MouseMoveX(e.Location))
            {
                if (OnTouchMove(e.X, e.Y))
                {
                    if (items == null || items.Count == 0) return;
                    int hand = 0;
                    foreach (var it in items) IMouseMove(it, e.X, e.Y, ref hand);
                    SetCursor(hand > 0);
                }
            }
            else ILeave();
        }

        void IMouseMove(TreeItem item, int x, int y, ref int hand)
        {
            if (item.show)
            {
                if (item.Contains(x, y, ScrollBar.ValueX, ScrollBar.ValueY, checkable, blockNode) > 0) hand++;
                if (item.items != null) foreach (var sub in item.items) IMouseMove(sub, x, y, ref hand);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ScrollBar.Leave();
            ILeave();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ScrollBar.Leave();
            ILeave();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ScrollBar.MouseWheel(e);
            base.OnMouseWheel(e);
        }
        protected override bool OnTouchScrollX(int value) => ScrollBar.MouseWheelXCore(value);
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelYCore(value);

        void ILeave()
        {
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int count = 0;
            foreach (var it in items)
            {
                ILeave(it, ref count);
            }
            if (count > 0) Invalidate();
        }
        void ILeave(TreeItem item, ref int count)
        {
            if (item.Hover) count++;
            item.Hover = false;
            if (item.items == null) return;
            foreach (var sub in item.items) ILeave(sub, ref count);
        }
        void IUSelect(TreeItem item)
        {
            item.Select = false;
            if (item.items == null) return;
            foreach (var sub in item.items) IUSelect(sub);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 选择指定项
        /// </summary>
        public bool Select(TreeItem item) => Select(items, item);

        bool Select(TreeItemCollection? items, TreeItem item)
        {
            if (items == null || items.Count == 0) return false;
            foreach (var it in items)
            {
                if (it == item)
                {
                    selectItem = item;
                    it.Select = true;
                    OnSelectChanged(it, TreeCType.None, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
                    return true;
                }
                if (Select(it.items, item)) return true;
            }
            return false;
        }

        /// <summary>
        /// 设置全部 Visible
        /// </summary>
        public void VisibleAll(bool value = true)
        {
            if (items == null || items.Count == 0) return;
            VisibleAll(value, items);
        }
        public void VisibleAll(bool value, TreeItemCollection items)
        {
            foreach (var it in items)
            {
                if (it.items == null || it.items.Count == 0) continue;
                it.Visible = value;
                VisibleAll(value, it.items);
            }
        }

        /// <summary>
        /// 取消全部选择
        /// </summary>
        public void USelect(bool clear = true)
        {
            if (clear) selectItem = null;
            if (items == null || items.Count == 0) return;
            foreach (var it in items) IUSelect(it);
        }

        /// <summary>
        /// 移除菜单
        /// </summary>
        /// <param name="item">项</param>
        public void Remove(TreeItem item)
        {
            if (items == null || items.Count == 0) return;
            Remove(item, items);
        }
        void Remove(TreeItem item, TreeItemCollection items)
        {
            foreach (var it in items)
            {
                if (it == item)
                {
                    items.Remove(it);
                    return;
                }
                else if (it.items != null && it.items.Count > 0) Remove(item, it.items);
            }
        }

        #region 集合操作

        /// <summary>
        /// 展开全部
        /// </summary>
        /// <param name="value">true 展开、false 收起</param>
        public void ExpandAll(bool value = true)
        {
            if (items != null && items.Count > 0) ExpandAll(items, value);
        }

        public void ExpandAll(TreeItemCollection items, bool value = true)
        {
            if (items != null && items.Count > 0)
            {
                foreach (var it in items)
                {
                    it.Expand = value;
                    ExpandAll(it.Sub, value);
                }
            }
        }

        #region 获取项

        /// <summary>
        /// 获取所有选择项
        /// </summary>
        public List<TreeItem> GetSelects()
        {
            if (items == null) return new List<TreeItem>(0);
            return GetSelects(items);
        }

        List<TreeItem> GetSelects(TreeItemCollection items)
        {
            var list = new List<TreeItem>(items.Count);
            foreach (var it in items)
            {
                if (it.Select) list.Add(it);
                if (it.items != null && it.items.Count > 0)
                {
                    var list_sub = GetSelects(it.items);
                    if (list_sub.Count > 0) list.AddRange(list_sub);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取所有选中项
        /// </summary>
        /// <param name="Indeterminate">是否包含 Indeterminate</param>
        public List<TreeItem> GetCheckeds(bool Indeterminate = true)
        {
            if (items == null) return new List<TreeItem>(0);
            return GetCheckeds(items, Indeterminate);
        }
        List<TreeItem> GetCheckeds(TreeItemCollection items, bool Indeterminate)
        {
            var list = new List<TreeItem>(items.Count);
            if (Indeterminate)
            {
                foreach (var it in items)
                {
                    if (it.CheckState != CheckState.Unchecked) list.Add(it);
                    if (it.items != null && it.items.Count > 0)
                    {
                        var list_sub = GetCheckeds(it.items, Indeterminate);
                        if (list_sub.Count > 0) list.AddRange(list_sub);
                    }
                }
            }
            else
            {
                foreach (var it in items)
                {
                    if (it.Checked) list.Add(it);
                    if (it.items != null && it.items.Count > 0)
                    {
                        var list_sub = GetCheckeds(it.items, Indeterminate);
                        if (list_sub.Count > 0) list.AddRange(list_sub);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 全选/全不选
        /// </summary>
        public void SetCheckeds()
        {
            if (items == null) return;
            var list = GetCheckeds();
            SetCheckeds(list.Count == 0);
        }

        /// <summary>
        /// 全选/全不选
        /// </summary>
        public void SetCheckeds(bool check)
        {
            if (items == null) return;
            SetCheckeds(items, check);
        }
        public void SetCheckeds(TreeItemCollection items, bool check)
        {
            foreach (var it in items)
            {
                it.Checked = check;
                if (it.items != null && it.items.Count > 0) SetCheckeds(it.items, check);
            }
        }

        public void Focus(TreeItem item, int gap = 0, bool force = false)
        {
            if (ScrollBar.ShowY && (force || !item.show)) ScrollBar.ValueY = item.rect.Y - gap - (int)(_gap * Config.Dpi);
        }

        #endregion

        #endregion

        public TreeItem? HitTest(int x, int y, out TreeCType type)
        {
            if (items == null || items.Count == 0)
            {
                type = TreeCType.None;
                return null;
            }
            foreach (var it in items)
            {
                if (IHitTest(x, y, it, out var md, out type)) return md;
            }
            type = TreeCType.None;
            return null;
        }
        bool IHitTest(int x, int y, TreeItem item, out TreeItem? mdown, out TreeCType down)
        {
            down = item.Contains(x, y, ScrollBar.ValueX, ScrollBar.ValueY, checkable, blockNode);
            if (down > 0)
            {
                mdown = item;
                return true;
            }
            if (item.ICanExpand && item.Expand)
            {
                foreach (var sub in item.Sub)
                {
                    if (IHitTest(x, y, sub, out mdown, out down)) return true;
                }
            }
            mdown = null;
            return false;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            ScrollBar.Dispose();
            base.Dispose(disposing);
        }
    }

    public class TreeItemCollection : iCollection<TreeItem>
    {
        public TreeItemCollection(Tree it)
        {
            BindData(it);
        }
        public TreeItemCollection(TreeItem it)
        {
            BindData(it);
        }

        internal TreeItemCollection BindData(Tree it)
        {
            action = render =>
            {
                if (render) it.ChangeList();
                it.Invalidate();
            };
            return this;
        }

        internal TreeItemCollection BindData(TreeItem it)
        {
            action = render =>
            {
                if (it.PARENT == null) return;
                if (render) it.PARENT.ChangeList();
                it.PARENT.Invalidate();
            };
            return this;
        }
    }

    public class TreeItem
    {
        public TreeItem() { }
        public TreeItem(string text)
        {
            Text = text;
        }
        public TreeItem(string text, Image? icon)
        {
            Text = text;
            Icon = icon;
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// ID
        /// </summary>
        [Description("ID"), Category("数据"), DefaultValue(null)]
        public string? ID { get; set; }

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
                Invalidates();
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
                Invalidates();
            }
        }
        #region 加载动画
        ITask? ThreadLoading;
        bool loading = false;
        public float AnimationLoadingValue = 0;
        float AnimationLoadingWaveValue = 0;
        /// <summary>
        /// 加载状态
        /// </summary>
        [Description("加载状态"), Category("外观"), DefaultValue(false)]
        public bool Loading
        {
            get => loading;
            set
            {
                if (loading == value) return;
                loading = value;
                ThreadLoading?.Dispose();
                if (loading)
                {
                    if (PARENT == null) Invalidate();
                    else
                    {
                        ThreadLoading = new ITask(PARENT, i =>
                        {
                            AnimationLoadingWaveValue += 1;
                            if (AnimationLoadingWaveValue > 100) AnimationLoadingWaveValue = 0;
                            AnimationLoadingValue = i;
                            Invalidate();
                            return loading;
                        }, 10, 360, 10, () => Invalidate());
                    }
                }
                else Invalidate();
            }
        }

        #endregion
        /// <summary>
        /// 是否包含图片
        /// </summary>
        internal bool HasIcon => iconSvg != null || Icon != null || loading;

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("数据"), DefaultValue(null)]
        public string? Name { get; set; }

        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public string? Text
        {
            get => Localization.GetLangI(LocalizationText, text, new string?[] { "{id}", ID });
            set
            {
                if (text == value) return;
                text = value;
                Invalidates();
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        string? subTitle;
        /// <summary>
        /// 子标题
        /// </summary>
        [Description("子标题"), Category("外观"), DefaultValue(null)]
        public string? SubTitle
        {
            get => Localization.GetLangI(LocalizationSubTitle, subTitle, new string?[] { "{id}", ID });
            set
            {
                if (string.IsNullOrEmpty(value)) value = null;
                if (subTitle == value) return;
                subTitle = value;
                Invalidates();
            }
        }

        [Description("子标题"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationSubTitle { get; set; }

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
                Invalidates();
            }
        }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        internal TreeItemCollection? items;
        /// <summary>
        /// 获取列表中所有列表项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("子集合"), Category("外观")]
        public TreeItemCollection Sub
        {
            get
            {
                items ??= new TreeItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        #region 禁用

        bool enabled = true;
        /// <summary>
        /// 禁掉响应
        /// </summary>
        [Description("禁掉响应"), Category("行为"), DefaultValue(true)]
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value) return;
                enabled = value;
                Invalidate();
            }
        }

        #endregion

        #region 展开

        ITask? ThreadExpand;
        bool expand = false;
        /// <summary>
        /// 展开
        /// </summary>
        [Description("展开"), Category("行为"), DefaultValue(false)]
        public bool Expand
        {
            get => expand;
            set
            {
                if (expand == value) return;
                expand = value;
                PARENT?.OnAfterExpand(this, value);
                if (items != null && items.Count > 0)
                {
                    if (PARENT != null && PARENT.IsHandleCreated && Config.HasAnimation(nameof(Tree)))
                    {
                        ThreadExpand?.Dispose();
                        float oldval = -1;
                        if (ThreadExpand?.Tag is float oldv) oldval = oldv;
                        ExpandThread = true;
                        if (value)
                        {
                            int time = ExpandCount(this) * 10;
                            if (time < 100) time = 100;
                            else if (time > 1000) time = 1000;
                            int t = Animation.TotalFrames(10, time);
                            ThreadExpand = new ITask(false, 10, t, oldval, AnimationType.Ball, (i, val) =>
                            {
                                ExpandProg = val;
                                Invalidates();
                            }, () =>
                            {
                                ExpandProg = 1F;
                                ExpandThread = false;
                                Invalidates();
                            });
                        }
                        else
                        {
                            var t = Animation.TotalFrames(10, 200);
                            ThreadExpand = new ITask(true, 10, t, oldval, AnimationType.Ball, (i, val) =>
                            {
                                ExpandProg = val;
                                Invalidates();
                            }, () =>
                            {
                                ExpandProg = 1F;
                                ExpandThread = false;
                                Invalidates();
                            });
                        }
                    }
                    else
                    {
                        ExpandProg = 1F;
                        Invalidates();
                    }
                }
                else
                {
                    ExpandProg = 1F;
                    Invalidates();
                }
            }
        }

        internal int ExpandCount(TreeItem it)
        {
            int count = 0;
            if (it.items != null && it.items.Count > 0)
            {
                count += it.items.Count;
                foreach (var item in it.items)
                {
                    if (item.Expand) count += ExpandCount(item);
                }
            }
            return count;
        }

        [Description("是否可以展开"), Category("行为"), DefaultValue(null)]
        public bool? CanExpand { get; set; }

        internal bool ICanExpand => CanExpand ?? visible && items != null && items.Count > 0;

        #endregion

        #region 选中状态

        internal bool AnimationCheck = false;
        internal float AnimationCheckValue = 0;

        ITask? ThreadCheck;

        bool _checked = false;
        [Description("选中状态"), Category("行为"), DefaultValue(false)]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                PARENT?.OnCheckedChanged(this, value);
                OnCheck();
                CheckState = value ? CheckState.Checked : CheckState.Unchecked;
            }
        }

        internal CheckState checkStateOld = CheckState.Unchecked;
        CheckState checkState = CheckState.Unchecked;
        [Description("选中状态"), Category("行为"), DefaultValue(CheckState.Unchecked)]
        public CheckState CheckState
        {
            get => checkState;
            set
            {
                if (checkState == value) return;
                checkState = value;
                bool __checked = value == CheckState.Checked;
                if (_checked != __checked)
                {
                    _checked = __checked;
                    PARENT?.OnCheckedChanged(this, __checked);
                    OnCheck();
                }
                if (value != CheckState.Unchecked) checkStateOld = value;
            }
        }

        void OnCheck()
        {
            ThreadCheck?.Dispose();
            ThreadCheck = null;
            if (PARENT != null && PARENT.IsHandleCreated && (PARENTITEM == null || PARENTITEM.expand) && show && Config.HasAnimation(nameof(Tree)))
            {
                AnimationCheck = true;
                if (_checked)
                {
                    ThreadCheck = new ITask(PARENT, () =>
                    {
                        AnimationCheckValue = AnimationCheckValue.Calculate(0.2F);
                        if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                        Invalidate();
                        return true;
                    }, 20, () =>
                    {
                        AnimationCheck = false;
                        Invalidate();
                    });
                }
                else if (checkStateOld == CheckState.Checked && CheckState == CheckState.Indeterminate)
                {
                    AnimationCheck = false;
                    AnimationCheckValue = 1F;
                    Invalidate();
                }
                else
                {
                    ThreadCheck = new ITask(PARENT, () =>
                    {
                        AnimationCheckValue = AnimationCheckValue.Calculate(-0.2F);
                        if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                        Invalidate();
                        return true;
                    }, 20, () =>
                    {
                        AnimationCheck = false;
                        Invalidate();
                    });
                }
            }
            else
            {
                AnimationCheckValue = _checked ? 1F : 0F;
                Invalidate();
            }
        }

        #endregion

        #region 样式

        Color? fore;
        /// <summary>
        /// 文本颜色
        /// </summary>
        [Description("文本颜色"), Category("外观"), DefaultValue(null)]
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
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

        #endregion

        #region 方法

        public void Remove()
        {
            if (PARENTITEM == null) PARENT?.Items.Remove(this);
            else PARENTITEM.items?.Remove(this);
        }

        #endregion

        void Invalidate() => PARENT?.Invalidate();
        void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.ChangeList();
            PARENT.Invalidate();
        }

        internal float SubY { get; set; }
        internal float SubHeight { get; set; }
        internal float ExpandHeight { get; set; }
        internal float ExpandProg { get; set; }
        internal bool ExpandThread { get; set; }
        internal bool show { get; set; }
        internal bool Show { get; set; }

        bool hover = false;
        /// <summary>
        /// 是否移动
        /// </summary>
        internal bool Hover
        {
            get => hover;
            set
            {
                if (hover == value) return;
                hover = value;
                PARENT?.OnNodeMouseMove(this, value);
                if (Config.HasAnimation(nameof(Tree)))
                {
                    ThreadHover?.Dispose();
                    AnimationHover = true;
                    var t = Animation.TotalFrames(20, 200);
                    if (value)
                    {
                        ThreadHover = new ITask((i) =>
                        {
                            AnimationHoverValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate();
                            return true;
                        }, 20, t, () =>
                        {
                            AnimationHover = false;
                            AnimationHoverValue = 1;
                            Invalidate();
                        });
                    }
                    else
                    {
                        ThreadHover = new ITask((i) =>
                        {
                            AnimationHoverValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate();
                            return true;
                        }, 20, t, () =>
                        {
                            AnimationHover = false;
                            AnimationHoverValue = 0;
                            Invalidate();
                        });
                    }
                }
                else Invalidate();
            }
        }

        bool select = false;
        [Description("激活状态"), Category("行为"), DefaultValue(false)]
        public bool Select
        {
            get => select;
            set
            {
                if (select == value) return;
                if (value && PARENT != null)
                {
                    PARENT.USelect(false);
                    PARENT.SelectItem = this;
                }
                select = value;
                Invalidate();
            }
        }

        internal void SetSelect(bool value = true)
        {
            if (select == value) return;
            select = value;
            Invalidate();
        }

        public int Depth { get; private set; }
        internal Tree? PARENT { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TreeItem? PARENTITEM { get; set; }

        internal void SetRect(Canvas g, Font font, int depth, bool checkable, bool blockNode, bool has_sub, Rectangle _rect, int depth_gap, int icon_size, int gap)
        {
            Depth = depth;
            int x = _rect.X + gap + (depth_gap * depth), tmpx = x, usew = 0, y = _rect.Y + (_rect.Height - icon_size) / 2, ui = icon_size + gap;
            if (has_sub)
            {
                arrow_rect = new Rectangle(x, y, icon_size, icon_size);
                usew += ui;
                x += ui;
            }

            if (checkable)
            {
                check_rect = new Rectangle(x, y, icon_size, icon_size);
                usew += ui;
                x += ui;
            }

            if (HasIcon)
            {
                ico_rect = new Rectangle(x, y, icon_size, icon_size);
                usew += ui;
                x += ui;
            }
            var size = g.MeasureText(Text, font);
            int txt_w = size.Width + gap, txt_h = size.Height + gap, txt_y = _rect.Y + (_rect.Height - txt_h) / 2;
            if (subTitle == null)
            {
                usew += txt_w;
                if (blockNode)
                {
                    int rw = _rect.Width - x - gap;
                    txt_rect = new Rectangle(x, txt_y, txt_w, txt_h);
                    if (rw < txt_w) rw = txt_w;
                    rect = new Rectangle(x, txt_rect.Y, rw, txt_rect.Height);
                }
                else
                {
                    txt_rect = new Rectangle(x, txt_y, txt_w, txt_h);
                    rect = txt_rect;
                }
            }
            else
            {
                var sizesub = g.MeasureText(SubTitle, font);
                usew += txt_w + sizesub.Width + gap;
                if (blockNode)
                {
                    int rw = _rect.Width - x - gap;
                    txt_rect = new Rectangle(x, txt_y, txt_w, txt_h);
                    subtxt_rect = new Rectangle(txt_rect.Right, txt_rect.Y, sizesub.Width, txt_rect.Height);
                    txt_w += sizesub.Width;
                    if (rw < txt_w) rw = txt_w;
                    rect = new Rectangle(x, txt_rect.Y, rw, txt_rect.Height);
                }
                else
                {
                    txt_rect = new Rectangle(x, txt_y, txt_w, txt_h);
                    subtxt_rect = new Rectangle(txt_rect.Right, txt_rect.Y, sizesub.Width, txt_rect.Height);
                    rect = new Rectangle(x, txt_y, txt_w + sizesub.Width, txt_h);
                }
            }
            rect_all = new Rectangle(tmpx, rect.Y, usew, rect.Height);
            Show = true;
        }
        internal Rectangle rect_all { get; set; }
        internal Rectangle rect { get; set; }
        internal Rectangle arrow_rect { get; set; }

        internal TreeCType Contains(int x, int y, int sx, int sy, bool checkable, bool blockNode)
        {
            if (visible && enabled)
            {
                if (arrow_rect.Contains(x + sx, y + sy) && ICanExpand)
                {
                    Hover = true;
                    return TreeCType.Arrow;
                }
                else if (checkable && check_rect.Contains(x + sx, y + sy))
                {
                    Hover = true;
                    return TreeCType.Check;
                }
                else if (rect_all.Contains(x + sx, y + sy) || rect.Contains(x + sx, y + sy))
                {
                    Hover = true;
                    return TreeCType.Item;
                }
            }
            Hover = false;
            return TreeCType.None;
        }

        internal float AnimationHoverValue = 0;
        internal bool AnimationHover = false;
        ITask? ThreadHover;

        internal Rectangle check_rect { get; set; }
        internal Rectangle txt_rect { get; set; }
        internal Rectangle subtxt_rect { get; set; }
        internal Rectangle ico_rect { get; set; }

        public Rectangle Rect(string type = "", bool actual = true)
        {
            if (actual || PARENT == null) return Rect(type, 0, 0);
            else return Rect(type, PARENT.ScrollBar.ValueX, PARENT.ScrollBar.ValueY);
        }
        public Rectangle Rect(int x, int y) => Rect("", x, y);
        public Rectangle Rect(string type, int x = 0, int y = 0)
        {
            if (x > 0 || y > 0)
            {
                switch (type)
                {
                    case "Text":
                        return new Rectangle(txt_rect.X - x, txt_rect.Y - y, txt_rect.Width, txt_rect.Height);
                    case "SubTitle":
                        return new Rectangle(subtxt_rect.X - x, subtxt_rect.Y - y, subtxt_rect.Width, subtxt_rect.Height);
                    case "Checked":
                        return new Rectangle(check_rect.X - x, check_rect.Y - y, check_rect.Width, check_rect.Height);
                    case "Icon":
                        return new Rectangle(ico_rect.X - x, ico_rect.Y - y, ico_rect.Width, ico_rect.Height);
                    case "Arrow":
                        return new Rectangle(arrow_rect.X - x, arrow_rect.Y - y, arrow_rect.Width, arrow_rect.Height);
                    default:
                        return new Rectangle(rect.X - x, rect.Y - y, rect.Width, rect.Height);
                }
            }
            switch (type)
            {
                case "Text":
                    return txt_rect;
                case "SubTitle":
                    return subtxt_rect;
                case "Checked":
                    return check_rect;
                case "Icon":
                    return ico_rect;
                case "Arrow":
                    return arrow_rect;
                default:
                    return rect;
            }
        }

        public override string? ToString() => Text;
    }
}
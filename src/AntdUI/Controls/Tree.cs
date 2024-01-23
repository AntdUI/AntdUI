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
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Tree 树形控件
    /// </summary>
    /// <remarks>多层次的结构列表。</remarks>
    [Description("Tree 树形控件")]
    [ToolboxItem(true)]
    [DefaultEvent("SelectIndexChanged")]
    public class Tree : IControl
    {
        #region 属性

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

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
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

        /// <summary>
        /// 激活字体颜色
        /// </summary>
        [Description("激活字体颜色"), Category("外观"), DefaultValue(null)]
        public Color? ForeActive { get; set; }

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
        /// 是否节点占据一行
        /// </summary>
        [Description("是否节点占据一行"), Category("外观"), DefaultValue(false)]
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
                foreach (TreeItem it in items)
                {
                    it.Expand = value;
                    ExpandAll(it.Sub, value);
                }
            }
        }

        public List<TreeItem> GetCheckeds()
        {
            var list = new List<TreeItem>();
            if (items == null) return list;
            foreach (TreeItem it in items)
            {
                if (it.Checked) list.Add(it);
                if (it.Sub != null && it.Sub.Count > 0) GetCheckeds(ref list, it.Sub);
            }
            return list;
        }
        void GetCheckeds(ref List<TreeItem> list, TreeItemCollection items)
        {
            if (items == null) return;
            foreach (TreeItem it in list)
            {
                if (it.Checked) list.Add(it);
                if (it.Sub != null && it.Sub.Count > 0) GetCheckeds(ref list, it.Sub);
            }
        }

        #region 事件

        public delegate void SelectEventHandler(object sender, TreeItem item);
        /// <summary>
        /// Select 属性值更改时发生
        /// </summary>
        [Description("Select 属性值更改时发生"), Category("行为")]
        public event SelectEventHandler? SelectChanged = null;

        public delegate void CheckedEventHandler(object sender, TreeItem item, bool value);
        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category("行为")]
        public event CheckedEventHandler? CheckedChanged = null;

        /// <summary>
        /// 点击事件
        /// </summary>
        [Description("点击事件"), Category("行为")]
        public event SelectEventHandler? NodeMouseClick = null;

        /// <summary>
        /// 双击事件
        /// </summary>
        [Description("双击事件"), Category("行为")]
        public event SelectEventHandler? NodeMouseDoubleClick = null;

        internal void OnSelectChanged(TreeItem item)
        {
            SelectChanged?.Invoke(this, item);
        }
        internal void OnNodeMouseClick(TreeItem item)
        {
            NodeMouseClick?.Invoke(this, item);
        }
        internal void OnNodeMouseDoubleClick(TreeItem item)
        {
            NodeMouseDoubleClick?.Invoke(this, item);
        }
        internal void OnCheckedChanged(TreeItem item, bool value)
        {
            CheckedChanged?.Invoke(this, item, value);
        }

        #endregion

        #endregion

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

        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ChangeList();
            scrollX.SizeChange(rect);
            if (scrollX.Show)
            {
                scrollX.SizeChange(new Rectangle(rect.X, rect.Y, rect.Width - 20, rect.Height));
                scrollY.SizeChange(new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - 20));
            }
            else scrollY.SizeChange(rect);
            base.OnSizeChanged(e);
        }


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
                    ChangeList();
                    Invalidate();
                }
            }
        }
        internal Rectangle ChangeList()
        {
            var rect = ClientRectangle;
            if (pauseLayout || items == null || items.Count == 0) return rect;
            if (rect.Width == 0 || rect.Height == 0) return rect;

            float x = 0, y = 0;
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    var size = g.MeasureString(Config.NullText, Font);
                    float icon_size = size.Height, gap = icon_size / 2F;
                    int height = (int)Math.Ceiling(size.Height + gap * 2);
                    int gapI = (int)(gap / 2);
                    ChangeList(g, rect, null, Items, ref x, ref y, height, icon_size, gap, gapI, 0);
                }
            }
            scrollX.SetVrSize(x + 20, rect.Width);
            scrollY.SetVrSize(y, rect.Height);
            return rect;
        }
        void ChangeList(Graphics g, Rectangle rect, TreeItem? Parent, TreeItemCollection items, ref float x, ref float y, int height, float icon_size, float gap, int gapI, int depth)
        {
            foreach (TreeItem it in items)
            {
                it.PARENT = this;
                it.PARENTITEM = Parent;
                it.SetRect(g, Font, depth, checkable, blockNode, new RectangleF(0, y, rect.Width, height), icon_size, gap);
                if (it.txt_rect.Right > x) x = it.txt_rect.Right;
                if (it.Show && it.Visible)
                {
                    y += height + gapI;
                    if (it.CanExpand)
                    {
                        float y_item = y;

                        ChangeList(g, rect, it, it.Sub, ref x, ref y, height, icon_size, gap, gapI, depth + 1);

                        if ((it.Expand || it.ExpandThread) && it.ExpandProg > 0)
                        {
                            it.ExpandHeight = y - y_item;
                            y = y_item + it.ExpandHeight * it.ExpandProg;
                        }
                        else if (!it.Expand) y = y_item;
                    }
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scrollY.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion

        #region 渲染

        ScrollX scrollX;
        ScrollY scrollY;
        public Tree() { scrollX = new ScrollX(this); scrollY = new ScrollY(this); }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            var rect = ClientRectangle;
            if (rect.Width == 0 || rect.Height == 0) return;
            var g = e.Graphics.High();
            float sx = scrollX.Value, sy = scrollY.Value;
            g.TranslateTransform(-sx, -sy);
            Color color_fore = fore.HasValue ? fore.Value : Style.Db.TextBase, color_fore_active = ForeActive.HasValue ? ForeActive.Value : Style.Db.Primary, color_hover = BackHover.HasValue ? BackHover.Value : Style.Db.FillSecondary;
            float _radius = radius * Config.Dpi;
            PaintItem(g, rect, sy, Items, color_fore, color_fore_active, color_hover, _radius);
            g.ResetTransform();
            scrollX.Paint(g);
            scrollY.Paint(g);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        void PaintItem(Graphics g, Rectangle rect, float sy, TreeItemCollection items, Color fore, Color fore_active, Color hover, float radius)
        {
            foreach (TreeItem it in items)
            {
                it.show = it.Show && it.Visible && it.rect.Y > sy - it.rect.Height - (it.Expand ? it.ExpandHeight : 0) && it.rect.Bottom < scrollY.Value + scrollY.Height + it.rect.Height;
                if (it.show)
                {
                    PaintItem(g, it, fore, fore_active, hover, radius);
                    if (it.Expand && it.Sub != null)
                    {
                        PaintItem(g, rect, sy, it.Sub, fore, fore_active, hover, radius);
                        if (it.ExpandThread)
                        {
                            using (var brush = new SolidBrush(BackColor))
                            {
                                g.FillRectangle(brush, new RectangleF(rect.X, it.rect.Bottom + it.ExpandHeight * it.ExpandProg, rect.Width, it.ExpandHeight));
                            }
                        }
                    }
                }
            }
        }
        void PaintItem(Graphics g, TreeItem item, Color fore, Color fore_active, Color hover, float radius)
        {
            if (item.Select)
            {
                PaintBack(g, BackActive.HasValue ? BackActive.Value : Style.Db.PrimaryBg, item.rect, radius);
                if (item.CanExpand) PanintArrow(g, item, fore_active);
                using (var brush = new SolidBrush(fore_active))
                {
                    g.DrawString(item.Text, Font, brush, item.txt_rect, blockNode ? Helper.stringFormatLeft : Helper.stringFormatCenter4);
                }
            }
            else
            {
                if (item.AnimationHover)
                {
                    PaintBack(g, Color.FromArgb((int)(item.AnimationHoverValue * hover.A), hover), item.rect, radius);
                }
                else if (item.Hover) PaintBack(g, hover, item.rect, radius);
                if (item.CanExpand) PanintArrow(g, item, fore);
                using (var brush = new SolidBrush(item.Enabled ? fore : Style.Db.TextQuaternary))
                {
                    g.DrawString(item.Text, Font, brush, item.txt_rect, blockNode ? Helper.stringFormatLeft : Helper.stringFormatCenter4);
                }
            }
            if (checkable)
            {
                using (var path_check = item.check_rect.RoundPath(item.check_radius, false))
                {
                    if (item.Enabled)
                    {
                        if (item.AnimationCheck)
                        {
                            int a = (int)(255 * item.AnimationCheckValue);

                            if (item.CheckState == CheckState.Indeterminate || (item.checkStateOld == CheckState.Indeterminate && !item.Checked))
                            {
                                using (var brush = new Pen(Style.Db.BorderColor, 2F))
                                {
                                    g.DrawPath(brush, path_check);
                                }
                                using (var brush = new SolidBrush(Color.FromArgb(a, Style.Db.Primary)))
                                {
                                    g.FillRectangle(brush, PaintBlock(item.check_rect));
                                }
                            }
                            else
                            {
                                float dot = item.check_rect.Width * 0.3F;

                                using (var brush = new SolidBrush(Color.FromArgb(a, Style.Db.Primary)))
                                {
                                    g.FillPath(brush, path_check);
                                }
                                using (var brush = new Pen(Color.FromArgb(a, Style.Db.BgBase), 3F))
                                {
                                    g.DrawLines(brush, PaintArrow(item.check_rect));
                                }

                                if (item.Checked)
                                {
                                    float max = item.check_rect.Height + item.check_rect.Height * item.AnimationCheckValue;
                                    int a2 = (int)(100 * (1f - item.AnimationCheckValue));
                                    using (var brush = new SolidBrush(Color.FromArgb(a2, Style.Db.Primary)))
                                    {
                                        g.FillEllipse(brush, new RectangleF(item.check_rect.X + (item.check_rect.Width - max) / 2F, item.check_rect.Y + (item.check_rect.Height - max) / 2F, max, max));
                                    }
                                }
                                using (var brush = new Pen(Style.Db.Primary, 2F))
                                {
                                    g.DrawPath(brush, path_check);
                                }
                            }
                        }
                        else if (item.CheckState == CheckState.Indeterminate)
                        {
                            using (var brush = new Pen(Style.Db.BorderColor, 2F))
                            {
                                g.DrawPath(brush, path_check);
                            }
                            using (var brush = new SolidBrush(Style.Db.Primary))
                            {
                                g.FillRectangle(brush, PaintBlock(item.check_rect));
                            }
                        }
                        else if (item.Checked)
                        {
                            using (var brush = new SolidBrush(Style.Db.Primary))
                            {
                                g.FillPath(brush, path_check);
                            }
                            using (var brush = new Pen(Style.Db.BgBase, 3F))
                            {
                                g.DrawLines(brush, PaintArrow(item.check_rect));
                            }
                        }
                        else
                        {
                            using (var brush = new Pen(Style.Db.BorderColor, 2F))
                            {
                                g.DrawPath(brush, path_check);
                            }
                        }
                    }
                    else
                    {
                        using (var brush = new SolidBrush(Style.Db.FillQuaternary))
                        {
                            g.FillPath(brush, path_check);
                        }
                        if (item.CheckState == CheckState.Indeterminate)
                        {
                            using (var brush = new SolidBrush(Style.Db.TextQuaternary))
                            {
                                g.FillRectangle(brush, PaintBlock(item.check_rect));
                            }
                        }
                        else if (item.Checked)
                        {
                            using (var brush = new Pen(Style.Db.TextQuaternary, 3F))
                            {
                                g.DrawLines(brush, PaintArrow(item.check_rect));
                            }
                        }
                        using (var brush = new Pen(Style.Db.BorderColorDisable, 2F))
                        {
                            g.DrawPath(brush, path_check);
                        }
                    }
                }
            }
            if (item.Icon != null) g.DrawImage(item.Icon, item.ico_rect);
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

        void PanintArrow(Graphics g, TreeItem item, Color color)
        {
            float size = item.arr_rect.Width, size2 = size / 2F;
            g.TranslateTransform(item.arr_rect.X + size2, item.arr_rect.Y + size2);
            g.RotateTransform(-90F + item.ArrowProg);
            using (var pen = new Pen(color, 2F))
            {
                pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                g.DrawLines(pen, new RectangleF(-size2, -size2, item.arr_rect.Width, item.arr_rect.Height).TriangleLines(-1, .4F));
            }
            g.ResetTransform();
            g.TranslateTransform(-scrollX.Value, -scrollY.Value);
        }

        void PaintBack(Graphics g, Color color, RectangleF rect, float radius)
        {
            using (var brush = new SolidBrush(color))
            {
                if (round || radius > 0)
                {
                    using (var path = rect.RoundPath(radius, round))
                    {
                        g.FillPath(brush, path);
                    }
                }
                else g.FillRectangle(brush, rect);
            }
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (scrollY.MouseDown(e.Location) && scrollX.MouseDown(e.Location))
            {
                if (items == null || items.Count == 0) return;
                if (e.Button == MouseButtons.Left)
                {
                    foreach (TreeItem it in Items)
                    {
                        if (IMouseDown(e, it, null)) return;
                    }
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            scrollX.MouseUp(e.Location);
            scrollY.MouseUp(e.Location);
        }

        bool IMouseDown(MouseEventArgs e, TreeItem item, TreeItem? fitem)
        {
            bool can = item.CanExpand;
            int down = item.Contains(e.Location, scrollX.Value, scrollY.Value, checkable);
            if (down > 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (e.Clicks > 1) OnNodeMouseDoubleClick(item);
                    else OnNodeMouseClick(item);
                }
                if (blockNode)
                {
                    if (can) item.Expand = !item.Expand;
                    IUSelect();
                    item.Select = true;
                    OnSelectChanged(item);
                    Invalidate();
                }
                else if (down == 3 && item.Enabled)
                {
                    item.Checked = !item.Checked;
                    if (CheckStrictly)
                    {
                        SetCheck(item, item.Checked);

                        if (fitem != null)
                        {
                            int check_count = 0;
                            foreach (TreeItem sub in fitem.Sub)
                            { if (sub.Checked) check_count++; }
                            if (check_count > 0) fitem.CheckState = check_count == fitem.Sub.Count ? CheckState.Checked : CheckState.Indeterminate;
                            else fitem.CheckState = CheckState.Unchecked;
                        }
                    }
                }
                else if (down == 2 && can) item.Expand = !item.Expand;
                else
                {
                    IUSelect();
                    item.Select = true;
                    OnSelectChanged(item);
                    Invalidate();
                }
                return true;
            }
            if (can && item.Expand)
            {
                foreach (TreeItem sub in item.Sub)
                {
                    if (IMouseDown(e, sub, item)) return true;
                }
            }
            return false;
        }

        void SetCheck(TreeItem item, bool value)
        {
            if (item.Sub != null && item.Sub.Count > 0)
            {
                foreach (TreeItem it in item.Sub)
                {
                    it.Checked = value;
                    SetCheck(it, value);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (scrollY.MouseMove(e.Location) && scrollX.MouseMove(e.Location))
            {
                if (items == null || items.Count == 0) return;
                int hand = 0;
                foreach (TreeItem it in Items)
                {
                    IMouseMove(it, e.Location, ref hand);
                }
                SetCursor(hand > 0);
            }
            else ILeave();
        }

        void IMouseMove(TreeItem item, Point point, ref int hand)
        {
            if (item.show)
            {
                if (item.Contains(point, scrollX.Value, scrollY.Value, checkable) > 0) hand++;
                if (item.Sub != null)
                    foreach (TreeItem sub in item.Sub)
                    {
                        IMouseMove(sub, point, ref hand);
                    }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            scrollX.Leave();
            scrollY.Leave();
            ILeave();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            scrollX.Leave();
            scrollY.Leave();
            ILeave();
        }

        void ILeave()
        {
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int count = 0;
            foreach (TreeItem it in Items)
            {
                ILeave(it, ref count);
            }
            if (count > 0) Invalidate();
        }
        void ILeave(TreeItem item, ref int count)
        {
            if (item.Hover) count++;
            item.Hover = false;
            if (item.Sub != null)
                foreach (TreeItem sub in item.Sub)
                {
                    ILeave(sub, ref count);
                }
        }

        void IUSelect()
        {
            foreach (TreeItem it in Items)
            {
                IUSelect(it);
            }
        }
        void IUSelect(TreeItem item)
        {
            item.Select = false;
            if (item.Sub != null)
                foreach (TreeItem sub in item.Sub)
                {
                    IUSelect(sub);
                }
        }

        #endregion
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
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("数据"), DefaultValue(null)]
        public string? Name { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public string? Text { get; set; }


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
        /// 与对象关联的用户定义数据
        /// </summary>
        [Description("与对象关联的用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        TreeItemCollection? items;
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

        ITask? ThreadExpand = null;
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
                if (Sub != null && Sub.Count > 0)
                {
                    expand = value;
                    if (PARENT != null && PARENT.IsHandleCreated && Config.Animation)
                    {
                        ThreadExpand?.Dispose();
                        float oldval = -1;
                        if (ThreadExpand?.Tag is float oldv) oldval = oldv;
                        ExpandThread = true;
                        if (value)
                        {
                            var t = Animation.TotalFrames(10, ExpandCount(this) * 50);
                            ThreadExpand = new ITask(false, 10, t, oldval, AnimationType.Ball, (i, val) =>
                            {
                                ExpandProg = val;
                                ArrowProg = Animation.Animate(i, t, 90F, AnimationType.Ball);
                                Invalidates();
                            }, () =>
                            {
                                ArrowProg = 90F;
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
                                ArrowProg = 90F - Animation.Animate(i, t, 90F, AnimationType.Ball);
                                Invalidates();
                            }, () =>
                            {
                                ExpandProg = 1F;
                                ExpandThread = false;
                                ArrowProg = 0F;
                                Invalidates();
                            });
                        }
                    }
                    else
                    {
                        ExpandProg = 1F; ArrowProg = value ? 90F : 0F;
                        Invalidates();
                    }
                }
            }
        }

        internal int ExpandCount(TreeItem it)
        {
            int count = 0;
            if (it.Sub != null && it.Sub.Count > 0)
            {
                count += it.Sub.Count;
                foreach (TreeItem item in it.Sub)
                {
                    if (item.Expand) count += ExpandCount(item);
                }
            }
            return count;
        }

        [Description("是否可以展开"), Category("行为"), DefaultValue(false)]
        public bool CanExpand
        {
            get => visible && Sub != null && Sub.Count > 0;
        }

        #endregion

        #region 选中状态

        internal bool AnimationCheck = false;
        internal float AnimationCheckValue = 0;

        //ITask? ThreadHover = null;
        ITask? ThreadCheck = null;

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
            if (PARENT != null && PARENT.IsHandleCreated && (PARENTITEM == null || PARENTITEM.expand) && show && Config.Animation)
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
            else AnimationCheckValue = _checked ? 1F : 0F;
            Invalidate();
        }

        #endregion

        void Invalidate()
        {
            PARENT?.Invalidate();
        }
        void Invalidates()
        {
            if (PARENT != null)
            {
                PARENT.ChangeList();
                PARENT.Invalidate();
            }
        }

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
                if (Config.Animation)
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
        internal bool Select { get; set; }

        public int Depth { get; private set; }
        internal float ArrowProg { get; set; } = 0F;
        internal Tree? PARENT { get; set; }
        internal TreeItem? PARENTITEM { get; set; }

        internal void SetRect(Graphics g, Font font, int depth, bool checkable, bool blockNode, RectangleF _rect, float icon_size, float gap)
        {
            Depth = depth;
            rect = _rect;
            float x = _rect.X + gap + (icon_size * depth);
            arr_rect = new RectangleF(x, _rect.Y + (_rect.Height - icon_size) / 2, icon_size, icon_size);
            x += icon_size + gap;

            if (checkable)
            {
                check_radius = arr_rect.Height * .2F;
                check_rect = new RectangleF(x, arr_rect.Y, arr_rect.Width, arr_rect.Height);
                x += icon_size + gap;
            }

            if (Icon != null)
            {
                ico_rect = new RectangleF(x, arr_rect.Y, arr_rect.Width, arr_rect.Height);
                x += icon_size + gap;
            }

            var size = g.MeasureString(Text, font);
            size.Width += gap;
            size.Height += gap;
            txt_rect = new RectangleF(x, _rect.Y + (_rect.Height - size.Height) / 2, size.Width, size.Height);
            if (!blockNode) rect = txt_rect;
            Show = true;
        }
        internal RectangleF rect { get; set; }
        internal RectangleF arr_rect { get; set; }

        internal int Contains(Point point, float x, float y, bool checkable)
        {
            var p = new PointF(point.X + x, point.Y + y);
            if (rect.Contains(p))
            {
                Hover = true;
                return 1;
            }
            else if (arr_rect.Contains(p) && CanExpand)
            {
                Hover = rect.Contains(arr_rect);
                return 2;
            }
            else if (checkable && check_rect.Contains(p))
            {
                Hover = rect.Contains(arr_rect);
                return 3;
            }
            else
            {
                Hover = false;
                return 0;
            }
        }

        internal float AnimationHoverValue = 0;
        internal bool AnimationHover = false;
        ITask? ThreadHover = null;

        internal RectangleF check_rect { get; set; }
        internal float check_radius { get; set; }
        internal RectangleF txt_rect { get; set; }
        internal RectangleF ico_rect { get; set; }
    }
}
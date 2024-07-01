﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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

        #region 系统

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

        #endregion

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
        [Obsolete("使用 ForeColor 属性替代"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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

        #region 获取项

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
            var list = new List<TreeItem>();
            if (Indeterminate)
            {
                foreach (TreeItem it in items)
                {
                    if (it.CheckState != CheckState.Unchecked) list.Add(it);
                    if (it.Sub != null && it.Sub.Count > 0)
                    {
                        var list_sub = GetCheckeds(it.Sub, Indeterminate);
                        if (list_sub.Count > 0) list.AddRange(list_sub);
                    }
                }
            }
            else
            {
                foreach (TreeItem it in items)
                {
                    if (it.Checked) list.Add(it);
                    if (it.Sub != null && it.Sub.Count > 0)
                    {
                        var list_sub = GetCheckeds(it.Sub, Indeterminate);
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
        void SetCheckeds(TreeItemCollection items, bool check)
        {
            foreach (TreeItem it in items)
            {
                it.Checked = check;
                if (it.Sub != null && it.Sub.Count > 0) SetCheckeds(it.Sub, check);
            }
        }

        #endregion

        #region 事件

        public delegate void SelectEventHandler(object sender, MouseEventArgs args, TreeItem item, Rectangle rect);
        public delegate void HoverEventHandler(object sender, TreeItem item, Rectangle rect, bool hover);
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
        /// 点击项事件
        /// </summary>
        [Description("点击项事件"), Category("行为")]
        public event SelectEventHandler? NodeMouseClick = null;

        /// <summary>
        /// 双击项事件
        /// </summary>
        [Description("双击项事件"), Category("行为")]
        public event SelectEventHandler? NodeMouseDoubleClick = null;

        /// <summary>
        /// 移动项事件
        /// </summary>
        [Description("移动项事件"), Category("行为")]
        public event HoverEventHandler? NodeMouseMove = null;
        internal void OnNodeMouseMove(TreeItem item, bool hover)
        {
            if (NodeMouseMove == null) return;
            int sx = scrollBar.ValueX, sy = scrollBar.ValueY;
            NodeMouseMove(this, item, new Rectangle(item.txt_rect.X, item.txt_rect.Y - sy, item.txt_rect.Width, item.txt_rect.Height), hover);
        }

        internal void OnSelectChanged(TreeItem item, MouseEventArgs args)
        {
            if (SelectChanged == null) return;
            int sx = scrollBar.ValueX, sy = scrollBar.ValueY;
            SelectChanged(this, args, item, new Rectangle(item.txt_rect.X, item.txt_rect.Y - sy, item.txt_rect.Width, item.txt_rect.Height));
        }
        internal void OnNodeMouseClick(TreeItem item, MouseEventArgs args)
        {
            if (NodeMouseClick == null) return;
            int sx = scrollBar.ValueX, sy = scrollBar.ValueY;
            NodeMouseClick(this, args, item, new Rectangle(item.txt_rect.X, item.txt_rect.Y - sy, item.txt_rect.Width, item.txt_rect.Height));
        }
        internal void OnNodeMouseDoubleClick(TreeItem item, MouseEventArgs args)
        {
            if (NodeMouseDoubleClick == null) return;
            int sx = scrollBar.ValueX, sy = scrollBar.ValueY;
            NodeMouseDoubleClick(this, args, item, new Rectangle(item.txt_rect.X, item.txt_rect.Y - sy, item.txt_rect.Width, item.txt_rect.Height));
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
            }
        }

        #endregion

        #region 布局

        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ChangeList();
            scrollBar.SizeChange(rect);
            base.OnSizeChanged(e);
        }

        internal Rectangle ChangeList()
        {
            var rect = ClientRectangle;
            if (pauseLayout || items == null || items.Count == 0) return rect;
            if (rect.Width == 0 || rect.Height == 0) return rect;

            int x = 0, y = 0;
            Helper.GDI(g =>
            {
                var size = g.MeasureString(Config.NullText, Font);
                int icon_size = (int)(size.Height), gap = icon_size / 2;
                int height = (int)Math.Ceiling(size.Height + gap * 2);
                int gapI = (int)(gap / 2);
                ChangeList(g, rect, null, Items, ref x, ref y, height, icon_size, gap, gapI, 0, true);
            });
            scrollBar.SetVrSize(x, y);
            return rect;
        }
        void ChangeList(Graphics g, Rectangle rect, TreeItem? Parent, TreeItemCollection items, ref int x, ref int y, int height, int icon_size, int gap, int gapI, int depth, bool expand)
        {
            foreach (TreeItem it in items)
            {
                it.PARENT = this;
                it.PARENTITEM = Parent;
                it.SetRect(g, Font, depth, checkable, blockNode, new Rectangle(0, y, rect.Width, height), icon_size, gap);
                if (expand && it.txt_rect.Right > x) x = it.txt_rect.Right;
                if (it.Show && it.Visible)
                {
                    y += height + gapI;
                    if (it.CanExpand)
                    {
                        int y_item = y;
                        ChangeList(g, rect, it, it.Sub, ref x, ref y, height, icon_size, gap, gapI, depth + 1, expand ? it.Expand : false);
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

        ScrollBar scrollBar;
        public Tree()
        {
            scrollBar = new ScrollBar(this, true, true);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            var rect = ClientRectangle;
            if (rect.Width == 0 || rect.Height == 0) return;
            var g = e.Graphics.High();
            int sx = scrollBar.ValueX, sy = scrollBar.ValueY;
            g.TranslateTransform(-sx, -sy);
            Color color_fore = fore ?? Style.Db.TextBase, color_fore_active = ForeActive ?? Style.Db.Primary, color_hover = BackHover ?? Style.Db.FillSecondary;
            float _radius = radius * Config.Dpi;
            PaintItem(g, rect, sx, sy, Items, color_fore, color_fore_active, color_hover, _radius);
            g.ResetTransform();
            scrollBar.Paint(g);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        void PaintItem(Graphics g, Rectangle rect, int sx, int sy, TreeItemCollection items, Color fore, Color fore_active, Color hover, float radius)
        {
            foreach (TreeItem it in items)
            {
                it.show = it.Show && it.Visible && it.rect.Y > sy - rect.Height - (it.Expand ? it.SubHeight : 0) && it.rect.Bottom < sy + rect.Height + it.rect.Height;
                if (it.show)
                {
                    PaintItem(g, it, fore, fore_active, hover, radius, sx, sy);
                    if (it.Expand && it.Sub != null && it.Sub.Count > 0)
                    {
                        PaintItem(g, rect, sx, sy, it.Sub, fore, fore_active, hover, radius);
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

        StringFormat sf_center = Helper.SF_Ellipsis();
        void PaintItem(Graphics g, TreeItem item, Color fore, Color fore_active, Color hover, float radius, int sx, int sy)
        {
            if (item.Select)
            {
                if (blockNode)
                {
                    g.ResetTransform();
                    g.TranslateTransform(0, -sy);
                    PaintBack(g, BackActive ?? Style.Db.PrimaryBg, item.rect, radius);
                    g.TranslateTransform(-sx, 0);
                }
                else PaintBack(g, BackActive ?? Style.Db.PrimaryBg, item.rect, radius);
                if (item.CanExpand) PanintArrow(g, item, fore_active, sx, sy);
                using (var brush = new SolidBrush(fore_active))
                {
                    g.DrawString(item.Text, Font, brush, item.txt_rect, blockNode ? Helper.stringFormatLeft : sf_center);
                }
                if (item.SubTitle != null)
                {
                    using (var brush = new SolidBrush(Style.Db.TextTertiary))
                    {
                        g.DrawString(item.SubTitle, Font, brush, item.subtxt_rect, Helper.stringFormatLeft);
                    }
                }
            }
            else
            {
                if (blockNode)
                {
                    g.ResetTransform();
                    g.TranslateTransform(0, -sy);
                    if (item.AnimationHover)
                    {
                        PaintBack(g, Helper.ToColorN(item.AnimationHoverValue, hover), item.rect, radius);
                    }
                    else if (item.Hover) PaintBack(g, hover, item.rect, radius);
                    g.TranslateTransform(-sx, 0);
                }
                else
                {
                    if (item.AnimationHover)
                    {
                        PaintBack(g, Helper.ToColorN(item.AnimationHoverValue, hover), item.rect, radius);
                    }
                    else if (item.Hover) PaintBack(g, hover, item.rect, radius);
                }
                if (item.CanExpand) PanintArrow(g, item, fore, sx, sy);
                using (var brush = new SolidBrush(item.Enabled ? fore : Style.Db.TextQuaternary))
                {
                    g.DrawString(item.Text, Font, brush, item.txt_rect, blockNode ? Helper.stringFormatLeft : sf_center);
                }
                if (item.SubTitle != null)
                {
                    using (var brush = new SolidBrush(Style.Db.TextTertiary))
                    {
                        g.DrawString(item.SubTitle, Font, brush, item.subtxt_rect, Helper.stringFormatLeft);
                    }
                }
            }
            if (checkable)
            {
                using (var path_check = Helper.RoundPath(item.check_rect, item.check_radius, false))
                {
                    if (item.Enabled)
                    {
                        if (item.AnimationCheck)
                        {
                            var alpha = 255 * item.AnimationCheckValue;

                            if (item.CheckState == CheckState.Indeterminate || (item.checkStateOld == CheckState.Indeterminate && !item.Checked))
                            {
                                using (var brush = new Pen(Style.Db.BorderColor, 2F))
                                {
                                    g.DrawPath(brush, path_check);
                                }
                                using (var brush = new SolidBrush(Helper.ToColor(alpha, Style.Db.Primary)))
                                {
                                    g.FillRectangle(brush, PaintBlock(item.check_rect));
                                }
                            }
                            else
                            {
                                float dot = item.check_rect.Width * 0.3F;

                                using (var brush = new SolidBrush(Helper.ToColor(alpha, Style.Db.Primary)))
                                {
                                    g.FillPath(brush, path_check);
                                }
                                using (var brush = new Pen(Helper.ToColor(alpha, Style.Db.BgBase), 3F))
                                {
                                    g.DrawLines(brush, PaintArrow(item.check_rect));
                                }

                                if (item.Checked)
                                {
                                    float max = item.check_rect.Height + item.check_rect.Height * item.AnimationCheckValue, alpha2 = 100 * (1F - item.AnimationCheckValue);
                                    using (var brush = new SolidBrush(Helper.ToColor(alpha2, Style.Db.Primary)))
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
            else if (item.IconSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(item.IconSvg, item.ico_rect, fore))
                {
                    if (_bmp != null) g.DrawImage(_bmp, item.ico_rect);
                }
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

        void PanintArrow(Graphics g, TreeItem item, Color color, int sx, int sy)
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
            g.TranslateTransform(-sx, -sy);
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
            if (scrollBar.MouseDownY(e.Location) && scrollBar.MouseDownX(e.Location))
            {
                if (items == null || items.Count == 0) return;
                foreach (TreeItem it in Items)
                {
                    if (IMouseDown(e, it, null)) return;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            scrollBar.MouseUpY();
            scrollBar.MouseUpX();
        }

        bool IMouseDown(MouseEventArgs e, TreeItem item, TreeItem? fitem)
        {
            bool can = item.CanExpand;
            int down = item.Contains(e.Location, blockNode ? 0 : scrollBar.ValueX, scrollBar.ValueY, checkable);
            if (down > 0)
            {
                if (e.Clicks > 1) OnNodeMouseDoubleClick(item, e);
                else OnNodeMouseClick(item, e);
                if (blockNode)
                {
                    if (can) item.Expand = !item.Expand;
                    item.Select = true;
                    OnSelectChanged(item, e);
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
                    item.Select = true;
                    OnSelectChanged(item, e);
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
            if (scrollBar.MouseMoveY(e.Location) && scrollBar.MouseMoveX(e.Location))
            {
                if (items == null || items.Count == 0) return;
                int hand = 0;
                foreach (TreeItem it in Items) IMouseMove(it, e.Location, ref hand);
                SetCursor(hand > 0);
            }
            else ILeave();
        }

        void IMouseMove(TreeItem item, Point point, ref int hand)
        {
            if (item.show)
            {
                if (item.Contains(point, blockNode ? 0 : scrollBar.ValueX, scrollBar.ValueY, checkable) > 0) hand++;
                if (item.Sub != null) foreach (TreeItem sub in item.Sub) IMouseMove(sub, point, ref hand);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            scrollBar.Leave();
            ILeave();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            scrollBar.Leave();
            ILeave();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scrollBar.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
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

        internal void IUSelect()
        {
            foreach (TreeItem it in Items) IUSelect(it);
        }
        void IUSelect(TreeItem item)
        {
            item.Select = false;
            if (item.Sub != null) foreach (TreeItem sub in item.Sub) IUSelect(sub);
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

    public class TreeItem : NotifyProperty
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
                OnPropertyChanged("Icon");
            }
        }

        string? iconSvg = null;
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
                OnPropertyChanged("IconSvg");
            }
        }

        /// <summary>
        /// 是否包含图片
        /// </summary>
        internal bool HasIcon
        {
            get => iconSvg != null || Icon != null;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("数据"), DefaultValue(null)]
        public string? Name { get; set; }

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

        string? subTitle = null;
        /// <summary>
        /// 子标题
        /// </summary>
        [Description("子标题"), Category("外观"), DefaultValue(null)]
        public string? SubTitle
        {
            get => subTitle;
            set
            {
                if (string.IsNullOrEmpty(value)) value = null;
                if (subTitle == value) return;
                subTitle = value;
                Invalidates();
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
                Invalidates();
            }
        }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
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
                if (expand == value) return;
                expand = value;
                if (Sub != null && Sub.Count > 0)
                {
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
                        ExpandProg = 1F;
                        ArrowProg = value ? 90F : 0F;
                        Invalidates();
                    }
                }
                else
                {
                    expand = false;
                    ExpandProg = 1F;
                    ArrowProg = 0F;
                    Invalidates();
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

        bool select = false;
        [Description("激活状态"), Category("行为"), DefaultValue(false)]
        public bool Select
        {
            get => select;
            set
            {
                if (select == value) return;
                if (value) PARENT?.IUSelect();
                select = value;
                Invalidate();
            }
        }

        public int Depth { get; private set; }
        internal float ArrowProg { get; set; } = 0F;
        internal Tree? PARENT { get; set; }
        public TreeItem? PARENTITEM { get; set; }

        internal void SetRect(Graphics g, Font font, int depth, bool checkable, bool blockNode, Rectangle _rect, int icon_size, int gap)
        {
            Depth = depth;
            rect = _rect;
            int x = _rect.X + gap + (icon_size * depth);
            arr_rect = new Rectangle(x, _rect.Y + (_rect.Height - icon_size) / 2, icon_size, icon_size);
            x += icon_size + gap;

            if (checkable)
            {
                check_radius = arr_rect.Height * .2F;
                check_rect = new Rectangle(x, arr_rect.Y, arr_rect.Width, arr_rect.Height);
                x += icon_size + gap;
            }

            if (HasIcon)
            {
                ico_rect = new Rectangle(x, arr_rect.Y, arr_rect.Width, arr_rect.Height);
                x += icon_size + gap;
            }

            var size = g.MeasureString(Text, font);
            int width = (int)Math.Ceiling(size.Width += gap);
            int height = (int)Math.Ceiling(size.Height += gap);
            txt_rect = new Rectangle(x, _rect.Y + (_rect.Height - height) / 2, width, height);
            if (SubTitle != null)
            {
                var sizesub = g.MeasureString(SubTitle, font);
                subtxt_rect = new Rectangle(txt_rect.Right, txt_rect.Y, (int)Math.Ceiling(sizesub.Width), txt_rect.Height);
                if (!blockNode) rect = new Rectangle(txt_rect.X, txt_rect.Y, txt_rect.Width + subtxt_rect.Width, subtxt_rect.Height);
            }
            else if (!blockNode) rect = txt_rect;

            Show = true;
        }
        internal Rectangle rect { get; set; }
        internal Rectangle arr_rect { get; set; }

        internal int Contains(Point point, int x, int y, bool checkable)
        {
            var p = new Point(point.X + x, point.Y + y);
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

        internal Rectangle check_rect { get; set; }
        internal float check_radius { get; set; }
        internal Rectangle txt_rect { get; set; }
        internal Rectangle subtxt_rect { get; set; }
        internal Rectangle ico_rect { get; set; }
    }
}
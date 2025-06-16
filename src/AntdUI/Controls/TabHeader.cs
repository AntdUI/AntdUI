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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI.Controls
{
    /// <summary>
    /// 自定义标签头部控件
    /// </summary>
    [ToolboxItem(true)]
    public class TabHeader : PageHeader
    {
        // 标签数据集合
        private List<TagtabItem> _tabs = new List<TagtabItem>();

        // 标签尺寸和样式参数
        private int _tabHeight = 30;
        private int _tabPadding = 10;
        private int _closeButtonSize = 16;
        private int _closeButtonPadding = 5;
        private int _iconSize = 16;
        private int _iconPadding = 5;
        private int _cornerRadius = 8; //圆角半径

        // 自定义标签绘制内边距（不影响子控件布局）
        private Padding _tabDrawingPadding = new Padding(60, 3, 80, 3);

        // 颜色设置
        private Color _selectedTabColor => AntdUI.Style.Get(Colour.PrimaryActive);// = Color.FromArgb(240, 240, 240);
        private Color _hoverTabColor => Style.Get(Colour.HoverBg);// Color.FromArgb(230, 230, 230);
        private Color _normalTabColor => Style.Get(Colour.BgContainer);// Color.White;
        private Color _closeButtonColor = Color.Gray;
        private Color _closeButtonHoverColor = Color.Red;
        private Color _borderColor => Style.Get(Colour.BorderColor);
        private Color _selectTabForeColor => Style.Get(Colour.TextBase);

        // 当前鼠标悬停的关闭按钮索引
        private int _hoveredCloseButtonIndex = -1;

        // 滚动相关
        private int _scrollOffset = 0;
        private int _totalTabWidth = 0;
        private bool _canScrollLeft = false;
        private bool _canScrollRight = false;

        // 事件委托
        public event EventHandler<TabChangedEventArgs> TabChanged;
        public event EventHandler<TabCloseEventArgs> TabClosing;

        /// <summary>
        /// 公有属性
        /// </summary>
        public List<TagtabItem> Items => _tabs;

        public TabHeader()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            // 选中第一个标签
            if (_tabs.Count > 0)
                _tabs[0].IsSelected = true;
        }

        /// <summary>
        /// 自定义标签绘制内边距属性
        /// </summary>
        [Category("外观"), Description("自定义标签绘制内边距属性")]
        [DefaultValue(typeof(Padding), "60,3,80,3")]
        public Padding TabDrawingPadding
        {
            get { return _tabDrawingPadding; }
            set
            {
                _tabDrawingPadding = value;
                RecalculateLayout();
                Invalidate();
            }
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="item"></param>
        public void AddTab(TagtabItem item)
        {
            _tabs.Add(item);
            RecalculateLayout();
            Invalidate();
        }
        /// <summary>
        /// 插入标签
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void InsertTab(int index, TagtabItem item)
        {
            _tabs.Insert(index, item);
            RecalculateLayout();
            Invalidate();
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        public void AddTab(string text, Image icon = null)
        {
            _tabs.Add(new TagtabItem(text, icon));
            RecalculateLayout();
            Invalidate();
        }

        /// <summary>
        /// 插入标签
        /// </summary>
        /// <param name="index"></param>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        public void InsertTab(int index, string text, Image icon = null)
        {
            _tabs.Insert(index, new TagtabItem(text, icon));
            RecalculateLayout();
            Invalidate();
        }

        /// <summary>
        /// 移除标签
        /// </summary>
        /// <param name="index"></param>
        public void RemoveTab(int index)
        {
            if (index >= 0 && index < _tabs.Count)
            {
                _tabs.RemoveAt(index);
                RecalculateLayout();
                Invalidate();
            }
        }

        /// <summary>
        /// 选中指定索引的标签
        /// </summary>
        /// <param name="index"></param>
        public void SelectTab(int index)
        {
            if (index >= 0 && index < _tabs.Count)
            {
                for (int i = 0; i < _tabs.Count; i++)
                {
                    _tabs[i].IsSelected = i == index;
                }

                // 触发标签变更事件
                TabChanged?.Invoke(this, new TabChangedEventArgs(index, _tabs[index]));

                // 确保选中的标签可见
                EnsureTabVisible(index);

                Invalidate();
            }
        }

        /// <summary>
        /// 确保指定索引的标签可见（滚动到视野内）
        /// </summary>
        /// <param name="index"></param>
        private void EnsureTabVisible(int index)
        {
            if (index < 0 || index >= _tabs.Count)
                return;

            Rectangle tabBounds = _tabs[index].Bounds;

            // 如果标签在左侧不可见区域
            if (tabBounds.Left < _tabDrawingPadding.Left)
            {
                _scrollOffset += tabBounds.Left - _tabDrawingPadding.Left;
            }
            // 如果标签在右侧不可见区域
            else if (tabBounds.Right > Width - _tabDrawingPadding.Right)
            {
                _scrollOffset -= Width - _tabDrawingPadding.Right - tabBounds.Right;
            }

            // 确保滚动偏移量在有效范围内
            _scrollOffset = Math.Max(0, Math.Min(_scrollOffset, _totalTabWidth - (Width - _tabDrawingPadding.Left - _tabDrawingPadding.Right)));

            UpdateScrollState();
        }

        /// <summary>
        /// 计算所有标签的布局和尺寸
        /// </summary>
        private void RecalculateLayout()
        {
            _totalTabWidth = 0;

            using (Graphics g = CreateGraphics())
            {
                for (int i = 0; i < _tabs.Count; i++)
                {
                    TagtabItem tab = _tabs[i];
                    SizeF textSize = g.MeasureString(tab.Text, Font);

                    // 计算标签宽度：文字宽度 + 左右内边距 + 关闭按钮宽度 + 关闭按钮内边距 + 图标宽度 + 图标内边距
                    int tabWidth = (int)Math.Ceiling(textSize.Width) +
                                  _tabPadding * 2 +
                                  _closeButtonSize +
                                  _closeButtonPadding;

                    // 如果有图标，增加图标宽度和间距
                    if (tab.Icon != null)
                    {
                        tabWidth += _iconSize + _iconPadding;
                    }

                    // 设置标签位置和大小（使用自定义绘制内边距）
                    tab.Bounds = new Rectangle(
                        _tabDrawingPadding.Left + _totalTabWidth - _scrollOffset,
                        _tabDrawingPadding.Top,
                        tabWidth,
                        _tabHeight
                    );

                    // 设置关闭按钮位置
                    tab.CloseButtonBounds = new Rectangle(
                        tab.Bounds.Right - _closeButtonSize - _closeButtonPadding,
                        tab.Bounds.Top + (tab.Bounds.Height - _closeButtonSize) / 2,
                        _closeButtonSize,
                        _closeButtonSize
                    );

                    _totalTabWidth += tabWidth;
                }
            }

            UpdateScrollState();
        }

        /// <summary>
        /// 更新滚动状态
        /// </summary>
        private void UpdateScrollState()
        {
            _canScrollLeft = _scrollOffset > 0;
            _canScrollRight = _scrollOffset < _totalTabWidth - (Width - _tabDrawingPadding.Left - _tabDrawingPadding.Right);
        }

        /// <summary>
        /// 向左滚动
        /// </summary>
        private void ScrollLeft()
        {
            if (_canScrollLeft)
            {
                _scrollOffset = Math.Max(0, _scrollOffset - 50);
                RecalculateLayout();
                Invalidate();
            }
        }

        /// <summary>
        /// 向右滚动
        /// </summary>
        private void ScrollRight()
        {
            if (_canScrollRight)
            {
                _scrollOffset = Math.Min(_totalTabWidth - (Width - _tabDrawingPadding.Left - _tabDrawingPadding.Right), _scrollOffset + 50);
                RecalculateLayout();
                Invalidate();
            }
        }

        /// <summary>
        /// 处理鼠标移动事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // 重置所有标签的悬停状态
            for (int i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].IsHovered = false;
            }

            _hoveredCloseButtonIndex = -1;

            // 检测鼠标悬停在哪个标签上
            for (int i = 0; i < _tabs.Count; i++)
            {
                TagtabItem tab = _tabs[i];

                // 检查是否悬停在标签上
                if (tab.Bounds.Contains(e.Location))
                {
                    tab.IsHovered = true;

                    // 检查是否悬停在关闭按钮上
                    if (tab.CloseButtonBounds.Contains(e.Location))
                    {
                        _hoveredCloseButtonIndex = i;
                    }

                    break;
                }
            }

            Invalidate();
        }

        /// <summary>
        /// 处理鼠标离开事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            // 重置所有标签的悬停状态
            for (int i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].IsHovered = false;
            }

            _hoveredCloseButtonIndex = -1;

            Invalidate();
        }

        /// <summary>
        /// 处理鼠标点击事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                // 检查是否点击了某个标签的关闭按钮
                for (int i = 0; i < _tabs.Count; i++)
                {
                    if (_tabs[i].CloseButtonBounds.Contains(e.Location))
                    {
                        // 触发标签关闭事件
                        var args = new TabCloseEventArgs(i);
                        TabClosing?.Invoke(this, args);

                        if (!args.Cancel)
                        {
                            RemoveTab(i);

                            // 如果关闭的是当前选中标签，自动选择下一个标签
                            if (i == _tabs.Count)
                            {
                                SelectTab(Math.Max(0, _tabs.Count - 1));
                            }
                        }

                        return;
                    }
                }

                // 检查是否点击了某个标签（非关闭按钮区域）
                for (int i = 0; i < _tabs.Count; i++)
                {
                    if (_tabs[i].Bounds.Contains(e.Location) &&
                        !_tabs[i].CloseButtonBounds.Contains(e.Location))
                    {
                        SelectTab(i);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 处理鼠标滚轮事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta > 0)
            {
                ScrollLeft();
            }
            else
            {
                ScrollRight();
            }
        }

        /// <summary>
        /// 处理大小改变事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RecalculateLayout();
            Invalidate();
        }

        /// <summary>
        /// 绘制控件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 设置剪裁区域，使用自定义绘制内边距
            Rectangle clipRect = new Rectangle(
                _tabDrawingPadding.Left,
                _tabDrawingPadding.Top,
                Width - _tabDrawingPadding.Left - _tabDrawingPadding.Right,
                _tabHeight
            );

            // 保存当前剪裁区域
            Region originalClip = g.Clip;

            try
            {
                // 设置新的剪裁区域
                g.SetClip(clipRect);

                // 绘制每个标签
                for (int i = 0; i < _tabs.Count; i++)
                {
                    // 只绘制在可见区域内的标签
                    if (_tabs[i].Bounds.Right > _tabDrawingPadding.Left &&
                        _tabs[i].Bounds.Left < Width - _tabDrawingPadding.Right)
                    {
                        DrawTab(g, _tabs[i], i);
                    }
                }
            }
            finally
            {
                // 恢复原始剪裁区域
                g.Clip = originalClip;
            }

            // 绘制底部边框，使用自定义绘制内边距
            using (Pen borderPen = new Pen(_selectedTabColor))
            {
                g.DrawLine(
                    borderPen,
                    _tabDrawingPadding.Left,
                    _tabDrawingPadding.Top + _tabHeight - 1,
                    Width - _tabDrawingPadding.Right,
                    _tabDrawingPadding.Top + _tabHeight - 1
                );
            }
        }

        /// <summary>
        /// 绘制单个标签（包含圆角）
        /// </summary>
        private void DrawTab(Graphics g, TagtabItem tab, int index)
        {
            // 计算在剪裁区域内的标签边界
            Rectangle visibleBounds = tab.Bounds;

            if (tab.Bounds.Left < _tabDrawingPadding.Left)
            {
                visibleBounds.X = _tabDrawingPadding.Left;
                visibleBounds.Width = tab.Bounds.Right - _tabDrawingPadding.Left;
            }

            if (tab.Bounds.Right > Width - _tabDrawingPadding.Right)
            {
                visibleBounds.Width = Width - _tabDrawingPadding.Right - visibleBounds.Left;
            }

            // 绘制标签背景（圆角矩形）
            using (GraphicsPath path = GetRoundedRectanglePath(visibleBounds, _cornerRadius))
            {
                // 确定标签背景颜色
                Color tabColor;
                if (tab.IsSelected)
                {
                    tabColor = _selectedTabColor;
                }
                else if (tab.IsHovered)
                {
                    tabColor = _hoverTabColor;
                }
                else
                {
                    tabColor = _normalTabColor;
                }

                using (SolidBrush brush = new SolidBrush(tabColor))
                {
                    g.FillPath(brush, path);
                }

                // 绘制标签边框
                using (Pen pen = new Pen(_borderColor))
                {
                    // 使用路径绘制边框，确保圆角处也有边框
                    g.DrawPath(pen, path);
                }
            }

            // 计算文本绘制区域
            Rectangle textRect = new Rectangle(
                visibleBounds.Left + _tabPadding,
                visibleBounds.Top,
                visibleBounds.Width - _tabPadding * 2 - _closeButtonSize - _closeButtonPadding,
                visibleBounds.Height
            );

            if (tab.Icon != null)
            {
                textRect.X += _iconSize + _iconPadding;
                textRect.Width -= _iconSize + _iconPadding;
            }

            if (textRect.Width > 10)
            {
                using var textBrush = new SolidBrush(_selectTabForeColor);
                using var sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags = StringFormatFlags.NoWrap;

                g.DrawString(tab.Text, Font, textBrush, textRect, sf);
            }

            // 绘制图标
            if (tab.Icon != null)
            {
                Rectangle iconRect = new Rectangle(
                    visibleBounds.Left + _tabPadding,
                    visibleBounds.Top + (visibleBounds.Height - _iconSize) / 2,
                    _iconSize,
                    _iconSize
                );

                if (iconRect.Right <= Width - _tabDrawingPadding.Right)
                {
                    g.DrawImage(tab.Icon, iconRect);
                }
            }

            // 绘制关闭按钮
            if (tab.CloseButtonBounds.Left >= _tabDrawingPadding.Left &&
                tab.CloseButtonBounds.Right <= Width - _tabDrawingPadding.Right)
            {
                DrawCloseButton(g, tab);
            }
        }

        /// <summary>
        /// 创建圆角矩形路径
        /// </summary>
        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;
            //左侧直线
            path.AddLine(rect.X, rect.Bottom, rect.X, radius);
            // 左上角
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // 右上角
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            //右侧直线
            path.AddLine(rect.Right, diameter, rect.Right, rect.Bottom);

            path.CloseFigure();
            return path;
        }
        /// <summary>
        /// 绘制关闭按钮
        /// </summary>
        /// <param name="g"></param>
        /// <param name="tab"></param>
        private void DrawCloseButton(Graphics g, TagtabItem tab)
        {
            // 仅当鼠标悬停在关闭按钮上时才显示红色，不考虑标签是否被选中
            Color buttonColor = _hoveredCloseButtonIndex == _tabs.IndexOf(tab)
                ? _closeButtonHoverColor
                : _closeButtonColor;

            // 绘制关闭按钮背景
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(200, 200, 200)))
            {
                g.FillEllipse(brush, tab.CloseButtonBounds);
            }

            // 绘制关闭按钮的叉号
            using (Pen pen = new Pen(buttonColor, 2))
            {
                int offset = 4;
                g.DrawLine(
                    pen,
                    tab.CloseButtonBounds.Left + offset,
                    tab.CloseButtonBounds.Top + offset,
                    tab.CloseButtonBounds.Right - offset,
                    tab.CloseButtonBounds.Bottom - offset
                );

                g.DrawLine(
                    pen,
                    tab.CloseButtonBounds.Right - offset,
                    tab.CloseButtonBounds.Top + offset,
                    tab.CloseButtonBounds.Left + offset,
                    tab.CloseButtonBounds.Bottom - offset
                );
            }
        }
    }

    /// <summary>
    /// 标签页数据结构
    /// </summary>
    public class TagtabItem
    {
        public string Text { get; set; }
        public Image Icon { get; set; }
        public bool IsSelected { get; set; }
        public bool IsHovered { get; set; }
        public object Tag { get; set; }
        public Rectangle Bounds { get; set; }
        public Rectangle CloseButtonBounds { get; set; }

        public TagtabItem(string text, Image icon = null)
        {
            Text = text;
            Icon = icon;
            IsSelected = false;
            IsHovered = false;
        }
    }

    /// <summary>
    /// 标签变更事件参数
    /// </summary>
    /// <param name="tabIndex"></param>
    /// <param name="item"></param>
    public class TabChangedEventArgs : EventArgs
    {
        public TabChangedEventArgs(int tabIndex, TagtabItem item)
        {
            this.TabIndex = tabIndex;
            this.Item = item;
        }
        /// <summary>
        /// 标签Index
        /// </summary>
        public int TabIndex { get; private set; }

        public TagtabItem Item { get; private set; }
    }

    /// <summary>
    /// 标签关闭事件参数
    /// </summary>
    /// <param name="tabIndex"></param>
    public class TabCloseEventArgs : EventArgs
    {
        public TabCloseEventArgs(int tabIndex)
        {
            this.TabIndex = tabIndex;
        }
        /// <summary>
        /// 标签Index
        /// </summary>
        public int TabIndex { get; private set; }
        /// <summary>
        /// 取消操作
        /// </summary>
        public bool Cancel { get; set; } = false;
    }
}

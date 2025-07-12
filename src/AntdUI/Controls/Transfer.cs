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
using System.Linq;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Transfer 穿梭框
    /// </summary>
    /// <remarks>双栏穿梭选择框，用于在两个区域之间移动元素。</remarks>
    [Description("Transfer 穿梭框")]
    [ToolboxItem(true)]
    [DefaultEvent("TransferChanged")]
    public class Transfer : IControl
    {
        #region 属性

        /// <summary>
        /// 左侧列表标题
        /// </summary>
        [Description("左侧列表标题"), Category("数据"), DefaultValue("源列表"), Localizable(true)]
        public string SourceTitle { get; set; } = "源列表";

        /// <summary>
        /// 右侧列表标题
        /// </summary>
        [Description("右侧列表标题"), Category("数据"), DefaultValue("目标列表"), Localizable(true)]
        public string TargetTitle { get; set; } = "目标列表";

        ///// <summary>
        ///// 是否显示搜索框
        ///// </summary>
        //[Description("是否显示搜索框"), Category("行为"), DefaultValue(false)]
        //public bool ShowSearch { get; set; } = false;

        /// <summary>
        /// 是否显示全选勾选框
        /// </summary>
        [Description("是否显示全选勾选框"), Category("行为"), DefaultValue(true)]
        public bool ShowSelectAll { get; set; } = true;

        /// <summary>
        /// 是否单向模式（只能从左到右）
        /// </summary>
        [Description("是否单向模式（只能从左到右）"), Category("行为"), DefaultValue(false)]
        public bool OneWay { get; set; } = false;

        /// <summary>
        /// 列表项高度
        /// </summary>
        [Description("列表项高度"), Category("外观"), DefaultValue(32)]
        public int ItemHeight { get; set; } = 32;

        /// <summary>
        /// 列表项圆角
        /// </summary>
        [Description("列表项圆角"), Category("外观"), DefaultValue(4)]
        public int ItemRadius { get; set; } = 4;

        /// <summary>
        /// 列表框圆角
        /// </summary>
        [Description("列表框圆角"), Category("外观"), DefaultValue(6)]
        public int PanelRadius { get; set; } = 6;

        /// <summary>
        /// 列表框边框颜色
        /// </summary>
        [Description("列表框边框颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor { get; set; }

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

        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor { get; set; }

        /// <summary>
        /// 激活文字颜色
        /// </summary>
        [Description("激活文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ForeActive { get; set; }

        /// <summary>
        /// 按钮文字颜色
        /// </summary>
        [Description("按钮文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonForeColor { get; set; }

        /// <summary>
        /// 按钮背景颜色
        /// </summary>
        [Description("按钮背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonBackColor { get; set; }

        /// <summary>
        /// 按钮悬停背景颜色
        /// </summary>
        [Description("按钮悬停背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonBackHover { get; set; }

        /// <summary>
        /// 按钮激活背景颜色
        /// </summary>
        [Description("按钮激活背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonBackActive { get; set; }

        /// <summary>
        /// 按钮禁用背景颜色
        /// </summary>
        [Description("按钮禁用背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonBackDisable { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        [Description("数据源"), Category("数据"), DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<TransferItem> Items { get; set; } = new List<TransferItem>();

        #endregion

        #region 事件

        /// <summary>
        /// 穿梭框选项变化事件参数
        /// </summary>
        public class TransferEventArgs : EventArgs
        {
            /// <summary>
            /// 源列表项
            /// </summary>
            public List<TransferItem> SourceItems { get; }

            /// <summary>
            /// 目标列表项
            /// </summary>
            public List<TransferItem> TargetItems { get; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="sourceItems">源列表项</param>
            /// <param name="targetItems">目标列表项</param>
            public TransferEventArgs(List<TransferItem> sourceItems, List<TransferItem> targetItems)
            {
                SourceItems = sourceItems;
                TargetItems = targetItems;
            }
        }

        /// <summary>
        /// 穿梭框选项变化事件
        /// </summary>
        [Description("穿梭框选项变化事件"), Category("行为")]
        public event EventHandler<TransferEventArgs>? TransferChanged;

        /// <summary>
        /// 触发穿梭框选项变化事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnTransferChanged(TransferEventArgs e)
        {
            TransferChanged?.Invoke(this, e);
        }

        /// <summary>
        /// 搜索事件参数
        /// </summary>
        public class SearchEventArgs : EventArgs
        {
            /// <summary>
            /// 搜索文本
            /// </summary>
            public string SearchText { get; }

            /// <summary>
            /// 是否为源列表搜索
            /// </summary>
            public bool IsSource { get; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="searchText">搜索文本</param>
            /// <param name="isSource">是否为源列表搜索</param>
            public SearchEventArgs(string searchText, bool isSource)
            {
                SearchText = searchText;
                IsSource = isSource;
            }
        }

        /// <summary>
        /// 搜索事件
        /// </summary>
        [Description("搜索事件"), Category("行为")]
        public event EventHandler<SearchEventArgs>? Search;

        /// <summary>
        /// 触发搜索事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnSearch(SearchEventArgs e)
        {
            Search?.Invoke(this, e);
        }

        #endregion

        #region 构造函数

        public Transfer()
        {
            SetStyle(ControlStyles.Selectable, false);
            MinimumSize = new Size(300, 200);
            Size = new Size(500, 300);

            // 初始化动画变量
            hover_source = null;
            hover_target = null;
            hover_to_right = null;
            hover_to_left = null;
            active_to_right = null;
            active_to_left = null;
        }

        #endregion

        #region 私有变量

        private List<TransferItem> sourceItems = new List<TransferItem>();
        private List<TransferItem> targetItems = new List<TransferItem>();
        private List<TransferItem> sourceFilteredItems = new List<TransferItem>();
        private List<TransferItem> targetFilteredItems = new List<TransferItem>();

        private string sourceSearchText = "";
        private string targetSearchText = "";

        private bool sourceSelectAll = false;
        private bool targetSelectAll = false;

        private Rectangle sourceRect;
        private Rectangle targetRect;
        private Rectangle toRightRect;
        private Rectangle toLeftRect;

        private int sourceScrollOffset = 0;
        private int targetScrollOffset = 0;

        private ITask? hover_source, hover_target;
        private ITask? hover_to_right, hover_to_left;
        private ITask? active_to_right, active_to_left;

        private bool hover_source_btn = false;
        private bool hover_target_btn = false;
        private bool hover_to_right_btn = false;
        private bool hover_to_left_btn = false;
        private bool active_to_right_btn = false;
        private bool active_to_left_btn = false;

        #endregion

        #region 重写方法

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            InitializeItems();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            CalculateLayout();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // 绘制背景
            PaintBackground(g);

            // 绘制源列表
            PaintSourceList(g);

            // 绘制目标列表
            PaintTargetList(g);

            // 绘制操作按钮
            PaintOperationButtons(g);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool refresh = false;

            // 检查源列表悬停
            if (sourceRect.Contains(e.Location))
            {
                if (!hover_source_btn)
                {
                    hover_source_btn = true;
                    hover_source?.Dispose();
                    hover_source = new ITask(this, () =>
                    {
                        // 这里添加动画逻辑
                        Invalidate();
                        return true;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                    refresh = true;
                }
            }
            else if (hover_source_btn)
            {
                hover_source_btn = false;
                hover_source?.Dispose();
                hover_source = new ITask(this, () =>
                {
                    // 这里添加动画逻辑
                    Invalidate();
                    return true;
                }, 10, () =>
                {
                    Invalidate();
                });
                refresh = true;
            }

            // 检查目标列表悬停
            if (targetRect.Contains(e.Location))
            {
                if (!hover_target_btn)
                {
                    hover_target_btn = true;
                    hover_target?.Dispose();
                    hover_target = new ITask(this, () =>
                    {
                        // 这里添加动画逻辑
                        Invalidate();
                        return true;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                    refresh = true;
                }
            }
            else if (hover_target_btn)
            {
                hover_target_btn = false;
                hover_target?.Dispose();
                hover_target = new ITask(this, () =>
                {
                    // 这里添加动画逻辑
                    Invalidate();
                    return true;
                }, 10, () =>
                {
                    Invalidate();
                });
                refresh = true;
            }

            // 检查向右按钮悬停
            if (toRightRect.Contains(e.Location))
            {
                if (!hover_to_right_btn)
                {
                    hover_to_right_btn = true;
                    hover_to_right?.Dispose();
                    hover_to_right = new ITask(this, () =>
                    {
                        // 这里添加动画逻辑
                        Invalidate();
                        return true;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                    refresh = true;
                }
            }
            else if (hover_to_right_btn)
            {
                hover_to_right_btn = false;
                hover_to_right?.Dispose();
                hover_to_right = new ITask(this, () =>
                {
                    // 这里添加动画逻辑
                    Invalidate();
                    return true;
                }, 10, () =>
                {
                    Invalidate();
                });
                refresh = true;
            }

            // 检查向左按钮悬停
            if (toLeftRect.Contains(e.Location) && !OneWay)
            {
                if (!hover_to_left_btn)
                {
                    hover_to_left_btn = true;
                    hover_to_left?.Dispose();
                    hover_to_left = new ITask(this, () =>
                    {
                        // 这里添加动画逻辑
                        Invalidate();
                        return true;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                    refresh = true;
                }
            }
            else if (hover_to_left_btn)
            {
                hover_to_left_btn = false;
                hover_to_left?.Dispose();
                hover_to_left = new ITask(this, () =>
                {
                    // 这里添加动画逻辑
                    Invalidate();
                    return true;
                }, 10, () =>
                {
                    Invalidate();
                });
                refresh = true;
            }

            if (refresh) Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                // 向右按钮点击
                if (toRightRect.Contains(e.Location))
                {
                    active_to_right_btn = true;
                    active_to_right?.Dispose();
                    active_to_right = new ITask(this, () =>
                    {
                        // 这里添加动画逻辑
                        Invalidate();
                        return true;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                }

                // 向左按钮点击
                if (toLeftRect.Contains(e.Location) && !OneWay)
                {
                    active_to_left_btn = true;
                    active_to_left?.Dispose();
                    active_to_left = new ITask(this, () =>
                    {
                        // 这里添加动画逻辑
                        Invalidate();
                        return true;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                }

                // 处理源列表项点击
                if (sourceRect.Contains(e.Location))
                {
                    HandleListItemClick(e.Location, sourceFilteredItems, sourceScrollOffset, true);
                }

                // 处理目标列表项点击
                if (targetRect.Contains(e.Location))
                {
                    HandleListItemClick(e.Location, targetFilteredItems, targetScrollOffset, false);
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                bool refresh = false;

                // 向右按钮释放
                if (active_to_right_btn)
                {
                    active_to_right_btn = false;
                    active_to_right?.Dispose();
                    active_to_right = new ITask(this, () =>
                    {
                        // 这里添加动画逻辑
                        Invalidate();
                        return true;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                    refresh = true;

                    // 执行向右移动操作
                    if (toRightRect.Contains(e.Location))
                    {
                        MoveSelectedItemsToRight();
                    }
                }

                // 向左按钮释放
                if (active_to_left_btn)
                {
                    active_to_left_btn = false;
                    active_to_left?.Dispose();
                    active_to_left = new ITask(this, () =>
                    {
                        // 这里添加动画逻辑
                        Invalidate();
                        return true;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                    refresh = true;

                    // 执行向左移动操作
                    if (toLeftRect.Contains(e.Location) && !OneWay)
                    {
                        MoveSelectedItemsToLeft();
                    }
                }

                if (refresh) Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            // 确定滚动方向和步长
            int scrollDelta = e.Delta > 0 ? -1 : 1;

            // 检查鼠标是否在源列表区域
            if (sourceRect.Contains(e.Location))
            {
                // 计算最大滚动偏移量
                int visibleItems = (sourceRect.Height - 80) / ItemHeight; // 减去标题、搜索框和全选区域的高度
                int maxScrollOffset = Math.Max(0, sourceFilteredItems.Count - visibleItems);

                // 更新滚动偏移量
                sourceScrollOffset = Math.Max(0, Math.Min(maxScrollOffset, sourceScrollOffset + scrollDelta));
                Invalidate();
            }
            // 检查鼠标是否在目标列表区域
            else if (targetRect.Contains(e.Location))
            {
                // 计算最大滚动偏移量
                int visibleItems = (targetRect.Height - 80) / ItemHeight; // 减去标题、搜索框和全选区域的高度
                int maxScrollOffset = Math.Max(0, targetFilteredItems.Count - visibleItems);

                // 更新滚动偏移量
                targetScrollOffset = Math.Max(0, Math.Min(maxScrollOffset, targetScrollOffset + scrollDelta));
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            bool refresh = false;

            if (hover_source_btn)
            {
                hover_source_btn = false;
                hover_source?.Dispose();
                hover_source = new ITask(this, () =>
                {
                    // 这里添加动画逻辑
                    Invalidate();
                    return true;
                }, 10, () =>
                {
                    Invalidate();
                });
                refresh = true;
            }

            if (hover_target_btn)
            {
                hover_target_btn = false;
                hover_target?.Dispose();
                hover_target = new ITask(this, () =>
                {
                    // 这里添加动画逻辑
                    Invalidate();
                    return true;
                }, 10, () =>
                {
                    Invalidate();
                });
                refresh = true;
            }

            if (hover_to_right_btn)
            {
                hover_to_right_btn = false;
                hover_to_right?.Dispose();
                hover_to_right = new ITask(this, () =>
                {
                    // 这里添加动画逻辑
                    Invalidate();
                    return true;
                }, 10, () =>
                {
                    Invalidate();
                });
                refresh = true;
            }

            if (hover_to_left_btn)
            {
                hover_to_left_btn = false;
                hover_to_left?.Dispose();
                hover_to_left = new ITask(this, () =>
                {
                    // 这里添加动画逻辑
                    Invalidate();
                    return true;
                }, 10, () =>
                {
                    Invalidate();
                });
                refresh = true;
            }

            if (refresh) Invalidate();
        }

        #endregion

        #region 资源释放

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                hover_source?.Dispose();
                hover_target?.Dispose();
                hover_to_right?.Dispose();
                hover_to_left?.Dispose();
                active_to_right?.Dispose();
                active_to_left?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region 私有方法

        private void HandleListItemClick(Point location, List<TransferItem> items, int scrollOffset, bool isSource)
        {
            // 计算列表区域
            Rectangle rect = isSource ? sourceRect : targetRect;
            int contentTop = rect.Y + 40; // 标题高度

            // 如果显示搜索框，检查是否点击了搜索框
            //if (ShowSearch)
            //{
            //    var searchRect = new Rectangle(rect.X + 10, contentTop, rect.Width - 20, 30);

            //    if (searchRect.Contains(location))
            //    {
            //        // 弹出输入框让用户输入搜索文本
            //        using (var inputBox = new TextBox
            //        {
            //            Text = isSource ? sourceSearchText : targetSearchText,
            //            Location = new Point(searchRect.X + 30, searchRect.Y),
            //            Size = new Size(searchRect.Width - 35, searchRect.Height),
            //            BorderStyle = BorderStyle.None,
            //            Font = new Font(Font.FontFamily, 9)
            //        })
            //        {
            //            // 添加到控件并聚焦
            //            Controls.Add(inputBox);
            //            inputBox.Focus();

            //            // 添加文本变更事件处理
            //            inputBox.TextChanged += (sender, e) =>
            //            {
            //                if (isSource)
            //                {
            //                    sourceSearchText = inputBox.Text;
            //                }
            //                else
            //                {
            //                    targetSearchText = inputBox.Text;
            //                }

            //                // 应用过滤
            //                ApplyFilter();

            //                // 触发搜索事件
            //                OnSearch(new SearchEventArgs(inputBox.Text, isSource));
            //            };

            //            // 添加失去焦点事件处理
            //            inputBox.LostFocus += (sender, e) =>
            //            {
            //                Controls.Remove(inputBox);
            //                inputBox.Dispose();
            //                Invalidate();
            //            };
            //        }

            //        return;
            //    }

            //    contentTop += 40;
            //}

            // 如果显示全选复选框，检查是否点击了全选复选框
            if (ShowSelectAll)
            {
                var checkboxRect = new Rectangle(rect.X + 10, contentTop, 20, 20);
                var checkboxTextRect = new Rectangle(rect.X + 35, contentTop, rect.Width - 45, 20);
                var selectAllRect = new Rectangle(rect.X + 10, contentTop, rect.Width - 20, 20);

                if (selectAllRect.Contains(location))
                {
                    // 切换全选状态
                    bool newSelectAllState = isSource ? !sourceSelectAll : !targetSelectAll;

                    // 更新全选状态
                    if (isSource)
                    {
                        sourceSelectAll = newSelectAllState;
                        // 更新所有项的选中状态
                        foreach (var item in items)
                        {
                            item.Selected = newSelectAllState;
                        }
                    }
                    else
                    {
                        targetSelectAll = newSelectAllState;
                        // 更新所有项的选中状态
                        foreach (var item in items)
                        {
                            item.Selected = newSelectAllState;
                        }
                    }

                    Invalidate();
                    return;
                }

                contentTop += 30;
            }

            // 计算列表区域
            int listHeight = rect.Height - (contentTop - rect.Y) - 10;
            var listRect = new Rectangle(rect.X + 10, contentTop, rect.Width - 20, listHeight);
            int visibleItems = listHeight / ItemHeight;

            // 计算滚动条
            bool showScrollbar = items.Count * ItemHeight > listHeight;
            int scrollbarWidth = showScrollbar ? 6 : 0;

            // 检查点击是否在列表项区域内
            if (location.X >= listRect.X && location.X <= listRect.Right - scrollbarWidth)
            {
                // 计算点击的项索引
                int clickedIndex = scrollOffset + (location.Y - listRect.Y) / ItemHeight;

                // 确保索引有效
                if (clickedIndex >= 0 && clickedIndex < items.Count)
                {
                    // 切换选中状态
                    items[clickedIndex].Selected = !items[clickedIndex].Selected;

                    // 更新全选状态
                    if (isSource)
                    {
                        sourceSelectAll = items.All(i => i.Selected);
                    }
                    else
                    {
                        targetSelectAll = items.All(i => i.Selected);
                    }

                    Invalidate();
                }
            }
        }

        private void InitializeItems()
        {
            sourceItems.Clear();
            targetItems.Clear();

            // 初始化源列表和目标列表
            foreach (var item in Items)
            {
                if (item.IsTarget)
                {
                    targetItems.Add(item);
                }
                else
                {
                    sourceItems.Add(item);
                }
            }

            // 应用过滤
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            // 过滤源列表
            if (string.IsNullOrEmpty(sourceSearchText))
            {
                sourceFilteredItems = new List<TransferItem>(sourceItems);
            }
            else
            {
                sourceFilteredItems = sourceItems
                    .Where(item => item.Text.IndexOf(sourceSearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            // 过滤目标列表
            if (string.IsNullOrEmpty(targetSearchText))
            {
                targetFilteredItems = new List<TransferItem>(targetItems);
            }
            else
            {
                targetFilteredItems = targetItems
                    .Where(item => item.Text.IndexOf(targetSearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            // 重置滚动位置
            sourceScrollOffset = 0;
            targetScrollOffset = 0;

            Invalidate();
        }

        private void CalculateLayout()
        {
            int panelWidth = (Width - 80) / 2;
            int panelHeight = Height;

            // 源列表区域
            sourceRect = new Rectangle(0, 0, panelWidth, panelHeight);

            // 目标列表区域
            targetRect = new Rectangle(Width - panelWidth, 0, panelWidth, panelHeight);

            // 操作按钮区域
            int buttonSize = 36;
            int buttonY = (Height - (buttonSize * 2 + 10)) / 2;
            toRightRect = new Rectangle((Width - buttonSize) / 2, buttonY, buttonSize, buttonSize);
            toLeftRect = new Rectangle((Width - buttonSize) / 2, buttonY + buttonSize + 10, buttonSize, buttonSize);
        }

        private void PaintBackground(Graphics g)
        {
            // 绘制背景
            using (var brush = new SolidBrush(BackColor))
            {
                g.FillRectangle(brush, ClientRectangle);
            }
        }

        private void PaintSourceList(Graphics g)
        {
            // 绘制源列表面板
            PaintListPanel(g, sourceRect, SourceTitle, sourceFilteredItems, sourceScrollOffset, sourceSelectAll, true);
        }

        private void PaintTargetList(Graphics g)
        {
            // 绘制目标列表面板
            PaintListPanel(g, targetRect, TargetTitle, targetFilteredItems, targetScrollOffset, targetSelectAll, false);
        }

        private void PaintListPanel(Graphics g, Rectangle rect, string title, List<TransferItem> items, int scrollOffset, bool selectAll, bool isSource)
        {
            // 获取颜色
            var borderColor = BorderColor ?? Style.Db.BorderColor;
            var foreColor = ForeColor ?? Style.Db.Text;
            var backHover = BackHover ?? Style.Db.FillSecondary;
            var backActive = BackActive ?? Style.Db.FillTertiary;
            var foreActive = ForeActive ?? Style.Db.Primary;

            // 绘制面板边框和背景
            using (var path = new GraphicsPath())
            {
                path.AddRoundedRectangle(rect, PanelRadius);
                using (var brush = new SolidBrush(Style.Db.FillQuaternary))
                {
                    g.FillPath(brush, path);
                }
                using (var pen = new Pen(borderColor))
                {
                    g.DrawPath(pen, path);
                }
            }

            // 绘制标题
            var titleRect = new Rectangle(rect.X + 10, rect.Y + 10, rect.Width - 20, 24);
            using (var brush = new SolidBrush(foreColor))
            using (var font = new Font(Font.FontFamily, 10, FontStyle.Bold))
            {
                g.DrawString(title, font, brush, titleRect, new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
            }

            // 绘制选中项数量
            int selectedCount = items.Count(i => i.Selected);
            string countText = $"{selectedCount}/{items.Count}";
            var countRect = new Rectangle(rect.X + rect.Width - 60, rect.Y + 10, 50, 24);
            using (var brush = new SolidBrush(foreColor))
            using (var font = new Font(Font.FontFamily, 9))
            {
                g.DrawString(countText, font, brush, countRect, new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
            }

            // 绘制搜索框
            int contentTop = rect.Y + 40;
            //if (ShowSearch)
            //{
            //    var searchRect = new Rectangle(rect.X + 10, contentTop, rect.Width - 20, 30);
            //    using (var path = new GraphicsPath())
            //    {
            //        path.AddRoundedRectangle(searchRect, 4);
            //        using (var brush = new SolidBrush(Style.Db.FillTertiary))
            //        {
            //            g.FillPath(brush, path);
            //        }
            //        using (var pen = new Pen(borderColor))
            //        {
            //            g.DrawPath(pen, path);
            //        }
            //    }

            //    // 绘制搜索图标
            //    var searchIconRect = new Rectangle(searchRect.X + 5, searchRect.Y + 5, 20, 20);
            //    g.DrawSvg("search", searchIconRect, foreColor);

            //    // 绘制搜索文本
            //    var searchTextRect = new Rectangle(searchRect.X + 30, searchRect.Y, searchRect.Width - 35, searchRect.Height);
            //    string searchText = isSource ? sourceSearchText : targetSearchText;
            //    if (string.IsNullOrEmpty(searchText))
            //    {
            //        using (var brush = new SolidBrush(Color.FromArgb(150, foreColor.R, foreColor.G, foreColor.B)))
            //        using (var font = new Font(Font.FontFamily, 9))
            //        {
            //            g.DrawString("搜索", font, brush, searchTextRect, new StringFormat { LineAlignment = StringAlignment.Center });
            //        }
            //    }
            //    else
            //    {
            //        using (var brush = new SolidBrush(foreColor))
            //        using (var font = new Font(Font.FontFamily, 9))
            //        {
            //            g.DrawString(searchText, font, brush, searchTextRect, new StringFormat { LineAlignment = StringAlignment.Center });
            //        }
            //    }

            //    contentTop += 40;
            //}

            // 绘制全选复选框
            if (ShowSelectAll)
            {
                var checkboxRect = new Rectangle(rect.X + 10, contentTop, 20, 20);
                var checkboxTextRect = new Rectangle(rect.X + 35, contentTop, rect.Width - 45, 20);

                // 绘制复选框
                using (var path = new GraphicsPath())
                {
                    path.AddRoundedRectangle(checkboxRect, 3);
                    if (selectAll)
                    {
                        using (var brush = new SolidBrush(Style.Db.Primary))
                        {
                            g.FillPath(brush, path);
                        }
                        g.DrawSvg("CheckOutlined", checkboxRect, Color.White);
                    }
                    else
                    {
                        using (var brush = new SolidBrush(Style.Db.FillTertiary))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    using (var pen = new Pen(selectAll ? Style.Db.Primary : borderColor))
                    {
                        g.DrawPath(pen, path);
                    }
                }

                // 绘制全选文本
                using (var brush = new SolidBrush(foreColor))
                using (var font = new Font(Font.FontFamily, 9))
                {
                    g.DrawString("全选", font, brush, checkboxTextRect, new StringFormat { LineAlignment = StringAlignment.Center });
                }

                contentTop += 30;
            }

            // 绘制列表项
            int listHeight = rect.Height - (contentTop - rect.Y) - 10;
            var listRect = new Rectangle(rect.X + 10, contentTop, rect.Width - 20, listHeight);
            int visibleItems = listHeight / ItemHeight;

            // 计算滚动条
            int totalHeight = items.Count * ItemHeight;
            bool showScrollbar = totalHeight > listHeight;
            int scrollbarWidth = 6;
            var scrollbarRect = new Rectangle(listRect.Right - scrollbarWidth, listRect.Y, scrollbarWidth, listRect.Height);
            int thumbHeight = showScrollbar ? Math.Max(30, (int)((float)listHeight / totalHeight * listHeight)) : 0;
            int maxScrollOffset = Math.Max(0, items.Count - visibleItems);
            int currentScrollOffset = Math.Min(maxScrollOffset, Math.Max(0, isSource ? sourceScrollOffset : targetScrollOffset));
            int thumbY = showScrollbar ? scrollbarRect.Y + (int)((float)currentScrollOffset / maxScrollOffset * (scrollbarRect.Height - thumbHeight)) : 0;
            var thumbRect = new Rectangle(scrollbarRect.X, thumbY, scrollbarRect.Width, thumbHeight);

            // 绘制列表项
            for (int i = currentScrollOffset; i < Math.Min(items.Count, currentScrollOffset + visibleItems); i++)
            {
                var item = items[i];
                int itemY = listRect.Y + (i - currentScrollOffset) * ItemHeight;
                var itemRect = new Rectangle(listRect.X, itemY, listRect.Width - (showScrollbar ? scrollbarWidth + 4 : 0), ItemHeight);

                // 绘制项背景
                using (var path = new GraphicsPath())
                {
                    path.AddRoundedRectangle(itemRect, ItemRadius);
                    if (item.Selected)
                    {
                        using (var brush = new SolidBrush(backActive))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    else if (item.Hover)
                    {
                        using (var brush = new SolidBrush(backHover))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }

                // 绘制复选框
                var checkboxRect = new Rectangle(itemRect.X + 5, itemRect.Y + (ItemHeight - 16) / 2, 16, 16);
                using (var path = new GraphicsPath())
                {
                    path.AddRoundedRectangle(checkboxRect, 3);
                    if (item.Selected)
                    {
                        using (var brush = new SolidBrush(Style.Db.Primary))
                        {
                            g.FillPath(brush, path);
                        }
                        g.DrawSvg("CheckOutlined", checkboxRect, Color.White);
                    }
                    else
                    {
                        using (var brush = new SolidBrush(Style.Db.FillTertiary))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    using (var pen = new Pen(item.Selected ? Style.Db.Primary : borderColor))
                    {
                        g.DrawPath(pen, path);
                    }
                }

                // 绘制项文本
                var textRect = new Rectangle(itemRect.X + 26, itemRect.Y, itemRect.Width - 31, itemRect.Height);
                using (var brush = new SolidBrush(item.Selected ? foreActive : foreColor))
                using (var font = new Font(Font.FontFamily, 9))
                {
                    g.DrawString(item.Text, font, brush, textRect, new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
                }
            }

            // 绘制滚动条
            if (showScrollbar)
            {
                using (var brush = new SolidBrush(Color.FromArgb(50, Style.Db.Text)))
                {
                    g.FillRoundedRectangle(brush, scrollbarRect, 3);
                }
                using (var brush = new SolidBrush(Color.FromArgb(150, Style.Db.Text)))
                {
                    g.FillRoundedRectangle(brush, thumbRect, 3);
                }
            }
        }

        private void PaintOperationButtons(Graphics g)
        {
            // 获取颜色
            var buttonForeColor = ButtonForeColor ?? Style.Db.Text;
            var buttonBackColor = ButtonBackColor ?? Style.Db.FillTertiary;
            var buttonBackHover = ButtonBackHover ?? Style.Db.FillSecondary;
            var buttonBackActive = ButtonBackActive ?? Style.Db.TextQuaternary;
            ;
            var buttonBackDisable = ButtonBackDisable ?? Color.FromArgb(50, Style.Db.Text);

            // 绘制向右按钮
            bool canMoveRight = sourceFilteredItems.Any(i => i.Selected);
            PaintOperationButton(g, toRightRect, "RightOutlined", canMoveRight, hover_to_right_btn, active_to_right_btn, hover_to_right?.Tag != null ? (float)hover_to_right.Tag : 0f, active_to_right?.Tag != null ? (float)active_to_right.Tag : 0f, buttonForeColor, buttonBackColor, buttonBackHover, buttonBackActive, buttonBackDisable);

            // 绘制向左按钮
            bool canMoveLeft = !OneWay && targetFilteredItems.Any(i => i.Selected);
            PaintOperationButton(g, toLeftRect, "LeftOutlined", canMoveLeft, hover_to_left_btn, active_to_left_btn, hover_to_left?.Tag != null ? (float)hover_to_left.Tag : 0f, active_to_left?.Tag != null ? (float)active_to_left.Tag : 0f, buttonForeColor, buttonBackColor, buttonBackHover, buttonBackActive, buttonBackDisable);
        }

        private void PaintOperationButton(Graphics g, Rectangle rect, string icon, bool enabled, bool hover, bool active, float hoverValue, float activeValue, Color foreColor, Color backColor, Color backHover, Color backActive, Color backDisable)
        {
            // 计算背景颜色
            Color backgroundColor;
            if (!enabled)
            {
                backgroundColor = backDisable;
            }
            else if (active)
            {
                backgroundColor = Helper.ValueBlend(backHover, backActive, activeValue);
            }
            else if (hover)
            {
                backgroundColor = Helper.ValueBlend(backColor, backHover, hoverValue);
            }
            else
            {
                backgroundColor = backColor;
            }

            // 绘制按钮背景
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(rect);
                using (var brush = new SolidBrush(backgroundColor))
                {
                    g.FillPath(brush, path);
                }
            }

            // 绘制按钮图标
            var iconRect = new Rectangle(rect.X + 8, rect.Y + 8, rect.Width - 16, rect.Height - 16);
            g.DrawSvg(icon, iconRect, enabled ? foreColor : Color.FromArgb(100, foreColor));
        }

        private void MoveSelectedItemsToRight()
        {
            bool changed = false;
            var selectedItems = sourceItems.Where(i => i.Selected).ToList();

            foreach (var item in selectedItems)
            {
                sourceItems.Remove(item);
                item.IsTarget = true;
                item.Selected = false;
                targetItems.Add(item);
                changed = true;
            }

            if (changed)
            {
                ApplyFilter();
                OnTransferChanged(new TransferEventArgs(sourceItems, targetItems));
            }
        }

        private void MoveSelectedItemsToLeft()
        {
            if (OneWay) return;

            bool changed = false;
            var selectedItems = targetItems.Where(i => i.Selected).ToList();

            foreach (var item in selectedItems)
            {
                targetItems.Remove(item);
                item.IsTarget = false;
                item.Selected = false;
                sourceItems.Add(item);
                changed = true;
            }

            if (changed)
            {
                ApplyFilter();
                OnTransferChanged(new TransferEventArgs(sourceItems, targetItems));
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 重新加载数据
        /// </summary>
        public void Reload()
        {
            InitializeItems();
            Invalidate();
        }

        /// <summary>
        /// 获取源列表项
        /// </summary>
        /// <returns>源列表项</returns>
        public List<TransferItem> GetSourceItems()
        {
            return new List<TransferItem>(sourceItems);
        }

        /// <summary>
        /// 获取目标列表项
        /// </summary>
        /// <returns>目标列表项</returns>
        public List<TransferItem> GetTargetItems()
        {
            return new List<TransferItem>(targetItems);
        }

        /// <summary>
        /// 设置源列表搜索文本
        /// </summary>
        /// <param name="text">搜索文本</param>
        public void SetSourceSearchText(string text)
        {
            sourceSearchText = text;
            OnSearch(new SearchEventArgs(text, true));
            ApplyFilter();
        }

        /// <summary>
        /// 设置目标列表搜索文本
        /// </summary>
        /// <param name="text">搜索文本</param>
        public void SetTargetSearchText(string text)
        {
            targetSearchText = text;
            OnSearch(new SearchEventArgs(text, false));
            ApplyFilter();
        }

        #endregion
    }

    /// <summary>
    /// 穿梭框项
    /// </summary>
    public class TransferItem
    {
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// 值
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// 是否悬停
        /// </summary>
        public bool Hover { get; set; }

        /// <summary>
        /// 是否在目标列表
        /// </summary>
        public bool IsTarget { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TransferItem() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text">文本</param>
        public TransferItem(string text)
        {
            Text = text;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="value">值</param>
        public TransferItem(string text, object value)
        {
            Text = text;
            Value = value;
        }
    }

    /// <summary>
    /// 图形路径扩展
    /// </summary>
    internal static class GraphicsPathExtensions
    {
        /// <summary>
        /// 添加圆角矩形
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="rect">矩形</param>
        /// <param name="radius">圆角半径</param>
        public static void AddRoundedRectangle(this GraphicsPath path, Rectangle rect, int radius)
        {
            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return;
            }

            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(rect.Location, size);

            // 左上角
            path.AddArc(arc, 180, 90);

            // 顶边
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // 右边
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // 底边
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
        }
    }

    /// <summary>
    /// 图形扩展
    /// </summary>
    internal static class GraphicsExtensions
    {
        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="g">图形</param>
        /// <param name="brush">画刷</param>
        /// <param name="rect">矩形</param>
        /// <param name="radius">圆角半径</param>
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using (var path = new GraphicsPath())
            {
                path.AddRoundedRectangle(rect, radius);
                g.FillPath(brush, path);
            }
        }

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="g">图形</param>
        /// <param name="pen">画笔</param>
        /// <param name="rect">矩形</param>
        /// <param name="radius">圆角半径</param>
        public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle rect, int radius)
        {
            using (var path = new GraphicsPath())
            {
                path.AddRoundedRectangle(rect, radius);
                g.DrawPath(pen, path);
            }
        }

        /// <summary>
        /// 绘制SVG图标
        /// </summary>
        /// <param name="g">图形</param>
        /// <param name="icon">图标名称</param>
        /// <param name="rect">矩形</param>
        /// <param name="color">颜色</param>
        public static void DrawSvg(this Graphics g, string icon, Rectangle rect, Color color)
        {
            try
            {
                using (var svg = SvgExtend.GetImgExtend(icon, rect, color))
                {
                    if (svg != null)
                    {
                        g.DrawImage(svg, rect);
                    }
                }
            }
            catch { }
        }
    }
}
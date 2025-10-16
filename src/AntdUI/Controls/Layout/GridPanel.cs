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
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace AntdUI
{
    /// <summary>
    /// GridPanel 格栅布局
    /// </summary>
    [Description("GridPanel 格栅布局")]
    [ToolboxItem(true)]
    [DefaultProperty("Span")]
    [Designer(typeof(IControlDesigner))]
    [ProvideProperty("Index", typeof(Control))]
    public class GridPanel : ContainerPanel, IExtenderProvider
    {
        #region 属性

        /// <summary>
        /// 跨度
        /// </summary>
        [Description("跨度"), Category("外观"), DefaultValue("50% 50%;50% 50%")]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        public string Span
        {
            get => layoutengine.Span;
            set
            {
                if (layoutengine.Span == value) return;
                layoutengine.Span = value;
                if (IsHandleCreated) IOnSizeChanged();
                OnPropertyChanged(nameof(Span));
            }
        }

        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(0)]
        public int Gap
        {
            get => layoutengine.Gap;
            set
            {
                if (layoutengine.Gap == value) return;
                layoutengine.Gap = value;
                if (IsHandleCreated) IOnSizeChanged();
                OnPropertyChanged(nameof(Gap));
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
                    Invalidate();
                    IOnSizeChanged();
                }
                OnPropertyChanged(nameof(PauseLayout));
            }
        }

        #endregion

        #region Index 排序

        public bool CanExtend(object extendee) => extendee is Control control && control.Parent == this;

        Dictionary<Control, int> Map = new Dictionary<Control, int>();

        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("Index"), Description("排序"), DefaultValue(-1)]
        public int GetIndex(Control control) => IndexExists(control);

        public void SetIndex(Control control, int index)
        {
            int old = IndexExists(control);
            if (old == index) return;
            if (index > -1)
            {
                if (old == -1) Map.Add(control, index);
                else Map[control] = index;
            }
            else if (old > -1) Map.Remove(control);
            if (IsHandleCreated) IOnSizeChanged();
        }

        int IndexExists(Control control)
        {
            if (Map.TryGetValue(control, out int index)) return index;
            return -1;
        }

        #endregion

        #region 布局
        public override Rectangle DisplayRectangle => ClientRectangle.DeflateRect(Padding);

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            IOnSizeChanged();
        }

        GridLayout layoutengine = new GridLayout();
        public override LayoutEngine LayoutEngine => layoutengine;
        internal class GridLayout : LayoutEngine
        {
            /// <summary>
            /// 内容大小
            /// </summary>
            public string Span { get; set; } = "50% 50%;50% 50%";

            /// <summary>
            /// 间距
            /// </summary>
            public int Gap { get; set; }

            public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
            {
                if (container is GridPanel parent && parent.IsHandleCreated)
                {
                    if (parent.PauseLayout) return false;
                    var rect = parent.DisplayRectangle;
                    if (!string.IsNullOrEmpty(Span) && parent.Controls.Count > 0)
                    {
                        var controls = SyncMap(parent);
                        if (controls.Count > 0)
                        {
                            var rects = parent.ConvertToRects(rect, Span);
                            HandLayout(controls, rects, rect);
                        }
                    }
                }
                return false;
            }

            #region 排序

            List<Control> SyncMap(GridPanel parent)
            {
                int count = parent.Controls.Count, i = count;
                var tmp = new List<IList>(count);
                foreach (Control it in parent.Controls)
                {
                    if (it.Visible)
                    {
                        if (parent.Map.TryGetValue(it, out int index)) tmp.Insert(0, new IList(it, index));
                        else tmp.Insert(0, new IList(it, i));
                        i--;
                    }
                }
                tmp.Sort((a, b) => a.Index.CompareTo(b.Index));
                var controls = new List<Control>(tmp.Count);
                foreach (var it in tmp) controls.Add(it.Control);
                return controls;
            }

            class IList
            {
                public IList(Control control, int index)
                {
                    Control = control;
                    Index = index;
                }

                public Control Control { get; set; }
                public int Index { get; set; }
            }

            #endregion

            void HandLayout(List<Control> controls, Rects[] rects, Rectangle rect)
            {
                if (rects.Length == 0 || controls.Count == 0) return;
                int gap = (int)Math.Round(Gap * Config.Dpi), gap2 = gap * 2;
                int index = 0;
                for (int i = 0; i < controls.Count; i++)
                {
                    var control = controls[i];
                    var rect_real = rects[index];
                    while (rect_real.IsEmpty)
                    {
                        index++;
                        if (index >= rects.Length)
                        {
                            ClearLayout(controls, i, rect);
                            return;
                        }
                        rect_real = rects[index];
                    }
                    Point point = rect_real.Location;
                    point.Offset(control.Margin.Left + gap, control.Margin.Top + gap);
                    control.Location = point;
                    control.Size = new Size(rect_real.Width - control.Margin.Horizontal - gap2, rect_real.Height - control.Margin.Vertical - gap2);
                    index++;
                    if (index >= rects.Length)
                    {
                        ClearLayout(controls, i + 1, rect);
                        return;
                    }
                }
            }

            void ClearLayout(List<Control> controls, int si, Rectangle rect)
            {
                for (int i = si; i < controls.Count; i++) controls[i].Location = new Point(-rect.Width, -rect.Height);
            }
        }

        #region 核心

        public Rects[] ConvertToRects(Rectangle rect, string span)
        {
            try
            {
                // 分割行定义和全局行高
                var spanParts = span.Split(new[] { '-' }, 2, StringSplitOptions.None);
                string rowDefinitions = spanParts[0], globalRowHeights = spanParts.Length > 1 ? spanParts[1] : "";

                // 解析所有行定义
                var rowParts = rowDefinitions.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                int totalRows = rowParts.Length;
                if (totalRows == 0) return new Rects[0];

                // 解析全局行高
                var globalHeightValues = globalRowHeights.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // 第一步：收集所有行的高度设置
                var rowHeightSettings = new List<string>();
                var columnDefinitionsList = new List<string[]>();

                for (int i = 0; i < totalRows; i++)
                {
                    string rowPart = rowParts[i], columnPart = rowPart;
                    string? heightStr = null;

                    // 提取行内高度设置
                    int colonIndex = rowPart.IndexOf(':');
                    if (colonIndex > 0)
                    {
                        heightStr = rowPart.Substring(0, colonIndex).Trim();
                        columnPart = rowPart.Substring(colonIndex + 1).Trim();
                    }
                    // 使用全局行高
                    else if (i < globalHeightValues.Length) heightStr = globalHeightValues[i];
                    // 默认填充
                    else heightStr = "fill";

                    rowHeightSettings.Add(heightStr);
                    columnDefinitionsList.Add(columnPart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                }

                // 第二步：计算所有行的实际高度
                var rowHeights = CalculateDimensions(rowHeightSettings, rect.Height, true);
                if (rowHeights == null) return new Rects[0];

                // 预估元素数量
                int estimatedItems = 0;
                foreach (var cols in columnDefinitionsList) estimatedItems += cols.Length;
                var rectsList = new List<Rects>(estimatedItems);

                // 第三步：生成矩形
                float currentY = rect.Y;
                for (int rowIndex = 0; rowIndex < totalRows; rowIndex++)
                {
                    string[] columnWidths = columnDefinitionsList[rowIndex];
                    float rowHeight = rowHeights[rowIndex];

                    if (columnWidths.Length == 0)
                    {
                        currentY += rowHeight;
                        continue;
                    }

                    // 计算列宽
                    var calculatedWidths = CalculateDimensions(columnWidths, rect.Width, false);
                    if (calculatedWidths == null)
                    {
                        currentY += rowHeight;
                        continue;
                    }

                    // 生成当前行的矩形
                    float currentX = rect.X;
                    for (int colIndex = 0; colIndex < columnWidths.Length; colIndex++)
                    {
                        string widthStr = columnWidths[colIndex];
                        float width = calculatedWidths[colIndex];
                        bool isEmpty = widthStr.Contains("_N");

                        // 确保非空位置有最小宽度
                        if (width <= 0 && !isEmpty)
                            width = 1;

                        rectsList.Add(new Rects
                        {
                            x = colIndex,
                            y = rowIndex,
                            IsEmpty = isEmpty,
                            Rect = new Rectangle(
                                (int)Math.Round(currentX),
                                (int)Math.Round(currentY),
                                (int)Math.Round(width),
                                (int)Math.Round(rowHeight)
                            )
                        });

                        currentX += width;
                    }

                    currentY += rowHeight;
                }

                return rectsList.ToArray();
            }
            catch
            {
                return new Rects[0];
            }
        }

        /// <summary>
        /// 通用尺寸计算方法
        /// 既可以计算行高，也可以计算列宽
        /// </summary>
        float[]? CalculateDimensions(IList<string> dimension, float maxValue, bool isHeight)
        {
            try
            {
                int totalItems = dimension.Count;
                var dimensions = new float[totalItems];
                float fixedSum = 0, percentSum = 0;
                int fillCount = 0;

                // 第一遍：分离固定值、百分比和填充项
                for (int i = 0; i < totalItems; i++)
                {
                    string dimStr = dimension[i], valueStr = dimStr.Contains("_N") ? dimStr.Replace("_N", "") : dimStr;

                    if (valueStr.Equals("fill", StringComparison.OrdinalIgnoreCase))
                    {
                        dimensions[i] = 0;
                        fillCount++;
                    }
                    else if (valueStr.EndsWith("%"))
                    {
                        // 百分比值先标记为0，后续统一计算
                        dimensions[i] = 0;
                        if (float.TryParse(valueStr.TrimEnd('%'), out float percentage)) percentSum += percentage;
                    }
                    else
                    {
                        // 固定值直接计算
                        float dimValue = ParseDimension(valueStr, maxValue, isHeight);
                        dimensions[i] = dimValue;
                        fixedSum += dimValue;
                    }
                }

                // 计算剩余可分配空间（优先扣除固定值）
                float remainingSpace = Math.Max(0, maxValue - fixedSum), percentBase = remainingSpace;

                // 处理百分比分配
                if (percentSum > 0 && remainingSpace > 0)
                {
                    foreach (int i in Enumerable.Range(0, totalItems))
                    {
                        string dimStr = dimension[i], valueStr = dimStr.Contains("_N") ? dimStr.Replace("_N", "") : dimStr;
                        if (valueStr.EndsWith("%") && float.TryParse(valueStr.TrimEnd('%'), out float percentage)) dimensions[i] = percentBase * (percentage / 100f);
                    }
                }

                // 处理填充项分配（剩余空间平均分配）
                if (fillCount > 0)
                {
                    // 计算扣除固定值和百分比后的剩余空间
                    float percentTotal = dimensions.Where((val, i) => dimension[i].Replace("_N", "").EndsWith("%")).Sum();

                    float fillRemaining = Math.Max(0, remainingSpace - percentTotal);
                    float fillValue = fillRemaining / fillCount;

                    for (int i = 0; i < totalItems; i++)
                    {
                        string dimStr = dimension[i], valueStr = dimStr.Contains("_N") ? dimStr.Replace("_N", "") : dimStr;

                        if (valueStr.Equals("fill", StringComparison.OrdinalIgnoreCase)) dimensions[i] = fillValue;
                    }
                }

                return dimensions;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 解析尺寸
        /// </summary>
        float ParseDimension(string dimensionStr, float maxValue, bool isHeight)
        {
            // 百分比处理（相对于maxValue）
            if (dimensionStr.EndsWith("%"))
            {
                if (float.TryParse(dimensionStr.TrimEnd('%'), out float percentageValue)) return maxValue * (Math.Abs(percentageValue) / 100f);
                return 0;
            }

            // int类型（仅int乘Dpi）
            if (int.TryParse(dimensionStr, out int intValue)) return (float)Math.Round(Math.Abs(intValue) * Config.Dpi);

            // float类型（直接使用）
            if (float.TryParse(dimensionStr, out float floatValue)) return floatValue;

            return 0;
        }

        public class Rects
        {
            /// <summary>
            /// 第几列
            /// </summary>
            public int x { get; set; }
            /// <summary>
            /// 第几行
            /// </summary>
            public int y { get; set; }
            /// <summary>
            /// 最终坐标
            /// </summary>
            public Rectangle Rect { get; set; }
            /// <summary>
            /// 是否空位置（定义边距等空占位）
            /// </summary>
            public bool IsEmpty { get; set; }

            public Point Location => Rect.Location;
            public int Width => Rect.Width;
            public int Height => Rect.Height;
        }

        #endregion

        #endregion

        protected override void OnDraw(DrawEventArgs e)
        {
            base.OnDraw(e);
            if (PaintBack(e.Canvas))
            {
                if (!string.IsNullOrEmpty(Span))
                {
                    var rects = ConvertToRects(DisplayRectangle, Span);
                    using (var fore = new SolidBrush(Style.Db.Text))
                    using (var bg = new SolidBrush(Style.Db.Fill))
                    using (var pen = new Pen(Style.Db.PrimaryBorder, Config.Dpi * 2))
                    {
                        int gap = (int)(3 * Config.Dpi), gap2 = gap * 2;
                        foreach (var it in rects)
                        {
                            var rect = new Rectangle(it.Rect.X + gap, it.Rect.Y + gap, it.Rect.Width - gap2, it.Rect.Height - gap2);
                            if (it.IsEmpty) e.Canvas.Fill(bg, rect);
                            else
                            {
                                e.Canvas.Draw(pen, rect);
                                e.Canvas.String(it.x + ":" + it.y, Font, fore, it.Rect);
                            }
                        }
                    }
                }
            }
        }
    }
}
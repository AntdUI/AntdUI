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
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    #region Int

    public class IntEventArgs : VEventArgs<int>
    {
        public IntEventArgs(int value) : base(value) { }
    }

    /// <summary>
    /// Int 类型事件
    /// </summary>
    public delegate void IntEventHandler(object sender, IntEventArgs e);

    /// <summary>
    /// Int 类型事件
    /// </summary>
    public delegate bool IntBoolEventHandler(object sender, IntEventArgs e);

    #endregion

    #region Float

    public class FloatEventArgs : VEventArgs<float>
    {
        public FloatEventArgs(float value) : base(value) { }
    }

    /// <summary>
    /// Float 类型事件
    /// </summary>
    public delegate void FloatEventHandler(object sender, FloatEventArgs e);

    #endregion

    #region Decimal

    public class DecimalEventArgs : VEventArgs<decimal>
    {
        public DecimalEventArgs(decimal value) : base(value) { }
    }

    /// <summary>
    /// Decimal 类型事件
    /// </summary>
    public delegate void DecimalEventHandler(object sender, DecimalEventArgs e);

    #endregion

    #region Object

    public class ObjectNEventArgs : VEventArgs<object?>
    {
        public ObjectNEventArgs(object? value) : base(value) { }
    }

    /// <summary>
    /// Object类型事件
    /// </summary>
    public delegate void ObjectNEventHandler(object sender, ObjectNEventArgs e);

    public class ObjectsEventArgs : VEventArgs<object[]>
    {
        public ObjectsEventArgs(object[] value) : base(value) { }
    }

    /// <summary>
    /// Object类型事件
    /// </summary>
    public delegate void ObjectsEventHandler(object sender, ObjectsEventArgs e);

    #endregion

    #region Bool

    public class BoolEventArgs : VEventArgs<bool>
    {
        public BoolEventArgs(bool value) : base(value) { }
    }

    /// <summary>
    /// Bool 类型事件
    /// </summary>
    public delegate void BoolEventHandler(object sender, BoolEventArgs e);

    #endregion

    #region Color

    public class ColorEventArgs : VEventArgs<Color>
    {
        public ColorEventArgs(Color value) : base(value) { }
    }

    /// <summary>
    /// Color 类型事件
    /// </summary>
    public delegate void ColorEventHandler(object sender, ColorEventArgs e);

    #endregion

    #region DateTime

    public class DateTimeEventArgs : VEventArgs<DateTime>
    {
        public DateTimeEventArgs(DateTime value) : base(value) { }
    }

    /// <summary>
    /// DateTime 类型事件
    /// </summary>
    public delegate void DateTimeEventHandler(object sender, DateTimeEventArgs e);

    public class DateTimeNEventArgs : VEventArgs<DateTime?>
    {
        public DateTimeNEventArgs(DateTime? value) : base(value) { }
    }

    /// <summary>
    /// DateTime 类型事件
    /// </summary>
    public delegate void DateTimeNEventHandler(object sender, DateTimeNEventArgs e);

    public class TimeSpanNEventArgs : VEventArgs<TimeSpan>
    {
        public TimeSpanNEventArgs(TimeSpan value) : base(value) { }
    }

    /// <summary>
    /// TimeSpan 类型事件
    /// </summary>
    public delegate void TimeSpanNEventHandler(object sender, TimeSpanNEventArgs e);

    #endregion

    #region DateTime[]

    public class DateTimesEventArgs : VEventArgs<DateTime[]?>
    {
        public DateTimesEventArgs(DateTime[]? value) : base(value) { }
    }


    public delegate void DateTimesEventHandler(object sender, DateTimesEventArgs e);

    #endregion

    #region 更多

    #region Input

    public class InputVerifyCharEventArgs : EventArgs
    {
        public InputVerifyCharEventArgs(char c)
        {
            Char = c;
        }

        /// <summary>
        /// 输入字符
        /// </summary>
        public char Char { get; private set; }

        /// <summary>
        /// 替换文本
        /// </summary>
        public string? ReplaceText { get; set; }

        /// <summary>
        /// 验证结果
        /// </summary>
        public bool Result { get; set; } = true;
    }


    public delegate void InputVerifyCharEventHandler(object sender, InputVerifyCharEventArgs e);

    public class InputVerifyKeyboardEventArgs : EventArgs
    {
        public InputVerifyKeyboardEventArgs(Keys keyData)
        {
            KeyData = keyData;
        }

        public Keys KeyData { get; private set; }

        /// <summary>
        /// 验证结果
        /// </summary>
        public bool Result { get; set; } = true;
    }


    public delegate void InputVerifyKeyboardEventHandler(object sender, InputVerifyKeyboardEventArgs e);

    #endregion

    #region Menu

    public class MenuSelectEventArgs : VEventArgs<MenuItem>
    {
        public MenuSelectEventArgs(MenuItem value) : base(value) { }
    }


    public delegate void SelectEventHandler(object sender, MenuSelectEventArgs e);

    public delegate bool SelectBoolEventHandler(object sender, MenuSelectEventArgs e);

    #endregion

    #region Pagination

    public class PagePageEventArgs : EventArgs
    {
        public PagePageEventArgs(int current, int total, int pageSize, int pageTotal)
        {
            Current = current;
            Total = total;
            PageSize = pageSize;
            PageTotal = pageTotal;
        }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int Current { get; private set; }
        /// <summary>
        /// 数据总数
        /// </summary>
        public int Total { get; private set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageTotal { get; private set; }
    }

    /// <summary>
    /// 显示数据总量
    /// </summary>
    public delegate void PageValueEventHandler(object sender, PagePageEventArgs e);

    /// <summary>
    /// 显示数据总量
    /// </summary>
    public delegate string PageValueRtEventHandler(object sender, PagePageEventArgs e);

    #endregion

    #region Breadcrumb

    public class BreadcrumbItemEventArgs : VMEventArgs<BreadcrumbItem>
    {
        public BreadcrumbItemEventArgs(BreadcrumbItem item, MouseEventArgs e) : base(item, e) { }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void BreadcrumbItemEventHandler(object sender, BreadcrumbItemEventArgs e);

    #endregion

    #region Segmented

    public class SegmentedItemEventArgs : VMEventArgs<SegmentedItem>
    {
        public SegmentedItemEventArgs(SegmentedItem item, MouseEventArgs e) : base(item, e) { }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void SegmentedItemEventHandler(object sender, SegmentedItemEventArgs e);

    #endregion

    #region VirtualPanel

    public class VirtualItemEventArgs : VMEventArgs<VirtualItem>
    {
        public VirtualItemEventArgs(VirtualItem item, MouseEventArgs e) : base(item, e) { }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void VirtualItemEventHandler(object sender, VirtualItemEventArgs e);

    #endregion

    #region Select

    public class IntXYEventArgs : EventArgs
    {
        public IntXYEventArgs(int x, int y) { X = x; Y = y; }
        public int X { get; private set; }
        public int Y { get; private set; }
    }

    public delegate void IntXYEventHandler(object sender, IntXYEventArgs e);

    #endregion

    #region Slider

    public delegate string ValueFormatEventHandler(object sender, IntEventArgs e);

    #endregion

    #region Progress

    public delegate string ProgressFormatEventHandler(object sender, FloatEventArgs e);

    #endregion

    #region ColorPicker

    public delegate string ColorFormatEventHandler(object sender, ColorEventArgs e);

    #endregion

    #region Steps

    public class StepsItemEventArgs : VMEventArgs<StepsItem>
    {
        public StepsItemEventArgs(StepsItem item, MouseEventArgs e) : base(item, e) { }
    }

    /// <summary>
    /// 点击项时发生
    /// </summary>
    public delegate void StepsItemEventHandler(object sender, StepsItemEventArgs e);

    #endregion

    #region Table

    public class TableCheckEventArgs : ITableEventArgs
    {
        public TableCheckEventArgs(bool value, object? record, int rowIndex, int columnIndex, Column? column) : base(record, rowIndex, columnIndex, column)
        {
            Value = value;
        }
        /// <summary>
        /// 数值
        /// </summary>
        public bool Value { get; private set; }
    }
    public class TableClickEventArgs : MouseEventArgs
    {
        public TableClickEventArgs(object? record, int rowIndex, int columnIndex, Column? column, Rectangle rect, MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Record = record;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Column = column;
            Rect = rect;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object? Record { get; private set; }
        /// <summary>
        /// 行序号
        /// </summary>
        public int RowIndex { get; private set; }
        /// <summary>
        /// 列序号
        /// </summary>
        public int ColumnIndex { get; private set; }
        /// <summary>
        /// 表头
        /// </summary>
        public Column? Column { get; private set; }
        /// <summary>
        /// 表格区域
        /// </summary>
        public Rectangle Rect { get; private set; }
    }
    public class TableHoverEventArgs : MouseEventArgs
    {
        public TableHoverEventArgs(object? record, int rowIndex, int columnIndex, Column? column, Rectangle? rect, MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Record = record;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Column = column;
            Rect = rect;
        }

        public TableHoverEventArgs(MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            RowIndex = ColumnIndex = -1;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object? Record { get; private set; }
        /// <summary>
        /// 行序号
        /// </summary>
        public int RowIndex { get; private set; }
        /// <summary>
        /// 列序号
        /// </summary>
        public int ColumnIndex { get; private set; }
        /// <summary>
        /// 表头
        /// </summary>
        public Column? Column { get; private set; }
        /// <summary>
        /// 表格区域
        /// </summary>
        public Rectangle? Rect { get; private set; }
    }
    public class TableButtonEventArgs : MouseEventArgs
    {
        public TableButtonEventArgs(CellLink btn, object? record, int rowIndex, int columnIndex, Column? column, MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Btn = btn;
            Record = record;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Column = column;
        }

        /// <summary>
        /// 触发按钮
        /// </summary>
        public CellLink Btn { get; private set; }

        /// <summary>
        /// 原始行
        /// </summary>
        public object? Record { get; private set; }
        /// <summary>
        /// 行序号
        /// </summary>
        public int RowIndex { get; private set; }
        /// <summary>
        /// 列序号
        /// </summary>
        public int ColumnIndex { get; private set; }
        /// <summary>
        /// 表头
        /// </summary>
        public Column? Column { get; private set; }
    }
    public class TableEventArgs : ITableEventArgs
    {
        public TableEventArgs(object? value, object? record, int rowIndex, int columnIndex, Column? column) : base(record, rowIndex, columnIndex, column)
        {
            Value = value;
        }

        /// <summary>
        /// 数值
        /// </summary>
        public object? Value { get; private set; }
    }

    public class TableBeginEditInputStyleEventArgs : ITableEventArgs
    {
        public TableBeginEditInputStyleEventArgs(object? value, object? record, int rowIndex, int columnIndex, Column? column, Input input) : base(record, rowIndex, columnIndex, column)
        {
            Value = value;
            Input = input;
        }

        /// <summary>
        /// 数值
        /// </summary>
        public object? Value { get; private set; }

        /// <summary>
        /// 文本框
        /// </summary>
        public Input Input { get; private set; }

        internal Action<TableEndEditEventArgs>? Call { get; set; }

        public InputNumber Set(InputNumber input, Action<Result<InputNumber>>? call = null)
        {
            SetInput(input);
            if (call == null) return input;
            Call = e => call(new Result<InputNumber>(input, Column, e));
            return input;
        }

        public Select Set(Select input, Action<Result<Select>>? call = null)
        {
            SetInput(input);
            if (call == null) return input;
            Call = e => call(new Result<Select>(input, Column, e));
            return input;
        }

        public SelectMultiple Set(SelectMultiple input, Action<Result<SelectMultiple>>? call = null)
        {
            SetInput(input);
            if (call == null) return input;
            Call = e => call(new Result<SelectMultiple>(input, Column, e));
            return input;
        }

        public DatePicker Set(DatePicker input, Action<Result<DatePicker>>? call = null)
        {
            SetInput(input);
            if (call == null) return input;
            Call = e => call(new Result<DatePicker>(input, Column, e));
            return input;
        }

        public DatePickerRange Set(DatePickerRange input, Action<Result<DatePickerRange>>? call = null)
        {
            SetInput(input);
            if (call == null) return input;
            Call = e => call(new Result<DatePickerRange>(input, Column, e));
            return input;
        }

        public TimePicker Set(TimePicker input, Action<Result<TimePicker>>? call = null)
        {
            SetInput(input);
            if (call == null) return input;
            Call = e => call(new Result<TimePicker>(input, Column, e));
            return input;
        }

        void SetInput(Input input)
        {
            var point = Input.Location;
            var size = Input.Size;
            Input.Dispose();
            input.Location = point;
            input.Size = size;
            if (Column != null)
            {
                input.ReadOnly = Column.ReadOnly;
                if (input.ReadOnly) input.BackColor = AntdUI.Style.Db.BorderSecondary;
            }
            Input = input;
        }

        public class Result<T> : ITableEventArgs
        {
            public Result(T input, Column? column, TableEndEditEventArgs e) : base(e.Record, e.RowIndex, e.ColumnIndex, column)
            {
                Input = input;
            }

            public T Input { get; private set; }
        }
    }

    public class TableEndEditEventArgs : ITableEventArgs
    {
        public TableEndEditEventArgs(string value, object? record, int rowIndex, int columnIndex, Column? column) : base(record, rowIndex, columnIndex, column)
        {
            Value = value;
        }

        /// <summary>
        /// 修改后值
        /// </summary>
        public string Value { get; private set; }
    }

    public class TableSetRowStyleEventArgs : EventArgs
    {
        public TableSetRowStyleEventArgs(object? record, int rowIndex, int index)
        {
            Record = record;
            RowIndex = rowIndex;
            Index = index;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object? Record { get; private set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// 行序号
        /// </summary>
        public int RowIndex { get; private set; }
    }

    public class TablePaintEventArgs : EventArgs
    {
        public TablePaintEventArgs(Canvas canvas, Rectangle rect, Rectangle rectreal, object? record, int rowIndex, int index, Column column)
        {
            g = canvas;
            Rect = rect;
            RectReal = rectreal;
            Record = record;
            RowIndex = rowIndex;
            Index = index;
            Column = column;
        }

        /// <summary>
        /// 画板
        /// </summary>
        public Canvas g { get; private set; }

        /// <summary>
        /// 区域
        /// </summary>
        public Rectangle Rect { get; private set; }

        /// <summary>
        /// 真实区域
        /// </summary>
        public Rectangle RectReal { get; private set; }

        /// <summary>
        /// 原始行
        /// </summary>
        public object? Record { get; private set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// 表头
        /// </summary>
        public Column Column { get; private set; }

        /// <summary>
        /// 行序号
        /// </summary>
        public int RowIndex { get; private set; }
    }

    public class TablePaintBeginEventArgs : TablePaintEventArgs
    {
        public TablePaintBeginEventArgs(Canvas canvas, Rectangle rect, Rectangle rectreal, object? record, int rowIndex, int index, Column column) : base(canvas, rect, rectreal, record, rowIndex, index, column) { }

        /// <summary>
        /// 是否处理
        /// </summary>
        public bool Handled { get; set; }
    }

    public class TableSortModeEventArgs : EventArgs
    {
        public TableSortModeEventArgs(SortMode sortMode, Column column)
        {
            SortMode = sortMode;
            Column = column;
        }

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortMode SortMode { get; private set; }

        /// <summary>
        /// 表头
        /// </summary>
        public Column Column { get; private set; }
    }

    public class TableSortTreeEventArgs : EventArgs
    {
        public TableSortTreeEventArgs(object? record, int[] sort, int from, int to)
        {
            Record = record;
            Sort = sort;
            From = from;
            To = to;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object? Record { get; private set; }

        public int[] Sort { get; private set; }

        public int From { get; private set; }

        public int To { get; private set; }
    }

    public class TableExpandEventArgs : EventArgs
    {
        public TableExpandEventArgs(object? record, bool expand)
        {
            Record = record;
            Expand = expand;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object? Record { get; private set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool Expand { get; private set; }
    }

    public class ITableEventArgs : EventArgs
    {
        public ITableEventArgs(object? record, int rowIndex, int columnIndex, Column? column)
        {
            Record = record;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Column = column;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object? Record { get; private set; }
        /// <summary>
        /// 行序号
        /// </summary>
        public int RowIndex { get; private set; }
        /// <summary>
        /// 列序号
        /// </summary>
        public int ColumnIndex { get; private set; }

        /// <summary>
        /// 表头
        /// </summary>
        public Column? Column { get; private set; }
    }
    public class TableFilterPopupBeginEventArgs : EventArgs
    {
        public TableFilterPopupBeginEventArgs(Column column)
        {
            Column = column;
        }

        /// <summary>
        /// 筛选列
        /// </summary>
        public Column Column { get; private set; }
        /// <summary>
        /// 筛选选项
        /// </summary>
        public FilterOption? Option { get { return Column.Filter; } }
        /// <summary>
        /// 当前列的自定义数据源
        /// </summary>
        public System.Collections.Generic.IList<object>? CustomSource { get; set; }
        /// <summary>
        /// 筛选栏字体
        /// </summary>
        public Font? Font { get; set; } = null;
        /// <summary>
        /// 筛选栏高度
        /// </summary>
        public int Height { get; set; } = 0;
        /// <summary>
        /// 是否取消弹出
        /// </summary>
        public bool Cancel { get; set; }
    }

    public class TableFilterPopupEndEventArgs : EventArgs
    {
        public TableFilterPopupEndEventArgs(Popover.Config config, FilterOption option)
        {
            Config = config;
            Option = option;
        }

        /// <summary>
        /// 参数
        /// </summary>
        public Popover.Config Config { get; private set; }

        /// <summary>
        /// 当前列的自定义数据源
        /// </summary>
        public FilterOption Option { get; private set; }
        /// <summary>
        /// 是否取消关闭
        /// </summary>
        public bool Cancel { get; set; }
    }

    #endregion

    #region Tabs

    public class ClosingPageEventArgs : VEventArgs<TabPage>
    {
        public ClosingPageEventArgs(TabPage value) : base(value) { }
    }

    public delegate bool ClosingPageEventHandler(object sender, ClosingPageEventArgs e);

    public class TabsItemEventArgs : VMEventArgs<TabPage>
    {
        public int Index { get; private set; }

        public Tabs.IStyle Style { get; private set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        public bool Cancel { get; set; }

        public TabsItemEventArgs(TabPage item, int index, Tabs.IStyle style, MouseEventArgs e) : base(item, e)
        {
            Index = index;
            Style = style;
        }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void TabsItemEventHandler(object sender, TabsItemEventArgs e);

    #endregion

    #region Tag

    public delegate bool RBoolEventHandler(object sender, EventArgs e);

    #endregion

    #region Timeline

    public class TimelineItemEventArgs : VMEventArgs<TimelineItem>
    {
        public TimelineItemEventArgs(TimelineItem item, MouseEventArgs e) : base(item, e) { }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void TimelineEventHandler(object sender, TimelineItemEventArgs e);

    #endregion

    #region Collapse

    public class CollapseExpandEventArgs : VEventArgs<CollapseItem>
    {
        public CollapseExpandEventArgs(CollapseItem value, bool expand) : base(value) { Expand = expand; }

        public bool Expand { get; private set; }
    }

    public delegate void CollapseExpandEventHandler(object sender, CollapseExpandEventArgs e);

    public class CollapseExpandingEventArgs : VEventArgs<CollapseItem>
    {
        public CollapseExpandingEventArgs(CollapseItem value, bool expand, Point location) : base(value)
        {
            Expand = expand;
            Location = location;
        }

        public bool Expand { get; private set; }
        public Point Location { get; private set; }

    }

    /// <summary>
    /// Collapse 类型展开/折叠进行时事件
    /// </summary>
    public delegate void CollapseExpandingEventHandler(object sender, CollapseExpandingEventArgs e);

    public class CollapseButtonClickEventArgs : VEventArgs<CollapseGroupButton>
    {
        public CollapseButtonClickEventArgs(CollapseGroupButton value, CollapseItem parent) : base(value)
        {
            Parent = parent;
        }

        public CollapseItem Parent { get; private set; }
    }

    /// <summary>
    /// CollapseItem.Button单击事件
    /// </summary>
    public delegate void CollapseButtonClickEventHandler(object sender, CollapseButtonClickEventArgs e);

    public class CollapseSwitchCheckedChangedEventArgs : CollapseButtonClickEventArgs
    {
        public CollapseSwitchCheckedChangedEventArgs(CollapseGroupButton switchItem, CollapseItem parent, bool _checked) : base(switchItem, parent) { Checked = _checked; }

        public bool Checked { get; private set; }

    }

    public delegate void CollapseSwitchCheckedChangedEventHandler(object sender, CollapseSwitchCheckedChangedEventArgs e);

    public class CollapseEditChangedEventArgs : VEventArgs<object>
    {
        public CollapseEditChangedEventArgs(Collapse parent, CollapseItem parentItem, object value) : base(value)
        {
            Parent = parent;
            ParentItem = parentItem;
        }
        public Collapse Parent { get; private set; }
        public CollapseItem ParentItem { get; private set; }

    }
    public delegate void CollapseEditChangedEventHandler(object sender, CollapseEditChangedEventArgs e);

    public class CollapseCustomInputEditEventArgs : EventArgs
    {
        public CollapseCustomInputEditEventArgs() { }

        public IControl Edit { get; set; }
    }
    public delegate void CollapseCustomInputEditEventHandler(object sender, CollapseCustomInputEditEventArgs e);

    #endregion

    #region Tree

    public class TreeSelectEventArgs : VMEventArgs<TreeItem>
    {
        public TreeSelectEventArgs(TreeItem item, Rectangle rect, TreeCType type, MouseEventArgs e) : base(item, e)
        {
            Rect = rect;
            Type = type;
        }
        public Rectangle Rect { get; private set; }
        public TreeCType Type { get; private set; }
    }

    public delegate void TreeSelectEventHandler(object sender, TreeSelectEventArgs e);

    public class TreeHoverEventArgs : EventArgs
    {
        public TreeHoverEventArgs(TreeItem item, Rectangle rect, bool hover)
        {
            Item = item;
            Hover = hover;
            Rect = rect;
        }
        public TreeItem Item { get; private set; }
        public Rectangle Rect { get; private set; }
        public bool Hover { get; private set; }
    }

    public delegate void TreeHoverEventHandler(object sender, TreeHoverEventArgs e);

    public class TreeCheckedEventArgs : EventArgs
    {
        public TreeCheckedEventArgs(TreeItem item, bool value)
        {
            Item = item;
            Value = value;
        }
        public TreeItem Item { get; private set; }
        public bool Value { get; private set; }
    }

    public delegate void TreeCheckedEventHandler(object sender, TreeCheckedEventArgs e);

    public class TreeExpandEventArgs : EventArgs
    {
        public TreeExpandEventArgs(TreeItem item, bool value)
        {
            Item = item;
            Value = value;
        }
        public TreeItem Item { get; private set; }
        public bool Value { get; private set; }
        public bool CanExpand { get; set; } = true;
    }

    public delegate void TreeExpandEventHandler(object sender, TreeExpandEventArgs e);

    #endregion

    #region Chat

    #region ChatList

    public class ChatItemEventArgs : VMEventArgs<Chat.IChatItem>
    {
        public ChatItemEventArgs(Chat.IChatItem item, MouseEventArgs e) : base(item, e) { }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void ClickEventHandler(object sender, ChatItemEventArgs e);

    #endregion

    #region MsgList

    public class MsgItemEventArgs : EventArgs
    {
        public MsgItemEventArgs(Chat.MsgItem item) { Item = item; }
        public Chat.MsgItem Item { get; private set; }
    }

    public delegate void ItemSelectedEventHandler(object sender, MsgItemEventArgs e);

    #endregion

    #endregion

    #region TabHeader

    public class TabChangedEventArgs : VEventArgs<TagTabItem>
    {
        public TabChangedEventArgs(TagTabItem value, int tabIndex) : base(value)
        {
            Index = tabIndex;
        }

        public int Index { get; private set; }
    }

    public class TabCloseEventArgs : TabChangedEventArgs
    {
        public TabCloseEventArgs(TagTabItem value, int tabIndex) : base(value, tabIndex) { }

        /// <summary>
        /// 取消操作
        /// </summary>
        public bool Cancel { get; set; }
    }

    #endregion

    #endregion

    #region 渲染

    public class DrawEventArgs : EventArgs
    {
        public DrawEventArgs(Canvas canvas, Rectangle rect)
        {
            Canvas = canvas;
            Rect = rect;
        }

        public Canvas Canvas { get; private set; }
        public Rectangle Rect { get; private set; }

        public Graphics? Graphics
        {
            get
            {
                if (Canvas is Core.CanvasGDI gdi) return gdi.g;
                return null;
            }
        }
    }

    public delegate void DrawEventHandler(object sender, DrawEventArgs e);

    #endregion

    #region 基础

    public class StringsEventArgs : VEventArgs<string[]>
    {
        public StringsEventArgs(string[] value) : base(value) { }
    }

    public class VEventArgs<T> : EventArgs
    {
        public VEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public object? Tag { get; set; }
    }


    public class VMEventArgs<T> : MouseEventArgs
    {
        public VMEventArgs(T item, MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Item = item;
        }

        public T Item { get; private set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public object? Tag { get; set; }
    }

    #endregion
}
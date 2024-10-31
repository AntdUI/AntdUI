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

    public class ColorEventArgs : VEventArgs<System.Drawing.Color>
    {
        public ColorEventArgs(System.Drawing.Color value) : base(value) { }
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

    #region Menu

    public class MenuSelectEventArgs : VEventArgs<MenuItem>
    {
        public MenuSelectEventArgs(MenuItem value) : base(value) { }
    }


    public delegate void SelectEventHandler(object sender, MenuSelectEventArgs e);

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
        public TableCheckEventArgs(bool value, object? record, int rowIndex, int columnIndex) : base(record, rowIndex, columnIndex)
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
        public TableClickEventArgs(object? record, int rowIndex, int columnIndex, Rectangle rect, MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Record = record;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
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
        /// 表格区域
        /// </summary>
        public Rectangle Rect { get; private set; }
    }
    public class TableButtonEventArgs : MouseEventArgs
    {
        public TableButtonEventArgs(CellLink btn, object? record, int rowIndex, int columnIndex, MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Btn = btn;
            Record = record;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
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
    }
    public class TableEventArgs : ITableEventArgs
    {
        public TableEventArgs(object? value, object? record, int rowIndex, int columnIndex) : base(record, rowIndex, columnIndex)
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
        public TableBeginEditInputStyleEventArgs(object? value, object? record, int rowIndex, int columnIndex, ref Input input) : base(record, rowIndex, columnIndex)
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
    }

    public class TableEndEditEventArgs : ITableEventArgs
    {
        public TableEndEditEventArgs(string value, object? record, int rowIndex, int columnIndex) : base(record, rowIndex, columnIndex)
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
        public TableSetRowStyleEventArgs(object? record, int rowIndex)
        {
            Record = record;
            RowIndex = rowIndex;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object? Record { get; private set; }
        /// <summary>
        /// 行序号
        /// </summary>
        public int RowIndex { get; private set; }
    }

    public class ITableEventArgs : EventArgs
    {
        public ITableEventArgs(object? record, int rowIndex, int columnIndex)
        {
            Record = record;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
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
    }

    #endregion

    #region Tabs

    public class ClosingPageEventArgs : VEventArgs<TabPage>
    {
        public ClosingPageEventArgs(TabPage value) : base(value) { }
    }

    public delegate bool ClosingPageEventHandler(object sender, ClosingPageEventArgs e);

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

    /// <summary>
    /// Color 类型事件
    /// </summary>
    public delegate void CollapseExpandEventHandler(object sender, CollapseExpandEventArgs e);

    #endregion

    #region Tree

    public class TreeSelectEventArgs : VMEventArgs<TreeItem>
    {
        public TreeSelectEventArgs(TreeItem item, Rectangle rect, MouseEventArgs e) : base(item, e) { Rect = rect; }
        public Rectangle Rect { get; private set; }
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
            Value = Value;
        }
        public TreeItem Item { get; private set; }
        public bool Value { get; private set; }
    }

    public delegate void TreeCheckedEventHandler(object sender, TreeCheckedEventArgs e);

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

    #endregion

    #region 基础

    public class VEventArgs<T> : EventArgs
    {
        public VEventArgs(T value)
        {
            Value = value;
        }
        public T Value { get; private set; }
    }


    public class VMEventArgs<T> : MouseEventArgs
    {
        public T Item { get; private set; }
        public VMEventArgs(T item, MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Item = item;
        }
    }

    #endregion
}
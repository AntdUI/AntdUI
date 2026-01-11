// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public class TableCheckEventArgs : ITableEventArgs
    {
        public TableCheckEventArgs(bool value, object record, int rowIndex, int columnIndex, Column column) : base(record, rowIndex, columnIndex, column)
        {
            Value = value;
        }

        /// <summary>
        /// 数值
        /// </summary>
        public bool Value { get; private set; }
    }
    public class TableClickEventArgs : ITableMouseNullEventArgs
    {
        public TableClickEventArgs(object record, RowType rowType, int rowIndex, int columnIndex, Column? column, Rectangle rect, MouseEventArgs e) : base(record, rowType, rowIndex, columnIndex, column, e)
        {
            Rect = rect;
        }

        /// <summary>
        /// 表格区域
        /// </summary>
        public Rectangle Rect { get; private set; }
    }
    public class TableHoverEventArgs : ITableMouseNullEventArgs
    {
        public TableHoverEventArgs(object record, RowType rowType, int rowIndex, int columnIndex, Column? column, Rectangle? rect, MouseEventArgs e) : base(record, rowType, rowIndex, columnIndex, column, e)
        {
            Rect = rect;
        }

        public TableHoverEventArgs(MouseEventArgs e) : base(e)
        {
        }

        /// <summary>
        /// 表格区域
        /// </summary>
        public Rectangle? Rect { get; private set; }
    }
    public class TableButtonEventArgs : TableClickEventArgs
    {
        public TableButtonEventArgs(CellLink btn, object record, RowType rowType, int rowIndex, int columnIndex, Column column, Rectangle rect, MouseEventArgs e) : base(record, rowType, rowIndex, columnIndex, column, rect, e)
        {
            Btn = btn;
        }

        /// <summary>
        /// 触发按钮
        /// </summary>
        public CellLink Btn { get; private set; }
    }

    #region 编辑模式

    public class TableEventArgs : ITableEventArgs
    {
        public TableEventArgs(object? value, object record, int rowIndex, int columnIndex, Column column) : base(record, rowIndex, columnIndex, column)
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
        public TableBeginEditInputStyleEventArgs(object? value, object record, int rowIndex, int columnIndex, Column column, Input input) : base(record, rowIndex, columnIndex, column)
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

        public T Set<T>(T input, Action<Result<T>>? call = null) where T : Input
        {
            SetInput(input);
            if (call == null) return input;
            Call = e => call(new Result<T>(input, Column, e));
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
                if (input.ReadOnly) input.BackColor = Colour.BorderSecondary.Get(nameof(Table));
            }
            Input = input;
        }

        public class Result<T> : ITableEventArgs
        {
            public Result(T input, Column column, TableEndEditEventArgs e) : base(e.Record, e.RowIndex, e.ColumnIndex, column)
            {
                Input = input;
            }

            public T Input { get; private set; }
        }
    }

    public class TableEndEditEventArgs : ITableEventArgs
    {
        public TableEndEditEventArgs(string value, object record, int rowIndex, int columnIndex, Column column) : base(record, rowIndex, columnIndex, column)
        {
            Value = value;
        }

        /// <summary>
        /// 修改后值
        /// </summary>
        public string Value { get; private set; }
    }

    public class TableEndValueEditEventArgs : ITableEventArgs
    {
        public TableEndValueEditEventArgs(object? value, object record, int rowIndex, int columnIndex, Column column) : base(record, rowIndex, columnIndex, column)
        {
            Value = value;
        }

        /// <summary>
        /// 修改后值
        /// </summary>
        public object? Value { get; private set; }
    }

    /// <summary>
    /// CellEditEnter 事件参数
    /// </summary>
    public class TableCellEditEnterEventArgs : EventArgs
    {
        public TableCellEditEnterEventArgs(object record, int rowIndex, int columnIndex, Column column)
        {
            Record = record;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Column = column;
        }

        /// <summary>
        /// 数据对象
        /// </summary>
        public object Record { get; private set; }

        /// <summary>
        /// 行索引
        /// </summary>
        public int RowIndex { get; private set; }

        /// <summary>
        /// 列索引
        /// </summary>
        public int ColumnIndex { get; private set; }

        /// <summary>
        /// 列对象
        /// </summary>
        public Column Column { get; private set; }
    }

    #endregion

    public class TableCellFocusedEventArgs : ITableEventArgs
    {
        public TableCellFocusedEventArgs(object record, RowType rowType, int rowIndex, int columnIndex, Column column, Rectangle rect) : base(record, rowIndex, columnIndex, column)
        {
            RowType = rowType;
            Rect = rect;
        }

        /// <summary>
        /// 表格区域
        /// </summary>
        public Rectangle Rect { get; private set; }

        /// <summary>
        /// 行类型
        /// </summary>
        public RowType RowType { get; private set; }
    }

    public class TableSetRowStyleEventArgs : EventArgs
    {
        public TableSetRowStyleEventArgs(object record, int rowIndex, int index)
        {
            Record = record;
            RowIndex = rowIndex;
            Index = index;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object Record { get; private set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// 行序号
        /// </summary>
        public int RowIndex { get; private set; }
    }

    #region 列调整
    public class TableColumnIndexChangedEventArgs : EventArgs
    {
        public TableColumnIndexChangedEventArgs(int source, int sourceReal, int target) : base()
        {
            IndexSource = source;
            IndexSourceReal = sourceReal;
            IndexTarget = target;
        }
        /// <summary>
        /// 原位置
        /// </summary>
        public int IndexSource { get; private set; }
        /// <summary>
        /// 真实原位置索引
        /// </summary>
        public int IndexSourceReal { get; private set; }
        /// <summary>
        /// 目标位置
        /// </summary>
        public int IndexTarget { get; private set; }

    }

    public class TableColumnIndexChangingEventArgs : TableColumnIndexChangedEventArgs
    {
        public TableColumnIndexChangingEventArgs(int source, int sourceReal, int target) : base(source, sourceReal, target) { }
        /// <summary>
        /// 是否取消拖放
        /// </summary>
        public bool Cancel { set; get; }

        #region 设置

        public TableColumnIndexChangingEventArgs SetCancel(bool value = true)
        {
            Cancel = value;
            return this;
        }

        #endregion
    }

    #endregion

    #region 绘制

    public class TablePaintEventArgs : EventArgs
    {
        public TablePaintEventArgs(Canvas canvas, Rectangle rect, Rectangle rectreal, object record, RowType rowType, int rowIndex, int index, Column column)
        {
            g = canvas;
            Rect = rect;
            RectReal = rectreal;
            Record = record;
            RowType = rowType;
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
        public object Record { get; private set; }

        /// <summary>
        /// 行类型
        /// </summary>
        public RowType RowType { get; private set; }

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
        public TablePaintBeginEventArgs(Canvas canvas, Rectangle rect, Rectangle rectreal, object record, RowType rowType, int rowIndex, int index, Column column) : base(canvas, rect, rectreal, record, rowType, rowIndex, index, column) { }

        /// <summary>
        /// 是否处理
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// 单元格前景色
        /// </summary>
        public SolidBrush? CellFore { get; set; }

        /// <summary>
        /// 单元格背景笔刷 (支持常用SolidBrush, HatchBrush, TextureBrush,LinearGradientBrush...)
        /// </summary>
        public Brush? CellBack { get; set; }

        /// <summary>
        /// 单元格字体
        /// </summary>
        public Font? CellFont { get; set; }

        #region 设置

        public TablePaintBeginEventArgs SetHandled(bool value = true)
        {
            Handled = value;
            return this;
        }
        public TablePaintBeginEventArgs SetFore(Color? value)
        {
            CellFore?.Dispose();
            if (value.HasValue) CellFore = new SolidBrush(value.Value);
            else CellFore = null;
            return this;
        }
        public TablePaintBeginEventArgs SetFore(SolidBrush? value)
        {
            CellFore?.Dispose();
            CellFore = value;
            return this;
        }
        public TablePaintBeginEventArgs SetBack(Color? value)
        {
            CellBack?.Dispose();
            if (value.HasValue) CellBack = new SolidBrush(value.Value);
            else CellBack = null;
            return this;
        }
        public TablePaintBeginEventArgs SetBack(Brush? value)
        {
            CellBack?.Dispose();
            CellBack = value;
            return this;
        }
        public TablePaintBeginEventArgs SetFont(Font? value)
        {
            CellFont = value;
            return this;
        }

        #endregion
    }

    public class TablePaintRowEventArgs : EventArgs
    {
        public TablePaintRowEventArgs(Canvas canvas, Rectangle rect, object record, RowType rowType, int rowIndex)
        {
            g = canvas;
            Rect = rect;
            Record = record;
            RowType = rowType;
            RowIndex = rowIndex;
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
        /// 原始行
        /// </summary>
        public object Record { get; private set; }

        /// <summary>
        /// 行类型
        /// </summary>
        public RowType RowType { get; private set; }

        /// <summary>
        /// 行序号
        /// </summary>
        public int RowIndex { get; private set; }
    }

    #endregion

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
        public TableSortTreeEventArgs(object record, int from, int to)
        {
            Record = record;
            From = from;
            To = to;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object Record { get; private set; }

        public int From { get; private set; }

        public int To { get; private set; }

        /// <summary>
        /// 是否处理
        /// </summary>
        public bool Handled { get; set; }

        #region 设置

        public TableSortTreeEventArgs SetHandled(bool value = true)
        {
            Handled = value;
            return this;
        }

        #endregion
    }

    public class TableExpandEventArgs : EventArgs
    {
        public TableExpandEventArgs(object record, bool expand)
        {
            Record = record;
            Expand = expand;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object Record { get; private set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool Expand { get; private set; }
    }

    public class ITableMouseEventArgs : MouseEventArgs
    {
        public ITableMouseEventArgs(object record, RowType rowType, int rowIndex, int columnIndex, Column column, MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Record = record;
            RowType = rowType;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Column = column;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object Record { get; private set; }

        /// <summary>
        /// 行类型
        /// </summary>
        public RowType RowType { get; private set; }

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
        public Column Column { get; private set; }
    }

    public class ITableMouseNullEventArgs : MouseEventArgs
    {
        public ITableMouseNullEventArgs(object record, RowType rowType, int rowIndex, int columnIndex, Column? column, MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Record = record;
            RowType = rowType;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Column = column;
        }

        public ITableMouseNullEventArgs(MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            RowIndex = ColumnIndex = -1;
            RowType = RowType.None;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object? Record { get; private set; }

        /// <summary>
        /// 行类型
        /// </summary>
        public RowType RowType { get; private set; }

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

    public class ITableEventArgs : EventArgs
    {
        public ITableEventArgs(object record, int rowIndex, int columnIndex, Column column)
        {
            Record = record;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Column = column;
        }

        /// <summary>
        /// 原始行
        /// </summary>
        public object Record { get; private set; }
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
        public Column Column { get; private set; }
    }

    #region 筛选

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
        public FilterOption? Option => Column.Filter;

        /// <summary>
        /// 当前列的自定义数据源
        /// </summary>
        public System.Collections.Generic.IList<object>? CustomSource { get; set; }

        /// <summary>
        /// 筛选栏字体
        /// </summary>
        public Font? Font { get; set; }

        /// <summary>
        /// 筛选栏高度
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// 是否取消弹出
        /// </summary>
        public bool Cancel { get; set; }

        #region 设置

        public TableFilterPopupBeginEventArgs SetCustomSource(System.Collections.Generic.IList<object>? value)
        {
            CustomSource = value;
            return this;
        }
        public TableFilterPopupBeginEventArgs SetCustomSource(params object[] value)
        {
            CustomSource = value;
            return this;
        }
        public TableFilterPopupBeginEventArgs SetCancel(bool value = true)
        {
            Cancel = value;
            return this;
        }
        public TableFilterPopupBeginEventArgs SetFont(Font? value)
        {
            Font = value;
            return this;
        }
        public TableFilterPopupBeginEventArgs SetHeight(int? value)
        {
            Height = value;
            return this;
        }

        #endregion
    }

    public class TableFilterDataChangedEventArgs : EventArgs
    {
        public TableFilterDataChangedEventArgs(object[] records)
        {
            Records = records;
        }

        /// <summary>
        /// 筛选后的记录
        /// </summary>
        public object[] Records { get; internal set; }
    }

    public class TableFilterPopupEndEventArgs : TableFilterDataChangedEventArgs
    {
        public TableFilterPopupEndEventArgs(FilterOption option, object[]? records) : base(records)
        {
            Option = option;
        }

        /// <summary>
        /// 当前筛选参数
        /// </summary>
        public FilterOption Option { get; private set; }

        /// <summary>
        /// 是否取消弹出
        /// </summary>
        public bool Cancel { get; set; }

        #region 设置

        public TableFilterPopupEndEventArgs SetCancel(bool value = true)
        {
            Cancel = value;
            return this;
        }

        #endregion
    }

    #endregion

    #region 自定义汇总

    public delegate void CustomSummaryEventHandler(object sender, TableCustomSummaryEventArgs e);
    public class TableCustomSummaryEventArgs
    {
        public TableCustomSummaryEventArgs(Column column, object record, int rowIndex, bool end)
        {
            Column = column;
            Record = record;
            RowIndex = rowIndex;
            Finalize = end;
        }
        /// <summary>
        /// 当前汇总的列
        /// </summary>
        public Column Column { get; private set; }
        /// <summary>
        /// 汇总的行对象 (DataRow, IList,IRow[] (Finalize=true时), ...)
        /// </summary>
        public object Record { get; private set; }
        /// <summary>
        /// 当前汇总行索引
        /// </summary>
        public int RowIndex { get; private set; }
        /// <summary>
        /// 是否为完成时的汇总运算
        /// </summary>
        public bool Finalize { get; private set; }
        /// <summary>
        /// 获取或设置自定义汇总值
        /// </summary>
        public double? TotalValue { get; set; }
    }

    #endregion
}
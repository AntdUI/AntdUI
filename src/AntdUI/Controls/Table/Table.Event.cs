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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        /// <summary>
        /// 选中改变事件
        /// </summary>
        public delegate void CheckEventHandler(object sender, TableCheckEventArgs e);

        /// <summary>
        /// 点击事件
        /// </summary>
        public delegate void ClickEventHandler(object sender, TableClickEventArgs e);

        /// <summary>
        /// 移动事件
        /// </summary>
        public delegate void HoverEventHandler(object sender, TableHoverEventArgs e);

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        public delegate void ClickButtonEventHandler(object sender, TableButtonEventArgs e);

        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category("行为")]
        public event CheckEventHandler? CheckedChanged;

        protected virtual void OnCheckedChanged(bool value, object record, int rowIndex, int columnIndex, Column column) => CheckedChanged?.Invoke(this, new TableCheckEventArgs(value, record, rowIndex, columnIndex, column));

        public class CheckStateEventArgs : EventArgs
        {
            public CheckStateEventArgs(ColumnCheck column, CheckState value)
            {
                Column = column;
                Value = value;
            }

            /// <summary>
            /// 触发表头对象
            /// </summary>
            public ColumnCheck Column { get; private set; }

            /// <summary>
            /// 数值
            /// </summary>
            public CheckState Value { get; private set; }
        }

        /// <summary>
        /// CheckState类型事件
        /// </summary>
        public delegate void CheckStateEventHandler(object sender, CheckStateEventArgs e);

        /// <summary>
        /// 全局 CheckState 属性值更改时发生
        /// </summary>
        [Description("全局 CheckState 属性值更改时发生"), Category("行为")]
        public event CheckStateEventHandler? CheckedOverallChanged;

        internal void OnICheckedOverallChanged(ColumnCheck column, CheckState checkState) => OnCheckedOverallChanged(column, checkState);
        protected virtual void OnCheckedOverallChanged(ColumnCheck column, CheckState checkState) => CheckedOverallChanged?.Invoke(this, new CheckStateEventArgs(column, checkState));

        /// <summary>
        /// 单击时发生
        /// </summary>
        [Description("单击时发生"), Category("行为")]
        public event ClickEventHandler? CellClick;

        protected virtual void OnCellClick(object record, RowType rowType, int rowIndex, int columnIndex, Column? column, Rectangle rect, MouseEventArgs e) => CellClick?.Invoke(this, new TableClickEventArgs(record, rowType, rowIndex, columnIndex, column, rect, e));

        /// <summary>
        /// 滑动时发生
        /// </summary>
        [Description("滑动时发生"), Category("行为")]
        public event HoverEventHandler? CellHover;

        protected virtual void OnCellHover(object record, RowType rowType, int rowIndex, int columnIndex, Column? column, Rectangle rect, MouseEventArgs e) => CellHover?.Invoke(this, new TableHoverEventArgs(record, rowType, rowIndex, columnIndex, column, rect, e));

        protected virtual void OnCellHover() => CellHover?.Invoke(this, new TableHoverEventArgs(new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0)));

        /// <summary>
        /// 单击按钮时发生
        /// </summary>
        [Description("单击按钮时发生"), Category("行为")]
        public event ClickButtonEventHandler? CellButtonClick;

        protected virtual void OnCellButtonClick(CellLink btn, object record, RowType rowType, int rowIndex, int columnIndex, Column column, Rectangle rect, MouseEventArgs e) => CellButtonClick?.Invoke(this, new TableButtonEventArgs(btn, record, rowType, rowIndex, columnIndex, column, rect, e));

        /// <summary>
        /// 按下按钮时发生
        /// </summary>
        [Description("按下按钮时发生"), Category("行为")]
        public event ClickButtonEventHandler? CellButtonDown;

        protected virtual void OnCellButtonDown(CellLink btn, object record, RowType rowType, int rowIndex, int columnIndex, Column column, Rectangle rect, MouseEventArgs e) => CellButtonDown?.Invoke(this, new TableButtonEventArgs(btn, record, rowType, rowIndex, columnIndex, column, rect, e));

        /// <summary>
        /// 放下按钮时发生
        /// </summary>
        [Description("放下按钮时发生"), Category("行为")]
        public event ClickButtonEventHandler? CellButtonUp;

        protected virtual void OnCellButtonUp(CellLink btn, object record, RowType rowType, int rowIndex, int columnIndex, Column column, Rectangle rect, MouseEventArgs e) => CellButtonUp?.Invoke(this, new TableButtonEventArgs(btn, record, rowType, rowIndex, columnIndex, column, rect, e));

        /// <summary>
        /// 双击时发生
        /// </summary>
        [Description("双击时发生"), Category("行为")]
        public event ClickEventHandler? CellDoubleClick;

        protected virtual void OnCellDoubleClick(object record, RowType rowType, int rowIndex, int columnIndex, Column? column, Rectangle rect, MouseEventArgs e) => CellDoubleClick?.Invoke(this, new TableClickEventArgs(record, rowType, rowIndex, columnIndex, column, rect, e));

        /// <summary>
        /// 单元格焦点变更后发生
        /// </summary>
        [Description("单元格焦点变更后发生"), Category("行为")]
        public event ClickEventHandler? CellFocused;

        protected virtual void OnCellFocused(object record, RowType rowType, int rowIndex, int columnIndex, Column? column, Rectangle rect, MouseEventArgs e) => CellFocused?.Invoke(this, new TableClickEventArgs(record, rowType, rowIndex, columnIndex, column, rect, e));

        #region 编辑

        /// <summary>
        /// 编辑前事件
        /// </summary>
        public delegate bool BeginEditEventHandler(object sender, TableEventArgs e);

        /// <summary>
        /// 编辑前事件文本框样式
        /// </summary>
        public delegate void BeginEditInputStyleEventHandler(object sender, TableBeginEditInputStyleEventArgs e);

        /// <summary>
        /// 编辑后事件
        /// </summary>
        public delegate bool EndEditEventHandler(object sender, TableEndEditEventArgs e);

        /// <summary>
        /// 编辑后事件
        /// </summary>
        public delegate bool EndValueEditEventHandler(object sender, TableEndValueEditEventArgs e);

        /// <summary>
        /// 编辑后事件
        /// </summary>
        public delegate void EndEditCompleteEventHandler(object sender, ITableEventArgs e);

        /// <summary>
        /// 编辑前发生
        /// </summary>
        [Description("编辑前发生"), Category("行为")]
        public event BeginEditEventHandler? CellBeginEdit;

        protected virtual bool OnCellBeginEdit(object? value, object record, int rowIndex, int columnIndex, Column column) => CellBeginEdit?.Invoke(this, new TableEventArgs(value, record, rowIndex, columnIndex, column)) ?? true;

        /// <summary>
        /// 编辑前文本框样式发生
        /// </summary>
        [Description("编辑前文本框样式发生"), Category("行为")]
        public event BeginEditInputStyleEventHandler? CellBeginEditInputStyle;

        protected virtual void OnCellBeginEditInputStyle(TableBeginEditInputStyleEventArgs e) => CellBeginEditInputStyle?.Invoke(this, e);

        /// <summary>
        /// 编辑后发生
        /// </summary>
        [Description("编辑后发生"), Category("行为")]
        public event EndEditEventHandler? CellEndEdit;

        protected virtual bool OnCellEndEdit(string value, object record, int rowIndex, int columnIndex, Column column) => CellEndEdit?.Invoke(this, new TableEndEditEventArgs(value, record, rowIndex, columnIndex, column)) ?? true;

        /// <summary>
        /// 编辑后发生
        /// </summary>
        [Description("编辑后发生"), Category("行为")]
        public event EndValueEditEventHandler? CellEndValueEdit;

        protected virtual bool OnCellEndValueEdit(object? value, object record, int rowIndex, int columnIndex, Column column) => CellEndValueEdit?.Invoke(this, new TableEndValueEditEventArgs(value, record, rowIndex, columnIndex, column)) ?? true;

        /// <summary>
        /// 编辑完成后发生
        /// </summary>
        [Description("编辑完成后发生"), Category("行为")]
        public event EndEditCompleteEventHandler? CellEditComplete;

        protected virtual void OnCellEditComplete(object record, int rowIndex, int columnIndex, Column column) => CellEditComplete?.Invoke(this, new ITableEventArgs(record, rowIndex, columnIndex, column));

        /// <summary>
        /// 单元格输入模式下按下回车键时发生
        /// </summary>
        public delegate void CellEditEnterEventHandler(object sender, TableCellEditEnterEventArgs e);

        /// <summary>
        /// 单元格按下回车键时发生
        /// </summary>
        [Description("单元格输入模式下按下回车键时发生"), Category("行为")]
        public event CellEditEnterEventHandler? CellEditEnter;

        protected virtual void OnCellEditEnter(object sender, TableCellEditEnterEventArgs e)
        {
            CellEditEnter?.Invoke(sender, e);
            Table_CellEditEnter(e);
        }

        #endregion

        #region 绘制

        /// <summary>
        /// 绘制单元格时发生
        /// </summary>
        public delegate void CellPaintEventHandler(object sender, TablePaintEventArgs e);
        public delegate void CellPaintRowEventHandler(object sender, TablePaintRowEventArgs e);

        /// <summary>
        /// 绘制单元格之前发生
        /// </summary>
        public delegate void CellPaintBeginEventHandler(object sender, TablePaintBeginEventArgs e);

        /// <summary>
        /// 绘制行时发生
        /// </summary>
        [Description("绘制行时发生"), Category("行为")]
        public event CellPaintRowEventHandler? RowPaint;

        protected virtual void OnRowPaint(Canvas canvas, Rectangle rect, object record, RowType rowType, int rowIndex) => RowPaint?.Invoke(this, new TablePaintRowEventArgs(canvas, rect, record, rowType, rowIndex));

        /// <summary>
        /// 绘制行前发生
        /// </summary>
        [Description("绘制行前发生"), Category("行为")]
        public event CellPaintRowEventHandler? RowPaintBegin;

        protected virtual void OnRowPaintBegin(Canvas canvas, Rectangle rect, object record, RowType rowType, int rowIndex) => RowPaintBegin?.Invoke(this, new TablePaintRowEventArgs(canvas, rect, record, rowType, rowIndex));

        /// <summary>
        /// 绘制单元格时发生
        /// </summary>
        [Description("绘制单元格时发生"), Category("行为")]
        public event CellPaintEventHandler? CellPaint;

        protected virtual void OnCellPaint(Canvas canvas, Rectangle rect, Rectangle rectreal, object record, RowType rowType, int rowIndex, int index, Column column) => CellPaint?.Invoke(this, new TablePaintEventArgs(canvas, rect, rectreal, record, rowType, rowIndex, index, column));

        /// <summary>
        /// 绘制单元格之前发生
        /// </summary>
        [Description("绘制单元格之前发生"), Category("行为")]
        public event CellPaintBeginEventHandler? CellPaintBegin;

        public delegate CellStyleInfo? SetRowStyleEventHandler(object sender, TableSetRowStyleEventArgs e);
        /// <summary>
        /// 设置行样式
        /// </summary>
        public event SetRowStyleEventHandler? SetRowStyle;

        protected virtual CellStyleInfo? OnSetRowStyle(object record, int rowIndex, int index) => SetRowStyle?.Invoke(this, new TableSetRowStyleEventArgs(record, rowIndex, index));

        public class CellStyleInfo
        {
            /// <summary>
            /// 背景颜色
            /// </summary>
            public Color? BackColor { get; set; }

            /// <summary>
            /// 文字颜色
            /// </summary>
            public Color? ForeColor { get; set; }
        }

        #endregion

        /// <summary>
        /// 选中变化后发生
        /// </summary>
        [Description("选中变化后发生"), Category("行为")]
        public event EventHandler? SelectIndexChanged;

        protected virtual void OnSelectIndexChanged() => SelectIndexChanged?.Invoke(this, EventArgs.Empty);

        #region 排序/拖拽

        /// <summary>
        /// 行排序时发生
        /// </summary>
        [Description("行排序时发生"), Category("行为")]
        public event IntEventHandler? SortRows;

        protected virtual void OnSortRows(int e) => SortRows?.Invoke(this, new IntEventArgs(e));

        /// <summary>
        /// 树行排序时发生
        /// </summary>
        [Description("树行排序时发生"), Category("行为")]
        public event SortTreeEventHandler? SortRowsTree;

        protected virtual bool OnSortRowsTree(object record, int from, int to)
        {
            if (SortRowsTree == null) return false;
            var arge = new TableSortTreeEventArgs(record, from, to);
            SortRowsTree(this, arge);
            return arge.Handled;
        }

        public delegate void SortTreeEventHandler(object sender, TableSortTreeEventArgs e);

        public delegate bool SortModeEventHandler(object sender, TableSortModeEventArgs e);

        /// <summary>
        /// 点击排序后发生
        /// </summary>
        [Description("点击排序后发生"), Category("行为")]
        public event SortModeEventHandler? SortModeChanged;

        protected virtual bool OnSortModeChanged(SortMode sortMode, Column column) => SortModeChanged?.Invoke(this, new TableSortModeEventArgs(sortMode, column)) ?? false;

        /// <summary>
        /// 列拖放新位置前事件
        /// </summary>
        public delegate void ColumnIndexChangingEventHandler(object sender, TableColumnIndexChangingEventArgs e);

        /// <summary>
        /// 列拖放新位置后事件
        /// </summary>
        public delegate void ColumnIndexChangedEventHandler(object sender, TableColumnIndexChangedEventArgs e);

        /// <summary>
        /// 列拖放到新位置时发生 (Cancel=true时取消)
        /// </summary>
        [Description("列拖放到新位置时发生 (Cancel=true时取消)"), Category("行为")]
        public event ColumnIndexChangingEventHandler? ColumnIndexChanging;
        /// <summary>
        /// 列拖放到新位置后发生
        /// </summary>
        [Description("列拖放到新位置后发生"), Category("行为")]
        public event ColumnIndexChangedEventHandler? ColumnIndexChanged;

        /// <summary>
        /// 自定义排序
        /// </summary>
        [Description("自定义排序"), Category("行为")]
        public event Comparison<string>? CustomSort;

        #endregion

        /// <summary>
        /// 展开事件
        /// </summary>
        public delegate void ExpandEventHandler(object sender, TableExpandEventArgs e);

        /// <summary>
        /// 展开改变时发生
        /// </summary>
        [Description("展开改变时发生"), Category("行为")]
        public event ExpandEventHandler? ExpandChanged;

        #region 筛选

        /// <summary>
        /// Table的列筛选关闭前的处理事件
        /// </summary>
        /// <param name="sender">Table</param>
        /// <param name="e">事件参数</param>
        public delegate void TableFilterPopupEndEventHandler(object sender, TableFilterPopupEndEventArgs e);

        /// <summary>
        /// Table的列筛选弹出前的数据处理事件
        /// </summary>
        /// <param name="sender">Table</param>
        /// <param name="e">事件参数</param>
        public delegate void TableFilterPopupBeginEventHandler(object sender, TableFilterPopupBeginEventArgs e);

        /// <summary>
        /// Table的筛选数据变更事件
        /// </summary>
        /// <param name="sender">Table</param>
        /// <param name="e">事件参数</param>
        public delegate void TableFilterDataChangedEventHandler(object sender, TableFilterDataChangedEventArgs e);

        /// <summary>
        /// <summary>
        /// 筛选窗口弹出前发生
        /// </summary>
        [Description("筛选窗口弹出前发生"), Category("行为")]
        public event TableFilterPopupBeginEventHandler? FilterPopupBegin;
        /// <summary>
        /// 筛选窗口关闭前发生
        /// </summary>
        [Description("筛选窗口关闭前发生"), Category("行为")]
        public event TableFilterPopupEndEventHandler? FilterPopupEnd;

        /// <summary>
        /// 筛选数据变更后发生
        /// </summary>
        [Description("筛选数据变更后发生"), Category("行为")]
        public event TableFilterDataChangedEventHandler? FilterDataChanged;

        /// <summary>
        /// 每行或行自定义汇总计算结束时发生
        /// </summary>
        [Description("每行或行自定义汇总计算结束时发生"), Category("数据")]
        public event CustomSummaryEventHandler? CustomSummaryCalculate;

        /// <summary>
        /// 内置汇总功能切换
        /// </summary>
        public delegate void SummaryCustomizeChangedEventHandler(object sender, BoolEventArgs e);

        /// <summary>
        /// 内置/外部汇总栏切换后发生
        /// </summary>
        [Description("内置/外部汇总栏切换后发生"), Category("行为")]
        public event SummaryCustomizeChangedEventHandler? SummaryCustomizeChanged;

        #endregion

        public class CELLDB
        {
            public CELLDB(CELL _cell, int _r_x, int _r_y, int _offset_x, int _offset_xi, int _offset_y, int _i_row, int _i_cel, Column _col)
            {
                cell = _cell;
                x = _r_x;
                y = _r_y;
                offset_x = _offset_x;
                offset_xi = _offset_xi;
                offset_y = _offset_y;
                i_row = _i_row;
                i_cel = _i_cel;
                col = _col;
            }

            public CELL cell { get; set; }

            public int x { get; set; }
            public int y { get; set; }
            public int offset_x { get; set; }
            public int offset_xi { get; set; }
            public int offset_y { get; set; }
            public int i_row { get; set; }
            public int i_cel { get; set; }
            public Column col { get; set; }
            public CELLDBMode mode { get; set; }
        }
        public enum CELLDBMode : int
        {
            None = 0,
            /// <summary>
            /// 表头
            /// </summary>
            Column = 1,
            /// <summary>
            /// 浮动表头
            /// </summary>
            ColumnFixed = 2,
            /// <summary>
            /// 总结栏
            /// </summary>
            Summary = 3
        }
    }
}
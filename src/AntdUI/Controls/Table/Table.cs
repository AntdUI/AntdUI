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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Table 表格
    /// </summary>
    /// <remarks>展示行列数据。</remarks>
    [Description("Table 表格")]
    [DefaultEvent("CellClick")]
    [ToolboxItem(true)]
    public partial class Table : IControl, IEventListener, IScrollBar
    {
        #region 属性

        ColumnCollection? columns;
        /// <summary>
        /// 表格列的配置
        /// </summary>
        [Browsable(false), Description("表格列的配置"), Category("数据"), DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColumnCollection Columns
        {
            get
            {
                columns ??= new ColumnCollection();
                columns.table = this;
                return columns;
            }
            set
            {
                if (columns == value) return;
                SortHeader = null;
                row_cache = null;
                columns = value;
                if (LoadLayout()) Invalidate();
                if (value == null) return;
                value.table = this;
                ExtractHeaderFixed();
                OnPropertyChanged(nameof(Columns));
            }
        }

        object? dataSource;
        /// <summary>
        /// 数据数组
        /// </summary>
        [Browsable(false), Description("数据数组"), Category("数据"), DefaultValue(null)]
        public object? DataSource
        {
            get => dataSource;
            set
            {
                enableDir.Clear();
                CellRanges = null;
                dataSource = value;
                SortData = null;
                focusedCell = null;
                ScrollBar.Clear();
                ExtractHeaderFixed();
                ExtractData();
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(DataSource));
            }
        }

        object? summary;
        /// <summary>
        /// 总结栏
        /// </summary>
        [Browsable(false), Description("总结栏"), Category("数据"), DefaultValue(null)]
        public object? Summary
        {
            get => summary;
            set
            {
                summary = value;
                if (ExtractDataSummary()) ExtractData();
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(Summary));
            }
        }

        /// <summary>
        /// 获取指定行的数据
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns>行</returns>
        public IRow? this[int index]
        {
            get
            {
                if (dataTmp == null || dataTmp.rows.Length == 0) return null;
                if (index < 0 || dataTmp.rows.Length - 1 < index) return null;
                if (SortData == null) return dataTmp.rows[index];
                return dataTmp.rows[SortData[index]];
            }
        }

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
                OnPropertyChanged(nameof(ForeColor));
            }
        }

        bool fixedHeader = true;
        /// <summary>
        /// 固定表头
        /// </summary>
        [Description("固定表头"), Category("外观"), DefaultValue(true)]
        public bool FixedHeader
        {
            get => fixedHeader;
            set
            {
                if (fixedHeader == value) return;
                fixedHeader = value;
                Invalidate();
                OnPropertyChanged(nameof(FixedHeader));
            }
        }

        bool visibleHeader = true;
        /// <summary>
        /// 显示表头
        /// </summary>
        [Description("显示表头"), Category("外观"), DefaultValue(true)]
        public bool VisibleHeader
        {
            get => visibleHeader;
            set
            {
                if (visibleHeader == value) return;
                visibleHeader = value;
                ScrollBar.RB = !value;
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(VisibleHeader));
            }
        }

        bool enableHeaderResizing = false;
        /// <summary>
        /// 手动调整列头宽度
        /// </summary>
        [Description("手动调整列头宽度"), Category("行为"), DefaultValue(false)]
        public bool EnableHeaderResizing
        {
            get => enableHeaderResizing;
            set
            {
                if (enableHeaderResizing == value) return;
                enableHeaderResizing = value;
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(EnableHeaderResizing));
            }
        }

        /// <summary>
        /// 列拖拽排序
        /// </summary>
        [Description("列拖拽排序"), Category("行为"), DefaultValue(false)]
        public bool ColumnDragSort { get; set; }

        /// <summary>
        /// 焦点离开清空选中
        /// </summary>
        [Description("焦点离开清空选中"), Category("行为"), DefaultValue(false)]
        public bool LostFocusClearSelection { get; set; }

        bool bordered = false;
        /// <summary>
        /// 显示列边框
        /// </summary>
        [Description("显示列边框"), Category("外观"), DefaultValue(false)]
        public bool Bordered
        {
            get => bordered;
            set
            {
                if (bordered == value) return;
                bordered = value;
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(Bordered));
            }
        }

        int radius = 0;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(0)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                ScrollBar.Radius = radius = value;
                Invalidate();
                OnPropertyChanged(nameof(Radius));
            }
        }

        /// <summary>
        /// 行复制
        /// </summary>
        [Description("行/列复制"), Category("行为"), DefaultValue(true)]
        public bool ClipboardCopy { get; set; } = true;

        /// <summary>
        /// 是否启用单元格复制
        /// </summary>
        [Description("是否启用单元格复制"), Category("行为"), DefaultValue(false)]
        public bool ClipboardCopyFocusedCell { get; set; }

        /// <summary>
        /// 列宽自动调整模式
        /// </summary>
        [Description("列宽自动调整模式"), Category("行为"), DefaultValue(ColumnsMode.Auto)]
        public ColumnsMode AutoSizeColumnsMode { get; set; } = ColumnsMode.Auto;

        /// <summary>
        /// 虚拟模式
        /// </summary>
        [Description("虚拟模式"), Category("外观"), DefaultValue(false)]
        public bool VirtualMode { get; set; }

        #region 间距

        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(null)]
        public int? Gap
        {
            get
            {
                if (_gap.Width == _gap.Height) return _gap.Width;
                return null;
            }
            set
            {
                int v = value ?? 12;
                Gaps = new Size(v, v);
            }
        }

        Size _gap = new Size(12, 12);
        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(typeof(Size), "12, 12")]
        public Size Gaps
        {
            get => _gap;
            set
            {
                if (_gap == value) return;
                _gap = value;
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(Gaps));
            }
        }

        /// <summary>
        /// 单元格内间距
        /// </summary>
        [Description("单元格内间距"), Category("外观"), DefaultValue(6)]
        public int? GapCell { get; set; } = 6;

        [Description("单元格调整高度"), Category("边框"), DefaultValue(null)]
        public bool? CellImpactHeight { get; set; }

        int _gapTree = 12;
        /// <summary>
        /// 树间距
        /// </summary>
        [Description("树间距"), Category("外观"), DefaultValue(12)]
        public int GapTree
        {
            get => _gapTree;
            set
            {
                if (_gapTree == value) return;
                _gapTree = value;
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(GapTree));
            }
        }

        int? rowHeight;
        /// <summary>
        /// 行高
        /// </summary>
        [Description("行高"), Category("外观"), DefaultValue(null)]
        public int? RowHeight
        {
            get => rowHeight;
            set
            {
                if (rowHeight == value) return;
                rowHeight = value;
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(RowHeight));
            }
        }

        int? rowHeightHeader;
        /// <summary>
        /// 表头行高
        /// </summary>
        [Description("表头行高"), Category("外观"), DefaultValue(null)]
        public int? RowHeightHeader
        {
            get => rowHeightHeader;
            set
            {
                if (rowHeightHeader == value) return;
                rowHeightHeader = value;
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(RowHeightHeader));
            }
        }

        #endregion

        #region 大小

        int _checksize = 16;
        /// <summary>
        /// 复选框大小
        /// </summary>
        [Description("复选框大小"), Category("外观"), DefaultValue(16)]
        public int CheckSize
        {
            get => _checksize;
            set
            {
                if (_checksize == value) return;
                _checksize = value;
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(CheckSize));
            }
        }

        int _switchsize = 16;
        /// <summary>
        /// 开关大小
        /// </summary>
        [Description("开关大小"), Category("外观"), DefaultValue(16)]
        public int SwitchSize
        {
            get => _switchsize;
            set
            {
                if (_switchsize == value) return;
                _switchsize = value;
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(SwitchSize));
            }
        }

        /// <summary>
        /// 树开关按钮大小
        /// </summary>
        [Description("树开关按钮大小"), Category("外观"), DefaultValue(16)]
        public int TreeButtonSize { get; set; } = 16;

        /// <summary>
        /// 拖拽手柄大小
        /// </summary>
        [Description("拖拽手柄大小"), Category("外观"), DefaultValue(24)]
        public int DragHandleSize { get; set; } = 24;

        /// <summary>
        /// 拖拽手柄图标大小
        /// </summary>
        [Description("拖拽手柄图标大小"), Category("外观"), DefaultValue(14)]
        public int DragHandleIconSize { get; set; } = 14;

        /// <summary>
        /// 排序大小
        /// </summary>
        [Description("排序大小"), Category("外观"), DefaultValue(null)]
        public int? SortOrderSize { get; set; }

        #endregion

        #region 焦点

        /// <summary>
        /// 焦点列样式
        /// </summary>
        [Description("焦点列样式"), Category("外观"), DefaultValue(null)]
        public TableCellFocusedStyle? CellFocusedStyle { get; set; }

        /// <summary>
        /// 焦点列背景色
        /// </summary>
        [Description("焦点列背景色"), Category("外观"), DefaultValue(null)]
        public Color? CellFocusedBg { get; set; }

        /// <summary>
        /// 焦点列边框色
        /// </summary>
        [Description("焦点列边框色"), Category("外观"), DefaultValue(null)]
        public Color? CellFocusedBorder { get; set; }

        /// <summary>
        /// 当前获得焦点的列
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Column? FocusedColumn => focusedCell?.COLUMN;

        /// <summary>
        /// 当前获得焦点的行
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object? FocusedRow => focusedCell?.ROW.RECORD;

        CELL? focusedCell;
        /// <summary>
        /// 当前获得焦点的单元格
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CELL? FocusedCell
        {
            get => focusedCell;
            private set
            {
                if (focusedCell == value) return;
                focusedCell = value;
                if (value != null) CellFocused?.Invoke(this, new TableClickEventArgs(value.ROW.RECORD, value.ROW.INDEX, value.INDEX, value.COLUMN, value.RECT, new MouseEventArgs(MouseButtons.Left, 1, value.RECT.X, value.RECT.Y, 1)));
                Invalidate();
            }
        }

        #endregion

        #region 为空

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

        bool emptyHeader = false;
        /// <summary>
        /// 空是否显示表头
        /// </summary>
        [Description("空是否显示表头"), Category("外观"), DefaultValue(false)]
        public bool EmptyHeader
        {
            get => emptyHeader;
            set
            {
                if (emptyHeader == value) return;
                emptyHeader = value;
                if (LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(EmptyHeader));
            }
        }

        #endregion

        #region 主题

        Color? rowHoverBg;
        /// <summary>
        /// 表格行悬浮背景色
        /// </summary>
        [Description("表格行悬浮背景色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? RowHoverBg
        {
            get => rowHoverBg;
            set
            {
                if (rowHoverBg == value) return;
                rowHoverBg = value;
                OnPropertyChanged(nameof(RowHoverBg));
            }
        }

        Color? rowSelectedBg;
        /// <summary>
        /// 表格行选中背景色
        /// </summary>
        [Description("表格行选中背景色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? RowSelectedBg
        {
            get => rowSelectedBg;
            set
            {
                if (rowSelectedBg == value) return;
                rowSelectedBg = value;
                if (selectedIndex.Length > 0) Invalidate();
                OnPropertyChanged(nameof(RowSelectedBg));
            }
        }

        Color? rowSelectedFore;
        /// <summary>
        /// 表格行选中字色
        /// </summary>
        [Description("表格行选中字色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? RowSelectedFore
        {
            get => rowSelectedFore;
            set
            {
                if (rowSelectedFore == value) return;
                rowSelectedFore = value;
                if (selectedIndex.Length > 0) Invalidate();
                OnPropertyChanged(nameof(RowSelectedFore));
            }
        }

        float borderWidth = 1F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(1F)]
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                Invalidate();
            }
        }

        Color? borderColor;
        /// <summary>
        /// 表格边框颜色
        /// </summary>
        [Description("表格边框颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                Invalidate();
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        /// <summary>
        /// 单元格边框宽度
        /// </summary>
        [Description("单元格边框宽度"), Category("边框"), DefaultValue(1F)]
        public float BorderCellWidth { get; set; } = 1F;

        #region 表头

        Font? columnfont;
        /// <summary>
        /// 表头字体
        /// </summary>
        [Description("表头字体"), Category("外观"), DefaultValue(null)]
        public Font? ColumnFont
        {
            get => columnfont;
            set
            {
                if (columnfont == value) return;
                columnfont = value;
                Invalidate();
                OnPropertyChanged(nameof(ColumnFont));
            }
        }

        Color? columnback;
        /// <summary>
        /// 表头背景色
        /// </summary>
        [Description("表头背景色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ColumnBack
        {
            get => columnback;
            set
            {
                if (columnback == value) return;
                columnback = value;
                Invalidate();
                OnPropertyChanged(nameof(ColumnBack));
            }
        }

        Color? columnfore;
        /// <summary>
        /// 表头文本色
        /// </summary>
        [Description("表头文本色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ColumnFore
        {
            get => columnfore;
            set
            {
                if (columnfore == value) return;
                columnfore = value;
                Invalidate();
                OnPropertyChanged(nameof(ColumnFore));
            }
        }

        #endregion

        #endregion

        #region 数据

        int[] selectedIndex = new int[0];
        /// <summary>
        /// 选中行（1开始）
        /// </summary>
        [Browsable(false), Description("选中行（1开始）"), Category("数据"), DefaultValue(-1)]
        public int SelectedIndex
        {
            get
            {
                if (selectedIndex.Length > 0) return selectedIndex[0];
                return -1;
            }
            set
            {
                if (SetIndex(value))
                {
                    Invalidate();
                    OnPropertyChanged(nameof(SelectedIndex));
                    SelectIndexChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 选中多行
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor", typeof(UITypeEditor))]
        [Browsable(false), Description("选中多行"), Category("数据")]
        public int[] SelectedIndexs
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex == value) return;
                selectedIndex = value;
                Invalidate();
                OnPropertyChanged(nameof(SelectedIndexs));
                SelectIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        bool SetIndex(int value)
        {
            if (selectedIndex.Length < 1)
            {
                if (value == -1) return false;
            }
            if (value == -1)
            {
                if (selectedIndex.Length == 0 || string.Join("", selectedIndex) == value.ToString()) return false;
                selectedIndex = new int[0];
                return true;
            }
            else
            {
                if (string.Join("", selectedIndex) == value.ToString()) return false;
                selectedIndex = new int[1] { value };
                if (focusedCell != null && rows?.Length > value) FocusedCell = rows[value].cells[focusedCell.INDEX];
                return true;
            }
        }
        int[] SetIndexs(int value)
        {
            var list = new List<int>(selectedIndex.Length + 1);
            list.AddRange(selectedIndex);
            if (list.Contains(value)) list.Remove(value);
            else list.Add(value);
            return list.ToArray();
        }
        int[] SetIndexs(int start, int end)
        {
            var list = new List<int>(end - start + 1);
            for (int i = start; i <= end; i++) list.Add(i);
            return list.ToArray();
        }

        /// <summary>
        /// 多选行
        /// </summary>
        [Description("多选行"), Category("行为"), DefaultValue(false)]
        public bool MultipleRows { get; set; }

        /// <summary>
        /// 返回当前树表格字段名
        /// </summary>
        public string? KeyTreeCurrent
        {
            get
            {
                foreach (Column col in Columns)
                {
                    if (col.KeyTree != null && !string.IsNullOrEmpty(col.KeyTree)) return col.KeyTree;
                }
                return null;
            }
        }

        /// <summary>
        /// 当前视图的数据行数
        /// </summary>
        public int DisplayRowCount
        {
            get
            {
                if (dataTmp == null || dataTmp.rows.Length == 0) return 0;
                int count = dataTmp.rows.Length;
                var keyTree = KeyTreeCurrent;
                if (keyTree == null) return count;
                for (int i = 0; i < count; i++)
                {
                    var value_tree = dataTmp.rows[i][keyTree];
                    if (value_tree is IList list) count += list.Count;
                }
                return count;
            }
        }

        #endregion

        /// <summary>
        /// 默认是否展开
        /// </summary>
        [Description("默认是否展开"), Category("外观"), DefaultValue(false)]
        public bool DefaultExpand { get; set; }

        /// <summary>
        /// 处理快捷键
        /// </summary>
        [Description("处理快捷键"), Category("行为"), DefaultValue(true)]
        public bool HandShortcutKeys { get; set; } = true;

        /// <summary>
        /// 省略文字提示
        /// </summary>
        [Description("省略文字提示"), Category("行为"), DefaultValue(true)]
        public bool ShowTip { get; set; } = true;

        /// <summary>
        /// 超出文字提示配置
        /// </summary>
        [Browsable(false)]
        [Description("超出文字提示配置"), Category("行为"), DefaultValue(null)]
        public TooltipConfig? TooltipConfig { get; set; }

        /// <summary>
        /// 滚动条
        /// </summary>
        [Browsable(false)]
        public ScrollBar ScrollBar;

        #region 编辑模式

        TEditMode editmode = TEditMode.None;
        /// <summary>
        /// 编辑模式
        /// </summary>
        [Description("编辑模式"), Category("行为"), DefaultValue(TEditMode.None)]
        public TEditMode EditMode
        {
            get => editmode;
            set
            {
                if (editmode == value) return;
                editmode = value;
                Invalidate();
                OnPropertyChanged(nameof(EditMode));
            }
        }

        /// <summary>
        /// 编辑模式下的默认文本选择动作
        /// </summary>
        [Description("编辑模式下的默认文本选择动作"), Category("行为"), DefaultValue(TEditSelection.Last)]
        public TEditSelection EditSelection { get; set; } = TEditSelection.Last;

        /// <summary>
        /// 编辑模式输入框样式
        /// </summary>
        [Description("编辑模式输入框样式"), Category("行为"), DefaultValue(TEditInputStyle.Default)]
        public TEditInputStyle EditInputStyle { get; set; } = TEditInputStyle.Default;

        /// <summary>
        /// 编辑模式自动高度
        /// </summary>
        [Description("编辑模式自动高度"), Category("行为"), DefaultValue(false)]
        public bool EditAutoHeight { get; set; }

        /// <summary>
        /// 树表格的箭头样式
        /// </summary>
        [Description("树表格的箭头样式"), Category("行为"), DefaultValue(TableTreeStyle.Button)]
        public TableTreeStyle TreeArrowStyle { get; set; } = TableTreeStyle.Button;

        /// <summary>
        /// 动画时长（ms）
        /// </summary>
        [Description("动画时长（ms）"), Category("行为"), DefaultValue(100)]
        public int AnimationTime { get; set; } = 100;

        #endregion

        bool pauseLayout = false;
        [Browsable(false), Description("暂停布局"), Category("行为"), DefaultValue(false)]
        public bool PauseLayout
        {
            get => pauseLayout;
            set
            {
                if (pauseLayout == value) return;
                pauseLayout = value;
                if (!value && LoadLayout()) Invalidate();
                OnPropertyChanged(nameof(PauseLayout));
            }
        }

        #endregion

        #region 初始化

        public Table() : base(ControlType.Select) { ScrollBar = new ScrollBar(this, true, true, radius, !visibleHeader); }

        protected override void Dispose(bool disposing)
        {
            ThreadState?.Dispose();
            ScrollBar.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <remarks>不适用DataSource为AntList<T>的场景</remarks>
        public override void Refresh()
        {
            ExtractHeaderFixed();
            if (dataSource == null || dataSource is DataTable || dataSource is IList) ExtractData();
            base.Refresh();
            if (LoadLayout()) Invalidate();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <remarks>适用DataSource为AntList<T>的场景</remarks>
        public void Refresh<T>(AntList<T>? list = null)
        {
            if (list == null) Refresh();
            else
            {
                IBinding(list);
                base.Refresh();
            }
        }

        List<int> enableDir = new List<int>();
        /// <summary>
        /// 获取行使能
        /// </summary>
        /// <param name="i">行</param>
        /// <returns>是否禁用</returns>
        public bool GetRowEnable(int i) => enableDir.Contains(i);

        /// <summary>
        /// 设置行使能
        /// </summary>
        /// <param name="i">行</param>
        /// <param name="value">值</param>
        /// <param name="ui">是否刷新ui</param>
        /// <returns>成功失败</returns>
        public void SetRowEnable(int i, bool value = true, bool ui = true)
        {
            if (value)
            {
                if (enableDir.Contains(i)) enableDir.Remove(i);
                else return;
            }
            else
            {
                if (enableDir.Contains(i)) return;
                else enableDir.Add(i);
            }
            if (rows == null) return;
            try
            {
                var selectRow = rows[i + 1];
                selectRow.ENABLE = value;
                if (ui) Invalidate();
            }
            catch { }
        }

        /// <summary>
        /// 滚动到指定行
        /// </summary>
        /// <param name="i">行</param>
        /// <param name="force">是否强制滚动</param>
        /// <returns>返回滚动量</returns>
        public int ScrollLine(int i, bool force = false)
        {
            if (rows == null || !ScrollBar.ShowY) return 0;
            return ScrollLine(i, rows, force);
        }

        /// <summary>
        /// 滚动到指定行
        /// </summary>
        /// <param name="record">行对象</param>
        /// <param name="force">是否强制滚动</param>
        /// <returns>返回滚动量</returns>
        public int ScrollLine(object record, bool force = false)
        {
            if (rows == null || !ScrollBar.ShowY) return 0;
            foreach (var row in rows)
            {
                if (row.RECORD == record)
                {
                    int i = Array.IndexOf(rows, row);
                    if (i < 0) return 0;
                    return ScrollLine(i, rows, force);
                }
            }
            return 0;
        }

        int ScrollLine(int i, RowTemplate[] rows, bool force = false)
        {
            if (!ScrollBar.ShowY) return 0;
            var selectRow = rows[i];
            int sy = ScrollBar.ValueY;
            if (force)
            {
                if (fixedHeader) ScrollBar.ValueY = rows[i].RECT.Y - rows[0].RECT.Height;
                else ScrollBar.ValueY = rows[i].RECT.Y;
                return sy - ScrollBar.ValueY;
            }
            else
            {
                if (visibleHeader && fixedHeader)
                {
                    if (selectRow.RECT.Y - rows[0].RECT.Height < sy || selectRow.RECT.Bottom > sy + rect_read.Height)
                    {
                        if (fixedHeader) ScrollBar.ValueY = rows[i].RECT.Y - rows[0].RECT.Height;
                        else ScrollBar.ValueY = rows[i].RECT.Y;
                        return sy - ScrollBar.ValueY;
                    }
                }
                else if (selectRow.RECT.Y < sy || selectRow.RECT.Bottom > sy + rect_read.Height)
                {
                    if (fixedHeader) ScrollBar.ValueY = rows[i].RECT.Y - rows[0].RECT.Height;
                    else ScrollBar.ValueY = rows[i].RECT.Y;
                    return sy - ScrollBar.ValueY;
                }
            }
            return 0;
        }

        /// <summary>
        /// 复制表格数据
        /// </summary>
        /// <param name="row">行</param>
        public bool CopyData(int row)
        {
            if (rows != null)
            {
                try
                {
                    var _row = rows[row];
                    var vals = new List<string?>(_row.cells.Length);
                    foreach (var cell in _row.cells) vals.Add(cell.ToString());
                    this.ClipboardSetText(string.Join("\t", vals));
                    return true;
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// 复制表格数据
        /// </summary>
        /// <param name="row">行</param>
        public bool CopyData(int[] row)
        {
            if (rows != null)
            {
                try
                {
                    var rowtmp = new List<string?>(row.Length);
                    foreach (var it in row)
                    {
                        var _row = rows[it];
                        var vals = new List<string?>(_row.cells.Length);
                        foreach (var cell in _row.cells) vals.Add(cell.ToString());
                        rowtmp.Add(string.Join("\t", vals));
                    }
                    this.ClipboardSetText(string.Join("\n", rowtmp));
                    return true;
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// 复制表格数据
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        public bool CopyData(int row, int column)
        {
            if (rows != null)
            {
                try
                {
                    var _row = rows[row];
                    var vals = _row.cells[column].ToString();
                    if (vals == null) return false;
                    this.ClipboardSetText(vals);
                    return true;
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// 复制表格数据
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        public bool CopyData(CELL cell)
        {
            if (cell != null)
            {
                try
                {
                    var vals = cell.VALUE?.ToString();
                    if (vals == null) return false;
                    this.ClipboardSetText(vals);
                    return true;
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// 获取排序序号
        /// </summary>
        public int[] SortIndex()
        {
            if (SortData == null)
            {
                if (dataTmp == null || dataTmp.rows.Length == 0) return new int[0];
                var list = new int[dataTmp.rows.Length];
                for (int i = 0; i < dataTmp.rows.Length; i++) list[i] = i;
                return list;
            }
            else return SortData;
        }

        /// <summary>
        /// 设置排序序号
        /// </summary>
        /// <param name="data">序号数据</param>
        public void SetSortIndex(int[]? data)
        {
            SortData = data;
            if (LoadLayout()) Invalidate();
        }

        /// <summary>
        /// 获取排序数据
        /// </summary>
        public object[] SortList()
        {
            if (dataTmp == null || dataTmp.rows.Length == 0) return new object[0];
            if (SortData == null)
            {
                var list = new object[dataTmp.rows.Length];
                for (int i = 0; i < dataTmp.rows.Length; i++) list[i] = dataTmp.rows[i].record;
                return list;
            }
            else
            {
                var list = new List<object>(dataTmp.rows.Length);
                foreach (var i in SortData) list.Add(dataTmp.rows[i].record);
                return list.ToArray();
            }
        }

        /// <summary>
        /// 设置排序数据
        /// </summary>
        public void SetSortList(object[]? data)
        {
            if (data == null)
            {
                SortData = null;
                if (LoadLayout()) Invalidate();
                return;
            }
            if (dataTmp == null || dataTmp.rows.Length == 0) return;
            var list = new List<int>(dataTmp.rows.Length);
            foreach (var it in data)
            {
                int index = SetSortList(it, dataTmp.rows);
                if (index > -1) list.Add(index);
            }
            SortData = list.ToArray();
            if (LoadLayout()) Invalidate();
        }

        int SetSortList(object it, IRow[] rows)
        {
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i].record == it) return i;
            }
            return -1;
        }

        /// <summary>
        /// 获取表头排序序号
        /// </summary>
        public int[] SortColumnsIndex()
        {
            if (SortHeader == null)
            {
                if (columns == null || columns.Count == 0) return new int[0];
                var list = new int[columns.Count];
                for (int i = 0; i < columns.Count; i++) list[i] = i;
                return list;
            }
            else return SortHeader;
        }

        /// <summary>
        /// 设置表头排序序号
        /// </summary>
        /// <param name="data">序号数据</param>
        public void SetSortColumnsIndex(int[]? data)
        {
            SortHeader = data;
            if (LoadLayout()) Invalidate();
        }

        /// <summary>
        /// 获取表头排序数据
        /// </summary>
        public Column[] SortColumnsList()
        {
            if (columns == null || columns.Count == 0) return new Column[0];
            if (SortHeader == null)
            {
                var list = new Column[columns.Count];
                for (int i = 0; i < columns.Count; i++) list[i] = columns[i];
                return list;
            }
            else
            {
                var list = new List<Column>(columns.Count);
                foreach (var i in SortHeader) list.Add(columns[i]);
                return list.ToArray();
            }
        }

        /// <summary>
        /// 获取表头真实索引
        /// </summary>
        public int GetColumnRealIndex(Column column) => column.INDEX_REAL;

        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        public Rectangle CellRectangle(int row, int column)
        {
            if (rows != null)
            {
                try
                {
                    var _row = rows[row];
                    var cel = _row.cells[column];
                    CellContains(rows, false, 0, 0, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out _, out _);
                    return RealRect(cel.RECT, offset_xi, offset_y);
                }
                catch { }
            }
            return Rectangle.Empty;
        }

        /// <summary>
        /// 导出表格数据
        /// </summary>
        /// <param name="enableRender">启用插槽</param>
        /// <param name="toString">使用toString</param>
        public DataTable? ToDataTable(bool enableRender = true, bool toString = true)
        {
            if (dataTmp == null || dataTmp.rows.Length == 0) return null;
            var dt = new DataTable();

            Dictionary<string, Column> dir_columns;

            #region 处理表头

            if (rows == null)
            {
                dir_columns = new Dictionary<string, Column>(0);
                foreach (var column in dataTmp.columns) dt.Columns.Add(new DataColumn(column.key) { Caption = column.text });
            }
            else
            {
                dir_columns = new Dictionary<string, Column>(dataTmp.columns.Length);
                var columns = new Dictionary<string, DataColumn>(dataTmp.columns.Length);
                foreach (var column in dataTmp.columns) columns.Add(column.key, new DataColumn(column.key) { Caption = column.text });
                foreach (TCellColumn item in rows[0].cells)
                {
                    dir_columns.Add(item.COLUMN.Key, item.COLUMN);
                    if (!string.IsNullOrWhiteSpace(item.value) && columns.TryGetValue(item.COLUMN.Key, out var find)) find.Caption = item.value;
                }
                foreach (var item in columns) dt.Columns.Add(item.Value);
            }

            #endregion

            if (toString)
            {
                foreach (var row in dataTmp.rows)
                {
                    var data = new List<object?>(row.cells.Count);
                    foreach (var cell in row.cells)
                    {
                        var obj = row[cell.Key];
                        if (enableRender && dir_columns.TryGetValue(cell.Key, out var column) && column.Render != null) obj = column.Render(obj, row.record, row.i);

                        if (obj is IList<ICell> cells)
                        {
                            var cs = new List<string?>(cells.Count);
                            foreach (var it in cells)
                            {
                                var str = it.ToString();
                                if (str != null) cs.Add(str);
                            }
                            if (cs.Count > 0) data.Add(string.Join(" ", cs));
                            else data.Add(null);
                        }
                        else data.Add(obj);
                    }
                    dt.Rows.Add(data.ToArray());
                }
            }
            else
            {
                foreach (var row in dataTmp.rows)
                {
                    var data = new List<object?>(row.cells.Count);
                    foreach (var cell in row.cells)
                    {
                        var obj = row[cell.Key];
                        if (enableRender && dir_columns.TryGetValue(cell.Key, out var column) && column.Render != null) obj = column.Render(obj, row.record, row.i);
                        data.Add(obj);
                    }
                    dt.Rows.Add(data.ToArray());
                }
            }
            return dt;
        }

        #region 树

        /// <summary>
        /// 展开全部
        /// </summary>
        /// <param name="value">展开或折叠</param>
        public void ExpandAll(bool value = true)
        {
            if (ExpandChanged == null)
            {
                if (value)
                {
                    if (rows == null) return;
                    rows_Expand = new List<object>(rows.Length - 1);
                    for (int i = 1; i < rows.Length; i++)
                    {
                        var record = rows[i].RECORD;
                        if (record == null) continue;
                        rows_Expand.Add(record);
                    }
                }
                else rows_Expand.Clear();
            }
            else
            {
                if (value)
                {
                    if (rows == null) return;
                    var temp = rows_Expand;
                    rows_Expand = new List<object>(rows.Length - 1);
                    rows_Expand.AddRange(temp);
                    for (int i = 1; i < rows.Length; i++)
                    {
                        var record = rows[i].RECORD;
                        if (record == null || rows_Expand.Contains(record)) continue;
                        rows_Expand.Add(record);
                        ExpandChanged(this, new TableExpandEventArgs(record, false));
                    }
                }
                else
                {
                    foreach (var it in rows_Expand) ExpandChanged(this, new TableExpandEventArgs(it, false));
                    rows_Expand.Clear();
                }
            }
            row_cache = null;
            if (LoadLayout()) Invalidate();
        }

        /// <summary>
        /// 展开或折叠
        /// </summary>
        /// <param name="record">元数据</param>
        /// <param name="value">展开或折叠</param>
        public void Expand(object record, bool value = true)
        {
            if (value)
            {
                if (rows_Expand.Contains(record)) return;
                rows_Expand.Add(record);
                ExpandChanged?.Invoke(this, new TableExpandEventArgs(record, true));
            }
            else
            {
                if (rows_Expand.Contains(record))
                {
                    rows_Expand.Remove(record);
                    ExpandChanged?.Invoke(this, new TableExpandEventArgs(record, false));
                }
                else return;
            }
            row_cache = null;
            if (LoadLayout()) Invalidate();
        }

        #endregion

        public CELL? HitTest(int x, int y)
        {
            if (rows == null) return null;
            var cell = CellContains(rows, false, x, y, out _, out _, out _, out _, out _, out _, out _, out _, out _);
            return cell;
        }

        public CELL? HitTest(int x, int y, out int i_row, out int i_cel)
        {
            if (rows == null)
            {
                i_row = i_cel = 0;
                return null;
            }
            var cell = CellContains(rows, false, x, y, out _, out _, out _, out _, out _, out i_row, out i_cel, out var _, out _);
            return cell;
        }

        public CELL? HitTest(int x, int y, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel)
        {
            if (rows == null)
            {
                r_x = r_y = offset_x = offset_xi = offset_y = i_row = i_cel = 0;
                return null;
            }
            var cell = CellContains(rows, false, x, y, out r_x, out r_y, out offset_x, out offset_xi, out offset_y, out i_row, out i_cel, out _, out _);
            return cell;
        }

        public CELL? HitTest(int x, int y, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out int mode)
        {
            if (rows == null)
            {
                mode = 0;
                r_x = r_y = offset_x = offset_xi = offset_y = i_row = i_cel = 0;
                return null;
            }
            var cell = CellContains(rows, false, x, y, out r_x, out r_y, out offset_x, out offset_xi, out offset_y, out i_row, out i_cel, out _, out mode);
            return cell;
        }

        #endregion

        #region 渲染

        public void Invalidate(int row)
        {
            if (ThreadState == null)
            {
                if (rows == null) return;
                var rect = rows[row].RECT;
                int sy = ScrollBar.ValueY;
                Invalidate(new Rectangle(rect.X, rect.Y - sy, rect.Width, rect.Height));
            }
        }
        public void Invalidate(int row, int column)
        {
            if (ThreadState == null)
            {
                if (rows == null) return;
                var rect = rows[row].cells[column].RECT;
                int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
                Invalidate(new Rectangle(rect.X - sx, rect.Y - sy, rect.Width, rect.Height));
            }
        }

        #endregion

        #region 合并单元格

        CellRange[]? CellRanges;
        public void AddMergedRegion(CellRange range)
        {
            if (CellRanges == null) CellRanges = new CellRange[1] { range };
            else
            {
                var tmp = new List<CellRange>(CellRanges.Length + 1);
                tmp.AddRange(CellRanges);
                tmp.Add(range);
                CellRanges = tmp.ToArray();
            }
            Invalidate();
        }
        public void AddMergedRegion(CellRange[] ranges)
        {
            if (CellRanges == null) CellRanges = ranges;
            else
            {
                var tmp = new List<CellRange>(CellRanges.Length + ranges.Length);
                tmp.AddRange(CellRanges);
                tmp.AddRange(ranges);
                CellRanges = tmp.ToArray();
            }
            Invalidate();
        }
        public bool ContainsMergedRegion(CellRange range)
        {
            if (CellRanges == null) return false;
            else
            {
                var tmp = new List<int>(CellRanges.Length);
                foreach (var it in CellRanges) tmp.Add(it.GetHashCode());
                return tmp.Contains(range.GetHashCode());
            }
        }
        public void ClearMergedRegion()
        {
            CellRanges = null;
            Invalidate();
        }

        #endregion

        #region 本地化

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AddListener();
            if (dataOne) LoadLayout();
        }

        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.LANG:
                    if (ColumnsHasLocalization() && LoadLayout()) Invalidate();
                    break;
            }
        }

        bool ColumnsHasLocalization()
        {
            if (columns == null) return false;
            foreach (var column in columns)
            {
                if (column.LocalizationTitle == null) continue;
                return true;
            }
            return false;
        }

        #endregion
    }

    #region 表头

    /// <summary>
    /// 复选框表头
    /// </summary>
    public class ColumnCheck : Column
    {
        public ColumnCheck(string key) : base(key, "")
        {
        }

        public ColumnCheck(string key, string title) : base(key, title)
        {
            NoTitle = false;
        }

        bool _checked = false;
        /// <summary>
        /// 选中状态
        /// </summary>
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                OnCheck();
                CheckState = value ? CheckState.Checked : CheckState.Unchecked;

                PARENT?.CheckAll(INDEX, this, value);
            }
        }

        CheckState checkState = CheckState.Unchecked;
        /// <summary>
        /// 选中状态
        /// </summary>
        public CheckState CheckState
        {
            get => checkState;
            internal set
            {
                if (checkState == value) return;
                checkState = value;
                PARENT?.OnCheckedOverallChanged(this, value);
                bool __checked = value == CheckState.Checked;
                if (_checked != __checked)
                {
                    _checked = __checked;
                    OnCheck();
                }
                if (value != CheckState.Unchecked)
                {
                    checkStateOld = value;
                    PARENT?.Invalidate();
                }
            }
        }

        /// <summary>
        /// 点击时自动改变选中状态
        /// </summary>
        public bool AutoCheck { get; set; } = true;
        public Column SetAutoCheck(bool value = false)
        {
            AutoCheck = value;
            return this;
        }

        void OnCheck()
        {
            ThreadCheck?.Dispose();
            if (PARENT != null && PARENT.IsHandleCreated)
            {
                if (Config.HasAnimation(nameof(Table)))
                {
                    AnimationCheck = true;
                    if (_checked)
                    {
                        ThreadCheck = new ITask(PARENT, () =>
                        {
                            AnimationCheckValue = AnimationCheckValue.Calculate(0.2F);
                            if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                            PARENT.Invalidate();
                            return true;
                        }, 20, () =>
                        {
                            AnimationCheck = false;
                            PARENT.Invalidate();
                        });
                    }
                    else if (checkStateOld == CheckState.Checked && CheckState == CheckState.Indeterminate)
                    {
                        AnimationCheck = false;
                        AnimationCheckValue = 1F;
                        PARENT.Invalidate();
                    }
                    else
                    {
                        ThreadCheck = new ITask(PARENT, () =>
                        {
                            AnimationCheckValue = AnimationCheckValue.Calculate(-0.2F);
                            if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                            PARENT.Invalidate();
                            return true;
                        }, 20, () =>
                        {
                            AnimationCheck = false;
                            PARENT.Invalidate();
                        });
                    }
                }
                else
                {
                    AnimationCheckValue = _checked ? 1F : 0F;
                    PARENT.Invalidate();
                }
            }
        }

        internal bool AnimationCheck = false;
        internal float AnimationCheckValue = 0;

        ITask? ThreadCheck;

        internal CheckState checkStateOld = CheckState.Unchecked;

        internal bool NoTitle { get; set; } = true;

        public Func<bool, object?, int, int, bool>? Call { get; set; }

        /// <summary>
        /// 插槽
        /// </summary>
        public new Func<object?, object, int, object?>? Render { get; }
    }

    /// <summary>
    /// 单选框表头
    /// </summary>
    public class ColumnRadio : Column
    {
        /// <summary>
        /// 单选框表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        public ColumnRadio(string key, string title) : base(key, title)
        {
            Align = ColumnAlign.Center;
        }

        /// <summary>
        /// 点击时自动改变选中状态
        /// </summary>
        public bool AutoCheck { get; set; } = true;
        public Column SetAutoCheck(bool value = false)
        {
            AutoCheck = value;
            return this;
        }

        public Func<bool, object?, int, int, bool>? Call { get; set; }

        /// <summary>
        /// 插槽
        /// </summary>
        public new Func<object?, object, int, object?>? Render { get; }
    }

    /// <summary>
    /// 开关表头
    /// </summary>
    public class ColumnSwitch : Column
    {
        /// <summary>
        /// 开关表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        public ColumnSwitch(string key, string title) : base(key, title) { }

        /// <summary>
        /// 开关表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        /// <param name="align">对齐方式</param>
        public ColumnSwitch(string key, string title, ColumnAlign align) : base(key, title, align) { }

        /// <summary>
        /// 点击时自动改变选中状态
        /// </summary>
        public bool AutoCheck { get; set; } = true;
        public Column SetAutoCheck(bool value = false)
        {
            AutoCheck = value;
            return this;
        }

        public Func<bool, object?, int, int, bool>? Call { get; set; }

        /// <summary>
        /// 插槽
        /// </summary>
        public new Func<object?, object, int, object?>? Render { get; }
    }

    public class TemplateColumn : Column
    {
        /// <summary>
        /// 表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        public TemplateColumn(string key, string title) : base(key, title) { }

        internal virtual Table.Template CreateCell(Table table, TemplateColumn column, PropertyDescriptor? prop, object? ov, ref int processing, object value) =>
            new Table.Template(table, column, prop, ov, ref processing, new ICell[] { GetCellValue(value) });

        public virtual ICell GetCellValue(object? value) => new CellText(value?.ToString() ?? string.Empty);
    }

    /// <summary>
    /// 拖拽手柄列
    /// </summary>
    public class ColumnSort : Column
    {
        public ColumnSort() : base("tsort", "")
        {
        }
    }

    public class Column<T> : Column
    {
        /// <summary>
        /// 表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        public Column(string key, string title) : base(key, title) { }

        /// <summary>
        /// 表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        /// <param name="align">对齐方式</param>
        public Column(string key, string title, ColumnAlign align) : base(key, title, align) { }

        Func<object?, T, int, object?>? render;
        /// <summary>
        /// 插槽
        /// </summary>
        public new Func<object?, T, int, object?>? Render
        {
            set
            {
                if (render == value) return;
                render = value;
                if (render == null) base.Render = null;
                else
                {
                    base.Render = (val, record, index) =>
                    {
                        if (record is T recordT)
                        {
                            var value = render(val, recordT, index);
                            return value;
                        }
                        return null;
                    };
                }
            }
        }
    }

    /// <summary>
    /// 丰富标识列 (弃用SelectItem.SubText, Sub, Online)
    /// </summary>
    public class ColumnSelect : Column
    {
        /// <summary>
        /// 表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        public ColumnSelect(string key, string title) : base(key, title) { }

        /// <summary>
        /// 表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        /// <param name="align">对齐方式</param>
        public ColumnSelect(string key, string title, ColumnAlign align) : base(key, title, align) { }

        /// <summary>
        /// 列值显示类型
        /// </summary>
        public SelectCellType CellType { get; set; } = SelectCellType.Both;

        /// <summary>
        /// 显示项成员 (SelectItem.Tag为值)
        /// </summary>
        public List<SelectItem> Items { get; set; } = new List<SelectItem>();

        /// <summary>
        /// 获取选择项
        /// </summary>
        /// <param name="val">项值</param>
        /// <returns></returns>
        public SelectItem? this[object? val]
        {
            get
            {
                foreach (var item in Items)
                {
                    if (item.Tag == val || item.Tag.Equals(val)) return item;
                }
                return null;
            }
        }
    }

    /// <summary>
    /// 表头
    /// </summary>
    public class Column
    {
        /// <summary>
        /// 表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        public Column(string key, string title)
        {
            Key = key;
            _title = title;
        }
        /// <summary>
        /// 表头
        /// </summary>
        /// <param name="key">绑定名称</param>
        /// <param name="title">显示文字</param>
        /// <param name="align">对齐方式</param>
        public Column(string key, string title, ColumnAlign align)
        {
            Key = key;
            _title = title;
            Align = align;
        }

        /// <summary>
        /// 绑定名称
        /// </summary>
        public string Key { get; set; }

        string _title;
        /// <summary>
        /// 显示文字
        /// </summary>
        public string Title
        {
            get => Localization.GetLangIN(LocalizationTitle, _title, new string[] { "{id}", Key });
            set
            {
                if (_title == value) return;
                _title = value;
                Invalidates();
            }
        }

        [Description("显示文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationTitle { get; set; }

        /// <summary>
        /// 设置国际化显示文本
        /// </summary>
        public Column SetLocalizationTitle(string? value)
        {
            LocalizationTitle = value;
            return this;
        }

        /// <summary>
        /// 设置国际化显示文本（后面插入id）
        /// </summary>
        public Column SetLocalizationTitleID(string value)
        {
            LocalizationTitle = value + "{id}";
            return this;
        }

        bool visible = true;
        /// <summary>
        /// 是否显示
        /// </summary>
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
        /// 返回当前列的显示索引
        /// </summary>
        public int VisibleIndex
        {
            get
            {
                if (PARENT?.rows == null || PARENT.rows.Length == 0) return -1;
                foreach (var col in PARENT.rows[0].cells)
                {
                    if (col.COLUMN == this) return col.INDEX;
                }
                return INDEX;
            }
        }

        /// <summary>
        /// 设置是否显示
        /// </summary>
        public Column SetVisible(bool value = false)
        {
            Visible = value;
            return this;
        }

        /// <summary>
        /// 对齐方式
        /// </summary>
        public ColumnAlign Align { get; set; } = ColumnAlign.Left;

        /// <summary>
        /// 设置对齐方式
        /// </summary>
        public Column SetAlign(ColumnAlign value = ColumnAlign.Center)
        {
            Align = value;
            return this;
        }

        /// <summary>
        /// 表头对齐方式
        /// </summary>
        public ColumnAlign? ColAlign { get; set; }

        /// <summary>
        /// 设置表头对齐方式
        /// </summary>
        public Column SetColAlign(ColumnAlign value = ColumnAlign.Center)
        {
            ColAlign = value;
            return this;
        }

        /// <summary>
        /// 设置对齐方式
        /// </summary>
        /// <param name="value">内容对齐方式</param>
        /// <param name="col">表头对齐方式</param>
        public Column SetAligns(ColumnAlign value = ColumnAlign.Center, ColumnAlign col = ColumnAlign.Center)
        {
            Align = value;
            ColAlign = col;
            return this;
        }

        /// <summary>
        /// 列宽度
        /// </summary>
        public string? Width { get; set; }

        /// <summary>
        /// 设置列宽度
        /// </summary>
        public Column SetWidth(string? value = null)
        {
            Width = value;
            return this;
        }

        /// <summary>
        /// 宽度（像素）
        /// </summary>
        public int WidthPixel { get; internal set; }

        /// <summary>
        /// 列最大宽度
        /// </summary>
        public string? MaxWidth { get; set; }

        /// <summary>
        /// 只读
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// 设置列最大宽度
        /// </summary>
        public Column SetMaxWidth(string? value = null)
        {
            MaxWidth = value;
            return this;
        }

        /// <summary>
        /// 超过宽度将自动省略
        /// </summary>
        public bool Ellipsis { get; set; }

        /// <summary>
        /// 设置超过宽度将自动省略
        /// </summary>
        public Column SetEllipsis(bool value = true)
        {
            Ellipsis = value;
            return this;
        }

        bool lineBreak = false;
        /// <summary>
        /// 自动换行
        /// </summary>
        public bool LineBreak
        {
            get => lineBreak;
            set
            {
                if (lineBreak == value) return;
                lineBreak = value;
                Invalidates();
            }
        }

        /// <summary>
        /// 设置自动换行
        /// </summary>
        public Column SetLineBreak(bool value = true)
        {
            LineBreak = value;
            return this;
        }

        bool colBreak = false;
        /// <summary>
        /// 表头自动换行
        /// </summary>
        public bool ColBreak
        {
            get => colBreak;
            set
            {
                if (colBreak == value) return;
                colBreak = value;
                Invalidates();
            }
        }

        /// <summary>
        /// 设置表头自动换行
        /// </summary>
        public Column SetColumBreak(bool value = true)
        {
            ColBreak = value;
            return this;
        }

        bool _fixed = false;
        /// <summary>
        /// 列是否固定
        /// </summary>
        public bool Fixed
        {
            get => _fixed;
            set
            {
                if (_fixed == value) return;
                _fixed = value;
                Invalidates();
            }
        }

        /// <summary>
        /// 设置列是否固定
        /// </summary>
        public Column SetFixed(bool value = true)
        {
            Fixed = value;
            return this;
        }

        /// <summary>
        /// 列可编辑
        /// </summary>
        public bool Editable { get; set; } = true;

        /// <summary>
        /// 设置列是否可编辑
        /// 注意：该方法必须配合EditMode使用控制某列在编辑模式下是否可编辑
        /// 默认设置为false
        /// </summary>
        public Column SetEditable(bool value = false)
        {
            Editable = value;
            return this;
        }

        #region 排序

        bool sortorder = false;
        /// <summary>
        /// 启用排序
        /// </summary>
        public bool SortOrder
        {
            get => sortorder;
            set
            {
                if (sortorder == value) return;
                sortorder = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 设置启用排序
        /// </summary>
        public Column SetSortOrder(bool value = true)
        {
            SortOrder = value;
            return this;
        }

        SortMode sortMode = SortMode.NONE;
        /// <summary>
        /// 排序模式
        /// </summary>
        public SortMode SortMode
        {
            get => sortMode;
            set
            {
                if (sortMode == value) return;
                if (PARENT == null || PARENT.rows == null)
                {
                    sortMode = value;
                    Invalidate();
                    return;
                }
                foreach (var item in PARENT.rows[0].cells)
                {
                    if (item.COLUMN.SortOrder) item.COLUMN.sortMode = SortMode.NONE;
                }
                sortMode = value;
                Invalidate();
            }
        }

        #endregion

        #region 筛选

        /// <summary>
        /// 存在筛选
        /// </summary>
        public bool HasFilter { get; private set; }

        FilterOption? filter { get; set; }
        /// <summary>
        /// 用户筛选选项
        /// </summary>
        public FilterOption? Filter
        {
            get => filter;
            set
            {
                filter = value;
                if (filter == null) HasFilter = false;
                else HasFilter = true;
                Invalidate();
            }
        }

        /// <summary>
        /// 设置默认筛选选项 (string)
        /// </summary>
        /// <returns></returns>
        public Column SetDefaultFilter() => SetDefaultFilter(typeof(string));

        /// <summary>
        /// 设置默认筛选选项
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns></returns>
        public Column SetDefaultFilter(Type type)
        {
            Filter = new FilterOption(type);
            return this;
        }

        #endregion

        #region 格式化

        /// <summary>
        /// 格式化显示（如日期：D, yyyy-MM-dd, dd MMM yyyy..., 数字格式化：C, D5, P2, 0.###...）
        /// </summary>
        public string? DisplayFormat { get; set; }

        /// <summary>
        /// 设置格式化显示
        /// 建议非string类型需要时设置
        /// </summary>
        /// <param name="format">string.Format格式化。如日期：D, yyyy-MM-dd, dd MMM yyyy..., 数字格式化：C, D5, P2, 0.###...</param>
        public Column SetDisplayFormat(string format)
        {
            DisplayFormat = format;
            return this;
        }

        /// <summary>
        /// 返回格式化的字符串
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns></returns>
        public string? GetDisplayText(object? value)
        {
            if (value == null || value == DBNull.Value) return null;
            else
            {
                if (DisplayFormat == null || string.IsNullOrEmpty(DisplayFormat)) return value?.ToString();
                try
                {
                    if (DisplayFormat.Contains("{0:")) return string.Format(DisplayFormat, value);
                    return string.Format("{0:" + DisplayFormat + "}", value);
                }
                catch { return value?.ToString(); }
            }
        }

        #endregion

        /// <summary>
        /// 列可拖拽
        /// </summary>
        public bool DragSort { get; set; } = true;

        /// <summary>
        /// 设置列可拖拽
        /// </summary>
        public Column SetDragSort(bool value = false)
        {
            DragSort = value;
            return this;
        }

        /// <summary>
        /// 树形列
        /// </summary>
        public string? KeyTree { get; set; }

        /// <summary>
        /// 设置树形列
        /// </summary>
        public Column SetTree(string? key)
        {
            KeyTree = key;
            return this;
        }

        /// <summary>
        /// 列样式
        /// </summary>
        public Table.CellStyleInfo? Style { get; set; }

        /// <summary>
        /// 设置列样式
        /// </summary>
        public Column SetStyle(Color? back, Color? fore = null) => SetStyle(new Table.CellStyleInfo { BackColor = back, ForeColor = fore });

        /// <summary>
        /// 设置列样式
        /// </summary>
        public Column SetStyle(Table.CellStyleInfo? style)
        {
            Style = style;
            return this;
        }

        /// <summary>
        /// 标题列样式
        /// </summary>
        public Table.CellStyleInfo? ColStyle { get; set; }

        /// <summary>
        /// 设置标题列样式
        /// </summary>
        public Column SetColStyle(Color? back, Color? fore = null) => SetColStyle(new Table.CellStyleInfo { BackColor = back, ForeColor = fore });

        /// <summary>
        /// 设置标题列样式
        /// </summary>
        public Column SetColStyle(Table.CellStyleInfo? style)
        {
            ColStyle = style;
            return this;
        }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        public object? Tag { get; set; }

        #region 内部

        internal Table? PARENT { get; set; }
        public int INDEX { get; internal set; }
        public int INDEX_REAL { get; internal set; }
        void Invalidate()
        {
            if (PARENT == null) return;
            if (PARENT.LoadLayout()) PARENT.Invalidate();
        }
        void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.ExtractHeaderFixed();
            if (PARENT.LoadLayout()) PARENT.Invalidate();
        }

        internal bool SetSortMode(SortMode value)
        {
            if (sortMode == value) return false;
            sortMode = value;
            return true;
        }

        #endregion

        /// <summary>
        /// 插槽
        /// </summary>
        public Func<object?, object, int, object?>? Render { get; set; }
    }

    #endregion

    /// <summary>
    /// 单元格范围
    /// </summary>
    public class CellRange
    {
        /// <summary>
        /// 单元格范围
        /// </summary>
        /// <param name="firstRow">第一行</param>
        /// <param name="lastRow">最后行</param>
        /// <param name="firstCol">第一列</param>
        /// <param name="lastCol">最后列</param>
        /// <exception cref="ArgumentException">参数异常</exception>
        public CellRange(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            FirstColumn = firstCol;
            FirstRow = firstRow;
            LastColumn = lastCol;
            LastRow = lastRow;
            if (lastRow < firstRow || lastCol < firstCol) throw new ArgumentException("lastRow < firstRow || lastCol < firstCol");
        }

        /// <summary>
        /// 第一行
        /// </summary>
        public int FirstRow { get; set; }
        /// <summary>
        /// 第一列
        /// </summary>
        public int FirstColumn { get; set; }
        /// <summary>
        /// 最后行
        /// </summary>
        public int LastRow { get; set; }

        /// <summary>
        /// 最后列
        /// </summary>
        public int LastColumn { get; set; }

        public int MinRow => Math.Min(FirstRow, LastRow);

        public int MaxRow => Math.Max(FirstRow, LastRow);

        public int MinColumn => Math.Min(FirstColumn, LastColumn);

        public int MaxColumn => Math.Max(FirstColumn, LastColumn);

        public bool IsInRange(int rowInd, int colInd)
        {
            if (FirstRow <= rowInd && rowInd <= LastRow && FirstColumn <= colInd) return colInd <= LastColumn;
            return false;
        }

        public bool ContainsRow(int rowInd)
        {
            if (FirstRow <= rowInd) return rowInd <= LastRow;
            return false;
        }

        public bool ContainsColumn(int colInd)
        {
            if (FirstColumn <= colInd) return colInd <= LastColumn;
            return false;
        }

        public override int GetHashCode() => MinColumn + (MaxColumn << 8) + (MinRow << 16) + (MaxRow << 24);

        public CellRange Copy() => new CellRange(FirstRow, LastRow, FirstColumn, LastColumn);
    }

    /// <summary>
    /// 列的对齐方式
    /// </summary>
    public enum ColumnAlign
    {
        Left,
        Right,
        Center
    }

    public enum SortMode : int
    {
        NONE = 0,
        /// <summary>
        /// 升序
        /// </summary>
        ASC = 1,
        /// <summary>
        /// 降序
        /// </summary>
        DESC = 2
    }
    /// <summary>
    /// 单元格显示类型
    /// </summary>
    public enum SelectCellType
    {
        /// <summary>
        /// 图标和文本 (如果有)
        /// </summary>
        Both = 0,
        /// <summary>
        /// 仅文本
        /// </summary>
        Text = 1,
        /// <summary>
        /// 仅图标
        /// </summary>
        Icon = 2
    }
}
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
    public partial class Table : IControl
    {
        #region 属性

        ColumnCollection? columns = null;
        /// <summary>
        /// 表格列的配置
        /// </summary>
        [Browsable(false), Description("表格列的配置"), Category("数据"), DefaultValue(null)]
        public ColumnCollection? Columns
        {
            get => columns;
            set
            {
                if (columns == value) return;
                SortHeader = null;
                columns = value;
                LoadLayout();
                Invalidate();
            }
        }

        object? dataSource = null;
        /// <summary>
        /// 数据数组
        /// </summary>
        [Browsable(false), Description("数据数组"), Category("数据"), DefaultValue(null)]
        public object? DataSource
        {
            get => dataSource;
            set
            {
                dataSource = value;
                SortData = null;
                ScrollBar.Clear();
                ExtractHeaderFixed();
                ExtractData();
                LoadLayout();
                Invalidate();
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
                var row = dataTmp.rows[index];
                return row;
            }
        }

        int _gap = 12;
        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(12)]
        public int Gap
        {
            get => _gap;
            set
            {
                if (_gap == value) return;
                _gap = value;
                LoadLayout();
                Invalidate();
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
                LoadLayout();
                Invalidate();
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
                LoadLayout();
                Invalidate();
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
                LoadLayout();
                Invalidate();
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
            }
        }

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
                LoadLayout();
                Invalidate();
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
                LoadLayout();
                Invalidate();
            }
        }

        /// <summary>
        /// 树开关按钮大小
        /// </summary>
        [Description("树开关按钮大小"), Category("外观"), DefaultValue(16)]
        public int TreeButtonSize { get; set; } = 16;

        #endregion

        /// <summary>
        /// 行复制
        /// </summary>
        [Description("行复制"), Category("行为"), DefaultValue(true)]
        public bool ClipboardCopy { get; set; } = true;

        /// <summary>
        /// 列宽自动调整模式
        /// </summary>
        [Description("列宽自动调整模式"), Category("行为"), DefaultValue(ColumnsMode.Auto)]
        public ColumnsMode AutoSizeColumnsMode { get; set; } = ColumnsMode.Auto;

        int? rowHeight = null;
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
                LoadLayout();
                Invalidate();
            }
        }

        int? rowHeightHeader = null;
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
                LoadLayout();
                Invalidate();
            }
        }

        #region 为空

        [Description("是否显示空样式"), Category("外观"), DefaultValue(true)]
        public bool Empty { get; set; } = true;

        string? emptyText;
        [Description("数据为空显示文字"), Category("外观"), DefaultValue(null)]
        public string? EmptyText
        {
            get => emptyText;
            set
            {
                if (emptyText == value) return;
                emptyText = value;
                Invalidate();
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
                LoadLayout();
                Invalidate();
            }
        }

        /// <summary>
        /// 默认是否展开
        /// </summary>
        [Description("默认是否展开"), Category("外观"), DefaultValue(false)]
        public bool DefaultExpand { get; set; }

        #endregion

        #region 主题

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
                if (selectedIndex > 0) Invalidate();
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
                if (selectedIndex > 0) Invalidate();
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
            }
        }

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
            }
        }

        #endregion

        #endregion

        int selectedIndex = -1;
        /// <summary>
        /// 选中行
        /// </summary>
        [Description("选中行"), Category("外观"), DefaultValue(-1)]
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex == value) return;
                selectedIndex = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 省略文字提示
        /// </summary>
        [Description("省略文字提示"), Category("行为"), DefaultValue(true)]
        public bool ShowTip { get; set; } = true;

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
            }
        }

        #endregion

        #endregion

        #region 初始化

        public Table() { ScrollBar = new ScrollBar(this, true, true, radius, !visibleHeader); }

        protected override void Dispose(bool disposing)
        {
            ThreadState?.Dispose();
            ScrollBar.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region 方法

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

        void OnCheck()
        {
            ThreadCheck?.Dispose();
            if (PARENT != null && PARENT.IsHandleCreated)
            {
                if (Config.Animation)
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

        ITask? ThreadCheck = null;

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

        public Func<bool, object?, int, int, bool>? Call { get; set; }

        /// <summary>
        /// 插槽
        /// </summary>
        public new Func<object?, object, int, object?>? Render { get; }
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
            Title = title;
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
            Title = title;
            Align = align;
        }

        /// <summary>
        /// 绑定名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 显示文字
        /// </summary>
        public string Title { get; set; }

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
        /// 对齐方式
        /// </summary>
        public ColumnAlign Align { get; set; } = ColumnAlign.Left;

        /// <summary>
        /// 表头对齐方式
        /// </summary>
        public ColumnAlign? ColAlign { get; set; }

        /// <summary>
        /// 列宽度
        /// </summary>
        public string? Width { get; set; }

        /// <summary>
        /// 列最大宽度
        /// </summary>
        public string? MaxWidth { get; set; }

        /// <summary>
        /// 超过宽度将自动省略
        /// </summary>
        public bool Ellipsis { get; set; }

        /// <summary>
        /// 自动换行
        /// </summary>
        public bool LineBreak { get; set; }

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
        /// 树形列
        /// </summary>
        public string? KeyTree { get; set; }

        /// <summary>
        /// 列样式
        /// </summary>
        public Table.CellStyleInfo? Style { get; set; }

        /// <summary>
        /// 标题列样式
        /// </summary>
        public Table.CellStyleInfo? ColStyle { get; set; }

        #region 内部

        internal Table? PARENT { get; set; }
        internal int INDEX { get; set; }
        internal int SortMode { get; set; }
        void Invalidate()
        {
            if (PARENT == null) return;
            PARENT.LoadLayout();
            PARENT.Invalidate();
        }
        void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.ExtractHeaderFixed();
            PARENT.LoadLayout();
            PARENT.Invalidate();
        }

        #endregion

        /// <summary>
        /// 插槽
        /// </summary>
        public Func<object?, object, int, object?>? Render { get; set; }
    }

    #endregion

    /// <summary>
    /// 列的对齐方式
    /// </summary>
    public enum ColumnAlign
    {
        Left,
        Right,
        Center
    }
}
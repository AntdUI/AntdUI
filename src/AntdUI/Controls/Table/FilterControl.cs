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
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace AntdUI
{
    [Browsable(false)]
    public partial class FilterControl : UserControl
    {
        public const string SVG_EQUAL = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M160 256c-35.4 0-64 28.6-64 64s28.6 64 64 64h704c35.4 0 64-28.6 64-64s-28.6-64-64-64H160z m0 384c-35.4 0-64 28.6-64 64s28.6 64 64 64h704c35.4 0 64-28.6 64-64s-28.6-64-64-64H160z\"></path></svg>";
        public const string SVG_NOT_EQUAL = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M803.6 74.8c29.4 19.6 37.4 59.4 17.8 88.8L738.2 288H864c35.4 0 64 28.6 64 64s-28.6 64-64 64H653l-128 192H864c35.4 0 64 28.6 64 64s-28.6 64-64 64H439.6l-130.4 195.4c-19.6 29.4-59.4 37.4-88.8 17.8s-37.4-59.4-17.8-88.8l83.2-124.4H160c-35.4 0-64-28.6-64-64s28.6-64 64-64h211l128-192H160c-35.4 0-64-28.6-64-64s28.6-64 64-64h424.4l130.4-195.4c19.6-29.4 59.4-37.4 88.8-17.8z\"></path></svg>";
        public const string SVG_GREATER = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M134.8 163.4c-15.8 31.6-3 70 28.6 85.8L689 512 163.4 774.8c-31.6 15.8-44.4 54.2-28.6 85.8s54.2 44.4 85.8 28.6l640-320c21.6-10.8 35.4-33 35.4-57.2s-13.6-46.4-35.4-57.2l-640-320c-31.6-15.8-70-3-85.8 28.6z\"></path></svg>";
        public const string SVG_LESS = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M889.2 163.4c15.8 31.6 3 70-28.6 85.8L335.2 512l525.4 262.8c31.6 15.8 44.4 54.2 28.6 85.8s-54.2 44.4-85.8 28.6l-640-320C141.6 558.4 128 536.2 128 512s13.6-46.4 35.4-57.2l640-320c31.6-15.8 70-3 85.8 28.6z\"></path></svg>";
        public const string SVG_CONTAIN = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M656.512 213.333333a42.666667 42.666667 0 0 1 42.410667 47.232l-54.826667 512A42.666667 42.666667 0 0 1 601.6 810.666667H399.530667a42.666667 42.666667 0 0 1-42.453334-47.232l54.869334-512A42.666667 42.666667 0 0 1 454.357333 213.333333h202.154667z m189.141333 194.816l71.68 1.194667-6.656 54.186667-90.496-0.512c-21.248 0-37.418667 5.632-48.512 16.981333-11.093333 11.221333-18.005333 27.946667-20.650666 50.176l-5.205334 43.989333-0.725333 8.789334-0.256 6.058666c0 34.432 17.536 51.626667 52.650667 51.626667h25.514666l13.312-0.469333 16.426667-0.853334 30.421333-2.218666 11.690667-0.938667-5.674667 54.656c-29.994667 3.114667-55.765333 4.693333-77.354666 4.693333h-20.992c-70.528 0-105.813333-32.128-105.813334-96.341333 0-8.448 0.469333-17.28 1.493334-26.496l4.992-41.002667c5.205333-41.002667 18.901333-71.850667 41.002666-92.501333 22.186667-20.650667 53.589333-31.018667 94.165334-31.018667h18.986666zM493.482667 298.666667h-58.837334L386.56 693.504h128l9.173333-0.170667c72.106667-2.944 114.048-40.96 125.696-114.005333l1.28-9.344 4.693334-38.826667 0.810666-8.789333a220.586667 220.586667 0 0 0 0.682667-16.853333c0-32.128-8.917333-56.32-26.666667-72.661334-15.914667-14.805333-39.082667-22.954667-69.461333-24.448l-10.410667-0.256h-70.144L493.482667 298.666667zM254.72 407.466667c35.2 0 61.824 8.405333 79.786667 25.173333 18.133333 16.810667 27.221333 39.125333 27.221333 66.986667l-0.085333 2.773333-0.938667 13.226667-20.992 177.194666H191.146667c-25.6 0-46.08-7.253333-61.525334-21.845333-15.36-14.506667-22.997333-33.365333-22.997333-56.490667 0-5.205333 0.213333-9.173333 0.682667-11.818666 2.986667-21.248 8.362667-38.058667 16.170666-50.517334 7.850667-12.458667 19.242667-21.461333 34.133334-27.008 14.933333-5.674667 34.474667-8.490667 58.666666-8.490666h86.016l0.512-4.181334a38.954667 38.954667 0 0 0 0.469334-7.168c0-13.312-4.437333-24.021333-13.312-32.170666-8.874667-8.192-22.741333-12.330667-41.514667-12.330667h-13.141333c-30.464 0-57.813333 0.853333-82.005334 2.517333l7.68-52.181333 21.632-1.365333 32.085334-1.621334 18.090666-0.597333 21.845334-0.085333z m-43.52 159.872c-12.970667-0.341333-23.338667 2.133333-30.976 7.466666-6.570667 4.48-10.965333 11.946667-13.098667 22.4l-0.896 5.461334-0.512 6.144c0 9.898667 2.773333 17.493333 8.32 22.826666a31.146667 31.146667 0 0 0 18.304 7.552l6.016 0.298667h89.173334l8.661333-71.68-84.992-0.469333z m332.501333-104.32c35.584 0 53.333333 17.408 53.333334 52.309333l-0.256 5.589333-0.725334 8.746667-5.162666 41.813333c-5.12 42.154667-26.026667 64.469333-62.634667 66.986667l-7.04 0.213333h-69.162667l21.504-175.658666h70.144z\"></path></svg>";
        public const string SVG_NOT_CONTAIN = "<svg viewBox=\"0 0 1024 1024\" data-spm-anchor-id=\"a313x.search_index.0.i20.6d6e3a81oSdXOG\"><path d=\"M656.512 213.333333a42.666667 42.666667 0 0 1 42.410667 47.232l-54.826667 512A42.666667 42.666667 0 0 1 601.6 810.666667H399.530667a42.666667 42.666667 0 0 1-42.453334-47.232l54.869334-512A42.666667 42.666667 0 0 1 454.357333 213.333333h202.154667z m189.141333 194.816l71.68 1.194667-6.656 54.186667-90.496-0.512c-21.248 0-37.418667 5.632-48.512 16.981333-11.093333 11.221333-18.005333 27.946667-20.650666 50.176l-5.205334 43.989333-0.725333 8.789334-0.256 6.058666c0 34.432 17.536 51.626667 52.650667 51.626667h25.514666l13.312-0.469333 16.426667-0.853334 30.421333-2.218666 11.690667-0.938667-5.674667 54.656c-29.994667 3.114667-55.765333 4.693333-77.354666 4.693333h-20.992c-70.528 0-105.813333-32.128-105.813334-96.341333 0-8.448 0.469333-17.28 1.493334-26.496l4.992-41.002667c5.205333-41.002667 18.901333-71.850667 41.002666-92.501333 22.186667-20.650667 53.589333-31.018667 94.165334-31.018667h18.986666zM493.482667 298.666667h-58.837334L386.56 693.504h128l9.173333-0.170667c72.106667-2.944 114.048-40.96 125.696-114.005333l1.28-9.344 4.693334-38.826667 0.810666-8.789333a220.586667 220.586667 0 0 0 0.682667-16.853333c0-32.128-8.917333-56.32-26.666667-72.661334-15.914667-14.805333-39.082667-22.954667-69.461333-24.448l-10.410667-0.256h-70.144L493.482667 298.666667zM254.72 407.466667c35.2 0 61.824 8.405333 79.786667 25.173333 18.133333 16.810667 27.221333 39.125333 27.221333 66.986667l-0.085333 2.773333-0.938667 13.226667-20.992 177.194666H191.146667c-25.6 0-46.08-7.253333-61.525334-21.845333-15.36-14.506667-22.997333-33.365333-22.997333-56.490667 0-5.205333 0.213333-9.173333 0.682667-11.818666 2.986667-21.248 8.362667-38.058667 16.170666-50.517334 7.850667-12.458667 19.242667-21.461333 34.133334-27.008 14.933333-5.674667 34.474667-8.490667 58.666666-8.490666h86.016l0.512-4.181334a38.954667 38.954667 0 0 0 0.469334-7.168c0-13.312-4.437333-24.021333-13.312-32.170666-8.874667-8.192-22.741333-12.330667-41.514667-12.330667h-13.141333c-30.464 0-57.813333 0.853333-82.005334 2.517333l7.68-52.181333 21.632-1.365333 32.085334-1.621334 18.090666-0.597333 21.845334-0.085333z m-43.52 159.872c-12.970667-0.341333-23.338667 2.133333-30.976 7.466666-6.570667 4.48-10.965333 11.946667-13.098667 22.4l-0.896 5.461334-0.512 6.144c0 9.898667 2.773333 17.493333 8.32 22.826666a31.146667 31.146667 0 0 0 18.304 7.552l6.016 0.298667h89.173334l8.661333-71.68-84.992-0.469333z m332.501333-104.32c35.584 0 53.333333 17.408 53.333334 52.309333l-0.256 5.589333-0.725334 8.746667-5.162666 41.813333c-5.12 42.154667-26.026667 64.469333-62.634667 66.986667l-7.04 0.213333h-69.162667l21.504-175.658666h70.144z\" data-spm-anchor-id=\"a313x.search_index.0.i19.6d6e3a81oSdXOG\" class=\"selected\" fill=\"#d81e06\"></path></svg>";
        public const string SVG_FILTER_CLEAR = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M789.312 128c23.616 0 42.688 19.072 42.688 42.688v192c0 7.488-3.84 14.08-9.728 17.92a39.808 39.808 0 0 1-12.288 11.328L716.48 448 552.32 446.912l194.24-116.544V213.312H149.312v117.312l214.016 128.576A42.688 42.688 0 0 1 384 498.56a38.656 38.656 0 0 1 0.256 4.736v324.48H298.88V519.936L86.016 392.128a42.56 42.56 0 0 1-12.672-11.584 21.888 21.888 0 0 1-9.344-17.92v-192C64 147.136 83.072 128 106.688 128h682.624z\"></path><path d=\"M902.960461 556.92179l-147.07821 147.07821 147.07821 147.07821-67.882251 67.882251L688 771.882251 540.92179 918.960461 473.039539 851.07821l147.07821-147.07821L473.039539 556.92179l67.882251-67.882251L688 636.117749 835.07821 489.039539l67.882251 67.882251z\" fill=\"#FF5151\"></path></svg>";
        private const string CHECKED_ALL = "(全选)";
        private const string BLANK_FIELD = "(空白)";

        #region Ctor

        public FilterControl(Table table, Column currentColumn, IList<object>? customSource)
        {
            InitializeComponent();

            _table = table;
            _column = currentColumn;
            if (Option.Table == null) Option.Table = table;
            if (Option.Column == null) Option.Column = currentColumn;
            inputSearch.PlaceholderText = $"搜索 {currentColumn.Title}";
            CustomSource = customSource;
            RowsCache = table?.dataTmp?.RowsCache;
            segmentedSource.SelectIndex = (int)Option.ActiveSource + 1;
            segmentedSource.Items[0].Enabled = Option.Enabled;
            segmentedSource.Items[0].IconSvg = SVG_FILTER_CLEAR;

            InitFilterEditor();
        }

        protected void InitFilterEditor()
        {
            InitConditions();
            InitFilterEdit();
            InitFilterValues(Option.Enabled && TableView.FilterColumns?[0] == FocusedColumn ? FilterSource.DataSource : Option.ActiveSource, CustomSource);
            segmentedSource.SelectIndexChanged += segmentedSource_SelectIndexChanged;
        }
        protected void InitConditions()
        {
            Array conditions = Enum.GetValues(typeof(FilterConditions));
            List<SelectItem> items = new List<SelectItem>(conditions.Length);
            foreach (FilterConditions condition in conditions)
            {
                items.Add(new SelectItem(condition) { IconSvg = GetConditionIconSvg(condition), Text = null, SubText = GetConditionText(condition) });
            }
            items.RemoveAt(items.Count - 1); // 移除 None 条件
            selectCondition.Items.AddRange(items.ToArray());
            selectCondition.SelectedIndex = (int)Option.Condition;
            selectCondition.SelectedIndexChanged += SelectCondition_SelectedIndexChanged;
        }

        private void InitFilterEdit()
        {
            Type? type = FocusedColumn.Filter?.DataType;
            if (type == null) type = typeof(string);
            if (type == typeof(decimal) || type == typeof(double) || type == typeof(float) || type == typeof(int) || type == typeof(short) || type == typeof(long))
            {
                InputNumber edit = new InputNumber();
                edit.TabIndex = 0;
                edit.Dock = DockStyle.Fill;
                edit.DecimalPlaces = type == typeof(int) || type == typeof(short) || type == typeof(long) ? 0 : 7;
                try
                {
                    edit.Value = Option.FilterValues != null && Option.FilterValues.Count == 1 ? Convert.ToDecimal(Option.FilterValues[0]) : 0;
                }
                catch { }
                edit.ValueChanged += Edit_ValueChanged;
                flowPanelConditionEdit.Controls.Add(edit);
            }
            else if (type == typeof(DateTime))
            {
                DatePicker edit = new DatePicker();
                edit.TabIndex = 0;
                edit.Dock = DockStyle.Fill;
                try
                {
                    edit.Value = Option.FilterValues != null && Option.FilterValues.Count == 1 ? Convert.ToDateTime(Option.FilterValues[0]) : null;
                }
                catch { }
                edit.ValueChanged += EditDate_ValueChanged;
                flowPanelConditionEdit.Controls.Add(edit);
            }
            else if (type == typeof(bool))
            {
                Switch edit = new Switch();
                edit.TabIndex = 0;
                edit.Dock = DockStyle.Fill;
                edit.CheckedText = "是";
                edit.UnCheckedText = "否";
                try
                {
                    edit.Checked = Option.FilterValues != null && Option.FilterValues.Count == 1 ? Convert.ToBoolean(Option.FilterValues[0]) : false;
                }
                catch { }
                edit.CheckedChanged += EditChecked_CheckedChanged;
                flowPanelConditionEdit.Controls.Add(edit);
            }
            else
            {
                Input edit = new Input();
                edit.TabIndex = 0;
                edit.Dock = DockStyle.Fill;
#pragma warning disable CS8601 // 引用类型赋值可能为 null。
                edit.Text = Option.FilterValues != null && Option.FilterValues.Count == 1 && Option.FilterValues[0] != DBNull.Value ? Option.FilterValues[0].ToString() : string.Empty;
#pragma warning restore CS8601 // 引用类型赋值可能为 null。
                edit.TextChanged += Edit_TextChanged;
                flowPanelConditionEdit.Controls.Add(edit);
            }
        }

        private void InitFilterValues(FilterSource sourceType, IList<object>? customSource)
        {
            treeList.Items.Clear();
            bool enabled = Option.Enabled;
            HashSet<object> values = new HashSet<object>();
            bool blankFlag = false;
            if (sourceType != FilterSource.DataSource && customSource != null && customSource.Count > 0)
            {
                segmentedSource.Items[2].Text = "用户数据";
                segmentedSource.SelectIndex = 2;
                foreach (object val in customSource)
                {
                    if (blankFlag == false) blankFlag = val == null || val == DBNull.Value;
                    values.Add(val);
                }
            }
            else
            {
                Table.IRow[]? source;
                if (sourceType == FilterSource.Current)
                    source = TableView?.dataTmp?.rowsFilter;
                else if (sourceType == FilterSource.CurrentFirst)
                {
                    source = TableView?.dataTmp?.rowsFilter;
                    if (source == null || source.Length == 0) source = RowsCache;
                }
                else
                    source = RowsCache;
                if (source == null) return;

                foreach (var row in source)
                {
                    object? value = row[FocusedColumn.Key];
                    if (blankFlag == false) blankFlag = value == null || value == DBNull.Value;

                    values.Add(value);

                }
            }
            EditLocked = true;
            try
            {
                List<TreeItem> items = new List<TreeItem>();
                items.Add(CreateItem(CHECKED_ALL, enabled));
                foreach (var val in values)
                {
                    if (blankFlag && Option.AllowNull == false && (val == null || val == DBNull.Value)) continue;
                    items.Add(CreateItem(val, enabled));
                }
                if (blankFlag && Option.AllowNull) items.Add(CreateItem(BLANK_FIELD, enabled));
                treeList.Items.AddRange(items.ToArray());
                UpdateCheckedStateAll();
            }
            finally { EditLocked = false; }
        }
        private TreeItem CreateItem(object val, bool enabled)
        {
            TreeItem item = new TreeItem(val == null || val == DBNull.Value ? "" : val.ToString());
            item.Checked = enabled == false || (Option.FilterValues != null && Option.FilterValues.Contains(val));
            item.Tag = val;
            return item;
        }
        private string GetConditionIconSvg(FilterConditions condition)
        {
            switch (condition)
            {
                case FilterConditions.NotEqual:
                    return SVG_NOT_EQUAL;
                case FilterConditions.Greater:
                    return SVG_GREATER;
                case FilterConditions.Less:
                    return SVG_LESS;
                case FilterConditions.Contain:
                    return SVG_CONTAIN;
                case FilterConditions.NotContain:
                    return SVG_NOT_CONTAIN;
                default:
                    return SVG_EQUAL;
            }
        }
        private string GetConditionText(FilterConditions condition)
        {
            switch (condition)//未本地化
            {
                case FilterConditions.NotEqual:
                    return "不等于";
                case FilterConditions.Greater:
                    return "大于 (以...开头)";
                case FilterConditions.Less:
                    return "小于 (以...结尾)";
                case FilterConditions.Contain:
                    return "存在";
                case FilterConditions.NotContain:
                    return "不存在";
                default:
                    return "等于";
            }
        }
        #endregion
        #region Properties
        public FilterOption Option { get { return FocusedColumn.Filter; } }
        protected IControl? Edit { get { if (flowPanelConditionEdit.Controls.Count == 0) return null; return flowPanelConditionEdit.Controls[0] as IControl; } }
        protected AntdUI.Table.IRow[]? RowsCache { get; set; }
        protected Table _table;
        protected IList<object>? CustomSource { get; set; }
        protected bool EditLocked { get; set; } = false;
        /// <summary>
        /// Table视图控件
        /// </summary>
        public Table TableView { get => _table; }
        protected Column _column;
        protected SegmentedItem ItemFilterEnabled { get { return segmentedSource.Items[0]; } }
        public Column FocusedColumn { get => _column; }

        #endregion
        private void treeList_CheckedChanged(object sender, TreeCheckedEventArgs e)
        {
            if (EditLocked) return;
            EditLocked = true;
            try
            {
                if (e.Item.Text == CHECKED_ALL)
                {
                    foreach (var item in treeList.Items)
                    {
                        UpdateCheckedState(item, e.Value);
                    }
                    ItemFilterEnabled.Enabled = false;
                }
                else
                {
                    e.Item.Select = e.Value;
                    UpdateCheckedStateAll();
                }
                UpdateFilterValues();
            }
            finally { EditLocked = false; }
        }
        private void UpdateCheckedStateAll()
        {
            var checkedList = treeList.Items.Where(c => c.Checked).ToList();
            treeList.Items[0].CheckState = checkedList.Count == 0 ? CheckState.Unchecked : checkedList.Count == treeList.Items.Count - 1 ? CheckState.Checked : CheckState.Indeterminate;

        }
        private void UpdateCheckedState(TreeItem parentItem, bool checkedState)
        {
            parentItem.Checked = checkedState;
            parentItem.Select = checkedState;
            foreach (var item in parentItem.Sub)
            {
                UpdateCheckedState(item, checkedState);
            }
        }
        private void UpdateFilterValues()
        {
            Option.FilterValues = new List<object>();
            foreach (var item in treeList.Items)
            {
                if (item.Text == CHECKED_ALL) continue;
                if (item.Checked && item.Text == BLANK_FIELD)
                {
                    Option.FilterValues.Add(DBNull.Value);
                }
                else
                {
                    if (item.Checked)
                    {
                        Option.FilterValues.Add(item.Tag);
                    }
                }
            }
            Option.Condition = Option.FilterValues.Count > 0 ? FilterConditions.Equal : (FilterConditions)selectCondition.SelectedIndex;
            Option.UpdateFilter();
        }
        private void inputSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputSearch.Text))
            {
                foreach (var item in treeList.Items)
                {
                    item.Visible = true;
                }
                return;
            }

            var list = treeList.Items.Where(c => c.Text?.IndexOf(inputSearch.Text, StringComparison.OrdinalIgnoreCase) == -1).ToList();
            foreach (var item in treeList.Items)
            {
                item.Visible = !list.Contains(item);

            }
        }
        private FilterConditions beforeCondition = FilterConditions.None;
        private void SelectCondition_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            FilterConditions condition = (FilterConditions)e.Value;
            if (beforeCondition == condition) return;
            beforeCondition = condition;

            Option.Condition = condition;
            Option.UpdateFilter();
            ItemFilterEnabled.Enabled = Option.Enabled;

        }

        string beforeText = string.Empty;
        void Edit_TextChanged(object? sender, EventArgs e)
        {
            if (sender is Input edit)
            {
                if (beforeText == edit.Text) return;
                beforeText = edit.Text;

                Option.FilterValues = new List<object> { edit.Text };
                Option.UpdateFilter();
                ItemFilterEnabled.Enabled = Option.Enabled;
            }

        }

        bool beforeState = false;
        void EditChecked_CheckedChanged(object sender, BoolEventArgs e)
        {
            if (beforeState == e.Value) return;
            beforeState = e.Value;
            Option.FilterValues = new List<object> { e.Value };
            Option.UpdateFilter();
            ItemFilterEnabled.Enabled = Option.Enabled;
        }

        decimal beforeVal = 0;
        void Edit_ValueChanged(object sender, DecimalEventArgs e)
        {
            if (beforeVal == e.Value) return;
            beforeVal = e.Value;
            Option.FilterValues = new List<object> { e.Value };
            Option.UpdateFilter();
            ItemFilterEnabled.Enabled = Option.Enabled;

        }

        DateTime? beforeDate = DateTime.MinValue;
        void EditDate_ValueChanged(object sender, DateTimeNEventArgs e)
        {
            if (beforeDate == e.Value) return;

            beforeDate = e.Value;
            if (e.Value != null) Option.FilterValues = new List<object> { e.Value };
            Option.UpdateFilter();
            ItemFilterEnabled.Enabled = Option.Enabled;

        }

        int beforeIndex = -1;
        void segmentedSource_SelectIndexChanged(object sender, IntEventArgs e)
        {
            if (beforeIndex == e.Value) return;
            beforeIndex = e.Value;
            if (e.Value == 0)
            {
                Option.ClearFilter();
                ItemFilterEnabled.Enabled = false;
                Form? frm = this.FindForm();
                if (frm != null) frm.Close();
            }
            else
            {
                Option.ActiveSource = (FilterSource)(e.Value - 1);
                InitFilterValues(Option.ActiveSource, CustomSource);
                Option.UpdateFilter();
            }
        }
    }
}
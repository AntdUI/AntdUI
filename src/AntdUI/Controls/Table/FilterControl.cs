// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace AntdUI
{
    [ToolboxItem(false)]
    public partial class FilterControl : UserControl
    {
        public const string SVG_EQUAL = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M160 256c-35.4 0-64 28.6-64 64s28.6 64 64 64h704c35.4 0 64-28.6 64-64s-28.6-64-64-64H160z m0 384c-35.4 0-64 28.6-64 64s28.6 64 64 64h704c35.4 0 64-28.6 64-64s-28.6-64-64-64H160z\"></path></svg>";
        public const string SVG_NOT_EQUAL = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M803.6 74.8c29.4 19.6 37.4 59.4 17.8 88.8L738.2 288H864c35.4 0 64 28.6 64 64s-28.6 64-64 64H653l-128 192H864c35.4 0 64 28.6 64 64s-28.6 64-64 64H439.6l-130.4 195.4c-19.6 29.4-59.4 37.4-88.8 17.8s-37.4-59.4-17.8-88.8l83.2-124.4H160c-35.4 0-64-28.6-64-64s28.6-64 64-64h211l128-192H160c-35.4 0-64-28.6-64-64s28.6-64 64-64h424.4l130.4-195.4c19.6-29.4 59.4-37.4 88.8-17.8z\"></path></svg>";
        public const string SVG_GREATER = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M134.8 163.4c-15.8 31.6-3 70 28.6 85.8L689 512 163.4 774.8c-31.6 15.8-44.4 54.2-28.6 85.8s54.2 44.4 85.8 28.6l640-320c21.6-10.8 35.4-33 35.4-57.2s-13.6-46.4-35.4-57.2l-640-320c-31.6-15.8-70-3-85.8 28.6z\"></path></svg>";
        public const string SVG_LESS = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M889.2 163.4c15.8 31.6 3 70-28.6 85.8L335.2 512l525.4 262.8c31.6 15.8 44.4 54.2 28.6 85.8s-54.2 44.4-85.8 28.6l-640-320C141.6 558.4 128 536.2 128 512s13.6-46.4 35.4-57.2l640-320c31.6-15.8 70-3 85.8 28.6z\"></path></svg>";
        public const string SVG_CONTAIN = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M656.512 213.333333a42.666667 42.666667 0 0 1 42.410667 47.232l-54.826667 512A42.666667 42.666667 0 0 1 601.6 810.666667H399.530667a42.666667 42.666667 0 0 1-42.453334-47.232l54.869334-512A42.666667 42.666667 0 0 1 454.357333 213.333333h202.154667z m189.141333 194.816l71.68 1.194667-6.656 54.186667-90.496-0.512c-21.248 0-37.418667 5.632-48.512 16.981333-11.093333 11.221333-18.005333 27.946667-20.650666 50.176l-5.205334 43.989333-0.725333 8.789334-0.256 6.058666c0 34.432 17.536 51.626667 52.650667 51.626667h25.514666l13.312-0.469333 16.426667-0.853334 30.421333-2.218666 11.690667-0.938667-5.674667 54.656c-29.994667 3.114667-55.765333 4.693333-77.354666 4.693333h-20.992c-70.528 0-105.813333-32.128-105.813334-96.341333 0-8.448 0.469333-17.28 1.493334-26.496l4.992-41.002667c5.205333-41.002667 18.901333-71.850667 41.002666-92.501333 22.186667-20.650667 53.589333-31.018667 94.165334-31.018667h18.986666zM493.482667 298.666667h-58.837334L386.56 693.504h128l9.173333-0.170667c72.106667-2.944 114.048-40.96 125.696-114.005333l1.28-9.344 4.693334-38.826667 0.810666-8.789333a220.586667 220.586667 0 0 0 0.682667-16.853333c0-32.128-8.917333-56.32-26.666667-72.661334-15.914667-14.805333-39.082667-22.954667-69.461333-24.448l-10.410667-0.256h-70.144L493.482667 298.666667zM254.72 407.466667c35.2 0 61.824 8.405333 79.786667 25.173333 18.133333 16.810667 27.221333 39.125333 27.221333 66.986667l-0.085333 2.773333-0.938667 13.226667-20.992 177.194666H191.146667c-25.6 0-46.08-7.253333-61.525334-21.845333-15.36-14.506667-22.997333-33.365333-22.997333-56.490667 0-5.205333 0.213333-9.173333 0.682667-11.818666 2.986667-21.248 8.362667-38.058667 16.170666-50.517334 7.850667-12.458667 19.242667-21.461333 34.133334-27.008 14.933333-5.674667 34.474667-8.490667 58.666666-8.490666h86.016l0.512-4.181334a38.954667 38.954667 0 0 0 0.469334-7.168c0-13.312-4.437333-24.021333-13.312-32.170666-8.874667-8.192-22.741333-12.330667-41.514667-12.330667h-13.141333c-30.464 0-57.813333 0.853333-82.005334 2.517333l7.68-52.181333 21.632-1.365333 32.085334-1.621334 18.090666-0.597333 21.845334-0.085333z m-43.52 159.872c-12.970667-0.341333-23.338667 2.133333-30.976 7.466666-6.570667 4.48-10.965333 11.946667-13.098667 22.4l-0.896 5.461334-0.512 6.144c0 9.898667 2.773333 17.493333 8.32 22.826666a31.146667 31.146667 0 0 0 18.304 7.552l6.016 0.298667h89.173334l8.661333-71.68-84.992-0.469333z m332.501333-104.32c35.584 0 53.333333 17.408 53.333334 52.309333l-0.256 5.589333-0.725334 8.746667-5.162666 41.813333c-5.12 42.154667-26.026667 64.469333-62.634667 66.986667l-7.04 0.213333h-69.162667l21.504-175.658666h70.144z\"></path></svg>";

        #region Ctor

        IList<object>? CustomSource;
        bool realTime = false;
        int offsetX, offsetY;
        public FilterControl(Table table, System.Drawing.Font font, Column currentColumn, IList<object>? customSource, int offsetx, int offsety)
        {
            InitializeComponent();
            Font = font;
            _table = table;
            _column = currentColumn;
            CustomSource = customSource;
            offsetX = offsetx;
            offsetY = offsety;
            realTime = table.FilterRealTime;
            dv.VirtualMode = table.VirtualMode;
            dv.Columns = new ColumnCollection { new ColumnCheck("check"), new Column("text", "(全选)").SetLocalizationTitle("Filter.SelectAll") };
            Option.Table = table;
            Option.Column = currentColumn;
            inputSearch.PlaceholderText = Localization.Get("Filter.Search", "搜索") + " " + currentColumn.Title;
            InitConditions();
            InitFilterEdit();
            InitFilterValues();

            btn_clean.Enabled = Option.Enabled;
        }

        protected void InitConditions()
        {
            var items = new SelectItem[] {
                new SelectItem(FilterConditions.Equal).SetText("等于","Filter.Equal").SetIcon(SVG_EQUAL),
                new SelectItem(FilterConditions.NotEqual).SetText("不等于", "Filter.NotEqual").SetIcon(SVG_NOT_EQUAL),
                new SelectItem(FilterConditions.Greater).SetText("大于 (以...开头)", "Filter.Greater").SetIcon(SVG_GREATER),
                new SelectItem(FilterConditions.Less).SetText("小于 (以...结尾)", "Filter.Less").SetIcon(SVG_LESS),
                new SelectItem(FilterConditions.Contain).SetText("存在", "Filter.Contain").SetIcon(SVG_CONTAIN),
                new SelectItem(FilterConditions.NotContain).SetText( "不存在", "Filter.NotContain").SetIcon(SVG_CONTAIN.Insert(SVG_CONTAIN.Length - 14, " fill=\"#d81e06\"")),
            };
            selectCondition.Items.AddRange(items);
            selectCondition.SelectedValue = Option.Condition;
            selectCondition.SelectedValueChanged += SelectCondition_Changed;
        }

        private void InitFilterEdit()
        {
            if (_column is ColumnSelect columnSelect)
            {
                var edit = new Select
                {
                    Margin = new Padding(0),
                    PlaceholderText = Localization.Get("Filter", "筛选"),
                    Dock = DockStyle.Fill,
                    List = true,
                    AutoPrefixSvg = true
                };
                edit.Items.AddRange(columnSelect.Items.ToArray());
                tablePanel.Controls.Add(edit, 1, 0);
                foreach (SelectItem item in selectCondition.Items)
                {
                    FilterConditions condition = (FilterConditions)item.Tag;
                    if (condition == FilterConditions.Contain || condition == FilterConditions.NotContain) item.Enable = false;
                }
                edit.SelectedValueChanged += EditSelect_ValueChanged;
                return;
            }

            var type = _column.Filter?.DataType;
            if (type == null) type = typeof(string);
            if (type == typeof(decimal) || type == typeof(double) || type == typeof(float) || type == typeof(int) || type == typeof(short) || type == typeof(long))
            {
                var edit = new InputNumber
                {
                    Margin = new Padding(0),
                    PlaceholderText = Localization.Get("Filter", "筛选"),
                    Dock = DockStyle.Fill,
                    DecimalPlaces = type == typeof(int) || type == typeof(short) || type == typeof(long) ? 0 : 7
                };
                tablePanel.Controls.Add(edit, 1, 0);
                foreach (SelectItem item in selectCondition.Items)
                {
                    var condition = (FilterConditions)item.Tag;
                    if (condition == FilterConditions.Contain || condition == FilterConditions.NotContain) item.Enable = false;
                }
                edit.ValueChanged += Edit_ValueChanged;
                edit.TextChanged += Edit_TextChanged;
            }
            else if (type == typeof(DateTime))
            {
                var edit = new DatePicker
                {
                    Margin = new Padding(0),
                    PlaceholderText = Localization.Get("Filter", "筛选"),
                    Dock = DockStyle.Fill
                };
                tablePanel.Controls.Add(edit, 1, 0);
                foreach (SelectItem item in selectCondition.Items)
                {
                    var condition = (FilterConditions)item.Tag;
                    if (condition == FilterConditions.Contain || condition == FilterConditions.NotContain) item.Enable = false;
                }
                edit.ValueChanged += EditDate_ValueChanged;
                edit.TextChanged += Edit_TextChanged;
            }
            else if (type == typeof(bool))
            {
                var edit = new Switch
                {
                    Margin = new Padding(0),
                    Dock = DockStyle.Fill,
                    CheckedText = "是",
                    UnCheckedText = "否"
                };
                tablePanel.Controls.Add(edit, 1, 0);
                foreach (SelectItem item in selectCondition.Items)
                {
                    var condition = (FilterConditions)item.Tag;
                    item.Enable = condition == FilterConditions.Equal || condition == FilterConditions.NotEqual;
                }
                edit.CheckedChanged += EditChecked_CheckedChanged;
            }
            else
            {
                var edit = new Input
                {
                    Margin = new Padding(0),
                    PlaceholderText = Localization.Get("Filter", "筛选"),
                    Dock = DockStyle.Fill
                };
                tablePanel.Controls.Add(edit, 1, 0);
                edit.TextChanged += Edit_TextChanged;
            }
        }

        private void InitFilterValues()
        {
            var source = TableView.dataTmp?.rows;
            if (source == null)
            {
                dv.Tag = dv.DataSource = null;
                return;
            }
            if (CustomSource != null && CustomSource.Count > 0) LoadListCustom(source, CustomSource);
            else LoadList(_table.IFilterList(_column.Key), source);
        }
        private void LoadList(IList<Table.IRow> values, IList<Table.IRow> list)
        {
            int count = values.Count, check_count = 0;
            var items = new List<AntItem[]>(count);
            var selects = GetColumnSelect();
            var dir = new Dictionary<string, List<object>>(count);
            AntItem[]? item_null = null;
            foreach (var val in values)
            {
                bool check = list.Contains(val);
                if (check) check_count++;
                var value = val[_column.Key];
                if (value == null)
                {
                    if (Option.AllowNull == false) continue;
                    if (item_null == null)
                    {
                        item_null = new AntItem[] { new AntItem("tag"), new AntItem("check", check), new AntItem("text", new CellText().SetText("(空白)", "Filter.Blank").SetFore(Style.Db.TextTertiary)) };
                        items.Add(item_null);
                    }
                }
                else
                {
                    string text;
                    if (_column.Render == null) text = _column.GetDisplayText(value) ?? "";
                    else text = _column.Render(value, val.record, val.i)?.ToString() ?? "";
                    if (dir.TryGetValue(text, out var tmp)) tmp.Add(value);
                    else
                    {
                        var tmps = new List<object> { value };
                        dir.Add(text, tmps);
                        if (selects.TryGetValue(value, out var find)) items.Add(new AntItem[] { new AntItem("tag", tmps), new AntItem("check", check), new AntItem("text", new CellText(find.Text).SetPrefix(find.IconSvg)) });
                        else items.Add(new AntItem[] { new AntItem("tag", tmps), new AntItem("check", check), new AntItem("text", text) });
                    }
                }
            }
            dv.Tag = dv.DataSource = items;
        }
        private void LoadListCustom(IList<Table.IRow> values, IList<object> list)
        {
            var has = new List<object?>(values.Count);
            foreach (var val in values) has.Add(val[_column.Key]);

            int count = list.Count, check_count = 0;
            var items = new List<AntItem[]>(count);
            var selects = GetColumnSelect();
            var dir = new Dictionary<string, List<object>>(count);
            foreach (var value in list)
            {
                bool check = has.Contains(value);
                if (check) check_count++;

                string text = _column.GetDisplayText(value) ?? "";
                if (dir.TryGetValue(text, out var tmp)) tmp.Add(value);
                else
                {
                    var tmps = new List<object> { value };
                    dir.Add(text, tmps);
                    if (selects.TryGetValue(value, out var find)) items.Add(new AntItem[] { new AntItem("tag", tmps), new AntItem("check", check), new AntItem("text", new CellText(find.Text).SetPrefix(find.IconSvg)) });
                    else items.Add(new AntItem[] { new AntItem("tag", tmps), new AntItem("check", check), new AntItem("text", text) });
                }
            }
            dv.Tag = dv.DataSource = items;
        }
        private Dictionary<object, SelectItem> GetColumnSelect()
        {
            Dictionary<object, SelectItem> selects;
            if (_column is ColumnSelect columnSelect)
            {
                selects = new Dictionary<object, SelectItem>(columnSelect.Items.Count);
                foreach (var it in columnSelect.Items) selects.Add(it.Tag, it);
            }
            else selects = new Dictionary<object, SelectItem>(0);
            return selects;
        }

        #region 初始参数

        Form? popover;
        public void Set(Form? _popover) => popover = _popover;
        void LoadOffset()
        {
            if (_table.rows == null) return;
            if (popover is LayeredFormPopover layered)
            {
                foreach (var it in _table.rows[0].cells)
                {
                    if (it.COLUMN.Key == _column.Key && it is Table.TCellColumn col)
                    {
                        layered.LoadOffset(new System.Drawing.Rectangle(col.rect_filter.X + offsetX, col.rect_filter.Y + offsetY, col.rect_filter.Width, col.rect_filter.Height));
                        return;
                    }
                }
            }
        }

        #endregion

        bool isLoad = false;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            isLoad = true;
        }

        #endregion

        #region Properties

        public FilterOption Option => _column.Filter!;

        protected Table _table;

        /// <summary>
        /// Table视图控件
        /// </summary>
        public Table TableView => _table;

        protected Column _column;
        public Column FocusedColumn => _column;

        #endregion

        #region 列表

        private void inputSearch_TextChanged(object sender, EventArgs e) => Search();

        object[]? cc;
        void Search()
        {
            var search = inputSearch.Text;
            if (dv.Tag is List<AntItem[]> list)
            {
                if (cc == null)
                {
                    if (string.IsNullOrEmpty(search)) dv.DataSource = list;
                    else
                    {
                        var nl = new List<AntItem[]>(list.Count);
                        foreach (var it in list)
                        {
                            if (it[2].value is string text && SearchText(text, search)) nl.Add(it);
                        }
                        dv.DataSource = nl;
                    }
                }
                else
                {
                    var condition = (FilterConditions)cc[0];
                    var val = cc[1];
                    if (string.IsNullOrEmpty(search))
                    {
                        var nl = new List<AntItem[]>(list.Count);
                        foreach (var it in list)
                        {
                            if (SearchFilter(condition, it[0].value!, val)) nl.Add(it);
                        }
                        dv.DataSource = nl;
                    }
                    else
                    {
                        var nl = new List<AntItem[]>(list.Count);
                        foreach (var it in list)
                        {
                            if (it[2].value is string text && SearchText(text, search) && SearchFilter(condition, it[0].value!, val)) nl.Add(it);
                        }
                        dv.DataSource = nl;
                    }
                }
                Apply();
            }
        }
        void Search(object[]? value)
        {
            if (isLoad)
            {
                if (cc == null || value == null)
                {
                    if (cc == value) return;
                }
                else
                {
                    if (cc[0] == value[0] && cc[1] == value[1]) return;
                }
                cc = value;
                Search();
            }
        }

        bool SearchText(string? text, string search) => text?.IndexOf(search, StringComparison.OrdinalIgnoreCase) > -1;
        bool SearchFilter(FilterConditions condition, object val, object? value)
        {
            if (val is List<object> list)
            {
                foreach (var it in list)
                {
                    if (FilterCondition(condition, it, value)) return true;
                }
                return false;
            }
            else return FilterCondition(condition, val, value);
        }

        #region 筛选核心

        public static bool FilterCondition(FilterConditions condition, object? val, object? value)
        {
            // 处理双方为null的情况
            if (val == null && value == null) return condition == FilterConditions.Equal || condition == FilterConditions.None;

            // 处理单方为null的情况
            if (val == null || value == null)
            {
                return condition switch
                {
                    FilterConditions.Equal => false,
                    FilterConditions.NotEqual => true,
                    _ => false // 其他条件在单方为null时无效
                };
            }

            // 处理int
            if (TryGetValue(val, out int intVal))
            {
                if (TryGetValue(value, out int intValue)) return CompareNumeric(intVal, intValue, condition);
                else if (int.TryParse(value.ToString(), out intValue)) return CompareNumeric(intVal, intValue, condition);
            }

            // 处理double
            if (TryGetValue(val, out double doubleVal))
            {
                if (TryGetValue(value, out double doubleValue)) return CompareNumeric(doubleVal, doubleValue, condition);
                else if (double.TryParse(value.ToString(), out doubleValue)) return CompareNumeric(doubleVal, doubleValue, condition);
            }

            // 处理float
            if (TryGetValue(val, out float floatVal))
            {
                if (TryGetValue(value, out float floatValue)) return CompareNumeric(floatVal, floatValue, condition);
                else if (float.TryParse(value.ToString(), out floatValue)) return CompareNumeric(floatVal, floatValue, condition);
            }

            // 处理decimal
            if (TryGetValue(val, out decimal decimalVal))
            {
                if (TryGetValue(value, out decimal decimalValue)) return CompareNumeric(decimalVal, decimalValue, condition);
                else if (decimal.TryParse(value.ToString(), out decimalValue)) return CompareNumeric(decimalVal, decimalValue, condition);
            }

            // 处理bool
            if (TryGetValue(val, out bool boolVal))
            {
                if (TryGetValue(value, out bool boolValue)) return CompareBool(boolVal, boolValue, condition);
                else if (bool.TryParse(value.ToString(), out boolValue)) return CompareBool(boolVal, boolValue, condition);
            }

            // 处理DateTime
            if (TryGetValue(val, out DateTime dateTimeVal))
            {
                if (TryGetValue(value, out DateTime dateTimeValue)) return CompareDateTime(dateTimeVal, dateTimeValue, condition);
                else if (DateTime.TryParse(value.ToString(), out dateTimeValue)) return CompareDateTime(dateTimeVal, dateTimeValue, condition);
            }

            // 处理string
            if (val is string strVal && value is string strValue) return CompareString(strVal, strValue, condition);

            // 兜底处理 - 尝试转换为字符串
            string valStr = val.ToString() ?? string.Empty, valueStr = value.ToString() ?? string.Empty;
            return CompareString(valStr, valueStr, condition);
        }

        // 安全获取可空类型值的辅助方法
        private static bool TryGetValue<T>(object obj, out T value) where T : struct
        {
            value = default;

            // 处理直接是T类型的情况
            if (obj is T directValue)
            {
                value = directValue;
                return true;
            }

            // 处理装箱的Nullable<T>
            try
            {
                var type = obj.GetType();
                if (type == typeof(T) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    try
                    {
                        value = (T)Convert.ChangeType(obj, typeof(T));
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch { }

            return false;
        }

        private static bool CompareNumeric<T>(T val, T value, FilterConditions condition) where T : IComparable<T>
        {
            // 验证条件是否适用于数值类型
            if (condition is FilterConditions.Contain or FilterConditions.NotContain) return false;

            int comparison = val.CompareTo(value);
            return condition switch
            {
                FilterConditions.Equal => comparison == 0,
                FilterConditions.NotEqual => comparison != 0,
                FilterConditions.Greater => comparison > 0,
                FilterConditions.Less => comparison < 0,
                FilterConditions.None => true,
                _ => false
            };
        }

        private static bool CompareBool(bool val, bool value, FilterConditions condition)
        {
            // bool仅支持Equal/NotEqual
            if (condition == FilterConditions.Equal || condition == FilterConditions.NotEqual)
            {
                return condition == FilterConditions.Equal
                    ? val == value
                    : val != value;
            }
            return false;
        }

        private static bool CompareDateTime(DateTime val, DateTime value, FilterConditions condition)
        {
            // 验证条件是否适用于日期类型
            if (condition is FilterConditions.Contain or FilterConditions.NotContain) return false;

            int comparison = val.CompareTo(value);
            return condition switch
            {
                FilterConditions.Equal => comparison == 0,
                FilterConditions.NotEqual => comparison != 0,
                FilterConditions.Greater => comparison > 0,
                FilterConditions.Less => comparison < 0,
                FilterConditions.None => true,
                _ => false
            };
        }

        private static bool CompareString(string val, string value, FilterConditions condition)
        {
            return condition switch
            {
                FilterConditions.Equal => string.Equals(val, value, StringComparison.OrdinalIgnoreCase),
                FilterConditions.NotEqual => !string.Equals(val, value, StringComparison.OrdinalIgnoreCase),
                FilterConditions.Greater => string.Compare(val, value, StringComparison.OrdinalIgnoreCase) > 0,
                FilterConditions.Less => string.Compare(val, value, StringComparison.OrdinalIgnoreCase) < 0,
                FilterConditions.Contain => val.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0,
                FilterConditions.NotContain => val.IndexOf(value, StringComparison.OrdinalIgnoreCase) < 0,
                FilterConditions.None => true,
                _ => false
            };
        }

        #endregion

        #endregion

        private void SelectCondition_Changed(object sender, ObjectNEventArgs e)
        {
            if (e.Value is FilterConditions condition)
            {
                if (condition == Option.Condition) return;
                Option.Condition = condition;
                if (cc == null) return;
                Search(new object[] { condition, cc[1] });
            }
        }

        #region 筛选

        void Edit_TextChanged(object? sender, EventArgs e)
        {
            if (sender is InputNumber editNum)
            {
                if (decimal.TryParse(editNum.Text, out var num)) Edit_ValueChanged(sender, new DecimalEventArgs(num));
            }
            else if (sender is DatePicker editDate)
            {
                if (DateTime.TryParse(editDate.Text, out var date)) EditDate_ValueChanged(sender, new DateTimeNEventArgs(date));
            }
            else if (sender is Input edit) Search(new object[] { Option.Condition, edit.Text });
        }

        void EditSelect_ValueChanged(object sender, ObjectNEventArgs e)
        {
            if (e.Value == null) Search(null);
            else Search(new object[] { Option.Condition, e.Value });
        }

        void EditChecked_CheckedChanged(object sender, BoolEventArgs e) => Search(new object[] { Option.Condition, e.Value });

        void Edit_ValueChanged(object sender, DecimalEventArgs e) => Search(new object[] { Option.Condition, e.Value });

        void EditDate_ValueChanged(object sender, DateTimeNEventArgs e)
        {
            if (e.Value.HasValue) Search(new object[] { Option.Condition, e.Value });
            else Search(null);
        }

        #endregion

        int count = 0;
        private void btn_clean_Click(object sender, EventArgs e)
        {
            count = 0;
            cc = null;
            inputSearch.Text = string.Empty;
            Option.ClearFilter();
            btn_clean.Enabled = false;
            InitFilterValues();
            LoadOffset();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (count == 0)
            {
                Dispose();
                return;
            }
            if (_table.dataTmp == null) return;
            if (dv.DataSource is List<AntItem[]> list)
            {
                var tmp = new List<object?>(list.Count);
                foreach (var it in list)
                {
                    if (it[1].value is bool check && check)
                    {
                        if (it[0].value is List<object> data) tmp.AddRange(data);
                        else tmp.Add(null);
                    }
                }
                if (tmp.Count == _table.dataTmp.RowsCache.Length) Option.FilterValues = null;
                else Option.FilterValues = tmp;
                Option.UpdateFilter();
                if (_table.Filter_PopupEndEventMethod(Option)) Dispose();
                else
                {
                    LoadOffset();
                    btn_clean.Enabled = Option.Enabled;
                    InitFilterValues();
                }
            }
        }

        void Apply()
        {
            count++;
            if (realTime)
            {
                if (_table.dataTmp == null) return;
                if (dv.DataSource is List<AntItem[]> list)
                {
                    var tmp = new List<object?>(list.Count);
                    foreach (var it in list)
                    {
                        if (it[1].value is bool check && check && it[0].value is List<object> data) tmp.AddRange(data);
                    }
                    if (tmp.Count == _table.dataTmp.RowsCache.Length) Option.FilterValues = null;
                    else Option.FilterValues = tmp;
                    Option.UpdateFilter();
                    LoadOffset();
                    btn_clean.Enabled = Option.Enabled;
                }
            }
        }

        private void dv_CheckedChanged(object sender, TableCheckEventArgs e) => Apply();
    }
}
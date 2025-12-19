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
using System.Linq;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        protected const string SvgSummaryMIN = "<svg viewBox=\"0 0 1194 1024\"><path d=\"M682.666667 256h170.666666v768H682.666667z\"></path><path d=\"M0 768h170.666667v256H0z\" fill=\"#FF0000\"></path><path d=\"M341.333333 512h170.666667v512H341.333333zM1024 0h170.666667v1024h-170.666667z\"></path></svg>";
        protected const string SvgSummaryMAX = "<svg viewBox=\"0 0 1194 1024\"><path d=\"M682.666667 256h170.666666v768H682.666667zM0 768h170.666667v256H0zM341.333333 512h170.666667v512H341.333333z\"></path><path d=\"M1024 0h170.666667v1024h-170.666667z\" fill=\"#FF0000\"></path></svg>";
        protected const string SvgSummaryAVG = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M89.6 0m38.4 0l0 0q38.4 0 38.4 38.4l0 947.2q0 38.4-38.4 38.4l0 0q-38.4 0-38.4-38.4l0-947.2q0-38.4 38.4-38.4Z\"></path><path d=\"M428.736 256m83.2 0l0 0q83.2 0 83.2 83.2l0 345.6q0 83.2-83.2 83.2l0 0q-83.2 0-83.2-83.2l0-345.6q0-83.2 83.2-83.2Z\" fill=\"#d81e06\"></path><path d=\"M857.6 0m38.4 0l0 0q38.4 0 38.4 38.4l0 947.2q0 38.4-38.4 38.4l0 0q-38.4 0-38.4-38.4l0-947.2q0-38.4 38.4-38.4Z\"></path></svg>";
        protected const string SvgSummarySUM = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M843.776 205.312v-88.064H266.24l339.968 362.496c21.504 22.528 20.992 57.856-0.512 80.384L269.824 906.752h574.464v-53.248c0-32.256 26.624-58.368 58.88-58.368 32.768 0 58.88 26.112 58.88 58.368V965.12c0 32.256-26.624 58.368-58.88 58.368H131.072c-23.552 0-45.056-13.824-54.272-35.84-9.216-21.504-4.608-46.592 11.776-63.488l393.728-406.016L88.064 97.792C71.68 81.408 67.072 56.32 76.8 35.328 86.016 13.824 107.52 0 131.072 0h772.096c32.768 0 58.88 26.112 58.88 58.368v146.944c0 32.256-26.624 58.368-58.88 58.368-32.768 0-59.392-26.112-59.392-58.368z\"></path></svg>";
        protected const string SvgSummaryCNT = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M609.792 877.568H166.4l283.136-346.624-282.624-427.52h419.328c84.992 0 157.184 56.832 216.576 169.984h14.336l-30.208-224.768H14.848L353.28 531.456 14.848 966.144h772.608l30.208-225.28h-14.336c-56.832 91.136-121.344 136.704-193.536 136.704z\" fill=\"#13227a\"></path><path d=\"M593.408 439.296c16.384 0 30.72 2.56 43.008 8.192V395.776c-11.776-4.608-30.72-7.168-49.664-7.168-59.904 0-95.232 25.6-95.232 118.272 0 92.16 35.328 118.272 95.232 118.784 17.408 0 39.936-3.072 51.2-8.704v-50.176c-13.824 6.144-28.16 8.192-43.52 8.192-35.328 0-50.176-14.848-50.176-67.584 0-52.736 14.848-68.096 49.152-68.096zM792.064 388.608c-28.16 0-50.176 10.24-60.416 31.744v-27.648h-51.712v228.352h51.712V470.016c5.632-23.552 19.456-31.232 40.96-31.232 25.088 0 31.232 11.776 31.232 42.496v139.264h51.712V464.896c0-50.176-15.872-76.288-63.488-76.288zM1002.496 441.344v-48.128H962.56V338.944h-52.224v53.76h-25.6v48.128h25.6v106.496c0 59.904 16.896 75.776 64.512 75.776 8.704 0 17.408-1.024 25.088-2.048v-47.104c-4.096 1.024-9.216 1.536-15.36 1.536-17.92 0-22.016-6.144-22.016-32.768V441.344h39.936z\"></path></svg>";
        protected const string SvgSummaryNONE = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M832 460.8l128-128c12.8-12.8 38.4-12.8 51.2 0 12.8 12.8 12.8 38.4 0 51.2l-128 128 128 128c12.8 12.8 12.8 38.4 0 51.2s-38.4 12.8-51.2 0l-128-128-128 128c-12.8 12.8-38.4 12.8-51.2 0s-12.8-38.4 0-51.2l128-128-128-128c-12.8-12.8-12.8-38.4 0-51.2 12.8-12.8 38.4-12.8 51.2 0l128 128z\" fill=\"#d81e06\"></path><path d=\"M736 64H38.4c-12.8 0-19.2 0-25.6 6.4-12.8 12.8-12.8 38.4 0 51.2l38.4 38.4 313.6 313.6c6.4 6.4 6.4 25.6 0 32L64 806.4l-64 64v51.2c0 19.2 19.2 38.4 38.4 38.4h704c19.2 0 32-12.8 32-32v-64c0-19.2-12.8-32-32-32s-38.4 12.8-38.4 32v32H70.4l345.6-345.6c32-32 32-89.6 0-121.6L108.8 128H704v32c0 19.2 12.8 32 32 32s32-12.8 32-32v-64c0-19.2-12.8-32-32-32z\"></path></svg>";
        protected const string SvgSummarySelectionMode = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M921.6 0 102.4 0C46.08 0 0 46.08 0 102.4l0 819.2c0 56.32 46.08 102.4 102.4 102.4l819.2 0c56.32 0 102.4-46.08 102.4-102.4L1024 102.4C1024 46.08 977.92 0 921.6 0zM972.8 921.6c0 28.2368-22.9632 51.2-51.2 51.2L102.4 972.8c-28.2368 0-51.2-22.9632-51.2-51.2L51.2 102.4c0-28.2368 22.9632-51.2 51.2-51.2l819.2 0c28.2368 0 51.2 22.9632 51.2 51.2L972.8 921.6z\"></path><path d=\"M128 422.4l768 0 0 179.2-768 0 0-179.2Z\"></path><path d=\"M128 704l768 0 0 179.2-768 0 0-179.2Z\" fill=\"#d81e06\"></path><path d=\"M128 140.8l768 0 0 179.2-768 0 0-179.2Z\" fill=\"#d81e06\"></path></svg>";
        protected static string SvgSummaryAllMode = SvgSummarySelectionMode.Replace("fill=\"#d81e06\"", "");

        protected IContextMenuStripItem[] InitSummaryMenu(TSummaryType type, bool selectionMode)
        {
            ContextMenuStripItem sum = GetMenu(TSummaryType.SUM, "总和", "SUM", SvgSummarySUM),
             avg = GetMenu(TSummaryType.AVG, "平均", "AVG", SvgSummaryAVG),
             min = GetMenu(TSummaryType.MIN, "最小", "MIN", SvgSummaryMIN),
             max = GetMenu(TSummaryType.MAX, "最大", "MAX", SvgSummaryMAX),
             count = GetMenu(TSummaryType.Count, "计数", "COUNT", SvgSummaryCNT),
             no = new ContextMenuStripItem().SetText("不汇总", "Table.Summary.NONE").SetIcon(SvgSummaryNONE).SetTag(TSummaryType.None);
            ContextMenuStripItem? mode = null;
            if (MultipleRows)
            {
                if (selectionMode) mode = new ContextMenuStripItem().SetText("选择行模式", "Table.Summary.Mode").SetIcon(SvgSummarySelectionMode).SetID("MODE").SetChecked();
                else mode = new ContextMenuStripItem().SetText("所有行模式", "Table.Summary.ModeALL").SetIcon(SvgSummaryAllMode).SetID("MODE");
            }
            switch (type)
            {
                case TSummaryType.SUM:
                    sum.Checked = true;
                    break;
                case TSummaryType.AVG:
                    avg.Checked = true;
                    break;
                case TSummaryType.MIN:
                    min.Checked = true;
                    break;
                case TSummaryType.MAX:
                    max.Checked = true;
                    break;
                case TSummaryType.Count:
                    count.Checked = true;
                    break;
                case TSummaryType.Custom:
                    count.Enabled = max.Enabled = min.Enabled = avg.Enabled = no.Enabled = sum.Enabled = false;
                    break;
                default:
                    no.Checked = true;
                    break;
            }
            if (mode == null)
            {
                return new IContextMenuStripItem[]
                {
                    sum,avg,min,max,count,
                    new ContextMenuStripItemDivider(),no
                };
            }
            return new IContextMenuStripItem[]
            {
                mode,new ContextMenuStripItemDivider(),
                sum,avg,min,max,count,
                new ContextMenuStripItemDivider(),no
            };
        }
        ContextMenuStripItem GetMenu(TSummaryType value, string key, string id, string svg) => new ContextMenuStripItem().SetText(key, "Table.Summary." + id).SetSubText(id).SetIcon(svg).SetTag(value);
        private Form? FMenuStrip;
        public bool SummaryRowFocused => FMenuStrip != null;
        private void Summary_RClick(CELLDB cell)
        {
            FMenuStrip?.Dispose();
            var colSummary = cell.col;
            if (colSummary.SummaryItem == null) colSummary.SummaryItem = new SummaryItemOption(TSummaryType.None);
            FMenuStrip = new ContextMenuStrip.Config(this, item =>
            {
                FMenuStrip = null;
                if (item.Tag is TSummaryType summaryType)
                {
                    colSummary.SummaryItem.SummaryType = summaryType;
                    OnUpdateSummaries();
                }
                else if (item.ID == "MODE")
                {
                    colSummary.SummaryItem.SelectionMode = !item.Checked;
                    OnUpdateSummaries();
                }
            }, InitSummaryMenu(colSummary.SummaryItem.SummaryType, colSummary.SummaryItem.SelectionMode)).SetFont(Font).SetAlign(TAlign.Top).SetRatio(0.7F).SetClose(() => FMenuStrip = null).open();
        }
        /// <summary>
        /// 已启用汇总的列
        /// </summary>
        public Column[]? SummaryColumns
        {
            get
            {
                if (columns == null) return null;
                var tmp = new List<Column>(columns.Count);
                foreach (var col in columns)
                {
                    if (col.SummaryItem != null && col.SummaryItem.SummaryType != TSummaryType.None) tmp.Add(col);
                }
                if (tmp.Count > 0) return tmp.ToArray();
                return null;
            }
        }

        /// <summary>
        /// 更新汇总列的数据（异步）
        /// </summary>
        public void OnUpdateSummaries()
        {
            if (SummaryCustomize == false) return;
            if (dataTmp == null || dataTmp.rows.Length == 0)
            {
                Summary = null;
                return;
            }
            ITask.Run(UpdateSummaries);
        }

        /// <summary>
        /// 更新汇总列的数据
        /// </summary>
        void UpdateSummaries()
        {
            lock (dataTmp!)
            {
                var summaryColumns = SummaryColumns;
                if (summaryColumns == null || summaryColumns.Length == 0) return;//外部汇总
                var items = new List<AntItem>(summaryColumns.Length);
                bool enabledSelectionMode = MultipleRows && selectedIndex.Length > 0;
                foreach (var col in summaryColumns)
                {
                    if (col.SummaryItem == null || col.SummaryItem?.SummaryType == TSummaryType.None) continue;
                    var rows = dataTmp.rows;//默认所有行
                    if (enabledSelectionMode && col.SummaryItem!.SelectionMode)
                    {
                        List<IRow> selectedRows = new List<IRow>(rows.Length);
                        foreach (var row in rows)
                        {
                            if (selectedIndex.Contains(row!.i + 1)) selectedRows.Add(row);
                        }
                        if (selectedRows.Count > 0) rows = selectedRows.ToArray();
                    }
                    var item = new AntItem(col.Key);
                    switch (col.SummaryItem?.SummaryType)
                    {
                        case TSummaryType.Text:
                            item.value = col.SummaryItem.DisplayText;
                            break;
                        case TSummaryType.SUM:
                            {
                                double sum = 0;
                                foreach (var row in rows)
                                {
                                    var val = row[col.Key];
                                    if (val != null && double.TryParse(val.ToString(), out double d)) sum += d;
                                }
                                item.value = GetSummaryItemText(sum, col);
                            }
                            break;
                        case TSummaryType.AVG:
                            {
                                double sum = 0;
                                int count = 0;
                                foreach (var row in rows)
                                {
                                    var val = row[col.Key];
                                    if (val != null && double.TryParse(val.ToString(), out double d))
                                    {
                                        sum += d;
                                        count++;
                                    }
                                }
                                var avg = count > 0 ? sum / count : 0;
                                item.value = GetSummaryItemText(avg, col);
                            }
                            break;
                        case TSummaryType.MIN:
                            {
                                bool strCompare = false;
                                double min = double.MaxValue;
                                string? minStr = string.Empty;
                                int count = 0;
                                foreach (var row in rows)
                                {
                                    var val = row[col.Key];
                                    if (!strCompare && val is string)
                                    {
                                        strCompare = true;
                                        minStr = val?.ToString();
                                        continue;
                                    }
                                    if (strCompare) minStr = CompareString(val?.ToString(), minStr, false);
                                    else
                                    {
                                        if (strCompare) continue;
                                        if (val != null && double.TryParse(val.ToString(), out double d))
                                        {
                                            if (d < min) min = d;
                                            count++;
                                        }
                                    }
                                }
                                var result = count > 0 ? min : 0;
                                item.value = strCompare ? minStr : GetSummaryItemText(result, col);
                            }
                            break;
                        case TSummaryType.MAX:
                            {
                                bool strCompare = false;
                                double max = double.MinValue;
                                string? maxStr = string.Empty;
                                int count = 0;
                                foreach (var row in rows)
                                {
                                    var val = row[col.Key];
                                    if (!strCompare && val is string)
                                    {
                                        strCompare = true;
                                        maxStr = val?.ToString();
                                        continue;
                                    }
                                    if (strCompare) maxStr = CompareString(maxStr, val?.ToString(), true);
                                    else
                                    {
                                        if (val != null && double.TryParse(val.ToString(), out double d))
                                        {
                                            if (d > max) max = d;
                                            count++;
                                        }
                                    }
                                }
                                double result = count > 0 ? max : 0;
                                item.value = strCompare ? maxStr : GetSummaryItemText(result, col);
                            }
                            break;
                        case TSummaryType.Count:
                            item.value = GetSummaryItemText(rows.Length, col.SummaryItem?.DisplayFormat);
                            break;
                        default://Custom
                                // 自定义汇总
                            double total = 0;
                            if (CustomSummaryCalculate != null)
                            {
                                foreach (var row in rows)
                                {
                                    var val = row[col.Key];
                                    var resultArg = new TableCustomSummaryEventArgs(col, row.record, row.i, false);
                                    CustomSummaryCalculate(this, resultArg);
                                    if (resultArg.TotalValue != null) total += resultArg.TotalValue.Value;
                                }
                                var resultEnd = new TableCustomSummaryEventArgs(col, rows, 0, true);
                                CustomSummaryCalculate(this, resultEnd);
                                if (resultEnd.TotalValue != null) total += resultEnd.TotalValue.Value;
                            }
                            item.value = GetSummaryItemText(total, col);
                            break;
                    }
                    items.Add(item);
                }
                if (items.Count > 0) Summary = new List<AntItem[]> { items.ToArray() };
            }
        }
        private string? GetSummaryItemText(object? value, string? format) => Column.GetDisplayText(value, string.IsNullOrEmpty(format) ? "0.#" : format);
        private string? GetSummaryItemText(object? value, Column column) => Column.GetDisplayText(value, column.SummaryItem != null && string.IsNullOrEmpty(column.SummaryItem.DisplayFormat) == false ? column.SummaryItem.DisplayFormat : string.IsNullOrEmpty(column.DisplayFormat) ? "0.#" : column.DisplayFormat);
        private string? CompareString(string? str1, string? str2, bool MAX, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            int comparison = str1?.Length != str2?.Length ? str1?.Length > str2?.Length ? 1 : 0 : string.Compare(str1, str2, comparisonType);

            return MAX ? (comparison > 0 ? str1 : str2) : (comparison <= 0 ? str1 : str2);
        }
    }
}
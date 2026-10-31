// Copyright (C) 17379620 Tom. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// 单元格编辑器类型
    /// </summary>
    public enum TEditorType
    {
        /// <summary>
        /// 文本
        /// </summary>
        Text,
        /// <summary>
        /// 数字
        /// </summary>
        Number,
        /// <summary>
        /// 日期
        /// </summary>
        Date,
        /// <summary>
        /// 下拉选择
        /// </summary>
        Select
    }

    /// <summary>
    /// 可编辑表格 EditableTable
    /// </summary>
    /// <remarks>
    /// 行级编辑模式：新增/编辑时一行多个单元格同时进入编辑，
    /// 保存时做行级校验，取消时还原原始值。基于 Table 的行级编辑扩展点实现。
    /// 编辑器用 SetEditor 在代码中配置（列集合与 Table 一致，代码配置）。
    /// </remarks>
    [Description("EditableTable 可编辑表格")]
    [ToolboxItem(true)]
    public class EditableTable : Table
    {
        /// <summary>内置操作列的 Key</summary>
        const string ACTION_KEY = "editable_actions";
        /// <summary>操作列按钮 Id，仅在 OnCellButtonClick 里区分点击的是哪个按钮</summary>
        const string BTN_EDIT = "edit", BTN_SAVE = "save", BTN_CANCEL = "cancel", BTN_DELETE = "delete", BTN_COPY = "copy";

        #region 编辑器配置

        Dictionary<string, CellEditor> editors = new Dictionary<string, CellEditor>();

        /// <summary>
        /// 注册列编辑器（列将自动设为可编辑）
        /// </summary>
        /// <param name="columnKey">列绑定名称（Key）</param>
        /// <param name="type">编辑器类型</param>
        public CellEditor SetEditor(string columnKey, TEditorType type)
        {
            var editor = new CellEditor(type);
            editors[columnKey] = editor;
            foreach (Column col in Columns)
            {
                if (col.Key == columnKey) { col.Editable = true; break; }
            }
            return editor;
        }

        /// <summary>
        /// 列编辑器配置
        /// </summary>
        public class CellEditor
        {
            public CellEditor(TEditorType type) { Type = type; }

            /// <summary>编辑器类型</summary>
            public TEditorType Type { get; private set; }
            /// <summary>是否必填（保存时校验，失败拦截保存并标红单元格）</summary>
            public bool Required { get; private set; }
            /// <summary>必填提示文案（默认 "{列名}不能为空"）</summary>
            public string? RequiredMessage { get; private set; }
            /// <summary>最大文本长度（0 不限制）</summary>
            public int MaxLength { get; private set; }
            /// <summary>数值最小值</summary>
            public decimal? Min { get; private set; }
            /// <summary>数值最大值</summary>
            public decimal? Max { get; private set; }
            /// <summary>下拉选项（SelectItem 或 原始值）</summary>
            internal List<object> Items { get; } = new List<object>();

            public CellEditor SetRequired(bool required = true, string? message = null)
            {
                Required = required;
                RequiredMessage = message;
                return this;
            }
            public CellEditor SetMaxLength(int length)
            {
                MaxLength = length;
                return this;
            }
            public CellEditor SetRange(decimal? min, decimal? max)
            {
                Min = min;
                Max = max;
                return this;
            }
            public CellEditor SetItems(params object[] items)
            {
                Items.Clear();
                Items.AddRange(items);
                return this;
            }

            internal string? Validate(object? value, string title)
            {
                if (Required)
                {
                    bool empty = value == null || (value is string str && string.IsNullOrWhiteSpace(str));
                    if (empty) return RequiredMessage ?? string.Format(Localization.Get("Table.Edit.Required", "{0}不能为空"), title);
                }
                if (MaxLength > 0 && value != null && value.ToString()!.Length > MaxLength)
                    return string.Format(Localization.Get("Table.Edit.MaxLength", "{0}超出最大长度 {1}"), title, MaxLength);
                if ((Min.HasValue || Max.HasValue) && value != null && !(value is string))
                {
                    try
                    {
                        decimal num = Convert.ToDecimal(value);
                        if (Min.HasValue && num < Min.Value) return string.Format(Localization.Get("Table.Edit.Min", "{0}不能小于 {1}"), title, Min.Value);
                        if (Max.HasValue && num > Max.Value) return string.Format(Localization.Get("Table.Edit.Max", "{0}不能大于 {1}"), title, Max.Value);
                    }
                    catch { }
                }
                return null;
            }
        }

        #endregion

        #region 行级编辑状态

        object? editingRecord = null, editingSnapshot = null;
        HashSet<object> newRecords = new HashSet<object>();
        List<string> errorColumns = new List<string>();

        /// <summary>
        /// 当前编辑中的行记录
        /// </summary>
        public object? EditingRecord => editingRecord;

        /// <summary>
        /// 是否有行正在编辑
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsRowEditing => editingRecord != null;

        /// <summary>
        /// 校验失败的列 Key 集合
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] ErrorColumns => errorColumns.ToArray();

        #endregion

        #region 行操作

        /// <summary>
        /// 新增一行并立即进入行编辑
        /// </summary>
        /// <param name="record">新行数据对象</param>
        /// <param name="position">插入位置（-1 为末尾，默认追加到末行；传 0 插入第一行）</param>
        public bool AddRecord(object record, int position = -1)
        {
            if (record == null) return false;
            if (editingRecord != null && !SaveRow()) return false;
            var list = DataSource as IList;
            if (list == null) return false;
            if (position < 0 || position > list.Count) position = list.Count;
            list.Insert(position, record);
            if (!(list is IBindingList)) Refresh();
            newRecords.Add(record);
            RowAdded?.Invoke(this, new RowEventArgs(record));
            return BeginRowEdit(record);
        }

        /// <summary>
        /// 复制一行并立即进入行编辑
        /// </summary>
        /// <param name="record">被复制的行记录</param>
        public bool CopyRecord(object record)
        {
            if (record == null) return false;
            if (editingRecord != null && !SaveRow()) return false;
            object? copy;
            try
            {
                copy = Activator.CreateInstance(record.GetType());
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(record))
                {
                    if (prop.IsReadOnly) continue;
                    try { prop.SetValue(copy, prop.GetValue(record)); } catch { }
                }
            }
            catch { return false; }
            if (copy == null) return false;
            var list = DataSource as IList;
            if (list == null) return false;
            int index = list.IndexOf(record);
            list.Insert(index + 1, copy);
            if (!(list is IBindingList)) Refresh();
            newRecords.Add(copy);
            RowAdded?.Invoke(this, new RowEventArgs(copy));
            return BeginRowEdit(copy);
        }

        /// <summary>
        /// 让一行进入行编辑
        /// </summary>
        /// <param name="record">行记录</param>
        public bool BeginRowEdit(object record)
        {
            if (record == null) return false;
            // rows 在首帧 LoadLayout 才构建，自举一次布局
            if (rows == null) LoadLayout();
            if (rows == null) return false;
            if (editingRecord != null && !ReferenceEquals(editingRecord, record))
            {
                if (!SaveRow()) return false;
            }
            int idx0 = IndexOfRecord(record);
            if (idx0 < 0) return false;

            editingRecord = record;
            editingSnapshot = Snapshot(record);
            errorColumns.Clear();
            EnsureActionColumn();

            Refresh();
            int rowIndex = IndexOfRecord(record);
            if (rowIndex < 0) { ResetEditState(); return false; }

            var columns = EditableColumnIndexes(rowIndex);
            if (columns.Count == 0) { ResetEditState(); return false; }

            bool ok = EnterEditMode(rowIndex, columns.ToArray());
            if (!ok) ResetEditState();
            Invalidate();
            return ok;
        }

        /// <summary>
        /// 保存当前编辑行：全部编辑控件值写回 → 行级校验 → 触发 RowSaving（可拦截）
        /// </summary>
        /// <remarks>
        /// 校验或保存被拦截时整行编辑控件保持活跃，可继续修改后再次保存
        /// </remarks>
        /// <returns>校验通过且未被拦截返回 true，编辑态结束；否则保持编辑并标记错误单元格</returns>
        public bool SaveRow()
        {
            if (editingRecord == null) return false;
            var record = editingRecord;
            var snapshot = editingSnapshot;

            EditModeCommitAll();

            if (!ValidateRow(record))
            {
                Invalidate();
                return false;
            }

            var args = new RowSavingEventArgs(record, snapshot);
            RowSaving?.Invoke(this, args);
            if (args.Cancel)
            {
                Invalidate();
                return false;
            }

            EditModeClose();
            newRecords.Remove(record);
            ResetEditState();
            Refresh();
            RowSaved?.Invoke(this, new RowEventArgs(record));
            Invalidate();
            return true;
        }

        /// <summary>
        /// 取消编辑并还原到进入编辑前的值（AddRecord 新增的行取消后被移除）
        /// </summary>
        public void CancelRow()
        {
            if (editingRecord == null) return;
            var record = editingRecord;
            bool wasNew = newRecords.Contains(record);

            EditModeClose();
            if (!wasNew && editingSnapshot != null) Restore(record, editingSnapshot);
            ResetEditState();
            if (wasNew)
            {
                newRecords.Remove(record);
                RemoveFromSource(record);
            }
            else Refresh();
            RowCanceled?.Invoke(this, new RowEventArgs(record));
            Invalidate();
        }

        /// <summary>
        /// 删除行（默认弹窗确认）
        /// </summary>
        /// <param name="record">行记录</param>
        /// <param name="confirm">是否弹窗确认</param>
        public void DeleteRow(object record, bool confirm = true)
        {
            if (record == null) return;
            if (confirm)
            {
                var form = FindForm();
                if (Modal.open(new Modal.Config(form!, Localization.Get("Table.Edit.DeleteTitle", "是否删除"), new Modal.TextLine[] {
                    new Modal.TextLine(Localization.Get("Table.Edit.DeleteText", "此行数据将被删除，删除后不可恢复"), AntdUI.Style.Db.TextSecondary)
                }, TType.Error)
                {
                    CancelText = null,
                    OkType = TTypeMini.Error,
                    OkText = Localization.Get("Table.Edit.DeleteOk", "删除")
                }) != DialogResult.OK) return;
            }
            if (ReferenceEquals(editingRecord, record)) { ResetEditState(); EditModeClose(); }
            newRecords.Remove(record);
            RemoveFromSource(record);
            RowDeleted?.Invoke(this, new RowEventArgs(record));
            Invalidate();
        }

        void ResetEditState()
        {
            editingRecord = null;
            editingSnapshot = null;
            errorColumns.Clear();
        }

        void RemoveFromSource(object record)
        {
            var list = DataSource as IList;
            if (list == null) return;
            try { list.Remove(record); } catch { }
            if (!(list is IBindingList)) Refresh();
        }

        #endregion

        #region 内部实现

        int IndexOfRecord(object record)
        {
            if (rows == null) return -1;
            var list = rows.List;
            for (int i = 0; i < list.Length; i++)
            {
                if (ReferenceEquals(list[i].RECORD, record)) return i;
            }
            return -1;
        }

        List<int> EditableColumnIndexes(int rowIndex)
        {
            var result = new List<int>();
            var row = rows!.List[rowIndex];
            var cells = row.cells;
            for (int i = 0; i < cells.Length; i++)
            {
                var column = cells[i].COLUMN;
                if (column == null || column.Editable != true) continue;
                if (!editors.ContainsKey(column.Key)) continue;
                result.Add(i);
            }
            return result;
        }

        /// <summary>
        /// 行快照
        /// </summary>
        static object? Snapshot(object record)
        {
            try
            {
                var method = record.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
                return method?.Invoke(record, null);
            }
            catch { return null; }
        }

        static void Restore(object record, object snapshot)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(record))
            {
                if (prop.IsReadOnly) continue;
                try { prop.SetValue(record, prop.GetValue(snapshot)); } catch { }
            }
        }

        static object? GetRecordValue(object record, string key)
        {
            var prop = TypeDescriptor.GetProperties(record).Find(key, false);
            return prop?.GetValue(record);
        }

        int ColumnIndexOf(string key)
        {
            for (int i = 0; i < Columns.Count; i++)
            {
                if (Columns[i].Key == key) return i;
            }
            return -1;
        }

        bool ValidateRow(object record)
        {
            errorColumns.Clear();
            string? firstError = null;
            foreach (Column col in Columns)
            {
                if (!editors.TryGetValue(col.Key, out var editor)) continue;
                object? value = GetRecordValue(record, col.Key);
                string? error = editor.Validate(value, col.Title);
                if (error != null)
                {
                    errorColumns.Add(col.Key);
                    if (firstError == null) firstError = error;
                }
            }
            if (firstError != null)
            {
                Message.warn(new Target(this), firstError);
                foreach (string key in errorColumns)
                {
                    int idx = ColumnIndexOf(key);
                    var input = idx >= 0 ? EditModeInputAtColumn(idx) : null;
                    if (input != null) input.Status = TType.Error;
                }
                return false;
            }
            return true;
        }

        void ClearErrorStatus(Column column)
        {
            if (column == null || !errorColumns.Contains(column.Key)) return;
            errorColumns.Remove(column.Key);
            int idx = ColumnIndexOf(column.Key);
            if (idx >= 0)
            {
                var input = EditModeInputAtColumn(idx);
                if (input != null) input.Status = TType.None;
            }
        }

        protected override bool OnCellEndEdit(string value, object record, int rowIndex, int columnIndex, Column column)
        {
            bool result = base.OnCellEndEdit(value, record, rowIndex, columnIndex, column);
            ClearErrorStatus(column);
            return result;
        }

        protected override bool OnCellEndValueEdit(object? value, object record, int rowIndex, int columnIndex, Column column)
        {
            bool result = base.OnCellEndValueEdit(value, record, rowIndex, columnIndex, column);
            ClearErrorStatus(column);
            return result;
        }

        #endregion

        #region 编辑器注入与绘制

        protected override void OnCellBeginEditInputStyle(TableBeginEditInputStyleEventArgs e)
        {
            base.OnCellBeginEditInputStyle(e);
            if (e.Column == null || !editors.TryGetValue(e.Column.Key, out var editor)) return;
            switch (editor.Type)
            {
                case TEditorType.Number:
                    // InputNumber 的 Text 会被内部值重置，必须用 Value 初始化
                    var number = new InputNumber { AcceptsEscape = true };
                    if (decimal.TryParse(e.Input.Text, out var num)) number.Value = num;
                    e.Set(number);
                    break;
                case TEditorType.Date:
                    // DatePicker 同理，用 Value 初始化
                    var picker = new DatePicker { AcceptsEscape = true };
                    if (DateTime.TryParse(e.Input.Text, out var dt)) picker.Value = dt;
                    e.Set(picker);
                    break;
                case TEditorType.Select:
                    var select = new Select { AcceptsEscape = true, List = true, ListAutoWidth = true };
                    foreach (var item in editor.Items)
                    {
                        if (item is SelectItem selectItem) select.Items.Add(selectItem);
                        else select.Items.Add(new SelectItem(item?.ToString() ?? string.Empty, item!));
                    }
                    select.SelectedValue = e.Value;
                    e.Set(select);
                    break;
                case TEditorType.Text:
                default:
                    if (editor.MaxLength > 0) e.Input.MaxLength = editor.MaxLength;
                    break;
            }
        }

        protected override void OnRowPaintBegin(Canvas canvas, Rectangle rect, object record, RowType rowType, int rowIndex)
        {
            base.OnRowPaintBegin(canvas, rect, record, rowType, rowIndex);
            if (editingRecord != null && ReferenceEquals(record, editingRecord))
            {
                canvas.Fill(Colour.PrimaryBg.Get(nameof(Table)), rect);
            }
        }

        #endregion

        #region 内置操作列

        bool builtinActions = false;
        /// <summary>
        /// 是否启用内置操作列（编辑/保存/取消/删除按钮，由 AddRecord/BeginRowEdit 自动补充）
        /// </summary>
        [Description("是否启用内置操作列"), Category("行为"), DefaultValue(false)]
        public bool BuiltinActions
        {
            get => builtinActions;
            set
            {
                builtinActions = value;
                if (value) EnsureActionColumn();
            }
        }

        bool showEditButton = true, showCopyButton = true, showDeleteButton = true;
        /// <summary>
        /// 内置操作列是否显示「编辑」按钮
        /// </summary>
        [Description("内置操作列显示「编辑」按钮"), Category("行为"), DefaultValue(true)]
        public bool ShowEditButton
        {
            get => showEditButton;
            set { if (showEditButton == value) return; showEditButton = value; Refresh(); }
        }

        /// <summary>
        /// 内置操作列是否显示「复制」按钮
        /// </summary>
        [Description("内置操作列显示「复制」按钮"), Category("行为"), DefaultValue(true)]
        public bool ShowCopyButton
        {
            get => showCopyButton;
            set { if (showCopyButton == value) return; showCopyButton = value; Refresh(); }
        }

        /// <summary>
        /// 内置操作列是否显示「删除」按钮
        /// </summary>
        [Description("内置操作列显示「删除」按钮"), Category("行为"), DefaultValue(true)]
        public bool ShowDeleteButton
        {
            get => showDeleteButton;
            set { if (showDeleteButton == value) return; showDeleteButton = value; Refresh(); }
        }

        void EnsureActionColumn()
        {
            if (!builtinActions) return;
            foreach (Column col in Columns)
            {
                if (col.Key == ACTION_KEY) return;
            }
            Columns.Add(new EditableActionColumn(this).SetWidthAuto());
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            EnsureActionColumn();
        }

        ICell[] BuildActionButtons(object? record)
        {
            if (record != null && ReferenceEquals(record, editingRecord))
            {
                return new ICell[] {
                    new CellButton(BTN_SAVE, Localization.Get("Table.Edit.Save", "保存"), TTypeMini.Primary),
                    new CellButton(BTN_CANCEL, Localization.Get("Table.Edit.Cancel", "取消"))
                };
            }
            var buttons = new List<ICell>();
            if (showEditButton) buttons.Add(new CellButton(BTN_EDIT, Localization.Get("Table.Edit.Edit", "编辑"), TTypeMini.Primary));
            if (showCopyButton) buttons.Add(new CellButton(BTN_COPY, Localization.Get("Table.Edit.Copy", "复制")));
            if (showDeleteButton) buttons.Add(new CellButton(BTN_DELETE, Localization.Get("Table.Edit.Delete", "删除"), TTypeMini.Error));
            return buttons.ToArray();
        }

        class EditableActionColumn : Column
        {
            readonly EditableTable owner;
            public EditableActionColumn(EditableTable owner) : base(ACTION_KEY, Localization.Get("Table.Edit.Action", "操作"))
            {
                this.owner = owner;
                Editable = false;
                // 属性缺失时 Table 以 Render 的返回值构建单元格；返回 ICell[] 即渲染为按钮模板
                Render = (value, record, index) => owner.BuildActionButtons(record);
            }
        }

        protected override void OnCellButtonClick(CellLink btn, object record, RowType rowType, int rowIndex, int columnIndex, Column column, Rectangle rect, MouseEventArgs e)
        {
            base.OnCellButtonClick(btn, record, rowType, rowIndex, columnIndex, column, rect, e);
            if (column == null || column.Key != ACTION_KEY || btn == null) return;
            switch (btn.Id)
            {
                case BTN_EDIT: BeginRowEdit(record); break;
                case BTN_SAVE: SaveRow(); break;
                case BTN_CANCEL: CancelRow(); break;
                case BTN_COPY: CopyRecord(record); break;
                case BTN_DELETE: DeleteRow(record); break;
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 行保存前发生（e.Cancel = true 可拦截保存，行保持编辑态）
        /// </summary>
        [Description("行保存前发生(e.Cancel = true 可拦截保存，行保持编辑态)"), Category("行为")]
        public event EventHandler<RowSavingEventArgs>? RowSaving;

        /// <summary>
        /// 行保存完成后发生
        /// </summary>
        [Description("行保存完成后发生"), Category("行为")]
        public event EventHandler<RowEventArgs>? RowSaved;

        /// <summary>
        /// 行取消编辑后发生
        /// </summary>
        [Description("行取消编辑后发生"), Category("行为")]
        public event EventHandler<RowEventArgs>? RowCanceled;

        /// <summary>
        /// 行删除后发生
        /// </summary>
        [Description("行删除后发生"), Category("行为")]
        public event EventHandler<RowEventArgs>? RowDeleted;

        /// <summary>
        /// 新行添加后发生（进入编辑前）
        /// </summary>
        [Description("新行添加后发生"), Category("行为")]
        public event EventHandler<RowEventArgs>? RowAdded;

        #endregion
    }

    #region 事件参数

    /// <summary>
    /// 行事件参数
    /// </summary>
    public class RowEventArgs : EventArgs
    {
        public RowEventArgs(object record) { Record = record; }
        /// <summary>行记录</summary>
        public object Record { get; private set; }
    }

    /// <summary>
    /// 行保存事件参数
    /// </summary>
    public class RowSavingEventArgs : CancelEventArgs
    {
        public RowSavingEventArgs(object record, object? origin)
        {
            Record = record;
            Origin = origin;
        }
        /// <summary>行记录（已含编辑后的值）</summary>
        public object Record { get; private set; }
        /// <summary>进入编辑前的原始行快照</summary>
        public object? Origin { get; private set; }
    }

    #endregion
}

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
        /// <param name="sender">触发对象</param>
        /// <param name="value">数值</param>
        /// <param name="record">原始行</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        public delegate void CheckEventHandler(object sender, bool value, object? record, int rowIndex, int columnIndex);

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="args">点击</param>
        /// <param name="record">原始行</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        /// <param name="rect">表格区域</param>
        public delegate void ClickEventHandler(object sender, MouseEventArgs args, object? record, int rowIndex, int columnIndex, Rectangle rect);

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="args">点击</param>
        /// <param name="record">原始行</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        public delegate void ClickButtonEventHandler(object sender, CellLink btn, MouseEventArgs args, object? record, int rowIndex, int columnIndex);

        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category("行为")]
        public event CheckEventHandler? CheckedChanged;

        /// <summary>
        /// CheckState类型事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="column">触发表头对象</param>
        /// <param name="value">数值</param>
        public delegate void CheckStateEventHandler(object sender, ColumnCheck column, CheckState value);

        /// <summary>
        /// 全局 CheckState 属性值更改时发生
        /// </summary>
        [Description("全局 CheckState 属性值更改时发生"), Category("行为")]
        public event CheckStateEventHandler? CheckedOverallChanged = null;

        internal void OnCheckedOverallChanged(ColumnCheck column, CheckState checkState)
        {
            CheckedOverallChanged?.Invoke(this, column, checkState);
        }

        /// <summary>
        /// 单击时发生
        /// </summary>
        [Description("单击时发生"), Category("行为")]
        public event ClickEventHandler? CellClick;

        /// <summary>
        /// 单击按钮时发生
        /// </summary>
        [Description("单击按钮时发生"), Category("行为")]
        public event ClickButtonEventHandler? CellButtonClick;

        /// <summary>
        /// 双击时发生
        /// </summary>
        [Description("双击时发生"), Category("行为")]
        public event ClickEventHandler? CellDoubleClick;

        #region 编辑

        /// <summary>
        /// 编辑前事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="value">数值</param>
        /// <param name="record">原始行</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        public delegate bool BeginEditEventHandler(object sender, object? value, object? record, int rowIndex, int columnIndex);

        /// <summary>
        /// 编辑前事件文本框样式
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="value">数值</param>
        /// <param name="record">原始行</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        /// <param name="input">文本框</param>
        public delegate void BeginEditInputStyleEventHandler(object sender, object? value, object? record, int rowIndex, int columnIndex, ref Input input);

        /// <summary>
        /// 编辑后事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="value">修改后值</param>
        /// <param name="record">原始行</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        public delegate bool EndEditEventHandler(object sender, string value, object? record, int rowIndex, int columnIndex);

        /// <summary>
        /// 编辑前发生
        /// </summary>
        [Description("编辑前发生"), Category("行为")]
        public event BeginEditEventHandler? CellBeginEdit;

        /// <summary>
        /// 编辑前文本框样式发生
        /// </summary>
        [Description("编辑前文本框样式发生"), Category("行为")]
        public event BeginEditInputStyleEventHandler? CellBeginEditInputStyle;

        /// <summary>
        /// 编辑后发生
        /// </summary>
        [Description("编辑后发生"), Category("行为")]
        public event EndEditEventHandler? CellEndEdit;

        #endregion

        public delegate CellStyleInfo? SetRowStyleEventHandler(object sender, object? record, int rowIndex);
        /// <summary>
        /// 设置行样式
        /// </summary>
        public event SetRowStyleEventHandler? SetRowStyle;

        public class CellStyleInfo
        {
            /// <summary>
            /// 背景颜色
            /// </summary>
            public Color BackColor { get; set; }
        }
    }
}
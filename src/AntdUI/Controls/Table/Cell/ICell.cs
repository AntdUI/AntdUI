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
using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// 容器
    /// </summary>
    public abstract class ICell
    {
        #region 模板

        /// <summary>
        /// 获取大小
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="font">字体</param>
        /// <param name="gap">边距</param>
        /// <param name="gap2">边距2</param>
        public abstract Size GetSize(Canvas g, Font font, int gap, int gap2);

        /// <summary>
        /// 设置渲染位置坐标
        /// </summary>
        /// <param name="g"></param>
        /// <param name="font">字体</param>
        /// <param name="rect">区域</param>
        /// <param name="size">真实区域</param>
        /// <param name="maxwidth">最大宽度</param>
        /// <param name="gap">边距</param>
        /// <param name="gap2">边距2</param>
        public abstract void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, int gap, int gap2);

        public abstract void PaintBack(Canvas g);
        public abstract void Paint(Canvas g, Font font, bool enable, SolidBrush fore);

        Table.CELL? _PARENT;
        /// <summary>
        /// 模板父级
        /// </summary>
        public Table.CELL PARENT
        {
            get
            {
                if (_PARENT == null) throw new ArgumentNullException();
                return _PARENT;
            }
        }

        internal void SetCELL(Table.CELL row) => _PARENT = row;

        #endregion

        public Rectangle Rect { get; set; }

        #region 下拉

        /// <summary>
        /// 菜单弹出位置
        /// </summary>
        public TAlignFrom DropDownPlacement { get; set; } = TAlignFrom.BL;

        /// <summary>
        /// 列表最多显示条数
        /// </summary>
        public int DropDownMaxCount { get; set; } = 4;

        /// <summary>
        /// 下拉圆角
        /// </summary>
        public int? DropDownRadius { get; set; }

        /// <summary>
        /// 下拉箭头是否显示
        /// </summary>
        public bool DropDownArrow { get; set; }

        /// <summary>
        /// 下拉边距
        /// </summary>
        public Size DropDownPadding { get; set; } = new Size(12, 5);

        /// <summary>
        /// 下拉文本方向
        /// </summary>
        public TAlign DropDownTextAlign { get; set; } = TAlign.Left;

        /// <summary>
        /// 点击到最里层（无节点才能点击）
        /// </summary>
        public bool DropDownClickEnd { get; set; }

        /// <summary>
        /// 点击切换下拉
        /// </summary>
        public bool DropDownClickSwitchDropdown { get; set; } = true;

        #region 数据

        /// <summary>
        /// 数据
        /// </summary>
        public IList<object>? DropDownItems { get; set; }

        /// <summary>
        /// 选中值
        /// </summary>
        public object? DropDownValue { get; set; }

        /// <summary>
        /// DropDownValue 属性值更改时发生
        /// </summary>
        public Action<object>? DropDownValueChanged;

        #endregion

        #endregion

        #region 属性

        public bool ImpactHeight { get; set; } = true;

        public ICell SetImpactHeight(bool value = false)
        {
            ImpactHeight = value;
            return this;
        }

        #endregion

        public Action<bool>? Changed { get; set; }
        public void OnPropertyChanged(bool layout = false) => Changed?.Invoke(layout);
    }
}
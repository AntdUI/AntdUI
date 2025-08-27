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

using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// 标签
    /// </summary>
    /// <seealso cref="ICell"/>
    public partial class CellTag : ICell
    {
        /// <summary>
        /// 标签
        /// </summary>
        /// <param name="text">文本</param>
        public CellTag(string text) { _text = text; }

        /// <summary>
        /// 标签
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="type">类型</param>
        public CellTag(string text, TTypeMini type)
        {
            _text = text;
            _type = type;
        }

        /// <summary>
        /// 标签
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="type">类型</param>
        /// <param name="gap">间隔</param>
        public CellTag(string text, TTypeMini type, int gap)
        {
            _text = text;
            _type = type;
            Gap = gap;
        }

        #region 属性

        Color? fore;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                OnPropertyChanged();
            }
        }

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                OnPropertyChanged();
            }
        }

        float borderWidth = 1F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                OnPropertyChanged();
            }
        }

        TTypeMini _type = TTypeMini.Default;
        /// <summary>
        /// 类型
        /// </summary>
        public TTypeMini Type
        {
            get => _type;
            set
            {
                if (_type == value) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        string _text;
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged(true);
            }
        }

        /// <summary>
        /// 间距
        /// </summary>
        public int? Gap { get; set; }

        #endregion

        #region 设置

        public CellTag SetFore(Color? value)
        {
            fore = value;
            return this;
        }
        public CellTag SetBack(Color? value)
        {
            back = value;
            return this;
        }
        public CellTag SetBorderWidth(float value = 0F)
        {
            borderWidth = value;
            return this;
        }
        public CellTag SetType(TTypeMini value = TTypeMini.Success)
        {
            _type = value;
            return this;
        }
        public CellTag SetGap(int? value)
        {
            Gap = value;
            return this;
        }

        #endregion

        public override string ToString() => _text;
    }
}
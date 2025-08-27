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
    /// 进度条
    /// </summary>
    /// <seealso cref="ICell"/>
    public partial class CellProgress : ICell
    {
        /// <summary>
        /// 进度条
        /// </summary>
        /// <param name="value">进度</param>
        public CellProgress(float value)
        {
            _value = value;
        }

        #region 属性

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

        Color? fill;
        /// <summary>
        /// 进度条颜色
        /// </summary>
        public Color? Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                OnPropertyChanged();
            }
        }

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                OnPropertyChanged();
            }
        }

        TShape shape = TShape.Round;
        /// <summary>
        /// 形状
        /// </summary>
        public TShape Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
                OnPropertyChanged(true);
            }
        }

        float _value = 0F;
        /// <summary>
        /// 进度条
        /// </summary>
        public float Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                _value = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 大小
        /// </summary>
        public Size? Size { get; set; }

        #endregion

        #region 设置

        public CellProgress SetBack(Color? value)
        {
            back = value;
            return this;
        }
        public CellProgress SetFill(Color? value)
        {
            fill = value;
            return this;
        }
        public CellProgress SetRadius(int value = 0)
        {
            radius = value;
            return this;
        }
        public CellProgress SetShape(TShape value = TShape.Default)
        {
            shape = value;
            return this;
        }
        public CellProgress SetValue(float value = 1F)
        {
            if (value < 0) value = 0;
            else if (value > 1) value = 1;
            _value = value;
            return this;
        }
        public CellProgress SetSize(Size? value)
        {
            Size = value;
            return this;
        }
        public CellProgress SetSize(int width, int height)
        {
            Size = new Size(width, height);
            return this;
        }
        public CellProgress SetSize(int size)
        {
            Size = new Size(size, size);
            return this;
        }

        #endregion

        public override string ToString() => (_value * 100F) + "%";
    }
}
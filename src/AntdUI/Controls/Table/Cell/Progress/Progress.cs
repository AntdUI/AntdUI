// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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
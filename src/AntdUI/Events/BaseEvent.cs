// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    #region Int

    public class IntEventArgs : VEventArgs<int>
    {
        public IntEventArgs(int value) : base(value) { }
    }

    /// <summary>
    /// Int 类型事件
    /// </summary>
    public delegate void IntEventHandler(object sender, IntEventArgs e);

    /// <summary>
    /// Int 类型事件
    /// </summary>
    public delegate bool IntBoolEventHandler(object sender, IntEventArgs e);

    #endregion

    #region Float

    public class FloatEventArgs : VEventArgs<float>
    {
        public FloatEventArgs(float value) : base(value) { }
    }

    /// <summary>
    /// Float 类型事件
    /// </summary>
    public delegate void FloatEventHandler(object sender, FloatEventArgs e);

    #endregion

    #region Decimal

    public class DecimalEventArgs : VEventArgs<decimal>
    {
        public DecimalEventArgs(decimal value) : base(value) { }
    }

    /// <summary>
    /// Decimal 类型事件
    /// </summary>
    public delegate void DecimalEventHandler(object sender, DecimalEventArgs e);

    #endregion

    #region Object

    public class ObjectNEventArgs : VEventArgs<object?>
    {
        public ObjectNEventArgs(object? value) : base(value) { }
    }

    /// <summary>
    /// Object类型事件
    /// </summary>
    public delegate void ObjectNEventHandler(object sender, ObjectNEventArgs e);

    public class ObjectsEventArgs : VEventArgs<object[]>
    {
        public ObjectsEventArgs(object[] value) : base(value) { }
    }

    /// <summary>
    /// Object类型事件
    /// </summary>
    public delegate void ObjectsEventHandler(object sender, ObjectsEventArgs e);

    #endregion

    #region Bool

    public class BoolEventArgs : VEventArgs<bool>
    {
        public BoolEventArgs(bool value) : base(value) { }
    }

    /// <summary>
    /// Bool 类型事件
    /// </summary>
    public delegate void BoolEventHandler(object sender, BoolEventArgs e);

    #endregion

    #region Color

    public class ColorEventArgs : VEventArgs<Color>
    {
        public ColorEventArgs(Color value) : base(value) { }
    }

    /// <summary>
    /// Color 类型事件
    /// </summary>
    public delegate void ColorEventHandler(object sender, ColorEventArgs e);

    #endregion

    #region DateTime

    public class DateTimeEventArgs : VEventArgs<DateTime>
    {
        public DateTimeEventArgs(DateTime value) : base(value) { }
    }

    /// <summary>
    /// DateTime 类型事件
    /// </summary>
    public delegate void DateTimeEventHandler(object sender, DateTimeEventArgs e);

    public class DateTimeNEventArgs : VEventArgs<DateTime?>
    {
        public DateTimeNEventArgs(DateTime? value) : base(value) { }
    }

    /// <summary>
    /// DateTime 类型事件
    /// </summary>
    public delegate void DateTimeNEventHandler(object sender, DateTimeNEventArgs e);

    public class TimeSpanNEventArgs : VEventArgs<TimeSpan>
    {
        public TimeSpanNEventArgs(TimeSpan value) : base(value) { }
    }

    /// <summary>
    /// TimeSpan 类型事件
    /// </summary>
    public delegate void TimeSpanNEventHandler(object sender, TimeSpanNEventArgs e);

    #endregion

    #region DateTime[]

    public class DateTimesEventArgs : VEventArgs<DateTime[]?>
    {
        public DateTimesEventArgs(DateTime[]? value) : base(value) { }
    }


    public delegate void DateTimesEventHandler(object sender, DateTimesEventArgs e);

    #endregion

    #region 渲染

    public class DrawEventArgs : EventArgs
    {
        public DrawEventArgs(Canvas canvas, Rectangle rect)
        {
            Canvas = canvas;
            Rect = rect;
        }

        public Canvas Canvas { get; private set; }
        public Rectangle Rect { get; private set; }

        public Graphics? Graphics
        {
            get
            {
                if (Canvas is Core.CanvasGDI gdi) return gdi.g;
                return null;
            }
        }
    }

    public delegate void DrawEventHandler(object sender, DrawEventArgs e);

    #endregion

    #region 基础

    public class StringsEventArgs : VEventArgs<string[]>
    {
        public StringsEventArgs(string[] value) : base(value) { }
    }

    public class VEventArgs<T> : EventArgs
    {
        public VEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public object? Tag { get; set; }
    }


    public class VMEventArgs<T> : MouseEventArgs
    {
        public VMEventArgs(T item, MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Item = item;
        }
        public VMEventArgs(T item, MouseEventArgs e, int click) : base(e.Button, click, e.X, e.Y, e.Delta)
        {
            Item = item;
        }

        public T Item { get; private set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public object? Tag { get; set; }
    }

    #endregion
}
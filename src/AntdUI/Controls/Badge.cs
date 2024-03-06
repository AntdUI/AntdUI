﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
    /// <summary>
    /// Badge 徽标数
    /// </summary>
    /// <remarks>图标右上角的圆形徽标数字。</remarks>
    [Description("Badge 徽标数")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    public class Badge : IControl
    {
        #region 属性

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
            }
        }

        TState state = TState.Default;
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态"), Category("外观"), DefaultValue(TState.Default)]
        public TState State
        {
            get => state;
            set
            {
                if (state == value) return;
                state = value;
                StartAnimation();
                Invalidate();
            }
        }

        bool has_text = true;
        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public new string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                has_text = string.IsNullOrEmpty(text);
                Invalidate();
            }
        }

        Color? fill = null;
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观"), DefaultValue(null)]
        public Color? Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                Invalidate();
            }
        }

        #region 动画

        void StartAnimation()
        {
            StopAnimation();
            if (Config.Animation && state == TState.Processing)
            {
                ThreadState = new ITask(this, i =>
                {
                    AnimationStateValue = i;
                    Invalidate();
                }, 50, 1F, 0.05F);
            }
        }
        void StopAnimation()
        {
            ThreadState?.Dispose();
        }
        protected override void Dispose(bool disposing)
        {
            StopAnimation();
            base.Dispose(disposing);
        }
        ITask? ThreadState = null;
        float AnimationStateValue = 0;

        #endregion

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle.PaddingRect(Padding);
            var g = e.Graphics.High();
            if (has_text)
            {
                var size = g.MeasureString(Config.NullText, Font);
                float dot_size = size.Height / 2.5F;
                using (var brush = new SolidBrush(GetColor(fill, state)))
                {
                    g.FillEllipse(brush, new RectangleF((rect.Width - dot_size) / 2F, (rect.Height - dot_size) / 2F, dot_size, dot_size));
                    if (state == TState.Processing)
                    {
                        float max = (size.Height - 6F) * AnimationStateValue;
                        int alpha = (int)(255 * (1f - AnimationStateValue));
                        using (var pen = new Pen(Color.FromArgb(alpha, brush.Color), 4F))
                        {
                            g.DrawEllipse(pen, new RectangleF((rect.Width - max) / 2F, (rect.Height - max) / 2F, max, max));
                        }
                    }
                }
            }
            else
            {
                var size = g.MeasureString(text, Font);
                float dot_size = size.Height / 2.5F;
                using (var brush = new SolidBrush(GetColor(fill, state)))
                {
                    var rect_dot = new RectangleF(rect.X + (size.Height - dot_size) / 2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
                    g.FillEllipse(brush, rect_dot);
                    if (state == TState.Processing)
                    {
                        float max = (size.Height - 6F) * AnimationStateValue;
                        int alpha = (int)(255 * (1f - AnimationStateValue));
                        using (var pen = new Pen(Color.FromArgb(alpha, brush.Color), 4F))
                        {
                            g.DrawEllipse(pen, new RectangleF(rect_dot.X + (rect_dot.Width - max) / 2F, rect_dot.Y + (rect_dot.Height - max) / 2F, max, max));
                        }
                    }
                }
                using (var brush = fore.Brush(Style.Db.Text, Style.Db.TextQuaternary, Enabled))
                {
                    g.DrawString(text, Font, brush, new RectangleF(rect.X + size.Height, rect.Y, rect.Width - size.Height, rect.Height), Helper.stringFormatLeft);
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        internal Color GetColor(Color? color, TState state)
        {
            if (color.HasValue) return color.Value;
            return GetColor(state);
        }
        internal Color GetColor(TState state)
        {
            switch (state)
            {
                case TState.Success: return Style.Db.Success;
                case TState.Error: return Style.Db.Error;
                case TState.Primary:
                case TState.Processing: return Style.Db.Primary;
                case TState.Warn: return Style.Db.Warning;
                default: return Style.Db.TextQuaternary;
            }
        }

        #endregion

        #endregion
    }
}
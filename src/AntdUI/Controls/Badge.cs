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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
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
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) fore = value;
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
        public override string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                has_text = string.IsNullOrEmpty(text);
                if (BeforeAutoSize()) Invalidate();
                OnTextChanged(EventArgs.Empty);
            }
        }

        StringFormat stringFormat = Helper.SF_ALL(lr: StringAlignment.Near);
        ContentAlignment textAlign = ContentAlignment.MiddleLeft;
        /// <summary>
        /// 文本位置
        /// </summary>
        [Description("文本位置"), Category("外观"), DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                if (textAlign == value) return;
                textAlign = value;
                textAlign.SetAlignment(ref stringFormat);
                Invalidate();
            }
        }

        Color? fill = null;
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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
                int dot_size = (int)(size.Height / 2.5F);
                using (var brush = new SolidBrush(GetColor(fill, state)))
                {
                    g.FillEllipse(brush, new RectangleF((rect.Width - dot_size) / 2F, (rect.Height - dot_size) / 2F, dot_size, dot_size));
                    if (state == TState.Processing)
                    {
                        float max = (size.Height - 6F) * AnimationStateValue, alpha = 255 * (1F - AnimationStateValue);
                        g.DrawEllipse(Helper.ToColor(alpha, brush.Color), 4F * Config.Dpi, new RectangleF((rect.Width - max) / 2F, (rect.Height - max) / 2F, max, max));
                    }
                }
            }
            else
            {
                var size = g.MeasureString(text, Font);
                int dot_size = (int)(size.Height / 2.5F);
                using (var brush = new SolidBrush(GetColor(fill, state)))
                {
                    var rect_dot = new RectangleF(rect.X + (size.Height - dot_size) / 2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
                    g.FillEllipse(brush, rect_dot);
                    if (state == TState.Processing)
                    {
                        float max = (size.Height - 6F) * AnimationStateValue, alpha = 255 * (1F - AnimationStateValue);
                        g.DrawEllipse(Helper.ToColor(alpha, brush.Color), 4F * Config.Dpi, new RectangleF(rect_dot.X + (rect_dot.Width - max) / 2F, rect_dot.Y + (rect_dot.Height - max) / 2F, max, max));
                    }
                }
                using (var brush = fore.Brush(Style.Db.Text, Style.Db.TextQuaternary, Enabled))
                {
                    g.String(text, Font, brush, new Rectangle(rect.X + size.Height, rect.Y, rect.Width - size.Height, rect.Height), stringFormat);
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

        #region 自动大小

        /// <summary>
        /// 自动大小
        /// </summary>
        [Browsable(true)]
        [Description("自动大小"), Category("外观"), DefaultValue(false)]
        public override bool AutoSize
        {
            get => base.AutoSize;
            set
            {
                if (base.AutoSize == value) return;
                base.AutoSize = value;
                if (value)
                {
                    if (autoSize == TAutoSize.None) autoSize = TAutoSize.Auto;
                }
                else autoSize = TAutoSize.None;
                BeforeAutoSize();
            }
        }

        TAutoSize autoSize = TAutoSize.None;
        /// <summary>
        /// 自动大小模式
        /// </summary>
        [Description("自动大小模式"), Category("外观"), DefaultValue(TAutoSize.None)]
        public TAutoSize AutoSizeMode
        {
            get => autoSize;
            set
            {
                if (autoSize == value) return;
                autoSize = value;
                base.AutoSize = autoSize != TAutoSize.None;
                BeforeAutoSize();
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            BeforeAutoSize();
            base.OnFontChanged(e);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (autoSize == TAutoSize.None) return base.GetPreferredSize(proposedSize);
            else if (autoSize == TAutoSize.Width) return new Size(PSize.Width, base.GetPreferredSize(proposedSize).Height);
            else if (autoSize == TAutoSize.Height) return new Size(base.GetPreferredSize(proposedSize).Width, PSize.Height);
            return PSize;
        }

        internal Size PSize
        {
            get
            {
                return Helper.GDI(g =>
                {
                    if (has_text)
                    {
                        var font_size = g.MeasureString(Config.NullText, Font);
                        font_size.Width = font_size.Height;
                        return font_size;
                    }
                    else
                    {
                        var font_size = g.MeasureString(text ?? Config.NullText, Font);
                        font_size.Width += font_size.Height;
                        return font_size;
                    }
                });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            BeforeAutoSize();
            base.OnResize(e);
        }

        internal bool BeforeAutoSize()
        {
            if (autoSize == TAutoSize.None) return true;
            if (InvokeRequired)
            {
                bool flag = false;
                Invoke(new Action(() =>
                {
                    flag = BeforeAutoSize();
                }));
                return flag;
            }
            var PS = PSize;
            switch (autoSize)
            {
                case TAutoSize.Width:
                    if (Width == PS.Width) return true;
                    Width = PS.Width;
                    break;
                case TAutoSize.Height:
                    if (Height == PS.Height) return true;
                    Height = PS.Height;
                    break;
                case TAutoSize.Auto:
                default:
                    if (Width == PS.Width && Height == PS.Height) return true;
                    Size = PS;
                    break;
            }
            return false;
        }

        #endregion
    }
}
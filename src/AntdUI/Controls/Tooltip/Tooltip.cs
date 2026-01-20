// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace AntdUI
{
    /// <summary>
    /// Tooltip 文字提示
    /// </summary>
    /// <remarks>简单的文字提示气泡框。</remarks>
    [Description("Tooltip 文字提示")]
    [ToolboxItem(true)]
    public partial class Tooltip : IControl, ITooltip
    {
        #region 参数

        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public override string? Text
        {
#pragma warning disable CS8764, CS8766
            get => this.GetLangI(LocalizationText, text);
#pragma warning restore CS8764, CS8766
            set
            {
                if (text == value) return;
                text = value;
                Invalidate();
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 箭头大小
        /// </summary>
        [Description("箭头大小"), Category("外观"), DefaultValue(null)]
        public int? ArrowSize { get; set; }

        /// <summary>
        /// 箭头方向
        /// </summary>
        [Description("箭头方向"), Category("外观"), DefaultValue(TAlign.Top)]
        public TAlign ArrowAlign { get; set; } = TAlign.Top;

        /// <summary>
        /// 自定义宽度
        /// </summary>
        [Description("自定义宽度"), Category("外观"), DefaultValue(null)]
        public int? CustomWidth { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        [Description("背景色"), Category("外观"), DefaultValue(null)]
        public Color? Back { get; set; }
        /// <summary>
        /// 前景色
        /// </summary>
        [Description("前景色"), Category("外观"), DefaultValue(null)]
        public Color? Fore { get; set; }

        #endregion

        #region 渲染

        int arrowSize = 0;
        readonly FormatFlags s_c = FormatFlags.Center | FormatFlags.NoWrap, s_l = FormatFlags.Left | FormatFlags.VerticalCenter;
        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            MaximumSize = MinimumSize = this.RenderMeasure(g, null, out var multiline, out _, out arrowSize);
            this.Render(g, e.Rect, multiline, arrowSize, -1, s_c, s_l);
            base.OnDraw(e);
        }

        #endregion
    }
}
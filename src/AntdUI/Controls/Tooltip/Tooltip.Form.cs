// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    internal class TooltipForm : ILayeredFormOpacity, ITooltip
    {
        Control ocontrol;
        bool multiline = false;
        int? maxWidth;
        int arrowSize = 0, arrowX = -1;
        // 新增：记录上一次用于定位的 rect（用于判断是否需要更新位置）
        Rectangle? _lastRect;
        public TooltipForm(Control control, string txt, ITooltipConfig component) : base(240)
        {
            PARENT = control;
            ocontrol = control;
            SetTopMost(control.Parent, Handle);
            SetDpi(control);
            CloseMode = CloseMode.Leave;
            Text = txt;
            Font = component.Font ?? Config.Font ?? control.Font;
            ArrowSize = component.ArrowSize;
            Radius = component.Radius;
            ArrowAlign = component.ArrowAlign;
            CustomWidth = component.CustomWidth;
            Back = component.Back;
            Fore = component.Fore;
            var screen = Screen.FromControl(control).WorkingArea;
            maxWidth = screen.Width;
            int gap = 0;
            this.GDI(g => SetSize(this.RenderMeasure(g, maxWidth, out multiline, out gap, out arrowSize)));
            if (component is Tooltip.Config config && config.Offset.HasValue)
            {
                _lastRect = config.Offset.Value;
                var align = ArrowAlign;
                new CalculateCoordinate(this, control, TargetRect, Radius, arrowSize, gap, gap * 2, config.Offset.Value).SetScreen(screen).Auto(ref align, gap + (int)(Radius * Dpi), out int x, out int y, out arrowX);
                ArrowAlign = align;
                SetLocation(x, y);
            }
            else
            {
                _lastRect = null;
                var align = ArrowAlign;
                new CalculateCoordinate(this, control, TargetRect, Radius, arrowSize, gap, gap * 2).Auto(ref align, gap + (int)(Radius * Dpi), out int x, out int y, out arrowX);
                ArrowAlign = align;
                SetLocation(x, y);
            }
        }
        public TooltipForm(Control control, Rectangle rect, string txt, ITooltipConfig component, bool hasmax = false) : base(240)
        {
            PARENT = control;
            ocontrol = control;
            SetTopMost(control, Handle);
            SetDpi(control);
            CloseMode = CloseMode.Click;
            Text = txt;
            Font = component.Font ?? Config.Font ?? control.Font;
            ArrowSize = component.ArrowSize;
            Radius = component.Radius;
            ArrowAlign = component.ArrowAlign;
            CustomWidth = component.CustomWidth;
            Back = component.Back;
            Fore = component.Fore;
            _lastRect = rect;
            var screen = Screen.FromControl(control).WorkingArea;
            maxWidth = hasmax ? control.Width : screen.Width;
            int gap = 0;
            this.GDI(g => SetSize(this.RenderMeasure(g, maxWidth, out multiline, out gap, out arrowSize)));
            var align = ArrowAlign;
            new CalculateCoordinate(this, control, TargetRect, Radius, arrowSize, gap, gap * 2, rect).SetScreen(screen).Auto(ref align, gap + (int)(Radius * Dpi), out int x, out int y, out arrowX);
            ArrowAlign = align;
            SetLocation(x, y);
        }
        public TooltipForm NoMessage()
        {
            CloseMode = CloseMode.None;
            return this;
        }

        public override string name => nameof(Tooltip);

        public bool SetText(Rectangle rect, string text)
        {
            // 关键修复：文本相同但 rect 变化时，也要更新位置
            bool sameText = Text == text, sameRect = _lastRect.HasValue && _lastRect.Value == rect;
            if (sameText && sameRect) return false;
            _lastRect = rect;
            Text = text;
            int gap = 0;
            this.GDI(g => SetSize(this.RenderMeasure(g, maxWidth, out multiline, out gap, out arrowSize)));
            var align = ArrowAlign;
            new CalculateCoordinate(this, ocontrol, TargetRect, Radius, arrowSize, gap, gap * 2, rect).Auto(ref align, gap + (int)(Radius * Dpi), out int x, out int y, out arrowX);
            ArrowAlign = align;
            SetLocation(x, y);
            if (Print() == RenderResult.OK) return false;
            else return true;
        }

        #region 参数

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius { get; set; } = 6;

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

        readonly FormatFlags s_c = FormatFlags.Center | FormatFlags.NoWrap, s_l = FormatFlags.Left | FormatFlags.VerticalCenter;
        public override Bitmap? PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap rbmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(rbmp).High(Dpi))
            {
                this.Render(g, rect, multiline, arrowSize, arrowX, s_c, s_l);
            }
            return rbmp;
        }

        #endregion
    }
}
// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    [ProvideProperty("Tip", typeof(Control)), Description("提示")]
    public partial class TooltipComponent : Component, IExtenderProvider, ITooltipConfig
    {
        public bool CanExtend(object target) => target is Control;

        #region 属性

        /// <summary>
        /// 字体
        /// </summary>
        [Description("字体"), DefaultValue(null)]
        public Font? Font { get; set; }

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

        /// <summary>
        /// 延迟时间（毫秒）
        /// </summary>
        [Description("延迟时间（毫秒）"), Category("行为"), DefaultValue(500)]
        public int Delay { get; set; } = 500;

        #endregion

        ConcurrentDictionary<Control, string> dic = new ConcurrentDictionary<Control, string>();

        [Description("设置是否提示"), DefaultValue(null)]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        [Localizable(true)]
        public string? GetTip(Control item)
        {
            if (dic.TryGetValue(item, out var value)) return value;
            return null;
        }
        public void SetTip(Control control, string? val)
        {
            if (val == null)
            {
                if (dic.TryRemove(control, out _))
                {
                    control.MouseEnter -= Control_Enter;
                    control.MouseLeave -= Control_Leave;
                    control.Leave -= Control_Leave;
                }
                return;
            }
            if (dic.TryGetValue(control, out var tmp))
            {
                var valn = val.Trim();
                dic.AddOrUpdate(control, valn, (a, b) => valn);
            }
            else
            {
                if (dic.TryAdd(control, val.Trim()))
                {
                    control.MouseEnter += Control_Enter;
                    control.MouseLeave += Control_Leave;
                    control.Leave -= Control_Leave;
                }
            }
        }

        #region 核心显示

        List<Control> dic_in = new List<Control>();
        private void Control_Leave(object? sender, EventArgs e)
        {
            if (sender is Control obj)
            {
                lock (dic_in) dic_in.Remove(obj);
            }
        }

        private void Control_Enter(object? sender, EventArgs e)
        {
            if (sender is Control control)
            {
                var parent = control.FindPARENT();
                lock (dic_in) dic_in.Add(control);
                if (parent == null) OpenTip(control);
                else OpenTip(parent, control);
            }
        }

        static ConcurrentDictionary<Form, TooltipForm> cache = new ConcurrentDictionary<Form, TooltipForm>();
        void OpenTip(Form form, Control control)
        {
            if (cache.TryRemove(form, out var tmp)) tmp.IClose();
            ITask.Run(() =>
            {
                Thread.Sleep(Delay);
                if (dic_in.Contains(control) && dic.TryGetValue(control, out var str))
                {
                    control.BeginInvoke(new Action(() =>
                    {
                        var tooltip = new TooltipForm(control, str, this);
                        tooltip.Show();
                        tooltip.Disposed += (a, b) => cache.TryRemove(form, out _);
                        cache.TryAdd(form, tooltip);
                    }));
                }
            });
        }

        void OpenTip(Control control)
        {
            ITask.Run(() =>
            {
                Thread.Sleep(Delay);
                if (dic_in.Contains(control) && dic.TryGetValue(control, out var str)) control.BeginInvoke(new Action(() => new TooltipForm(control, str, this).Show()));
            });
        }

        #endregion
    }
}
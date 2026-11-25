// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Helper
    {
        #region 统一会话入口（根源修复）

        /// <summary>
        /// 创建蒙版并把它与业务窗体一起纳入同一个多层级会话。
        /// <para>无论用户是走 Modal.open，还是直接调用 ShowDialogMask / FormMask，
        /// 都会自动获得统一的 Z 序重排、前台焦点托管与动画世代号闸门，
        /// 不再依赖各浮层内部的 owner.Activate() 补救。</para>
        /// </summary>
        static LayeredFormMask FormMaskCore(Form owner, Control? control, Form form, bool maskClosable, Action<LayeredFormMask> show)
        {
            var mask = control == null ? new LayeredFormMask(owner) : new LayeredFormMask(owner, control);

            // 与 Helper.SetTopMost 的判定保持一致：父窗体本身 TopMost，或父浮层已被置顶。
            // 整栈（Mask + 业务窗体）使用同一个组，避免内容窗体被自己的蒙版/面板盖住。
            bool topMost = owner.TopMost || (owner is ILayeredForm layeredOwner && layeredOwner.topMost);
            var session = LayeredWindowSession.Create(owner, topMost);
            mask.Session = session;
            LayeredWindowSession.BindHost(form, session);

            if (maskClosable) mask.SetForm(form);
            form.FormClosed += (s1, e1) => mask.IClose();

            // 注册顺序即 Z 序：Mask 在底，Content 在顶；句柄未创建时自动延迟注册。
            session.Attach(mask, LayeredRole.Mask);
            session.Attach(form, LayeredRole.Content);

            show(mask);
            return mask;
        }

        /// <summary>
        /// 模态显示并在收口时统一进入关闭流程，最后由会话在所有浮层销毁后
        /// 做一次条件式焦点归还（用户主动切到别的软件时不抢焦点）。
        /// </summary>
        static DialogResult ShowDialogCore(ILayeredFormOpacity mask, Form form)
        {
            var session = mask.Session;
            try
            {
                return form.ShowDialog(mask);
            }
            finally
            {
                // 先采样前台（用于区分"用户主动切走"与"焦点被动丢失"），再让 Mask 淡出
                session?.BeginClose();
                mask.IClose();
            }
        }

        #endregion

        /// <summary>
        /// ShowDialog 叠加蒙版
        /// </summary>
        /// <param name="owner">父窗口</param>
        /// <param name="form">操作对象</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        public static DialogResult ShowDialogMask(this Form owner, Form form, out ILayeredFormOpacity mask, bool MaskClosable = false)
        {
            mask = owner.FormMask(form, MaskClosable);
            return ShowDialogCore(mask, form);
        }

        /// <summary>
        /// 叠加蒙版
        /// </summary>
        /// <param name="owner">父控件</param>
        /// <param name="form">操作对象</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        public static DialogResult ShowDialogMask(this Control owner, Form form, out ILayeredFormOpacity mask, bool MaskClosable = false)
        {
            mask = owner.FormMask(form, MaskClosable);
            return ShowDialogCore(mask, form);
        }

        /// <summary>
        /// ShowDialog 叠加蒙版
        /// </summary>
        /// <param name="owner">父窗口</param>
        /// <param name="form">操作对象</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        public static DialogResult ShowDialogMask(this Form owner, Form form, bool MaskClosable = false)
        {
            ILayeredFormOpacity mask;
            return ShowDialogMask(owner, form, out mask, MaskClosable);
        }

        /// <summary>
        /// 叠加蒙版
        /// </summary>
        /// <param name="owner">父控件</param>
        /// <param name="form">操作对象</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        public static DialogResult ShowDialogMask(this Control owner, Form form, bool MaskClosable = false)
        {
            ILayeredFormOpacity mask;
            return ShowDialogMask(owner, form, out mask, MaskClosable);
        }

        /// <summary>
        /// 叠加蒙版
        /// </summary>
        /// <param name="owner">父窗口</param>
        /// <param name="form">操作对象</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        public static ILayeredFormOpacity FormMask(this Form owner, Form form, bool MaskClosable = false)
            => FormMaskCore(owner, null, form, MaskClosable, mask => mask.Show(owner));

        /// <summary>
        /// 叠加蒙版
        /// </summary>
        /// <param name="owner">父控件</param>
        /// <param name="form">操作对象</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        public static ILayeredFormOpacity FormMask(this Control owner, Form form, bool MaskClosable = false)
        {
            var tmp = owner.FindPARENT();
            if (tmp == null) throw new System.Exception("无法找到父窗口");
            return FormMaskCore(tmp, owner, form, MaskClosable, mask => mask.Show(owner));
        }

        /// <summary>
        /// 叠加蒙版
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="form">操作对象</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        public static ILayeredFormOpacity FormMask(this Target target, Form form, bool MaskClosable = false)
        {
            Form owner;
            Control? control;
            if (target.Value is Form tmpForm) { owner = tmpForm; control = null; }
            else if (target.Value is Control tmpControl)
            {
                var tmp = tmpControl.FindPARENT();
                if (tmp == null) throw new System.Exception("无法找到父窗口");
                owner = tmp;
                control = tmpControl;
            }
            else throw new System.Exception("Target只能是Form或Control");
            return FormMaskCore(owner, control, form, MaskClosable, mask => target.Show(mask));
        }

        /// <summary>
        /// 叠加蒙版
        /// </summary>
        /// <param name="owner">父窗口</param>
        /// <param name="form">操作对象</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        public static ILayeredFormOpacity FormMask(this Form owner, ILayeredForm form, bool MaskClosable = false) => FormMaskCore(owner, null, form, MaskClosable, mask => mask.Show(owner));

        public static bool FormFrame(this Form form, out int Radius, out int Padd)
        {
            Padd = Radius = 0;
            if (form.WindowState != FormWindowState.Maximized)
            {
                if (form is BorderlessForm borderless)
                {
                    if (borderless.UseDwm)
                    {
                        if (OS.Win11) Radius = (int)System.Math.Round(8 * borderless.Dpi); //Win11
                        return false;
                    }
                    else Radius = (int)(borderless.Radius * borderless.Dpi);
                    return false;
                }
                else if (form is BaseForm baseForm)
                {
                    if (form.FormBorderStyle == FormBorderStyle.None) return false;
                    if (OS.Win11) Radius = (int)System.Math.Round(8 * baseForm.Dpi); //Win11
                    if (form is Window || form is FormNoBar) return false;//无边框处理
                    var rect = new Win32.RECT();
                    Win32.User32.AdjustWindowRectEx(ref rect, Win32.User32.WindowStyles.WS_OVERLAPPEDWINDOW | Win32.User32.WindowStyles.WS_CLIPCHILDREN, false, Win32.User32.WindowStylesEx.WS_EX_CONTROLPARENT | Win32.User32.WindowStylesEx.WS_EX_APPWINDOW);
                    Padd = rect.bottom;
                    return true;
                }
                else
                {
                    if (form.FormBorderStyle == FormBorderStyle.None) return false;
                    if (OS.Win11) Radius = (int)System.Math.Round(8 * Config.Dpi); //Win11
                    if (form is Window || form is FormNoBar) return false;//无边框处理
                    var rect = new Win32.RECT();
                    Win32.User32.AdjustWindowRectEx(ref rect, Win32.User32.WindowStyles.WS_OVERLAPPEDWINDOW | Win32.User32.WindowStyles.WS_CLIPCHILDREN, false, Win32.User32.WindowStylesEx.WS_EX_CONTROLPARENT | Win32.User32.WindowStylesEx.WS_EX_APPWINDOW);
                    Padd = rect.bottom;
                    return true;
                }
            }
            return false;
        }
    }
}
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

using System.Windows.Forms;

namespace AntdUI
{
    partial class Helper
    {
        /// <summary>
        /// 叠加蒙版
        /// </summary>
        /// <param name="owner">父窗口</param>
        /// <param name="form">操作对象</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        public static ILayeredFormOpacity FormMask(this Form owner, Form form, bool MaskClosable = false)
        {
            var mask = new LayeredFormMask(owner);
            if (MaskClosable) mask.SetForm(form);
            form.FormClosed += (s1, e1) => mask.IClose();
            mask.Show(owner);
            return mask;
        }

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
            var mask = new LayeredFormMask(tmp, owner);
            if (MaskClosable) mask.SetForm(form);
            form.FormClosed += (s1, e1) => mask.IClose();
            mask.Show(owner);
            return mask;
        }

        /// <summary>
        /// 叠加蒙版
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="form">操作对象</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        public static ILayeredFormOpacity FormMask(this Target target, Form form, bool MaskClosable = false)
        {
            LayeredFormMask mask;
            if (target.Value is Form owner) mask = new LayeredFormMask(owner);
            else if (target.Value is Control control)
            {
                var tmp = control.FindPARENT();
                if (tmp == null) throw new System.Exception("无法找到父窗口");
                mask = new LayeredFormMask(tmp, control);
            }
            else throw new System.Exception("Target只能是Form或Control");
            if (MaskClosable) mask.SetForm(form);
            form.FormClosed += (s1, e1) => mask.IClose();
            target.Show(mask);
            return mask;
        }

        /// <summary>
        /// 叠加蒙版
        /// </summary>
        /// <param name="owner">父窗口</param>
        /// <param name="form">操作对象</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        public static ILayeredFormOpacity FormMask(this Form owner, ILayeredForm form, bool MaskClosable = false)
        {
            var mask = new LayeredFormMask(owner);
            if (MaskClosable) mask.SetForm(form);
            mask.Show(owner);
            form.FormClosed += (s1, e1) => mask.IClose();
            return mask;
        }

        public static bool FormFrame(this Form form, out int Radius, out int Padd)
        {
            Padd = Radius = 0;
            if (form.WindowState != FormWindowState.Maximized)
            {
                if (form is BorderlessForm borderless)
                {
                    if (borderless.UseDwm)
                    {
                        if (OS.Win11) Radius = (int)System.Math.Round(8 * Config.Dpi); //Win11
                        return false;
                    }
                    else Radius = (int)(borderless.Radius * Config.Dpi);
                    return false;
                }
                else
                {
                    if (form.FormBorderStyle == FormBorderStyle.None) return false;
                    if (OS.Win11) Radius = (int)System.Math.Round(8 * Config.Dpi); //Win11
                    if (form is Window || form is FormNoBar) return false;//无边框处理
                    var rect = new Vanara.PInvoke.RECT();
                    Vanara.PInvoke.User32.AdjustWindowRectEx(ref rect, Vanara.PInvoke.User32.WindowStyles.WS_OVERLAPPEDWINDOW | Vanara.PInvoke.User32.WindowStyles.WS_CLIPCHILDREN, false, Vanara.PInvoke.User32.WindowStylesEx.WS_EX_CONTROLPARENT | Vanara.PInvoke.User32.WindowStylesEx.WS_EX_APPWINDOW);
                    Padd = rect.bottom;
                    return true;
                }
            }
            return false;
        }
    }
}
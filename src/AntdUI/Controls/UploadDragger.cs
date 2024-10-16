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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// UploadDragger 拖拽上传
    /// </summary>
    /// <remarks>文件选择上传和拖拽上传控件。</remarks>
    [Description("UploadDragger 拖拽上传")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    [Designer(typeof(IControlDesigner))]
    public class UploadDragger : IControl
    {
        #region 属性

        int radius = 8;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(8)]
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
                Invalidate();
                OnTextChanged(EventArgs.Empty);
            }
        }

        string? textDesc = null;
        /// <summary>
        /// 文本描述
        /// </summary>
        [Description("文本描述"), Category("外观"), DefaultValue(null)]
        public string? TextDesc
        {
            get => textDesc;
            set
            {
                if (textDesc == value) return;
                textDesc = value;
                Invalidate();
            }
        }

        Color? fore = null;
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
                if (fore == value) return;
                fore = value;
                Invalidate();
            }
        }

        #region 图标

        float iconratio = 1.92F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(1.92F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                Invalidate();
            }
        }

        Image? icon = null;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                Invalidate();
            }
        }

        string? iconSvg = "InboxOutlined";
        /// <summary>
        /// 图标SVG
        /// </summary>
        [Description("图标SVG"), Category("外观"), DefaultValue("InboxOutlined")]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                Invalidate();
            }
        }

        #endregion

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        Image? backImage = null;
        /// <summary>
        /// 背景图片
        /// </summary>
        [Description("背景图片"), Category("外观"), DefaultValue(null)]
        public new Image? BackgroundImage
        {
            get => backImage;
            set
            {
                if (backImage == value) return;
                backImage = value;
                Invalidate();
            }
        }

        TFit backFit = TFit.Fill;
        /// <summary>
        /// 背景图片布局
        /// </summary>
        [Description("背景图片布局"), Category("外观"), DefaultValue(TFit.Fill)]
        public new TFit BackgroundImageLayout
        {
            get => backFit;
            set
            {
                if (backFit == value) return;
                backFit = value;
                Invalidate();
            }
        }

        #endregion

        #region 边框

        float borderWidth = 1F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(1F)]
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                Invalidate();
            }
        }

        Color? borderColor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                if (borderWidth > 0) Invalidate();
            }
        }

        DashStyle borderStyle = DashStyle.Dash;
        /// <summary>
        /// 边框样式
        /// </summary>
        [Description("边框样式"), Category("边框"), DefaultValue(DashStyle.Dash)]
        public DashStyle BorderStyle
        {
            get => borderStyle;
            set
            {
                if (borderStyle == value) return;
                borderStyle = value;
                if (borderWidth > 0) Invalidate();
            }
        }

        #endregion

        public override Rectangle DisplayRectangle
        {
            get => ClientRectangle.PaddingRect(Padding, (borderWidth / 2F * Config.Dpi));
        }

        #endregion

        #region 渲染

        readonly StringFormat s_f = Helper.SF_Ellipsis();

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = DisplayRectangle;
            var g = e.Graphics.High();
            float _radius = radius * Config.Dpi;
            using (var path = rect.RoundPath(_radius))
            {
                using (var brush = new SolidBrush(back ?? Style.Db.FillQuaternary))
                {
                    g.FillPath(brush, path);
                }
                if (backImage != null) g.PaintImg(rect, backImage, backFit, _radius, false);

                #region 渲染主体

                var size = g.MeasureString(Config.NullText, Font).Size();
                int sp = (int)(4 * Config.Dpi), gap = (int)(16 * Config.Dpi), gap2 = gap * 2, icon_size = (int)(size.Height * iconratio);

                if (string.IsNullOrWhiteSpace(iconSvg) && icon == null)
                {
                    if (string.IsNullOrWhiteSpace(textDesc))
                    {
                        int y = rect.Y + (rect.Height - size.Height) / 2;
                        Rectangle rect_text = new Rectangle(rect.X + gap, y, rect.Width - gap2, size.Height);
                        using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                        {
                            g.DrawStr(text, Font, brush, rect_text, s_f);
                        }
                    }
                    else
                    {
                        using (var font_desc = new Font(Font.FontFamily, Font.Size * .875F))
                        {
                            var size_desc = g.MeasureString(textDesc, font_desc, rect.Width - gap2).Size();
                            int th = sp + size.Height + size_desc.Height, y = rect.Y + (rect.Height - th) / 2;
                            Rectangle rect_text = new Rectangle(rect.X + gap, y, rect.Width - gap2, size.Height),
                                rect_desc = new Rectangle(rect_text.X, rect_text.Bottom + sp, rect_text.Width, size_desc.Height);
                            using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                            {
                                g.DrawStr(text, Font, brush, rect_text, s_f);
                            }
                            using (var brush = new SolidBrush(Style.Db.TextTertiary))
                            {
                                g.DrawStr(textDesc, font_desc, brush, rect_desc, s_f);
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(textDesc))
                    {
                        int th = gap + icon_size + size.Height, y = rect.Y + (rect.Height - th) / 2;
                        Rectangle rect_icon = new Rectangle(rect.X + (rect.Width - icon_size) / 2, y, icon_size, icon_size),
                            rect_text = new Rectangle(rect.X + gap, y + icon_size + gap, rect.Width - gap2, size.Height);
                        if (iconSvg != null) g.GetImgExtend(iconSvg, rect_icon, Style.Db.Primary);
                        if (icon != null) g.DrawImage(icon, rect_icon);
                        using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                        {
                            g.DrawStr(text, Font, brush, rect_text, s_f);
                        }
                    }
                    else
                    {
                        using (var font_desc = new Font(Font.FontFamily, Font.Size * .875F))
                        {
                            var size_desc = g.MeasureString(textDesc, font_desc, rect.Width - gap2).Size();
                            int th = sp + gap + icon_size + size.Height + size_desc.Height, y = rect.Y + (rect.Height - th) / 2;
                            Rectangle rect_icon = new Rectangle(rect.X + (rect.Width - icon_size) / 2, y, icon_size, icon_size),
                                rect_text = new Rectangle(rect.X + gap, y + icon_size + gap, rect.Width - gap2, size.Height),
                                rect_desc = new Rectangle(rect_text.X, rect_text.Bottom + sp, rect_text.Width, size_desc.Height);
                            if (iconSvg != null) g.GetImgExtend(iconSvg, rect_icon, Style.Db.Primary);
                            if (icon != null) g.DrawImage(icon, rect_icon);
                            using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                            {
                                g.DrawStr(text, Font, brush, rect_text, s_f);
                            }
                            using (var brush = new SolidBrush(Style.Db.TextTertiary))
                            {
                                g.DrawStr(textDesc, font_desc, brush, rect_desc, s_f);
                            }
                        }
                    }
                }

                #endregion

                if (borderWidth > 0)
                {
                    var borw = borderWidth * Config.Dpi;
                    if (AnimationHover)
                    {
                        using (var brush = new Pen(borderColor ?? Style.Db.BorderColor, borw))
                        {
                            g.DrawPath(brush, path);
                        }
                        using (var brush = new Pen(Helper.ToColor(AnimationHoverValue, Style.Db.PrimaryHover), borw))
                        {
                            g.DrawPath(brush, path);
                        }
                    }
                    else if (ExtraMouseHover)
                    {
                        using (var brush_bor = new Pen(Style.Db.PrimaryHover, borw))
                        {
                            brush_bor.DashStyle = borderStyle;
                            g.DrawPath(brush_bor, path);
                        }
                    }
                    else
                    {
                        using (var brush_bor = new Pen(borderColor ?? Style.Db.BorderColor, borw))
                        {
                            brush_bor.DashStyle = borderStyle;
                            g.DrawPath(brush_bor, path);
                        }
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        public override Rectangle ReadRectangle
        {
            get => DisplayRectangle;
        }

        public override GraphicsPath RenderRegion
        {
            get => DisplayRectangle.RoundPath(radius * Config.Dpi);
        }

        #endregion

        #region 事件

        #region 拖拽上传

        FileDropHandler? fileDrop = null;
        /// <summary>
        /// 使用管理员权限拖拽上传
        /// </summary>
        public void UseAdmin()
        {
            new FileDropHandler(this);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            ExtraMouseHover = true;
            e.Effect = DragDropEffects.All;
        }

        protected override void OnDragLeave(EventArgs e)
        {
            ExtraMouseHover = false;
            base.OnDragLeave(e);
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            if (e.Data == null) return;
            foreach (string format in e.Data.GetFormats())
            {
                if (e.Data.GetData(format) is string[] files)
                {
                    DragChanged?.Invoke(this, new StringsEventArgs(files));
                    ExtraMouseHover = false;
                    return;
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.AllowDrop = true;
            base.OnHandleCreated(e);
        }

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            ExtraMouseHover = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            ExtraMouseHover = false;
            base.OnMouseLeave(e);
        }

        int AnimationHoverValue = 0;
        bool AnimationHover = false, _mouseHover = false;
        bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                if (Enabled)
                {
                    if (Config.Animation)
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        if (value)
                        {
                            ThreadHover = new ITask(this, () =>
                            {
                                AnimationHoverValue += 20;
                                if (AnimationHoverValue > 255) { AnimationHoverValue = 255; return false; }
                                Invalidate();
                                return true;
                            }, 10, () =>
                            {
                                AnimationHover = false;
                                Invalidate();
                            });
                        }
                        else
                        {
                            ThreadHover = new ITask(this, () =>
                            {
                                AnimationHoverValue -= 20;
                                if (AnimationHoverValue < 1) { AnimationHoverValue = 0; return false; }
                                Invalidate();
                                return true;
                            }, 10, () =>
                            {
                                AnimationHover = false;
                                Invalidate();
                            });
                        }
                    }
                    else AnimationHoverValue = 255;
                    Invalidate();
                }
            }
        }

        #endregion

        #region 事件

        public class StringsEventArgs : VEventArgs<string[]>
        {
            public StringsEventArgs(string[] value) : base(value) { }
        }

        /// <summary>
        /// Bool 类型事件
        /// </summary>
        public delegate void DragEventHandler(object sender, StringsEventArgs e);

        /// <summary>
        /// 文件拖拽后时发生
        /// </summary>
        [Description("文件拖拽后时发生"), Category("行为")]
        public event DragEventHandler? DragChanged = null;

        #endregion

        protected override void Dispose(bool disposing)
        {
            ThreadHover?.Dispose();
            fileDrop?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadHover = null;
    }

    sealed class FileDropHandler : IMessageFilter, IDisposable
    {
        #region native members

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint message, ChangeFilterAction action, in ChangeFilterStruct pChangeFilterStruct);

        [DllImport("shell32.dll", SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        private static extern void DragAcceptFiles(IntPtr hWnd, bool fAccept);

        [DllImport("shell32.dll", SetLastError = false, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        private static extern uint DragQueryFile(IntPtr hWnd, uint iFile, StringBuilder lpszFile, int cch);

        [DllImport("shell32.dll", SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        private static extern void DragFinish(IntPtr hDrop);

        [StructLayout(LayoutKind.Sequential)]
        private struct ChangeFilterStruct
        {
            public uint CbSize;
            public ChangeFilterStatu ExtStatus;
        }

        private enum ChangeFilterAction : uint
        {
            MSGFLT_RESET,
            MSGFLT_ALLOW,
            MSGFLT_DISALLOW
        }

        private enum ChangeFilterStatu : uint
        {
            MSGFLTINFO_NONE,
            MSGFLTINFO_ALREADYALLOWED_FORWND,
            MSGFLTINFO_ALREADYDISALLOWED_FORWND,
            MSGFLTINFO_ALLOWED_HIGHER
        }

        private const uint WM_COPYGLOBALDATA = 0x0049;
        private const uint WM_COPYDATA = 0x004A;
        private const uint WM_DROPFILES = 0x0233;

        #endregion

        const uint GetIndexCount = 0xFFFFFFFFU;

        Control ContainerControl;

        public FileDropHandler(Control containerControl)
        {
            ContainerControl = containerControl ?? throw new ArgumentNullException("control", "control is null.");
            if (containerControl.IsDisposed) throw new ObjectDisposedException("control");
            var status = new ChangeFilterStruct { CbSize = 8 };

            if (!ChangeWindowMessageFilterEx(containerControl.Handle, WM_DROPFILES, ChangeFilterAction.MSGFLT_ALLOW, in status)) throw new Win32Exception(Marshal.GetLastWin32Error());
            if (!ChangeWindowMessageFilterEx(containerControl.Handle, WM_COPYGLOBALDATA, ChangeFilterAction.MSGFLT_ALLOW, in status)) throw new Win32Exception(Marshal.GetLastWin32Error());
            if (!ChangeWindowMessageFilterEx(containerControl.Handle, WM_COPYDATA, ChangeFilterAction.MSGFLT_ALLOW, in status)) throw new Win32Exception(Marshal.GetLastWin32Error());
            DragAcceptFiles(containerControl.Handle, true);

            Application.AddMessageFilter(this);
        }

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (ContainerControl == null || ContainerControl.IsDisposed) return false;
            if (ContainerControl.AllowDrop) return ContainerControl.AllowDrop = false;
            if (m.Msg == WM_DROPFILES)
            {
                var handle = m.WParam;

                var fileCount = DragQueryFile(handle, GetIndexCount, null, 0);

                var fileNames = new string[fileCount];

                var sb = new StringBuilder(262);
                var charLength = sb.Capacity;
                for (uint i = 0; i < fileCount; i++)
                {
                    if (DragQueryFile(handle, i, sb, charLength) > 0) fileNames[i] = sb.ToString();
                }
                DragFinish(handle);
                ContainerControl.AllowDrop = true;
                ContainerControl.DoDragDrop(fileNames, DragDropEffects.All);
                ContainerControl.AllowDrop = false;
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            Application.RemoveMessageFilter(this);
        }
    }
}
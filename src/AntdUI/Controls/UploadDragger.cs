// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
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
        public UploadDragger() : base(ControlType.Select) { }

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
                OnPropertyChanged(nameof(Radius));
            }
        }

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
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged(nameof(Text));
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        string? textDesc;
        /// <summary>
        /// 文本描述
        /// </summary>
        [Description("文本描述"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? TextDesc
        {
            get => textDesc;
            set
            {
                if (textDesc == value) return;
                textDesc = value;
                Invalidate();
                OnPropertyChanged(nameof(TextDesc));
            }
        }

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
                if (fore == value) return;
                fore = value;
                Invalidate();
                OnPropertyChanged(nameof(ForeColor));
            }
        }

        /// <summary>
        /// 点击上传
        /// </summary>
        [Description("点击上传"), Category("行为"), DefaultValue(true)]
        public bool ClickHand { get; set; } = true;

        /// <summary>
        /// 多个文件
        /// </summary>
        [Description("多个文件"), Category("行为"), DefaultValue(true)]
        public bool Multiselect { get; set; } = true;

        string? filter;
        /// <summary>
        /// 文件名筛选器字符串
        /// </summary>
        [Description("文件名筛选器字符串"), Category("行为"), DefaultValue(null)]
        public string? Filter
        {
            get => filter;
            set
            {
                if (filter == value) return;
                filter = value;
                if (value == null) ONDRAG = null;
                else
                {
                    ONDRAG = (files) =>
                    {
                        if (!Multiselect && files.Length > 1) files = new string[] { files[0] };
                        if (filter == null) return files;
                        // 实现文件路径过滤 Filter
                        var filters = filter.Split('|');
                        if (filters.Length > 1)
                        {
                            var tmp = new List<string>(files.Length);
                            foreach (var file in files)
                            {
                                var fileExtension = System.IO.Path.GetExtension(file);
                                if (HandFilter(fileExtension, filters)) tmp.Add(file);
                            }
                            if (tmp.Count > 0) return tmp.ToArray();
                        }
                        return null;
                    };
                }
            }
        }

        bool HandFilter(string fileExtension, string[] filters)
        {
            for (int i = 1; i < filters.Length; i += 2)
            {
                if (filters[i] == "*.*") return true;
                var extensions = filters[i].Split(';');
                if (Array.Exists(extensions, ext => ext.Equals($"*{fileExtension}", StringComparison.OrdinalIgnoreCase))) return true;
            }
            return false;
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
                OnPropertyChanged(nameof(IconRatio));
            }
        }

        Image? icon;
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
                OnPropertyChanged(nameof(Icon));
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
                OnPropertyChanged(nameof(IconSvg));
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
                OnPropertyChanged(nameof(Back));
            }
        }

        Image? backImage;
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
                OnPropertyChanged(nameof(BackgroundImage));
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
                OnPropertyChanged(nameof(BackgroundImageLayout));
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
                OnPropertyChanged(nameof(BorderWidth));
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
                OnPropertyChanged(nameof(BorderColor));
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
                OnPropertyChanged(nameof(BorderStyle));
            }
        }

        #endregion

        public override Rectangle DisplayRectangle => ClientRectangle.PaddingRect(Padding, (borderWidth / 2F * Dpi));

        #endregion

        #region 渲染

        readonly FormatFlags s_f = FormatFlags.Center | FormatFlags.EllipsisCharacter;

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            var rect = DisplayRectangle;
            float _radius = radius * Dpi;
            using (var path = rect.RoundPath(_radius))
            {
                g.Fill(back ?? Colour.FillQuaternary.Get(nameof(UploadDragger), ColorScheme), path);
                if (backImage != null) g.Image(rect, backImage, backFit, _radius, false);

                #region 渲染主体

                var size = g.MeasureString(Config.NullText, Font);
                int sp = (int)(4 * Dpi), gap = (int)(16 * Dpi), gap2 = gap * 2, icon_size = (int)(size.Height * iconratio);

                if (string.IsNullOrWhiteSpace(iconSvg) && icon == null)
                {
                    if (string.IsNullOrWhiteSpace(TextDesc))
                    {
                        int y = rect.Y + (rect.Height - size.Height) / 2;
                        var rect_text = new Rectangle(rect.X + gap, y, rect.Width - gap2, size.Height);
                        using (var brush = new SolidBrush(fore ?? Colour.Text.Get(nameof(UploadDragger), ColorScheme)))
                        {
                            g.DrawText(Text, Font, brush, rect_text, s_f);
                        }
                    }
                    else
                    {
                        using (var font_desc = new Font(Font.FontFamily, Font.Size * .875F))
                        {
                            var size_desc = g.MeasureText(TextDesc, font_desc, rect.Width - gap2);
                            int th = sp + size.Height + size_desc.Height, y = rect.Y + (rect.Height - th) / 2;
                            Rectangle rect_text = new Rectangle(rect.X + gap, y, rect.Width - gap2, size.Height),
                                rect_desc = new Rectangle(rect_text.X, rect_text.Bottom + sp, rect_text.Width, size_desc.Height);
                            using (var brush = new SolidBrush(fore ?? Colour.Text.Get(nameof(UploadDragger), ColorScheme)))
                            {
                                g.DrawText(Text, Font, brush, rect_text, s_f);
                            }
                            using (var brush = new SolidBrush(Colour.TextTertiary.Get(nameof(UploadDragger), ColorScheme)))
                            {
                                g.DrawText(TextDesc, font_desc, brush, rect_desc, s_f);
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(TextDesc))
                    {
                        int th = gap + icon_size + size.Height, y = rect.Y + (rect.Height - th) / 2;
                        Rectangle rect_icon = new Rectangle(rect.X + (rect.Width - icon_size) / 2, y, icon_size, icon_size),
                            rect_text = new Rectangle(rect.X + gap, y + icon_size + gap, rect.Width - gap2, size.Height);
                        if (icon != null) g.Image(icon, rect_icon);
                        if (iconSvg != null) g.GetImgExtend(iconSvg, rect_icon, Colour.Primary.Get(nameof(UploadDragger), ColorScheme));
                        using (var brush = new SolidBrush(fore ?? Colour.Text.Get(nameof(UploadDragger), ColorScheme)))
                        {
                            g.DrawText(Text, Font, brush, rect_text, s_f);
                        }
                    }
                    else
                    {
                        using (var font_desc = new Font(Font.FontFamily, Font.Size * .875F))
                        {
                            var size_desc = g.MeasureText(TextDesc, font_desc, rect.Width - gap2);
                            int th = sp + gap + icon_size + size.Height + size_desc.Height, y = rect.Y + (rect.Height - th) / 2;
                            Rectangle rect_icon = new Rectangle(rect.X + (rect.Width - icon_size) / 2, y, icon_size, icon_size),
                                rect_text = new Rectangle(rect.X + gap, y + icon_size + gap, rect.Width - gap2, size.Height),
                                rect_desc = new Rectangle(rect_text.X, rect_text.Bottom + sp, rect_text.Width, size_desc.Height);
                            if (icon != null) g.Image(icon, rect_icon);
                            if (iconSvg != null) g.GetImgExtend(iconSvg, rect_icon, Colour.Primary.Get(nameof(UploadDragger), ColorScheme));
                            using (var brush = new SolidBrush(fore ?? Colour.Text.Get(nameof(UploadDragger), ColorScheme)))
                            {
                                g.DrawText(Text, Font, brush, rect_text, s_f);
                            }
                            using (var brush = new SolidBrush(Colour.TextTertiary.Get(nameof(UploadDragger), ColorScheme)))
                            {
                                g.DrawText(TextDesc, font_desc, brush, rect_desc, s_f);
                            }
                        }
                    }
                }

                #endregion

                if (borderWidth > 0)
                {
                    var borw = borderWidth * Dpi;
                    if (AnimationHover) g.Draw((borderColor ?? Colour.BorderColor.Get(nameof(UploadDragger), ColorScheme)).BlendColors(AnimationHoverValue, Colour.PrimaryHover.Get(nameof(UploadDragger), ColorScheme)), borw, path);
                    else if (ExtraMouseHover) g.Draw(Colour.PrimaryHover.Get(nameof(UploadDragger), ColorScheme), borw, borderStyle, path);
                    else g.Draw(borderColor ?? Colour.BorderColor.Get(nameof(UploadDragger), ColorScheme), borw, borderStyle, path);
                }
            }
            base.OnDraw(e);
        }

        public override Rectangle ReadRectangle => DisplayRectangle;

        public override GraphicsPath RenderRegion => DisplayRectangle.RoundPath(radius * Dpi);

        #endregion

        #region 事件

        #region 拖拽上传

        protected override void OnDragEnter() => ExtraMouseHover = true;
        protected override void OnDragLeave() => ExtraMouseHover = false;

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
                    if (Config.HasAnimation(nameof(UploadDragger), Name))
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        ThreadHover = new AnimationTask(new AnimationLinearConfig(this, i =>
                        {
                            AnimationHoverValue = i;
                            Invalidate();
                            return true;
                        }, 10).SetValueColor(AnimationHoverValue, value, 20).SetEnd(() => AnimationHover = false));
                    }
                    else AnimationHoverValue = 255;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (ClickHand && e.Button == MouseButtons.Left) ManualSelection();
        }

        public void ManualSelection()
        {
            using (var dialog = new OpenFileDialog
            {
                Multiselect = Multiselect,
                Filter = Filter ?? (Localization.Get("All Files", "所有文件") + "|*.*")
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK) OnDragChanged(dialog.FileNames);
            }
        }

        #endregion

        public void SetFilter(FilterType filterType)
        {
            bool all = filterType.HasFlag(FilterType.ALL), video = filterType.HasFlag(FilterType.Video), imgs = filterType.HasFlag(FilterType.Imgs), img = filterType.HasFlag(FilterType.Img);
            if (video || imgs || img)
            {
                var fs = new List<string>(2);
                if (video) fs.Add(Localization.Get("Video Files", "视频文件") + "|*.mp4;*.avi;*.rm;*.rmvb;*.flv;*.xr;*.mpg;*.vcd;*.svcd;*.dvd;*.vob;*.asf;*.wmv;*.mov;*.qt;*.3gp;*.sdp;*.yuv;*.mkv;*.dat;*.torrent;*.mp3;*.3g2;*.3gp2;*.3gpp;*.aac;*.ac3;*.aif;*.aifc;*.aiff;*.amr;*.amv;*.ape;*.asp;*.bik;*.csf;*.divx;*.evo;*.f4v;*.hlv;*.ifo;*.ivm;*.m1v;*.m2p;*.m2t;*.m2ts;*.m2v;*.m4b;*.m4p;*.m4v;*.mag;*.mid;*.mod;*.movie;*.mp2v;*.mp2;*.mpa;*.mpeg;*.mpeg4;*.mpv2;*.mts;*.ogg;*.ogm;*.pmp;*.pss;*.pva;*.qt;*.ram;*.rp;*.rpm;*.rt;*.scm;*.smi;*.smil;*.svx;*.swf;*.tga;*.tod;*.tp;*.tpr;*.ts;*.voc;*.vp6;*.wav;*.webm;*.wma;*.wm;*.wmp;*.xlmv;*.xv;*.xvid");
                if (imgs) fs.Add(Localization.Get("Picture Files", "图片文件") + "|*.png;*.gif;*.jpg;*.jpeg;*.bmp");
                if (img) fs.Add(Localization.Get("Picture Files", "图片文件") + "|*.jpg;*.jpeg;*.png;*.bmp");
                if (all) fs.Add(Localization.Get("All Files", "所有文件") + "|*.*");
                Filter = string.Join("|", fs);
            }
            else Filter = null;
        }

        [Flags]
        public enum FilterType
        {
            ALL = 0,
            Img = 1,
            Imgs = 2,
            Video = 4
        }

        protected override void Dispose(bool disposing)
        {
            ThreadHover?.Dispose();
            base.Dispose(disposing);
        }
        AnimationTask? ThreadHover;
    }

    public sealed class FileDropHandler : IMessageFilter, IDisposable
    {
        #region native members

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint message, ChangeFilterAction action, in ChangeFilterStruct pChangeFilterStruct);

        [DllImport("shell32.dll", SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        private static extern void DragAcceptFiles(IntPtr hWnd, bool fAccept);

        [DllImport("shell32.dll", SetLastError = false, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        private static extern uint DragQueryFile(IntPtr hWnd, uint iFile, StringBuilder? lpszFile, int cch);

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
            ContainerControl = containerControl;
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
                var point = Control.MousePosition;
                var rect = ContainerControl.RectangleToScreen(ContainerControl.ClientRectangle);
                if (rect.Contains(point))
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
            }
            return false;
        }

        public void Dispose() => Application.RemoveMessageFilter(this);
    }
}
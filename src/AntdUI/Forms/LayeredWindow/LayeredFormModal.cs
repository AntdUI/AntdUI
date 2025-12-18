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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormModal : Window, IEventListener, LayeredFormAsynLoad
    {
        internal Modal.Config config;
        Panel? panel_main;
        public LayeredFormModal(Modal.Config _config)
        {
            config = _config;
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            UpdateStyles();

            DisableTheme = true;
            Resizable = config.Resizable;
            if (!config.Resizable) FormBorderStyle = FormBorderStyle.FixedSingle;
            ShowInTaskbar = false;
            var form_tmp = config.Target.FindPARENT();
            if (form_tmp != null && form_tmp.TopMost || (form_tmp is LayeredFormPopover layered && layered.topMost) || (form_tmp is LayeredFormDrawer layeredDrawer && layeredDrawer.topMost) || (form_tmp is LayeredFormTour layeredTour && layeredTour.topMost)) TopMost = true;
            config.Target.SetIcon(this);
            close_button = new ITaskOpacity(nameof(AntdUI.Modal), this);

            #region InitializeComponent

            SuspendLayout();

            int butt_h = (int)Math.Round(config.BtnHeight * Dpi);
            BackColor = Colour.BgElevated.Get(nameof(AntdUI.Modal), config.ColorScheme);
            Size = new Size(416, 122 + butt_h);
            config.Target.SetFontConfig(config.Font, this);
            ForeColor = Colour.TextBase.Get(nameof(AntdUI.Modal), config.ColorScheme);

            if (butt_h > 0)
            {
                btn_ok = new Button
                {
                    AutoSizeMode = TAutoSize.Width,
                    Dock = DockStyle.Right,
                    Location = new Point(304, 0),
                    Name = "btn_ok",
                    Size = new Size(64, butt_h),
                    TabIndex = 0,
                    Type = config.OkType,
                    Text = config.OkText
                };
                config.OnButtonStyle?.Invoke("OK", btn_ok);
                btn_ok.Click += btn_ok_Click;
                if (config.OkFont != null) btn_ok.Font = config.OkFont;

                if (config.CancelText != null)
                {
                    btn_no = new Button
                    {
                        AutoSizeMode = TAutoSize.Width,
                        BorderWidth = 1F,
                        Dock = DockStyle.Right,
                        Location = new Point(240, 0),
                        Name = "btn_no",
                        Size = new Size(64, butt_h),
                        TabIndex = 1,
                        Text = config.CancelText
                    };
                    config.OnButtonStyle?.Invoke("Cancel", btn_no);
                    btn_no.Click += btn_no_Click;
                    if (config.CancelFont != null) btn_no.Font = config.CancelFont;
                }

                panel_main = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Back = Color.Transparent,
                    Size = new Size(368, butt_h)
                };
                if (btn_no != null) panel_main.Controls.Add(btn_no);
                panel_main.Controls.Add(btn_ok);

                if (config.Btns != null)
                {
                    var btns = new List<Button>(config.Btns.Length);
                    foreach (var btn in config.Btns)
                    {
                        var _btn = new Button
                        {
                            AutoSizeMode = TAutoSize.Width,
                            Dock = DockStyle.Right,
                            Size = new Size(64, butt_h),
                            Name = btn.Name,
                            Text = btn.Text,
                            Type = btn.Type,
                            BackColor = btn.Back,
                            ForeColor = btn.Fore,
                            Tag = btn
                        };
                        config.OnButtonStyle?.Invoke(btn.Name, _btn);
                        panel_main.Controls.Add(_btn);
                        btns.Insert(0, _btn);
                    }
                    foreach (var btn in btns)
                    {
                        btn.BringToFront();
                        btn.Click += (a, b) =>
                        {
                            isclose = false;
                            btn.Loading = true;
                            bool DisableCancel = false;
                            if (config.LoadingDisableCancel && btn_no != null)
                            {
                                btn_no.Enabled = false;
                                DisableCancel = true;
                            }
                            ITask.Run(() =>
                            {
                                bool result = true;
                                if (config.OnBtns != null)
                                {
                                    try
                                    {
                                        result = config.OnBtns(btn);
                                    }
                                    catch { result = false; }
                                }
                                btn.Loading = false;
                                isclose = true;
                                if (result)
                                {
                                    System.Threading.Thread.Sleep(10);
                                    BeginInvoke(() =>
                                    {
                                        if (IsHandleCreated && !IsDisposed)
                                        {
                                            if (btn.Tag is Modal.Btn btnResult)
                                            {
                                                if (btnResult.DialogResult != DialogResult.None)
                                                {
                                                    DialogResult = btnResult.DialogResult;
                                                    return;
                                                }
                                            }
                                            Close();
                                        }
                                    });
                                }
                                else if (DisableCancel && btn_no != null)
                                {
                                    BeginInvoke(() =>
                                    {
                                        if (btn_no.IsHandleCreated && !btn_no.IsDisposed) btn_no.Enabled = true;
                                    });
                                }
                            });
                        };
                    }
                }
                Controls.Add(panel_main);
                if (config.Draggable) panel_main.MouseMove += Window_MouseDown;
            }

            if (config.Keyboard)
            {
                if (butt_h > 0)
                {
                    CancelButton = btn_no;
                    if (config.DefaultAcceptButton) AcceptButton = btn_ok;
                }
                else ONESC = () => DialogResult = DialogResult.No;
            }

            #endregion

            rectsContent = Helper.GDI(g =>
            {
                int gap = (int)Math.Round(8F * Dpi), paddingx = (int)Math.Round(config.Padding.Width * Dpi), paddingy = (int)Math.Round(config.Padding.Height * Dpi),
                cpaddingx = (int)Math.Round(config.ContentPadding.Width * Dpi), cpaddingy = (int)Math.Round(config.ContentPadding.Height * Dpi), cpaddingx2 = cpaddingx * 2, cpaddingy2 = cpaddingy * 2,
                    w = (int)Math.Round(config.Width * Dpi), wp = w - paddingx * 2;
                Padding = new Padding(paddingx, paddingy, paddingx, paddingy);
                if (panel_main != null) panel_main.Height = butt_h;

                using (var fontTitle = new Font(Font.FontFamily, Font.Size * 1.14F, FontStyle.Bold))
                {
                    int tmpicon = 0;
                    if (config.Content is Control control)
                    {
                        Win32.WindowTheme(control, Config.IsDark);
                        Helper.DpiAuto(Dpi, control);
                        w = control.Width + paddingx * 2 + cpaddingx2;
                        wp = control.Width + cpaddingx2;
                        Controls.Add(control);
                        if (config.Resizable)
                        {
                            control.Dock = DockStyle.Fill;
                            control.BringToFront();
                        }
                        control.Disposed += (a, b) => Close();
                        if (_config.Icon == TType.None && _config.IconCustom == null)
                        {
                            if (config.Title == null && !config.CloseIcon)
                            {
                                rectTitle = new Rectangle(0, 0, 0, 0);
                                int h = control.Height + butt_h + cpaddingy2;
                                rectContent = new Rectangle(paddingx + cpaddingx, paddingy + cpaddingy, wp - cpaddingx2, h - butt_h - cpaddingy2);
                                Size = new Size(w, h + paddingy * 2);
                            }
                            else
                            {
                                var sizeTitle = g.MeasureText(config.Title, fontTitle, wp - cpaddingx2);

                                int h = sizeTitle.Height + gap + control.Height + butt_h + cpaddingy2;

                                rectTitle = new Rectangle(paddingx, paddingy, wp - cpaddingx2, sizeTitle.Height + gap);
                                rectContent = new Rectangle(rectTitle.X + cpaddingx, rectTitle.Bottom + cpaddingy, wp - cpaddingx2, h - butt_h - sizeTitle.Height - gap - cpaddingy2);
                                Size = new Size(w, h + paddingy * 2);
                            }
                        }
                        else
                        {
                            var sizeT = g.MeasureString(Config.NullText, fontTitle);
                            int icon_size = tmpicon = sizeT.Height, icon_size_x = (int)(icon_size * 0.54F), ww = wp - (icon_size + icon_size_x) - cpaddingx2;
                            var sizeTitle = g.MeasureText(config.Title, fontTitle, ww);
                            int h = sizeTitle.Height + gap + control.Height + butt_h + cpaddingy2;
                            rectTitle = new Rectangle(paddingx + icon_size + icon_size_x, paddingy, ww, sizeTitle.Height + gap);
                            rectIcon = new Rectangle(paddingx, rectTitle.Y + (rectTitle.Height - icon_size) / 2, icon_size, icon_size);
                            if (config.UseIconPadding)
                            {
                                wp = ww + cpaddingx2;
                                rectContent = new Rectangle(rectTitle.X + cpaddingx, rectTitle.Bottom + cpaddingy, ww, h - butt_h - sizeTitle.Height - gap - cpaddingy2);
                            }
                            else rectContent = new Rectangle(paddingx + cpaddingx, rectTitle.Bottom + cpaddingy, wp - cpaddingx2, h - butt_h - sizeTitle.Height - gap - cpaddingy2);
                            control.Location = new Point(rectContent.X, rectContent.Y);
                            Size = new Size(w, h + paddingy * 2);
                        }
                        if (config.CloseIcon)
                        {
                            if (tmpicon == 0) tmpicon = g.MeasureString(Config.NullText, fontTitle).Height;
                            int close_size = tmpicon;
                            rect_close = new Rectangle(rectTitle.Right - close_size, rectTitle.Y, close_size, close_size);
                        }
                        control.Location = new Point(rectContent.X, rectContent.Y);
                        control.Size = new Size(rectContent.Width, rectContent.Height);
                    }
                    else if (config.Content is IList<Modal.TextLine> list)
                    {
                        rtext = true;
                        var texts = new List<Rectangle>(list.Count);
                        if (_config.Icon == TType.None && _config.IconCustom == null)
                        {
                            var sizeTitle = g.MeasureText(config.Title, fontTitle, wp - cpaddingx2);
                            rectTitle = new Rectangle(paddingx, paddingy, wp - cpaddingx2, sizeTitle.Height + gap);

                            int has_y = paddingy + sizeTitle.Height + gap + cpaddingy, h_temp = 0;
                            foreach (var txt in list)
                            {
                                var sizeContent = g.MeasureText(txt.Text, txt.Font ?? Font, wp - cpaddingx2);
                                int txt_h = sizeContent.Height + (int)(txt.Gap * Dpi);
                                texts.Add(new Rectangle(rectTitle.X + cpaddingx, has_y, wp - cpaddingx2, txt_h));
                                has_y += txt_h;
                                h_temp += txt_h;
                            }

                            int h = sizeTitle.Height + gap + h_temp + butt_h + cpaddingy2;
                            rectContent = new Rectangle(rectTitle.X + cpaddingx, rectTitle.Bottom + cpaddingy, wp - cpaddingx2, h - butt_h - sizeTitle.Height - gap - cpaddingy2);
                            Size = new Size(w, h + paddingy * 2);
                        }
                        else
                        {
                            var sizeT = g.MeasureString(Config.NullText, fontTitle);
                            int icon_size = tmpicon = sizeT.Height, icon_size_x = (int)(icon_size * 0.54F), ww = wp - (icon_size + icon_size_x) - cpaddingx2, h;
                            var sizeTitle = g.MeasureText(config.Title, fontTitle, ww);
                            rectTitle = new Rectangle(paddingx + icon_size + icon_size_x, paddingy, ww, sizeTitle.Height + gap);
                            rectIcon = new Rectangle(paddingx, rectTitle.Y + (rectTitle.Height - icon_size) / 2, icon_size, icon_size);
                            if (config.UseIconPadding)
                            {
                                wp = ww + cpaddingx2;
                                int has_y = paddingy + sizeTitle.Height + gap + cpaddingy, h_temp = 0;
                                foreach (var txt in list)
                                {
                                    var sizeContent = g.MeasureText(txt.Text, txt.Font ?? Font, ww);
                                    int txt_h = sizeContent.Height + (int)(txt.Gap * Dpi);
                                    texts.Add(new Rectangle(rectTitle.X + cpaddingx, has_y, ww, txt_h));
                                    has_y += txt_h;
                                    h_temp += txt_h;
                                }
                                h = sizeTitle.Height + gap + h_temp + butt_h + cpaddingy2;
                                rectContent = new Rectangle(rectTitle.X + cpaddingx, rectTitle.Bottom + cpaddingy, ww, h - butt_h - sizeTitle.Height - gap - cpaddingy2);
                            }
                            else
                            {
                                int has_y = paddingy + sizeTitle.Height + gap + cpaddingy, h_temp = 0;
                                foreach (var txt in list)
                                {
                                    var sizeContent = g.MeasureText(txt.Text, txt.Font ?? Font, wp - cpaddingx2);
                                    int txt_h = sizeContent.Height + (int)(txt.Gap * Dpi);
                                    texts.Add(new Rectangle(paddingx + cpaddingx, has_y, wp - cpaddingx2, txt_h));
                                    has_y += txt_h;
                                    h_temp += txt_h;
                                }
                                h = sizeTitle.Height + gap + h_temp + butt_h + cpaddingy2;
                                rectContent = new Rectangle(paddingx + cpaddingx, rectTitle.Bottom + cpaddingy, wp - cpaddingx2, h - butt_h - sizeTitle.Height - gap - cpaddingy2);
                            }
                            Size = new Size(w, h + paddingy * 2);
                        }
                        if (config.CloseIcon)
                        {
                            if (tmpicon == 0) tmpicon = g.MeasureString(Config.NullText, fontTitle).Height;
                            int close_size = tmpicon;
                            rect_close = new Rectangle(rectTitle.Right - close_size, rectTitle.Y, close_size, close_size);
                        }
                        return texts.ToArray();
                    }
                    else
                    {
                        rtext = true;
                        var content = config.Content.ToString();
                        if (_config.Icon == TType.None && _config.IconCustom == null)
                        {
                            Size sizeTitle = g.MeasureText(config.Title, fontTitle, wp - cpaddingx2), sizeContent = g.MeasureText(content, Font, wp - cpaddingx2);
                            int h = sizeTitle.Height + gap + sizeContent.Height + butt_h + cpaddingy2;

                            rectTitle = new Rectangle(paddingx, paddingy, wp - cpaddingx2, sizeTitle.Height + gap);
                            rectContent = new Rectangle(rectTitle.X + cpaddingx, rectTitle.Bottom + cpaddingy, wp - cpaddingx2, h - butt_h - sizeTitle.Height - gap - cpaddingy2);

                            Size = new Size(w, h + paddingy * 2);
                        }
                        else
                        {
                            var sizeT = g.MeasureString(Config.NullText, fontTitle);
                            int icon_size = tmpicon = sizeT.Height, icon_size_x = (int)(icon_size * 0.54F), ww = wp - (icon_size + icon_size_x) - cpaddingx2, h;
                            if (config.UseIconPadding)
                            {
                                wp = ww + cpaddingx2;
                                Size sizeTitle = g.MeasureText(config.Title, fontTitle, ww), sizeContent = g.MeasureText(content, Font, ww);
                                h = sizeTitle.Height + gap + sizeContent.Height + butt_h + cpaddingy2;

                                rectTitle = new Rectangle(paddingx + icon_size + icon_size_x, paddingy, ww, sizeTitle.Height + gap);
                                rectContent = new Rectangle(rectTitle.X + cpaddingx, rectTitle.Bottom + cpaddingy, ww, h - butt_h - sizeTitle.Height - gap - cpaddingy2);
                            }
                            else
                            {
                                Size sizeTitle = g.MeasureText(config.Title, fontTitle, ww), sizeContent = g.MeasureText(content, Font, wp - cpaddingx2);
                                h = sizeTitle.Height + gap + sizeContent.Height + butt_h + cpaddingy2;

                                rectTitle = new Rectangle(paddingx + icon_size + icon_size_x, paddingy, ww, sizeTitle.Height + gap);
                                rectContent = new Rectangle(paddingx + cpaddingx, rectTitle.Bottom + cpaddingy, wp - cpaddingx2, h - butt_h - sizeTitle.Height - gap - cpaddingy2);
                            }
                            rectIcon = new Rectangle(paddingx, rectTitle.Y + (rectTitle.Height - icon_size) / 2, icon_size, icon_size);
                            Size = new Size(w, h + paddingy * 2);
                        }
                        if (config.CloseIcon)
                        {
                            if (tmpicon == 0) tmpicon = g.MeasureString(Config.NullText, fontTitle).Height;
                            int close_size = tmpicon;
                            rect_close = new Rectangle(rectTitle.Right - close_size, rectTitle.Y, close_size, close_size);
                        }
                    }
                    return new Rectangle[0];
                }
            });
            if (config.Resizable)
            {
                if (config.MinimumSize.HasValue) MinimumSize = config.MinimumSize.Value;
                if (config.MaximumSize.HasValue) MaximumSize = config.MaximumSize.Value;
            }
            else MinimumSize = MaximumSize = Size;
            ResumeLayout();
            config.Layered = this;
            if (config.Content is Modal.UserControl tmp) tmp.Layered = this;

            if (config.Target.Value is Form form)
            {
                if (form.WindowState == FormWindowState.Minimized || !form.Visible) StartPosition = FormStartPosition.CenterScreen;
                else
                {
                    StartPosition = FormStartPosition.Manual;
                    Top = form.Top + (form.Height - Height) / 2;
                    Left = form.Left + (form.Width - Width) / 2;
                }
            }
            else StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 是否正在加载
        /// </summary>
        [Description("是否正在加载"), Category("参数"), DefaultValue(true)]
        public bool IsLoad { get; set; } = true;

        /// <summary>
        /// 加载完成回调
        /// </summary>
        [Description("加载完成回调"), Category("参数"), DefaultValue(null)]
        public Action? LoadCompleted { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IsLoad = false;
            LoadCompleted?.Invoke();
            if (config.Content is Control control)
            {
                control.ControlEvent();
                if (config.DefaultFocus)
                {
                    ITask.Run(() => System.Threading.Thread.Sleep(100), () =>
                    {
                        if (IsDisposed) return;
                        BeginInvoke(() => control.Focus());
                    });
                }
            }
            if (config.Content is ControlEvent controlEvent) controlEvent.LoadCompleted();

            if (config.EnableSound)
            {
                MessageType soundType = config.Icon switch
                {
                    TType.Success => MessageType.Information,
                    TType.Info => MessageType.Information,
                    TType.Warn => MessageType.Warning,
                    TType.Error => MessageType.Error,
                    _ => MessageType.Information
                };
                SystemSoundHelper.PlaySound(soundType);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoHandDpi { get; set; }

        protected override void DestroyHandle()
        {
            base.DestroyHandle();
            config.Layered = null;
            btn_ok?.Dispose();
            btn_no?.Dispose();
            close_button.Dispose();
            if (panel_main != null)
            {
                panel_main.MouseMove -= Window_MouseDown;
                panel_main?.Dispose();
                panel_main = null;
            }
            if (config.Content is Control control) control.Dispose();
            Dispose();

            if (config.ManualActivateParent)
            {
                // 在抽屉关闭后恢复主窗体的激活/置前
                try
                {
                    var owner = config.Target.Parent();
                    if (owner != null && owner.IsHandleCreated && owner.Visible)
                    {
                        if (owner.InvokeRequired) owner.BeginInvoke(new Action(() => owner.Activate()));
                        else owner.Activate();
                    }
                }
                catch { }
            }
        }

        DateTime old_now;
        int count = 0;
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (config.MaskClosable && isclose)
            {
                if (m.Msg == 0xa0 || m.Msg == 0x200) count = 0;
                else if (m.Msg == 134)
                {
                    var now = DateTime.Now;
                    if (now > old_now)
                    {
                        count = 0;
                        old_now = now.AddSeconds(1);
                    }
                    count++;
                    if (count > 2) DialogResult = DialogResult.No;
                }
            }
            base.WndProc(ref m);
        }

        void Window_MouseDown(object? sender, MouseEventArgs e) => DraggableMouseDown();

        Rectangle rectIcon, rectTitle, rectContent;
        Rectangle[] rectsContent;
        bool rtext = false;

        readonly FormatFlags stringLeft = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.EllipsisCharacter,
            stringTL = FormatFlags.Left | FormatFlags.Top | FormatFlags.EllipsisCharacter;
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            if (config.IconCustom != null) g.PaintIcons(config.IconCustom, rectIcon);
            else if (config.Icon != TType.None) g.PaintIcons(config.Icon, rectIcon, "Modal", TAMode.Auto);
            if (config.CloseIcon)
            {
                if (close_button.Animation)
                {
                    using (var path = rect_close.RoundPath((int)(4 * Dpi)))
                    {
                        g.Fill(Helper.ToColor(close_button.Value, Colour.FillSecondary.Get(nameof(AntdUI.Modal), config.ColorScheme)), path);
                    }
                    g.PaintIconClose(rect_close, Colour.Text.Get(nameof(AntdUI.Modal), config.ColorScheme), .6F);
                }
                else if (close_button.Switch)
                {
                    using (var path = rect_close.RoundPath((int)(4 * Dpi)))
                    {
                        g.Fill(Colour.FillSecondary.Get(nameof(AntdUI.Modal), config.ColorScheme), path);
                    }
                    g.PaintIconClose(rect_close, Colour.Text.Get(nameof(AntdUI.Modal), config.ColorScheme), .6F);
                }
                else g.PaintIconClose(rect_close, Colour.TextTertiary.Get(nameof(AntdUI.Modal), config.ColorScheme), .6F);
            }
            using (var brush = new SolidBrush(Colour.Text.Get(nameof(AntdUI.Modal), config.ColorScheme)))
            {
                using (var fontTitle = new Font(Font.FontFamily, Font.Size * 1.14F, FontStyle.Bold))
                {
                    g.DrawText(config.Title, fontTitle, brush, rectTitle, stringLeft);
                }
                if (rtext)
                {
                    if (config.Content is IList<Modal.TextLine> list)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            var txt = list[i];
                            if (txt.Fore.HasValue)
                            {
                                using (var fore = new SolidBrush(txt.Fore.Value))
                                {
                                    g.DrawText(txt.Text, txt.Font ?? Font, fore, rectsContent[i], stringLeft);
                                }
                            }
                            else g.DrawText(txt.Text, txt.Font ?? Font, brush, rectsContent[i], stringLeft);
                        }
                    }
                    else g.DrawText(config.Content.ToString(), Font, brush, rectContent, stringTL);
                }
            }
        }

        #region 鼠标

        ITaskOpacity close_button;
        Rectangle rect_close;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (config.CloseIcon)
            {
                close_button.MaxValue = Colour.FillSecondary.Get(nameof(AntdUI.Modal), config.ColorScheme).A;
                close_button.Switch = rect_close.Contains(e.X, e.Y);
                SetCursor(close_button.Switch);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && config.CloseIcon && rect_close.Contains(e.X, e.Y))
            {
                base.OnMouseUp(e);
                return;
            }
            if (config.Draggable) DraggableMouseDown();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && config.CloseIcon && rect_close.Contains(e.X, e.Y))
            {
                DialogResult = DialogResult.No;
                return;
            }
            base.OnMouseUp(e);
        }

        void btn_no_Click(object? sender, EventArgs e) => DialogResult = DialogResult.No;

        bool isclose = true;
        void btn_ok_Click(object? sender, EventArgs e)
        {
            if (config.OnOk == null) DialogResult = DialogResult.OK;
            else
            {
                if (btn_ok == null) return;
                isclose = false;
                btn_ok.Loading = true;
                bool DisableCancel = false;
                if (config.LoadingDisableCancel && btn_no != null)
                {
                    btn_no.Enabled = false;
                    DisableCancel = true;
                }
                ITask.Run(() =>
                {
                    bool result = false;
                    try
                    {
                        result = config.OnOk(config);
                    }
                    catch { }
                    isclose = true;
                    btn_ok.Loading = false;
                    // 等待窗口关闭
                    if (IsHandleCreated && !IsDisposed)
                    {
                        if (result)
                        {
                            System.Threading.Thread.Sleep(10);
                            BeginInvoke(() =>
                            {
                                if (IsHandleCreated && !IsDisposed) DialogResult = DialogResult.OK;
                            });
                        }
                        else if (DisableCancel && btn_no != null)
                        {
                            BeginInvoke(() =>
                            {
                                if (btn_no.IsHandleCreated && !btn_no.IsDisposed) btn_no.Enabled = true;
                            });
                        }
                    }
                });
            }
        }

        Button? btn_ok, btn_no;

        #endregion

        public void SetOkText(string value)
        {
            if (btn_ok == null) return;
            btn_ok.Text = value;
        }

        public void SetCancelText(string? value)
        {
            if (btn_no == null)
            {
                if (panel_main == null) return;
                if (!string.IsNullOrEmpty(value))
                {
                    btn_no = new Button
                    {
                        AutoSizeMode = TAutoSize.Width,
                        BorderWidth = 1F,
                        Dock = DockStyle.Right,
                        Location = new Point(240, 0),
                        Name = "btn_no",
                        Size = new Size(64, config.BtnHeight),
                        TabIndex = 1,
                        Text = value
                    };
                    btn_no.Click += btn_no_Click;
                    if (config.CancelFont != null) btn_no.Font = config.CancelFont;
                    panel_main.Controls.Add(btn_no);
                    CancelButton = btn_no;
                }
                return;
            }
            if (string.IsNullOrEmpty(value)) btn_no.Visible = false;
            else
            {
                btn_no.Text = value;
                btn_no.Visible = true;
            }
        }

        #region 主题

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AddListener();
        }

        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.THEME:
                    BackColor = Colour.BgElevated.Get(nameof(AntdUI.Modal), config.ColorScheme);
                    ForeColor = Colour.TextBase.Get(nameof(AntdUI.Modal), config.ColorScheme);
                    break;
            }
        }

        #endregion
    }

    internal interface LayeredFormAsynLoad
    {
        bool IsLoad { get; set; }
        Action? LoadCompleted { get; set; }
    }
}
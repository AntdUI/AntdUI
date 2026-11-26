// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Demo
{
    public class Module : UserControl
    {
        static Font FontHeader = new Font("Microsoft YaHei UI", 12F), FontDesc = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);

        internal string Id;
        AntdUI.BaseForm form;
        readonly List<ViewPage> modules = new List<ViewPage>();
        bool thumbnailsLoaded;

        AntdUI.VirtualPanel virtualPanel;
        AntdUI.In.Panel vpanel;
        public Module(AntdUI.BaseForm _form, string id, string key, string desc, List<ViewPage> pages)
        {
            Id = id;
            form = _form;
            modules = pages;

            var panel = new AntdUI.In.Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                TabIndex = 1
            };
            var header = new AntdUI.PageHeader
            {
                Text = id + " " + key,
                Description = desc,
                LocalizationText = id,
                LocalizationDescription = id + ".Description",
                Dock = DockStyle.Top,
                Font = FontHeader,
                Name = "header1",
                Padding = new Padding(0, 0, 0, 10),
                Size = new Size(845, 74),
                TabIndex = 0,
                UseTitleFont = true
            };
            virtualPanel = new AntdUI.VirtualPanel
            {
                Dock = DockStyle.Fill,
                Font = FontDesc,
                Padding = new Padding(8),
                JustifyContent = AntdUI.TJustifyContent.Center,
                Waterfall = true
            };
            vpanel = new AntdUI.In.Panel
            {
                Dock = DockStyle.Fill,
                Visible = false
            };
            Controls.AddRange(vpanel, virtualPanel);
            Controls.AddRange(panel, header);
            virtualPanel.ItemClick += virtualPanel_ItemClick;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (thumbnailsLoaded) return;
            thumbnailsLoaded = true;
            BeginInvoke(new Action(LoadThumbnails));
        }

        /// <summary>
        /// 加载全部子页面并对每个子模块截图，截图作为虚拟容器的卡片图
        /// </summary>
        void LoadThumbnails()
        {
            virtualPanel.PauseLayout = true;
            foreach (var module in modules)
            {
                var control = module.Control;
                control.BackColor = BackColor;
                EnsureDpi(module);
                control.CreateControl();
                control.PerformLayout();
                module.Image = AntdUI.Helper.CaptureControl(control);
                virtualPanel.Items.Add(new VItem(module));
            }
            virtualPanel.PauseLayout = false;
            virtualPanel.Invalidate();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            if (thumbnailsLoaded)
            {
                foreach (var it in modules) it.Control.BackColor = BackColor;
            }
        }

        void virtualPanel_ItemClick(object sender, AntdUI.VirtualItemEventArgs e)
        {
            if (e.Item is VItem item) OpenViewPage(item.Data);
        }

        /// <summary>
        /// 子模块不在总页控件树里，Overview.AutoDpi 不会缩放它们；这里按当前窗体 DPI 补一次缩放
        /// </summary>
        void EnsureDpi(ViewPage module)
        {
            float dpi = form.Dpi;
            if (Math.Abs(dpi - module.Dpi) < 0.001F) return;
            AntdUI.Helper.DpiAuto(dpi / module.Dpi, module.Control);
            module.Dpi = dpi;
        }

        void OpenViewPage(ViewPage module)
        {
            var module_current = module.Control;
            module_current.Dock = DockStyle.Fill;
            vpanel.Controls.Add(module_current);
            vpanel.Visible = true;
        }

        public bool Back()
        {
            if (vpanel.Visible)
            {
                vpanel.Controls.Clear();
                vpanel.Visible = false;
                return true;
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            foreach (var it in modules) it.Control.Dispose();
        }

        #region 渲染

        class VItem : AntdUI.VirtualItem
        {
            public ViewPage Data { get; private set; }
            public VItem(ViewPage data) { Data = data; }

            Rectangle rect_bor, rect_img, rect_text;
            public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = g.Dpi;
                int sp = (int)(30 * dpi), titleH = (int)(40 * dpi), p = (int)(4 * dpi), pad = (int)(10 * dpi), pad2 = pad * 2, pad_img = (int)(18 * dpi), pad_img2 = pad_img * 2,
                   w = e.Panel.Width - sp, hc = (int)(Data.H * dpi) + pad, h = hc + titleH + pad2 + pad_img2;
                rect_text = new Rectangle(pad + p, pad, w - pad2, titleH);
                rect_bor = new Rectangle(pad, pad + titleH, w - pad2, h - pad2 - titleH);
                rect_img = new Rectangle(rect_bor.X + pad_img, rect_bor.Y + pad_img, rect_bor.Width - pad_img2, hc);
                return new Size(w, h);
            }

            public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                using (var fore = new SolidBrush(AntdUI.Style.Db.Text))
                {
                    g.String(AntdUI.Localization.Get(Data.Name, Data.Text), e.Panel.Font, fore, rect_text, AntdUI.FormatFlags.Left | AntdUI.FormatFlags.VerticalCenter);
                }
                if (Data.Image != null) g.Image(rect_img, Data.Image, AntdUI.TFit.Contain);
                using (var path = AntdUI.Helper.RoundPath(rect_bor, e.Radius))
                {
                    if (Hover)
                    {
                        int sp = (int)(3 * g.Dpi), ps = (int)(12 * g.Dpi);
                        g.Fill(AntdUI.Style.Db.Primary, new Rectangle(rect_text.X - sp * 2, rect_text.Y + ps, sp, rect_text.Height - ps * 2));
                        using (var pen = new Pen(AntdUI.Style.Db.Primary, (int)Math.Round(g.Dpi * 2)))
                        {
                            g.Draw(pen, path);
                        }
                    }
                    else
                    {
                        using (var pen = new Pen(AntdUI.Style.Db.BorderColor, (int)Math.Round(g.Dpi)))
                        {
                            g.Draw(pen, path);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
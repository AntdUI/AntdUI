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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    internal class LayeredFormPreview : ILayeredFormOpacity
    {
        int Radius = 0, Bor = 0;
        bool HasBor = false;
        Form form;

        PreBtns[] btns;
        Preview.Config config;
        public LayeredFormPreview(Preview.Config _config)
        {
            maxalpha = 255;
            config = _config;
            form = _config.Form;
            Font = form.Font;
            TopMost = _config.Form.TopMost;
            if (form.WindowState != FormWindowState.Maximized)
            {
                if (form is BorderlessForm borderless) Radius = (int)(borderless.Radius * Config.Dpi);
                else if (Helper.OSVersionWin11) Radius = (int)(8 * Config.Dpi); //Win11
                if (form is Window || form is FormNoBar)
                {
                    //无边框处理
                }
                else if (form.FormBorderStyle != FormBorderStyle.None)
                {
                    HasBor = true;
                    Bor = (int)(7 * Config.Dpi);
                }
            }
            if (form is Window window)
            {
                SetSize(window.Size);
                SetLocation(window.Location);
                Size = window.Size;
                Location = window.Location;
            }
            else
            {
                SetSize(form.Size);
                SetLocation(form.Location);
                Size = form.Size;
                Location = form.Location;
            }
            PageSize = config.Content.Count;
            string flip = "<svg viewBox=\"64 64 896 896\"><path d=\"M847.9 592H152c-4.4 0-8 3.6-8 8v60c0 4.4 3.6 8 8 8h605.2L612.9 851c-4.1 5.2-.4 13 6.3 13h72.5c4.9 0 9.5-2.2 12.6-6.1l168.8-214.1c16.5-21 1.6-51.8-25.2-51.8zM872 356H266.8l144.3-183c4.1-5.2.4-13-6.3-13h-72.5c-4.9 0-9.5 2.2-12.6 6.1L150.9 380.2c-16.5 21-1.6 51.8 25.1 51.8h696c4.4 0 8-3.6 8-8v-60c0-4.4-3.6-8-8-8z\"></path></svg>";

            var btnwiths = new PreBtns[] {
                new PreBtns("@t_flipY",flip.Insert(28," transform=\"rotate(90),translate(0 -100%)\"")),
                new PreBtns("@t_flipX",flip),
                new PreBtns("@t_rotateL","<svg viewBox=\"64 64 896 896\"><defs><style></style></defs><path d=\"M672 418H144c-17.7 0-32 14.3-32 32v414c0 17.7 14.3 32 32 32h528c17.7 0 32-14.3 32-32V450c0-17.7-14.3-32-32-32zm-44 402H188V494h440v326z\"></path><path d=\"M819.3 328.5c-78.8-100.7-196-153.6-314.6-154.2l-.2-64c0-6.5-7.6-10.1-12.6-6.1l-128 101c-4 3.1-3.9 9.1 0 12.3L492 318.6c5.1 4 12.7.4 12.6-6.1v-63.9c12.9.1 25.9.9 38.8 2.5 42.1 5.2 82.1 18.2 119 38.7 38.1 21.2 71.2 49.7 98.4 84.3 27.1 34.7 46.7 73.7 58.1 115.8a325.95 325.95 0 016.5 140.9h74.9c14.8-103.6-11.3-213-81-302.3z\"></path></svg>"),
                new PreBtns("@t_rotateR","<svg viewBox=\"64 64 896 896\"><defs><style></style></defs><path d=\"M480.5 251.2c13-1.6 25.9-2.4 38.8-2.5v63.9c0 6.5 7.5 10.1 12.6 6.1L660 217.6c4-3.2 4-9.2 0-12.3l-128-101c-5.1-4-12.6-.4-12.6 6.1l-.2 64c-118.6.5-235.8 53.4-314.6 154.2A399.75 399.75 0 00123.5 631h74.9c-.9-5.3-1.7-10.7-2.4-16.1-5.1-42.1-2.1-84.1 8.9-124.8 11.4-42.2 31-81.1 58.1-115.8 27.2-34.7 60.3-63.2 98.4-84.3 37-20.6 76.9-33.6 119.1-38.8z\"></path><path d=\"M880 418H352c-17.7 0-32 14.3-32 32v414c0 17.7 14.3 32 32 32h528c17.7 0 32-14.3 32-32V450c0-17.7-14.3-32-32-32zm-44 402H396V494h440v326z\"></path></svg>"),
                new PreBtns("@t_zoomOut","<svg viewBox=\"64 64 896 896\"><path d=\"M637 443H325c-4.4 0-8 3.6-8 8v60c0 4.4 3.6 8 8 8h312c4.4 0 8-3.6 8-8v-60c0-4.4-3.6-8-8-8zm284 424L775 721c122.1-148.9 113.6-369.5-26-509-148-148.1-388.4-148.1-537 0-148.1 148.6-148.1 389 0 537 139.5 139.6 360.1 148.1 509 26l146 146c3.2 2.8 8.3 2.8 11 0l43-43c2.8-2.7 2.8-7.8 0-11zM696 696c-118.8 118.7-311.2 118.7-430 0-118.7-118.8-118.7-311.2 0-430 118.8-118.7 311.2-118.7 430 0 118.7 118.8 118.7 311.2 0 430z\"></path></svg>"),
                new PreBtns("@t_zoomIn","<svg viewBox=\"64 64 896 896\"><path d=\"M637 443H519V309c0-4.4-3.6-8-8-8h-60c-4.4 0-8 3.6-8 8v134H325c-4.4 0-8 3.6-8 8v60c0 4.4 3.6 8 8 8h118v134c0 4.4 3.6 8 8 8h60c4.4 0 8-3.6 8-8V519h118c4.4 0 8-3.6 8-8v-60c0-4.4-3.6-8-8-8zm284 424L775 721c122.1-148.9 113.6-369.5-26-509-148-148.1-388.4-148.1-537 0-148.1 148.6-148.1 389 0 537 139.5 139.6 360.1 148.1 509 26l146 146c3.2 2.8 8.3 2.8 11 0l43-43c2.8-2.7 2.8-7.8 0-11zM696 696c-118.8 118.7-311.2 118.7-430 0-118.7-118.8-118.7-311.2 0-430 118.8-118.7 311.2-118.7 430 0 118.7 118.8 118.7 311.2 0 430z\"></path></svg>"),
            };
            if (config.Btns == null || config.Btns.Length == 0) btns = btnwiths;
            else
            {
                var btntmp = new List<PreBtns>(config.Btns.Length + btnwiths.Length);
                foreach (var it in config.Btns) btntmp.Add(new PreBtns(it.Name, it.IconSvg, it.Tag));
                btntmp.AddRange(btnwiths);
                btns = btntmp.ToArray();
            }
        }

        int PageSize = 0;

        string closesvg = "<svg fill-rule=\"evenodd\" viewBox=\"64 64 896 896\"><path d=\"M799.86 166.31c.02 0 .04.02.08.06l57.69 57.7c.04.03.05.05.06.08a.12.12 0 010 .06c0 .03-.02.05-.06.09L569.93 512l287.7 287.7c.04.04.05.06.06.09a.12.12 0 010 .07c0 .02-.02.04-.06.08l-57.7 57.69c-.03.04-.05.05-.07.06a.12.12 0 01-.07 0c-.03 0-.05-.02-.09-.06L512 569.93l-287.7 287.7c-.04.04-.06.05-.09.06a.12.12 0 01-.07 0c-.02 0-.04-.02-.08-.06l-57.69-57.7c-.04-.03-.05-.05-.06-.07a.12.12 0 010-.07c0-.03.02-.05.06-.09L454.07 512l-287.7-287.7c-.04-.04-.05-.06-.06-.09a.12.12 0 010-.07c0-.02.02-.04.06-.08l57.7-57.69c.03-.04.05-.05.07-.06a.12.12 0 01.07 0c.03 0 .05.02.09.06L512 454.07l287.7-287.7c.04-.04.06-.05.09-.06a.12.12 0 01.07 0z\"></path></svg>",
            leftsvg = "<svg viewBox=\"64 64 896 896\"><path d=\"M724 218.3V141c0-6.7-7.7-10.4-12.9-6.3L260.3 486.8a31.86 31.86 0 000 50.3l450.8 352.1c5.3 4.1 12.9.4 12.9-6.3v-77.3c0-4.9-2.3-9.6-6.1-12.6l-360-281 360-281.1c3.8-3 6.1-7.7 6.1-12.6z\"></path></svg>",
            rightsvg = "<svg viewBox=\"64 64 896 896\"><path d=\"M765.7 486.8L314.9 134.7A7.97 7.97 0 00302 141v77.3c0 4.9 2.3 9.6 6.1 12.6l360 281.1-360 281.1c-3.9 3-6.1 7.7-6.1 12.6V883c0 6.7 7.7 10.4 12.9 6.3l450.8-352.1a31.96 31.96 0 000-50.4z\"></path></svg>";

        protected override void OnLoad(EventArgs e)
        {
            if (form is Window window)
            {
                SetSize(window.Size);
                SetLocation(window.Location);
                Size = window.Size;
                Location = window.Location;
            }
            else
            {
                SetSize(form.Size);
                SetLocation(form.Location);
                Size = form.Size;
                Location = form.Location;
            }
            form.LocationChanged += Form_LSChanged;
            form.SizeChanged += Form_LSChanged;
            LoadImg();
            base.OnLoad(e);
        }

        private void Form_LSChanged(object? sender, EventArgs e)
        {
            if (form is Window window)
            {
                SetSize(window.Size);
                SetLocation(window.Location);
                Size = window.Size;
                Location = window.Location;
            }
            else
            {
                SetSize(form.Size);
                SetLocation(form.Location);
                Size = form.Size;
                Location = form.Location;
            }
            Paints();
        }

        protected override void Dispose(bool disposing)
        {
            form.LocationChanged -= Form_LSChanged;
            form.SizeChanged -= Form_LSChanged;
            temp?.Dispose();
            temp = null;
            base.Dispose(disposing);
        }

        #region 渲染

        #region 渲染图片

        Image? Img = null;
        int SelectIndex = 0;
        Size ImgSize = new Size();
        void LoadImg()
        {
            autoDpi = true;
            Img = config.Content[SelectIndex];
            ImgSize = Img.Size;
            FillScaleImg();
        }

        #region 缩放

        bool autoDpi = true;
        PointF rect_img_oxy;
        RectangleF rect_img_dpi;
        float offsetX = 0, offsetY = 0;
        float _dpi = 1F;
        float Dpi
        {
            get => _dpi;
            set
            {
                if (value < 0.06) { value = 0.06F; }
                else if (value > _dpi && _dpi < 1F && value > 1F) value = 1F;
                _dpi = value;
                rect_img_dpi = ScaleImg(rect_read, _dpi);
            }
        }

        RectangleF ScaleImg(Rectangle rect, float dpi)
        {
            float width = ImgSize.Width * dpi, height = ImgSize.Height * dpi;
            rect_img_oxy = new PointF((rect.Width - width) / 2, (rect.Height - height) / 2);
            if (width < rect.Width || height < rect.Height)
            {
                if (width < rect.Width && height < rect.Height)
                {
                    //小
                    if (offsetX < -rect_img_oxy.X) offsetX = -rect_img_oxy.X;
                    else if (offsetX > rect_img_oxy.X) offsetX = rect_img_oxy.X;

                    if (offsetY < -rect_img_oxy.Y) offsetY = -rect_img_oxy.Y;
                    else if (offsetY > rect_img_oxy.Y) offsetY = rect_img_oxy.Y;
                }
                else if (width < rect.Width)
                {
                    offsetX = 0;
                    if (offsetY > -rect_img_oxy.Y) offsetY = -rect_img_oxy.Y;
                    else if (offsetY < rect_img_oxy.Y) offsetY = rect_img_oxy.Y;
                }
                else
                {
                    offsetY = 0;
                    if (offsetX > -rect_img_oxy.X) offsetX = -rect_img_oxy.X;
                    else if (offsetX < rect_img_oxy.X) offsetX = rect_img_oxy.X;
                }
            }
            else
            {
                if (offsetX < rect_img_oxy.X) offsetX = rect_img_oxy.X;
                else if (offsetX > -rect_img_oxy.X) offsetX = -rect_img_oxy.X;

                if (offsetY < rect_img_oxy.Y) offsetY = rect_img_oxy.Y;
                else if (offsetY > -rect_img_oxy.Y) offsetY = -rect_img_oxy.Y;
            }
            return new RectangleF(offsetX + rect_img_oxy.X, offsetY + rect_img_oxy.Y, width, height);
        }

        void FillScaleImg()
        {
            if (autoDpi)
            {
                var rect = rect_read;
                float DpiX = (float)((rect.Width * 1.0) / (ImgSize.Width * 1.0)), DpiY = (float)((rect.Height * 1.0) / (ImgSize.Height * 1.0));
                if (DpiX > 1 && DpiY > 0) Dpi = 1F;
                else if (ImgSize.Width > ImgSize.Height)
                {
                    if (rect.Width > rect.Height) Dpi = DpiX;
                    else Dpi = DpiY;
                }
                else
                {
                    if (rect.Width > rect.Height) Dpi = DpiY;
                    else Dpi = (float)((rect.Width * 1.0) / (ImgSize.Height * 1.0));
                }
            }
        }

        #endregion

        #endregion

        void Paints()
        {
            temp?.Dispose();
            temp = null;
            Print();
        }
        Bitmap? temp = null;
        public override Bitmap PrintBit()
        {
            if (temp == null || (temp.Width != TargetRect.Width || temp.Height != TargetRect.Height))
            {
                temp?.Dispose();
                temp = new Bitmap(TargetRect.Width, TargetRect.Height);
                using (var g = Graphics.FromImage(temp).High())
                {
                    using (var brush = new SolidBrush(Color.FromArgb(115, 0, 0, 0)))
                    {
                        if (Radius > 0)
                        {
                            using (var path = rect_read.RoundPath(Radius))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        else g.FillRectangle(brush, rect_read);
                    }

                    if (Img != null) g.DrawImage(Img, rect_img_dpi, new RectangleF(0, 0, ImgSize.Width, ImgSize.Height), GraphicsUnit.Pixel);

                    using (var path = rect_panel.RoundPath(rect_panel.Height))
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(26, 0, 0, 0)))
                        {
                            g.FillPath(brush, path);
                            PaintBtn(g, brush, rect_close, rect_close_icon, closesvg, hoverClose, true);
                            if (PageSize > 1)
                            {
                                PaintBtn(g, brush, rect_left, rect_left_icon, leftsvg, hoverLeft, enabledLeft);
                                PaintBtn(g, brush, rect_right, rect_right_icon, rightsvg, hoverRight, enabledRight);
                            }
                        }
                    }
                    foreach (var it in btns)
                    {
                        using (var bmp = SvgExtend.GetImgExtend(it.svg, it.rect, it.hover ? colorHover : colorDefault))
                        {
                            if (bmp != null)
                            {
                                if (it.enabled) g.DrawImage(bmp, it.rect);
                                else g.DrawImage(bmp, it.rect, 0.3F);
                            }
                        }
                    }

                }
            }
            return (Bitmap)temp.Clone();
        }

        void PaintBtn(Graphics g, SolidBrush brush, Rectangle rect, Rectangle rect_ico, string svg, bool hover, bool enabled)
        {
            using (var bmp = SvgExtend.GetImgExtend(svg, rect_ico, Color.White))
            {
                if (bmp != null)
                {
                    if (hover)
                    {
                        using (var brush_hover = new SolidBrush(Color.FromArgb(51, 0, 0, 0)))
                        { g.FillEllipse(brush_hover, rect); }
                    }
                    else g.FillEllipse(brush, rect);
                    if (enabled) g.DrawImage(bmp, rect_ico);
                    else g.DrawImage(bmp, rect_ico, 0.3F);
                }
            }
        }

        readonly Color colorDefault = Color.FromArgb(166, 255, 255, 255), colorHover = Color.FromArgb(217, 255, 255, 255);

        internal class PreBtns
        {
            public PreBtns(string _id, string _svg)
            {
                id = _id;
                svg = _svg;
            }
            public PreBtns(string _id, string _svg, object? _tag) : this(_id, _svg)
            {
                tag = _tag;
                div = true;
            }
            public string id { get; set; }
            public string svg { get; set; }
            public bool div { get; set; }
            public object? tag { get; set; }
            public Rectangle Rect { get; set; }
            public Rectangle rect { get; set; }

            public bool hover { get; set; }
            public bool enabled { get; set; } = true;
            public bool mdown { get; set; }
        }

        #endregion

        #region 坐标

        Rectangle rect_read, rect_left, rect_left_icon, rect_right, rect_right_icon, rect_close, rect_close_icon, rect_panel;
        protected override void OnSizeChanged(EventArgs e)
        {
            if (btns == null) return;
            if (HasBor) rect_read = new Rectangle(Bor, 0, TargetRect.Width - Bor * 2, TargetRect.Height - Bor);
            else rect_read = TargetRectXY;

            int btn_height = (int)(46 * Config.Dpi), lr_size = (int)(40 * Config.Dpi), btn_width = (int)(42 * Config.Dpi),
                padding = (int)(24 * Config.Dpi), padding_lr = (int)(12 * Config.Dpi), padding_buttom = (int)(32 * Config.Dpi),
                icon_size = (int)(18 * Config.Dpi);
            rect_close = new Rectangle(rect_read.Right - padding_buttom - btn_width, rect_read.Y + padding_buttom, btn_width, btn_width);
            rect_close_icon = GetCentered(rect_close, icon_size);

            if (PageSize > 1)
            {
                rect_left = new Rectangle(rect_read.X + padding_lr, rect_read.Y + (rect_read.Height - lr_size) / 2, lr_size, lr_size);
                rect_left_icon = GetCentered(rect_left, icon_size);

                rect_right = new Rectangle(rect_read.Right - padding_lr - lr_size, rect_read.Y + (rect_read.Height - lr_size) / 2, lr_size, lr_size);
                rect_right_icon = GetCentered(rect_right, icon_size);
            }

            int w = (btn_width * btns.Length - 1) + padding * 2, x = rect_read.X + (rect_read.Width - w) / 2, y = rect_read.Bottom - padding_buttom - btn_height;
            rect_panel = new Rectangle(x, y, w, btn_height);
            x += padding;
            foreach (var it in btns)
            {
                it.Rect = new Rectangle(x, y, btn_width, btn_height);
                it.rect = GetCentered(it.Rect, icon_size);
                x += btn_width;
            }
            base.OnSizeChanged(e);
        }

        Rectangle GetCentered(Rectangle rect, int size)
        {
            int xy = (rect.Width - size) / 2;
            return new Rectangle(rect.X + xy, rect.Y + xy, size, size);
        }

        #endregion

        #region 鼠标

        bool hoverClose = false, hoverLeft = false, hoverRight = false;
        bool enabledLeft
        {
            get => SelectIndex > 0;
        }
        bool enabledRight
        {
            get => SelectIndex < PageSize - 1;
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (Img != null)
            {
                autoDpi = false;
                if (e.Delta > 0)
                {
                    Dpi += 0.1F;
                    SetBtnEnabled("@t_zoomOut", true);
                }
                else
                {
                    Dpi -= 0.1F;
                    SetBtnEnabled("@t_zoomOut", Dpi >= 0.06);
                }
                Paints();
            }
            base.OnMouseWheel(e);
        }

        void SetBtnEnabled(string id, bool enabled)
        {
            foreach (var it in btns)
            {
                if (it.id == id) it.enabled = enabled;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (moveImg)
            {
                if ((offsetXOld != e.Location.X && offsetYOld != e.Location.Y) || moveImging)
                {
                    moveImging = true;
                    offsetX = offsetXOld + e.Location.X - movePos.X;
                    offsetY = offsetYOld + e.Location.Y - movePos.Y;
                    Dpi = _dpi;
                    Paints();
                    return;
                }
            }
            if (btns == null) return;
            int count = 0, hand = 0;
            if (rect_close.Contains(e.Location))
            {
                hand++;
                if (!hoverClose) { hoverClose = true; count++; }
            }
            else
            {
                if (hoverClose) { hoverClose = false; count++; }
            }
            if (PageSize > 1)
            {
                if (enabledLeft && rect_left.Contains(e.Location))
                {
                    hand++;
                    if (!hoverLeft) { hoverLeft = true; count++; }
                }
                else
                {
                    if (hoverLeft) { hoverLeft = false; count++; }
                }
                if (enabledRight && rect_right.Contains(e.Location))
                {
                    hand++;
                    if (!hoverRight) { hoverRight = true; count++; }
                }
                else
                {
                    if (hoverRight) { hoverRight = false; count++; }
                }
            }
            foreach (var it in btns)
            {
                if (it.enabled && it.Rect.Contains(e.Location))
                {
                    hand++;
                    if (!it.hover) { it.hover = true; count++; }
                }
                else
                {
                    if (it.hover) { it.hover = false; count++; }
                }
            }
            SetCursor(hand > 0);
            if (count > 0)
            {
                Paints();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                foreach (var it in btns)
                {
                    if (it.enabled && it.Rect.Contains(e.Location))
                    {
                        it.mdown = true;
                        return;
                    }
                }
                if (rect_img_dpi.Contains(e.Location))
                {
                    if (rect_img_dpi.Width < Width && rect_img_dpi.Height < Height)
                    {
                        //小
                    }
                    else
                    {
                        movePos = e.Location;
                        offsetXOld = offsetX;
                        offsetYOld = offsetY;
                        moveImging = false;
                        moveImg = true;
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (moveImg)
            {
                moveImg = false;
                if (moveImging)
                {
                    moveImging = false;
                    base.OnMouseUp(e);
                    return;
                }
            }
            foreach (var it in btns)
            {
                if (it.mdown)
                {
                    if (it.Rect.Contains(e.Location))
                    {
                        switch (it.id)
                        {
                            case "@t_flipY":
                                if (Img != null)
                                {
                                    Img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                    Paints();
                                }
                                break;
                            case "@t_flipX":
                                if (Img != null)
                                {
                                    Img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                    Paints();
                                }
                                break;
                            case "@t_rotateL":
                                if (Img != null)
                                {
                                    float old = _dpi;
                                    bool oldautoDpi = autoDpi;
                                    Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                    ImgSize = Img.Size;
                                    autoDpi = true;
                                    FillScaleImg();
                                    Dpi = old;
                                    autoDpi = oldautoDpi;
                                    Paints();
                                }
                                break;
                            case "@t_rotateR":
                                if (Img != null)
                                {
                                    float old = _dpi;
                                    bool oldautoDpi = autoDpi;
                                    Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                    ImgSize = Img.Size;
                                    autoDpi = true;
                                    FillScaleImg();
                                    Dpi = old;
                                    autoDpi = oldautoDpi;
                                    Paints();
                                }
                                break;
                            case "@t_zoomOut":
                                Dpi -= 0.1F;
                                SetBtnEnabled("@t_zoomOut", Dpi >= 0.06);
                                Paints();
                                break;
                            case "@t_zoomIn":
                                Dpi += 0.1F;
                                SetBtnEnabled("@t_zoomOut", true);
                                Paints();
                                break;
                            default:
                                config.OnBtns?.Invoke(it.id, it.tag);
                                break;
                        }
                    }
                    it.mdown = false;
                    return;
                }
            }
            if (rect_close.Contains(e.Location))
            {
                IClose(); return;
            }
            if (PageSize > 1)
            {
                if (enabledLeft && rect_left.Contains(e.Location))
                {
                    SelectIndex--;
                    LoadImg();
                    Paints();
                    return;
                }
                if (enabledRight && rect_right.Contains(e.Location))
                {
                    SelectIndex++;
                    LoadImg();
                    Paints();
                    return;
                }
            }
            if (!rect_img_dpi.Contains(e.Location)) IClose();
            base.OnMouseUp(e);
        }

        bool moveImg = false, moveImging = false;
        Point movePos;
        float offsetXOld = 0, offsetYOld = 0;

        #endregion
    }
}
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

            var btnwiths = new PreBtns[] {
                new PreBtns("@t_flipY",SvgDb.IcoFlip.Insert(28," transform=\"rotate(90),translate(0 -100%)\"")),
                new PreBtns("@t_flipX",SvgDb.IcoFlip),
                new PreBtns("@t_rotateL",SvgDb.IcoRotateLeft),
                new PreBtns("@t_rotateR",SvgDb.IcoRotateRight),
                new PreBtns("@t_zoomOut",SvgDb.IcoZoomOut),
                new PreBtns("@t_zoomIn",SvgDb.IcoZoomIn),
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
                            PaintBtn(g, brush, rect_close, rect_close_icon, SvgDb.IcoClose, hoverClose, true);
                            if (PageSize > 1)
                            {
                                PaintBtn(g, brush, rect_left, rect_left_icon, SvgDb.IcoLeft, hoverLeft, enabledLeft);
                                PaintBtn(g, brush, rect_right, rect_right_icon, SvgDb.IcoRight, hoverRight, enabledRight);
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
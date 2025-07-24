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
    internal class LayeredFormPreview : ILayeredFormOpacity
    {
        int Radius = 0, Bor = 0;
        bool HasBor = false;
        Form form;

        PreBtns[] btns;
        Preview.Config config;
        public LayeredFormPreview(Preview.Config _config)
        {
            config = _config;
            form = _config.Form;
            Font = form.Font;
            TopMost = _config.Form.TopMost;
            HasBor = form.FormFrame(out Radius, out Bor);
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
            PageSize = config.ContentCount;
            SelectIndex = config.SelectIndex;
            int len = 8;
            if (config.Btns != null && config.Btns.Length > 0) len += config.Btns.Length;
            var btnwiths = new List<PreBtns>(len)
            {
                new PreBtns("@t_flipY",SvgDb.Custom["SwapOutlined"].Insert(28," transform=\"rotate(90),translate(0 -100%)\"")),
                new PreBtns("@t_flipX","SwapOutlined"),
                new PreBtns("@t_rotateL","RotateLeftOutlined"),
                new PreBtns("@t_rotateR","RotateRightOutlined"),
                new PreBtns("@t_zoomOut","ZoomOutOutlined"),
                new PreBtns("@t_zoomIn","ZoomInOutlined"),
            };
            if (config.Content is IList<Preview.ImageTextContent>) btnwiths.Add(new PreBtns("@t_copyText", SvgDb.Custom["CopyOutlined"]));//这里是如果存在文字，则添加一个可以复制文本的按钮
            if (config.Btns != null && config.Btns.Length > 0)
            {
                foreach (var it in config.Btns) btnwiths.Add(new PreBtns(it.Name, it.IconSvg, it.Tag));
            }
            btns = btnwiths.ToArray();
        }

        public override string name => nameof(Preview);

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
            if (OS.Win7OrLower) Select();
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
            Print();
        }

        protected override void Dispose(bool disposing)
        {
            form.LocationChanged -= Form_LSChanged;
            form.SizeChanged -= Form_LSChanged;
            base.Dispose(disposing);
        }

        #region 渲染

        #region 渲染图片

        bool loading = false;
        /// <summary>
        /// 加载状态
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Loading
        {
            get => loading;
            set
            {
                if (loading == value) return;
                loading = value;
                Print();
            }
        }

        string? LoadingProgressStr;
        float _value = -1F;
        /// <summary>
        /// 加载进度
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float LoadingProgress
        {
            get => _value;
            set
            {
                if (_value == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                _value = value;
                if (loading) Print();
            }
        }

        Image? Img;
        int SelectIndex = 0;
        object? SelectValue;
        Size ImgSize = new Size();
        void LoadImg()
        {
            autoDpi = true;
            if (config.Content is IList<Image> images)
            {
                Img = images[SelectIndex];
                ImgSize = Img.Size;
                FillScaleImg();
            }
            else if (config.Content is IList<Preview.ImageTextContent> imgTxtList)
            {
                Img = imgTxtList[SelectIndex].Image;
                Tag = imgTxtList[SelectIndex];
                ImgSize = Img.Size;
                FillScaleImg();
            }
            else if (config.Content is object[] list && list[0] is IList<object> data)
            {
                if (list[1] is Func<int, object, Image?> call)
                {
                    Img?.Dispose();
                    SelectValue = data[SelectIndex];
                    Img = call.Invoke(SelectIndex, SelectValue);
                    if (Img == null)
                    {
                        Print();
                        return;
                    }
                    ImgSize = Img.Size;
                    FillScaleImg();
                }
                else if (list[1] is Func<int, object, Action<float, string?>, Image?> callprog)
                {
                    LoadingProgressStr = null;
                    _value = -1F;
                    Loading = true;
                    int selectIndex = SelectIndex;
                    SelectValue = data[SelectIndex];
                    DateTime now = DateTime.Now, now2 = DateTime.Now;
                    ITask.Run(() =>
                    {
                        var img = callprog.Invoke(SelectIndex, SelectValue, (prog, progstr) =>
                        {
                            LoadingProgressStr = progstr;
                            LoadingProgress = prog;
                        });
                        now2 = DateTime.Now;
                        if (selectIndex == SelectIndex)
                        {
                            if (img == null)
                            {
                                Img?.Dispose();
                                Img = null;
                                return;
                            }
                            LoadingProgressStr = null;
                            Img?.Dispose();
                            Img = img;
                            ImgSize = Img.Size;
                            FillScaleImg();
                        }
                        else img?.Dispose();
                    }, () =>
                    {
                        if (selectIndex == SelectIndex)
                        {
                            Loading = false;
                            if ((now2 - now).TotalMilliseconds < 100)
                            {
                                System.Threading.Thread.Sleep(100);
                                if (selectIndex == SelectIndex) Print();
                            }
                        }
                    });
                }
            }
            else
            {
                Img = null;
                Print();
            }
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

        readonly StringFormat s_f = Helper.SF_NoWrap();
        public override Bitmap PrintBit()
        {
            var original_bmp = new Bitmap(TargetRect.Width, TargetRect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                using (var brush = new SolidBrush(Color.FromArgb(115, 0, 0, 0)))
                {
                    if (Radius > 0)
                    {
                        using (var path = rect_read.RoundPath(Radius))
                        {
                            g.Fill(brush, path);
                        }
                    }
                    else g.Fill(brush, rect_read);
                }

                if (Img == null)
                {
                    if (LoadingProgressStr != null) PaintLoading(g, true);
                }
                else
                {
                    g.Image(Img, rect_img_dpi, new RectangleF(0, 0, ImgSize.Width, ImgSize.Height), GraphicsUnit.Pixel);
                    if (loading) PaintLoading(g);
                }
                using (var path = rect_panel.RoundPath(rect_panel.Height))
                {
                    using (var brush = new SolidBrush(Color.FromArgb(26, 0, 0, 0)))
                    {
                        g.Fill(brush, path);
                        PaintBtn(g, brush, rect_close, rect_close_icon, SvgDb.IcoClose, hoverClose, true);
                        if (PageSize > 1)
                        {
                            PaintBtn(g, brush, rect_left, rect_left_icon, "LeftOutlined", hoverLeft, enabledLeft);
                            PaintBtn(g, brush, rect_right, rect_right_icon, "RightOutlined", hoverRight, enabledRight);
                        }
                    }
                }
                foreach (var it in btns)
                {
                    using (var bmp = SvgExtend.GetImgExtend(it.svg, it.rect, it.hover ? colorHover : colorDefault))
                    {
                        if (bmp != null)
                        {
                            if (it.enabled) g.Image(bmp, it.rect);
                            else g.Image(bmp, it.rect, 0.3F);
                        }
                    }
                }

                if (Tag is Preview.ImageTextContent content && content.Text != null)
                {
                    // 测量文本大小
                    var size = g.MeasureText(content.Text, content.Font ?? Font);
                    using (var brush = new SolidBrush(content.ForeColor ?? Style.Db.Text))
                    using (var format = new StringFormat())
                    {
                        Rectangle textRect;
                        int width = TargetRect.Width, height = size.Height;

                        if (size.Width > TargetRect.Width)
                        {
                            format.FormatFlags = StringFormatFlags.LineLimit;
                            format.Trimming = StringTrimming.Word;
                            // 重新测量换行后的文本所需区域
                            size = g.MeasureText(content.Text, content.Font ?? Font, TargetRect.Width, format);

                            //重新测量后重新赋值矩形高度
                            height = Math.Min(size.Height, TargetRect.Height);
                            width = TargetRect.Width;
                        }

                        switch (content.TextAlign)
                        {
                            case ContentAlignment.TopLeft:
                                textRect = new Rectangle(0, 0, width, height);
                                format.Alignment = StringAlignment.Near;
                                break;
                            case ContentAlignment.TopCenter:
                                textRect = new Rectangle(0, 0, width, height);
                                format.Alignment = StringAlignment.Center;
                                break;
                            case ContentAlignment.TopRight:
                                textRect = new Rectangle(0, 0, width, height);
                                format.Alignment = StringAlignment.Far;
                                break;
                            case ContentAlignment.MiddleLeft:
                                textRect = new Rectangle(0, TargetRect.Height / 2, width, height);
                                format.Alignment = StringAlignment.Near;
                                break;
                            case ContentAlignment.MiddleCenter:
                                textRect = new Rectangle(0, TargetRect.Height / 2, width, height);
                                format.Alignment = StringAlignment.Center;
                                break;
                            case ContentAlignment.MiddleRight:
                                textRect = new Rectangle(0, TargetRect.Height / 2, width, height);
                                format.Alignment = StringAlignment.Far;
                                break;
                            case ContentAlignment.BottomLeft:
                                textRect = new Rectangle(0, TargetRect.Height - height, width, height);
                                format.Alignment = StringAlignment.Near;
                                break;
                            case ContentAlignment.BottomCenter:
                                textRect = new Rectangle(0, TargetRect.Height - height, width, height);
                                format.Alignment = StringAlignment.Center;
                                break;
                            case ContentAlignment.BottomRight:
                                textRect = new Rectangle(0, TargetRect.Height - height, width, height);
                                format.Alignment = StringAlignment.Far;
                                break;
                            default:
                                throw new Exception("什么鬼，你怎么可能进入这个异常");
                        }

                        format.LineAlignment = StringAlignment.Far;

                        g.DrawText(content.Text, content.Font ?? Font, brush, textRect, format);
                    }
                }
            }

            return original_bmp;
        }

        void PaintLoading(Canvas g, bool error = false)
        {
            var bor6 = 6F * Config.Dpi;
            int loading_size = (int)(40 * Config.Dpi);
            var rect_loading = new Rectangle(rect_read.X + (rect_read.Width - loading_size) / 2, rect_read.Y + (rect_read.Height - loading_size) / 2, loading_size, loading_size);
            Color color, bg;
            if (error)
            {
                bg = Colour.Error.Get("Preview");
                color = Colour.ErrorColor.Get("Preview");
            }
            else
            {
                bg = Colour.Primary.Get("Preview");
                color = Colour.PrimaryColor.Get("Preview");
            }
            g.DrawEllipse(Color.FromArgb(220, color), bor6, rect_loading);
            if (_value > -1)
            {
                using (var penpro = new Pen(bg, bor6))
                {
                    g.DrawArc(penpro, rect_loading, -90, 360F * _value);
                }
                if (LoadingProgressStr != null)
                {
                    rect_loading.Offset(0, loading_size);
                    g.String(LoadingProgressStr, Font, color, rect_loading, s_f);
                }
            }
            else if (LoadingProgressStr != null)
            {
                g.DrawEllipse(Colour.Error.Get("Preview"), bor6, rect_loading);
                rect_loading.Offset(0, loading_size);
                g.String(LoadingProgressStr, Font, Colour.ErrorColor.Get("Preview"), rect_loading, s_f);
            }
        }

        void PaintBtn(Canvas g, SolidBrush brush, Rectangle rect, Rectangle rect_ico, string svg, bool hover, bool enabled)
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
                    if (enabled) g.Image(bmp, rect_ico);
                    else g.Image(bmp, rect_ico, 0.3F);
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
            var rect_target = TargetRectXY;
            rect_read = HasBor ? new Rectangle(Bor, 0, rect_target.Width - Bor * 2, rect_target.Height - Bor) : rect_target;
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
        bool enabledLeft => SelectIndex > 0;
        bool enabledRight => SelectIndex < PageSize - 1;
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
                Print();
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
                if ((offsetXOld != e.X && offsetYOld != e.Y) || moveImging)
                {
                    moveImging = true;
                    offsetX = offsetXOld + e.X - movePos.X;
                    offsetY = offsetYOld + e.Y - movePos.Y;
                    Dpi = _dpi;
                    Print();
                    return;
                }
            }
            if (btns == null) return;
            int count = 0, hand = 0;
            if (rect_close.Contains(e.X, e.Y))
            {
                hand++;
                if (!hoverClose)
                {
                    hoverClose = true;
                    count++;
                }
            }
            else
            {
                if (hoverClose)
                {
                    hoverClose = false;
                    count++;
                }
            }
            if (PageSize > 1)
            {
                if (enabledLeft && rect_left.Contains(e.X, e.Y))
                {
                    hand++;
                    if (!hoverLeft)
                    {
                        hoverLeft = true;
                        count++;
                    }
                }
                else
                {
                    if (hoverLeft)
                    {
                        hoverLeft = false;
                        count++;
                    }
                }
                if (enabledRight && rect_right.Contains(e.X, e.Y))
                {
                    hand++;
                    if (!hoverRight)
                    {
                        hoverRight = true;
                        count++;
                    }
                }
                else
                {
                    if (hoverRight)
                    {
                        hoverRight = false;
                        count++;
                    }
                }
            }
            foreach (var it in btns)
            {
                if (it.enabled && it.Rect.Contains(e.X, e.Y))
                {
                    hand++;
                    if (!it.hover)
                    {
                        it.hover = true;
                        count++;
                    }
                }
                else
                {
                    if (it.hover)
                    {
                        it.hover = false;
                        count++;
                    }
                }
            }
            SetCursor(hand > 0);
            if (count > 0) Print();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                foreach (var it in btns)
                {
                    if (it.enabled && it.Rect.Contains(e.X, e.Y))
                    {
                        it.mdown = true;
                        return;
                    }
                }
                if (rect_img_dpi.Contains(e.X, e.Y))
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
                    if (it.Rect.Contains(e.X, e.Y))
                    {
                        switch (it.id)
                        {
                            case "@t_flipY":
                                if (Img != null)
                                {
                                    Img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                    Print();
                                }
                                break;
                            case "@t_flipX":
                                if (Img != null)
                                {
                                    Img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                    Print();
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
                                    Print();
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
                                    Print();
                                }
                                break;
                            case "@t_zoomOut":
                                Dpi -= 0.1F;
                                SetBtnEnabled("@t_zoomOut", Dpi >= 0.06);
                                Print();
                                break;
                            case "@t_zoomIn":
                                Dpi += 0.1F;
                                SetBtnEnabled("@t_zoomOut", true);
                                Print();
                                break;
                            case "@t_copyText":
                                if (Tag is Preview.ImageTextContent content && content.Text != null && content.Text.Length > 0)
                                {
                                    if (Helper.ClipboardSetText(this, content.Text))
                                    {
                                        Message.open(new Message.Config(this, "复制成功", TType.Success, Font)
                                        {
                                            ShowInWindow = true
                                        });
                                    }
                                }
                                break;
                            default:
                                config.OnBtns?.Invoke(it.id, new Preview.BtnEvent(SelectIndex, SelectValue, it.tag));
                                break;
                        }
                    }
                    it.mdown = false;
                    return;
                }
            }
            if (rect_close.Contains(e.X, e.Y))
            {
                IClose(); return;
            }
            if (PageSize > 1)
            {
                if (enabledLeft && rect_left.Contains(e.X, e.Y))
                {
                    int newIndex = SelectIndex - 1;
                    if (config.OnSelectIndexChanged == null) SelectIndex = newIndex;
                    else
                    {
                        if (config.OnSelectIndexChanged(newIndex)) SelectIndex = newIndex;
                        else return;
                    }
                    LoadImg();
                    Print();
                    return;
                }
                if (enabledRight && rect_right.Contains(e.X, e.Y))
                {
                    int newIndex = SelectIndex + 1;
                    if (config.OnSelectIndexChanged == null) SelectIndex = newIndex;
                    else
                    {
                        if (config.OnSelectIndexChanged(newIndex)) SelectIndex = newIndex;
                        else return;
                    }
                    LoadImg();
                    Print();
                    return;
                }
            }
            if (!rect_img_dpi.Contains(e.X, e.Y)) IClose();
            base.OnMouseUp(e);
        }

        bool moveImg = false, moveImging = false;
        Point movePos;
        float offsetXOld = 0, offsetYOld = 0;

        #endregion
    }
}
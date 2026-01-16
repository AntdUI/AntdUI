// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// ImagePreview 图片预览
    /// </summary>
    /// <remarks>常驻图片预览。</remarks>
    [Description("ImagePreview 图片预览")]
    [ToolboxItem(true)]
    [DefaultProperty("SelectIndex")]
    [DefaultEvent("SelectIndexChanged")]
    public class ImagePreview : IControl
    {
        #region 属性

        #region 数据

        private int selectIndex = 0;
        /// <summary>
        /// 选择序号
        /// </summary>
        [Description("选择序号"), Category("数据"), DefaultValue(0)]
        public int SelectIndex
        {
            get => selectIndex;
            set
            {
                if (selectIndex == value) return;
                if (items != null)
                {
                    if (items.ListExceed(value))
                    {
                        selectIndex = 0;
                        return;
                    }
                    selectIndex = value;
                    OnSelectIndexChanged(value);
                    if (IsHandleCreated) LoadImg();
                }
                else selectIndex = 0;
            }
        }

        ImagePreviewItemCollection? items;
        /// <summary>
        /// 图片集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("图片集合"), Category("数据")]
        public ImagePreviewItemCollection Image
        {
            get
            {
                items ??= new ImagePreviewItemCollection();
                return items;
            }
            set => items = value;
        }

        /// <summary>
        /// SelectIndex 属性值更改时发生
        /// </summary>
        [Description("SelectIndex 属性值更改时发生"), Category("行为")]
        public event IntEventHandler? SelectIndexChanged;

        protected virtual void OnSelectIndexChanged(int e) => SelectIndexChanged?.Invoke(this, new IntEventArgs(e));

        /// <summary>
        /// 总共多少图
        /// </summary>
        public int PageSize
        {
            get
            {
                if (items == null) return 0;
                return items.Count;
            }
        }

        #endregion

        /// <summary>
        /// 图片布局
        /// </summary>
        [Description("图片布局"), Category("外观"), DefaultValue(null)]
        public TFit? Fit { get; set; }

        #region 按钮

        PreBtns[]? btns;

        bool showBtn = true;
        /// <summary>
        /// 是否显示按钮
        /// </summary>
        [Description("是否显示按钮"), Category("外观"), DefaultValue(true)]
        public bool ShowBtn
        {
            get => showBtn;
            set
            {
                if (showBtn == value) return;
                showBtn = value;
                if (IsHandleCreated)
                {
                    InitBtns();
                    Invalidate();
                }
            }
        }

        bool showDefaultBtn = true;
        /// <summary>
        /// 是否显示默认按钮
        /// </summary>
        [Description("是否显示默认按钮"), Category("外观"), DefaultValue(true)]
        public bool ShowDefaultBtn
        {
            get => showDefaultBtn;
            set
            {
                if (showDefaultBtn == value) return;
                showDefaultBtn = value;
                if (IsHandleCreated)
                {
                    InitBtns();
                    Invalidate();
                }
            }
        }

        ImagePreviewButtonCollection? customButton;
        /// <summary>
        /// 自定义按钮
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("自定义按钮"), Category("数据")]
        public ImagePreviewButtonCollection CustomButton
        {
            get
            {
                customButton ??= new ImagePreviewButtonCollection();
                return customButton;
            }
            set => customButton = value;
        }

        /// <summary>
        /// 按钮点击时发生
        /// </summary>
        [Description("按钮点击时发生"), Category("行为")]
        public event ImagePreviewButtonEventHandler? ButtonClick;

        protected virtual void OnButtonClick(ImagePreviewItem item, string id, object? tag) => ButtonClick?.Invoke(this, new ImagePreviewButtonEventArgs(item, id, tag));

        Size btnSize = new Size(42, 46);
        /// <summary>
        /// 按钮大小
        /// </summary>
        [Description("按钮大小"), Category("外观"), DefaultValue(typeof(Size), "42, 46")]
        public Size BtnSize
        {
            get => btnSize;
            set
            {
                btnSize = value;
                if (IsHandleCreated)
                {
                    SizeChange(ClientRectangle);
                    Invalidate();
                }
            }
        }

        int btnIconSize = 18;
        /// <summary>
        /// 按钮图标大小
        /// </summary>
        [Description("按钮图标大小"), Category("外观"), DefaultValue(18)]
        public int BtnIconSize
        {
            get => btnIconSize;
            set
            {
                if (btnIconSize == value) return;
                btnIconSize = value;
                if (IsHandleCreated)
                {
                    SizeChange(ClientRectangle);
                    Invalidate();
                }
            }
        }

        int btnLRSize = 40;
        /// <summary>
        /// 左右按钮大小
        /// </summary>
        [Description("左右按钮大小"), Category("外观"), DefaultValue(40)]
        public int BtnLRSize
        {
            get => btnLRSize;
            set
            {
                if (btnLRSize == value) return;
                btnLRSize = value;
                if (IsHandleCreated)
                {
                    SizeChange(ClientRectangle);
                    Invalidate();
                }
            }
        }

        int containerPadding = 24;
        /// <summary>
        /// 容器边距
        /// </summary>
        [Description("容器边距"), Category("外观"), DefaultValue(24)]
        public int ContainerPadding
        {
            get => containerPadding;
            set
            {
                if (containerPadding == value) return;
                containerPadding = value;
                if (IsHandleCreated)
                {
                    SizeChange(ClientRectangle);
                    Invalidate();
                }
            }
        }

        Size btnPadding = new Size(12, 32);
        /// <summary>
        /// 按钮边距
        /// </summary>
        [Description("按钮边距"), Category("外观"), DefaultValue(typeof(Size), "12, 32")]
        public Size BtnPadding
        {
            get => btnPadding;
            set
            {
                btnPadding = value;
                if (IsHandleCreated)
                {
                    SizeChange(ClientRectangle);
                    Invalidate();
                }
            }
        }

        void InitBtns()
        {
            if (showBtn)
            {
                if (showDefaultBtn)
                {
                    int len = 8;
                    if (customButton != null && customButton.Count > 0) len += customButton.Count;
                    var btnwiths = new List<PreBtns>(len)
                    {
                        new PreBtns("@t_flipY",SvgDb.Custom["SwapOutlined"].Insert(28," transform=\"rotate(90),translate(0 -100%)\"")),
                        new PreBtns("@t_flipX","SwapOutlined"),
                        new PreBtns("@t_rotateL","RotateLeftOutlined"),
                        new PreBtns("@t_rotateR","RotateRightOutlined"),
                        new PreBtns("@t_zoomOut","ZoomOutOutlined"),
                        new PreBtns("@t_zoomIn","ZoomInOutlined"),
                    };
                    if (customButton != null && customButton.Count > 0)
                    {
                        foreach (var it in customButton) btnwiths.Add(new PreBtns(it.Name, it.IconSvg, it.Tag));
                    }
                    btns = btnwiths.ToArray();
                }
                else if (customButton != null && customButton.Count > 0)
                {
                    var btnwiths = new List<PreBtns>(customButton.Count);
                    foreach (var it in customButton) btnwiths.Add(new PreBtns(it.Name, it.IconSvg, it.Tag));
                    btns = btnwiths.ToArray();
                }
                else btns = null;
            }
            else btns = null;

            SizeChange(ClientRectangle);
        }

        #endregion

        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            InitBtns();
            LoadImg(false);
        }

        protected override void Dispose(bool disposing)
        {
            PlayGIF = false;
            base.Dispose(disposing);
        }

        #region 渲染

        #region 渲染图片

        bool loading = false;
        /// <summary>
        /// 加载状态
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool Loading
        {
            get => loading;
            set
            {
                if (loading == value) return;
                loading = value;
                Invalidate();
            }
        }

        string? LoadingProgressStr;
        float _value = -1F;
        /// <summary>
        /// 加载进度
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal float LoadingProgress
        {
            get => _value;
            set
            {
                if (_value == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                _value = value;
                if (loading) Invalidate();
            }
        }

        #region 图片

        Image? imgtmp;
        Image? Img
        {
            get => imgtmp;
            set
            {
                imgtmp = value;
                LoadGif();
            }
        }

        #region GIF

        bool PlayGIF = true;
        void LoadGif()
        {
            if (imgtmp == null) return;
            var fd = new FrameDimension(imgtmp.FrameDimensionsList[0]);
            int count = imgtmp.GetFrameCount(fd);
            if (count > 1) PlayGif(imgtmp, fd, count);
        }

        void PlayGif(Image value, FrameDimension fd, int count)
        {
            ITask.Run(() =>
            {
                int[] delays = GifDelays(value, count);
                while (PlayGIF && imgtmp == value)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (PlayGIF && imgtmp == value)
                        {
                            lock (_lock)
                            {
                                try
                                {
                                    value.SelectActiveFrame(fd, i);
                                }
                                catch { }
                            }
                            Invalidate();
                            Thread.Sleep(Math.Max(delays[i], 10));
                        }
                        else
                        {
                            value.SelectActiveFrame(fd, 0);
                            return;
                        }
                    }
                }
            }, () => Invalidate());
        }

        object _lock = new object();
        int[] GifDelays(Image value, int count)
        {
            int PropertyTagFrameDelay = 0x5100;
            var propItem = value.GetPropertyItem(PropertyTagFrameDelay);
            if (propItem != null)
            {
                var bytes = propItem.Value;
                if (bytes != null)
                {
                    int[] delays = new int[count];
                    for (int i = 0; i < delays.Length; i++) delays[i] = BitConverter.ToInt32(bytes, i * 4) * 10;
                    return delays;
                }
            }
            int[] delaysd = new int[count];
            for (int i = 0; i < delaysd.Length; i++) delaysd[i] = 100;
            return delaysd;
        }

        #endregion

        #endregion

        Size ImgSize = new Size();
        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="r">是否渲染</param>
        public void LoadImg(bool r = true)
        {
            autoDpi = true;
            if (items == null) return;
            try
            {
                int index = selectIndex;
                var it = items[index];
                if (it.Img == null)
                {
                    if (it.Call != null)
                    {
                        imgtmp?.Dispose();
                        Img = it.Call(index, it);
                        if (Img == null)
                        {
                            if (r) Invalidate();
                            return;
                        }
                        ImgSize = Img.Size;
                        FillScaleImg();
                        if (r) Invalidate();
                    }
                    else if (it.CallProg != null)
                    {
                        LoadingProgressStr = null;
                        _value = -1F;
                        Loading = true;
                        DateTime now = DateTime.Now, now2 = DateTime.Now;
                        ITask.Run(() =>
                        {
                            var img = it.CallProg(index, it, (prog, progstr) =>
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
                                imgtmp?.Dispose();
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
                                    Thread.Sleep(100);
                                    if (selectIndex == SelectIndex) Invalidate();
                                }
                            }
                        });
                    }
                    else
                    {
                        Img = null;
                        if (r) Invalidate();
                    }
                }
                else
                {
                    Img = it.Img;
                    ImgSize = Img.Size;
                    FillScaleImg();
                    if (r) Invalidate();
                }
            }
            catch { }
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
                rect_img_dpi = ScaleImg(ClientRectangle, _dpi);
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
                var rect = ClientRectangle;
                float DpiX = (float)((rect.Width * 1.0) / (ImgSize.Width * 1.0)), DpiY = (float)((rect.Height * 1.0) / (ImgSize.Height * 1.0));
                if (Fit.HasValue)
                {
                    switch (Fit.Value)
                    {
                        case TFit.Contain:
                            Dpi = Math.Min(DpiX, DpiY);
                            break;
                        case TFit.Cover:
                            Dpi = Math.Max(DpiX, DpiY);
                            break;
                        default:
                            Dpi = 1F;
                            break;
                    }
                }
                else
                {
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
        }

        #endregion

        #endregion

        protected override void OnDraw(DrawEventArgs e)
        {
            base.OnDraw(e);
            var g = e.Canvas;
            var rect = ClientRectangle;
            if (imgtmp == null)
            {
                if (LoadingProgressStr != null) PaintLoading(g, rect, true);
            }
            else
            {
                g.Image(imgtmp, rect_img_dpi, new RectangleF(0, 0, ImgSize.Width, ImgSize.Height), GraphicsUnit.Pixel);
                if (loading) PaintLoading(g, rect);
            }
            using (var brush = new SolidBrush(Color.FromArgb(26, 0, 0, 0)))
            {
                if (PageSize > 1)
                {
                    PaintBtn(g, brush, rect_left, rect_left_icon, "LeftOutlined", hoverLeft, enabledLeft);
                    PaintBtn(g, brush, rect_right, rect_right_icon, "RightOutlined", hoverRight, enabledRight);
                }
                if (btns == null) return;
                using (var path = rect_panel.RoundPath(rect_panel.Height))
                {
                    g.Fill(brush, path);
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
                }
            }
        }

        void PaintLoading(Canvas g, Rectangle rect, bool error = false)
        {
            var bor6 = 6F * Dpi;
            int loading_size = (int)(40 * Dpi);
            var rect_loading = new Rectangle(rect.X + (rect.Width - loading_size) / 2, rect.Y + (rect.Height - loading_size) / 2, loading_size, loading_size);
            Color color, bg;
            if (error)
            {
                bg = Colour.Error.Get(nameof(Preview));
                color = Colour.ErrorColor.Get(nameof(Preview));
            }
            else
            {
                bg = Colour.Primary.Get(nameof(Preview));
                color = Colour.PrimaryColor.Get(nameof(Preview));
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
                    g.String(LoadingProgressStr, Font, color, rect_loading, FormatFlags.Center | FormatFlags.NoWrap);
                }
            }
            else if (LoadingProgressStr != null)
            {
                g.DrawEllipse(Colour.Error.Get(nameof(Preview)), bor6, rect_loading);
                rect_loading.Offset(0, loading_size);
                g.String(LoadingProgressStr, Font, Colour.ErrorColor.Get(nameof(Preview)), rect_loading, FormatFlags.Center | FormatFlags.NoWrap);
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

        Rectangle rect_left, rect_left_icon, rect_right, rect_right_icon, rect_panel;
        protected override void OnSizeChanged(EventArgs e)
        {
            SizeChange(ClientRectangle);
            if (Img != null) FillScaleImg();
            base.OnSizeChanged(e);
        }

        void SizeChange(Rectangle rect)
        {
            int btn_height = (int)(BtnSize.Height * Dpi), lr_size = (int)(BtnLRSize * Dpi), btn_width = (int)(BtnSize.Width * Dpi),
                padding = (int)(ContainerPadding * Dpi), padding_lr = (int)(BtnPadding.Width * Dpi), padding_buttom = (int)(BtnPadding.Height * Dpi),
                icon_size = (int)(BtnIconSize * Dpi);

            if (PageSize > 1)
            {
                rect_left = new Rectangle(rect.X + padding_lr, rect.Y + (rect.Height - lr_size) / 2, lr_size, lr_size);
                rect_left_icon = GetCentered(rect_left, icon_size);

                rect_right = new Rectangle(rect.Right - padding_lr - lr_size, rect.Y + (rect.Height - lr_size) / 2, lr_size, lr_size);
                rect_right_icon = GetCentered(rect_right, icon_size);
            }

            if (btns == null) return;
            int w = (btn_width * btns.Length - 1) + padding * 2, x = rect.X + (rect.Width - w) / 2, y = rect.Bottom - padding_buttom - btn_height;
            rect_panel = new Rectangle(x, y, w, btn_height);
            x += padding;
            foreach (var it in btns)
            {
                it.Rect = new Rectangle(x, y, btn_width, btn_height);
                it.rect = GetCentered(it.Rect, icon_size);
                x += btn_width;
            }
        }

        Rectangle GetCentered(Rectangle rect, int size) => new Rectangle(rect.X + (rect.Width - size) / 2, rect.Y + (rect.Height - size) / 2, size, size);

        #endregion

        #region 鼠标

        bool hoverLeft = false, hoverRight = false;
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
                Invalidate();
            }
            base.OnMouseWheel(e);
        }

        void SetBtnEnabled(string id, bool enabled)
        {
            if (btns == null) return;
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
                    Invalidate();
                    return;
                }
            }
            if (btns == null) return;
            int count = 0, hand = 0;
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
            if (count > 0) Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                if (btns != null)
                {
                    foreach (var it in btns)
                    {
                        if (it.enabled && it.Rect.Contains(e.X, e.Y))
                        {
                            it.mdown = true;
                            return;
                        }
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
            if (btns != null)
            {
                foreach (var it in btns)
                {
                    if (it.mdown)
                    {
                        if (it.Rect.Contains(e.X, e.Y))
                        {
                            switch (it.id)
                            {
                                case "@t_flipY":
                                    FlipY();
                                    break;
                                case "@t_flipX":
                                    FlipX();
                                    break;
                                case "@t_rotateL":
                                    RotateL();
                                    break;
                                case "@t_rotateR":
                                    RotateR();
                                    break;
                                case "@t_zoomOut":
                                    ZoomOut();
                                    break;
                                case "@t_zoomIn":
                                    ZoomIn();
                                    break;
                                default:
                                    if (items == null) return;
                                    OnButtonClick(items[selectIndex], it.id, it.tag);
                                    break;
                            }
                        }
                        it.mdown = false;
                        return;
                    }
                }
            }
            if (PageSize > 1)
            {
                if (enabledLeft && rect_left.Contains(e.X, e.Y))
                {
                    SelectIndex = selectIndex - 1;
                    return;
                }
                if (enabledRight && rect_right.Contains(e.X, e.Y))
                {
                    SelectIndex = selectIndex + 1;
                    return;
                }
            }
            base.OnMouseUp(e);
        }

        bool moveImg = false, moveImging = false;
        Point movePos;
        float offsetXOld = 0, offsetYOld = 0;

        #endregion

        #region 方法

        public void ZoomToControl()
        {
            if (Img == null) return;
            Dpi = 1;
            var rect = ClientRectangle;
            float fControlWidth = rect.Width, fControlHeight = rect.Height;
            float fImageWidth = ImgSize.Width, fImageHeight = ImgSize.Height;
            if (fImageWidth / fImageHeight > fControlWidth / fControlHeight) Dpi = fControlWidth / fImageWidth;
            else Dpi = fControlHeight / fImageHeight;
            Invalidate();
        }

        public void FlipY()
        {
            if (Img == null) return;
            Img.RotateFlip(RotateFlipType.RotateNoneFlipY);
            Invalidate();
        }

        public void FlipX()
        {
            if (Img == null) return;
            Img.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Invalidate();
        }

        public void RotateL()
        {
            if (Img == null) return;
            float old = _dpi;
            bool oldautoDpi = autoDpi;
            Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
            ImgSize = Img.Size;
            autoDpi = true;
            FillScaleImg();
            Dpi = old;
            autoDpi = oldautoDpi;
            Invalidate();
        }

        public void RotateR()
        {
            if (Img == null) return;
            float old = _dpi;
            bool oldautoDpi = autoDpi;
            Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            ImgSize = Img.Size;
            autoDpi = true;
            FillScaleImg();
            Dpi = old;
            autoDpi = oldautoDpi;
            Invalidate();
        }

        public void ZoomOut()
        {
            Dpi -= 0.1F;
            SetBtnEnabled("@t_zoomOut", Dpi >= 0.06);
            Invalidate();
        }

        public void ZoomIn()
        {
            Dpi += 0.1F;
            SetBtnEnabled("@t_zoomOut", true);
            Invalidate();
        }

        #endregion
    }

    public class ImagePreviewItemCollection : iCollection<ImagePreviewItem>
    {
    }

    public class ImagePreviewItem
    {
        /// <summary>
        /// ID
        /// </summary>
        [Description("ID"), Category("数据"), DefaultValue(null)]
        public string? ID { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片"), Category("外观"), DefaultValue(null)]
        public Image? Img { get; set; }

        public Func<int, ImagePreviewItem, Image?>? Call { get; set; }
        public Func<int, ImagePreviewItem, Action<float, string?>, Image?>? CallProg { get; set; }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        #region 设置

        public ImagePreviewItem SetID(string? value)
        {
            ID = value;
            return this;
        }

        public ImagePreviewItem SetImage(Image? value)
        {
            Img = value;
            return this;
        }

        public ImagePreviewItem SetImage(Func<int, object, Image?>? value)
        {
            Call = value;
            return this;
        }

        public ImagePreviewItem SetImage(Func<int, object, Action<float, string?>, Image?>? value)
        {
            CallProg = value;
            return this;
        }

        public ImagePreviewItem SetTag(object? value)
        {
            Tag = value;
            return this;
        }

        #endregion
    }


    public class ImagePreviewButtonCollection : iCollection<ImagePreviewButton>
    {
    }

    /// <summary>
    /// 按钮
    /// </summary>
    public class ImagePreviewButton
    {
        /// <summary>
        /// 自定义按钮
        /// </summary>
        public ImagePreviewButton() { }

        /// <summary>
        /// 自定义按钮
        /// </summary>
        /// <param name="name">按钮名称</param>
        /// <param name="svg">图标SVG</param>
        public ImagePreviewButton(string name, string svg)
        {
            Name = name;
            IconSvg = svg;
        }

        /// <summary>
        /// 按钮名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标SVG
        /// </summary>
        public string IconSvg { get; set; }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        #region 设置

        public ImagePreviewButton SetName(string value)
        {
            Name = value;
            return this;
        }

        public ImagePreviewButton SetIcon(string value)
        {
            IconSvg = value;
            return this;
        }

        public ImagePreviewButton SetTag(object? value)
        {
            Tag = value;
            return this;
        }

        #endregion
    }
}
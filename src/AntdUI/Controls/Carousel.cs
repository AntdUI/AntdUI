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
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Carousel 走马灯
    /// </summary>
    /// <remarks>旋转木马，一组轮播的区域。</remarks>
    [Description("Carousel 走马灯")]
    [ToolboxItem(true)]
    [DefaultProperty("Image")]
    [DefaultEvent("SelectIndexChanged")]
    [Designer(typeof(IControlDesigner))]
    public class Carousel : IControl
    {
        #region 属性

        /// <summary>
        /// 手势滑动
        /// </summary>
        [Description("手势滑动"), Category("行为"), DefaultValue(true)]
        public bool Touch { get; set; } = true;

        /// <summary>
        /// 滑动到外面
        /// </summary>
        [Description("滑动到外面"), Category("行为"), DefaultValue(false)]
        public bool TouchOut { get; set; } = false;

        bool autoplay = false;
        /// <summary>
        /// 自动切换
        /// </summary>
        [Description("自动切换"), Category("行为"), DefaultValue(false)]
        public bool Autoplay
        {
            get => autoplay;
            set
            {
                if (autoplay == value) return;
                autoplay = value;
                if (value) new Thread(LongTime) { IsBackground = true }.Start();
            }
        }

        /// <summary>
        /// 自动切换延迟(s)
        /// </summary>
        [Description("自动切换延迟(s)"), Category("行为"), DefaultValue(4)]
        public int Autodelay { get; set; } = 4;

        /// <summary>
        /// 面板指示点大小
        /// </summary>
        [Description("面板指示点大小"), Category("面板"), DefaultValue(typeof(Size), "28, 4")]
        public Size DotSize { get; set; } = new Size(28, 4);

        /// <summary>
        /// 面板指示点边距
        /// </summary>
        [Description("面板指示点边距"), Category("面板"), DefaultValue(12)]
        public int DotMargin { get; set; } = 12;

        /// <summary>
        /// 面板指示点位置
        /// </summary>
        [Description("面板指示点位置"), Category("面板"), DefaultValue(TAlignMini.None)]
        public TAlignMini DotPosition { get; set; } = TAlignMini.None;

        int radius = 0;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(0)]
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

        bool round = false;
        /// <summary>
        /// 圆角样式
        /// </summary>
        [Description("圆角样式"), Category("外观"), DefaultValue(false)]
        public bool Round
        {
            get => round;
            set
            {
                if (round == value) return;
                round = value;
                Invalidate();
            }
        }

        CarouselItemCollection? items;
        /// <summary>
        /// 图片集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("图片集合"), Category("数据")]
        public CarouselItemCollection Image
        {
            get
            {
                items ??= new CarouselItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        TFit imageFit = TFit.Cover;
        /// <summary>
        /// 图片布局
        /// </summary>
        [Description("图片布局"), Category("外观"), DefaultValue(TFit.Cover)]
        public TFit ImageFit
        {
            get => imageFit;
            set
            {
                if (imageFit == value) return;
                imageFit = value;
                Invalidate();
            }
        }

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
                if (autoplay) now = DateTime.Now.AddSeconds(Autodelay);
                if (selectIndex == value) return;
                if (items != null)
                {
                    if (items.Count <= selectIndex || selectIndex < 0)
                    {
                        selectIndex = 0;
                        return;
                    }
                    SetSelectIndex(value);
                }
                else selectIndex = 0;
            }
        }

        /// <summary>
        /// SelectIndex 属性值更改时发生
        /// </summary>
        [Description("SelectIndex 属性值更改时发生"), Category("行为")]
        public event IntEventHandler? SelectIndexChanged = null;

        void SetSelectIndex(int value, bool auto = false)
        {
            int width = ClientRectangle.Width - Padding.Horizontal;
            if (items != null && IsHandleCreated && Config.Animation)
            {
                ThreadChange?.Dispose();
                AnimationChangeAuto = false;
                float end = value * width;
                int len = items.Count;
                bool left = value * width > AnimationChangeValue;
                AnimationChangeMax = len * width;
                AnimationChangeMaxW = AnimationChangeMax - width;
                AnimationChange = true;
                var old = selectIndex;
                selectIndex = value;
                SelectIndexChanged?.Invoke(this, value);
                var speed = Math.Abs(end - AnimationChangeValue) / 50F;
                if (speed < 8) speed = 8F;
                if (left)
                {
                    float modera = end - width * 0.05F;
                    ThreadChange = new ITask(this, () =>
                    {
                        AnimationChangeValue = AnimationChangeValue.Calculate(Speed(speed, modera));
                        if (AnimationChangeValue > end)
                        {
                            AnimationChangeValue = end;
                            return false;
                        }
                        Invalidate();
                        return true;
                    }, 10, () =>
                    {
                        AnimationChange = false;
                        Invalidate();
                    });
                }
                else
                {
                    if (auto && value == 0 && len > 2 && old == len - 1)
                    {
                        AnimationChangeAuto = true;
                        end = len * width;
                        float modera = end - width * 0.05F;
                        ThreadChange = new ITask(this, () =>
                        {
                            AnimationChangeValue = AnimationChangeValue.Calculate(Speed(speed, modera));
                            if (AnimationChangeValue > end)
                            {
                                AnimationChangeValue = end;
                                return false;
                            }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationChange = false;
                            AnimationChangeValue = 0;
                            Invalidate();
                        });
                    }
                    else
                    {
                        float modera = end + width * 0.05F;
                        ThreadChange = new ITask(this, () =>
                        {
                            AnimationChangeValue -= Speed2(speed, modera);
                            if (AnimationChangeValue <= end)
                            {
                                AnimationChangeValue = end;
                                return false;
                            }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationChange = false;
                            Invalidate();
                        });
                    }
                }
            }
            else
            {
                AnimationChangeValue = selectIndex * width;
                selectIndex = value;
                Invalidate();
            }
        }

        #region 动画

        DateTime now = DateTime.Now;
        internal float Speed(float speed, float modera)
        {
            if (modera < AnimationChangeValue) return 0.8F;
            return speed;
        }
        internal float Speed2(float speed, float modera)
        {
            if (modera > AnimationChangeValue) return 0.8F;
            return speed;
        }

        protected override void OnCreateControl()
        {
            int width = ClientRectangle.Width;
            AnimationChangeValue = selectIndex * width;
            base.OnCreateControl();
        }

        bool AnimationChangeAuto = false;
        int AnimationChangeMax = 0;
        int AnimationChangeMaxW = 0;
        float AnimationChangeValue = 0F;
        bool AnimationChange = false;
        protected override void Dispose(bool disposing)
        {
            ThreadChange?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadChange = null;

        void LongTime()
        {
            while (autoplay)
            {
                if (Autodelay > 0) Thread.Sleep(Autodelay * 1000);
                else Thread.Sleep(1000);
                if (!down && !ExtraMouseHover && DateTime.Now > now)
                {
                    if (items == null) continue;
                    if (selectIndex >= items.Count - 1) SetSelectIndex(0, true);
                    else SetSelectIndex(selectIndex + 1, true);
                }
            }
        }

        #endregion

        protected override void OnSizeChanged(EventArgs e)
        {
            ChangeImg();
            base.OnSizeChanged(e);
        }

        CarouselDotItem[] dot_list = new CarouselDotItem[0];
        internal void ChangeImg()
        {
            if (DotPosition == TAlignMini.None || items == null || items.Count == 0) return;
            var _rect = ClientRectangle;
            if (_rect.Width == 0 || _rect.Height == 0) return;
            bmp?.Dispose();
            bmp = null;
            var rect = _rect.PaddingRect(Padding);
            int len = items.Count;
            var list = new List<CarouselDotItem>(len);
            if (DotPosition == TAlignMini.Top || DotPosition == TAlignMini.Bottom)
            {
                int dot_size = DotSize.Width * len, y = DotPosition == TAlignMini.Bottom ? rect.Y + rect.Height - (DotMargin + DotSize.Height) : rect.Y + DotMargin,
                     y2 = DotPosition == TAlignMini.Bottom ? rect.Y + rect.Height - (DotMargin + DotSize.Height) - DotMargin / 2 : rect.Y + DotMargin / 2;
                int temp_x = rect.X + (rect.Width - dot_size) / 2;

                for (int i = 0; i < len; i++)
                {
                    list.Add(new CarouselDotItem
                    {
                        i = i,
                        rect_fill = new RectangleF(temp_x, y2, DotSize.Width, DotMargin),
                        rect_action = new RectangleF(temp_x + 2, y, DotSize.Width - 4, DotSize.Height),
                        rect = new RectangleF(temp_x + 4, y, DotSize.Width - 8, DotSize.Height)
                    });
                    temp_x += DotSize.Width;
                }
            }
            else
            {
                int dot_size = DotSize.Width * len, x = DotPosition == TAlignMini.Right ? rect.X + rect.Width - (DotMargin + DotSize.Height) : rect.X + DotMargin,
                     x2 = DotPosition == TAlignMini.Right ? rect.X + rect.Width - (DotMargin + DotSize.Height) - DotMargin / 2 : rect.X + DotMargin / 2;
                int temp_y = rect.Y + (rect.Height - dot_size) / 2;
                for (int i = 0; i < len; i++)
                {
                    list.Add(new CarouselDotItem
                    {
                        i = i,
                        rect_fill = new RectangleF(x2, temp_y, DotMargin, DotSize.Width),
                        rect_action = new RectangleF(x, temp_y + 2, DotSize.Height, DotSize.Width - 4),
                        rect = new RectangleF(x, temp_y + 4, DotSize.Height, DotSize.Width - 8)
                    });
                    temp_y += DotSize.Width;
                }
            }
            dot_list = list.ToArray();
        }

        #endregion

        #region 渲染

        string? bmpcode = null;
        Bitmap? bmp = null;
        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            var _rect = ClientRectangle;
            if (_rect.Width == 0 || _rect.Height == 0) return;
            var rect = _rect.PaddingRect(Padding);
            var g = e.Graphics.High();
            int len = items.Count;
            var image = items[selectIndex]?.Img;
            float _radius = radius * Config.Dpi;
            if (image != null)
            {
                if (AnimationChange)
                {
                    var select_range = SelectRange(len, rect);
                    if (bmp == null || bmpcode != select_range.i)
                    {
                        bmpcode = select_range.i;
                        bmp = PaintBmp(select_range, rect, _radius);
                    }
                    g.DrawImage(bmp, rect.X - AnimationChangeValue, rect.Y, bmp.Width, bmp.Height);
                }
                else g.PaintImg(rect, image, imageFit, _radius, round);
            }
            if (dot_list.Length > 0)
            {
                using (var brush = new SolidBrush(Style.Db.BgBase))
                using (var brush2 = new SolidBrush(Color.FromArgb(77, brush.Color)))
                {
                    if (round || radius > 0)
                    {
                        foreach (var it in dot_list)
                        {
                            if (it.i == selectIndex)
                            {
                                using (var path = it.rect_action.RoundPath(DotSize.Height))
                                    g.FillPath(brush, path);
                            }
                            else
                            {
                                using (var path = it.rect.RoundPath(DotSize.Height))
                                    g.FillPath(brush2, path);
                            }
                        }
                    }
                    else
                    {
                        foreach (var it in dot_list)
                        {
                            if (it.i == selectIndex) g.FillRectangle(brush, it.rect_action);
                            else g.FillRectangle(brush2, it.rect);
                        }
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        Bitmap PaintBmp(CarouselRectPanel select_range, Rectangle rect, float radius)
        {
            bmpcode = select_range.i;
            Bitmap bmp;
            if (AnimationChangeAuto)
            {
                bmp = new Bitmap(AnimationChangeMax + rect.Width, rect.Height);
                using (var g2 = Graphics.FromImage(bmp).High())
                {
                    PaintBmp(select_range, g2, radius);
                    var bmo = Image[0]?.Img;
                    if (bmo != null) g2.PaintImg(new RectangleF(AnimationChangeMax, 0, rect.Width, rect.Height), bmo, imageFit, radius, round);
                }
            }
            else
            {
                bmp = new Bitmap(AnimationChangeMax, rect.Height);
                using (var g2 = Graphics.FromImage(bmp).High())
                {
                    PaintBmp(select_range, g2, radius);
                }
            }
            return bmp;
        }
        void PaintBmp(CarouselRectPanel select_range, Graphics g2, float radius)
        {
            foreach (var it in select_range.list)
            {
                var bmo = Image[it.i]?.Img;
                if (bmo != null)
                {
                    g2.PaintImg(it.rect, bmo, imageFit, radius, round);
                }
            }
        }


        /// <summary>
        /// 选择脏渲染序号
        /// </summary>
        CarouselRectPanel SelectRange(int len, Rectangle rect)
        {
            var r = new CarouselRectPanel
            {
                list = new List<CarouselRect>(len)
            };
            var indes = new List<int>(len);
            int temp = 0;
            for (int i = 0; i < len; i++)
            {
                var rect1 = new RectangleF(temp, 0, rect.Width, rect.Height);
                if (rect1.Contains(AnimationChangeValue, 0))
                {
                    indes.Add(i);
                    r.list.Add(new CarouselRect
                    {
                        i = i,
                        rect = rect1,
                    });
                }
                if (i < len - 1)
                {
                    var rect2 = new RectangleF(temp + rect.Width, 0, rect.Width, rect.Height);
                    if (rect2.Contains(AnimationChangeValue + rect.Width, 0))
                    {
                        indes.Add(i + 1);
                        r.list.Add(new CarouselRect
                        {
                            i = i + 1,
                            rect = rect2,
                        });
                    }
                }
                temp += rect.Width;
                if (temp > AnimationChangeValue + rect.Width)
                    break;
            }
            if (r.list.Count == 0 && AnimationChangeValue < 0)
            {
                indes.Add(0);
                r.list.Add(new CarouselRect
                {
                    i = 0,
                    rect = new RectangleF(0, 0, rect.Width, rect.Height),
                });
            }
            r.i = string.Join("", indes);
            return r;
        }
        CarouselRect? SelectRangeOne(int len, Rectangle rect)
        {
            var r = new List<CarouselRect>(len);
            int temp = 0;
            for (int i = 0; i < len; i++)
            {
                int cen = (temp + rect.Width / 2);
                if (AnimationChangeValue > cen - rect.Width && AnimationChangeValue < cen + rect.Width)
                {
                    var prog = AnimationChangeValue / cen;
                    r.Add(new CarouselRect
                    {
                        p = prog,
                        i = i,
                        rect = new RectangleF(temp, 0, rect.Width, rect.Height),
                    });
                }
                temp += rect.Width;
                if (temp > AnimationChangeValue + rect.Width)
                    break;
            }
            if (r.Count > 0)
            {
                r.Sort((x, y) => x.p.CompareTo(y.p));
                return r[0];
            }
            return null;
        }

        #endregion

        #region 鼠标

        bool _mouseHover = false;
        bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                var enabled = Enabled;
                SetCursor(value && enabled);
                if (!value && autoplay) now = DateTime.Now.AddSeconds(Autodelay);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ExtraMouseHover = true;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ExtraMouseHover = false;
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ExtraMouseHover = false;
        }

        bool down = false;
        float x = 0;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && items != null)
            {
                if (dot_list.Length > 0)
                {
                    for (int i = 0; i < dot_list.Length; i++)
                    {
                        if (dot_list[i].rect_fill.Contains(e.Location))
                        {
                            SetSelectIndex(dot_list[i].i);
                            return;
                        }
                    }
                }
                if (Touch)
                {
                    x = AnimationChangeValue + e.Location.X;
                    int width = ClientRectangle.Width;
                    int len = items.Count;
                    AnimationChangeMax = len * width;
                    AnimationChangeMaxW = AnimationChangeMax - width;
                    down = true;
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (down)
            {
                var val = x - e.Location.X;
                if (!TouchOut)
                {
                    if (val < 0) val = 0;
                    else if (val > AnimationChangeMaxW) val = AnimationChangeMaxW;
                }
                AnimationChange = true;
                AnimationChangeValue = val;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (down)
            {
                int len = Image.Count;
                var rect = ClientRectangle;
                int width = rect.Width;
                var val = x - e.Location.X;
                var select_range = SelectRangeOne(len, rect);
                AnimationChange = false;
                if (select_range != null)
                {
                    SetSelectIndex(select_range.i);
                }
                else if (val > AnimationChangeMax - width)
                {
                    SetSelectIndex(Image.Count - 1);
                }
                else if (val < 0)
                {
                    SetSelectIndex(0);
                }
                Invalidate();
            }
            down = false;
            base.OnMouseUp(e);
        }

        #endregion
    }

    public class CarouselItemCollection : iCollection<CarouselItem>
    {
        public CarouselItemCollection(Carousel it)
        {
            BindData(it);
        }

        internal CarouselItemCollection BindData(Carousel it)
        {
            action = render =>
            {
                it.ChangeImg();
                it.Invalidate();
            };
            return this;
        }
    }
    public class CarouselItem : NotifyProperty
    {
        Image? img;
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片"), Category("外观"), DefaultValue(null)]
        public Image? Img
        {
            get => img;
            set
            {
                if (img == value) return;
                img = value;
                OnPropertyChanged("Img");
            }
        }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }
    }

    internal class CarouselRectPanel
    {
        public string i { get; set; }
        public List<CarouselRect> list { get; set; }
    }
    internal class CarouselRect
    {
        public int i { get; set; }
        public float p { get; set; }
        public RectangleF rect { get; set; }
    }
    internal class CarouselDotItem : CarouselRect
    {
        public RectangleF rect_action { get; set; }
        public RectangleF rect_fill { get; set; }
    }
}
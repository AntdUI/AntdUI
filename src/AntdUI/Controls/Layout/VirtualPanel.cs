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
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// VirtualPanel 虚拟容器
    /// </summary>
    [Description("VirtualPanel 虚拟容器")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    [DefaultEvent("ItemClick")]
    public class VirtualPanel : IControl
    {
        ScrollBar scroll;
        public VirtualPanel()
        {
            scroll = new ScrollBar(this);
        }

        #region 属性

        #region 数据

        VirtualCollection? items;
        /// <summary>
        /// 数据
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据")]
        public VirtualCollection Items
        {
            get
            {
                items ??= new VirtualCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        #endregion

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
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

        #region 阴影

        int shadow = 0;
        /// <summary>
        /// 阴影大小
        /// </summary>
        [Description("阴影"), Category("外观"), DefaultValue(0)]
        public int Shadow
        {
            get => shadow;
            set
            {
                if (shadow == value) return;
                shadow = value;
                DisposeShadow();
                LoadLayout();
            }
        }

        Color? shadowColor;
        /// <summary>
        /// 阴影颜色
        /// </summary>
        [Description("阴影颜色"), Category("阴影"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ShadowColor
        {
            get => shadowColor;
            set
            {
                if (shadowColor == value) return;
                shadowColor = value;
                DisposeShadow();
                LoadLayout();
            }
        }

        int shadowOffsetX = 0;
        /// <summary>
        /// 阴影偏移X
        /// </summary>
        [Description("阴影偏移X"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetX
        {
            get => shadowOffsetX;
            set
            {
                if (shadowOffsetX == value) return;
                shadowOffsetX = value;
                DisposeShadow();
                LoadLayout();
            }
        }

        int shadowOffsetY = 0;
        /// <summary>
        /// 阴影偏移Y
        /// </summary>
        [Description("阴影偏移Y"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetY
        {
            get => shadowOffsetY;
            set
            {
                if (shadowOffsetY == value) return;
                shadowOffsetY = value;
                DisposeShadow();
                LoadLayout();
            }
        }

        float shadowOpacity = 0.1F;
        /// <summary>
        /// 阴影透明度
        /// </summary>
        [Description("阴影透明度"), Category("阴影"), DefaultValue(0.1F)]
        public float ShadowOpacity
        {
            get => shadowOpacity;
            set
            {
                if (shadowOpacity == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                shadowOpacity = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 阴影透明度动画使能
        /// </summary>
        [Description("阴影透明度动画使能"), Category("阴影"), DefaultValue(false)]
        public bool ShadowOpacityAnimation { get; set; }

        float shadowOpacityHover = 0.3F;
        /// <summary>
        /// 悬停阴影后透明度
        /// </summary>
        [Description("悬停阴影后透明度"), Category("阴影"), DefaultValue(0.3F)]
        public float ShadowOpacityHover
        {
            get => shadowOpacityHover;
            set
            {
                if (shadowOpacityHover == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                shadowOpacityHover = value;
                Invalidate();
            }
        }

        TAlignMini shadowAlign = TAlignMini.None;
        [Description("阴影方向"), Category("阴影"), DefaultValue(TAlignMini.None)]
        public TAlignMini ShadowAlign
        {
            get => shadowAlign;
            set
            {
                if (shadowAlign == value) return;
                shadowAlign = value;
                DisposeShadow();
                LoadLayout();
            }
        }

        void DisposeShadow()
        {
            if (items == null || items.Count == 0) return;
            if (shadow_dir_tmp.Count == 0) return;
            lock (shadow_dir_tmp)
            {
                foreach (var item in shadow_dir_tmp) item.Value.Dispose();
                shadow_dir_tmp.Clear();
            }
        }

        #endregion

        int gap = 0;
        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(0)]
        public int Gap
        {
            get => gap;
            set
            {
                if (gap == value) return;
                gap = value;
                LoadLayout();
                Invalidate();
            }
        }

        #region 布局

        bool wrap = true;
        /// <summary>
        /// 换行
        /// </summary>
        [Description("换行"), Category("布局"), DefaultValue(true)]
        public bool Wrap
        {
            get => wrap;
            set
            {
                if (wrap == value) return;
                wrap = value;
                LoadLayout();
                Invalidate();
            }
        }

        bool waterfall = false;
        /// <summary>
        /// 瀑布流
        /// </summary>
        [Description("瀑布流"), Category("布局"), DefaultValue(false)]
        public bool Waterfall
        {
            get => waterfall;
            set
            {
                if (waterfall == value) return;
                waterfall = value;
                LoadLayout();
                Invalidate();
            }
        }

        TAlignItems alignitems = TAlignItems.Start;
        /// <summary>
        /// 侧轴(纵轴)对齐方式
        /// </summary>
        [Description("侧轴(纵轴)对齐方式"), Category("布局"), DefaultValue(TAlignItems.Start)]
        public TAlignItems AlignItems
        {
            get => alignitems;
            set
            {
                if (alignitems == value) return;
                alignitems = value;
                LoadLayout();
                Invalidate();
            }
        }

        TJustifyContent justifycontent = TJustifyContent.Start;
        /// <summary>
        /// 主轴(横轴)对齐方式
        /// </summary>
        [Description("主轴(横轴)对齐方式"), Category("布局"), DefaultValue(TJustifyContent.Start)]
        public TJustifyContent JustifyContent
        {
            get => justifycontent;
            set
            {
                if (justifycontent == value) return;
                justifycontent = value;
                LoadLayout();
                Invalidate();
            }
        }

        TAlignContent aligncontent = TAlignContent.Start;
        /// <summary>
        /// 没有占用交叉轴上所有可用的空间时对齐容器内的各项(垂直)
        /// </summary>
        [Description("没有占用交叉轴上所有可用的空间时对齐容器内的各项(垂直)"), Category("布局"), DefaultValue(TAlignContent.Start)]
        public TAlignContent AlignContent
        {
            get => aligncontent;
            set
            {
                if (aligncontent == value) return;
                aligncontent = value;
                LoadLayout();
                Invalidate();
            }
        }

        #endregion

        bool pauseLayout = false;
        [Browsable(false), Description("暂停布局"), Category("行为"), DefaultValue(false)]
        public bool PauseLayout
        {
            get => pauseLayout;
            set
            {
                if (pauseLayout == value) return;
                pauseLayout = value;
                if (!value)
                {
                    LoadLayout();
                    Invalidate();
                }
            }
        }

        #endregion

        #region 布局

        protected override void OnSizeChanged(EventArgs e)
        {
            LoadLayout();
            base.OnSizeChanged(e);
        }

        public void LoadLayout()
        {
            if (IsHandleCreated)
            {
                if (items == null || items.Count == 0) { scroll.Value = 0; return; }
                if (pauseLayout) return;
                var controls = new List<VirtualItem>(items.Count);
                foreach (var it in items)
                {
                    it.SHOW = false;
                    if (it.Visible) controls.Add(it);
                }
                if (controls.Count > 0)
                {
                    int val = HandLayout(controls);
                    scroll.SetVrSize(val);
                }
            }
        }

        int HandLayout(List<VirtualItem> items)
        {
            var _rect = ClientRectangle;
            if (_rect.Width == 0 || _rect.Height == 0) return 0;
            scroll.SizeChange(_rect);
            var rect = _rect.PaddingRect(Padding);
            return Helper.GDI(g =>
            {
                int gap = (int)Math.Round(Gap * Config.Dpi), use_x = rect.X, use_y = rect.Y + gap, last_len = 0, max_height = 0;
                int shadow = (int)(Shadow * Config.Dpi), shadow2 = shadow * 2, r = (int)(radius * Config.Dpi);
                var rows = new List<RItem>();
                var tmps = new List<VirtualItem>(items.Count);
                foreach (var it in items)
                {
                    var size = it.Size(g, new VirtualPanelArgs(this, rect, r));
                    it.WIDTH = size.Width;
                    it.HEIGHT = size.Height;
                    if (tmps.Count > 0 && use_x + size.Width > rect.Width)
                    {
                        rows.Add(new RItem(use_x, use_y, max_height, tmps, true));
                        tmps.Clear();
                        use_x = rect.X;
                        use_y += max_height + gap;
                        max_height = 0;
                    }
                    if (max_height < it.HEIGHT) max_height = it.HEIGHT;
                    if (it is VirtualShadowItem virtualShadow)
                    {
                        it.RECT.Width = size.Width - shadow2;
                        it.RECT.Height = size.Height - shadow2;
                        it.RECT.X = use_x + shadow;
                        it.RECT.Y = use_y + shadow;

                        virtualShadow.RECT_S.Size = size;
                        virtualShadow.RECT_S.X = use_x;
                        virtualShadow.RECT_S.Y = use_y;
                    }
                    else
                    {
                        it.RECT.Size = size;
                        it.RECT.X = use_x;
                        it.RECT.Y = use_y;
                    }
                    use_x += size.Width + gap;
                    last_len = use_y + size.Height + gap;
                    it.SHOW = true;
                    tmps.Add(it);
                }
                if (tmps.Count > 0) rows.Add(new RItem(use_x, use_y, max_height, tmps));
                tmps.Clear();

                #region 布局

                if (last_len > rect.Height) rect.Height = last_len;

                switch (justifycontent)
                {
                    case TJustifyContent.Start:
                        break;
                    case TJustifyContent.End:
                        foreach (var row in rows)
                        {
                            int x = (rect.Width - row.use_x + gap);
                            HandLayout(rect, row.cel, x, 0);
                        }
                        break;
                    case TJustifyContent.SpaceBetween:
                        foreach (var row in rows)
                        {
                            if (row.cel.Count > 1)
                            {
                                int totalWidth = 0;
                                foreach (var cell in row.cel) totalWidth += cell.RECT.Width;
                                int sp = (rect.Width - totalWidth) / (row.cel.Count - 1);
                                int ux = rect.X;
                                foreach (var item in row.cel)
                                {
                                    HandLayout(rect, item, ux - item.RECT.X, 0);
                                    ux += item.RECT.Width + sp;
                                }
                            }
                        }
                        break;
                    case TJustifyContent.SpaceEvenly:
                        foreach (var row in rows)
                        {
                            if (row.cel.Count > 1)
                            {
                                int totalWidth = 0;
                                foreach (var cell in row.cel) totalWidth += cell.RECT.Width;
                                int sp = (rect.Width - totalWidth) / (row.cel.Count + 1),
                                ux = rect.X + sp;
                                foreach (var item in row.cel)
                                {
                                    HandLayout(rect, item, ux - item.RECT.X, 0);
                                    ux += item.RECT.Width + sp;
                                }
                            }
                            else
                            {
                                int x = (rect.Width - row.use_x + gap) / 2;
                                HandLayout(rect, row.cel, x, 0);
                            }
                        }
                        break;
                    case TJustifyContent.SpaceAround:
                        foreach (var row in rows)
                        {
                            if (row.cel.Count > 1)
                            {
                                int totalWidth = 0;
                                foreach (var cell in row.cel) totalWidth += cell.RECT.Width;
                                int availableSpace = rect.Width - totalWidth, spaceBetweenItems = availableSpace / row.cel.Count, spaceAroundItems = availableSpace / (row.cel.Count + 1),
                                ux = rect.X + spaceAroundItems / 2;
                                foreach (var item in row.cel)
                                {
                                    HandLayout(rect, item, ux - item.RECT.X, 0);
                                    ux += item.RECT.Width + spaceBetweenItems;
                                }
                            }
                            else
                            {
                                int x = (rect.Width - row.use_x + gap) / 2;
                                HandLayout(rect, row.cel, x, 0);
                            }
                        }
                        break;
                    case TJustifyContent.Center:
                    default:
                        foreach (var row in rows)
                        {
                            int x = (rect.Width - row.use_x + gap) / 2;
                            HandLayout(rect, row.cel, x, 0);
                        }
                        break;
                }

                switch (aligncontent)
                {
                    case TAlignContent.Start:
                        break;
                    case TAlignContent.End:
                        int yEnd = rect.Height - GetTotalHeight(rows);
                        foreach (var row in rows) HandLayout(rect, row.cel, 0, yEnd);
                        break;
                    case TAlignContent.SpaceBetween:
                        if (rows.Count > 1)
                        {
                            int totalHeight = GetTotalHeight(rows);
                            int sp = (rect.Height - totalHeight) / (rows.Count - 1);
                            int uy = rect.Y;
                            foreach (var row in rows)
                            {
                                foreach (var item in row.cel) HandLayout(rect, item, 0, uy - item.RECT.Y);
                                uy += row.h + sp;
                            }
                        }
                        break;
                    case TAlignContent.SpaceEvenly:
                        if (rows.Count > 1)
                        {
                            int totalHeight = GetTotalHeight(rows);
                            int sp = (rect.Height - totalHeight) / (rows.Count + 1),
                            uy = rect.Y + sp;
                            foreach (var row in rows)
                            {
                                foreach (var item in row.cel) HandLayout(rect, item, 0, uy - item.RECT.Y);
                                uy += row.h + sp;
                            }
                        }
                        else
                        {
                            int yCenter2 = (rect.Height - GetTotalHeight(rows)) / 2;
                            foreach (var row in rows) HandLayout(rect, row.cel, 0, yCenter2);
                        }
                        break;
                    case TAlignContent.SpaceAround:
                        if (rows.Count > 1)
                        {
                            int Height = GetTotalHeight(rows);
                            int availableSpace = rect.Height - Height, spaceBetweenItems = availableSpace / rows.Count, spaceAroundItems = availableSpace / (rows.Count + 1),
                            uy = rect.Y + spaceAroundItems / 2;
                            foreach (var row in rows)
                            {
                                foreach (var item in row.cel) HandLayout(rect, item, 0, uy - item.RECT.Y);
                                uy += row.h + spaceBetweenItems;
                            }
                        }
                        else
                        {
                            int yCenter2 = (rect.Height - GetTotalHeight(rows)) / 2;
                            foreach (var row in rows) HandLayout(rect, row.cel, 0, yCenter2);
                        }
                        break;
                    case TAlignContent.Center:
                    default:
                        int yCenter = (rect.Height - GetTotalHeight(rows)) / 2;
                        foreach (var row in rows) HandLayout(rect, row.cel, 0, yCenter);
                        break;
                }

                if (waterfall)
                {
                    var celdir = new Dictionary<int, int>(rows[0].cel.Count);
                    for (int i = 1; i < rows.Count; i++)
                    {
                        var rowold = rows[i - 1];
                        var row = rows[i];
                        if (rowold.cel.Count >= row.cel.Count)
                        {
                            for (int j = 0; j < row.cel.Count; j++)
                            {
                                int rj = j;
                                if (rowold.cel.Count > row.cel.Count)
                                {
                                    switch (justifycontent)
                                    {
                                        case TJustifyContent.Start:
                                            break;
                                        case TJustifyContent.End:
                                            rj = rowold.cel.Count - 1 - j;
                                            break;
                                        case TJustifyContent.Center:
                                            rj = rowold.cel.Count / 2;
                                            break;
                                        default:
                                            HandLayout(rect, row.cel[j], rowold.cel[j].RECT.X - row.cel[j].RECT.X, 0);
                                            break;
                                    }
                                }

                                var item = rowold.cel[rj];
                                if (item.HEIGHT < rowold.h)
                                {
                                    int xc = rowold.h - item.HEIGHT;
                                    celdir.TryGetValue(rj, out int tmpY);
                                    HandLayout(rect, row.cel[j], 0, -xc - tmpY);
                                    if (celdir.ContainsKey(rj)) celdir[rj] += xc;
                                    else celdir.Add(rj, xc);
                                }
                                else if (celdir.TryGetValue(rj, out int tmpY)) HandLayout(rect, row.cel[j], 0, -tmpY);
                            }
                        }
                    }
                }
                else
                {
                    switch (alignitems)
                    {
                        case TAlignItems.Start:
                            break;
                        case TAlignItems.End:
                            foreach (var row in rows)
                            {
                                foreach (var cel in row.cel)
                                {
                                    int y = row.h - cel.RECT.Height;
                                    HandLayout(rect, cel, 0, y);
                                }
                            }
                            break;
                        case TAlignItems.Center:
                        default:
                            foreach (var row in rows)
                            {
                                foreach (var cel in row.cel)
                                {
                                    int y = (row.h - cel.RECT.Height) / 2;
                                    HandLayout(rect, cel, 0, y);
                                }
                            }
                            break;
                    }
                }

                #endregion

                return last_len + gap * 2;
            });
        }

        int GetTotalHeight(List<RItem> rows)
        {
            int totalHeight = 0;
            foreach (var row in rows) totalHeight += row.h;
            return totalHeight;
        }
        void HandLayout(Rectangle rect, List<VirtualItem> d, int x, int y)
        {
            if (x == 0 && y == 0) return;
            foreach (var item in d)
            {
                if (item.WIDTH == rect.Width)
                {
                    if (y != 0)
                    {
                        if (item is VirtualShadowItem virtualShadow2) virtualShadow2.RECT_S.Offset(0, y);
                        item.RECT.Offset(0, y);
                    }
                }
                else
                {
                    if (item is VirtualShadowItem virtualShadow2) virtualShadow2.RECT_S.Offset(x, y);
                    item.RECT.Offset(x, y);
                }
            }
        }
        void HandLayout(Rectangle rect, VirtualItem d, int x, int y)
        {
            if (d.WIDTH == rect.Width) x = 0;
            if (x == 0 && y == 0) return;
            if (d is VirtualShadowItem virtualShadow2) virtualShadow2.RECT_S.Offset(x, y);
            d.RECT.Offset(x, y);
        }

        class RItem
        {
            public RItem(int usex, int usey, int height, List<VirtualItem> cell, bool _wrap = false)
            {
                use_x = usex;
                use_y = usey;
                h = height;
                cel = new List<VirtualItem>(cell);
                wrap = _wrap;
            }

            public bool wrap { get; set; }
            public int use_x { get; set; }
            public int use_y { get; set; }
            public int h { get; set; }
            public List<VirtualItem> cel { get; set; }
        }

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            var g = e.Graphics.High();
            int sy = scroll.Value;
            var rect = ClientRectangle;
            rect.Offset(0, sy);
            g.TranslateTransform(0, -sy);
            int r = (int)(radius * Config.Dpi);
            foreach (var it in items)
            {
                if (it.SHOW)
                {
                    it.SHOW_RECT = rect.Contains(rect.X, it.RECT.Y) || rect.Contains(rect.X, it.RECT.Bottom);
                    if (it.SHOW_RECT)
                    {
                        if (it is VirtualShadowItem virtualShadow) DrawShadow(virtualShadow, g, r);
                        it.Paint(g, new VirtualPanelArgs(this, it.RECT, r));
                    }
                }
                else it.SHOW_RECT = false;
            }
            g.ResetTransform();
            scroll.Paint(g);
            base.OnPaint(e);
        }

        Dictionary<string, Bitmap> shadow_dir_tmp = new Dictionary<string, Bitmap>();
        /// <summary>
        /// 绘制阴影
        /// </summary>
        void DrawShadow(VirtualShadowItem it, Graphics g, float radius)
        {
            if (shadow > 0)
            {
                string id = it.RECT_S.Width.ToString() + "_" + it.RECT_S.Height.ToString();
                lock (shadow_dir_tmp)
                {
                    if (!shadow_dir_tmp.ContainsKey(id))
                    {
                        int shadow = (int)(Shadow * Config.Dpi);
                        using (var path = new Rectangle(shadow, shadow, it.RECT.Width, it.RECT.Height).RoundPath(radius, shadowAlign))
                        {
                            shadow_dir_tmp.Add(id, path.PaintShadow(it.RECT_S.Width, it.RECT_S.Height, shadowColor ?? Style.Db.TextBase, shadow));
                        }
                    }
                    if (shadow_dir_tmp.TryGetValue(id, out var shadow_temp))
                    {
                        using (var attributes = new ImageAttributes())
                        {
                            var matrix = new ColorMatrix();
                            if (it.AnimationHover) matrix.Matrix33 = it.AnimationHoverValue;
                            else if (it.Hover) matrix.Matrix33 = shadowOpacityHover;
                            else matrix.Matrix33 = shadowOpacity;
                            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            g.DrawImage(shadow_temp, new Rectangle(it.RECT_S.X + shadowOffsetX, it.RECT_S.Y + shadowOffsetY, it.RECT_S.Width, it.RECT_S.Height), 0, 0, shadow_temp.Width, shadow_temp.Height, GraphicsUnit.Pixel, attributes);
                        }
                    }
                }
            }
        }

        #endregion

        #region 鼠标

        /// <summary>
        /// 点击项时发生
        /// </summary>
        [Description("点击项时发生"), Category("行为")]
        public event VirtualItemEventHandler? ItemClick = null;

        VirtualItem? MDown = null;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MDown = null;
            if (scroll.MouseDown(e.Location))
            {
                if (items == null || items.Count == 0) return;
                int x = e.X, y = e.Y + scroll.Value;
                foreach (var it in items)
                {
                    if (it.SHOW && it.SHOW_RECT && it.CanClick && it.RECT.Contains(x, y))
                    {
                        MDown = it;
                        return;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (scroll.MouseMove(e.Location))
            {
                if (items == null || items.Count == 0) return;
                int x = e.X, y = e.Y + scroll.Value;
                int count = 0, hand = 0;
                foreach (var it in items)
                {
                    if (it.SHOW && it.SHOW_RECT && it.CanClick && it.RECT.Contains(x, y))
                    {
                        hand++;
                        if (!it.Hover)
                        {
                            it.Hover = true;
                            count++;
                            SetHover(it, true);
                        }
                    }
                    else
                    {
                        if (it.Hover)
                        {
                            it.Hover = false;
                            count++;
                            SetHover(it, false);
                        }
                    }
                }

                if (count > 0) Invalidate();
                SetCursor(hand > 0);
            }
        }
        void SetHover(VirtualItem it, bool value)
        {
            if (Enabled && ShadowOpacityAnimation && shadow > 0 && shadowOpacityHover > 0 && it is VirtualShadowItem virtualShadow && shadowOpacityHover > shadowOpacity)
            {
                if (Config.Animation)
                {
                    virtualShadow.ThreadHover?.Dispose();
                    virtualShadow.AnimationHover = true;
                    float addvalue = shadowOpacityHover / 12F;
                    if (value)
                    {
                        virtualShadow.ThreadHover = new ITask(this, () =>
                        {
                            virtualShadow.AnimationHoverValue = virtualShadow.AnimationHoverValue.Calculate(addvalue);
                            if (virtualShadow.AnimationHoverValue >= shadowOpacityHover) { virtualShadow.AnimationHoverValue = shadowOpacityHover; return false; }
                            Invalidate();
                            return true;
                        }, 20, () =>
                        {
                            virtualShadow.AnimationHover = false;
                            Invalidate();
                        });
                    }
                    else
                    {
                        virtualShadow.ThreadHover = new ITask(this, () =>
                        {
                            virtualShadow.AnimationHoverValue = virtualShadow.AnimationHoverValue.Calculate(-addvalue);
                            if (virtualShadow.AnimationHoverValue <= shadowOpacity) { virtualShadow.AnimationHoverValue = shadowOpacity; return false; }
                            Invalidate();
                            return true;
                        }, 20, () =>
                        {
                            virtualShadow.AnimationHover = false;
                            Invalidate();
                        });
                    }
                }
                else Invalidate();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (scroll.MouseUp())
            {
                if (MDown != null) ItemClick?.Invoke(this, new VirtualItemEventArgs(MDown, e));
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            scroll.Leave();
            ILeave();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            scroll.Leave();
            ILeave();
        }

        void ILeave()
        {
            if (items == null || items.Count == 0) return;
            int count = 0;
            foreach (var it in items)
            {
                if (it.Hover)
                {
                    it.Hover = false;
                    count++;
                    SetHover(it, false);
                }
            }

            if (count > 0) Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scroll.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion
    }

    public class VirtualCollection : iCollection<VirtualItem>
    {
        public VirtualCollection(VirtualPanel it)
        {
            BindData(it);
        }

        internal VirtualCollection BindData(VirtualPanel it)
        {
            action = render =>
            {
                if (render) it.LoadLayout();
                it.Invalidate();
            };
            return this;
        }
    }

    public abstract class VirtualShadowItem : VirtualItem
    {
        internal Rectangle RECT_S;
        internal ITask? ThreadHover = null;
        internal float AnimationHoverValue = 0.1F;
        internal bool AnimationHover = false;
    }
    public abstract class VirtualItem
    {
        public bool Visible { get; set; } = true;
        public bool CanClick { get; set; } = true;
        public bool Hover { get; set; }
        public object? Tag { get; set; }
        public abstract Size Size(Graphics g, VirtualPanelArgs e);
        public abstract void Paint(Graphics g, VirtualPanelArgs e);

        internal bool SHOW = false;
        internal bool SHOW_RECT = false;
        internal Rectangle RECT;
        internal int WIDTH;
        internal int HEIGHT;
    }

    public class VirtualPanelArgs : EventArgs
    {
        public VirtualPanelArgs(VirtualPanel panel, Rectangle rect, int radius)
        {
            Panel = panel;
            Rect = rect;
            Radius = radius;
        }
        public VirtualPanel Panel { get; private set; }
        public Rectangle Rect { get; private set; }
        public int Radius { get; private set; }
    }
}
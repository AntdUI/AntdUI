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
using System.Linq;
using System.Threading;
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
    public class VirtualPanel : IControl, IEventListener
    {
        public VirtualPanel()
        {
            ScrollBar = new ScrollBar(this);
            invalidate = (it, rect) =>
            {
                if (rect.HasValue) Invalidate(new Rectangle(it.RECT.X + rect.Value.X - ScrollBar.ValueX, it.RECT.Y + rect.Value.Y - ScrollBar.ValueY, rect.Value.Width, rect.Value.Height));
                else Invalidate(new Rectangle(it.RECT.X - ScrollBar.ValueX, it.RECT.Y - ScrollBar.ValueY, it.RECT.Width, it.RECT.Height));
            };
            new Thread(LongTask)
            {
                IsBackground = true
            }.Start();
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
                OnPropertyChanged(nameof(Radius));
            }
        }

        TAlignRound radiusAlign = TAlignRound.ALL;
        [Description("圆角方向"), Category("外观"), DefaultValue(TAlignRound.ALL)]
        public TAlignRound RadiusAlign
        {
            get => radiusAlign;
            set
            {
                if (radiusAlign == value) return;
                radiusAlign = value;
                Invalidate();
                OnPropertyChanged(nameof(RadiusAlign));
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
                OnPropertyChanged(nameof(Shadow));
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
                OnPropertyChanged(nameof(ShadowColor));
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
                OnPropertyChanged(nameof(ShadowOffsetX));
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
                OnPropertyChanged(nameof(ShadowOffsetY));
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
                OnPropertyChanged(nameof(ShadowOpacity));
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
                OnPropertyChanged(nameof(ShadowOpacityHover));
            }
        }

        TAlignMini shadowAlign = TAlignMini.None;
        /// <summary>
        /// 阴影方向
        /// </summary>
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
                OnPropertyChanged(nameof(ShadowAlign));
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
                OnPropertyChanged(nameof(Gap));
            }
        }

        #region 为空

        bool isEmpty = false;

        [Description("是否显示空样式"), Category("外观"), DefaultValue(false)]
        public bool Empty { get; set; }

        string? emptyText;
        [Description("数据为空显示文字"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? EmptyText
        {
            get => emptyText;
            set
            {
                if (emptyText == value) return;
                emptyText = value;
                Invalidate();
                OnPropertyChanged(nameof(EmptyText));
            }
        }

        [Description("数据为空显示图片"), Category("外观"), DefaultValue(null)]
        public Image? EmptyImage { get; set; }

        #endregion

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
                OnPropertyChanged(nameof(PauseLayout));
            }
        }

        /// <summary>
        /// 滚动条
        /// </summary>
        [Browsable(false)]
        public ScrollBar ScrollBar;

        #endregion

        #region 布局

        protected override void OnSizeChanged(EventArgs e)
        {
            CellCount = -1;
            LoadLayout();
            base.OnSizeChanged(e);
        }

        public void LoadLayout()
        {
            if (IsHandleCreated)
            {
                if (items == null || items.Count == 0)
                {
                    ScrollBar.Value = 0;
                    return;
                }
                if (pauseLayout) return;
                var controls = new List<VirtualItem>(items.Count);
                foreach (var it in items)
                {
                    it.SHOW = false;
                    if (it.Visible) controls.Add(it);
                }
                if (controls.Count > 0)
                {
                    isEmpty = false;
                    HandLayout(controls);
                }
                else isEmpty = true;
            }
        }

        internal int CellCount = -1;
        Action<VirtualItem, Rectangle?> invalidate;
        void HandLayout(List<VirtualItem> items)
        {
            var _rect = ClientRectangle;
            if (_rect.Width == 0 || _rect.Height == 0) return;
            var rect = _rect.PaddingRect(Padding);
            var val = this.GDI(g =>
            {
                int gap = (int)Math.Round(Gap * Dpi), use_x = rect.X, use_y = rect.Y + gap, last_len = 0, max_height = 0;
                int shadow = (int)(Shadow * Dpi), shadow2 = shadow * 2, r = (int)(radius * Dpi);
                var rows = new List<RItem>();
                var tmps = new List<VirtualItem>(items.Count);
                foreach (var it in items)
                {
                    var size = it.Size(g, new VirtualPanelArgs(this, rect, r));
                    it.WIDTH = size.Width;
                    it.HEIGHT = size.Height;
                    it.invalidate = invalidate;
                }
                if (waterfall)
                {
                    if (CellCount == -1)
                    {
                        var counts = new List<int>(items.Count);
                        int count = 0;
                        foreach (var it in items)
                        {
                            if (use_x + it.WIDTH >= rect.Width)
                            {
                                use_x = rect.X;
                                use_y += max_height + gap;
                                if (count > 0) counts.Add(count);
                                count = 0;
                            }
                            use_x += it.WIDTH + gap;
                            count++;
                        }
                        use_x = rect.X;
                        use_y = rect.Y + gap;
                        if (counts.Count > 0) CellCount = counts.Max();
                    }
                }
                foreach (var it in items)
                {
                    if (tmps.Count > 0 && use_x + it.WIDTH >= rect.Width)
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
                        it.SetRECT(use_x + shadow, use_y + shadow, it.WIDTH - shadow2, it.HEIGHT - shadow2);
                        virtualShadow.SetRECTS(use_x, use_y, it.WIDTH, it.HEIGHT);
                    }
                    else it.SetRECT(use_x, use_y, it.WIDTH, it.HEIGHT);
                    use_x += it.WIDTH + gap;
                    last_len = use_y + it.HEIGHT + gap;
                    it.SHOW = true;
                    tmps.Add(it);
                }
                if (tmps.Count > 0)
                {
                    rows.Add(new RItem(use_x, use_y, max_height, tmps));
                    last_len = use_y + max_height + gap;
                }
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
                                int totalCount = row.cel.Count, totalWidth = 0;
                                if (CellCount == -1 || CellCount <= row.cel.Count) totalWidth = row.cel.Sum(a => a.RECT.Width);
                                else
                                {
                                    totalCount = CellCount;
                                    totalWidth = row.cel.Sum(a => a.RECT.Width) + (CellCount - row.cel.Count) * row.cel[0].RECT.Width;
                                }
                                int sp = (rect.Width - totalWidth) / (totalCount - 1);
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
                        if (waterfall)
                        {
                            foreach (var row in rows)
                            {
                                int totalCount = row.cel.Count, totalWidth = 0;
                                if (CellCount == -1 || CellCount <= row.cel.Count) totalWidth = row.cel.Sum(a => a.RECT.Width);
                                else
                                {
                                    totalCount = CellCount;
                                    totalWidth = row.cel.Sum(a => a.RECT.Width) + (CellCount - row.cel.Count) * row.cel[0].RECT.Width;
                                }
                                int sp = (rect.Width - totalWidth) / (totalCount + 1),
                                ux = rect.X + sp;
                                foreach (var item in row.cel)
                                {
                                    HandLayout(rect, item, ux - item.RECT.X, 0);
                                    ux += item.RECT.Width + sp;
                                }
                            }
                        }
                        else
                        {
                            foreach (var row in rows)
                            {
                                if (row.cel.Count > 1)
                                {
                                    int totalCount = row.cel.Count, totalWidth = totalWidth = row.cel.Sum(a => a.RECT.Width);
                                    int sp = (rect.Width - totalWidth) / (totalCount + 1),
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
                        }
                        break;
                    case TJustifyContent.SpaceAround:
                        foreach (var row in rows)
                        {
                            if (row.cel.Count > 1)
                            {
                                int totalWidth = row.cel.Sum(a => a.RECT.Width);
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

                if (waterfall) last_len = WaterfallLayout(rect, rows);
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
            ScrollBar.SetVrSize(val);
            ScrollBar.SizeChange(_rect);
        }

        #region 瀑布流

        int WaterfallLayout(Rectangle rect, List<RItem> rows)
        {
            switch (justifycontent)
            {
                case TJustifyContent.Start:
                    WaterfallLayoutStart(rect, rows);
                    break;
                case TJustifyContent.End:
                    WaterfallLayoutEnd(rect, rows);
                    break;
                case TJustifyContent.Center:
                default:
                    WaterfallLayoutCenter(rect, rows);
                    break;
            }

            int last_h = 0;
            foreach (var row in rows)
            {
                foreach (var item in row.cel)
                {
                    if (item is VirtualShadowItem shadowItem)
                    {
                        if (last_h < shadowItem.RECT_S.Bottom) last_h = shadowItem.RECT_S.Bottom;
                    }
                    else if (last_h < item.RECT.Bottom) last_h = item.RECT.Bottom;
                }
            }
            return last_h + Padding.Bottom + gap;
        }
        void WaterfallLayoutStart(Rectangle rect, List<RItem> rows)
        {
            var celdir = new Dictionary<int, int>(rows[0].cel.Count);
            for (int i = 1; i < rows.Count; i++)
            {
                RItem row_new = rows[i], row_old = rows[i - 1];
                if (row_old.cel.Count >= row_new.cel.Count)
                {
                    for (int j = 0; j < row_new.cel.Count; j++)
                    {
                        VirtualItem item_new = row_new.cel[j], item_old = row_old.cel[j];
                        if (item_old.HEIGHT < row_old.h)
                        {
                            int xc = row_old.h - item_old.HEIGHT;
                            if (celdir.ContainsKey(j)) celdir[j] += xc;
                            else celdir.Add(j, xc);
                        }
                        if (celdir.TryGetValue(j, out int tmpY)) HandLayout(rect, item_new, 0, -tmpY);
                    }
                }
            }
        }
        void WaterfallLayoutEnd(Rectangle rect, List<RItem> rows)
        {
            var celdir = new Dictionary<int, int>(rows[0].cel.Count);
            for (int i = 1; i < rows.Count; i++)
            {
                RItem row_new = rows[i], row_old = rows[i - 1];
                if (row_old.cel.Count == row_new.cel.Count)
                {
                    for (int j = 0; j < row_new.cel.Count; j++)
                    {
                        VirtualItem item_new = row_new.cel[j], item_old = row_old.cel[j];
                        if (item_old.HEIGHT < row_old.h)
                        {
                            int xc = row_old.h - item_old.HEIGHT;
                            if (celdir.ContainsKey(j)) celdir[j] += xc;
                            else celdir.Add(j, xc);
                        }
                        if (celdir.TryGetValue(j, out int tmpY)) HandLayout(rect, item_new, 0, -tmpY);
                    }
                }
                else if (row_old.cel.Count > row_new.cel.Count)
                {
                    for (int j = 0; j < row_new.cel.Count; j++)
                    {
                        int rj = row_old.cel.Count - row_new.cel.Count + j;
                        VirtualItem item_new = row_new.cel[j], item_old = row_old.cel[rj];
                        if (item_old.HEIGHT < row_old.h)
                        {
                            int xc = row_old.h - item_old.HEIGHT;
                            if (celdir.ContainsKey(rj)) celdir[rj] += xc;
                            else celdir.Add(rj, xc);
                        }
                        if (celdir.TryGetValue(rj, out int tmpY)) HandLayout(rect, item_new, 0, -tmpY);
                    }
                }
            }
        }
        void WaterfallLayoutCenter(Rectangle rect, List<RItem> rows)
        {
            var celdir = new Dictionary<int, int>(rows[0].cel.Count);
            for (int i = 1; i < rows.Count; i++)
            {
                RItem row_new = rows[i], row_old = rows[i - 1];
                if (row_old.cel.Count == row_new.cel.Count)
                {
                    for (int j = 0; j < row_new.cel.Count; j++)
                    {
                        VirtualItem item_new = row_new.cel[j], item_old = row_old.cel[j];
                        if (item_old.HEIGHT < row_old.h)
                        {
                            int xc = row_old.h - item_old.HEIGHT;
                            if (celdir.ContainsKey(j)) celdir[j] += xc;
                            else celdir.Add(j, xc);
                        }
                        if (celdir.TryGetValue(j, out int tmpY)) HandLayout(rect, item_new, 0, -tmpY);
                    }
                }
                else if (row_old.cel.Count > row_new.cel.Count)
                {
                    #region 挑选最短的插入

                    var hasi = new List<int>(row_new.cel.Count);
                    foreach (var item_new in row_new.cel)
                    {
                        int y = -1, rj = 0;
                        for (int j = 0; j < row_old.cel.Count; j++)
                        {
                            if (hasi.Contains(j)) continue;
                            if (row_old.cel[j].RECT.Y < y || y == -1)
                            {
                                rj = j;
                                y = row_old.cel[j].RECT.Y;
                            }
                        }
                        hasi.Add(rj);
                        var item_old = row_old.cel[rj];

                        if (item_old.HEIGHT < row_old.h)
                        {
                            int xc = row_old.h - item_old.HEIGHT;
                            if (celdir.ContainsKey(rj)) celdir[rj] += xc;
                            else celdir.Add(rj, xc);
                        }

                        if (celdir.TryGetValue(rj, out int tmpY))
                        {
                            HandLayout(rect, item_new, item_old.RECT.X - item_new.RECT.X, -tmpY);
                        }
                    }

                    #endregion
                }
            }
        }

        #endregion

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

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            if (items == null || items.Count == 0 || isEmpty)
            {
                if (Empty) e.Canvas.PaintEmpty(e.Rect, Font, Colour.Text.Get(nameof(VirtualPanel), "emptyFore", ColorScheme), EmptyText, EmptyImage);
            }
            else
            {
                int sy = ScrollBar.Value;
                var rect = e.Rect;
                g.TranslateTransform(0, -sy);
                int r = (int)(radius * Dpi);
                foreach (var it in items)
                {
                    if (it.SHOW)
                    {
                        it.SHOW_RECT = rect.IsItemVisible(sy, it.RECT);
                        if (it.SHOW_RECT)
                        {
                            var state = g.Save();
                            if (it is VirtualShadowItem virtualShadow) DrawShadow(virtualShadow, g, r);
                            g.TranslateTransform(it.RECT.X, it.RECT.Y);
                            it.Paint(g, new VirtualPanelArgs(this, new Rectangle(0, 0, it.RECT.Width, it.RECT.Height), r));
                            g.Restore(state);
                        }
                    }
                    else it.SHOW_RECT = false;
                }
                g.ResetTransform();
                ScrollBar.Paint(g, ColorScheme);
                if (Config.HasAnimation(nameof(VirtualPanel), Name) && BlurBar != null) _event.SetWait();
            }
            base.OnDraw(e);
        }

        #region 模糊标题

        public Control? BlurBar;
        ManualResetEvent _event = new ManualResetEvent(false);
        void LongTask()
        {
            while (true)
            {
                if (_event.Wait()) return;
                try
                {
                    if (items != null && items.Count > 0 && BlurBar != null)
                    {
                        int sy = ScrollBar.Value;
                        int BlurBarHeight = BlurBar.Height;
                        if (sy > BlurBarHeight)
                        {
                            sy -= BlurBarHeight;
                            var rect = ClientRectangle;
                            var bmp = new Bitmap(rect.Width, BlurBarHeight);
                            using (var g = Graphics.FromImage(bmp).HighLay(Dpi))
                            {
                                rect.Offset(0, sy);
                                g.TranslateTransform(0, -sy);
                                int r = (int)(radius * Dpi);
                                foreach (var it in items)
                                {
                                    if (it.SHOW && it.SHOW_RECT)
                                    {
                                        var state = g.Save();
                                        if (it is VirtualShadowItem virtualShadow) DrawShadow(virtualShadow, g, r);
                                        g.TranslateTransform(it.RECT.X, it.RECT.Y);
                                        it.Paint(g, new VirtualPanelArgs(this, new Rectangle(0, 0, it.RECT.Width, it.RECT.Height), r));
                                        g.Restore(state);
                                    }
                                }
                                g.ResetTransform();

                                using (var brush = new SolidBrush(Color.FromArgb(45, BlurBar.BackColor)))
                                {
                                    g.Fill(brush, 0, 0, bmp.Width, bmp.Height);
                                }
                            }
                            Helper.Blur(bmp, BlurBarHeight * 6);
                            IBlurBar(BlurBar, bmp);
                        }
                        else IBlurBar(BlurBar, null);
                    }
                    else if (BlurBar != null) IBlurBar(BlurBar, null);
                    _event.ResetWait();
                }
                catch { return; }
            }
        }

        void IBlurBar(Control BlurBar, Bitmap? bmp)
        {
            Invoke(() =>
            {
                BlurBar.BackgroundImage?.Dispose();
                BlurBar.BackgroundImage = bmp;
            });
        }

        protected override void Dispose(bool disposing)
        {
            BlurBar = null;
            _event?.WaitDispose();
            base.Dispose(disposing);
        }

        #endregion

        #region 主题变化

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadLayout();
            this.AddListener();
        }
        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.THEME:
                    if (Config.HasAnimation(nameof(VirtualPanel), Name) && BlurBar != null) _event.SetWait();
                    break;
            }
        }

        #endregion

        Dictionary<string, Bitmap> shadow_dir_tmp = new Dictionary<string, Bitmap>();
        /// <summary>
        /// 绘制阴影
        /// </summary>
        void DrawShadow(VirtualShadowItem it, Canvas g, float radius)
        {
            if (shadow > 0)
            {
                string id = it.RECT_S.Width.ToString() + "_" + it.RECT_S.Height.ToString();
                lock (shadow_dir_tmp)
                {
                    if (!shadow_dir_tmp.ContainsKey(id))
                    {
                        int shadow = (int)(Shadow * Dpi);
                        using (var path = new Rectangle(shadow, shadow, it.RECT.Width, it.RECT.Height).RoundPath(radius, shadowAlign, radiusAlign))
                        {
                            shadow_dir_tmp.Add(id, path.PaintShadowO(it.RECT_S.Width, it.RECT_S.Height, shadowColor ?? Colour.TextBase.Get(nameof(VirtualPanel), ColorScheme), shadow));
                        }
                    }
                    if (shadow_dir_tmp.TryGetValue(id, out var shadow_temp))
                    {
                        float opacity;
                        if (it.AnimationHover) opacity = it.AnimationHoverValue;
                        else if (it.Hover) opacity = shadowOpacityHover;
                        else opacity = shadowOpacity;
                        g.Image(shadow_temp, new Rectangle(it.RECT_S.X + shadowOffsetX, it.RECT_S.Y + shadowOffsetY, it.RECT_S.Width, it.RECT_S.Height), opacity);
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
        public event VirtualItemEventHandler? ItemClick;

        protected virtual void OnItemClick(VirtualItem item, MouseEventArgs e) => ItemClick?.Invoke(this, new VirtualItemEventArgs(item, e));

        VirtualItem? MDown;
        int isdouclick = 0;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MDown = null;
            if (ScrollBar.MouseDown(e.X, e.Y))
            {
                if (items == null || items.Count == 0) return;
                OnTouchDown(e.X, e.Y);
                int x = e.X, y = e.Y + ScrollBar.Value;
                foreach (var it in items)
                {
                    if (it.SHOW && it.SHOW_RECT && it.CanClick && it.RECT.Contains(x, y))
                    {
                        isdouclick = e.Clicks;
                        MDown = it;
                        return;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ScrollBar.MouseMove(e.X, e.Y) && OnTouchMove(e.X, e.Y))
            {
                if (items == null || items.Count == 0) return;
                int x = e.X, y = e.Y + ScrollBar.Value;
                int count = 0, hand = 0;
                foreach (var it in items)
                {
                    if (it.SHOW && it.SHOW_RECT && it.CanClick && it.RECT.Contains(x, y))
                    {
                        if (it.MouseMove(this, new VirtualPanelMouseArgs(it, it.RECT, x, y, e))) hand++;
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
                            it.MouseLeave(this, new VirtualPanelMouseArgs(it, it.RECT, x, y, e));
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
                if (Config.HasAnimation(nameof(VirtualPanel), Name))
                {
                    virtualShadow.ThreadHover?.Dispose();
                    virtualShadow.AnimationHover = true;
                    float addvalue = shadowOpacityHover / 12F;
                    if (value)
                    {
                        virtualShadow.ThreadHover = new AnimationTask(new AnimationLinearFConfig(this, i =>
                        {
                            virtualShadow.AnimationHoverValue = i;
                            Invalidate();
                            return true;
                        }, 20).SetAdd(addvalue).SetMax(shadowOpacityHover).SetValue(virtualShadow.AnimationHoverValue).SetEnd(() =>
                        {
                            virtualShadow.AnimationHover = false;
                            Invalidate();
                        }));
                    }
                    else
                    {
                        virtualShadow.ThreadHover = new AnimationTask(new AnimationLinearFConfig(this, i =>
                        {
                            virtualShadow.AnimationHoverValue = i;
                            Invalidate();
                            return true;
                        }, 20).SetAdd(-addvalue).SetMax(shadowOpacity).SetValue(virtualShadow.AnimationHoverValue).SetEnd(() =>
                        {
                            virtualShadow.AnimationHover = false;
                            Invalidate();
                        }));
                    }
                }
                else Invalidate();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (ScrollBar.MouseUp() && OnTouchUp())
            {
                if (MDown != null)
                {
                    int x = e.X, y = e.Y + ScrollBar.Value;
                    if (MDown.RECT.Contains(x, y))
                    {
                        OnItemClick(MDown, e);
                        MDown.MouseClick(this, new VirtualPanelMouseArgs(MDown, MDown.RECT, x, y, e, isdouclick));
                    }
                }
            }
            MDown = null;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ScrollBar.Leave();
            ILeave();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ScrollBar.Leave();
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
            ScrollBar.MouseWheel(e);
            base.OnMouseWheel(e);
        }
        protected override bool OnTouchScrollX(int value) => ScrollBar.MouseWheelXCore(value);
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelYCore(value);

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
                if (render) { it.CellCount = -1; it.LoadLayout(); }
                it.Invalidate();
            };
            return this;
        }
    }

    public abstract class VirtualShadowItem : VirtualItem
    {
        internal Rectangle RECT_S;
        internal AnimationTask? ThreadHover;
        internal float AnimationHoverValue = 0.1F;
        internal bool AnimationHover = false;

        internal void SetRECTS(int x, int y, int w, int h)
        {
            RECT_S.Width = w;
            RECT_S.Height = h;
            RECT_S.X = x;
            RECT_S.Y = y;
        }
    }
    public abstract class VirtualItem
    {
        public bool Visible { get; set; } = true;
        public bool CanClick { get; set; } = true;
        public bool Hover { get; set; }
        public object? Tag { get; set; }
        public abstract Size Size(Canvas g, VirtualPanelArgs e);
        public abstract void Paint(Canvas g, VirtualPanelArgs e);
        public virtual bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e) => true;
        public virtual void MouseLeave(VirtualPanel sender, VirtualPanelMouseArgs e) { }
        public virtual void MouseClick(VirtualPanel sender, VirtualPanelMouseArgs e) { }

        public void Invalidate() => invalidate?.Invoke(this, null);
        public void Invalidate(Rectangle rect) => invalidate?.Invoke(this, rect);

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool SHOW = false;

        /// <summary>
        /// 是否在容器中渲染
        /// </summary>
        public bool SHOW_RECT = false;

        /// <summary>
        /// 渲染区域
        /// </summary>
        public Rectangle RECT;

        internal void SetRECT(int x, int y, int w, int h)
        {
            RECT.Width = w;
            RECT.Height = h;
            RECT.X = x;
            RECT.Y = y;
        }

        internal Action<VirtualItem, Rectangle?>? invalidate;
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

    public class VirtualPanelMouseArgs : MouseEventArgs
    {
        public VirtualPanelMouseArgs(VirtualItem item, Rectangle rect, int x, int y, MouseEventArgs e) : base(e.Button, e.Clicks, x - rect.X, y - rect.Y, e.Delta)
        {
            Item = item;
            Rect = rect;
        }
        public VirtualPanelMouseArgs(VirtualItem item, Rectangle rect, int x, int y, MouseEventArgs e, int doubleclick) : base(e.Button, doubleclick, x - rect.X, y - rect.Y, e.Delta)
        {
            Item = item;
            Rect = rect;
        }
        public VirtualItem Item { get; private set; }
        public Rectangle Rect { get; private set; }
    }
}
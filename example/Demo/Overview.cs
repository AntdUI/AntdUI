// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Overview : AntdUI.Window
    {
        public Overview(bool top)
        {
            InitializeComponent();
            windowBar.Text += " " + windowBar.ProductVersion;
            TopMost = top;
            var globals = new AntdUI.SelectItem[] {
                new AntdUI.SelectItem("中文","zh-CN"),
                new AntdUI.SelectItem("English","en-US")
            };
            btn_global.Items.AddRange(globals);
            btn_more.Items.AddRange(new AntdUI.SelectItem[] {
                new AntdUI.SelectItem("Github","github").SetIcon(Properties.Resources.icon_github),
                new AntdUI.SelectItem("Gitee","gitee").SetIcon(Properties.Resources.icon_gitee),
                new AntdUI.SelectItem("Gitcode","gitcode").SetIcon(Properties.Resources.icon_gitcode),
                new AntdUI.SelectItem("色板工具","color").SetIcon("BgColorsOutlined"),
                new AntdUI.SelectItem("演示 Demo","m"),
                new AntdUI.SelectItem("演示 Tab","tab"),
            });
            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en")) btn_global.SelectedValue = globals[1].Tag;
            else btn_global.SelectedValue = globals[0].Tag;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            DraggableMouseDown();
            base.OnMouseDown(e);
        }

        private void ItemClick(object sender, AntdUI.VirtualItemEventArgs e)
        {
            if (e.Item is VItem item) OpenPage(item.data.id, item.data.key);
        }

        public void OpenPage(string id, string key)
        {
            var control_add = GetPage(id, key);
            if (control_add == null) return;
            int w = virtualPanel.Width, h = virtualPanel.Height;
            virtualPanel.Visible = false;
            panel_main.Bounds = new Rectangle(-w * 2, -h * 2, w, h);
            windowBar.SubText = id;
            if (windowBar.Tag is Control control)
            {
                control.Dispose();
                Controls.Remove(control);
            }
            windowBar.Tag = control_add;
            windowBar.ShowBack = true;
            OpenPage(control_add);
        }
        async void OpenPage(Control control_add)
        {
            control_add.Dock = DockStyle.Fill;
            AntdUI.Win32.WindowTheme(control_add);
            AutoDpi(control_add);
            panel_main.Controls.Add(control_add);
            await Task.Delay(100);
            panel_main.Dock = DockStyle.Fill;
            control_add.Focus();
        }

        Control GetPage(string id, string key)
        {
            switch (id)
            {
                case "Button":
                    return Helper.AddPage(id, key, "按钮用于开始一个即时操作。", Button.Page(this));
                case "Icon":
                    return new Controls.Icon(this);
                case "Avatar":
                    return new Controls.Avatar(this);
                case "Carousel":
                    return new Controls.Carousel(this);
                case "Badge":
                    return new Controls.Badge(this);
                case "Checkbox":
                    return new Controls.Checkbox(this);
                case "Radio":
                    return new Controls.Radio(this);
                case "Input":
                    return new Controls.Input(this);
                case "Select":
                    return new Controls.Select(this);
                case "Panel":
                    return new Controls.Panel(this);
                case "Progress":
                    return new Controls.Progress(this);
                case "Result":
                    return new Controls.Result(this);
                case "Tooltip":
                    return new Controls.Tooltip(this);
                case "Tour":
                    return new Controls.Tour(this);
                case "Divider":
                    return new Controls.Divider(this);
                case "Slider":
                    return new Controls.Slider(this);
                case "Tabs":
                    return new Controls.Tabs(this);
                case "Switch":
                    return new Controls.Switch(this);
                case "Pagination":
                    return new Controls.Pagination(this);
                case "Alert":
                    return new Controls.Alert(this);
                case "Message":
                    return new Controls.Message(this);
                case "Notification":
                    return new Controls.Notification(this);
                case "Menu":
                    return new Controls.Menu(this);
                case "Segmented":
                    return new Controls.Segmented(this);
                case "Modal":
                    return new Controls.Modal(this);
                case "DatePicker":
                    return new Controls.DatePicker(this);
                case "TimePicker":
                    return new Controls.TimePicker(this);
                case "Dropdown":
                    return new Controls.Dropdown(this);
                case "Tree":
                    return new Controls.Tree(this);
                case "Popover":
                    return new Controls.Popover(this);
                case "Timeline":
                    return new Controls.Timeline(this);
                case "Steps":
                    return new Controls.Steps(this);
                case "ColorPicker":
                    return new Controls.ColorPicker(this);
                case "InputNumber":
                    return new Controls.InputNumber(this);
                case "Tag":
                    return new Controls.Tag(this);
                case "Drawer":
                    return new Controls.Drawer(this);
                case "FloatButton":
                    OpenFloatButton();
                    break;
                case "Rate":
                    return new Controls.Rate(this);
                case "Table":
#if DEBUG
                    return new Controls.Table(this);
#else
                    return new Controls.TableAOT(this);
#endif
                case "Image":
                    return new Controls.Preview(this);
                case "VirtualPanel":
                    return new Controls.VirtualPanel(this);
                case "PageHeader":
                    return new Controls.PageHeader(this);
                case "Breadcrumb":
                    return new Controls.Breadcrumb(this);
                case "Collapse":
                    return new Controls.Collapse(this);

                case "GridPanel":
                    return new Controls.GridPanel(this);
                case "Splitter":
                    return new Controls.Splitter(this);
                case "Calendar":
                    return new Controls.Calendar(this);
                case "Battery":
                    return new Controls.Battery(this);
                case "Signal":
                    return new Controls.Signal(this);
                case "Spin":
                    return new Controls.Spin(this);
                case "ContextMenuStrip":
                    return new Controls.ContextMenuStrip(this);
                case "Shield":
                    return new Controls.Shield(this);
                case "Transfer":
                    return new Controls.Transfer(this);
                case "Chart":
                    return new Controls.Chart(this);
                case "Watermark":
                    return new Controls.Watermark(this);
                case "HyperlinkLabel":
                    return new Controls.HyperlinkLabel(this);
            }
            return null;
        }

        #region 悬浮按钮

        AntdUI.FormFloatButton FloatButton = null, FloatButtonSub = null;

        void OpenFloatButton()
        {
            if (FloatButton == null)
            {
                FloatButton = AntdUI.FloatButton.open(new AntdUI.FloatButton.Config(this, new AntdUI.FloatButton.ConfigBtn[] {
                    new AntdUI.FloatButton.ConfigBtn("id1").SetIcon("ArrowUpOutlined").SetTooltip("展开","FloatButton.Arrow"),
                }, btn => OpenFloatButtonSub()));
            }
            else
            {
                FloatButtonSub?.Close();
                FloatButtonSub = null;
                FloatButton.Close();
                FloatButton = null;
            }
        }
        void OpenFloatButtonSub()
        {
            if (FloatButtonSub == null)
            {
                FloatButtonSub = AntdUI.FloatButton.open(new AntdUI.FloatButton.Config(this, new AntdUI.FloatButton.ConfigBtn[] {
                    new AntdUI.FloatButton.ConfigBtn("id1").SetType().SetIcon("SearchOutlined").SetTooltip("搜索一下","FloatButton.1"),
                    new AntdUI.FloatButton.ConfigBtn("id2").SetBadge().SetIcon(Properties.Resources.img1).SetTooltip("笑死人","FloatButton.2"),
                    new AntdUI.FloatButton.ConfigBtn("id3").SetBadge("9").SetIcon(Properties.Resources.icon_like).SetTooltip("救救我","FloatButton.3"),
                    new AntdUI.FloatButton.ConfigBtn("id4").SetType().SetRound().SetBadge("99+").SetIcon("PoweroffOutlined").SetTooltip("没救了","FloatButton.4")
                }, btn =>
                {
                    btn.Loading = true;
                    AntdUI.ITask.Run(() =>
                    {
                        System.Threading.Thread.Sleep(2000);
                        btn.Loading = false;
                    });
                    AntdUI.Message.info(this, AntdUI.Localization.Get("Click:", "点击了：") + btn.Name, Font);
                })
                {
                    MarginY = 40 * 2 + 24
                });
                if (FloatButton == null) return;
                FloatButton.config.Btns[0].Type = AntdUI.TTypeMini.Primary;
            }
            else
            {
                FloatButtonSub.Close();
                FloatButtonSub = null;
                if (FloatButton == null) return;
                FloatButton.config.Btns[0].Type = AntdUI.TTypeMini.Default;
            }
        }

        #endregion

        private void btn_back_Click(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                if (windowBar.Tag is Control control)
                {
                    control.Dispose();
                    Controls.Remove(control);
                }
                windowBar.ShowBack = false;
                panel_main.Dock = DockStyle.None;
                panel_main.Bounds = new Rectangle(-100, -100, 0, 0);
                virtualPanel.Visible = true;
                windowBar.SubText = "Overview";
            }));
        }

        private void btn_mode_Click(object sender, EventArgs e)
        {
            btn_mode.Toggle = AntdUI.Config.IsDark = !AntdUI.Config.IsDark;
            OnSizeChanged(e);
        }

        private void colorTheme_ValueChanged(object sender, AntdUI.ColorEventArgs e)
        {
            AntdUI.Style.SetPrimary(e.Value);
            Refresh();
        }

        private void btn_setting_Click(object sender, EventArgs e)
        {
            var setting = new Setting(this);
            if (AntdUI.Modal.open(this, AntdUI.Localization.Get("Setting", "设置"), setting) == DialogResult.OK)
            {
                AntdUI.Config.Animation = setting.Animation;
                AntdUI.Config.ShadowEnabled = setting.ShadowEnabled;
                AntdUI.Config.ShowInWindow = setting.ShowInWindow;
                AntdUI.Config.ScrollBarHide = setting.ScrollBarHide;
                if (AntdUI.Config.TextRenderingHighQuality == setting.TextRenderingHighQuality) return;
                AntdUI.Config.TextRenderingHighQuality = setting.TextRenderingHighQuality;
                Refresh();
            }
        }

        private void btn_global_Changed(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is string lang)
            {
                if (lang.StartsWith("en")) AntdUI.Localization.Provider = new Localizer();
                else AntdUI.Localization.Provider = null;
                AntdUI.Localization.SetLanguage(lang);
                Refresh();
            }
        }

        private void btn_more_Changed(object sender, AntdUI.ObjectNEventArgs e)
        {
            btn_more.SelectedValue = null;
            if (e.Value is string code)
            {
                BeginInvoke(() =>
                {
                    switch (code)
                    {
                        case "github":
                            Process.Start(new ProcessStartInfo("https://github.com/AntdUI/AntdUI")
                            {
                                UseShellExecute = true
                            });
                            break;
                        case "gitee":
                            Process.Start(new ProcessStartInfo("https://gitee.com/AntdUI/AntdUI")
                            {
                                UseShellExecute = true
                            });
                            break;
                        case "gitcode":
                            Process.Start(new ProcessStartInfo("https://gitcode.com/AntdUI/AntdUI")
                            {
                                UseShellExecute = true
                            });
                            break;
                        case "color":
                            new Colors().Show();
                            break;
                        case "m":
                            new Main().Show();
                            break;
                        case "tab":
                            new TabHeaderForm().Show();
                            break;
                    }
                });
            }
        }

        #region 搜索

        private void txt_search_PrefixClick(object sender, MouseEventArgs e) => LoadSearchList();

        private void txt_search_TextChanged(object sender, EventArgs e) => LoadSearchList();

        void LoadSearchList()
        {
            string search = txt_search.Text.Trim();
            windowBar.Loading = true;
            BeginInvoke(new Action(() =>
            {
                virtualPanel.PauseLayout = true;
                if (string.IsNullOrEmpty(search))
                {
                    foreach (var it in virtualPanel.Items)
                    {
                        it.ResetVisible();
                        if (it is AntdUI.TItem itemTitle) itemTitle.Restore();
                    }
                    virtualPanel.Empty = false;
                }
                else
                {
                    virtualPanel.Empty = true;
                    var titles = new List<AntdUI.TItem>(virtualPanel.Items.Count);
                    var listSearch = new List<AntdUI.ItemSearchWeigth<AntdUI.VirtualItem>>(virtualPanel.Items.Count);
                    foreach (var it in virtualPanel.Items)
                    {
                        if (it is VItem item)
                        {
                            int score = AntdUI.Helper.SearchContains(search, item.data.id, item.data.keyword, out _);
                            it.Visible = score > 0;
                            if (it.Visible) listSearch.Add(new AntdUI.ItemSearchWeigth<AntdUI.VirtualItem>(score, it).SetGroup(item.group));
                        }
                        else if (it is AntdUI.TItem itemTitle) titles.Add(itemTitle);

                    }
                    AntdUI.Helper.SearchWeightSortByVirtualItem(listSearch);
                    foreach (var it in titles)
                    {
                        it.SortIndex = -1;
                        int count = 0;
                        foreach (var item in it.data)
                        {
                            if (item.Visible)
                            {
                                count++;
                                if (it.SortIndex == -1 || it.SortIndex > item.SortIndex) it.SortIndex = item.SortIndex;
                            }
                        }
                        it.SetCount(count);
                    }
                }
                virtualPanel.PauseLayout = false;
                windowBar.Loading = false;
            }));
        }

        #endregion

        #region 加载列表

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            windowBar.Loading = true;
            AntdUI.ITask.Run(LoadList);
        }

        void LoadList()
        {
            string pid = nameof(AntdUI) + " - " + windowBar.ProductVersion;
            IList[] dir_General = new IList[]
            {
                //通用
                new IList("Button", "按钮", res_light.Button, res_dark.Button),
                new IList("FloatButton", "悬浮按钮", res_light.FloatButton, res_dark.FloatButton),
                new IList("Icon", "图标", res_light.Icon, res_dark.Icon)
            },
            dir_Layout = new IList[]
            {
                //布局
                new IList("Divider", "分割线", res_light.Divider, res_dark.Divider),
                new IList("GridPanel", "格栅布局", res_light.Grid, res_dark.Grid),
                new IList("Splitter", "分隔面板", res_light.Splitter, res_dark.Splitter)
            },
            dir_Navigation = new IList[]
            {
                //导航
                new IList("Breadcrumb", "面包屑", res_light.Breadcrumb, res_dark.Breadcrumb),
                new IList("Dropdown", "下拉菜单", res_light.Dropdown, res_dark.Dropdown),
                new IList("Menu", "导航菜单", res_light.Menu, res_dark.Menu),
                new IList("PageHeader", "页头", res_light.PageHeader, res_dark.PageHeader),
                new IList("Pagination", "分页", res_light.Pagination, res_dark.Pagination),
                new IList("Steps", "步骤条", res_light.Steps, res_dark.Steps)
            },
            dir_DataEntry = new IList[]
            {
                //数据录入
                new IList("Checkbox", "多选框", res_light.Checkbox, res_dark.Checkbox),
                new IList("ColorPicker", "颜色选择器", res_light.ColorPicker, res_dark.ColorPicker),
                new IList("DatePicker", "日期选择框", res_light.DatePicker, res_dark.DatePicker),
                new IList("Input", "输入框", res_light.Input, res_dark.Input),
                new IList("InputNumber", "数字输入框", res_light.InputNumber, res_dark.InputNumber),
                new IList("Radio", "单选框", res_light.Radio, res_dark.Radio),
                new IList("Rate", "评分", res_light.Rate, res_dark.Rate),
                new IList("Select", "选择器", res_light.Select, res_dark.Select),
                new IList("Slider", "滑动输入条", res_light.Slider, res_dark.Slider),
                new IList("Switch", "开关", res_light.Switch, res_dark.Switch),
                new IList("TimePicker", "时间选择框", res_light.TimePicker, res_dark.TimePicker),
                new IList("Transfer", "穿梭框", res_light.Transfer, res_dark.Transfer)

            },
            dir_DataDisplay = new IList[]
            {
                //数据展示
                new IList("Avatar", "头像", res_light.Avatar, res_dark.Avatar),
                new IList("Badge", "徽标数", res_light.Badge, res_dark.Badge),
                new IList("Calendar", "日历", res_light.Calendar, res_dark.Calendar),
                new IList("Panel", "面板", res_light.Panel, res_dark.Panel),
                new IList("Carousel", "走马灯", res_light.Carousel, res_dark.Carousel),
                new IList("Collapse", "折叠面板", res_light.Collapse, res_dark.Collapse),
                new IList("Image", "图片", res_light.Image, res_dark.Image),
                new IList("Popover", "气泡卡片", res_light.Popover, res_dark.Popover),
                new IList("Segmented", "分段控制器", res_light.Segmented, res_dark.Segmented),
                new IList("Table", "表格", res_light.Table, res_dark.Table),
                new IList("Tabs", "标签页", res_light.Tabs, res_dark.Tabs),
                new IList("Tag", "标签", res_light.Tag, res_dark.Tag),
                new IList("Timeline", "时间轴", res_light.Timeline, res_dark.Timeline),
                new IList("Tooltip", "文字提示", res_light.Tooltip, res_dark.Tooltip),
                new IList("Tour", "漫游式引导", res_light.Tour, res_dark.Tour),
                new IList("Tree", "树形控件", res_light.Tree, res_dark.Tree),
                new IList("HyperlinkLabel", "超链接文本", res_light.HyperlinkLabel, res_dark.HyperlinkLabel),
                new IList("Chart", "图表", res_light.Chart, res_dark.Chart)
            },
            dir_Feedback = new IList[]
            {
                //反馈
                new IList("Alert", "警告提示", res_light.Alert, res_dark.Alert),
                new IList("Drawer", "抽屉", res_light.Drawer, res_dark.Drawer),
                new IList("Message", "全局提示", res_light.Message, res_dark.Message),
                new IList("Modal", "对话框", res_light.Modal, res_dark.Modal),
                new IList("Notification", "通知提醒框", res_light.Notification, res_dark.Notification),
                new IList("Progress", "进度条", res_light.Progress, res_dark.Progress),
                new IList("Result", "结果", res_light.Result, res_dark.Result),
                new IList("Spin", "加载中", res_light.Spin, res_dark.Spin),
                new IList("Watermark", "水印", res_light.Watermark.Replace("Ant Design", pid), res_dark.Watermark.Replace("Ant Design", pid)),
            },
            dir_Other = new IList[]
            {
                //反馈
                new IList("Battery", "电量", res_light.Battery, res_dark.Battery),
                new IList("Signal", "信号强度", res_light.Singal, res_dark.Signal),
                new IList("ContextMenuStrip", "右键菜单", res_light.Menu, res_dark.Menu),
                new IList("Shield", "徽章", ShieldSvg("#1677ff"), ShieldSvg("#1668dc")),
            };


            var dir = new Dictionary<string, IList[]> {
                { "General", dir_General },
                { "Layout", dir_Layout },
                { "Navigation", dir_Navigation },
                { "DataEntry", dir_DataEntry },
                { "DataDisplay", dir_DataDisplay },
                { "Feedback", dir_Feedback },
                { "Other", dir_Other }
            };

            var list = new List<AntdUI.VirtualItem>(dir.Count + dir_General.Length + dir_Layout.Length + dir_Navigation.Length + dir_DataEntry.Length + dir_DataDisplay.Length + dir_Feedback.Length);

            foreach (var it in dir)
            {
                var list_sub = new List<AntdUI.VirtualItem>(it.Value.Length);
                foreach (var item in it.Value) list_sub.Add(new VItem(item, it.Key));
                list.Add(new AntdUI.TItem(GetChinaGroup(it.Key), it.Key, list_sub.ToArray()));
                list.AddRange(list_sub);
            }
            virtualPanel.Items.AddRange(list);
            windowBar.Loading = false;
            virtualPanel.BlurBar = windowBar;
        }
        string GetChinaGroup(string key)
        {
            switch (key)
            {
                case "General":
                    return "通用";
                case "Layout":
                    return "布局";
                case "Navigation":
                    return "导航";
                case "DataEntry":
                    return "数据录入";
                case "DataDisplay":
                    return "数据展示";
                case "Feedback":
                    return "反馈";
                default:
                    return "其他";
            }
        }

        string ShieldSvg(string color)
        {
            string version = windowBar.ProductVersion;
            if (version.Length > 5) return "<svg viewBox=\"0 0 98 20\" width=\"90\" height=\"90\"><g><rect width=\"49\" height=\"20\" fill=\"#555555\"/><rect x=\"49\" width=\"49\" height=\"20\" fill=\"" + color + "\"/></g><g fill=\"#ffffff\" text-anchor=\"middle\" font-family=\"Verdana,Geneva,DejaVu Sans,sans-serif\" font-size=\"110\"><text x=\"255\" y=\"140\" transform=\"scale(.1)\" fill=\"#ffffff\" textLength=\"390\">AntdUI</text><text x=\"725\" y=\"140\" transform=\"scale(.1)\" fill=\"#ffffff\" textLength=\"390\">" + version + "</text></g></svg>";
            return "<svg viewBox=\"0 0 88 20\" width=\"90\" height=\"90\"><g><rect width=\"49\" height=\"20\" fill=\"#555555\"/><rect x=\"49\" width=\"39\" height=\"20\" fill=\"" + color + "\"/></g><g fill=\"#ffffff\" text-anchor=\"middle\" font-family=\"Verdana,Geneva,DejaVu Sans,sans-serif\" font-size=\"110\"><text x=\"255\" y=\"140\" transform=\"scale(.1)\" fill=\"#ffffff\" textLength=\"390\">AntdUI</text><text x=\"675\" y=\"140\" transform=\"scale(.1)\" fill=\"#ffffff\" textLength=\"290\">" + version + "</text></g></svg>";
        }

        class IList
        {
            public IList(string _id, string _key, string _img_light, string _img_dark)
            {
                id = _id;
                key = _key;
                keyword = new string[] {
                    _id.ToLower(),
                    AntdUI.Pinyin.GetPinyin(_key).ToLower(),
                    AntdUI.Pinyin.GetInitials(_key).ToLower()
                };
                imgs = new Image[] { AntdUI.SvgExtend.SvgToBmp(_img_light), AntdUI.SvgExtend.SvgToBmp(_img_dark) };
            }
            public string id { get; set; }
            public string[] keyword { get; set; }
            public string key { get; set; }
            public Image[] imgs { get; set; }
        }

        #region 渲染

        class VItem : AntdUI.VirtualShadowItem
        {
            public IList data;
            public string group;
            string LocalizationName;
            public VItem(IList d, string g)
            {
                data = d;
                LocalizationName = d.id + " " + d.key;
                group = g;
            }

            AntdUI.FormatFlags s_f = AntdUI.FormatFlags.Left | AntdUI.FormatFlags.VerticalCenter;
            public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                using (var brush = new SolidBrush(AntdUI.Style.Db.BgContainer))
                {
                    using (var path = AntdUI.Helper.RoundPath(e.Rect, e.Radius))
                    {
                        g.Fill(brush, path);
                        using (var brush_bor = new Pen(Hover ? AntdUI.Style.Db.BorderColorDisable : AntdUI.Style.Db.BorderColor, thickness))
                        {
                            g.Draw(brush_bor, path);
                        }
                    }
                }
                using (var fore = new SolidBrush(AntdUI.Style.Db.Text))
                {
                    using (var font_title = new Font(e.Panel.Font.FontFamily, 11F, FontStyle.Bold))
                    {
                        g.String(AntdUI.Localization.Get(data.id, LocalizationName), font_title, fore, rect_title, s_f);
                    }
                }
                using (var brush = new SolidBrush(AntdUI.Style.Db.Split))
                {
                    g.Fill(brush, rect_line);
                }
                try
                {
                    var bmp = AntdUI.Config.IsDark ? data.imgs[1] : data.imgs[0];
                    if (bmp.Width > rect_ico.Width && bmp.Height > rect_ico.Height) g.Image(rect_ico, bmp, AntdUI.TFit.Contain);
                    else g.Image(bmp, rect_ico.X + (rect_ico.Width - bmp.Width) / 2, rect_ico.Y + (rect_ico.Height - bmp.Height) / 2, bmp.Width, bmp.Height);
                }
                catch { }
            }

            int thickness;
            Rectangle rect_title, rect_ico;
            RectangleF rect_line;
            public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = g.Dpi;
                thickness = (int)(1 * dpi);
                int title_height = (int)(44 * dpi), size = (int)(10 * dpi), size2 = size * 2, size4 = size2 * 2;

                int shadow = (int)(e.Panel.Shadow * dpi) * 2,
                    iconMax = (int)(116 * dpi), w = (int)(282 * dpi), h = (int)(244 * dpi), rw = w - shadow, rh = h - shadow;

                rect_title = new Rectangle(size2, 0, rw - size4, title_height);
                rect_line = new RectangleF(size, title_height - thickness / 2F, rw - size2, thickness);
                rect_ico = new Rectangle(size, title_height + (rh - title_height - iconMax) / 2, rw - size2, iconMax);
                return new Size(w, h);
            }
        }

        #endregion

        #endregion
    }

    public class ViewPage
    {
        public ViewPage(string name, string text, UserControl control)
        {
            Name = name;
            Text = text;
            Control = control;
        }
        public string Name { get; set; }
        public string Text { get; set; }
        public UserControl Control { get; set; }
    }
}
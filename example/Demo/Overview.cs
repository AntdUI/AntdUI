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
using System.Diagnostics;
using System.Drawing;
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
            colorTheme.ValueChanged += ColorTheme_ValueChanged;
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

        private void ItemClick(object sender, AntdUI.VirtualItemEventArgs e) => OpenPage(e.Item.Tag.ToString());

        //AntdUI.Tabs tabs = null;
        public void OpenPage(string id)
        {
            Control control_add = null;
            switch (id)
            {
                case "Button":
                    control_add = new Controls.Button(this);
                    break;
                case "Icon":
                    control_add = new Controls.Icon(this);
                    break;
                case "Avatar":
                    control_add = new Controls.Avatar(this);
                    break;
                case "Carousel":
                    control_add = new Controls.Carousel(this);
                    break;
                case "Badge":
                    control_add = new Controls.Badge(this);
                    break;
                case "Checkbox":
                    control_add = new Controls.Checkbox(this);
                    break;
                case "Radio":
                    control_add = new Controls.Radio(this);
                    break;
                case "Input":
                    control_add = new Controls.Input(this);
                    break;
                case "Select":
                    control_add = new Controls.Select(this);
                    break;
                case "Panel":
                    control_add = new Controls.Panel(this);
                    break;
                case "Progress":
                    control_add = new Controls.Progress(this);
                    break;
                case "Result":
                    control_add = new Controls.Result(this);
                    break;
                case "Tooltip":
                    control_add = new Controls.Tooltip(this);
                    break;
                case "Tour":
                    control_add = new Controls.Tour(this);
                    break;
                case "Divider":
                    control_add = new Controls.Divider(this);
                    break;
                case "Slider":
                    control_add = new Controls.Slider(this);
                    break;
                case "Tabs":
                    control_add = new Controls.Tabs(this);
                    break;
                case "Switch":
                    control_add = new Controls.Switch(this);
                    break;
                case "Pagination":
                    control_add = new Controls.Pagination(this);
                    break;
                case "Alert":
                    control_add = new Controls.Alert(this);
                    break;
                case "Message":
                    control_add = new Controls.Message(this);
                    break;
                case "Notification":
                    control_add = new Controls.Notification(this);
                    break;
                case "Menu":
                    control_add = new Controls.Menu(this);
                    break;
                case "Segmented":
                    control_add = new Controls.Segmented(this);
                    break;
                case "Modal":
                    control_add = new Controls.Modal(this);
                    break;
                case "DatePicker":
                    control_add = new Controls.DatePicker(this);
                    break;
                case "TimePicker":
                    control_add = new Controls.TimePicker(this);
                    break;
                case "Dropdown":
                    control_add = new Controls.Dropdown(this);
                    break;
                case "Tree":
                    control_add = new Controls.Tree(this);
                    break;
                case "Popover":
                    control_add = new Controls.Popover(this);
                    break;
                case "Timeline":
                    control_add = new Controls.Timeline(this);
                    break;
                case "Steps":
                    control_add = new Controls.Steps(this);
                    break;
                case "ColorPicker":
                    control_add = new Controls.ColorPicker(this);
                    break;
                case "InputNumber":
                    control_add = new Controls.InputNumber(this);
                    break;
                case "Tag":
                    control_add = new Controls.Tag(this);
                    break;
                case "Drawer":
                    control_add = new Controls.Drawer(this);
                    break;
                case "FloatButton":
                    OpenFloatButton();
                    break;
                case "Rate":
                    control_add = new Controls.Rate(this);
                    break;
                case "Table":
#if DEBUG
                    control_add = new Controls.Table(this);
#else
                    control_add = new Controls.TableAOT(this);
#endif
                    break;
                case "Image":
                    control_add = new Controls.Preview(this);
                    break;
                case "VirtualPanel":
                    control_add = new Controls.VirtualPanel(this);
                    break;
                case "PageHeader":
                    control_add = new Controls.PageHeader(this);
                    break;
                case "Breadcrumb":
                    control_add = new Controls.Breadcrumb(this);
                    break;
                case "Collapse":
                    control_add = new Controls.Collapse(this);
                    break;

                case "GridPanel":
                    control_add = new Controls.GridPanel(this);
                    break;
                case "Splitter":
                    control_add = new Controls.Splitter(this);
                    break;
                case "Calendar":
                    control_add = new Controls.Calendar(this);
                    break;
                case "Battery":
                    control_add = new Controls.Battery(this);
                    break;
                case "Signal":
                    control_add = new Controls.Signal(this);
                    break;
                case "Spin":
                    control_add = new Controls.Spin(this);
                    break;
                case "ContextMenuStrip":
                    control_add = new Controls.ContextMenuStrip(this);
                    break;
                case "Shield":
                    control_add = new Controls.Shield(this);
                    break;
                case "Transfer":
                    control_add = new Controls.Transfer(this);
                    break;
                case "Chart":
                    control_add = new Controls.Chart(this);
                    break;
                case "Watermark":
                    control_add = new Controls.Watermark(this);
                    break;
                case "HyperlinkLabel":
                    control_add = new Controls.HyperlinkLabel(this);
                    break;
            }
            if (control_add != null)
            {
                //if (tabs == null)
                //{
                //    tabs = new AntdUI.Tabs { Dock = DockStyle.Bottom, Size = new Size(0, 400),Type= AntdUI.TabType.Card };
                //    Controls.Add(tabs);
                //}
                //var page = new AntdUI.TabPage { Text=id };
                //page.Controls.Add(control_add);
                //AutoDpi(control_add);
                //tabs.Pages.Add(page);
                //tabs.SelectedTab = page;
                //return;
                windowBar.SubText = id;
                if (windowBar.Tag is Control control)
                {
                    control.Dispose();
                    Controls.Remove(control);
                }
                windowBar.Tag = control_add;
                BeginInvoke(new Action(() =>
                {
                    virtualPanel.Visible = false;
                    control_add.Dock = DockStyle.Fill;
                    AntdUI.Win32.WindowTheme(control_add);
                    AutoDpi(control_add);
                    Controls.Add(control_add);
                    control_add.BringToFront();
                    control_add.Focus();
                    windowBar.ShowBack = true;
                }));
            }
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
                virtualPanel.Visible = true;
                windowBar.SubText = "Overview";
            }));
        }

        private void btn_mode_Click(object sender, EventArgs e)
        {
            if (setcolor)
            {
                var color = AntdUI.Style.Db.Primary;
                AntdUI.Config.IsDark = !AntdUI.Config.IsDark;
                Dark = AntdUI.Config.IsDark;
                AntdUI.Style.SetPrimary(color);
            }
            else
            {
                AntdUI.Config.IsDark = !AntdUI.Config.IsDark;
                Dark = AntdUI.Config.IsDark;
            }

            btn_mode.Toggle = Dark;
            OnSizeChanged(e);
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
                btn_global.Loading = true;
                if (lang.StartsWith("en")) AntdUI.Localization.Provider = new Localizer();
                else AntdUI.Localization.Provider = null;
                AntdUI.Localization.SetLanguage(lang);
                Refresh();
                AntdUI.ITask.Run(() =>
                {
                    int ScrollBarValue = virtualPanel.ScrollBar.Value;
                    virtualPanel.PauseLayout = true;
                    virtualPanel.Items.Clear();
                    LoadList();

                    string search = txt_search.Text;
                    if (string.IsNullOrEmpty(search)) virtualPanel.Empty = false;
                    else
                    {
                        virtualPanel.Empty = true;
                        string searchLower = search.ToLower();
                        var titles = new List<TItem>(virtualPanel.Items.Count);
                        foreach (var it in virtualPanel.Items)
                        {
                            if (it is VItem item) it.Visible = item.data.id.Contains(search) || item.data.key.Contains(search) || item.data.keyword.Contains(searchLower) || item.data.keywordmini.Contains(searchLower);
                            else if (it is TItem itemTitle) titles.Add(itemTitle);
                        }
                        foreach (var it in titles)
                        {
                            int count = 0;
                            foreach (var item in it.data)
                            {
                                if (item.Visible) count++;
                            }
                            it.Visible = count > 0;
                        }
                    }

                    virtualPanel.ScrollBar.Value = ScrollBarValue;
                    virtualPanel.PauseLayout = false;
                }, () => btn_global.Loading = false);
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

        bool setcolor = false;
        private void ColorTheme_ValueChanged(object sender, AntdUI.ColorEventArgs e)
        {
            setcolor = true;
            AntdUI.Style.SetPrimary(e.Value);
            Refresh();
        }

        #region 搜索

        private void txt_search_PrefixClick(object sender, MouseEventArgs e) => LoadSearchList();

        private void txt_search_TextChanged(object sender, EventArgs e) => LoadSearchList();

        void LoadSearchList()
        {
            string search = txt_search.Text;
            windowBar.Loading = true;
            BeginInvoke(new Action(() =>
            {
                virtualPanel.PauseLayout = true;
                if (string.IsNullOrEmpty(search))
                {
                    foreach (var it in virtualPanel.Items) it.Visible = true;
                    virtualPanel.Empty = false;
                }
                else
                {
                    virtualPanel.Empty = true;
                    string searchLower = search.ToLower();
                    var titles = new List<TItem>(virtualPanel.Items.Count);
                    foreach (var it in virtualPanel.Items)
                    {
                        if (it is VItem item) it.Visible = item.data.id.Contains(search) || item.data.key.Contains(search) || item.data.keyword.Contains(searchLower) || item.data.keywordmini.Contains(searchLower);
                        else if (it is TItem itemTitle) titles.Add(itemTitle);
                    }
                    foreach (var it in titles)
                    {
                        int count = 0;
                        foreach (var item in it.data)
                        {
                            if (item.Visible) count++;
                        }
                        it.Visible = count > 0;
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
                { AntdUI.Localization.Get("General", "通用"), dir_General },
                { AntdUI.Localization.Get("Layout", "布局"), dir_Layout },
                { AntdUI.Localization.Get("Navigation", "导航"), dir_Navigation },
                { AntdUI.Localization.Get("DataEntry", "数据录入"), dir_DataEntry },
                { AntdUI.Localization.Get("DataDisplay", "数据展示"), dir_DataDisplay },
                { AntdUI.Localization.Get("Feedback", "反馈"), dir_Feedback },
                { AntdUI.Localization.Get("Other", "其他"), dir_Other }
            };

            var list = new List<AntdUI.VirtualItem>(dir.Count + dir_General.Length + dir_Layout.Length + dir_Navigation.Length + dir_DataEntry.Length + dir_DataDisplay.Length + dir_Feedback.Length);

            bool china = true;
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.StartsWith("en")) china = false;
            foreach (var it in dir)
            {
                var list_sub = new List<AntdUI.VirtualItem>(it.Value.Length);
                foreach (var item in it.Value) list_sub.Add(new VItem(item, china));
                list.Add(new TItem(it.Key, list_sub));
                list.AddRange(list_sub);
            }
            virtualPanel.Items.AddRange(list);
            windowBar.Loading = false;
            virtualPanel.BlurBar = windowBar;
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
                keyword = _id.ToLower() + AntdUI.Pinyin.GetPinyin(_key).ToLower();
                keywordmini = AntdUI.Pinyin.GetInitials(_key).ToLower();
                imgs = new Image[] { AntdUI.SvgExtend.SvgToBmp(_img_light), AntdUI.SvgExtend.SvgToBmp(_img_dark) };
            }
            public string id { get; set; }
            public string keyword { get; set; }
            public string keywordmini { get; set; }
            public string key { get; set; }
            public Image[] imgs { get; set; }
        }

        #region 渲染

        class TItem : AntdUI.VirtualItem
        {
            string title, count;
            public List<AntdUI.VirtualItem> data;
            public TItem(string t, List<AntdUI.VirtualItem> d)
            {
                CanClick = false;
                data = d;
                title = t;
                count = d.Count.ToString();
            }

            public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                using (var font_title = new Font(e.Panel.Font, FontStyle.Bold))
                using (var font_count = new Font(e.Panel.Font.FontFamily, e.Panel.Font.Size * .74F, e.Panel.Font.Style))
                {
                    var size = AntdUI.Helper.Size(g.MeasureString(title, font_title));
                    g.String(title, font_title, AntdUI.Style.Db.Text, new Rectangle(e.Rect.X + x, e.Rect.Y, e.Rect.Width, e.Rect.Height), AntdUI.FormatFlags.Left | AntdUI.FormatFlags.VerticalCenter | AntdUI.FormatFlags.NoWrap);

                    var rect_count = new Rectangle(e.Rect.X + x + size.Width + gap, e.Rect.Y + (e.Rect.Height - size.Height) / 2, size.Height, size.Height);
                    using (var path = AntdUI.Helper.RoundPath(rect_count, e.Radius))
                    {
                        g.Fill(AntdUI.Style.Db.TagDefaultBg, path);
                        g.Draw(AntdUI.Style.Db.DefaultBorder, sp, path);
                    }
                    g.String(count, font_count, AntdUI.Style.Db.Text, rect_count, AntdUI.FormatFlags.Center | AntdUI.FormatFlags.NoWrap);
                }
            }

            int gap = 8, sp = 1, x = 30;
            public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = g.Dpi;
                gap = (int)(8 * dpi);
                sp = (int)(1 * dpi);
                x = (int)(30 * dpi);
                return new Size(e.Rect.Width, (int)(44 * dpi));
            }
        }

        class VItem : AntdUI.VirtualShadowItem
        {
            public IList data;
            string name;
            public VItem(IList d, bool china)
            {
                data = d;
                Tag = d.id;
                if (china) name = data.id + " " + data.key;
                else name = data.id;
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
                        g.String(name, font_title, fore, rect_title, s_f);
                    }
                }
                using (var brush = new SolidBrush(AntdUI.Style.Db.Split))
                {
                    g.Fill(brush, rect_line);
                }
                //g.Fill(Color.Blue, rect_ico);
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
}
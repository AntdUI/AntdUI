// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Icon : UserControl
    {
        AntdUI.BaseForm form;
        public Icon(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            segmented1.Items.Add(new AntdUI.SegmentedItem
            {
                IconSvg = emoji_svg_all,
                Text = "Emoji"
            });
            LoadData();
        }

        #region 数据

        private void segmented1_SelectIndexChanged(object sender, AntdUI.IntEventArgs e) => LoadData();

        void LoadData()
        {
            int index = segmented1.SelectIndex;
            if (index == 2)
            {
                var data = GetDataEmoji();
                var svgs = new List<AntdUI.VirtualItem>(data.Count);
                foreach (var it in data)
                {
                    if (it.Key == "people body") svgs.Add(new AntdUI.TItem_Skin(GetChinaEmojiGroup(it.Key), it.Key, it.Value.ToArray()));
                    else svgs.Add(new AntdUI.TItem(GetChinaEmojiGroup(it.Key), it.Key, it.Value.ToArray()));
                    svgs.AddRange(it.Value);
                }
                vpanel.Items.Clear();
                txt_search.Text = "";
                vpanel.Items.AddRange(svgs);
            }
            else
            {
                var data = GetData(index);
                var svgs = new List<AntdUI.VirtualItem>(data.Count);
                foreach (var it in data)
                {
                    svgs.Add(new AntdUI.TItem(GetChinaGroup(it.Key), it.Key, it.Value.ToArray()));
                    svgs.AddRange(it.Value);
                }
                vpanel.Items.Clear();
                txt_search.Text = "";
                vpanel.Items.AddRange(svgs);
            }
        }

        Dictionary<string, List<AntdUI.VIItem>> GetData(int index)
        {
            var dir = new Dictionary<string, List<AntdUI.VIItem>>(AntdUI.SvgDb.Custom.Count);
            var tmp = new List<AntdUI.VIItem>(AntdUI.SvgDb.Custom.Count);
            if (index == 0)
            {
                foreach (var it in AntdUI.SvgDb.Custom)
                {
                    if (it.Key == "QuestionOutlined")
                    {
                        var id = "Icon.Directional";
                        foreach (var item in tmp) item.Group = id;
                        dir.Add(id, new List<AntdUI.VIItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "EditOutlined")
                    {
                        var id = "Icon.Suggested";
                        foreach (var item in tmp) item.Group = id;
                        dir.Add(id, new List<AntdUI.VIItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AreaChartOutlined")
                    {
                        var id = "Icon.Editor";
                        foreach (var item in tmp) item.Group = id;
                        dir.Add(id, new List<AntdUI.VIItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AndroidOutlined")
                    {
                        var id = "Icon.Data";
                        foreach (var item in tmp) item.Group = id;
                        dir.Add(id, new List<AntdUI.VIItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AccountBookOutlined")
                    {
                        var id = "Icon.Logos";
                        foreach (var item in tmp) item.Group = id;
                        dir.Add(id, new List<AntdUI.VIItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "StepBackwardFilled")
                    {
                        var id = "Icon.Application";
                        foreach (var item in tmp) item.Group = id;
                        dir.Add(id, new List<AntdUI.VIItem>(tmp));
                        tmp.Clear();
                        return dir;
                    }
                    tmp.Add(new AntdUI.VIItem(it.Key, it.Value));
                }
                foreach (var item in tmp) item.Group = "Icon.Application";
                dir.Add("Icon.Application", new List<AntdUI.VIItem>(tmp));
                tmp.Clear();
            }
            else
            {
                bool isadd = false;
                foreach (var it in AntdUI.SvgDb.Custom)
                {
                    if (it.Key == "StepBackwardFilled") isadd = true;
                    else if (it.Key == "QuestionCircleFilled")
                    {
                        var id = "Icon.Directional";
                        foreach (var item in tmp) item.Group = id;
                        dir.Add(id, new List<AntdUI.VIItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "EditFilled")
                    {
                        var id = "Icon.Suggested";
                        foreach (var item in tmp) item.Group = id;
                        dir.Add(id, new List<AntdUI.VIItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "PieChartFilled")
                    {
                        var id = "Icon.Editor";
                        foreach (var item in tmp) item.Group = id;
                        dir.Add(id, new List<AntdUI.VIItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AndroidFilled")
                    {
                        var id = "Icon.Data";
                        foreach (var item in tmp) item.Group = id;
                        dir.Add(id, new List<AntdUI.VIItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AccountBookFilled")
                    {
                        var id = "Icon.Logos";
                        foreach (var item in tmp) item.Group = id;
                        dir.Add(id, new List<AntdUI.VIItem>(tmp));
                        tmp.Clear();
                    }
                    if (isadd) tmp.Add(new AntdUI.VIItem(it.Key, it.Value));
                }
                foreach (var item in tmp) item.Group = "Icon.Application";
                dir.Add("Icon.Application", new List<AntdUI.VIItem>(tmp));
                tmp.Clear();
            }
            return dir;
        }
        Dictionary<string, List<AntdUI.EItem>> GetDataEmoji()
        {
            var dir = new Dictionary<string, List<AntdUI.EItem>>(9);
            int count = AntdUI.SvgDb.Emoji.Count;
            var hs = new List<string>();
            foreach (var it in AntdUI.SvgDb.Emoji)
            {
                var group = emoji_group[it.Key];
                if (emoji_skins.TryGetValue(it.Key, out var skins))
                {
                    var fid = emoji_skins[it.Key];
                    hs.AddRange(skins);
                    var item = new AntdUI.EItem(it.Key, it.Value, emoji_keywords[it.Key], group);
                    item.Skins = skins.ToArray();
                    if (dir.TryGetValue(group, out var tmp)) tmp.Add(item);
                    else dir.Add(group, new List<AntdUI.EItem>(count) { item });
                }
                else if (hs.Contains(it.Key)) continue;
                else
                {
                    var item = new AntdUI.EItem(it.Key, it.Value, emoji_keywords[it.Key], group);
                    if (dir.TryGetValue(group, out var tmp)) tmp.Add(item);
                    else dir.Add(group, new List<AntdUI.EItem>(count) { item });
                }
            }
            return dir;
        }

        string GetChinaGroup(string key)
        {
            switch (key)
            {
                case "QuestionOutlined":
                case "QuestionCircleFilled":
                case "Icon.Directional":
                    return "方向性图标";
                case "EditOutlined":
                case "EditFilled":
                case "Icon.Suggested":
                    return "提示建议性图标";
                case "AreaChartOutlined":
                case "PieChartFilled":
                case "Icon.Editor":
                    return "编辑类图标";
                case "AndroidOutlined":
                case "AndroidFilled":
                case "Icon.Data":
                    return "数据类图标";
                case "AccountBookOutlined":
                case "AccountBookFilled":
                case "Icon.Logos":
                    return "品牌和标识";
                case "StepBackwardFilled":
                case "Icon.Application":
                    return "网站通用图标";

                default:
                    return "未知";
            }
        }
        string GetChinaEmojiGroup(string key)
        {
            switch (key)
            {
                case "smileys emotion":
                    return "笑脸和情感";
                case "people body":
                    return "人物和身体";
                case "animals nature":
                    return "动物和自然";
                case "food drink":
                    return "食物和饮料";
                case "travel places":
                    return "旅行和地点";
                case "activities":
                    return "活动";
                case "objects":
                    return "物品";
                case "symbols":
                    return "符号";
                case "flags":
                    return "旗帜";
                default:
                    return "未知";
            }
        }

        #endregion

        private void vpanel_ItemClick(object sender, AntdUI.VirtualItemEventArgs e)
        {
            if (e.Item is AntdUI.VIItem item)
            {
                if (AntdUI.Helper.ClipboardSetText(this, item.Key)) AntdUI.Message.success(form, item.Key + " " + AntdUI.Localization.Get("CopyOK", "复制成功"));
                else AntdUI.Message.error(form, item.Key + " " + AntdUI.Localization.Get("CopyFailed", "复制失败"));
            }
            else if (e.Item is AntdUI.EItem emoji)
            {
                if (AntdUI.Helper.ClipboardSetText(this, emoji.Key)) AntdUI.Message.success(form, emoji.Key + " " + AntdUI.Localization.Get("CopyOK", "复制成功"));
                else AntdUI.Message.error(form, emoji.Key + " " + AntdUI.Localization.Get("CopyFailed", "复制失败"));
            }
        }

        #region 搜索

        private void txt_search_TextChanged(object sender, EventArgs e) => LoadSearchList();
        private void txt_search_SuffixClick(object sender, MouseEventArgs e) => LoadSearchList();

        void LoadSearchList()
        {
            string search = txt_search.Text.Trim();
            BeginInvoke(new Action(() =>
            {
                vpanel.PauseLayout = true;
                if (string.IsNullOrEmpty(search))
                {
                    foreach (var it in vpanel.Items)
                    {
                        it.Visible = true;
                        if (it is AntdUI.TItem itemTitle) itemTitle.Restore();
                    }
                    vpanel.Empty = false;
                }
                else
                {
                    vpanel.Empty = true;
                    var titles = new List<AntdUI.TItem>(vpanel.Items.Count);
                    var listSearch = new List<AntdUI.ItemSearchWeigth<AntdUI.VirtualItem>>(vpanel.Items.Count);
                    foreach (var it in vpanel.Items)
                    {
                        if (it is AntdUI.EItem emoji_item)
                        {
                            int score = AntdUI.Helper.SearchContains(search, emoji_item.Key, emoji_item.Keywords, out _);
                            it.Visible = score > 0;
                            if (it.Visible) listSearch.Add(new AntdUI.ItemSearchWeigth<AntdUI.VirtualItem>(score, it).SetGroup(emoji_item.Group));
                        }
                        else if (it is AntdUI.VIItem item)
                        {
                            int score = AntdUI.Helper.SearchContains(search, item.Key, out _);
                            it.Visible = score > 0;
                            if (it.Visible) listSearch.Add(new AntdUI.ItemSearchWeigth<AntdUI.VirtualItem>(score, it).SetGroup(item.Group));
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
                vpanel.PauseLayout = false;
            }));
        }

        #endregion
    }
}
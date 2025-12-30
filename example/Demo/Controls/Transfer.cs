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
    public partial class Transfer : UserControl, AntdUI.IEventListener
    {
        AntdUI.BaseForm form;
        public Transfer(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            InitializeTransfer();
            AntdUI.EventHub.AddListener(this);
        }

        private void InitializeTransfer()
        {
            // 初始化穿梭框数据
            var items = new List<AntdUI.TransferItem>(10);
            for (int i = 0; i <= 10; i++) items.Add(new AntdUI.TransferItem(AntdUI.Localization.Get("Transfer.Content", "选项") + (i + 1), i));
            // 设置穿梭框数据源
            transfer1.Items.AddRange(items);

            label_source.Suffix = transfer1.GetSourceItems().Count + AntdUI.Localization.Get("Transfer.Items", "项");
            label_target.Suffix = transfer1.GetTargetItems().Count + AntdUI.Localization.Get("Transfer.Items", "项");

            // 注册穿梭框事件
            transfer1.TransferChanged += transfer1_TransferChanged;
            transfer1.Search += transfer1_Search;

            // 设置单向模式开关事件
            switch_oneWay.CheckedChanged += switch_oneWay_CheckedChanged;
        }

        private void transfer1_TransferChanged(object sender, AntdUI.Transfer.TransferEventArgs e)
        {
            // 更新源列表和目标列表的数量显示
            label_source.Suffix = e.SourceItems.Count + AntdUI.Localization.Get("Transfer.Items", "项");
            label_target.Suffix = e.TargetItems.Count + AntdUI.Localization.Get("Transfer.Items", "项");
        }

        private void transfer1_Search(object sender, AntdUI.Transfer.SearchEventArgs e)
        {
            // 搜索事件处理
            string listType = e.IsSource ? "源列表" : "目标列表";
            AntdUI.Message.info(FindForm(), $"{listType}搜索: {e.SearchText}");
        }

        private void switch_oneWay_CheckedChanged(object sender, EventArgs e)
        {
            // 设置是否单向模式
            transfer1.OneWay = switch_oneWay.Checked;
        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            // 重新加载数据
            Random random = new Random();
            int count = random.Next(10, 40);
            var items = new List<AntdUI.TransferItem>(count);
            for (int i = 1; i <= count; i++) items.Add(new AntdUI.TransferItem(AntdUI.Localization.Get("Transfer.Option", "新选项") + i, i));
            transfer1.Items.Clear();
            transfer1.Items.AddRange(items);
            transfer1.Reload();

            // 更新源列表和目标列表的数量显示
            label_source.Suffix = transfer1.GetSourceItems().Count + AntdUI.Localization.Get("Transfer.Items", "项");
            label_target.Suffix = transfer1.GetTargetItems().Count + AntdUI.Localization.Get("Transfer.Items", "项");
        }

        public void HandleEvent(AntdUI.EventType id, object tag)
        {
            switch (id)
            {
                case AntdUI.EventType.LANG:
                    label_source.Suffix = transfer1.GetSourceItems().Count + AntdUI.Localization.Get("Transfer.Items", "项");
                    label_target.Suffix = transfer1.GetTargetItems().Count + AntdUI.Localization.Get("Transfer.Items", "项");
                    break;
            }
        }
    }
}
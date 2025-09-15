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
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Transfer : UserControl, AntdUI.IEventListener
    {
        Form form;
        public Transfer(Form _form)
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
            transfer1.Items = items;

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

            transfer1.Items = items;
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
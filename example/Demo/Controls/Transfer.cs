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
    public partial class Transfer : UserControl
    {
        Form form;
        public Transfer(Form _form)
        {
            form = _form;
            InitializeComponent();
            InitializeTransfer();
        }

        private void InitializeTransfer()
        {
            // 初始化穿梭框数据
            var items = new List<AntdUI.TransferItem>
            {
                new AntdUI.TransferItem("选项1", 1),
                new AntdUI.TransferItem("选项2", 2),
                new AntdUI.TransferItem("选项3", 3),
                new AntdUI.TransferItem("选项4", 4),
                new AntdUI.TransferItem("选项5", 5),
                new AntdUI.TransferItem("选项6", 6),
                new AntdUI.TransferItem("选项7", 7),
                new AntdUI.TransferItem("选项8", 8),
                new AntdUI.TransferItem("选项9", 9),
                new AntdUI.TransferItem("选项10", 10)
            };

            // 设置穿梭框数据源
            transfer1.Items = items;

            // 注册穿梭框事件
            transfer1.TransferChanged += Transfer1_TransferChanged;
            transfer1.Search += Transfer1_Search;

            // 设置单向模式开关事件
            switch_oneWay.CheckedChanged += Switch_oneWay_CheckedChanged;

            // 设置搜索框开关事件
            //switch_search.CheckedChanged += Switch_search_CheckedChanged;
        }

        private void Transfer1_TransferChanged(object sender, AntdUI.Transfer.TransferEventArgs e)
        {
            // 更新源列表和目标列表的数量显示
            label_source.Text = $"源列表: {e.SourceItems.Count}项";
            label_target.Text = $"目标列表: {e.TargetItems.Count}项";
        }

        private void Transfer1_Search(object sender, AntdUI.Transfer.SearchEventArgs e)
        {
            // 搜索事件处理
            string listType = e.IsSource ? "源列表" : "目标列表";
            AntdUI.Message.info(FindForm(), $"{listType}搜索: {e.SearchText}");
        }

        private void Switch_oneWay_CheckedChanged(object sender, EventArgs e)
        {
            // 设置是否单向模式
            transfer1.OneWay = switch_oneWay.Checked;
        }

        private void Switch_search_CheckedChanged(object sender, EventArgs e)
        {
            // 设置是否显示搜索框
            //transfer1.ShowSearch = switch_search.Checked;
        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            // 重新加载数据
            var items = new List<AntdUI.TransferItem>();
            Random random = new Random();
            int count = random.Next(5, 15);

            for (int i = 1; i <= count; i++)
            {
                items.Add(new AntdUI.TransferItem($"新选项{i}", i));
            }

            transfer1.Items = items;
            transfer1.Reload();

            // 更新源列表和目标列表的数量显示
            label_source.Text = $"源列表: {transfer1.GetSourceItems().Count}项";
            label_target.Text = $"目标列表: {transfer1.GetTargetItems().Count}项";
        }
    }
}// This file was created by the assistant

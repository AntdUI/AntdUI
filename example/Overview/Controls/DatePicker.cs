// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

namespace Overview.Controls
{
    public partial class DatePicker : UserControl
    {
        public DatePicker()
        {
            InitializeComponent();
            DateTime now = DateTime.Now.AddDays(new Random().Next(-5, 5));
            datePicker1.BadgeAction = dates =>
            {
                return new List<AntdUI.DateBadge> {
                    new AntdUI.DateBadge(now.ToString("yyyy-MM-dd"),0,Color.FromArgb(112, 237, 58)),
                    new AntdUI.DateBadge(now.AddDays(1).ToString("yyyy-MM-dd"),5),
                    new AntdUI.DateBadge(now.AddDays(-2).ToString("yyyy-MM-dd"),99),
                    new AntdUI.DateBadge(now.AddDays(-6).ToString("yyyy-MM-dd"),998),
                };
            };
            datePickerRange1.BadgeAction = dates =>
            {
                return new List<AntdUI.DateBadge> {
                    new AntdUI.DateBadge(now.ToString("yyyy-MM-dd"),0,Color.FromArgb(112, 237, 58)),
                    new AntdUI.DateBadge(now.AddDays(1).ToString("yyyy-MM-dd"),5),
                    new AntdUI.DateBadge(now.AddDays(-2).ToString("yyyy-MM-dd"),99),
                    new AntdUI.DateBadge(now.AddDays(-6).ToString("yyyy-MM-dd"),998),
                };
            };
        }

        private void datePickerRange4_PresetsClickChanged(object sender, object value)
        {
            AntdUI.Message.info((Form)Parent, "ÒÑµã»÷£º" + value, Font);
        }
    }
}
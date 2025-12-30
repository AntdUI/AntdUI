// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Calendar : UserControl
    {
        AntdUI.BaseForm form;
        public Calendar(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            calendar1.DateChanged += Calendar_DateChanged;
            switch_showtoday.CheckedChanged += Switch_showtoday_CheckedChanged;
            switch_showchinese.CheckedChanged += Switch_showchinese_CheckedChanged;

            calendar1.BadgeAction = dates =>
            {
                // dates 参数为 DateTime[] 数组长度固定为2，返回UI上显示的开始日期与结束日期
                // DateTime start_date = dates[0], end_date = dates[1];
                var now = dates[1];
                return new List<AntdUI.DateBadge> {
                    new AntdUI.DateBadge(now.ToString("yyyy-MM-dd"),0),
                    new AntdUI.DateBadge(now.AddDays(-20).ToString("yyyy-MM-dd"),5),
                    new AntdUI.DateBadge(now.AddDays(-14).ToString("yyyy-MM-dd"), "休"),
                    new AntdUI.DateBadge(now.AddDays(-7).ToString("yyyy-MM-dd"), "休")
                    {
                        //圆角支持,默认值是6
                        Radius = 12
                    },
                    new AntdUI.DateBadge(now.AddDays(-6).ToString("yyyy-MM-dd"),998,Color.FromArgb(112, 237, 58)),
                };
            };
            calendar1.LoadBadge();
        }

        private void Switch_showchinese_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            calendar1.ShowChinese = e.Value;
        }

        private void Switch_showtoday_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            calendar1.ShowButtonToDay = e.Value;
        }

        private void Calendar_DateChanged(object sender, AntdUI.DateTimeEventArgs e)
        {
            AntdUI.Message.info(form, e.Value.ToString("yyyy-MM-dd"), autoClose: 1);
        }
    }
}

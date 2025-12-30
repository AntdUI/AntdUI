// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class DatePicker : UserControl
    {
        AntdUI.BaseForm form;
        public DatePicker(AntdUI.BaseForm _form)
        {
            form = _form;
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

        private void datePicker_PresetsClickChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            AntdUI.Message.info(form, "已点击：" + e.Value, Font);
        }
    }
}
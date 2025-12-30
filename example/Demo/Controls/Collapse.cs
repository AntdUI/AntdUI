// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Collapse : UserControl
    {
        AntdUI.BaseForm form;
        public Collapse(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }
        private void collapse1_ButtonClickChanged(object sender, AntdUI.CollapseButtonClickEventArgs e)
        {
            AntdUI.Notification.info(form, e.Parent.Text, e.Value.Text, AntdUI.TAlignFrom.Top, Font);
        }

        private void edit1_TextChanged(object sender, AntdUI.CollapseEditChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value == null ? string.Empty : e.Value.ToString())) return;
            AntdUI.Notification.info(form, e.Parent.Text, e.Value.ToString(), AntdUI.TAlignFrom.Top, Font);
        }

        private void CollapseGroupButton11_CustomInputEdit(object sender, AntdUI.CollapseCustomInputEditEventArgs e)
        {
            AntdUI.Select select = new AntdUI.Select()
            {
                TabIndex = 0,
                PrefixText = "动画速度：",
                PlaceholderText = "<=10 关闭动画",
                Width = 260,
            };
            select.Items.AddRange(new object[] { 10, 20, 50, 100, 200, 300, 400, 500 });
            select.SelectedValue = 100;
            select.SelectedValueChanged += Select_SelectedValueChanged;
            e.Edit = select;
        }

        private void Select_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            collapse1.AnimationSpeed = (int)e.Value;
        }

        private void switchButton_CheckedChanged(object sender, AntdUI.CollapseSwitchCheckedChangedEventArgs e)
        {
            AntdUI.Notification.info(form, e.Parent.Text, e.Checked ? e.Value.CheckedText : e.Value.UnCheckedText, AntdUI.TAlignFrom.Top, Font);
        }
    }

}
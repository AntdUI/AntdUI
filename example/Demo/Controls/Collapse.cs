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

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Collapse : UserControl
    {
        Form form;
        public Collapse(Form _form)
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
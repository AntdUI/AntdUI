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
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Watermark : UserControl
    {
        AntdUI.BaseForm form;

        public Watermark(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            colorPicker.Value = AntdUI.Style.Db.Text;
            inputContent2.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        Form currentWatermark = null;
        private void trackOpacity_ValueChanged(object sender, AntdUI.IntEventArgs e)
        {
            label2.Text = e.Value + "%";
            if (config == null) return;
            config.Opacity = e.Value / 100.0F;
        }

        private void trackRotate_ValueChanged(object sender, AntdUI.IntEventArgs e)
        {
            label3.Text = e.Value + "°";
            if (config == null) return;
            config.Rotate = e.Value;
            config.Print();
        }

        private void trackGap_ValueChanged(object sender, AntdUI.IntEventArgs e)
        {
            label4.Text = e.Value + "px";
            if (config == null) return;
            config.Gap = new int[] { e.Value, e.Value };
            config.Print();
        }

        private void colorPicker_ValueChanged(object sender, AntdUI.ColorEventArgs e)
        {
            if (config == null) return;
            config.ForeColor = e.Value;
            config.Print();
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearWatermark();

        private void ClearWatermark()
        {
            if (currentWatermark != null)
            {
                currentWatermark.Close();
                currentWatermark = null;
                //AntdUI.Message.info(form, "水印已清除");
            }
        }

        private void btnForm_Click(object sender, EventArgs e)
        {
            ClearWatermark();
            try
            {
                config = new AntdUI.Watermark.Config(form, inputContent.Text, inputContent2.Text);
                SetWatermark(config);
                currentWatermark = AntdUI.Watermark.open(config);
                if (currentWatermark == null) AntdUI.Message.error(form, AntdUI.Localization.Get("Watermark.btnFormFailed", "窗体水印创建失败"));
                else AntdUI.Message.success(form, AntdUI.Localization.Get("Watermark.btnFormOK", "窗体水印创建成功！"));
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(form, AntdUI.Localization.Get("Watermark.btnFormError", "创建窗体水印时发生错误：") + ex.Message);
            }
        }

        AntdUI.Watermark.Config config;
        private void btnPanel_Click(object sender, EventArgs e)
        {
            ClearWatermark();
            try
            {
                config = new AntdUI.Watermark.Config(panel8, inputContent.Text, inputContent2.Text);
                SetWatermark(config);
                currentWatermark = AntdUI.Watermark.open(config);
                if (currentWatermark == null) AntdUI.Message.error(form, AntdUI.Localization.Get("Watermark.btnPanelFailed", "面板水印创建失败"));
                else AntdUI.Message.success(form, AntdUI.Localization.Get("Watermark.btnPanelOK", "面板水印创建成功！"));
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(form, AntdUI.Localization.Get("Watermark.btnPanelError", "创建面板水印时发生错误：") + ex.Message);
            }
        }

        AntdUI.Watermark.Config SetWatermark(AntdUI.Watermark.Config config) => config.SetRotate(trackRotate.Value).SetOpacity(trackOpacity.Value / 100.0F).SetFore(colorPicker.Value).SetGap(trackGap.Value);

        protected override void DestroyHandle()
        {
            base.DestroyHandle();
            ClearWatermark();
        }
    }
}

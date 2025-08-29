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
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Watermark : UserControl
    {
        private AntdUI.FormWatermark? currentWatermark = null;
        private Form form;

        public Watermark(Form _form)
        {
            form = _form;
            InitializeComponent();

            inputContent2.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void trackOpacity_ValueChanged(object sender, AntdUI.IntEventArgs e)
        {
            var opacity = e.Value / 100.0F;
            lblOpacity.Text = $"透明度: {e.Value}%";

            // 如果当前有水印，更新透明度
            if (currentWatermark != null)
            {
                //CreateWatermark();
            }
        }

        private void trackRotate_ValueChanged(object sender, AntdUI.IntEventArgs e)
        {
            lblRotate.Text = $"旋转角度: {e.Value}°";

            // 如果当前有水印，更新旋转角度
            if (currentWatermark != null)
            {
                //CreateWatermark();
            }
        }

        private void trackGap_ValueChanged(object sender, AntdUI.IntEventArgs e)
        {
            lblGap.Text = $"间距: {e.Value}px";

            // 如果当前有水印，更新间距
            if (currentWatermark != null)
            {
                //CreateWatermark();
            }
        }

        private void colorPicker_ValueChanged(object sender, AntdUI.ColorEventArgs e)
        {
            // 如果当前有水印，更新颜色
            if (currentWatermark != null)
            {
                //CreateWatermark();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearWatermark();
        }

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
            // 清除现有水印
            ClearWatermark();

            try
            {
                var config = new AntdUI.Watermark.Config(form, inputContent.Text, inputContent2.Text)
                {
                    Rotate = trackRotate.Value,
                    Gap = new int[] { trackGap.Value, trackGap.Value }
                };

                // 设置字体颜色透明度
                var opacity = trackOpacity.Value / 100.0F;
                var alpha = (int)(opacity * 255);
                var selectedColor = colorPicker.Value;
                config.Font.Color = Color.FromArgb(alpha, selectedColor.R, selectedColor.G, selectedColor.B);

                currentWatermark = AntdUI.Watermark.open(config);
                if (currentWatermark != null)
                {
                    AntdUI.Message.success(form, "窗体水印创建成功！");
                }
                else
                {
                    AntdUI.Message.error(form, "窗体水印创建失败");
                }
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(form, $"创建窗体水印时发生错误：{ex.Message}");
            }
        }

        private void btnPanel_Click(object sender, EventArgs e)
        {
            // 清除现有水印
            ClearWatermark();

            try
            {
                var config = new AntdUI.Watermark.Config(panel8, inputContent.Text, inputContent2.Text)
                {
                    Rotate = trackRotate.Value,
                    Gap = new int[] { trackGap.Value, trackGap.Value }
                };

                // 设置字体颜色透明度
                var opacity = trackOpacity.Value / 100.0F;
                var alpha = (int)(opacity * 255);
                var selectedColor = colorPicker.Value;
                config.Font.Color = Color.FromArgb(alpha, selectedColor.R, selectedColor.G, selectedColor.B);

                currentWatermark = AntdUI.Watermark.open(config);
                if (currentWatermark != null)
                {
                    AntdUI.Message.success(form, "面板水印创建成功！");
                }
                else
                {
                    AntdUI.Message.error(form, "面板水印创建失败");
                }
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(form, $"创建面板水印时发生错误：{ex.Message}");
            }
        }

        private void header1_BackClick(object sender, EventArgs e)
        {
            // 清理水印
            //ClearWatermark();
        }

        //private void CreateWatermark(Image? image = null)
        //{
        //    // 清除现有水印
        //    ClearWatermark();

        //    try
        //    {
        //        var config = new AntdUI.Watermark.Config(form, inputContent.Text, inputContent2.Text)
        //        {
        //            Rotate = trackRotate.Value,
        //            Gap = new int[] { trackGap.Value, trackGap.Value + 10 },
        //            Image = image
        //        };

        //        // 设置字体颜色透明度
        //        var opacity = trackOpacity.Value / 100.0F;
        //        var alpha = (int)(opacity * 255);
        //        var selectedColor = colorPicker.Value;
        //        config.Font.Color = Color.FromArgb(alpha, selectedColor.R, selectedColor.G, selectedColor.B);

        //        currentWatermark = AntdUI.Watermark.open(config);
        //        if (currentWatermark != null)
        //        {
        //            var watermarkType = image != null ? "图片水印" : "文本水印";
        //            AntdUI.Message.success(form, $"{watermarkType}创建成功！");
        //        }
        //        else
        //        {
        //            AntdUI.Message.error(form, "水印创建失败，请检查窗体状态");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AntdUI.Message.error(form, $"创建水印时发生错误：{ex.Message}");
        //    }
        //}
    }
}

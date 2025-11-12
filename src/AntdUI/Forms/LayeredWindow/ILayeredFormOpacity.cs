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
using System.ComponentModel;
using System.Windows.Forms;

namespace AntdUI
{
    public abstract class ILayeredFormOpacity : ILayeredForm, LayeredFormAsynLoad
    {
        public OpacityAnimateConfig animateConfig;
        public ILayeredFormOpacity(byte maxalpha = 255)
        {
            animateConfig = new OpacityAnimateConfig(this, LoadOK, ClosingAnimation, maxalpha);
        }
        protected override void OnLoad(EventArgs e)
        {
            animateConfig.Start(name);
            base.OnLoad(e);
        }

        public abstract string name { get; }

        public bool RunAnimation = true;

        /// <summary>
        /// 是否正在加载
        /// </summary>
        [Description("是否正在加载"), Category("参数"), DefaultValue(true)]
        public bool IsLoad { get; set; } = true;

        /// <summary>
        /// 加载完成回调
        /// </summary>
        [Description("加载完成回调"), Category("参数"), DefaultValue(null)]
        public Action? LoadCompleted { get; set; }

        public virtual void LoadOK()
        {
            RunAnimation = false;
            IsLoad = false;
            LoadCompleted?.Invoke();
        }
        public virtual void ClosingAnimation() { }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (animateConfig.End(name)) e.Cancel = true;
            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            animateConfig.Dispose();
            base.Dispose(disposing);
        }
    }
}
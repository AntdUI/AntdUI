// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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
            if (animateConfig.End(name, e.CloseReason)) e.Cancel = true;
            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            animateConfig.Dispose();
            base.Dispose(disposing);
        }
    }
}
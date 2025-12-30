// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace AntdUI.Design
{
    public class ColorEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context) => UITypeEditorEditStyle.DropDown;

        public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
        {
            var service = provider.GetService(typeof(IWindowsFormsEditorService));
            if (service is IWindowsFormsEditorService editorService)
            {
                var editorControl = new FrmColorEditor(value);
                editorService.DropDownControl(editorControl);
                return editorControl.Value;
            }
            return null;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext? context) => true;

        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value is Color color)
            {
                using var brush = new SolidBrush(color);
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
        }
    }
}
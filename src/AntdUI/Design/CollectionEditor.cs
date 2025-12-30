// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace AntdUI.Design
{
    public class CollectionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context) => UITypeEditorEditStyle.Modal;

        public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
        {
            var service = provider.GetService(typeof(IWindowsFormsEditorService));
            if (service is IWindowsFormsEditorService editorService)
            {
                if (value is IList list)
                {
                    var collectionEditor = new System.ComponentModel.Design.CollectionEditor(list.GetType());
                    return collectionEditor.EditValue(context, provider, value);
                }
            }
            return value;
        }
    }
}
// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;

namespace AntdUI
{
    partial class Input
    {
        void AddHistoryRecord()
        {
            if (history_I > -1)
            {
                history_Log.RemoveRange(history_I + 1, history_Log.Count - (history_I + 1));
                history_I = -1;
            }
            if (IsPassWord && !PasswordCopy) return;
            history_Log.Add(new TextHistoryRecord(this));
        }

        int history_I = -1;
        List<TextHistoryRecord> history_Log = new List<TextHistoryRecord>();
        internal class TextHistoryRecord
        {
            public TextHistoryRecord(Input input)
            {
                SelectionStart = input.SelectionStart;
                SelectionLength = input.SelectionLength;
                Text = input.Text;
            }
            public int SelectionStart { get; set; }
            public int SelectionLength { get; set; }
            public string Text { get; set; }
        }
    }
}
﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

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
            if (IsPassWord) return;
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
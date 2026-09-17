// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace AntdUI
{
    /// <summary>
    /// Tree 节点拖拽放置模式（对齐 Ant Design Tree onDrop 的 dropPosition + dropToGap 语义）
    /// </summary>
    public enum TreeDropMode
    {
        /// <summary>
        /// 拖至目标上方，成为其前一个兄弟节点（等效 antd dropToGap=true 且 dropPosition=-1）
        /// </summary>
        Before = 0,
        /// <summary>
        /// 拖至目标下方，成为其后一个兄弟节点（等效 antd dropToGap=true 且 dropPosition=1）
        /// </summary>
        After = 1,
        /// <summary>
        /// 拖至目标内部，成为其子节点并追加至末尾（等效 antd dropToGap=false）
        /// </summary>
        Into = 2
    }
}
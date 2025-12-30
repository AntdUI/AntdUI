// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace AntdUI
{
    /// <summary>
    /// 步骤状态
    /// </summary>
    public enum TStepState
    {
        Wait,
        Process,
        Finish,
        Error
    }

    /// <summary>
    /// 里程碑类型
    /// </summary>
    public enum TMilestoneType
    {
        /// <summary>
        /// 精确到天：2025-7-12
        /// </summary>
        Day = 0,
        /// <summary>
        /// 仅显示时间：10:50:25
        /// </summary>
        Time = 1,
        /// <summary>
        /// 显示完整时间：2025-7-12 10:50:25
        /// </summary>
        Full = 2,
    }
}
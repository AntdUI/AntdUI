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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Int类型事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="value">数值</param>
    public delegate void IntEventHandler(object sender, int value);

    /// <summary>
    /// Int类型事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="value">数值</param>
    public delegate void IntNEventHandler(object sender, int? value);

    /// <summary>
    /// Float类型事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="value">数值</param>
    public delegate void FloatEventHandler(object sender, float value);

    /// <summary>
    /// Decimal类型事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="value">数值</param>
    public delegate void DecimalEventHandler(object sender, decimal value);

    /// <summary>
    /// Decimal类型事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="value">数值</param>
    public delegate void DecimalNEventHandler(object sender, decimal? value);

    /// <summary>
    /// Object类型事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="value">数值</param>
    public delegate void ObjectNEventHandler(object sender, object? value);

    /// <summary>
    /// Bool类型事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="value">数值</param>
    public delegate void BoolEventHandler(object sender, bool value);

    /// <summary>
    /// CheckStateE类型事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="value">数值</param>
    public delegate void CheckStateEventHandler(object sender, CheckState value);

    /// <summary>
    /// DateTime类型事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="value">数值</param>
    public delegate void DateTimeNEventHandler(object sender, DateTime? value);

    /// <summary>
    /// TimeSpan类型事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="value">数值</param>
    public delegate void TimeSpanNEventHandler(object sender, TimeSpan value);

    /// <summary>
    /// Color类型事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="value">数值</param>
    public delegate void ColorEventHandler(object sender, System.Drawing.Color value);
}
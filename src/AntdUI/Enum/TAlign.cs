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

namespace AntdUI
{
    /// <summary>
    /// 方向
    /// </summary>
    public enum TAlign
    {
        /// <summary>
        /// 无
        /// </summary>
        None,

        #region 上

        /// <summary>
        /// ↖ 上左
        /// </summary>
        TL,
        /// <summary>
        /// ↑上
        /// </summary>
        Top,
        /// <summary>
        /// ↗ 上右
        /// </summary>
        TR,

        #endregion

        #region 右

        /// <summary>
        /// ↗ 右上
        /// </summary>
        RT,
        /// <summary>
        /// → 右
        /// </summary>
        Right,
        /// <summary>
        /// ↘ 右下
        /// </summary>
        RB,

        #endregion

        #region 下

        /// <summary>
        /// ↘ 下右
        /// </summary>
        BR,
        /// <summary>
        /// ↓ 下
        /// </summary>
        Bottom,
        /// <summary>
        /// ↙ 下左
        /// </summary>
        BL,

        #endregion

        #region 左

        /// <summary>
        /// ↙ 左下
        /// </summary>
        LB,
        /// <summary>
        /// ← 左
        /// </summary>
        Left,

        /// <summary>
        /// ↖ 左上
        /// </summary>
        LT,

        #endregion
    }

    /// <summary>
    /// 大致方向
    /// </summary>
    public enum TAlignMini
    {
        /// <summary>
        /// 无
        /// </summary>
        None,


        /// <summary>
        /// ↑上
        /// </summary>
        Top,


        /// <summary>
        /// → 右
        /// </summary>
        Right,


        /// <summary>
        /// ↓ 下
        /// </summary>
        Bottom,


        /// <summary>
        /// ← 左
        /// </summary>
        Left
    }

    /// <summary>
    /// 圆角方向
    /// </summary>
    public enum TAlignRound
    {
        /// <summary>
        /// 无
        /// </summary>
        ALL,


        /// <summary>
        /// ↑上
        /// </summary>
        Top,


        /// <summary>
        /// → 右
        /// </summary>
        Right,


        /// <summary>
        /// ↓ 下
        /// </summary>
        Bottom,


        /// <summary>
        /// ← 左
        /// </summary>
        Left,


        /// <summary>
        /// ↖左上
        /// </summary>
        TL,


        /// <summary>
        /// ↗右上
        /// </summary>
        TR,


        /// <summary>
        /// ↘右下
        /// </summary>
        BR,

        /// <summary>
        /// ↙左下
        /// </summary>
        BL
    }

    /// <summary>
    /// 方向
    /// </summary>
    public enum TAlignFrom
    {
        #region 上

        /// <summary>
        /// ↖ 上左
        /// </summary>
        TL,
        /// <summary>
        /// ↑上
        /// </summary>
        Top,
        /// <summary>
        /// ↗ 上右
        /// </summary>
        TR,

        #endregion

        #region 下

        /// <summary>
        /// ↘ 下右
        /// </summary>
        BR,
        /// <summary>
        /// ↓ 下
        /// </summary>
        Bottom,
        /// <summary>
        /// ↙ 下左
        /// </summary>
        BL

        #endregion
    }

    /// <summary>
    /// Flow方向
    /// </summary>
    public enum TAlignFlow
    {
        /// <summary>
        /// ← 左中
        /// </summary>
        LeftCenter,
        /// <summary>
        /// ← 左
        /// </summary>
        Left,
        /// <summary>
        /// 中
        /// </summary>
        Center,
        /// <summary>
        /// → 右中
        /// </summary>
        RightCenter,
        /// <summary>
        /// → 右
        /// </summary>
        Right
    }
}
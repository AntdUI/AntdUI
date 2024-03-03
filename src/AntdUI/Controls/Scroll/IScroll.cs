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

using System.Drawing;

namespace AntdUI
{
    public interface IScroll
    {
        /// <summary>
        /// 是否显示
        /// </summary>
        bool Show { get; set; }

        /// <summary>
        /// 滚动条进度
        /// </summary>
        float Value { get; set; }
        float VrValue { get; set; }
        float VrValueI { get; set; }
        int SIZE { get; set; }

        bool MouseDown(Point e);
        bool MouseUp(Point e);
        bool MouseMove(Point e);
        void Leave();

        void SizeChange(Rectangle rect);
        void SetVrSize(float len, int value);
        void SetVrSize(float len);
        void Paint(Graphics g);
    }
}
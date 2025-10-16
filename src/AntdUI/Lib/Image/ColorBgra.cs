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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;
using System.Runtime.InteropServices;

namespace AntdUI
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ColorBgra
    {
        [FieldOffset(0)]
        public uint Bgra;

        [FieldOffset(0)]
        public byte Blue;
        [FieldOffset(1)]
        public byte Green;
        [FieldOffset(2)]
        public byte Red;
        [FieldOffset(3)]
        public byte Alpha;

        public const byte SizeOf = 4;

        public ColorBgra(uint bgra) : this()
        {
            Bgra = bgra;
        }

        public ColorBgra(byte b, byte g, byte r, byte a = 255) : this()
        {
            Blue = b;
            Green = g;
            Red = r;
            Alpha = a;
        }

        public ColorBgra(Color color) : this(color.B, color.G, color.R, color.A)
        {
        }

        public static bool operator ==(ColorBgra c1, ColorBgra c2) => c1.Bgra == c2.Bgra;

        public static bool operator !=(ColorBgra c1, ColorBgra c2) => c1.Bgra != c2.Bgra;

        public override bool Equals(object? obj) => obj is ColorBgra color && color.Bgra == Bgra;

        public override int GetHashCode()
        {
            unchecked
            {
                return (int)Bgra;
            }
        }

        public static implicit operator ColorBgra(uint color) => new ColorBgra(color);

        public static implicit operator uint(ColorBgra color) => color.Bgra;

        public Color ToColor() => Color.FromArgb(Alpha, Red, Green, Blue);

        public override string ToString() => string.Format("B: {0}, G: {1}, R: {2}, A: {3}", Blue, Green, Red, Alpha);

        public static uint BgraToUInt32(uint b, uint g, uint r, uint a) => b + (g << 8) + (r << 16) + (a << 24);

        public static uint BgraToUInt32(byte b, byte g, byte r, byte a) => b + ((uint)g << 8) + ((uint)r << 16) + ((uint)a << 24);
    }
}
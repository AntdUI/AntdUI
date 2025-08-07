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

using System;

namespace AntdUI
{
    public class OS
    {
        public static Version Version;

        /// <summary>
        /// 大于等于Windows 11
        /// </summary>
        public static bool Win11
        {
            get
            {
                var version = Version;
                if (version.Major >= 10 && version.Build > 22000) return true;
                return false;
            }
        }

        /// <summary>
        /// 大于等于Windows 10
        /// </summary>
        public static bool Win10 => Version.Major >= 10;

        public static bool Win10OrGreater(int build = -1)
        {
            var os = Version;
            return os.Major >= 10 && os.Build >= build;
        }

        /// <summary>
        /// 小于等于Windows 7
        /// </summary>
        public static bool Win7OrLower => Version.Major < 6 || (Version.Major == 6 && Version.Minor <= 1);

        #region OS版本

#if NET40 || NET46 || NET48
        static OS()
        {
            try
            {
                var osVersionInfo = new OSVERSIONINFOEX { OSVersionInfoSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(OSVERSIONINFOEX)) };
                if (RtlGetVersion(ref osVersionInfo) == 0)
                {
                    Version = new Version(osVersionInfo.MajorVersion, osVersionInfo.MinorVersion, osVersionInfo.BuildNumber);
                    return;
                }
            }
            catch { }
            Version = Environment.OSVersion.Version;
        }

        [System.Runtime.InteropServices.DllImport("ntdll.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        internal static extern int RtlGetVersion(ref OSVERSIONINFOEX versionInfo);

        internal struct OSVERSIONINFOEX
        {
            internal int OSVersionInfoSize;
            internal int MajorVersion;
            internal int MinorVersion;
            internal int BuildNumber;
            internal int PlatformId;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string CSDVersion;
            internal ushort ServicePackMajor;
            internal ushort ServicePackMinor;
            internal short SuiteMask;
            internal byte ProductType;
            internal byte Reserved;
        }
#else
        static OS()
        {
            Version = Environment.OSVersion.Version;
        }
#endif
        #endregion
    }
}
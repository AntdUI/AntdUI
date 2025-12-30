// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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

        /// <summary>
        /// 小于等于Windows 7
        /// </summary>
        public static bool Win7OrLower => Version.Major < 6 || (Version.Major == 6 && Version.Minor <= 1);

        public static bool Win10OrGreater(int build = -1)
        {
            var os = Version;
            return os.Major >= 10 && os.Build >= build;
        }

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
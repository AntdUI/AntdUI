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

using System.Media;
using System.Runtime.InteropServices;

namespace AntdUI
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 信息
        /// </summary>
        Information,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 问题
        /// </summary>
        Question
    }

    /// <summary>
    /// 系统声音助手
    /// </summary>
    public static class SystemSoundHelper
    {
        // P/Invoke 作为备选方案
        [DllImport("user32.dll", SetLastError = false)]
        private static extern bool MessageBeep(uint uType);

        private const uint MB_ICONASTERISK = 0x00000040;
        private const uint MB_ICONEXCLAMATION = 0x00000030;
        private const uint MB_ICONHAND = 0x00000010;
        private const uint MB_ICONQUESTION = 0x00000020;

        /// <summary>
        /// 播放系统声音
        /// </summary>
        /// <param name="messageType">消息类型</param>
        public static void PlaySound(MessageType messageType)
        {
            try
            {
                // 优先使用 SystemSounds（兼容性最好）
                PlaySystemSound(messageType);
            }
            catch
            {
                try
                {
                    // 降级到 P/Invoke
                    PlayMessageBeep(messageType);
                }
                catch
                {
                    // 静默失败，不影响主要功能
                }
            }
        }

        private static void PlaySystemSound(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Information:
                    SystemSounds.Asterisk.Play();
                    break;
                case MessageType.Warning:
                    SystemSounds.Exclamation.Play();
                    break;
                case MessageType.Error:
                    SystemSounds.Hand.Play();
                    break;
                case MessageType.Question:
                    SystemSounds.Question.Play();
                    break;
            }
        }

        private static void PlayMessageBeep(MessageType messageType)
        {
            uint beepType = messageType switch
            {
                MessageType.Information => MB_ICONASTERISK,
                MessageType.Warning => MB_ICONEXCLAMATION,
                MessageType.Error => MB_ICONHAND,
                MessageType.Question => MB_ICONQUESTION,
                _ => MB_ICONASTERISK
            };

            MessageBeep(beepType);
        }
    }
}
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

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Splitter 分隔面板
    /// </summary>
    /// <remarks>自由切分指定区域</remarks>
    [Description("Splitter 分隔面板")]
    [ToolboxItem(true)]
    public class Splitter : SplitContainer
    {
        public Splitter()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);

            SplitterMoving += Splitter_SplitterMoving;
            SplitterMoved += Splitter_SplitterMoved;
        }

        #region 属性

        /// <summary>
        /// 滑块大小
        /// </summary>
        [Description("滑块大小"), Category("行为"), DefaultValue(20)]
        public int SplitterSize { get; set; } = 20;

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Panel1Collapsed || Panel2Collapsed) return;
            var g = e.Graphics.High();
            var rect = SplitterRectangle;
            if (moving) g.Fill(Style.Db.PrimaryBg, rect);
            else g.Fill(Style.Db.FillTertiary, rect);
            int size = (int)(SplitterSize * Config.Dpi);
            if (Orientation == Orientation.Horizontal) g.Fill(Style.Db.Fill, new Rectangle(rect.X + (rect.Width - size) / 2, rect.Y, size, rect.Height));
            else g.Fill(Style.Db.Fill, new Rectangle(rect.X, rect.Y + (rect.Height - size) / 2, rect.Width, size));
        }

        #endregion

        #region 鼠标

        bool moving = false;
        private void Splitter_SplitterMoving(object? sender, SplitterCancelEventArgs e)
        {
            if (e.Cancel) return;
            moving = true;
            Invalidate();
        }

        private void Splitter_SplitterMoved(object? sender, SplitterEventArgs e)
        {
            moving = false;
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            SplitterMoving -= Splitter_SplitterMoving;
            SplitterMoved -= Splitter_SplitterMoved;
            base.Dispose(disposing);
        }

        #endregion
    }
}
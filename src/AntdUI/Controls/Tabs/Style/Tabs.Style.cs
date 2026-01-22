// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.ComponentModel;
using System.Drawing;

namespace AntdUI
{
    partial class Tabs
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public interface IStyle
        {
            void LoadLayout(Tabs owner, Rectangle rect, TabCollection items);
            void Paint(Tabs owner, Canvas g, TabCollection items);
            void SelectedIndexChanged(int i, int old);
            bool MouseClick(TabPage page, int i, int x, int y);
            void MouseMove(int x, int y);
            void MouseLeave();
            void Dispose();

            TabPageRect GetTabRect(int i);
        }
    }

    public class TabPageRect
    {
        public TabPageRect() { }
        public TabPageRect(Rectangle rect, Size size, int gap, bool left, int? rw = null)
        {
            Rect = rect;
            if (left) Rect_Text = new Rectangle(rect.X + gap, rect.Y, rw ?? size.Width, rect.Height);
            else
            {
                int w = rw ?? size.Width;
                Rect_Text = new Rectangle(rect.Right - gap - w, rect.Y, w, rect.Height);
            }
        }

        #region Line

        public TabPageRect SetLine(int barPadding, int barPadding2, int barSize)
        {
            Rect_Line = new Rectangle(Rect.X + barPadding, Rect.Y, Rect.Width - barPadding2, barSize);
            return this;
        }
        public TabPageRect SetLineB(int barPadding, int barPadding2, int barSize)
        {
            Rect_Line = new Rectangle(Rect.X + barPadding, Rect.Bottom - barSize, Rect.Width - barPadding2, barSize);
            return this;
        }
        public TabPageRect SetLine(Rectangle rect)
        {
            Rect_Line = rect;
            return this;
        }

        #endregion

        #region Card

        /// <summary>
        /// 布局（图标+文本）
        /// </summary>
        /// <param name="rect">项区域</param>
        /// <param name="size">文字大小</param>
        /// <param name="gap">边距</param>
        /// <param name="ico_size">图标大小</param>
        /// <param name="ico_gap">图标边距</param>
        public TabPageRect(Rectangle rect, Size size, int gap, int ico_size, int ico_gap, bool left, int? rw = null)
        {
            Rect = rect;
            int ux = rect.X + gap;
            Rect_Ico = new Rectangle(ux, rect.Y + (rect.Height - ico_size) / 2, ico_size, ico_size);
            if (left) Rect_Text = new Rectangle(ux + ico_size + ico_gap, rect.Y, rw ?? size.Width, rect.Height);
            else
            {
                int w = rw ?? size.Width;
                Rect_Text = new Rectangle(rect.Right - gap - w, rect.Y, w, rect.Height);
            }
        }

        /// <summary>
        /// 布局（图标+文本+关闭）
        /// </summary>
        /// <param name="rect">项区域</param>
        /// <param name="size">文字大小</param>
        /// <param name="gap">边距</param>
        /// <param name="ico_size">图标大小</param>
        /// <param name="close_size">关闭大小</param>
        /// <param name="ico_gap">图标边距</param>
        public TabPageRect(Rectangle rect, Size size, int gap, int ico_size, int close_size, int ico_gap, bool left, int? rw = null, int? tw = null)
        {
            Rect = rect;
            int ux = rect.X + gap;
            Rect_Ico = new Rectangle(ux, rect.Y + (rect.Height - ico_size) / 2, ico_size, ico_size);
            ux += ico_size + ico_gap;
            if (left) Rect_Text = new Rectangle(ux, rect.Y, rw ?? size.Width, rect.Height);
            else
            {
                int w = rw ?? size.Width;
                Rect_Text = new Rectangle(rect.Right - gap - close_size - ico_gap - w, rect.Y, w, rect.Height);
            }
            Rect_Close = new Rectangle(ux + (tw ?? size.Width) + ico_gap, rect.Y + (rect.Height - close_size) / 2, close_size, close_size);
        }

        /// <summary>
        /// 布局（图标+关闭）
        /// </summary>
        /// <param name="rect">项区域</param>
        /// <param name="size">文字大小</param>
        /// <param name="gap">边距</param>
        /// <param name="close_size">关闭大小</param>
        /// <param name="ico_gap">图标边距</param>
        public TabPageRect(Rectangle rect, bool test, Size size, int gap, int close_size, int ico_gap, bool left, int? rw = null, int? tw = null)
        {
            Rect = rect;
            int ux = rect.X + gap;
            if (left) Rect_Text = new Rectangle(ux, rect.Y, rw ?? size.Width, rect.Height);
            else
            {
                int w = rw ?? size.Width;
                Rect_Text = new Rectangle(rect.Right - gap - close_size - ico_gap - w, rect.Y, w, rect.Height);
            }
            Rect_Close = new Rectangle(ux + (tw ?? size.Width) + ico_gap, rect.Y + (rect.Height - close_size) / 2, close_size, close_size);
        }

        #endregion

        /// <summary>
        /// 总区域
        /// </summary>
        public Rectangle Rect;
        /// <summary>
        /// 线条区域
        /// </summary>
        public Rectangle Rect_Line;
        /// <summary>
        /// 文本区域
        /// </summary>
        public Rectangle Rect_Text;
        /// <summary>
        /// 图标区域
        /// </summary>
        public Rectangle Rect_Ico;
        /// <summary>
        /// 关闭按钮区域
        /// </summary>
        public Rectangle Rect_Close;

        internal ITaskOpacity? hover_close;
    }
}
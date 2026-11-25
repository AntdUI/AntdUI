// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// 消息/通知 动画基类（二代）
    /// </summary>
    internal abstract class ILayeredFormAnimate : ILayeredForm
    {
        #region 静态管理

        /// <summary>分组：key = name|align|area</summary>
        internal static ConcurrentDictionary<string, NoticeGroup> groups = new ConcurrentDictionary<string, NoticeGroup>();

        /// <summary>正在显示的 id 索引：key = 前缀+id</summary>
        internal static ConcurrentDictionary<string, ILayeredFormAnimate> id_map = new ConcurrentDictionary<string, ILayeredFormAnimate>();

        internal static bool CloseId(string prefix, string id) => CloseId(prefix + id);
        internal static bool CloseId(string idkey)
        {
            if (id_map.TryGetValue(idkey, out var form))
            {
                form.CloseMe();
                return true;
            }
            return false;
        }

        internal static void CloseAll(string name)
        {
            var close_list = new List<ILayeredFormAnimate>(10);
            foreach (var it in groups.Values)
            {
                if (it.Name != name) continue;
                foreach (var item in it.ToArray()) close_list.Add(item);
            }
            foreach (var it in close_list) it.CloseMe();
        }

        #endregion

        public abstract string name { get; }
        internal abstract TAlignFrom Align { get; }
        internal virtual int? MaxCount => null;

        internal string? id_key;
        internal NoticeGroup? group;
        internal Rectangle area;
        internal bool isTop => Align == TAlignFrom.Top || Align == TAlignFrom.TL || Align == TAlignFrom.TR;

        internal int end_X = 0, end_Y = 0;

        /// <summary>关闭中（已脱离联动队列）</summary>
        internal volatile bool closing = false;
        AnimationTask? task_show, task_chase, task_close;

        /// <summary>自动关闭计时代数（内容更新/堆叠时 +1 重置计时）</summary>
        internal int gen = 0;
        internal volatile bool expand_requested = false;

        #region 定位

        object[]? OffsetInWindow;
        Form? tmp_parent;

        /// <summary>
        /// 计算位置并入组（满溢固定关闭最旧一条，始终可以显示）
        /// </summary>
        internal void SetPosition(Target target, bool InWindow)
        {
            Rectangle? offset = null;
            Rectangle workingArea;
            if (target.Value is Form form)
            {
                if (InWindow || Config.ShowInWindow)
                {
                    workingArea = new Rectangle(form.Location, form.Size);
                    offset = workingArea;
                }
                else workingArea = Screen.FromControl(form).WorkingArea;
            }
            else if (target.Value is Control control)
            {
                var point = control.PointToScreen(Point.Empty);
                if (InWindow || Config.ShowInWindow)
                {
                    workingArea = new Rectangle(point, control.Size);
                    offset = workingArea;
                }
                else workingArea = Screen.FromPoint(point).WorkingArea;
            }
            else throw new ArgumentException("Target must be Form or Control");

            var align = Align;
            area = workingArea;
            var gkey = name + "|" + align.ToString() + "|" + workingArea.X + "|" + workingArea.Y + "|" + workingArea.Right + "|" + workingArea.Bottom;
            if (!groups.TryGetValue(gkey, out var g))
            {
                g = new NoticeGroup(name, align, workingArea);
                groups.TryAdd(gkey, g);
            }
            group = g;

            int width = TargetRect.Width;
            switch (align)
            {
                case TAlignFrom.Top:
                case TAlignFrom.Bottom:
                    end_X = workingArea.X + (workingArea.Width - width) / 2;
                    break;
                case TAlignFrom.TL:
                case TAlignFrom.BL:
                    end_X = workingArea.X;
                    break;
                case TAlignFrom.TR:
                case TAlignFrom.BR:
                    end_X = workingArea.X + workingArea.Width - width;
                    break;
            }
            // 入组并联动布局（满溢时固定关闭最旧一条）
            g.Add(this);
            if (offset.HasValue) OffsetInWindow = new object[] { target, isTop ? end_Y - workingArea.Y : workingArea.Bottom - end_Y };
            SetLocation(end_X, end_Y);
            inited = true;
        }

        /// <summary>初始布局完成（之前不启动追赶动画）</summary>
        bool inited = false;

        /// <summary>
        /// Y 联动：更新本条消息的目标 Y（由分组布局统一调用）
        /// </summary>
        internal void UpdateTargetY(int y)
        {
            if (end_Y == y) return;
            end_Y = y;
            SetPositionOffsetInWindow(y);
            if (closing || task_show != null || !inited) return;
            BeginChaseY();
        }

        void BeginChaseY()
        {
            if (task_chase != null) return;
            task_chase = new AnimationTask(new AnimationLoopConfig(this, ChaseFrame, 10).SetEnd(() => task_chase = null).SetPriority());
        }

        /// <summary>
        /// 追赶动画帧：指数缓动逼近目标 Y。
        /// 每条消息独立执行，目标变化时无需重启动画，连续关闭亦平滑过渡。
        /// </summary>
        bool ChaseFrame()
        {
            int target = end_Y, cur = TargetRect.Y;
            int diff = target - cur;
            if (diff == 0) return false;
            int step = (int)Math.Round(diff * 0.3F);
            if (step == 0) step = diff > 0 ? 1 : -1;
            SetAnimateValueY(cur + step);
            return true;
        }

        internal void SetPositionOffsetInWindow(int y)
        {
            if (OffsetInWindow == null) return;
            var target = (Target)OffsetInWindow[0];
            var workingArea = GetWorkingArea(target);
            if (isTop) OffsetInWindow = new object[] { target, y - workingArea.Y };
            else OffsetInWindow = new object[] { target, workingArea.Bottom - y };
        }

        internal Rectangle GetWorkingArea(Target target)
        {
            if (target.Value is Form form) return new Rectangle(form.Location, form.Size);
            else if (target.Value is Control control) return new Rectangle(control.PointToScreen(Point.Empty), control.Size);
            else return Rectangle.Empty;
        }

        /// <summary>
        /// 内容尺寸变化后调整 X（宽度变化时居中/靠边联动）
        /// </summary>
        internal void SetPositionX(int oldw)
        {
            int w = TargetRect.Width;
            switch (Align)
            {
                case TAlignFrom.Top:
                case TAlignFrom.Bottom:
                    if (end_X != (area.X + (area.Width - w) / 2))
                    {
                        end_X = area.X + (area.Width - w) / 2;
                        SetLocationX(end_X);
                    }
                    break;
                case TAlignFrom.TL:
                case TAlignFrom.BL:
                    end_X = area.X;
                    break;
                case TAlignFrom.TR:
                case TAlignFrom.BR:
                    if (end_X != (area.X + area.Width - w))
                    {
                        end_X = area.X + area.Width - w;
                        SetLocationX(end_X);
                    }
                    break;
            }
        }

        #endregion

        #region 显示动画

        /// <summary>
        /// 播放显示动画（方向位移 + 透明度过渡）
        /// </summary>
        internal void PlayShow()
        {
            if (!Config.HasAnimation(name))
            {
                SetAnimateValue(end_X, end_Y, 240);
                AfterShow();
                return;
            }
            var t = Animation.TotalFrames(10, 200);
            bool vertical = Align == TAlignFrom.Top || Align == TAlignFrom.Bottom;
            int start_Y = Align == TAlignFrom.Top ? end_Y - TargetRect.Height / 2 : end_Y + TargetRect.Height / 2;
            int start_X;
            switch (Align)
            {
                case TAlignFrom.TL:
                case TAlignFrom.BL:
                    start_X = end_X - TargetRect.Width / 3;
                    break;
                case TAlignFrom.TR:
                case TAlignFrom.BR:
                    start_X = end_X + TargetRect.Width / 3;
                    break;
                default:
                    start_X = end_X;
                    break;
            }
            SetLocation(start_X, vertical ? start_Y : end_Y);
            alpha = 0;
            task_show = new AnimationTask(i =>
            {
                var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                if (vertical) SetAnimateValueY(start_Y + (int)((end_Y - start_Y) * val), (byte)(240 * val));
                else SetAnimateValueX(start_X + (int)((end_X - start_X) * val), (byte)(240 * val));
                return true;
            }, 10, t, () =>
            {
                task_show = null;
                SetAnimateValue(end_X, end_Y, 240);
                AfterShow();
            });
        }

        /// <summary>显示动画结束：挂接父容器移动监听</summary>
        internal virtual void AfterShow()
        {
            if (OffsetInWindow == null) return;
            var target = (Target)OffsetInWindow[0];
            var parent = target.Parent();
            if (parent == null) return;
            tmp_parent = parent;
            parent.LocationChanged += Parent_LSChanged;
            parent.SizeChanged += Parent_LSChanged;
        }

        void Parent_LSChanged(object? sender, EventArgs e)
        {
            if (OffsetInWindow == null || closing) return;
            var target = (Target)OffsetInWindow[0];
            var offset = (int)OffsetInWindow[1];
            var workingArea = GetWorkingArea(target);
            int end_Y_new = isTop ? workingArea.Y + offset : workingArea.Bottom - offset;
            int end_X_new;
            switch (Align)
            {
                case TAlignFrom.Top:
                case TAlignFrom.Bottom:
                    end_X_new = workingArea.X + (workingArea.Width - TargetRect.Width) / 2;
                    break;
                case TAlignFrom.TL:
                case TAlignFrom.BL:
                    end_X_new = workingArea.X;
                    break;
                default:
                    end_X_new = workingArea.X + workingArea.Width - TargetRect.Width;
                    break;
            }
            end_X = end_X_new;
            end_Y = end_Y_new;
            SetLocation(end_X, end_Y);
            PrintCache();
        }

        #endregion

        #region 关闭

        /// <summary>
        /// 关闭：瞬间脱离联动队列，自行播放关闭动画（方向位移 + 淡出），其余消息独立过渡补位。
        /// </summary>
        public void CloseMe()
        {
            if (closing) return;
            closing = true;
            task_show?.Dispose(); task_show = null;
            task_chase?.Dispose(); task_chase = null;
            if (id_key != null) id_map.TryRemove(id_key, out _);
            if (group != null)
            {
                var g = group;
                group = null;
                g.Remove(this);
            }

            if (!Config.HasAnimation(name) || !IsHandleCreated)
            {
                IClose(true);
                return;
            }
            var t = Animation.TotalFrames(10, 200);
            int x0 = TargetRect.X, y0 = TargetRect.Y, w = TargetRect.Width, h = TargetRect.Height;
            int dx, dy;
            switch (Align)
            {
                case TAlignFrom.Top: dy = -h / 2; dx = 0; break;        // 从下往上
                case TAlignFrom.Bottom: dy = h / 2; dx = 0; break;      // 从上往下
                case TAlignFrom.TL: case TAlignFrom.BL: dx = -w / 3; dy = 0; break;  // 从右往左
                default: dx = w / 3; dy = 0; break;                     // 从左往右
            }
            task_close = new AnimationTask(i =>
            {
                var val = Animation.Animate(i, t, 1F, AnimationType.Ball);
                SetAnimateValue(x0 + (int)(dx * val), y0 + (int)(dy * val), (byte)(240 * (1F - val)));
                return true;
            }, 10, t, () => IClose(true));
        }

        /// <summary>
        /// 自动关闭计时（内容更新 gen+1 / contains(id,true) 均会重置）
        /// </summary>
        internal void StartAutoClose(int seconds)
        {
            if (seconds <= 0) return;
            ITask.Run(() =>
            {
                int gen0 = gen, slept = 0;
                while (true)
                {
                    Thread.Sleep(200);
                    slept += 200;
                    if (closing || IsDisposed) return;
                    if (gen != gen0) { gen0 = gen; slept = 0; }
                    if (expand_requested) { expand_requested = false; slept = 0; }
                    if (slept >= seconds * 1000) { CloseMe(); return; }
                }
            });
        }

        #endregion

        #region 渲染

        Bitmap? bmp_tmp;
        void SetAnimateValueX(int x, byte _alpha)
        {
            if (TargetRect.X != x || alpha != _alpha)
            {
                SetLocationX(x);
                alpha = _alpha;
                bmp_tmp ??= Printmap();
                if (bmp_tmp == null) return;
                if (Print(bmp_tmp) == Win32.RenderResult.Invalid) bmp_tmp = null;
            }
        }
        void SetAnimateValueY(int y, byte _alpha)
        {
            if (TargetRect.Y != y || alpha != _alpha)
            {
                SetLocationY(y);
                alpha = _alpha;
                bmp_tmp ??= Printmap();
                if (bmp_tmp == null) return;
                if (Print(bmp_tmp) == Win32.RenderResult.Invalid) bmp_tmp = null;
            }
        }
        internal void SetAnimateValueY(int y)
        {
            if (TargetRect.Y != y)
            {
                SetLocationY(y);
                bmp_tmp ??= Printmap();
                if (bmp_tmp == null) return;
                if (Print(bmp_tmp) == Win32.RenderResult.Invalid) bmp_tmp = null;
            }
        }
        void SetAnimateValue(int x, int y, byte _alpha)
        {
            if (TargetRect.X != x || TargetRect.Y != y || alpha != _alpha)
            {
                SetLocation(x, y);
                alpha = _alpha;
                bmp_tmp ??= Printmap();
                if (bmp_tmp == null) return;
                if (Print(bmp_tmp) == Win32.RenderResult.Invalid) bmp_tmp = null;
            }
        }
        internal void DisposeAnimation()
        {
            bmp_tmp?.Dispose();
            bmp_tmp = null;
        }

        #endregion

        #region 释放

        protected override void Dispose(bool disposing)
        {
            task_show?.Dispose(); task_show = null;
            task_chase?.Dispose(); task_chase = null;
            task_close?.Dispose(); task_close = null;
            if (id_key != null) id_map.TryRemove(id_key, out _);
            if (tmp_parent != null)
            {
                tmp_parent.LocationChanged -= Parent_LSChanged;
                tmp_parent.SizeChanged -= Parent_LSChanged;
                tmp_parent = null;
            }
            if (group != null)
            {
                var g = group;
                group = null;
                g.Remove(this);
            }
            DisposeAnimation();
            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// 消息/通知 分组（同 name + 同方向 + 同工作区域为一组，上下堆叠）
    /// </summary>
    internal class NoticeGroup
    {
        internal readonly string Name;
        internal readonly TAlignFrom Align;
        internal readonly Rectangle Area;
        internal readonly bool IsTop;
        readonly List<ILayeredFormAnimate> items = new List<ILayeredFormAnimate>(4);
        readonly object locker = new object();

        public NoticeGroup(string name, TAlignFrom align, Rectangle area)
        {
            Name = name;
            Align = align;
            Area = area;
            IsTop = align == TAlignFrom.Top || align == TAlignFrom.TL || align == TAlignFrom.TR;
        }

        int EdgeOffset => (int)(Config.NoticeWindowOffsetXY * Config.Dpi);

        internal void Add(ILayeredFormAnimate item)
        {
            lock (locker)
            {
                if (IsTop) items.Insert(0, item); // Top/TL/TR：新的在上面
                else items.Add(item);             // Bottom/BL/BR：新的在下面
                Relayout();
            }
        }

        internal void Remove(ILayeredFormAnimate item)
        {
            lock (locker)
            {
                if (items.Remove(item)) Relayout();
            }
        }

        internal ILayeredFormAnimate[] ToArray()
        {
            lock (locker) return items.ToArray();
        }

        /// <summary>
        /// 联动布局：重排所有消息的目标 Y（每条消息独立追赶过渡）；
        /// 满溢固定关闭最旧一条。
        /// </summary>
        internal void Relayout()
        {
            lock (locker)
            {
                int off = EdgeOffset;
                if (IsTop)
                {
                    // 满溢：从最旧（列表尾部）开始关闭
                    while (items.Count > 1)
                    {
                        int total = 0;
                        foreach (var it in items) total += it.TargetRect.Height;
                        if (Area.Y + off + total <= Area.Bottom) break;
                        items[items.Count - 1].CloseMe();
                    }
                    // 最大数量限制：从最旧开始关闭
                    var mc = items.Count > 0 ? items[0].MaxCount : null;
                    while (mc.HasValue && items.Count > mc.Value) items[items.Count - 1].CloseMe();
                    int y_temp = Area.Y + off;
                    foreach (var it in items)
                    {
                        it.UpdateTargetY(y_temp);
                        y_temp += it.TargetRect.Height;
                    }
                }
                else
                {
                    // 满溢：从最旧（列表头部）开始关闭
                    while (items.Count > 1)
                    {
                        int total = 0;
                        foreach (var it in items) total += it.TargetRect.Height;
                        if (Area.Bottom - off - total >= Area.Y) break;
                        items[0].CloseMe();
                    }
                    // 最大数量限制：从最旧开始关闭
                    var mc = items.Count > 0 ? items[0].MaxCount : null;
                    while (mc.HasValue && items.Count > mc.Value) items[0].CloseMe();
                    int y_temp = Area.Bottom - off;
                    for (int i = items.Count - 1; i >= 0; i--)
                    {
                        var it = items[i];
                        y_temp -= it.TargetRect.Height;
                        it.UpdateTargetY(y_temp);
                    }
                }
            }
        }
    }
}
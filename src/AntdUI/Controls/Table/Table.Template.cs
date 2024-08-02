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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI
{
    partial class Table
    {
        /// <summary>
        /// 行数据
        /// </summary>
        internal class RowTemplate
        {
            Table PARENT;
            public RowTemplate(Table table, TCell[] cell, object? value)
            {
                PARENT = table;
                cells = cell;
                RECORD = value;
            }

            /// <summary>
            /// 内部判断脏渲染
            /// </summary>
            internal bool SHOW { get; set; }

            /// <summary>
            /// 行区域
            /// </summary>
            public Rectangle RECT { get; set; }

            /// <summary>
            /// 原始行数据
            /// </summary>
            public object? RECORD { get; set; }

            public int INDEX { get; set; }

            /// <summary>
            /// 列数据
            /// </summary>
            public TCell[] cells { get; set; }

            /// <summary>
            /// 行高度
            /// </summary>
            public int Height { get; set; }

            internal bool IsColumn = false;

            #region 悬浮状态

            internal bool hover = false;
            /// <summary>
            /// 是否移动
            /// </summary>
            internal bool Hover
            {
                get => hover;
                set
                {
                    if (hover == value) return;
                    hover = value;
                    if (SHOW)
                    {
                        if (Config.Animation)
                        {
                            ThreadHover?.Dispose();
                            AnimationHover = true;
                            var t = Animation.TotalFrames(20, 200);
                            if (value)
                            {
                                ThreadHover = new ITask((i) =>
                                {
                                    AnimationHoverValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    PARENT.Invalidate();
                                    return true;
                                }, 20, t, () =>
                                {
                                    AnimationHover = false;
                                    AnimationHoverValue = 1;
                                    PARENT.Invalidate();
                                });
                            }
                            else
                            {
                                ThreadHover = new ITask((i) =>
                                {
                                    AnimationHoverValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    PARENT.Invalidate();
                                    return true;
                                }, 20, t, () =>
                                {
                                    AnimationHover = false;
                                    AnimationHoverValue = 0;
                                    PARENT.Invalidate();
                                });
                            }
                        }
                        else PARENT.Invalidate();
                    }
                }
            }

            internal bool Contains(int x, int y)
            {
                if (CONTAINS(x, y))
                {
                    Hover = true;
                    return true;
                }
                else
                {
                    Hover = false;
                    return false;
                }
            }
            internal bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }

            internal float AnimationHoverValue = 0;
            internal bool AnimationHover = false;
            internal ITask? ThreadHover = null;

            #endregion

            public bool Select { get; set; }

            public bool CanExpand { get; set; }

            public bool Expand { get; set; }
            public bool ShowExpand { get; set; } = true;

            internal int ExpandDepth { get; set; }
            internal int KeyTreeINDEX { get; set; } = -1;
            internal Rectangle RectExpand;
        }

        #region 单元格

        /// <summary>
        /// 复选框
        /// </summary>
        class TCellCheck : TCell
        {
            public TCellCheck(Table table, PropertyDescriptor? prop, object ov, bool value, ColumnCheck column)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                _checked = value;
                AnimationCheckValue = _checked ? 1F : 0F;
                NoTitle = column.NoTitle;
                AutoCheck = column.AutoCheck;
            }

            #region 选中状态

            internal bool AnimationCheck = false;
            internal float AnimationCheckValue = 0;

            ITask? ThreadCheck = null;

            bool _checked = false;
            [Description("选中状态"), Category("行为"), DefaultValue(false)]
            public bool Checked
            {
                get => _checked;
                set
                {
                    if (_checked == value) return;
                    _checked = value;
                    OnCheck();
                }
            }

            void OnCheck()
            {
                ThreadCheck?.Dispose();
                if (ROW.SHOW && PARENT.IsHandleCreated)
                {
                    if (Config.Animation)
                    {
                        AnimationCheck = true;
                        if (_checked)
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(0.2F);
                                if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                        else
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(-0.2F);
                                if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                    }
                    else
                    {
                        AnimationCheckValue = _checked ? 1F : 0F;
                        PARENT.Invalidate();
                    }
                }
            }

            #endregion

            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle rect { get; set; }

            public void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2)
            {
            }
            internal void SetSize(Rectangle _rect, int check_size)
            {
                RECT = _rect;
                rect = new Rectangle(_rect.X + (_rect.Width - check_size) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size, check_size);
            }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public int INDEX { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }

            internal bool Contains(int x, int y)
            {
                return rect.Contains(x, y);
            }

            public SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2)
            {
                var sizef = g.MeasureString(Config.NullText, font);
                MinWidth = (int)Math.Ceiling(sizef.Width);
                return sizef;
            }

            public int MouseDown { get; set; }
            public int MinWidth { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif

            public override string ToString()
            {
                return Checked.ToString();
            }

            internal bool NoTitle { get; set; }
            internal bool AutoCheck { get; set; }
        }

        /// <summary>
        /// 单选框
        /// </summary>
        class TCellRadio : TCell
        {
            public TCellRadio(Table table, PropertyDescriptor? prop, object ov, bool value, ColumnRadio column)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                _checked = value;
                AnimationCheckValue = _checked ? 1F : 0F;
                AutoCheck = column.AutoCheck;
            }

            #region 选中状态

            internal bool AnimationCheck = false;
            internal float AnimationCheckValue = 0;

            ITask? ThreadCheck = null;

            bool _checked = false;
            [Description("选中状态"), Category("行为"), DefaultValue(false)]
            public bool Checked
            {
                get => _checked;
                set
                {
                    if (_checked == value) return;
                    _checked = value;
                    OnCheck();
                }
            }

            void OnCheck()
            {
                ThreadCheck?.Dispose();
                if (ROW.SHOW && PARENT.IsHandleCreated)
                {
                    if (Config.Animation)
                    {
                        AnimationCheck = true;
                        if (_checked)
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(0.2F);
                                if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                        else
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(-0.2F);
                                if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 20, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                    }
                    else
                    {
                        AnimationCheckValue = _checked ? 1F : 0F;
                        PARENT.Invalidate();
                    }
                }
            }

            #endregion

            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle rect { get; set; }

            public void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2)
            {
            }
            internal void SetSize(Rectangle _rect, int check_size)
            {
                RECT = _rect;
                rect = new Rectangle(_rect.X + (_rect.Width - check_size) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size, check_size);
            }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public int INDEX { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }

            internal bool Contains(int x, int y)
            {
                return rect.Contains(x, y);
            }

            public SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2)
            {
                var sizef = g.MeasureString(Config.NullText, font);
                MinWidth = (int)Math.Ceiling(sizef.Width);
                return sizef;
            }

            public int MouseDown { get; set; }
            public int MinWidth { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public override string ToString()
            {
                return Checked.ToString();
            }
            internal bool AutoCheck { get; set; }
        }

        /// <summary>
        /// 开关
        /// </summary>
        class TCellSwitch : TCell
        {
            public TCellSwitch(Table table, PropertyDescriptor? prop, object ov, bool value, ColumnSwitch _column)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                column = _column;
                _checked = value;
                AnimationCheckValue = _checked ? 1F : 0F;
                AutoCheck = _column.AutoCheck;
            }

            public ColumnSwitch column { get; set; }

            #region 选中状态

            internal bool AnimationCheck = false;
            internal float AnimationCheckValue = 0;

            ITask? ThreadCheck = null;

            bool _checked = false;
            [Description("选中状态"), Category("行为"), DefaultValue(false)]
            public bool Checked
            {
                get => _checked;
                set
                {
                    if (_checked == value) return;
                    _checked = value;
                    OnCheck();
                }
            }

            void OnCheck()
            {
                ThreadCheck?.Dispose();
                if (ROW.SHOW && PARENT.IsHandleCreated)
                {
                    if (Config.Animation)
                    {
                        AnimationCheck = true;
                        if (_checked)
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(0.1F);
                                if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 10, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                        else
                        {
                            ThreadCheck = new ITask(PARENT, () =>
                            {
                                AnimationCheckValue = AnimationCheckValue.Calculate(-0.1F);
                                if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 10, () =>
                            {
                                AnimationCheck = false;
                                PARENT.Invalidate();
                            });
                        }
                    }
                    else
                    {
                        AnimationCheckValue = _checked ? 1F : 0F;
                        PARENT.Invalidate();
                    }
                }
            }

            #endregion

            #region 悬浮

            ITask? ThreadHover = null;
            internal float AnimationHoverValue = 0;
            internal bool AnimationHover = false;
            internal bool _mouseHover = false;
            internal bool ExtraMouseHover
            {
                get => _mouseHover;
                set
                {
                    if (_mouseHover == value) return;
                    _mouseHover = value;
                    if (ROW.SHOW && PARENT.IsHandleCreated && Config.Animation)
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        if (value)
                        {
                            ThreadHover = new ITask(PARENT, () =>
                            {
                                AnimationHoverValue = AnimationHoverValue.Calculate(0.1F);
                                if (AnimationHoverValue > 1) { AnimationHoverValue = 1F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 10, () =>
                            {
                                AnimationHover = false;
                                PARENT.Invalidate();
                            });
                        }
                        else
                        {
                            ThreadHover = new ITask(PARENT, () =>
                            {
                                AnimationHoverValue = AnimationHoverValue.Calculate(-0.1F);
                                if (AnimationHoverValue <= 0) { AnimationHoverValue = 0F; return false; }
                                PARENT.Invalidate();
                                return true;
                            }, 10, () =>
                            {
                                AnimationHover = false;
                                PARENT.Invalidate();
                            });
                        }
                    }
                    else AnimationHoverValue = 255;
                    PARENT.Invalidate();
                }
            }

            #endregion

            #region 加载中

            bool loading = false;
            public bool Loading
            {
                get => loading;
                set
                {
                    if (loading == value) return;
                    loading = value;
                    if (ROW.SHOW && PARENT.IsHandleCreated)
                    {
                        if (loading)
                        {
                            bool ProgState = false;
                            ThreadLoading = new ITask(PARENT, () =>
                            {
                                if (ProgState)
                                {
                                    LineAngle = LineAngle.Calculate(9F);
                                    LineWidth = LineWidth.Calculate(0.6F);
                                    if (LineWidth > 75) ProgState = false;
                                }
                                else
                                {
                                    LineAngle = LineAngle.Calculate(9.6F);
                                    LineWidth = LineWidth.Calculate(-0.6F);
                                    if (LineWidth < 6) ProgState = true;
                                }
                                if (LineAngle >= 360) LineAngle = 0;
                                PARENT.Invalidate();
                                return true;
                            }, 10);
                        }
                        else ThreadLoading?.Dispose();
                    }
                    PARENT.Invalidate();
                }
            }

            ITask? ThreadLoading = null;
            internal float LineWidth = 6, LineAngle = 0;

            #endregion

            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle rect { get; set; }

            public void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2)
            {
            }
            internal void SetSize(Rectangle _rect, int check_size)
            {
                int check_size2 = check_size * 2;
                RECT = _rect;
                rect = new Rectangle(_rect.X + (_rect.Width - check_size2) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size2, check_size);
            }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public int INDEX { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }

            internal bool Contains(int x, int y)
            {
                return rect.Contains(x, y);
            }

            public SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2)
            {
                var sizef = g.MeasureString(Config.NullText, font);
                MinWidth = (int)Math.Ceiling(sizef.Width);
                return sizef;
            }

            public int MouseDown { get; set; }
            public int MinWidth { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public override string ToString()
            {
                return Checked.ToString();
            }
            internal bool AutoCheck { get; set; }
        }

        /// <summary>
        /// 普通文本
        /// </summary>
        class TCellText : TCell
        {
            public TCellText(Table table, PropertyDescriptor? prop, object? ov, Column _column, string? _value)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                column = _column;
                value = _value;
            }

            /// <summary>
            /// 值
            /// </summary>
            public string? value { get; set; }
            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle rect { get; set; }
            public void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2)
            {
                RECT = _rect;
                rect = new Rectangle(_rect.X + gap + ox, _rect.Y + gap, _rect.Width - gap2, _rect.Height - gap2);
            }
            public Column column { get; set; }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public int INDEX { get; set; }
            public object? VALUE { get; set; }
            public Rectangle RECT { get; set; }

            public SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2)
            {
                if (column.LineBreak)
                {
                    if (column.Width != null)
                    {
                        if (column.Width.EndsWith("%") && float.TryParse(column.Width.TrimEnd('%'), out var f))
                        {
                            var size2 = g.MeasureString(value, font, (int)Math.Ceiling(width * (f / 100F)));
                            MinWidth = (int)Math.Ceiling(size2.Width);
                            return new SizeF(size2.Width + gap2, size2.Height);
                        }
                        else if (int.TryParse(column.Width, out var i))
                        {
                            var size2 = g.MeasureString(value, font, (int)Math.Ceiling(i * Config.Dpi));
                            MinWidth = (int)Math.Ceiling(size2.Width);
                            return new SizeF(size2.Width + gap2, size2.Height);
                        }
                    }
                }
                var size = g.MeasureString(value, font);
                MinWidth = (int)Math.Ceiling(size.Width);
                return new SizeF(size.Width + gap2, size.Height);
            }

            public int MouseDown { get; set; }
            public int MinWidth { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public override string? ToString()
            {
                return value;
            }
        }

        /// <summary>
        /// 表头
        /// </summary>
        internal class TCellColumn : TCell
        {
            public TCellColumn(Table table, Column _column)
            {
                PARENT = table;
                column = _column;
                value = _column.Title;
            }

            /// <summary>
            /// 值
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle rect { get; set; }

            public Rectangle rect_up { get; set; }
            public Rectangle rect_down { get; set; }
            public void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2)
            {
                RECT = _rect;
                if (column.SortOrder)
                {
                    int icon_sp = (int)(gap * 0.34F), y = _rect.Y + (_rect.Height - (gap * 2) + icon_sp) / 2;
                    rect_up = new Rectangle(_rect.Right - gap2, y, gap, gap);
                    rect_down = new Rectangle(rect_up.X, rect_up.Bottom - icon_sp, gap, gap);
                }
            }

            public Column column { get; set; }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public int INDEX { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }

            internal bool Contains(int x, int y)
            {
                return rect.Contains(x, y);
            }

            internal int SortWidth = 0;
            public SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2)
            {
                var size = g.MeasureString(value, font);
                SortWidth = column.SortOrder ? (int)(size.Height * 0.8F) : 0;
                MinWidth = (int)Math.Ceiling(size.Width) + gap2 + SortWidth;
                return new SizeF(size.Width + gap2 + SortWidth, size.Height);
            }

            public int MouseDown { get; set; }
            public int MinWidth { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public override string ToString()
            {
                return value;
            }
        }

        internal interface TCell
        {
            PropertyDescriptor? PROPERTY { get; set; }
            object VALUE { get; set; }
            int INDEX { get; set; }
            Table PARENT { get; set; }
            RowTemplate ROW { get; set; }
            Rectangle RECT { get; set; }
            Rectangle rect { get; set; }

            void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2);
            SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2);

            int MinWidth { get; set; }

            int MouseDown { get; set; }

#if NET40 || NET46 || NET48
            bool CONTAINS(int x, int y);
#else
            internal bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        #region 容器

        /// <summary>
        /// 包裹容器
        /// </summary>
        class Template : TCell
        {
            public Template(Table table, PropertyDescriptor? prop, object ov, Column _column, ref int processing, IList<ICell> _value)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                column = _column;
                var list = new List<ITemplate>();
                foreach (var it in _value)
                {
                    if (it is CellBadge badge)
                    {
                        if (badge.State == TState.Processing) processing++;
                        list.Add(new TemplateBadge(badge, this));
                    }
                    else if (it is CellTag tag) list.Add(new TemplateTag(tag, this));
                    else if (it is CellImage image) list.Add(new TemplateImage(image, this));
                    else if (it is CellLink link)
                    {
                        HasBtn = true;
                        list.Add(new TemplateButton(link, this));
                    }
                    else if (it is CellText text) list.Add(new TemplateText(text, this));
                    else if (it is CellProgress progress) list.Add(new TemplateProgress(progress, this));
                    else if (it is CellDivider divider) list.Add(new TemplateDivider(divider, this));
                }
                value = list;
            }

            /// <summary>
            /// 值
            /// </summary>
            public IList<ITemplate> value { get; set; }

            internal bool HasBtn = false;

            public void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int _gap, int _gap2)
            {
                RECT = rect = _rect;
                int rx = _rect.X + ox;
                int gap = _gap / 2, gap2 = _gap;
                if (value.Count == 1 && (value[0] is TemplateText || value[0] is TemplateProgress))
                {
                    var it = value[0];
                    var size = SIZES[0];
                    it.SetRect(g, font, new Rectangle(rx, _rect.Y, _rect.Width, _rect.Height), size, gap, gap2);
                }
                else
                {
                    int use_x;
                    switch (column.Align)
                    {
                        case ColumnAlign.Center: use_x = rx + (_rect.Width - MinWidth) / 2; break;
                        case ColumnAlign.Right: use_x = _rect.Right - MinWidth; break;
                        case ColumnAlign.Left:
                        default: use_x = rx + gap2; break;
                    }
                    for (int i = 0; i < value.Count; i++)
                    {
                        var it = value[i];
                        var size = SIZES[i];
                        it.SetRect(g, font, new Rectangle(use_x, _rect.Y, size.Width, _rect.Height), size, gap, gap2);
                        use_x += size.Width;
                    }
                }
            }
            public Column column { get; set; }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public int INDEX { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }
            public Rectangle rect { get; set; }

            Size[] SIZES;
            public SizeF GetSize(Graphics g, Font font, int width, int _gap, int _gap2)
            {
                int gap = _gap / 2, gap2 = _gap;
                float w = 0, h = 0;
                var sizes = new List<Size>(value.Count);
                foreach (var it in value)
                {
                    var size = it.GetSize(g, font, gap, gap2);
                    sizes.Add(size);
                    w += size.Width;
                    if (h < size.Height) h = size.Height;
                }
                MinWidth = (int)Math.Ceiling(w);
                SIZES = sizes.ToArray();
                return new SizeF(MinWidth + _gap2, h);
            }

            public int MouseDown { get; set; }
            public int MinWidth { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public override string? ToString()
            {
                var vals = new List<string>(value.Count);
                foreach (var cell in value)
                {
                    var str = cell.ToString();
                    if (!string.IsNullOrEmpty(str)) vals.Add(str);
                }
                return string.Join(" ", vals);
            }
        }

        /// <summary>
        /// Text 文本
        /// </summary>
        class TemplateText : ITemplate
        {
            public TemplateText(CellText value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void PaintBack(Graphics g, TCell it)
            {
                if (Value.Back.HasValue)
                {
                    using (var brush = new SolidBrush(Value.Back.Value))
                    {
                        g.FillRectangle(brush, it.RECT);
                    }
                }
            }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                if (Value.Fore.HasValue)
                {
                    using (var brush = new SolidBrush(Value.Fore.Value))
                    {
                        g.DrawString(Value.Text, Value.Font ?? font, brush, Rect, StringF(PARENT.column));
                    }
                }
                else g.DrawString(Value.Text, Value.Font ?? font, fore, Rect, StringF(PARENT.column));

                if (Value.PrefixSvg != null)
                {
                    using (var _bmp = SvgExtend.GetImgExtend(Value.PrefixSvg, RectL, Value.Fore ?? fore.Color))
                    {
                        if (_bmp != null) g.DrawImage(_bmp, RectL);
                    }
                }
                else if (Value.Prefix != null) g.DrawImage(Value.Prefix, RectL);

                if (Value.SuffixSvg != null)
                {
                    using (var _bmp = SvgExtend.GetImgExtend(Value.SuffixSvg, RectR, Value.Fore ?? fore.Color))
                    {
                        if (_bmp != null) g.DrawImage(_bmp, RectR);
                    }
                }
                else if (Value.Suffix != null) g.DrawImage(Value.Suffix, RectR);
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(Value.Text, Value.Font ?? font);
                bool has_prefix = Value.HasPrefix, has_suffix = Value.HasSuffix;
                if (has_prefix && has_suffix)
                {
                    int icon_size = (int)(size.Height * Value.IconRatio);
                    return new Size((icon_size * 2) + gap2 + (int)Math.Ceiling(size.Width) + gap2, (int)Math.Ceiling(size.Height));
                }
                else if (has_prefix || has_suffix)
                {
                    int icon_size = (int)(size.Height * Value.IconRatio);
                    return new Size(icon_size + gap + (int)Math.Ceiling(size.Width) + gap2, (int)Math.Ceiling(size.Height));
                }
                return new Size((int)Math.Ceiling(size.Width) + gap2, (int)Math.Ceiling(size.Height));
            }

            Rectangle Rect, RectL, RectR;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                bool has_prefix = Value.HasPrefix, has_suffix = Value.HasSuffix;
                if (has_prefix && has_suffix)
                {
                    int icon_size = (int)(size.Height * Value.IconRatio);
                    RectL = new Rectangle(rect.X + gap, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                    RectR = new Rectangle(rect.Right - gap - icon_size, RectL.Y, icon_size, icon_size);

                    Rect = new Rectangle(RectL.Right + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2 - (icon_size * 2 + gap2), size.Height);
                }
                else if (has_prefix)
                {
                    int icon_size = (int)(size.Height * Value.IconRatio);
                    RectL = new Rectangle(rect.X + gap, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                    Rect = new Rectangle(RectL.Right + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2 - icon_size - gap, size.Height);
                }
                else if (has_suffix)
                {
                    int icon_size = (int)(size.Height * Value.IconRatio);
                    RectR = new Rectangle(rect.Right - gap - icon_size, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                    Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2 - icon_size - gap, size.Height);
                }
                else Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2, size.Height);
            }

            public CellText Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public bool SValue(object obj)
            {
                if (obj is CellText value) { Value = value; return true; }
                return false;
            }
            public override string? ToString()
            {
                return Value.Text;
            }
        }

        /// <summary>
        /// Tag 标签
        /// </summary>
        class TemplateTag : ITemplate
        {
            public TemplateTag(CellTag value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void PaintBack(Graphics g, TCell it) { }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                using (var path = Rect.RoundPath(6))
                {
                    #region 绘制背景

                    Color _fore, _back, _bor;
                    switch (Value.Type)
                    {
                        case TTypeMini.Default:
                            _back = Style.Db.TagDefaultBg;
                            _fore = Style.Db.TagDefaultColor;
                            _bor = Style.Db.DefaultBorder;
                            break;
                        case TTypeMini.Error:
                            _back = Style.Db.ErrorBg;
                            _fore = Style.Db.Error;
                            _bor = Style.Db.ErrorBorder;
                            break;
                        case TTypeMini.Success:
                            _back = Style.Db.SuccessBg;
                            _fore = Style.Db.Success;
                            _bor = Style.Db.SuccessBorder;
                            break;
                        case TTypeMini.Info:
                            _back = Style.Db.InfoBg;
                            _fore = Style.Db.Info;
                            _bor = Style.Db.InfoBorder;
                            break;
                        case TTypeMini.Warn:
                            _back = Style.Db.WarningBg;
                            _fore = Style.Db.Warning;
                            _bor = Style.Db.WarningBorder;
                            break;
                        case TTypeMini.Primary:
                        default:
                            _back = Style.Db.PrimaryBg;
                            _fore = Style.Db.Primary;
                            _bor = Style.Db.Primary;
                            break;
                    }

                    if (Value.Fore.HasValue) _fore = Value.Fore.Value;
                    if (Value.Back.HasValue) _back = Value.Back.Value;

                    using (var brush = new SolidBrush(_back))
                    {
                        g.FillPath(brush, path);
                    }

                    if (Value.BorderWidth > 0)
                    {
                        float border = Value.BorderWidth * Config.Dpi;
                        using (var brush = new Pen(_bor, border))
                        {
                            g.DrawPath(brush, path);
                        }
                    }

                    #endregion

                    using (var brush = new SolidBrush(_fore))
                    {
                        g.DrawString(Value.Text, font, brush, Rect, stringCenter);
                    }
                }
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(Value.Text, font);
                return new Size((int)Math.Ceiling(size.Width) + gap2 * 2, (int)Math.Ceiling(size.Height) + gap);
            }

            Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2, size.Height);
            }

            public CellTag Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public bool SValue(object obj)
            {
                if (obj is CellTag value) { Value = value; return true; }
                return false;
            }
            public override string ToString()
            {
                return Value.Text;
            }
        }

        /// <summary>
        /// Badge 徽标
        /// </summary>
        class TemplateBadge : ITemplate
        {
            public TemplateBadge(CellBadge value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void PaintBack(Graphics g, TCell it) { }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                Color color;
                if (Value.Fill.HasValue) color = Value.Fill.Value;
                else
                {
                    switch (Value.State)
                    {
                        case TState.Success:
                            color = Style.Db.Success; break;
                        case TState.Error:
                            color = Style.Db.Error; break;
                        case TState.Primary:
                        case TState.Processing:
                            color = Style.Db.Primary; break;
                        case TState.Warn:
                            color = Style.Db.Warning; break;
                        default:
                            color = Style.Db.TextQuaternary; break;
                    }
                }
                using (var brush = new SolidBrush(color))
                {
                    if (Value.State == TState.Processing && PARENT.PARENT != null)
                    {
                        float max = (TxtHeight - 6F) * PARENT.PARENT.AnimationStateValue, alpha = 255 * (1F - PARENT.PARENT.AnimationStateValue);
                        using (var pen = new Pen(Helper.ToColor(alpha, brush.Color), 4F))
                        {
                            g.DrawEllipse(pen, new RectangleF(RectDot.X + (RectDot.Width - max) / 2F, RectDot.Y + (RectDot.Height - max) / 2F, max, max));
                        }
                    }
                    g.FillEllipse(brush, RectDot);
                }
                if (Value.Fore.HasValue)
                {
                    using (var brush = new SolidBrush(Value.Fore.Value))
                    {
                        g.DrawString(Value.Text, font, brush, Rect, StringF(PARENT.column));
                    }
                }
                else g.DrawString(Value.Text, font, fore, Rect, StringF(PARENT.column));
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                if (string.IsNullOrEmpty(Value.Text))
                {
                    var size = g.MeasureString(Config.NullText, font);
                    int height = (int)Math.Ceiling(size.Height);
                    return new Size(height + gap2, (int)Math.Ceiling(size.Height));
                }
                else
                {
                    var size = g.MeasureString(Value.Text, font);
                    int height = (int)Math.Ceiling(size.Height);
                    return new Size((int)Math.Ceiling(size.Width) + height + gap2, height);
                }
            }

            int TxtHeight = 0;
            RectangleF Rect;
            RectangleF RectDot;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                TxtHeight = size.Height;
                float dot_size = size.Height / 2.5F;
                if (string.IsNullOrEmpty(Value.Text)) RectDot = new RectangleF(rect.X + (rect.Width - dot_size) / 2F, rect.Y + (rect.Height - dot_size) / 2F, dot_size, dot_size);
                else
                {
                    Rect = new RectangleF(rect.X + gap + size.Height, rect.Y, rect.Width - size.Height - gap2, rect.Height);
                    switch (PARENT.column.Align)
                    {
                        case ColumnAlign.Center:
                            var sizec = g.MeasureString(Value.Text, font);
                            RectDot = new RectangleF(rect.X + (rect.Width - sizec.Width - sizec.Height + gap2) / 2F, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
                            break;
                        case ColumnAlign.Right:
                            var sizer = g.MeasureString(Value.Text, font);
                            RectDot = new RectangleF(Rect.Right - sizer.Width - gap2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
                            break;
                        case ColumnAlign.Left:
                        default:
                            RectDot = new RectangleF(rect.X + gap + (size.Height - dot_size) / 2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
                            break;
                    }
                }
            }

            public CellBadge Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public bool SValue(object obj)
            {
                if (obj is CellBadge value) { Value = value; return true; }
                return false;
            }
            public override string? ToString()
            {
                return Value.Text;
            }
        }

        /// <summary>
        /// Image 图片
        /// </summary>
        class TemplateImage : ITemplate
        {
            public TemplateImage(CellImage value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void PaintBack(Graphics g, TCell it) { }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                float radius = Value.Radius * Config.Dpi;
                using (var path = Rect.RoundPath(radius))
                {
                    using (var bmp = new Bitmap(Rect.Width, Rect.Height))
                    {
                        using (var g2 = Graphics.FromImage(bmp).High())
                        {
                            if (Value.ImageSvg != null)
                            {
                                using (var bmpsvg = Value.ImageSvg.SvgToBmp(Rect.Width, Rect.Height, Value.FillSvg))
                                {
                                    g2.PaintImg(new RectangleF(0, 0, Rect.Width, Rect.Height), bmpsvg, Value.ImageFit);
                                }
                            }
                            else g2.PaintImg(new RectangleF(0, 0, Rect.Width, Rect.Height), Value.Image, Value.ImageFit);
                        }
                        using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                        {
                            brush.TranslateTransform(Rect.X, Rect.Y);
                            if (Value.Round) g.FillEllipse(brush, Rect);
                            else
                            {
                                g.FillPath(brush, path);
                            }
                        }
                    }

                    if (Value.BorderWidth > 0 && Value.BorderColor.HasValue)
                    {
                        float border = Value.BorderWidth * Config.Dpi;
                        using (var brush = new Pen(Value.BorderColor.Value, border))
                        {
                            g.DrawPath(brush, path);
                        }
                    }
                }
            }

            internal Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                int w = size.Width - gap2, h = size.Height - gap2;
                Rect = new Rectangle(rect.X + (rect.Width - w) / 2, rect.Y + (rect.Height - h) / 2, w, h);
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                if (Value.Size.HasValue)
                {
                    return new Size((int)Math.Ceiling(Value.Size.Value.Width * Config.Dpi) + gap2, (int)Math.Ceiling(Value.Size.Value.Height * Config.Dpi) + gap2);
                }
                else
                {
                    int size = gap2 + (int)Math.Round(g.MeasureString(Config.NullText, font).Height);
                    return new Size(size, size);
                }
            }

            public CellImage Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public bool SValue(object obj)
            {
                if (obj is CellImage value) { Value = value; return true; }
                return false;
            }
            public override string? ToString()
            {
                return null;
            }
        }

        /// <summary>
        /// Button 按钮
        /// </summary>
        class TemplateButton : ITemplate
        {
            public TemplateButton(CellLink value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void PaintBack(Graphics g, TCell it) { }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                if (Value is CellButton btn) PaintButton(g, font, PARENT.PARENT == null ? 12 : PARENT.PARENT._gap, Rect, this, btn);
                else PaintLink(g, font, Rect, this, Value);
            }

            internal Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2, size.Height);
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                if (Value is CellButton btn)
                {
                    if (string.IsNullOrEmpty(Value.Text))
                    {
                        var size = g.MeasureString(Config.NullText, font);
                        int sizei = (int)Math.Ceiling(size.Height) + gap;
                        return new Size(sizei + gap2, sizei);
                    }
                    else
                    {
                        var size = g.MeasureString(Value.Text ?? Config.NullText, font);
                        if (btn.HasImage && btn.ShowArrow) return new Size((int)Math.Ceiling(size.Width + size.Height * 2) + gap2 * 3, (int)Math.Ceiling(size.Height) + gap);
                        else if (btn.HasImage || btn.ShowArrow) return new Size((int)Math.Ceiling(size.Width + size.Height + gap) + gap2 * 2, (int)Math.Ceiling(size.Height) + gap);
                        return new Size((int)Math.Ceiling(size.Width) + gap2 * 2, (int)Math.Ceiling(size.Height) + gap);
                    }
                }
                else
                {
                    var size = g.MeasureString(Value.Text ?? Config.NullText, font);
                    return new Size((int)Math.Ceiling(size.Width) + gap2 * 2, (int)Math.Ceiling(size.Height) + gap);
                }
            }

            #region 按钮

            #region 动画

            internal ITask? ThreadHover = null;
            internal ITask? ThreadImageHover = null;

            internal bool _mouseDown = false;
            internal bool ExtraMouseDown
            {
                get => _mouseDown;
                set
                {
                    if (_mouseDown == value) return;
                    _mouseDown = value;
                    PARENT.PARENT?.Invalidate();
                }
            }

            internal int AnimationHoverValue = 0;
            internal bool AnimationHover = false;
            internal bool _mouseHover = false;
            internal virtual bool ExtraMouseHover
            {
                get => _mouseHover;
                set
                {
                    if (_mouseHover == value) return;
                    _mouseHover = value;
                    if (PARENT.PARENT == null) return;
                    var enabled = Value.Enabled;
                    if (enabled && Value is CellButton btn)
                    {
                        Color _back_hover;
                        switch (btn.Type)
                        {
                            case TTypeMini.Default:
                                if (btn.BorderWidth > 0) _back_hover = Style.Db.PrimaryHover;
                                else _back_hover = Style.Db.FillSecondary;
                                break;
                            case TTypeMini.Success:
                                _back_hover = Style.Db.SuccessHover;
                                break;
                            case TTypeMini.Error:
                                _back_hover = Style.Db.ErrorHover;
                                break;
                            case TTypeMini.Info:
                                _back_hover = Style.Db.InfoHover;
                                break;
                            case TTypeMini.Warn:
                                _back_hover = Style.Db.WarningHover;
                                break;
                            case TTypeMini.Primary:
                            default:
                                _back_hover = Style.Db.PrimaryHover;
                                break;
                        }

                        if (btn.BackHover.HasValue) _back_hover = btn.BackHover.Value;
                        if (Config.Animation)
                        {
                            if (btn.ImageHoverAnimation > 0 && btn.HasImage && (btn.ImageHoverSvg != null || btn.ImageHover != null))
                            {
                                ThreadImageHover?.Dispose();
                                AnimationImageHover = true;
                                var t = Animation.TotalFrames(10, btn.ImageHoverAnimation);
                                if (value)
                                {
                                    ThreadImageHover = new ITask((i) =>
                                    {
                                        AnimationImageHoverValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                                        PARENT.PARENT.Invalidate();
                                        return true;
                                    }, 10, t, () =>
                                    {
                                        AnimationImageHoverValue = 1F;
                                        AnimationImageHover = false;
                                        PARENT.PARENT.Invalidate();
                                    });
                                }
                                else
                                {
                                    ThreadImageHover = new ITask((i) =>
                                    {
                                        AnimationImageHoverValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                        PARENT.PARENT.Invalidate();
                                        return true;
                                    }, 10, t, () =>
                                    {
                                        AnimationImageHoverValue = 0F;
                                        AnimationImageHover = false;
                                        PARENT.PARENT.Invalidate();
                                    });
                                }
                            }
                            if (_back_hover.A > 0)
                            {
                                int addvalue = _back_hover.A / 12;
                                ThreadHover?.Dispose();
                                AnimationHover = true;
                                if (value)
                                {
                                    ThreadHover = new ITask(PARENT.PARENT, () =>
                                    {
                                        AnimationHoverValue += addvalue;
                                        if (AnimationHoverValue > _back_hover.A) { AnimationHoverValue = _back_hover.A; return false; }
                                        PARENT.PARENT.Invalidate();
                                        return true;
                                    }, 10, () =>
                                    {
                                        AnimationHover = false;
                                        PARENT.PARENT.Invalidate();
                                    });
                                }
                                else
                                {
                                    ThreadHover = new ITask(PARENT.PARENT, () =>
                                    {
                                        AnimationHoverValue -= addvalue;
                                        if (AnimationHoverValue < 1) { AnimationHoverValue = 0; return false; }
                                        PARENT.PARENT.Invalidate();
                                        return true;
                                    }, 10, () =>
                                    {
                                        AnimationHover = false;
                                        PARENT.PARENT.Invalidate();
                                    });
                                }
                            }
                            else
                            {
                                AnimationHoverValue = _back_hover.A;
                                PARENT.PARENT.Invalidate();
                            }
                        }
                        else AnimationHoverValue = _back_hover.A;
                        PARENT.PARENT.Invalidate();
                    }
                }
            }

            internal bool AnimationImageHover = false;
            internal float AnimationImageHoverValue = 0F;

            #region 点击动画

            ITask? ThreadClick = null;
            internal bool AnimationClick = false;
            internal float AnimationClickValue = 0;
            internal void Click()
            {
                if (_mouseDown && Config.Animation && PARENT.PARENT != null)
                {
                    ThreadClick?.Dispose();
                    AnimationClickValue = 0;
                    AnimationClick = true;
                    ThreadClick = new ITask(PARENT.PARENT, () =>
                    {
                        if (AnimationClickValue > 0.6) AnimationClickValue = AnimationClickValue.Calculate(0.04F);
                        else AnimationClickValue += AnimationClickValue = AnimationClickValue.Calculate(0.1F);
                        if (AnimationClickValue > 1) { AnimationClickValue = 0F; return false; }
                        PARENT.PARENT.Invalidate();
                        return true;
                    }, 50, () =>
                    {
                        AnimationClick = false;
                        PARENT.PARENT.Invalidate();
                    });
                }
            }

            #endregion

            #endregion

            #endregion

            public CellLink Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public bool SValue(object obj)
            {
                if (obj is CellLink value) { Value = value; return true; }
                return false;
            }
            public override string? ToString()
            {
                return Value.Text;
            }
        }

        /// <summary>
        /// Progress 进度条
        /// </summary>
        class TemplateProgress : ITemplate
        {
            public TemplateProgress(CellProgress value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void PaintBack(Graphics g, TCell it) { }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                Color _color = Value.Fill ?? Style.Db.Primary, _back = Value.Back ?? Style.Db.FillSecondary;
                if (Value.Shape == TShape.Circle)
                {
                    float w = Value.Radius * Config.Dpi;
                    using (var brush = new Pen(_back, w))
                    {
                        g.DrawEllipse(brush, Rect);
                    }
                    if (Value.Value > 0)
                    {
                        int max = (int)(360 * Value.Value);
                        using (var brush = new Pen(_color, w))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, Rect, -90, max);
                        }
                    }
                }
                else
                {
                    float radius = Value.Radius * Config.Dpi;
                    if (Value.Shape == TShape.Round) radius = Rect.Height;

                    using (var path = Rect.RoundPath(radius))
                    {
                        using (var brush = new SolidBrush(_back))
                        {
                            g.FillPath(brush, path);
                        }
                        if (Value.Value > 0)
                        {
                            var _w = Rect.Width * Value.Value;
                            if (_w > radius)
                            {
                                using (var path_prog = new RectangleF(Rect.X, Rect.Y, _w, Rect.Height).RoundPath(radius))
                                {
                                    using (var brush = new SolidBrush(_color))
                                    {
                                        g.FillPath(brush, path_prog);
                                    }
                                }
                            }
                            else
                            {
                                using (var bmp = new Bitmap(Rect.Width, Rect.Height))
                                {
                                    using (var g2 = Graphics.FromImage(bmp).High())
                                    {
                                        using (var brush = new SolidBrush(_color))
                                        {
                                            g2.FillEllipse(brush, new RectangleF(0, 0, _w, Rect.Height));
                                        }
                                    }
                                    using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                                    {
                                        brush.TranslateTransform(Rect.X, Rect.Y);
                                        g.FillPath(brush, path);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                int w = rect.Width - gap2, h = size.Height;
                if (Value.Shape == TShape.Circle)
                {
                    w = size.Width - gap2;
                    h = size.Height - gap2;
                }
                Rect = new Rectangle(rect.X + (rect.Width - w) / 2, rect.Y + (rect.Height - h) / 2, w, h);
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                int height = (int)Math.Round(g.MeasureString(Config.NullText, font).Height);
                if (Value.Shape == TShape.Circle)
                {
                    int size = gap2 + height;
                    return new Size(size, size);
                }
                else
                {
                    int size = gap2 + height;
                    return new Size(size, height / 2);
                }
            }

            public CellProgress Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public bool SValue(object obj)
            {
                if (obj is CellProgress value) { Value = value; return true; }
                return false;
            }
            public override string? ToString()
            {
                return Math.Round(Value.Value * 100F) + "%";
            }
        }

        /// <summary>
        /// Divider 分割线
        /// </summary>
        class TemplateDivider : ITemplate
        {
            public TemplateDivider(CellDivider value, Template template)
            {
                Value = value;
                PARENT = template;
            }

            public void PaintBack(Graphics g, TCell it) { }

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                using (var brush = new SolidBrush(Style.Db.Split))
                {
                    g.FillRectangle(brush, Rect);
                }
            }

            Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                int h = size.Height - gap2;
                Rect = new Rectangle(rect.X + (rect.Width - 1) / 2, rect.Y + (rect.Height - h) / 2, 1, h);
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(Config.NullText, font);
                return new Size(gap, (int)Math.Ceiling(size.Height) + gap);
            }

            public CellDivider Value { get; set; }
            public Template PARENT { get; set; }
            public Rectangle RECT { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public bool SValue(object obj)
            {
                if (obj is CellDivider value) { Value = value; return true; }
                return false;
            }
            public override string? ToString()
            {
                return null;
            }
        }

        interface ITemplate
        {
            /// <summary>
            /// 模板父级
            /// </summary>
            Template PARENT { get; set; }

            /// <summary>
            /// 真实区域
            /// </summary>
            Rectangle RECT { get; set; }

            /// <summary>
            /// 获取大小
            /// </summary>
            /// <param name="g">GDI</param>
            /// <param name="font">字体</param>
            /// <param name="gap">边距</param>
            /// <param name="gap2">边距2</param>
            Size GetSize(Graphics g, Font font, int gap, int gap2);

            /// <summary>
            /// 设置渲染位置坐标
            /// </summary>
            /// <param name="g"></param>
            /// <param name="font">字体</param>
            /// <param name="rect">区域</param>
            /// <param name="size">真实区域</param>
            /// <param name="gap">边距</param>
            /// <param name="gap2">边距2</param>
            void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2);

            void PaintBack(Graphics g, TCell it);
            void Paint(Graphics g, TCell it, Font font, SolidBrush fore);

#if NET40 || NET46 || NET48
            bool CONTAINS(int x, int y);
#else
            internal bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            bool SValue(object value);
        }

        #endregion

        #endregion

        internal class StyleRow
        {
            public StyleRow(RowTemplate _row, CellStyleInfo? _style)
            {
                row = _row;
                style = _style;
            }
            public RowTemplate row { get; set; }
            public CellStyleInfo? style { get; set; }
        }
    }
}
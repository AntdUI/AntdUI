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
            /// <summary>
            /// 复选框
            /// </summary>
            /// <param name="table">表格</param>
            /// <param name="column">表头</param>
            /// <param name="prop">反射</param>
            /// <param name="ov">行数据</param>
            /// <param name="value">值</param>
            public TCellCheck(Table table, ColumnCheck column, PropertyDescriptor? prop, object? ov, bool value) : base(table, column, prop, ov)
            {
                _checked = value;
                AnimationCheckValue = _checked ? 1F : 0F;
                NoTitle = column.NoTitle;
                AutoCheck = column.AutoCheck;
            }

            #region 选中状态

            public bool AnimationCheck = false;
            public float AnimationCheckValue = 0;

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

            public override void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2)
            {
            }
            public void SetSize(Rectangle _rect, int check_size)
            {
                RECT = _rect;
                RECT_REAL = new Rectangle(_rect.X + (_rect.Width - check_size) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size, check_size);
            }

            public override SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2)
            {
                var sizef = g.MeasureString(Config.NullText, font);
                MinWidth = (int)Math.Ceiling(sizef.Width);
                return sizef;
            }

            public bool NoTitle { get; set; }
            public bool AutoCheck { get; set; }
            public override string ToString() => Checked.ToString();
        }

        /// <summary>
        /// 单选框
        /// </summary>
        class TCellRadio : TCell
        {
            /// <summary>
            /// 单选框
            /// </summary>
            /// <param name="table">表格</param>
            /// <param name="column">表头</param>
            /// <param name="prop">反射</param>
            /// <param name="ov">行数据</param>
            /// <param name="value">值</param>
            public TCellRadio(Table table, ColumnRadio column, PropertyDescriptor? prop, object? ov, bool value) : base(table, column, prop, ov)
            {
                _checked = value;
                AnimationCheckValue = _checked ? 1F : 0F;
                AutoCheck = column.AutoCheck;
            }

            #region 选中状态

            public bool AnimationCheck = false;
            public float AnimationCheckValue = 0;

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

            public override void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2)
            {
            }
            public void SetSize(Rectangle _rect, int check_size)
            {
                RECT = _rect;
                RECT_REAL = new Rectangle(_rect.X + (_rect.Width - check_size) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size, check_size);
            }

            public override SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2)
            {
                var sizef = g.MeasureString(Config.NullText, font);
                MinWidth = (int)Math.Ceiling(sizef.Width);
                return sizef;
            }

            public bool AutoCheck { get; set; }
            public override string ToString() => Checked.ToString();
        }

        /// <summary>
        /// 开关
        /// </summary>
        class TCellSwitch : TCell
        {
            /// <summary>
            /// 开关
            /// </summary>
            /// <param name="table">表格</param>
            /// <param name="column">表头</param>
            /// <param name="prop">反射</param>
            /// <param name="ov">行数据</param>
            /// <param name="value">值</param>
            public TCellSwitch(Table table, ColumnSwitch column, PropertyDescriptor? prop, object? ov, bool value) : base(table, column, prop, ov)
            {
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

            public override void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2)
            {
            }

            public void SetSize(Rectangle _rect, int check_size)
            {
                int check_size2 = check_size * 2;
                RECT = _rect;
                RECT_REAL = new Rectangle(_rect.X + (_rect.Width - check_size2) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size2, check_size);
            }

            public override SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2)
            {
                var sizef = g.MeasureString(Config.NullText, font);
                MinWidth = (int)Math.Ceiling(sizef.Width);
                return sizef;
            }

            public bool AutoCheck { get; set; }
            public override string ToString() => Checked.ToString();
        }

        /// <summary>
        /// 普通文本
        /// </summary>
        class TCellText : TCell
        {
            /// <summary>
            /// 普通文本
            /// </summary>
            /// <param name="table">表格</param>
            /// <param name="column">表头</param>
            /// <param name="prop">反射</param>
            /// <param name="ov">行数据</param>
            /// <param name="txt">文本</param>
            public TCellText(Table table, Column column, PropertyDescriptor? prop, object? ov, string? txt) : base(table, column, prop, ov)
            {
                value = txt;
            }

            /// <summary>
            /// 值
            /// </summary>
            public string? value { get; set; }

            public override void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2)
            {
                RECT = _rect;
                RECT_REAL = new Rectangle(_rect.X + gap + ox, _rect.Y + gap, _rect.Width - gap2, _rect.Height - gap2);
            }

            public override SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2)
            {
                if (COLUMN.LineBreak)
                {
                    if (COLUMN.Width != null)
                    {
                        if (COLUMN.Width.EndsWith("%") && float.TryParse(COLUMN.Width.TrimEnd('%'), out var f))
                        {
                            var size2 = g.MeasureString(value, font, (int)Math.Ceiling(width * (f / 100F)));
                            MinWidth = (int)Math.Ceiling(size2.Width);
                            return new SizeF(size2.Width + gap2, size2.Height);
                        }
                        else if (int.TryParse(COLUMN.Width, out var i))
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

            public override string? ToString() => value;
        }

        /// <summary>
        /// 表头
        /// </summary>
        internal class TCellColumn : TCell
        {
            public TCellColumn(Table table, Column column) : base(table, column)
            {
                value = column.Title;
            }

            /// <summary>
            /// 值
            /// </summary>
            public string value { get; set; }

            public Rectangle rect_up { get; set; }
            public Rectangle rect_down { get; set; }
            public override void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2)
            {
                RECT = _rect;
                if (COLUMN.SortOrder)
                {
                    int icon_sp = (int)(gap * 0.34F), y = _rect.Y + (_rect.Height - (gap * 2) + icon_sp) / 2;
                    rect_up = new Rectangle(_rect.Right - gap2, y, gap, gap);
                    rect_down = new Rectangle(rect_up.X, rect_up.Bottom - icon_sp, gap, gap);
                }
            }

            public override SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2)
            {
                var size = g.MeasureString(value, font);
                SortWidth = COLUMN.SortOrder ? (int)(size.Height * 0.8F) : 0;
                MinWidth = (int)Math.Ceiling(size.Width) + gap2 + SortWidth;
                return new SizeF(size.Width + gap2 + SortWidth, size.Height);
            }

            public int SortWidth = 0;
            public override string ToString() => value;
        }

        /// <summary>
        /// 单元格
        /// </summary>
        internal abstract class TCell
        {
            public TCell(Table table, Column column)
            {
                COLUMN = column;
                PARENT = table;
            }

            public TCell(Table table, Column column, PropertyDescriptor? prop, object? ov)
            {
                COLUMN = column;
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
            }

            /// <summary>
            /// 表对象
            /// </summary>
            public Table PARENT { get; set; }

            public Column COLUMN { get; set; }
            /// <summary>
            /// 列对象
            /// </summary>
            public int INDEX { get; set; }

            public PropertyDescriptor? PROPERTY { get; set; }
            public object? VALUE { get; set; }

            RowTemplate? _ROW = null;
            /// <summary>
            /// 行对象
            /// </summary>
            public RowTemplate ROW
            {
                get
                {
                    if (_ROW == null) throw new ArgumentNullException();
                    return _ROW;
                }
            }

            public void SetROW(RowTemplate row)
            {
                _ROW = row;
            }

            #region 区域

            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle RECT { get; set; }

            /// <summary>
            /// 显示区域
            /// </summary>
            public Rectangle RECT_REAL { get; set; }

            /// <summary>
            /// 最小宽度
            /// </summary>
            public int MinWidth { get; set; }

            /// <summary>
            /// 鼠标按下
            /// </summary>
            public int MouseDown { get; set; }

            public bool CONTAIN(int x, int y) => RECT.Contains(x, y);
            public bool CONTAIN_REAL(int x, int y) => RECT_REAL.Contains(x, y);

            public abstract void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int gap, int gap2);
            public abstract SizeF GetSize(Graphics g, Font font, int width, int gap, int gap2);

            #endregion
        }

        #region 容器

        /// <summary>
        /// 包裹容器
        /// </summary>
        internal class Template : TCell
        {
            public Template(Table table, Column column, PropertyDescriptor? prop, object? ov, ref int processing, IList<ICell> cels) : base(table, column, prop, ov)
            {
                var list = new List<ITemplate>(cels.Count);
                foreach (var it in cels)
                {
                    if (it is CellBadge badge)
                    {
                        if (badge.State == TState.Processing) processing++;
                    }
                    list.Add(new ITemplate(it, this));
                }
                value = list;
            }

            /// <summary>
            /// 值
            /// </summary>
            internal IList<ITemplate> value { get; set; }

            public override void SetSize(Graphics g, Font font, Rectangle _rect, int ox, int _gap, int _gap2)
            {
                RECT = RECT_REAL = _rect;
                int rx = _rect.X + ox;
                int gap = _gap / 2, gap2 = _gap;
                if (value.Count == 1 && (value[0].Value is CellText || value[0].Value is CellProgress))
                {
                    var it = value[0];
                    var size = SIZES[0];
                    it.RECT = new Rectangle(rx, _rect.Y, _rect.Width, _rect.Height);
                    it.SetRect(g, font, new Rectangle(rx, _rect.Y, _rect.Width, _rect.Height), size, gap, gap2);
                }
                else
                {
                    int use_x;
                    switch (COLUMN.Align)
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

            Size[] SIZES = new Size[0];
            public override SizeF GetSize(Graphics g, Font font, int width, int _gap, int _gap2)
            {
                int gap = _gap / 2, gap2 = _gap;
                float w = 0, h = 0;
                var sizes = new List<Size>(value.Count);
                foreach (var it in value)
                {
                    var size = it.Value.GetSize(g, font, gap, gap2);
                    sizes.Add(size);
                    w += size.Width;
                    if (h < size.Height) h = size.Height;
                }
                MinWidth = (int)Math.Ceiling(w);
                SIZES = sizes.ToArray();
                return new SizeF(MinWidth + _gap2, h);
            }

            public override string? ToString()
            {
                var vals = new List<string>(value.Count);
                foreach (var cell in value)
                {
                    var str = cell.ToString();
                    if (str != null && !string.IsNullOrEmpty(str)) vals.Add(str);
                }
                return string.Join(" ", vals);
            }
        }

        internal class ITemplate
        {
            public ITemplate(ICell value, Template template)
            {
                value.PARENT = template;
                Value = value;
            }

            public ICell Value { get; set; }

            /// <summary>
            /// 真实区域
            /// </summary>
            public Rectangle RECT { get; set; }

            public bool CONTAINS(int x, int y) => RECT.Contains(x, y);

            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                Value.SetRect(g, font, rect, size, gap, gap2);
            }

            public bool SValue(object obj)
            {
                if (obj is ICell value)
                {
                    Value = value;
                    return true;
                }
                return false;
            }

            public override string? ToString() => Value.ToString();
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
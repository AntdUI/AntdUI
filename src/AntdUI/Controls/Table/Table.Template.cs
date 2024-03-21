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
using System.Windows.Forms;

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
            public RowTemplate(Table table, TCell[] cell, object value)
            {
                PARENT = table;
                cells = cell;
                RECORD = value;
            }

            public RowTemplate(Table table, TCell[] cell, object value, bool check)
            {
                PARENT = table;
                cells = cell;
                RECORD = value;
                checkState = CheckState.Checked;
                _checked = check;
            }
            public RowTemplate(Table table, TCell[] cell, object value, CheckState check)
            {
                PARENT = table;
                cells = cell;
                RECORD = value;
                IsColumn = true;
                checkState = check;
                _checked = check == CheckState.Checked;
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
            public object RECORD { get; set; }
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
            ITask? ThreadHover = null;

            #endregion

            #region 选中状态(总)

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
                    if (IsColumn) OnCheck();
                    CheckState = value ? CheckState.Checked : CheckState.Unchecked;
                }
            }

            internal CheckState checkStateOld = CheckState.Unchecked;
            CheckState checkState = CheckState.Unchecked;
            [Description("选中状态"), Category("行为"), DefaultValue(CheckState.Unchecked)]
            public CheckState CheckState
            {
                get => checkState;
                set
                {
                    if (checkState == value) return;
                    checkState = value;
                    bool __checked = value == CheckState.Checked;
                    if (_checked != __checked)
                    {
                        _checked = __checked;
                        OnCheck();
                    }
                    if (value != CheckState.Unchecked)
                    {
                        checkStateOld = value;
                        PARENT.Invalidate();
                    }
                }
            }

            void OnCheck()
            {
                ThreadCheck?.Dispose();
                if (PARENT.IsHandleCreated)
                {
                    if (SHOW && Config.Animation)
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
        }

        #region 单元格

        /// <summary>
        /// 复选框
        /// </summary>
        class TCellCheck : TCell
        {
            public TCellCheck(Table table, PropertyDescriptor prop, object ov, bool value)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                _checked = value;
                AnimationCheckValue = _checked ? 1F : 0F;
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

            public void SetSize(Graphics g, Font font, Rectangle _rect, int gap, int gap2)
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

            public SizeF GetSize(Graphics g, Font font, int gap, int gap2)
            {
                return g.MeasureString(Config.NullText, font);
            }

            public bool MouseDown { get; set; }

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
        }

        /// <summary>
        /// 单选框
        /// </summary>
        class TCellRadio : TCell
        {
            public TCellRadio(Table table, PropertyDescriptor prop, object ov, bool value)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                _checked = value;
                AnimationCheckValue = _checked ? 1F : 0F;
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

            public void SetSize(Graphics g, Font font, Rectangle _rect, int gap, int gap2)
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

            public SizeF GetSize(Graphics g, Font font, int gap, int gap2)
            {
                return g.MeasureString(Config.NullText, font);
            }

            public bool MouseDown { get; set; }

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
        }

        /// <summary>
        /// 普通文本
        /// </summary>
        class TCellText : TCell
        {
            public TCellText(Table table, PropertyDescriptor? prop, object ov, Column column, string? _value)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                align = column.Align;
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
            public void SetSize(Graphics g, Font font, Rectangle _rect, int gap, int gap2)
            {
                RECT = _rect;
                rect = new Rectangle(_rect.X + gap, _rect.Y + gap, _rect.Width - gap2, _rect.Height - gap2);
            }
            public ColumnAlign align { get; set; }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public int INDEX { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }

            public SizeF GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(value, font);
                return new SizeF(size.Width + gap2, size.Height);
            }

            public bool MouseDown { get; set; }

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
            public TCellColumn(Table table, Column column)
            {
                PARENT = table;
                align = column.Align;
                tag = column;
                value = column.Title;
            }

            /// <summary>
            /// 值
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle rect { get; set; }

            public ColumnAlign align { get; set; }
            public void SetSize(Graphics g, Font font, Rectangle _rect, int gap, int gap2)
            {
                RECT = _rect;
            }

            public Column tag { get; set; }
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

            public SizeF GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(value, font);
                return new SizeF(size.Width + gap2, size.Height);
            }

            public bool MouseDown { get; set; }

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
            void SetSize(Graphics g, Font font, Rectangle _rect, int gap, int gap2);
            SizeF GetSize(Graphics g, Font font, int gap, int gap2);

            bool MouseDown { get; set; }

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
            public Template(Table table, PropertyDescriptor prop, object ov, Column column, ref int processing, IList<ICell> _value)
            {
                PARENT = table;
                PROPERTY = prop;
                VALUE = ov;
                align = column.Align;
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
                }
                value = list;
            }
            /// <summary>
            /// 值
            /// </summary>
            public IList<ITemplate> value { get; set; }

            internal bool HasBtn = false;

            public void SetSize(Graphics g, Font font, Rectangle _rect, int _gap, int _gap2)
            {
                RECT = _rect;
                int gap = _gap / 2, gap2 = _gap;
                if (value.Count == 1 && value[0] is TemplateText)
                {
                    var it = value[0];
                    var size = SIZES[0];
                    it.SetRect(g, font, new Rectangle(_rect.X, _rect.Y, _rect.Width, _rect.Height), size, gap, gap2);
                }
                else
                {
                    int use_x;
                    switch (align)
                    {
                        case ColumnAlign.Center: use_x = _rect.X + (_rect.Width - USE_Width) / 2; break;
                        case ColumnAlign.Right: use_x = _rect.Right - USE_Width; break;
                        case ColumnAlign.Left:
                        default: use_x = _rect.X + gap2; break;
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
            public ColumnAlign align { get; set; }
            public Table PARENT { get; set; }
            public RowTemplate ROW { get; set; }
            public PropertyDescriptor? PROPERTY { get; set; }
            public int INDEX { get; set; }
            public object VALUE { get; set; }
            public Rectangle RECT { get; set; }

            Size[] SIZES;
            int USE_Width = 0;
            public SizeF GetSize(Graphics g, Font font, int _gap, int _gap2)
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
                USE_Width = (int)Math.Ceiling(w);
                SIZES = sizes.ToArray();
                return new SizeF(USE_Width + _gap2, h);
            }
            public bool MouseDown { get; set; }

#if NET40 || NET46 || NET48
            public bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
            public override string? ToString()
            {
                var vals = new List<string?>(value.Count);
                foreach (var cell in value)
                {
                    vals.Add(cell.ToString());
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

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                if (Value.Fore.HasValue)
                {
                    using (var brush = new SolidBrush(Value.Fore.Value))
                    {
                        g.DrawString(Value.Text, Value.Font == null ? font : Value.Font, brush, Rect, StringF(PARENT.align));
                    }
                }
                else g.DrawString(Value.Text, Value.Font == null ? font : Value.Font, fore, Rect, StringF(PARENT.align));
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(Value.Text, Value.Font == null ? font : Value.Font);
                return new Size((int)Math.Ceiling(size.Width) + gap2, (int)Math.Ceiling(size.Height));
            }

            Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2, size.Height);
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
                return new Size((int)Math.Ceiling(size.Width) + gap2, (int)Math.Ceiling(size.Height));
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
                        float max = (TxtHeight - 6F) * PARENT.PARENT.AnimationStateValue;
                        int alpha = (int)(255 * (1f - PARENT.PARENT.AnimationStateValue));
                        using (var pen = new Pen(Color.FromArgb(alpha, brush.Color), 4F))
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
                        g.DrawString(Value.Text, font, brush, Rect, StringF(PARENT.align));
                    }
                }
                else g.DrawString(Value.Text, font, fore, Rect, StringF(PARENT.align));
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                if (Value.Text == null)
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
                if (Value.Text == null) RectDot = new RectangleF(rect.X + (rect.Width - dot_size) / 2F, rect.Y + (rect.Height - dot_size) / 2F, dot_size, dot_size);
                else
                {
                    Rect = new RectangleF(rect.X + gap + size.Height, rect.Y, rect.Width - size.Height - gap2, rect.Height);
                    switch (PARENT.align)
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

            Rectangle Rect;
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

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                if (Value is CellButton btn) PaintButton(g, font, PARENT.PARENT == null ? 12 : PARENT.PARENT._gap, Rect, this, btn);
                else PaintLink(g, font, Rect, this, Value);
            }

            Rectangle Rect;
            public void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
            {
                RECT = rect;
                Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2, size.Height);
            }

            public Size GetSize(Graphics g, Font font, int gap, int gap2)
            {
                var size = g.MeasureString(Value.Text, font);
                if (Value is CellButton btn && btn.ShowArrow) return new Size((int)Math.Ceiling(size.Width + size.Height) + gap2 * 2, (int)Math.Ceiling(size.Height) + gap);
                else return new Size((int)Math.Ceiling(size.Width) + gap2 * 2, (int)Math.Ceiling(size.Height) + gap);
            }

            #region 按钮

            #region 动画

            internal ITask? ThreadHover = null;

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
                    if (enabled)
                    {
                        if (Value is CellButton btn)
                        {
                            Color _back_hover;
                            switch (btn.Type)
                            {
                                case TTypeMini.Error:
                                    _back_hover = Style.Db.ErrorHover;
                                    break;
                                case TTypeMini.Success:
                                    _back_hover = Style.Db.SuccessHover;
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

                            if (btn.Type == TTypeMini.Default)
                            {
                                if (btn.BorderWidth > 0) _back_hover = Style.Db.PrimaryHover;
                                else _back_hover = Style.Db.FillSecondary;
                            }

                            if (btn.BackHover.HasValue) _back_hover = btn.BackHover.Value;
                            if (Config.Animation)
                            {
                                ThreadHover?.Dispose();
                                AnimationHover = true;
                                int addvalue = _back_hover.A / 12;
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
                            else AnimationHoverValue = _back_hover.A;
                            PARENT.PARENT.Invalidate();
                        }
                        else
                        {
                            int a = Style.Db.PrimaryHover.A;
                            if (Config.Animation)
                            {
                                ThreadHover?.Dispose();
                                AnimationHover = true;
                                int addvalue = a / 12;
                                if (value)
                                {
                                    ThreadHover = new ITask(PARENT.PARENT, () =>
                                    {
                                        AnimationHoverValue += addvalue;
                                        if (AnimationHoverValue > a) { AnimationHoverValue = a; return false; }
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
                            else AnimationHoverValue = a;
                            PARENT.PARENT.Invalidate();
                        }
                    }
                }
            }

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

            public void Paint(Graphics g, TCell it, Font font, SolidBrush fore)
            {
                Color _color = Value.Fill.HasValue ? Value.Fill.Value : Style.Db.Primary, _back = Value.Back.HasValue ? Value.Back.Value : Style.Db.FillSecondary;
                if (Value.Shape == TShape.Circle)
                {
                    float w = Value.Radius * Config.Dpi;
                    using (var brush = new Pen(_back, w))
                    {
                        g.DrawEllipse(brush, Rect);
                    }
                    if (Value.Value > 0)
                    {
                        float max = 360F * Value.Value;
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
                                using (var brush = new SolidBrush(_color))
                                {
                                    g.FillEllipse(brush, new RectangleF(Rect.X, Rect.Y, _w, Rect.Height));
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
                int w = size.Width - gap2, h = size.Height;
                if (Value.Shape == TShape.Circle) h = size.Height - gap2;
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
                    return new Size(size * 2, height / 2);
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
            public override string? ToString()
            {
                return Math.Round(Value.Value * 100F) + "%";
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

            void Paint(Graphics g, TCell it, Font font, SolidBrush fore);

#if NET40 || NET46 || NET48
            bool CONTAINS(int x, int y);
#else
            internal bool CONTAINS(int x, int y)
            {
                return RECT.Contains(x, y);
            }
#endif
        }

        #endregion

        #endregion
    }
}
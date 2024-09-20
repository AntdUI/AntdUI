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
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Input 输入框
    /// </summary>
    /// <remarks>通过鼠标或键盘输入内容，是最基础的表单域的包装。</remarks>
    [Description("Input 输入框")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    [DefaultEvent("TextChanged")]
    public partial class Input : IControl
    {
        public Input()
        {
            base.BackColor = Color.Transparent;
            SetStyle(ControlStyles.Selectable, true);
            UpdateStyles();
            CurrentCaret.Width = (int)(1 * Config.Dpi);
        }

        #region 属性

        internal Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
            }
        }

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? BackColor
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        string? backExtend = null;
        /// <summary>
        /// 背景渐变色
        /// </summary>
        [Description("背景渐变色"), Category("外观"), DefaultValue(null)]
        public string? BackExtend
        {
            get => backExtend;
            set
            {
                if (backExtend == value) return;
                backExtend = value;
                Invalidate();
            }
        }

        Image? backImage = null;
        /// <summary>
        /// 背景图片
        /// </summary>
        [Description("背景图片"), Category("外观"), DefaultValue(null)]
        public new Image? BackgroundImage
        {
            get => backImage;
            set
            {
                if (backImage == value) return;
                backImage = value;
                Invalidate();
            }
        }

        TFit backFit = TFit.Fill;
        /// <summary>
        /// 背景图片布局
        /// </summary>
        [Description("背景图片布局"), Category("外观"), DefaultValue(TFit.Fill)]
        public new TFit BackgroundImageLayout
        {
            get => backFit;
            set
            {
                if (backFit == value) return;
                backFit = value;
                Invalidate();
            }
        }

        #endregion

        Color selection = Color.FromArgb(102, 0, 127, 255);
        /// <summary>
        /// 选中颜色
        /// </summary>
        [Description("选中颜色"), Category("外观"), DefaultValue(typeof(Color), "102, 0, 127, 255")]
        public Color SelectionColor
        {
            get => selection;
            set
            {
                if (selection == value) return;
                selection = value;
                Invalidate();
            }
        }

        #region 光标

        /// <summary>
        /// 光标颜色
        /// </summary>
        [Description("光标颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? CaretColor { get; set; }

        /// <summary>
        /// 光标速度
        /// </summary>
        [Description("光标速度"), Category("外观"), DefaultValue(1000)]
        public int CaretSpeed { get; set; } = 1000;

        #endregion

        #region 边框

        internal float borderWidth = 1F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(1F)]
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        internal Color? borderColor;
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 悬停边框颜色
        /// </summary>
        [Description("悬停边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderHover { get; set; }

        /// <summary>
        /// 激活边框颜色
        /// </summary>
        [Description("激活边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderActive { get; set; }

        #endregion

        /// <summary>
        /// 波浪大小
        /// </summary>
        [Description("波浪大小"), Category("外观"), DefaultValue(4)]
        public int WaveSize { get; set; } = 4;

        internal int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
            }
        }

        internal bool round = false;
        /// <summary>
        /// 圆角样式
        /// </summary>
        [Description("圆角样式"), Category("外观"), DefaultValue(false)]
        public bool Round
        {
            get => round;
            set
            {
                if (round == value) return;
                round = value;
                CalculateRect();
                Invalidate();
            }
        }

        TType status = TType.None;
        /// <summary>
        /// 设置校验状态
        /// </summary>
        [Description("设置校验状态"), Category("外观"), DefaultValue(TType.None)]
        public TType Status
        {
            get => status;
            set
            {
                if (status == value) return;
                status = value;
                Invalidate();
            }
        }

        #region 图标

        float iconratio = .7F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(.7F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                CalculateRect();
                Invalidate();
            }
        }

        Image? prefix = null;
        /// <summary>
        /// 前缀
        /// </summary>
        [Description("前缀"), Category("外观"), DefaultValue(null)]
        public Image? Prefix
        {
            get => prefix;
            set
            {
                if (prefix == value) return;
                prefix = value;
                CalculateRect();
                Invalidate();
            }
        }

        string? prefixSvg = null;
        /// <summary>
        /// 前缀SVG
        /// </summary>
        [Description("前缀SVG"), Category("外观"), DefaultValue(null)]
        public string? PrefixSvg
        {
            get => prefixSvg;
            set
            {
                if (prefixSvg == value) return;
                prefixSvg = value;
                CalculateRect();
                Invalidate();
            }
        }

        /// <summary>
        /// 是否包含前缀
        /// </summary>
        public bool HasPrefix => prefixSvg != null || prefix != null;

        Image? suffix = null;
        /// <summary>
        /// 后缀
        /// </summary>
        [Description("后缀"), Category("外观"), DefaultValue(null)]
        public Image? Suffix
        {
            get => suffix;
            set
            {
                if (suffix == value) return;
                suffix = value;
                CalculateRect();
                Invalidate();
            }
        }

        string? suffixSvg = null;
        /// <summary>
        /// 后缀SVG
        /// </summary>
        [Description("后缀SVG"), Category("外观"), DefaultValue(null)]
        public string? SuffixSvg
        {
            get => suffixSvg;
            set
            {
                if (suffixSvg == value) return;
                suffixSvg = value;
                CalculateRect();
                Invalidate();
            }
        }

        /// <summary>
        /// 是否包含后缀
        /// </summary>
        public virtual bool HasSuffix => suffixSvg != null || suffix != null;

        string? prefixText = null;
        [Description("前缀文本"), Category("外观"), DefaultValue(null)]
        public string? PrefixText
        {
            get => prefixText;
            set
            {
                if (prefixText == value) return;
                prefixText = value;
                CalculateRect();
                Invalidate();
            }
        }

        string? suffixText = null;
        [Description("后缀文本"), Category("外观"), DefaultValue(null)]
        public string? SuffixText
        {
            get => suffixText;
            set
            {
                if (suffixText == value) return;
                suffixText = value;
                CalculateRect();
                Invalidate();
            }
        }

        #endregion

        /// <summary>
        /// 连接左边
        /// </summary>
        [Description("连接左边"), Category("外观"), DefaultValue(false)]
        public bool JoinLeft { get; set; } = false;

        /// <summary>
        /// 连接右边
        /// </summary>
        [Description("连接右边"), Category("外观"), DefaultValue(false)]
        public bool JoinRight { get; set; } = false;

        bool allowclear = false, is_clear = false, is_clear_down = false;
        bool is_prefix_down = false, is_suffix_down = false;
        /// <summary>
        /// 支持清除
        /// </summary>
        [Description("支持清除"), Category("行为"), DefaultValue(false)]
        public virtual bool AllowClear
        {
            get => allowclear;
            set
            {
                if (allowclear == value) return;
                allowclear = value;
                OnAllowClear();
            }
        }

        void OnAllowClear()
        {
            bool _is_clear = !ReadOnly && allowclear && _mouseHover && (!isempty || HasValue);
            if (is_clear == _is_clear) return;
            is_clear = _is_clear;
            CalculateRect();
        }

        bool autoscroll = false;
        [Description("是否显示滚动条"), Category("外观"), DefaultValue(false)]
        public bool AutoScroll
        {
            get => autoscroll;
            set
            {
                if (autoscroll == value) return;
                autoscroll = value;
                Invalidate();
            }
        }

        #endregion

        #region 原生属性

        #region 文本

        internal bool isempty = true;
        string _text = "";
        [Description("文本"), Category("外观"), DefaultValue("")]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        public override string Text
        {
            get => _text;
            set
            {
                if (value is null) value = "";
                if (_text == value) return;
                _text = value;
                isempty = string.IsNullOrEmpty(_text);
                FixFontWidth();
                OnAllowClear();
                if (isempty)
                {
                    if (selectionStart > 0) SelectionStart = 0;
                }
                else if (cache_font != null)
                {
                    if (selectionStart > cache_font.Length) SelectionStart = cache_font.Length;
                }
                Invalidate();
                OnTextChanged(EventArgs.Empty);
            }
        }

        #endregion

        [Description("Emoji字体"), Category("外观"), DefaultValue("Segoe UI Emoji")]
        public string EmojiFont { get; set; } = "Segoe UI Emoji";

        #region 原生文本框

        int selectionStart = 0, selectionStartTemp = 0, selectionLength = 0;
        /// <summary>
        /// 所选文本的起点
        /// </summary>
        [Browsable(false), DefaultValue(0)]
        public int SelectionStart
        {
            get => selectionStart;
            set
            {
                if (value < 0) value = 0;
                else if (value > 0)
                {
                    if (cache_font == null) value = 0;
                    else if (value > cache_font.Length) value = cache_font.Length;
                }
                if (selectionStart == value) return;
                selectionStart = selectionStartTemp = value;
                SetCaretPostion(value);
            }
        }

        /// <summary>
        /// 所选文本的长度
        /// </summary>
        [Browsable(false), DefaultValue(0)]
        public int SelectionLength
        {
            get => selectionLength;
            set
            {
                if (selectionLength == value) return;
                selectionLength = value;
                Invalidate();
            }
        }

        #endregion

        /// <summary>
        /// 多行编辑是否允许输入制表符
        /// </summary>
        [Description("多行编辑是否允许输入制表符"), Category("行为"), DefaultValue(false)]
        public bool AcceptsTab { get; set; } = false;

        /// <summary>
        /// 焦点离开清空选中
        /// </summary>
        [Description("焦点离开清空选中"), Category("行为"), DefaultValue(true)]
        public bool LostFocusClearSelection { get; set; } = true;

        /// <summary>
        /// 只读
        /// </summary>
        [Description("只读"), Category("行为"), DefaultValue(false)]
        public bool ReadOnly { get; set; }

        bool multiline = false;
        /// <summary>
        /// 多行文本
        /// </summary>
        [Description("多行文本"), Category("行为"), DefaultValue(false)]
        public bool Multiline
        {
            get => multiline;
            set
            {
                if (multiline == value) return;
                multiline = value;
                if (multiline)
                {
                    sf_placeholder.FormatFlags &= ~StringFormatFlags.NoWrap;
                    sf_placeholder.LineAlignment = StringAlignment.Near;
                }
                else
                {
                    sf_placeholder.FormatFlags |= StringFormatFlags.NoWrap;
                    sf_placeholder.LineAlignment = StringAlignment.Center;
                }
                CalculateRect();
                Invalidate();
            }
        }

        int lineheight = 0;
        [Description("多行行高"), Category("行为"), DefaultValue(0)]
        public int LineHeight
        {
            get => lineheight;
            set
            {
                if (lineheight == value) return;
                lineheight = value;
                CalculateRect();
                Invalidate();
            }
        }

        HorizontalAlignment textalign = HorizontalAlignment.Left;
        /// <summary>
        /// 文本对齐方向
        /// </summary>
        [Description("文本对齐方向"), Category("外观"), DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get => textalign;
            set
            {
                if (textalign == value) return;
                textalign = value;
                textalign.SetAlignment(ref sf_placeholder);
                CalculateRect();
                Invalidate();
            }
        }

        #region 水印

        string? placeholderText = null;
        /// <summary>
        /// 水印文本
        /// </summary>
        [Description("水印文本"), Category("行为"), DefaultValue(null)]
        public virtual string? PlaceholderText
        {
            get => placeholderText;
            set
            {
                if (placeholderText == value) return;
                placeholderText = value;
                if (isempty && ShowPlaceholder) Invalidate();
            }
        }

        Color? placeholderColor = null;
        /// <summary>
        /// 水印颜色
        /// </summary>
        [Description("水印颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? PlaceholderColor
        {
            get => placeholderColor;
            set
            {
                if (placeholderColor == value) return;
                placeholderColor = value;
                if (isempty && ShowPlaceholder) Invalidate();
            }
        }

        string? placeholderColorExtend = null;
        /// <summary>
        /// 水印渐变色
        /// </summary>
        [Description("水印渐变色"), Category("外观"), DefaultValue(null)]
        public string? PlaceholderColorExtend
        {
            get => placeholderColorExtend;
            set
            {
                if (placeholderColorExtend == value) return;
                placeholderColorExtend = value;
                if (isempty && ShowPlaceholder) Invalidate();
            }
        }

        #endregion

        /// <summary>
        /// 文本最大长度
        /// </summary>
        [Description("文本最大长度"), Category("行为"), DefaultValue(32767)]
        public int MaxLength { get; set; } = 32767;

        #region 密码框

        bool IsPassWord = false;
        string PassWordChar = "●";

        bool useSystemPasswordChar = false;
        /// <summary>
        /// 使用密码框
        /// </summary>
        [Description("使用密码框"), Category("行为"), DefaultValue(false)]
        public bool UseSystemPasswordChar
        {
            get => useSystemPasswordChar;
            set
            {
                if (useSystemPasswordChar == value) return;
                useSystemPasswordChar = value;
                SetPassWord();
            }
        }

        char passwordChar = '\0';
        /// <summary>
        /// 自定义密码字符
        /// </summary>
        [Description("自定义密码字符"), Category("行为"), DefaultValue((char)0)]
        public char PasswordChar
        {
            get => passwordChar;
            set
            {
                if (passwordChar == value) return;
                passwordChar = value;
                SetPassWord();
            }
        }

        /// <summary>
        /// 密码可以复制
        /// </summary>
        [Description("密码可以复制"), Category("行为"), DefaultValue(false)]
        public bool PasswordCopy { get; set; }

        /// <summary>
        /// 密码可以粘贴
        /// </summary>
        [Description("密码可以粘贴"), Category("行为"), DefaultValue(true)]
        public bool PasswordPaste { get; set; } = true;

        void SetPassWord()
        {
            if (passwordChar != '\0')
            {
                PassWordChar = passwordChar.ToString();
                IsPassWord = true;
            }
            else if (useSystemPasswordChar)
            {
                PassWordChar = "●";
                IsPassWord = true;
            }
            else IsPassWord = false;
            FixFontWidth(true);
            Invalidate();
        }

        #endregion

        #endregion

        #region 方法

        /// <summary>
        /// 将文本追加到当前文本中
        /// </summary>
        /// <param name="text">追加的文本</param>
        public void AppendText(string text)
        {
            var tmp = _text + text;
            Text = tmp;
            CurrentPosIndex = tmp.Length;
        }

        /// <summary>
        /// 清除所有文本
        /// </summary>
        public void Clear()
        {
            Text = "";
        }

        /// <summary>
        /// 清除撤消缓冲区信息
        /// </summary>
        public void ClearUndo()
        {
            history_Log.Clear();
        }

        /// <summary>
        /// 复制
        /// </summary>
        public void Copy()
        {
            if (IsPassWord && !PasswordCopy) return;
            var text = GetSelectionText();
            if (string.IsNullOrEmpty(text)) return;
            this.ClipboardSetText(text);
        }

        /// <summary>
        /// 剪贴
        /// </summary>
        public void Cut()
        {
            if (IsPassWord && !PasswordCopy) return;
            var text = GetSelectionText();
            if (string.IsNullOrEmpty(text)) return;
            this.ClipboardSetText(text);
            ProcessBackSpaceKey();
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        public void Paste()
        {
            if (IsPassWord && !PasswordPaste) return;
            var strText = this.ClipboardGetText();
            if (strText == null || string.IsNullOrEmpty(strText)) return;
            var chars = new List<string>(strText.Length);
            foreach (char key in strText)
            {
                if (Verify(key, out var change)) chars.Add(change ?? key.ToString());
            }
            if (chars.Count > 0) EnterText(string.Join("", chars), false);
        }

        /// <summary>
        /// 取消全部选中
        /// </summary>
        public void DeselectAll()
        {
            SelectionLength = 0;
        }

        /// <summary>
        /// 撤消
        /// </summary>
        public void Undo()
        {
            if (IsPassWord && !PasswordCopy) return;
            if (history_Log.Count > 0)
            {
                int index;
                if (history_I == -1)
                {
                    index = history_Log.Count - 1;
                    AddHistoryRecord();
                }
                else index = history_I - 1;
                if (index > -1)
                {
                    var it = history_Log[index];
                    history_I = index;
                    Text = it.Text;
                    SelectionStart = it.SelectionStart;
                    SelectionLength = it.SelectionLength;
                }
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        public void Redo()
        {
            if (IsPassWord && !PasswordCopy) return;
            if (history_Log.Count > 0 && history_I > -1)
            {
                int index = history_I + 1;
                if (history_Log.Count > index)
                {
                    var it = history_Log[index];
                    history_I = index;
                    Text = it.Text;
                    SelectionStart = it.SelectionStart;
                    SelectionLength = it.SelectionLength;
                }
            }
        }

        /// <summary>
        /// 文本选择范围
        /// </summary>
        /// <param name="start">第一个字符的位置</param>
        /// <param name="length">字符长度</param>
        public void Select(int start, int length)
        {
            SelectionStart = start;
            SelectionLength = length;
        }

        /// <summary>
        /// 选择所有文本
        /// </summary>
        public void SelectAll()
        {
            if (cache_font != null)
            {
                SelectionStart = 0;
                SelectionLength = cache_font.Length;
            }
        }

        void EnterText(string text, bool ismax = true)
        {
            if (ReadOnly || BanInput) return;
            AddHistoryRecord();
            int len = 0;
            GraphemeSplitter.Each(text, 0, (str, nStart, nLen) =>
            {
                len++;
                return true;
            });
            if (cache_font == null)
            {
                if (ismax && text.Length > MaxLength) text = text.Substring(0, MaxLength);
                Text = text;
                SelectionStart = len;
            }
            else
            {
                if (selectionLength > 0)
                {
                    int start = selectionStartTemp, end = selectionLength;
                    AddHistoryRecord();
                    int end_temp = start + end;
                    var texts = new List<string>(end);
                    foreach (var it in cache_font)
                    {
                        if (it.i < start || it.i >= end_temp)
                            texts.Add(it.text);
                    }
                    texts.Insert(start, text);
                    var tmp = string.Join("", texts);
                    if (ismax && tmp.Length > MaxLength) tmp = tmp.Substring(0, MaxLength);
                    Text = tmp;
                    SelectionLength = 0;
                    SelectionStart = start + len;
                }
                else
                {
                    int start = selectionStart - 1;
                    var texts = new List<string>(cache_font.Length);
                    foreach (var it in cache_font) texts.Add(it.text);
                    texts.Insert(start + 1, text);
                    var tmp = string.Join("", texts);
                    if (ismax && tmp.Length > MaxLength) tmp = tmp.Substring(0, MaxLength);
                    Text = tmp;
                    SelectionStart = start + 1 + len;
                }
            }
        }

        string? GetSelectionText()
        {
            if (cache_font == null) return null;
            else
            {
                if (selectionLength > 0)
                {
                    int start = selectionStartTemp, end = selectionLength;
                    int end_temp = start + end;
                    if (end_temp > cache_font.Length) end_temp = cache_font.Length;
                    var texts = new List<string>(end);
                    foreach (var it in cache_font)
                    {
                        if (it.i >= start && end_temp > it.i) texts.Add(it.text);
                    }
                    return string.Join("", texts);
                }
                return null;
            }
        }

        /// <summary>
        /// 内容滚动到当前插入符号位置
        /// </summary>
        public void ScrollToCaret()
        {
            if (cache_font != null)
            {
                Rectangle r;
                if (CurrentPosIndex >= cache_font.Length) r = cache_font[cache_font.Length - 1].rect;
                else r = cache_font[CurrentPosIndex].rect;
                ScrollY = r.Bottom;
            }
        }

        /// <summary>
        /// 内容滚动到最下面
        /// </summary>
        public void ScrollToEnd()
        {
            ScrollY = ScrollYMax;
        }

        #endregion

        #region 重写

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            FixFontWidth(true);
        }

        #region 焦点

        bool AnimationFocus = false;
        int AnimationFocusValue = 0;
        DateTime HasFocusTime;
        public bool HasFocus { get; internal set; }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            HasFocusTime = DateTime.Now;
            HasFocus = true;
            ShowCaret = true;
            ExtraMouseDown = true;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            HasFocus = false;
            ShowCaret = false;
            if (LostFocusClearSelection) SelectionLength = 0;
            ExtraMouseDown = false;
        }

        #endregion

        #region 系统消息

        IntPtr m_hIMC;
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_IME_STARTCOMPOSITION:
                    m_hIMC = Win32.ImmGetContext(Handle);
                    OnImeStartPrivate(m_hIMC);
                    break;
                case Win32.WM_IME_ENDCOMPOSITION:
                    OnImeEndPrivate(m_hIMC);
                    break;
                case Win32.WM_IME_COMPOSITION:
                    if (((int)m.LParam & Win32.GCS_RESULTSTR) == Win32.GCS_RESULTSTR)
                    {
                        m.Result = (IntPtr)1;
                        OnImeResultStrPrivate(m_hIMC, Win32.ImmGetCompositionString(m_hIMC, Win32.GCS_RESULTSTR));
                        return;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        #endregion

        #region 虚拟

        /// <summary>
        /// 禁止输入
        /// </summary>
        protected virtual bool BanInput => false;
        protected virtual bool HasValue => false;
        protected virtual bool ModeRange => false;
        protected virtual void ModeRangeCaretPostion(bool Null) { }

        /// <summary>
        /// 是否显示水印
        /// </summary>
        protected virtual bool ShowPlaceholder => true;
        protected virtual bool HasLeft() => false;
        protected virtual int UseLeft(Rectangle rect, bool delgap) => 0;

        protected virtual void IBackSpaceKey() { }

        protected virtual bool IMouseDown(Point e) => false;

        protected virtual bool IMouseMove(Point e) => false;

        /// <summary>
        /// 清空值
        /// </summary>
        protected virtual void OnClearValue() => Text = "";

        /// <summary>
        /// 焦点点击
        /// </summary>
        /// <param name="SetFocus">是否已经设置过焦点</param>
        protected virtual void OnFocusClick(bool SetFocus) { }

        /// <summary>
        /// 改变鼠标状态
        /// </summary>
        /// <param name="Hover">移入</param>
        /// <param name="Focus">焦点</param>
        protected virtual void ChangeMouseHover(bool Hover, bool Focus) { }

        #endregion

        #endregion

        #region 光标

        internal bool ReadShowCaret = false;
        bool showCaret = false;
        bool showCaretFlag = false;
        internal bool ShowCaret
        {
            get => showCaret;
            set
            {
                if (showCaret == value) return;
                showCaret = value;
                CaretPrint?.Dispose();
                if (IsHandleCreated)
                {
                    showCaretFlag = true;
                    if (showCaret)
                    {
                        if (ReadShowCaret)
                        {
                            showCaret = false;
                            return;
                        }
                        CaretPrint = new ITask(this, () =>
                        {
                            showCaretFlag = !showCaretFlag;
                            Invalidate();
                            return showCaret;
                        }, CaretSpeed);
                        SetCaretPostion();
                    }
                    else
                    {
                        CaretPrint = null;
                        Invalidate();
                    }
                }
            }
        }

        ITask? CaretPrint;

        internal Rectangle CurrentCaret = new Rectangle(-1, -1000, 1, 0);

        internal void SetCaretX(int x)
        {
            if (CurrentCaret.X == x && showCaretFlag) return;
            CurrentCaret.X = x;
            showCaretFlag = true;
            Invalidate();
        }
        internal void SetCaretY(int y)
        {
            if (CurrentCaret.Y == y && showCaretFlag) return;
            CurrentCaret.Y = y;
            showCaretFlag = true;
            Invalidate();
        }
        internal void SetCaretXY(int x, int y)
        {
            if (CurrentCaret.X == x && CurrentCaret.Y == y && showCaretFlag) return;
            CurrentCaret.X = x;
            CurrentCaret.Y = y;
            showCaretFlag = true;
            Invalidate();
        }

        #region 得到光标位置

        /// <summary>
        /// 通过坐标系查找光标位置
        /// </summary>
        int GetCaretPostion(int x, int y)
        {
            if (cache_font == null) return 0;
            else
            {
                foreach (var it in cache_font)
                {
                    if (it.rect.X <= x && it.rect.Right >= x && it.rect.Y <= y && it.rect.Bottom >= y)
                    {
                        if (x > it.rect.X + it.rect.Width / 2) return it.i + 1;
                        else return it.i;
                    }
                }
                var nearest = FindNearestFont(x, y, cache_font, out bool isold);
                if (nearest == null)
                {
                    if (x > cache_font[cache_font.Length - 1].rect.Right) return cache_font.Length;
                    else return 0;
                }
                else
                {
                    if (isold) return nearest.i;
                    else if (x > nearest.rect.X + nearest.rect.Width / 2) return nearest.i + 1;
                    else return nearest.i;
                }
            }
        }

        /// <summary>
        /// 寻找最近的矩形和距离的辅助方法
        /// </summary>
        CacheFont? FindNearestFont(int x, int y, CacheFont[] cache_font, out bool isold)
        {
            var findy = FindNearestFontY(y, cache_font, out isold);
            CacheFont? result = null;
            if (findy == null)
            {
                double minDistance = int.MaxValue;
                for (int i = 0; i < cache_font.Length; i++)
                {
                    var it = cache_font[i];
                    // 计算点到矩形四个边的最近距离，取最小值作为当前矩形的最近距离
                    int distanceToLeft = Math.Abs(x - (it.rect.X + it.rect.Width / 2)),
                        distanceToTop = Math.Abs(y - (it.rect.Y + it.rect.Height / 2));
                    double currentMinDistance = new int[] { distanceToLeft, distanceToTop }.Average();

                    // 如果当前矩形的最近距离比之前找到的最近距离小，更新最近距离和最近矩形信息
                    if (currentMinDistance < minDistance)
                    {
                        minDistance = currentMinDistance;
                        result = it;
                    }
                }
            }
            else
            {
                int ry = isold ? findy.rect_old.Y : findy.rect.Y;
                int minDistance = int.MaxValue;
                for (int i = 0; i < cache_font.Length; i++)
                {
                    var it = cache_font[i];
                    if (it.rect.Y == ry || (it.retun == 1 && it.rect_old.Y == ry))
                    {
                        // 计算点到矩形四个边的最近距离，取最小值作为当前矩形的最近距离
                        int currentMinDistance = Math.Abs(x - (it.rect.X + it.rect.Width / 2));
                        // 如果当前矩形的最近距离比之前找到的最近距离小，更新最近距离和最近矩形信息
                        if (currentMinDistance < minDistance)
                        {
                            minDistance = currentMinDistance;
                            result = it;
                        }
                    }
                }
            }
            if (result == null) return result;
            return result;
        }
        CacheFont? FindNearestFontY(int y, CacheFont[] cache_font, out bool old)
        {
            int minDistance = int.MaxValue;
            CacheFont? result = null;
            old = false;
            for (int i = 0; i < cache_font.Length; i++)
            {
                var it = cache_font[i];
                // 计算点到矩形四个边的最近距离，取最小值作为当前矩形的最近距离
                int currentMinDistance = Math.Abs(y - (it.rect.Y + it.rect.Height / 2));
                // 如果当前矩形的最近距离比之前找到的最近距离小，更新最近距离和最近矩形信息
                if (currentMinDistance < minDistance)
                {
                    old = false;
                    minDistance = currentMinDistance;
                    result = it;
                }
                if (it.retun == 1)
                {
                    int currentMinDistance2 = Math.Abs(y - (it.rect_old.Y + it.rect_old.Height / 2));
                    if (currentMinDistance2 < minDistance)
                    {
                        old = true;
                        minDistance = currentMinDistance2;
                        result = it;
                    }
                }
            }
            return result;
        }

        #endregion

        int CurrentPosIndex = 0;
        internal void SetCaretPostion()
        {
            SetCaretPostion(CurrentPosIndex);
        }
        internal void SetCaretPostion(int PosIndex)
        {
            CurrentPosIndex = PosIndex;
            if (showCaret)
            {
                if (cache_font == null)
                {
                    if (ModeRange) ModeRangeCaretPostion(true);
                    else
                    {
                        if (textalign == HorizontalAlignment.Center) SetCaretX(rect_text.X + rect_text.Width / 2);
                        else if (textalign == HorizontalAlignment.Right) SetCaretX(rect_text.Right);
                    }
                }
                else
                {
                    Rectangle r;
                    if (PosIndex >= cache_font.Length)
                    {
                        var it = cache_font[cache_font.Length - 1];
                        r = it.rect;
                        SetCaretXY(it.retun == 1 ? r.X : r.Right, r.Y);
                    }
                    else
                    {
                        var it = cache_font[PosIndex];
                        if (it.retun == 1)
                        {
                            if (PosIndex > 0)
                            {
                                it = cache_font[PosIndex - 1];
                                r = it.rect;
                                SetCaretXY(r.Right, r.Y);
                            }
                            else
                            {
                                r = it.rect_old;
                                SetCaretXY(r.X, r.Y);
                            }
                        }
                        else
                        {
                            r = it.rect;
                            SetCaretXY(r.X, r.Y);
                        }
                    }
                    if (ModeRange) ModeRangeCaretPostion(false);
                    ITask.Run(() => { ScrollTo(r); });
                }
                showCaretFlag = true;
                Invalidate();
            }
        }

        void OnImeStartPrivate(IntPtr hIMC)
        {
            var point = CurrentCaret.Location;
            point.Offset(0, -scrolly);
            var CandidateForm = new Win32.CANDIDATEFORM()
            {
                dwStyle = Win32.CFS_CANDIDATEPOS,
                ptCurrentPos = point,
            };
            Win32.ImmSetCandidateWindow(hIMC, ref CandidateForm);
            var CompositionForm = new Win32.COMPOSITIONFORM()
            {
                dwStyle = Win32.CFS_FORCE_POSITION,
                ptCurrentPos = point,
            };
            Win32.ImmSetCompositionWindow(hIMC, ref CompositionForm);
            var logFont = new Win32.LOGFONT()
            {
                lfHeight = CurrentCaret.Height,
                lfFaceName = Font.Name + "\0"
            };
            Win32.ImmSetCompositionFont(hIMC, ref logFont);
        }
        void OnImeEndPrivate(IntPtr hIMC)
        {
            Win32.ImmReleaseContext(Handle, hIMC);
        }
        void OnImeResultStrPrivate(IntPtr hIMC, string? strResult)
        {
            var CompositionForm = new Win32.COMPOSITIONFORM()
            {
                dwStyle = Win32.CFS_FORCE_POSITION,
                ptCurrentPos = CurrentCaret.Location
            };
            Win32.ImmSetCompositionWindow(hIMC, ref CompositionForm);
            if (strResult != null && !string.IsNullOrEmpty(strResult))
            {
                var chars = new List<string>(strResult.Length);
                foreach (char key in strResult)
                {
                    if (Verify(key, out var change)) chars.Add(change ?? key.ToString());
                }
                if (chars.Count > 0) EnterText(string.Join("", chars));
            }
        }

        protected virtual bool Verify(char key, out string? change)
        {
            change = null;
            return true;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            ThreadFocus?.Dispose();
            ThreadHover?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadHover = null;
        ITask? ThreadFocus = null;
    }
}
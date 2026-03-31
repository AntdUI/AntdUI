// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;

namespace AntdUI
{
    partial class Input
    {
        bool SetText(string value, bool active, bool changed = true)
        {
            value ??= "";
            if (_text == value) return false;
            _text = value;
            isempty = string.IsNullOrEmpty(_text);
            OnAllowClear();
            FixFontWidth();
            if (active) OnSetText(value, isempty);
            if (!DesignMode)
            {
                if (isempty)
                {
                    if (selectionStart > 0) SetSelectionStart(0);
                }
                else if (cache_font != null && (SetTextSelectionEnd || cache_font.Count < selectionStart)) SetSelectionStart(cache_font.Count);
            }
            if (changed)
            {
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged(nameof(Text));
            }
            return true;
        }
        bool SetTextIn(string text, int start, out int len, bool changed = true)
        {
            var value = _text.Insert(start, text);
            _text = value;
            var font_widths = FixFontWidth(text, start, out len);
            if (cache_font == null) cache_font = font_widths;
            else cache_font.InsertRange(start, font_widths);
            isempty = false;
            OnAllowClear();
            CalculateRect();
            OnSetText(value, isempty);
            SetSelectionStart(cache_font.Count);
            OnTextChanged(EventArgs.Empty);
            OnPropertyChanged(nameof(Text));
            return true;
        }
        bool SetTextAppend(string text, int start, out int len)
        {
            var value = _text + text;
            _text = value;
            var font_widths = FixFontWidth(text, start, out len);
            if (cache_font == null) cache_font = font_widths;
            else cache_font.AddRange(font_widths);
            isempty = false;
            OnAllowClear();
            CalculateRect();
            OnSetText(value, isempty);
            SetSelectionStart(cache_font.Count);
            OnTextChanged(EventArgs.Empty);
            OnPropertyChanged(nameof(Text));
            return true;
        }
        bool SetTextRemove(ref List<CacheFont> cache_font, int start, int end, bool r = false)
        {
            try
            {
                cache_font.RemoveRange(start, end);
            }
            catch
            {
                cache_font.RemoveAll(a => a.i == start);
            }
            if (cache_font.Count > 0)
            {
                var texts = new List<string>(cache_font.Count);
                foreach (var it in cache_font) texts.Add(it.text);
                var value = string.Join("", texts);
                if (_text == value) return false;
                isempty = false;
                _text = value;
                if (r) return true;
                OnAllowClear();
                CalculateRect();
                OnSetText(value, isempty);
                SetSelectionStart(cache_font.Count);
            }
            else
            {
                var value = "";
                if (_text == value) return false;
                isempty = true;
                _text = value;
                if (r) return true;
                OnAllowClear();
                CleanCacheFont();
                OnSetText(value, isempty);
                if (selectionStart > 0) SetSelectionStart(0);
            }
            OnTextChanged(EventArgs.Empty);
            OnPropertyChanged(nameof(Text));
            return true;
        }

        protected virtual void OnSetText(string text, bool isempty)
        {
        }
    }
}
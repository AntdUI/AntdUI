// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;

namespace AntdUI
{
    partial class CellButton
    {
        public override void Paint(Canvas g, Font font, bool enable, SolidBrush fore) => Table.PaintButton(g, font, PARENT.PARENT.Gaps, Rect, this, enable, PARENT.PARENT.ColorScheme);

        #region GetSize

        public override Size GetSize(Canvas g, Font font, TableGaps gap)
        {
            if (Gap.HasValue)
            {
                int sp = (int)(Gap.Value * g.Dpi);
                return GetSizeCore(g, font, sp, sp * 2);
            }
            else if (PARENT.PARENT.GapCell.HasValue)
            {
                int sp = (int)(PARENT.PARENT.GapCell.Value * g.Dpi);
                return GetSizeCore(g, font, sp, sp * 2);
            }
            else return GetSizeCore(g, font, gap.x, gap.x2);
        }

        Size GetSizeCore(Canvas g, Font font, int gap, int gap2)
        {
            if (string.IsNullOrEmpty(Text))
            {
                var size = g.MeasureString(Config.NullText, font);
                int sizei = size.Height + gap;
                return new Size(sizei, sizei);
            }
            else
            {
                var size = g.MeasureText(Text ?? Config.NullText, font);
                bool has_icon = HasIcon || loading;
                if (has_icon || ShowArrow)
                {
                    if (has_icon && (IconPosition == TAlignMini.Top || IconPosition == TAlignMini.Bottom))
                    {
                        int size_read = (int)Math.Ceiling(size.Height * 1.2F);
                        return new Size(size.Width + gap2 * 2 + size_read, size.Height + gap + size_read);
                    }
                    int height = size.Height + gap;
                    if (has_icon && ShowArrow) return new Size(size.Width + gap2 + size.Height * 2, height);
                    else if (has_icon) return new Size(size.Width + gap2 + (int)Math.Ceiling(size.Height * 1.2F), height);
                    else return new Size(size.Width + gap2 + (int)Math.Ceiling(size.Height * .8F), height);
                }
                else return new Size(size.Width + gap2, size.Height + gap);
            }
        }

        #endregion

        #region 动画

        internal override bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                if (Enabled)
                {
                    Color _back_hover;
                    switch (Type)
                    {
                        case TTypeMini.Default:
                            if (BorderWidth > 0) _back_hover = Colour.PrimaryHover.Get(nameof(Button), PARENT.PARENT.ColorScheme);
                            else _back_hover = Colour.FillSecondary.Get(nameof(Button), PARENT.PARENT.ColorScheme);
                            break;
                        case TTypeMini.Success:
                            _back_hover = Colour.SuccessHover.Get(nameof(Button), PARENT.PARENT.ColorScheme);
                            break;
                        case TTypeMini.Error:
                            _back_hover = Colour.ErrorHover.Get(nameof(Button), PARENT.PARENT.ColorScheme);
                            break;
                        case TTypeMini.Info:
                            _back_hover = Colour.InfoHover.Get(nameof(Button), PARENT.PARENT.ColorScheme);
                            break;
                        case TTypeMini.Warn:
                            _back_hover = Colour.WarningHover.Get(nameof(Button), PARENT.PARENT.ColorScheme);
                            break;
                        case TTypeMini.Primary:
                        default:
                            _back_hover = Colour.PrimaryHover.Get(nameof(Button), PARENT.PARENT.ColorScheme);
                            break;
                    }

                    if (BackHover.HasValue) _back_hover = BackHover.Value;
                    if (Config.HasAnimation(nameof(Table)))
                    {
                        if (IconHoverAnimation > 0 && HasIcon && (IconHoverSvg != null || IconHover != null))
                        {
                            ThreadImageHover?.Dispose();
                            AnimationImageHover = true;
                            ThreadImageHover = new AnimationTask(new AnimationFixedConfig(i =>
                            {
                                AnimationImageHoverValue = i;
                                OnPropertyChanged();
                            }, 10, Animation.TotalFrames(10, IconHoverAnimation), value, AnimationType.Ball).SetEnd(() => AnimationImageHover = false));
                        }
                        if (_back_hover.A > 0)
                        {
                            int addvalue = _back_hover.A / 12;
                            ThreadHover?.Dispose();
                            AnimationHover = true;
                            ThreadHover = new AnimationTask(new AnimationLinearConfig(PARENT.PARENT, i =>
                            {
                                AnimationHoverValue = i;
                                OnPropertyChanged();
                                return true;
                            }, 10).SetValueColor(AnimationHoverValue, value, addvalue, _back_hover.A).SetEnd(() => AnimationHover = false));
                        }
                        else
                        {
                            AnimationHoverValue = _back_hover.A;
                            OnPropertyChanged();
                        }
                    }
                    else AnimationHoverValue = _back_hover.A;
                    OnPropertyChanged();
                }
            }
        }

        internal override void Click()
        {
            if (Config.HasAnimation(nameof(Table)))
            {
                ThreadClick?.Dispose();
                AnimationClickValue = 0;
                AnimationClick = true;
                ThreadClick = new AnimationTask(new AnimationLoopConfig(PARENT.PARENT, () =>
                {
                    if (AnimationClickValue > 0.6) AnimationClickValue = AnimationClickValue.Calculate(0.04F);
                    else AnimationClickValue += AnimationClickValue = AnimationClickValue.Calculate(0.1F);
                    if (AnimationClickValue > 1) { AnimationClickValue = 0F; return false; }
                    OnPropertyChanged();
                    return true;
                }, 50).SetEnd(() =>
                {
                    AnimationClick = false;
                    OnPropertyChanged();
                }));
            }
        }

        #region 加载动画

        AnimationTask? ThreadLoading;

        #endregion

        #endregion
    }
}
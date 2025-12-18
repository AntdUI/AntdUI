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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Icon : UserControl, AntdUI.IEventListener
    {
        AntdUI.BaseForm form;
        public Icon(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            segmented1.Items.Add(new AntdUI.SegmentedItem
            {
                Badge = "NEW",
                IconSvg = "<svg viewBox=\"0 0 1400 1400\"><g transform=\"rotate(22 598.4441528320312,238.4444885253905) matrix(15.128077386709723,0,0,15.128077386709723,-1642.157987708274,-1535.3378108111465) \"><path d=\"m148.108852,131.25001c9.334,0 13.999,-6.268 13.999,-14c0,-7.73 -4.665,-13.998 -14,-13.998c-9.333,0 -13.998,6.268 -13.998,13.999s4.664,13.999 13.999,13.999\" fill=\"#F8312F\"/><path d=\"m142.609852,122.25201a4.5,4.5 0 1 0 0,-9a4.5,4.5 0 0 0 0,9m11,0a4.5,4.5 0 1 0 0,-9a4.5,4.5 0 0 0 0,9\" fill=\"#fff\"/><path d=\"m146.399852,113.75301a0.75,0.75 0 0 1 -0.08,1.498c-1.017,-0.054 -1.989,-0.304 -2.817,-0.88c-0.835,-0.582 -1.46,-1.452 -1.854,-2.631a0.75,0.75 0 1 1 1.422,-0.476c0.31,0.928 0.762,1.509 1.29,1.876c0.534,0.372 1.21,0.569 2.039,0.613m3.42,0a0.75,0.75 0 0 0 0.08,1.498c1.017,-0.054 1.989,-0.304 2.817,-0.88c0.835,-0.582 1.46,-1.452 1.854,-2.631a0.75,0.75 0 1 0 -1.422,-0.476c-0.31,0.928 -0.763,1.509 -1.29,1.876c-0.534,0.372 -1.21,0.569 -2.039,0.613m-1.71,11.499c-2.005,0 -2.934,1.104 -3.106,1.447a1,1 0 1 1 -1.789,-0.894c0.497,-0.99 2.1,-2.553 4.895,-2.553s4.4,1.562 4.894,2.553a1,1 0 1 1 -1.788,0.894c-0.172,-0.343 -1.101,-1.447 -3.106,-1.447m-2,-7a2,2 0 1 1 -4,0a2,2 0 0 1 4,0m8,0a2,2 0 1 1 -4,0a2,2 0 0 1 4,0\" fill=\"#402A32\"/></g><g><g transform=\"matrix(19.14066672327408,0,0,19.14066672327408,0,0) \"><path d=\"m56.598733,31.240002c9.334,0 13.999,-6.268 13.999,-14c0,-7.73 -4.665,-13.998 -14,-13.998c-9.333,0 -13.998,6.268 -13.998,13.999s4.664,13.999 13.999,13.999\" fill=\"#FFB02E\"/><path d=\"m56.599733,26.242002c-9,0 -9,-9 -9,-9l18,0s0,9 -9,9\" fill=\"#BB1D80\"/><path d=\"m48.599733,17.742002l0,-0.5l16,0l0,0.5a1,1 0 0 1 -1,1l-14,0a1,1 0 0 1 -1,-1\" fill=\"#fff\"/><path d=\"m43.780733,10.992002c2.016,2.327 4.698,3.346 6.073,3.74a1.6,1.6 0 0 0 1.57,-0.412c1.194,-1.178 3.6,-3.879 4.114,-6.963c0.718,-4.306 -4.682,-5.62 -6.858,-2.07c-5.09,-2.39 -7.784,2.376 -4.9,5.705m25.637,0.002c-2.015,2.326 -4.695,3.344 -6.07,3.737c-0.563,0.162 -1.154,0 -1.57,-0.411c-1.194,-1.177 -3.598,-3.877 -4.112,-6.96c-0.718,-4.304 4.68,-5.617 6.855,-2.069c5.089,-2.39 7.78,2.375 4.897,5.703\" fill=\"#F70A8D\"/></g></g><g><g transform=\"matrix(19.14066672327408,0,0,19.14066672327408,0,0) \"><path d=\"m26.442705,67.193412c6.6,-6.6 5.467,-14.33 0,-19.798s-13.197,-6.6 -19.797,0s-5.467,14.33 0,19.799s13.197,6.6 19.797,0\" fill=\"#FFB02E\"/><path d=\"m15.596705,47.078412a1,1 0 1 1 1.898,0.633l-0.782,2.345l2.345,-0.782a1,1 0 1 1 0.633,1.898l-4.243,1.414a1,1 0 0 1 -1.265,-1.265l1.414,-4.243zm-10.681,13.51a1,1 0 1 0 0.633,1.898l2.345,-0.782l-0.782,2.345a1,1 0 0 0 1.898,0.633l1.414,-4.243a1,1 0 0 0 -1.265,-1.265l-4.243,1.414z\" fill=\"#402A32\"/><path d=\"m22.909705,63.658412c-6.364,6.364 -12.728,0 -12.728,0l12.728,-12.728s6.364,6.364 0,12.728\" fill=\"#BB1D80\"/><path d=\"m11.242705,63.304412l-0.354,-0.353l11.314,-11.314l0.353,0.354a1,1 0 0 1 0,1.414l-9.9,9.9a1,1 0 0 1 -1.413,0\" fill=\"#fff\"/><path d=\"m11.595705,63.658412l0,4.766a2.121,2.121 0 1 1 -4.242,0l0,-4.766a2.121,2.121 0 1 1 4.242,0m11.49,-11.49l4.766,0a2.121,2.121 0 0 0 0,-4.243l-4.765,0a2.121,2.121 0 1 0 0,4.243\" fill=\"#3F5FFF\"/></g></g><g><g transform=\"matrix(19.14066672327408,0,0,19.14066672327408,0,0) \"><path d=\"m55.192429,70.597491c9.334,0 13.999,-6.268 13.999,-14c0,-7.73 -4.665,-13.998 -14,-13.998c-9.333,0 -13.998,6.268 -13.998,13.999s4.664,13.999 13.999,13.999\" fill=\"#FFB02E\"/><path d=\"m53.151429,57.979491a4.5,4.5 0 1 0 -6.915,0c0.419,-0.21 0.946,-0.435 1.547,-0.604c0.58,-0.164 1.91,-0.776 1.91,-0.776s1.935,0.744 2.683,1.035c0.286,0.112 0.547,0.23 0.775,0.345\" fill=\"#fff\"/><path d=\"m46.167429,50.257491c0.144,-0.43 0.502,-1.108 1.095,-1.67c0.585,-0.555 1.383,-0.988 2.431,-0.988a0.5,0.5 0 0 0 0,-1c-1.351,0 -2.386,0.567 -3.118,1.262c-0.724,0.688 -1.166,1.51 -1.355,2.08a0.5,0.5 0 0 0 0.948,0.316m14.025,-1.158a0.5,0.5 0 0 1 0.5,-0.5c0.774,0 1.742,0.284 2.585,0.83c0.848,0.55 1.612,1.396 1.9,2.549a0.5,0.5 0 0 1 -0.97,0.242c-0.212,-0.847 -0.781,-1.502 -1.475,-1.952c-0.7,-0.453 -1.48,-0.669 -2.04,-0.669a0.5,0.5 0 0 1 -0.5,-0.5m-7,6.5a3,3 0 0 1 -0.802,2.041c-0.75,-0.294 -1.683,-0.541 -2.698,-0.541c-0.687,0 -1.336,0.113 -1.917,0.278a3,3 0 1 1 5.417,-1.778m4.97,0.243c0.078,-0.313 0.575,-1.243 2.03,-1.243s1.952,0.931 2.03,1.242a1,1 0 1 0 1.94,-0.485c-0.255,-1.021 -1.425,-2.757 -3.97,-2.757s-3.715,1.735 -3.97,2.758a1,1 0 1 0 1.94,0.485m-2.3,2.677a1,1 0 1 0 0,2a0.52,0.52 0 1 1 0,1.04a1,1 0 1 0 0,2a0.52,0.52 0 1 1 0,1.04a1,1 0 1 0 0,2a2.52,2.52 0 0 0 2.01,-4.04a2.52,2.52 0 0 0 -2.01,-4.04\" fill=\"#402A32\"/><path d=\"m62.935429,67.169491c1.097,-0.018 2.999,-0.232 4.737,-1.339c2.693,-1.714 1.81,-5.582 -2.22,-4.976c-0.776,-2.916 -4.798,-3.123 -5.191,0.016c-0.264,2.102 0.69,4.315 1.293,5.464a1.53,1.53 0 0 0 1.38,0.834\" fill=\"#F70A8D\"/></g></g><g transform=\"rotate(-30 353.3338012695311,596.666748046875) \"><g transform=\"matrix(19.14066672327408,0,0,19.14066672327408,0,0) \"><path d=\"m32.425344,30.749708c0,7.731 -4.665,13.999 -14,13.999c-9.333,0 -13.998,-6.268 -13.998,-14q0,-1.018 0.108,-1.998l13.89,-6l13.892,6q0.108,0.98 0.108,1.999\" fill=\"#FFB02E\"/><path d=\"m32.317344,28.750708c-0.747,-6.785 -5.376,-12 -13.891,-12s-13.144,5.215 -13.891,12l27.782,0z\" fill=\"#5092FF\"/><path d=\"m12.927344,30.750708a4.5,4.5 0 1 0 0,-9a4.5,4.5 0 0 0 0,9m11,0a4.5,4.5 0 1 0 0,-9a4.5,4.5 0 0 0 0,9\" fill=\"#fff\"/><path d=\"m15.427344,35.750708a3,3 0 1 1 6,0l0,3a3,3 0 1 1 -6,0l0,-3z\" fill=\"#BB1D80\"/><path d=\"m3.427344,33.250708l0,5.381c0,1.367 0.49,2.69 1.379,3.728l1.571,1.833a4.017,4.017 0 1 0 5.278,-5.957l-4.513,-3.008a1.6,1.6 0 0 1 -0.715,-1.337l0,-0.64a1.5,1.5 0 0 0 -3,0m30.065,0l0,5.381c0,1.367 -0.489,2.69 -1.379,3.728l-1.57,1.833a4.017,4.017 0 1 1 -5.279,-5.957l4.514,-3.008c0.446,-0.298 0.714,-0.8 0.714,-1.336l0,-0.641a1.5,1.5 0 0 1 3,0\" fill=\"#FF822D\"/></g></g><g><g transform=\"matrix(19.14066672327408,0,0,19.14066672327408,0,0) \"><path d=\"m36.569745,50.570348c9.334,0 13.999,-6.268 13.999,-14c0,-7.73 -4.665,-13.998 -14,-13.998c-9.333,0 -13.998,6.268 -13.998,13.999s4.664,13.999 13.999,13.999\" fill=\"#FFB02E\"/><path d=\"m23.818745,38.914348a1,1 0 0 0 -0.237,0.798a1,1 0 0 0 -1.161,1.62l0.292,0.25a1,1 0 0 0 -0.781,1.758l1.985,1.654l-0.126,-0.062a1.116,1.116 0 0 0 -1.146,1.903c1.292,0.917 3.02,2.132 3.926,2.737c1.5,1 3.21,1.342 5,0c1,-0.75 2.265,-3.062 0.935,-5.366c-0.252,-0.436 -0.478,-0.895 -0.572,-1.39c-0.411,-2.18 -1.025,-4.413 -2.363,-3.744c-0.725,0.362 -0.662,0.987 -0.572,1.875c0.034,0.337 0.072,0.712 0.072,1.125l-0.08,0.04l-3.762,-3.293a1,1 0 0 0 -1.41,0.095m25.505,0c0.199,0.227 0.276,0.52 0.237,0.798a1,1 0 0 1 1.16,1.62l-0.292,0.25a1,1 0 0 1 0.782,1.758l-1.985,1.654l0.126,-0.062a1.116,1.116 0 0 1 1.146,1.903c-1.292,0.917 -3.02,2.132 -3.927,2.737c-1.5,1 -3.211,1.342 -5,0c-1,-0.75 -2.265,-3.062 -0.935,-5.366c0.252,-0.436 0.478,-0.895 0.572,-1.39c0.411,-2.18 1.025,-4.413 2.363,-3.744c0.725,0.362 0.662,0.987 0.572,1.875c-0.034,0.337 -0.072,0.712 -0.072,1.125l0.08,0.04l3.762,-3.293a1,1 0 0 1 1.41,0.095\" fill=\"#FF822D\"/><path d=\"m27.570745,35.072348c0,1.38 -0.895,2.5 -2,2.5s-2,-1.12 -2,-2.5s0.895,-2.5 2,-2.5s2,1.12 2,2.5m22,0c0,1.38 -0.895,2.5 -2,2.5s-2,-1.12 -2,-2.5s0.895,-2.5 2,-2.5s2,1.12 2,2.5\" fill=\"#FF6723\"/><path d=\"m29.919745,30.985348c-0.21,0.227 -0.32,0.53 -0.367,0.778a1,1 0 0 1 -1.964,-0.382c0.089,-0.456 0.31,-1.153 0.858,-1.749c0.577,-0.629 1.44,-1.06 2.624,-1.06c1.195,0 2.062,0.452 2.636,1.082c0.544,0.597 0.77,1.292 0.85,1.747a1,1 0 0 1 -1.971,0.342a1.55,1.55 0 0 0 -0.358,-0.742c-0.187,-0.207 -0.515,-0.429 -1.157,-0.429c-0.653,0 -0.972,0.217 -1.151,0.412m11.001,0.001c-0.21,0.227 -0.32,0.53 -0.368,0.778a1,1 0 0 1 -1.963,-0.382c0.088,-0.456 0.31,-1.153 0.857,-1.749c0.577,-0.629 1.44,-1.06 2.624,-1.06c1.196,0 2.062,0.452 2.636,1.082c0.544,0.597 0.77,1.292 0.85,1.747a1,1 0 0 1 -1.971,0.342a1.55,1.55 0 0 0 -0.358,-0.742c-0.187,-0.207 -0.515,-0.429 -1.157,-0.429c-0.653,0 -0.971,0.217 -1.15,0.412m-10.55,4.988a1,1 0 1 0 -1.6,1.2c0.702,0.936 3.008,2.9 7.8,2.9s7.098,-1.964 7.8,-2.9a1,1 0 0 0 -1.6,-1.2c-0.298,0.397 -1.992,2.1 -6.2,2.1s-5.902,-1.703 -6.2,-2.1\" fill=\"#402A32\"/></g></g></svg>",
                Text = "Emoji"
            });
            LoadData();
        }

        #region 数据

        private void segmented1_SelectIndexChanged(object sender, AntdUI.IntEventArgs e) => LoadData();

        void LoadData()
        {
            int index = segmented1.SelectIndex;
            if (index == 2)
            {
                var list = new List<AntdUI.VirtualItem>(AntdUI.SvgDb.Emoji.Count);
                foreach (var it in AntdUI.SvgDb.Emoji) list.Add(new EItem(it.Key, it.Value));
                vpanel.Items.Clear();
                txt_search.Text = "";
                vpanel.Items.AddRange(list);
            }
            else
            {
                var data = GetData(index);
                var svgs = new List<AntdUI.VirtualItem>(data.Count);
                foreach (var it in data)
                {
                    svgs.Add(new TItem(it.Key, it.Value));
                    svgs.AddRange(it.Value);
                }
                vpanel.Items.Clear();
                txt_search.Text = "";
                vpanel.Items.AddRange(svgs);
            }
        }

        Dictionary<string, List<VItem>> GetData(int index)
        {
            var dir = new Dictionary<string, List<VItem>>(AntdUI.SvgDb.Custom.Count);
            var tmp = new List<VItem>(AntdUI.SvgDb.Custom.Count);
            if (index == 0)
            {
                foreach (var it in AntdUI.SvgDb.Custom)
                {
                    if (it.Key == "QuestionOutlined")
                    {
                        dir.Add(AntdUI.Localization.Get("Icon.Directional", "方向性图标"), new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "EditOutlined")
                    {
                        dir.Add(AntdUI.Localization.Get("Icon.Suggested", "提示建议性图标"), new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AreaChartOutlined")
                    {
                        dir.Add(AntdUI.Localization.Get("Icon.Editor", "编辑类图标"), new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AndroidOutlined")
                    {
                        dir.Add(AntdUI.Localization.Get("Icon.Data", "数据类图标"), new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AccountBookOutlined")
                    {
                        dir.Add(AntdUI.Localization.Get("Icon.Logos", "品牌和标识"), new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "StepBackwardFilled")
                    {
                        dir.Add(AntdUI.Localization.Get("Icon.Application", "网站通用图标"), new List<VItem>(tmp));
                        tmp.Clear();
                        return dir;
                    }
                    tmp.Add(new VItem(it.Key, it.Value));
                }
                dir.Add(AntdUI.Localization.Get("Icon.Application", "网站通用图标"), new List<VItem>(tmp));
                tmp.Clear();
            }
            else
            {
                bool isadd = false;
                foreach (var it in AntdUI.SvgDb.Custom)
                {
                    if (it.Key == "StepBackwardFilled") isadd = true;
                    else if (it.Key == "QuestionCircleFilled")
                    {
                        dir.Add(AntdUI.Localization.Get("Icon.Directional", "方向性图标"), new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "EditFilled")
                    {
                        dir.Add(AntdUI.Localization.Get("Icon.Suggested", "提示建议性图标"), new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "PieChartFilled")
                    {
                        dir.Add(AntdUI.Localization.Get("Icon.Editor", "编辑类图标"), new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AndroidFilled")
                    {
                        dir.Add(AntdUI.Localization.Get("Icon.Data", "数据类图标"), new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    else if (it.Key == "AccountBookFilled")
                    {
                        dir.Add(AntdUI.Localization.Get("Icon.Logos", "品牌和标识"), new List<VItem>(tmp));
                        tmp.Clear();
                    }
                    if (isadd) tmp.Add(new VItem(it.Key, it.Value));
                }
                dir.Add(AntdUI.Localization.Get("Icon.Application", "网站通用图标"), new List<VItem>(tmp));
                tmp.Clear();
            }
            return dir;
        }

        #endregion

        #region 渲染

        class TItem : AntdUI.VirtualItem
        {
            string title, count;
            public List<VItem> data;
            public TItem(string t, List<VItem> d)
            {
                CanClick = false;
                data = d;
                title = t;
                count = d.Count.ToString();
            }

            public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                using (var font_title = new Font(e.Panel.Font, FontStyle.Bold))
                using (var font_count = new Font(e.Panel.Font.FontFamily, e.Panel.Font.Size * .74F, e.Panel.Font.Style))
                {
                    var size = AntdUI.Helper.Size(g.MeasureString(title, font_title));
                    g.String(title, font_title, AntdUI.Style.Db.Text, new Rectangle(e.Rect.X + x, e.Rect.Y, e.Rect.Width, e.Rect.Height), AntdUI.FormatFlags.Left | AntdUI.FormatFlags.VerticalCenter | AntdUI.FormatFlags.NoWrap);

                    var rect_count = new Rectangle(e.Rect.X + x + size.Width + gap, e.Rect.Y + (e.Rect.Height - size.Height) / 2, size.Height, size.Height);
                    using (var path = AntdUI.Helper.RoundPath(rect_count, e.Radius))
                    {
                        g.Fill(AntdUI.Style.Db.TagDefaultBg, path);
                        g.Draw(AntdUI.Style.Db.DefaultBorder, sp, path);
                    }
                    g.String(count, font_count, AntdUI.Style.Db.Text, rect_count, AntdUI.FormatFlags.Center);
                }
            }

            int gap = 8, sp = 1, x = 30;
            public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = g.Dpi;
                gap = (int)(8 * dpi);
                sp = (int)(1 * dpi);
                x = (int)(30 * dpi);
                return new Size(e.Rect.Width, (int)(44 * dpi));
            }
        }
        class VItem : AntdUI.VirtualItem
        {
            public string Key, Value;
            public VItem(string key, string value) { Tag = Key = key; Value = value; }

            internal Bitmap bmp = null, bmp_ac = null;
            public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                if (Hover)
                {
                    using (var path = AntdUI.Helper.RoundPath(e.Rect, e.Radius))
                    {
                        g.Fill(AntdUI.Style.Db.Primary, path);
                    }
                    if (bmp_ac == null) bmp_ac = AntdUI.SvgExtend.SvgToBmp(Value, icon_size, icon_size, AntdUI.Style.Db.PrimaryColor);
                    g.Image(bmp_ac, rect_icon);
                    g.String(Key, e.Panel.Font, AntdUI.Style.Db.PrimaryColor, rect_text, AntdUI.FormatFlags.Center | AntdUI.FormatFlags.NoWrap);
                }
                else
                {
                    if (bmp == null) bmp = AntdUI.SvgExtend.SvgToBmp(Value, icon_size, icon_size, AntdUI.Style.Db.Text);
                    g.Image(bmp, rect_icon);
                    g.String(Key, e.Panel.Font, AntdUI.Style.Db.Text, rect_text, AntdUI.FormatFlags.Center | AntdUI.FormatFlags.NoWrap);

                }
            }

            int icon_size = 36;
            Rectangle rect_icon, rect_text;
            public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = g.Dpi;
                int w = (int)(200 * dpi), h = (int)(100 * dpi);
                icon_size = (int)(36 * dpi);
                int text_size = (int)(24 * dpi), y = (h - (icon_size + text_size)) / 2;
                rect_icon = new Rectangle((w - icon_size) / 2, y, icon_size, icon_size);
                rect_text = new Rectangle(0, y + icon_size / 2 + text_size, w, text_size);
                return new Size(w, h);
            }
        }

        class EItem : AntdUI.VirtualItem
        {
            public string Key, Value;
            public EItem(string key, string value)
            {
                Tag = Key = key;
                Value = value;
            }

            Bitmap bmp = null;
            public override void Paint(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                if (bmp == null) bmp = AntdUI.SvgExtend.SvgToBmp(Value, rect_icon_hover.Width, rect_icon_hover.Height, AntdUI.Style.Db.Text);
                if (Hover)
                {
                    using (var path = AntdUI.Helper.RoundPath(e.Rect, e.Radius))
                    {
                        g.Draw(AntdUI.Style.Db.Primary, sp, path);
                    }
                    g.Image(bmp, rect_icon_hover);
                }
                else g.Image(bmp, rect_icon);
            }

            int sp = 4;
            Rectangle rect_icon, rect_icon_hover;
            public override Size Size(AntdUI.Canvas g, AntdUI.VirtualPanelArgs e)
            {
                var dpi = g.Dpi;
                sp = (int)(4 * dpi);
                int size = (int)(100 * dpi);
                int icon_size = (int)(48 * dpi), xy = (size - icon_size) / 2;
                int icon_size_hover = (int)(78 * dpi), xy_hover = (size - icon_size_hover) / 2;
                rect_icon = new Rectangle(xy, xy, icon_size, icon_size);
                rect_icon_hover = new Rectangle(xy_hover, xy_hover, icon_size_hover, icon_size_hover);
                return new Size(size, size);
            }
        }

        #endregion

        private void vpanel_ItemClick(object sender, AntdUI.VirtualItemEventArgs e)
        {
            if (e.Item is VItem item)
            {
                if (AntdUI.Helper.ClipboardSetText(this, item.Key)) AntdUI.Message.success(form, item.Key + " " + AntdUI.Localization.Get("CopyOK", "复制成功"));
                else AntdUI.Message.error(form, item.Key + " " + AntdUI.Localization.Get("CopyFailed", "复制失败"));
            }
            else if (e.Item is EItem emoji)
            {
                if (AntdUI.Helper.ClipboardSetText(this, emoji.Key)) AntdUI.Message.success(form, emoji.Key + " " + AntdUI.Localization.Get("CopyOK", "复制成功"));
                else AntdUI.Message.error(form, emoji.Key + " " + AntdUI.Localization.Get("CopyFailed", "复制失败"));
            }
        }

        #region 搜索

        private void txt_search_TextChanged(object sender, System.EventArgs e) => LoadSearchList();
        private void txt_search_SuffixClick(object sender, MouseEventArgs e) => LoadSearchList();

        void LoadSearchList()
        {
            string search = txt_search.Text;
            BeginInvoke(new Action(() =>
            {
                vpanel.PauseLayout = true;
                if (string.IsNullOrEmpty(search))
                {
                    foreach (var it in vpanel.Items) it.Visible = true;
                    vpanel.Empty = false;
                }
                else
                {
                    vpanel.Empty = true;
                    string searchLower = search.ToLower();
                    if (segmented1.SelectIndex == 2)
                    {
                        foreach (var it in vpanel.Items)
                        {
                            if (it is EItem item) it.Visible = item.Key.ToLower().Contains(searchLower);
                        }
                    }
                    else
                    {
                        var titles = new List<TItem>(vpanel.Items.Count);
                        foreach (var it in vpanel.Items)
                        {
                            if (it is VItem item) it.Visible = item.Key.ToLower().Contains(searchLower);
                            else if (it is TItem itemTitle) titles.Add(itemTitle);
                        }
                        foreach (var it in titles)
                        {
                            int count = 0;
                            foreach (var item in it.data)
                            {
                                if (item.Visible) count++;
                            }
                            it.Visible = count > 0;
                        }
                    }
                }
                vpanel.PauseLayout = false;
            }));
        }

        #endregion

        #region 语言变化

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AntdUI.EventHub.AddListener(this);
        }

        public void HandleEvent(AntdUI.EventType id, object tag)
        {
            switch (id)
            {
                case AntdUI.EventType.THEME:
                    foreach (var it in vpanel.Items)
                    {
                        if (it is VItem item)
                        {
                            item.bmp?.Dispose();
                            item.bmp = null;
                        }
                    }
                    break;
            }
        }

        #endregion
    }
}
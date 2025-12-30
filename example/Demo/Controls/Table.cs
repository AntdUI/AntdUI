// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Table : UserControl
    {
        AntdUI.BaseForm form;
        public Table(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();

            #region Table

            table1.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("check").SetFixed(),
                new AntdUI.Column("name", "姓名").SetFixed().SetTree("Sub").SetLocalizationTitleID("Table.Column."),
                new AntdUI.ColumnCheck("checkTitle", "不全选标题").SetColAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.ColumnRadio("radio", "单选").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("online", "状态", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.ColumnSwitch("enable", "启用", AntdUI.ColumnAlign.Center)
                {
                    LocalizationTitle ="Table.Column.{id}",
                    Call = (value, record, i_row, i_col) => {
                        System.Threading.Thread.Sleep(2000);
                        return value;
                    }
                },
                new AntdUI.ColumnSelect("hobby", "爱好") { Items=new List<AntdUI.SelectItem>(){ new AntdUI.SelectItem(EHobbies.读书.ToString(), (int)EHobbies.读书) {IconSvg= "BookOutlined" }, new AntdUI.SelectItem(EHobbies.旅游.ToString(), (int)EHobbies.旅游) { IconSvg = "GlobalOutlined" }, new AntdUI.SelectItem(EHobbies.社交.ToString(), (int)EHobbies.社交) { IconSvg = "CommentOutlined" }, new AntdUI.SelectItem(EHobbies.运动.ToString(), (int)EHobbies.运动) { IconSvg = "DribbbleOutlined" } } }.SetAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("age", "年龄").SetAlign().SetLocalizationTitleID("Table.Column.").SetSummaryItem(AntdUI.TSummaryType.AVG),
                new AntdUI.Column("address", "住址").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("date", "日期").SetLocalizationTitleID("Table.Column.").SetSummaryItem(AntdUI.TSummaryType.Custom,"20后 {0:0} 位"),
                new AntdUI.Column("tag", "Tag"),
                new AntdUI.Column("imgs", "图片").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("btns", "操作").SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
            };

            table1.DataSource = GetPageData(pagination1.Current, pagination1.PageSize);
            pagination1.PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };

            // 配置焦点跳转顺序 
            table1.ConfigureFocusNavigation(["age", "address", "date"], selectAll: true, lineBreak: true);

            //设置总结栏
            SummarySet();
            table1.CustomSummaryCalculate += table1_CustomSummaryCalculate;

            #endregion

            selectEditMode.Items.AddRange(EnumList(typeof(AntdUI.TEditMode)));

            selectEditStyle.Items.AddRange(EnumList(typeof(AntdUI.TEditInputStyle)));

            selectFocusedStyle.Items.AddRange(EnumList(typeof(AntdUI.TableCellFocusedStyle)));
        }


        AntdUI.SelectItem[] EnumList(Type data)
        {
            Array list = Enum.GetValues(data);
            var lists = new List<AntdUI.SelectItem>(list.Length);
            foreach (var it in list) lists.Add(new AntdUI.SelectItem(it));
            lists.RemoveAt(0);
            return lists.ToArray();
        }

        #region 示例

        void checkFixedHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.FixedHeader = e.Value;
        }

        void checkColumnDragSort_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.ColumnDragSort = e.Value;
        }

        void checkRowsDragSort_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value) table1.Columns.Insert(0, new AntdUI.ColumnSort() { Fixed = true });
            else table1.Columns.RemoveAt(0);
        }

        void checkBordered_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.Bordered = e.Value;
        }

        void checkScrollBarAvoidHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.ScrollBarAvoidHeader = e.Value;
        }

        #region 奇偶列

        void checkSetRowStyle_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value) table1.SetRowStyle += table1_SetRowStyle;
            else table1.SetRowStyle -= table1_SetRowStyle;
            table1.Invalidate();
        }
        AntdUI.Table.CellStyleInfo table1_SetRowStyle(object sender, AntdUI.TableSetRowStyleEventArgs e)
        {
            if (e.Index % 2 == 0)
            {
                return new AntdUI.Table.CellStyleInfo
                {
                    BackColor = AntdUI.Style.Db.ErrorBg,
                    ForeColor = AntdUI.Style.Db.Error
                };
            }
            return null;
        }

        #endregion

        void checkSortOrder_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            foreach (var it in table1.Columns)
            {
                switch (it.Key)
                {
                    case "age":
                    case "address":
                        it.SortOrder = e.Value;
                        break;
                }
            }
        }

        void checkEnableHeaderResizing_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.EnableHeaderResizing = e.Value;
        }

        void checkVisibleHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.VisibleHeader = e.Value;
        }

        void checkAddressLineBreak_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            foreach (var it in table1.Columns)
            {
                switch (it.Key)
                {
                    case "address":
                        if (e.Value) it.Width = "120";
                        else it.Width = null;
                        it.LineBreak = e.Value;
                        return;
                }
            }
        }

        void checkFilter_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value)
            {
                foreach (var it in table1.Columns)
                {
                    switch (it.Key)
                    {
                        case "name":
                            it.Filter = new AntdUI.FilterOption();
                            break;
                        case "hobby":
                        case "age":
                            it.SetDefaultFilter(typeof(int));
                            break;
                        case "address":
                            it.SetDefaultFilter(typeof(string));
                            break;
                        case "date":
                            it.SetDefaultFilter(typeof(DateTime));
                            break;
                    }
                }
            }
            else
            {
                foreach (var it in table1.Columns) it.Filter = null;
            }
        }

        void selectEditMode_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is AntdUI.TEditMode v)
            {
                table1.EditMode = v;
                selectEditStyle.Enabled = true;
            }
            else
            {
                table1.EditMode = AntdUI.TEditMode.None;
                selectEditStyle.Enabled = false;
            }
        }

        void selectEditStyle_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is AntdUI.TEditInputStyle v) table1.EditInputStyle = v;
            else table1.EditInputStyle = AntdUI.TEditInputStyle.Default;
        }

        void selectFocusedStyle_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is AntdUI.TableCellFocusedStyle v) table1.CellFocusedStyle = v;
            else table1.CellFocusedStyle = AntdUI.TableCellFocusedStyle.None;
        }

        bool table1_CellBeginEdit(object sender, AntdUI.TableEventArgs e)
        {
            if (e.Column == null) return true;
            if (e.Column.Key == "tag" || e.Column.Key == "btns" || e.Column.Key == "imgs") return false;
            return true;
        }

        void checkboxFocusNavigation_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.EnableFocusNavigation = checkboxFocusNavigation.Checked;
        }
        void checkboxSummaryCustomize_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.MultipleRows = e.Value;
            table1.SummaryCustomize = e.Value;
            SummarySet();
        }
        void table1_CustomSummaryCalculate(object sender, AntdUI.TableCustomSummaryEventArgs e)
        {
            if (!e.Finalize)
            {
                TestClass item = e.Record as TestClass;
                if (item.date.Year > 1999) e.TotalValue = 1;
            }
        }
        void SummarySet()
        {
            if (table1.SummaryCustomize)
            {
                table1.OnUpdateSummaries();
                return;
            }
            var dataList = (IEnumerable<TestClass>)table1.DataSource;
            table1.Summary = new Dictionary<string, object>
            {
                { "age", dataList.Any() ?(int)dataList.Average(x => x.age) : 0},
                { "address", $"共{dataList.Sum(x =>string.IsNullOrEmpty(x.address) ? 0 : x.address.Split('\n').Length)}地址"} ,
                { "hobby", $"共{dataList.Select(x => x.hobby).Distinct().Count()}种爱好" }
            };
        }

        #endregion

        #region 事件

        void table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Column == null) return;
            if (e.RowIndex > 0 && e.Record is TestClass data)
            {
                if (e.Column.Key == "online") AntdUI.Popover.open(new AntdUI.Popover.Config(table1, "演示一下能弹出自定义") { Offset = e.Rect });
                else if (e.Column.Key == "tag")
                {
                    if (data.tag == null) data.tag = new AntdUI.CellTag[] { new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) };
                    else
                    {
                        if (data.tag.Length == 1)
                        {
                            if (data.tag[0].Text == "ERROR") data.tag = new AntdUI.CellTag[] { new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) };
                            else
                            {
                                data.tag[0].Type = AntdUI.TTypeMini.Error;
                                data.tag[0].Text = "ERROR";
                            }
                        }
                        else data.tag = new AntdUI.CellTag[] { new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) };
                    }
                }
            }
        }

        void table1_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {
            if (e.Record is TestClass data)
            {
                if (e.Btn.Id == "download")
                {
                    e.Btn.Enabled = false;
                    tmpRowIndex = e.RowIndex;
                    tmpProg = 0;
                    AntdUI.ITask.Run(() =>
                    {
                        for (int i = 0; i < 600; i++)
                        {
                            System.Threading.Thread.Sleep(2);
                            tmpProg = i / 600F;
                            table1.Invalidate(tmpRowIndex);
                        }
                        System.Threading.Thread.Sleep(800);
                    }, () =>
                    {
                        tmpRowIndex = -1;
                        tmpProg = 0;
                        e.Btn.Enabled = true;
                    });
                }
                else
                {
                    if (AntdUI.Modal.open(new AntdUI.Modal.Config(form, "是否删除", new AntdUI.Modal.TextLine[] {
                        new AntdUI.Modal.TextLine(data.name,AntdUI.Style.Db.Primary),
                        new AntdUI.Modal.TextLine(data.address,6,AntdUI.Style.Db.TextSecondary)
                    }, AntdUI.TType.Error)
                    {
                        CancelText = null,
                        OkType = AntdUI.TTypeMini.Error,
                        OkText = "删除"
                    }) == DialogResult.OK)
                    {
                        table1.Spin(AntdUI.Localization.Get("Loading2", "正在加载中..."), config =>
                        {
                            System.Threading.Thread.Sleep(1000);
                            for (int i = 0; i < 101; i++)
                            {
                                config.Value = i / 100F;
                                config.Text = AntdUI.Localization.Get("Processing", "处理中") + " " + i + "%";
                                System.Threading.Thread.Sleep(20);
                            }
                            System.Threading.Thread.Sleep(1000);
                            config.Value = null;
                            config.Text = AntdUI.Localization.Get("PleaseWait", "请耐心等候...");
                            System.Threading.Thread.Sleep(2000);
                        }, () =>
                        {
                            System.Diagnostics.Debug.WriteLine("加载结束");
                        });
                    }
                }
            }
        }

        int tmpRowIndex = -1;
        float tmpProg = 0F;
        void table1_RowPaint(object sender, AntdUI.TablePaintRowEventArgs e)
        {
            if (tmpRowIndex == e.RowIndex && tmpProg > 0) e.g.Fill(AntdUI.Style.rgba(AntdUI.Style.Db.SuccessBorder, tmpProg), new Rectangle(e.Rect.X, e.Rect.Y, (int)(e.Rect.Width * tmpProg), e.Rect.Height));
        }

        void table1_FilterPopupEnd(object sender, AntdUI.TableFilterPopupEndEventArgs e)
        {
            AntdUI.Notification.info(form, "筛选结果", $"共筛选到 {(e.Records == null ? 0 : e.Records.Length)} 条结果。", AntdUI.TAlignFrom.Top);
        }

        void table1_FilterDataChanged(object sender, AntdUI.TableFilterDataChangedEventArgs e)
        {
            if (e.Records == null || e.Records.Length == 0)
            {
                table1.Summary = null;
                return;
            }
            table1.Summary = GetPageSummaryData(e.Records);
        }

        void table1_SortRowsTree(object sender, AntdUI.TableSortTreeEventArgs e)
        {
            if (e.Record is TestClass data)
            {
                var temp = data.Sub[e.From];
                data.Sub[e.From] = data.Sub[e.To];
                data.Sub[e.To] = temp;
                e.SetHandled();
            }
        }

        #endregion

        #region 分页与数据

        object GetPageData(int current, int pageSize)
        {
            var list = new List<TestClass>(pageSize);
            int start = Math.Abs(current - 1) * pageSize;
            DateTime now = DateTime.Now;
            DateTime birthday_TigerHu = new DateTime(1983, 7, 4);//数据来源：https://baike.baidu.com/link?url=7UWQOAPtuaXDjkFQZ92-cNlijS9voNgkQEJSmPLDV73RX1RLogXTLRQdBIQ6KMO7s1nIEZDKjvCJXa_e9fOrrhA9HxRDvRbkgGvPdWYMmP7
            DateTime birthday_DanielWu = new DateTime(1974, 9, 30);//数据来源：https://baike.baidu.com/link?url=zk3KO7qvnfny-fZ2QfgQ2-lZleCeNUaCfketfcE6Ur5p_LowHOhlttu0c4tEXDKN673QcgpSRRRUmymic58Rf5NiUpsMJrctl1SXaR2RXuu

            list.Add(new TestClass(start, 0, AntdUI.Localization.Get("Table.Data.Name1", "胡彦斌"), (int)Math.Round((now - birthday_TigerHu).TotalDays / 365)));
            list.Add(new TestClass(start + 1, 1, AntdUI.Localization.Get("Table.Data.Name2", "吴彦祖"), (int)Math.Round((now - birthday_DanielWu).TotalDays / 365))
            {
                tag = new AntdUI.CellTag[] { new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) }
            });
            for (int i = 2; i < pageSize; i++)
            {
                int index = start + i;
                list.Add(new TestClass(index, i, AntdUI.Localization.Get("Table.Data.Name3", "胡彦祖"), 20 + index));
            }

            // 插入分组数据（用于测试行号折叠功能）
            var wuYanzu = new TestClass(1000, 0, "吴彦祖", 50)
            {
                Sub = new List<TestClass>()
            };
            list.Add(wuYanzu);

            var wuWife = new TestClass(1001, 0, "Lisa.S（老婆）", 47);
            wuYanzu.Sub.Add(wuWife);

            var wuDaughter = new TestClass(1002, 0, "吴斐然（女儿）", 10)
            {
                Sub = new List<TestClass>()
            };

            var pet1 = new TestClass(1011, 0, "旺财（宠物狗）", 3);
            var pet2 = new TestClass(1012, 0, "咪咪（宠物猫）", 2);
            wuDaughter.Sub.Add(pet1);
            wuDaughter.Sub.Add(pet2);
            wuYanzu.Sub.Add(wuDaughter);

            var wuFan1 = new TestClass(1003, 0, "粉丝团成员1", 28);
            var wuFan2 = new TestClass(1004, 0, "粉丝团成员2", 25);
            wuYanzu.Sub.Add(wuFan1);
            wuYanzu.Sub.Add(wuFan2);

            var huYanbin = new TestClass(2000, 0, "胡彦斌", 41)
            {
                Sub = new List<TestClass>()
            };
            list.Add(huYanbin);

            var huExGirlfriend = new TestClass(2001, 0, "郑爽（前女友）", 32)
            {
                Sub = new List<TestClass>()
            };

            var affair1 = new TestClass(2011, 0, "绯闻女友A", 28);
            var affair2 = new TestClass(2012, 0, "绯闻女友B", 26);
            huExGirlfriend.Sub.Add(affair1);
            huExGirlfriend.Sub.Add(affair2);
            huYanbin.Sub.Add(huExGirlfriend);


            var huSon = new TestClass(2002, 0, "胡小宝（儿子）", 8)
            {
                Sub = new List<TestClass>()
            };

            var classmate1 = new TestClass(2021, 0, "小明（同学）", 8);
            var classmate2 = new TestClass(2022, 0, "小红（同学）", 8);
            huSon.Sub.Add(classmate1);
            huSon.Sub.Add(classmate2);
            huYanbin.Sub.Add(huSon);


            var huWorks = new TestClass(2003, 0, "音乐团队", 35)
            {
                Sub = new List<TestClass>()
            };
            var music1 = new TestClass(2031, 0, "编曲师", 38);
            var music2 = new TestClass(2032, 0, "吉他手", 32);
            huWorks.Sub.Add(music1);
            huWorks.Sub.Add(music2);
            huYanbin.Sub.Add(huWorks);


            var liXian = new TestClass(3000, 0, "李现（单身）", 33);
            list.Add(liXian);


            var eddiePeng = new TestClass(3100, 0, "彭于晏", 42)
            {
                Sub = new List<TestClass>()
            };
            list.Add(eddiePeng);


            var eddieMother = new TestClass(3101, 0, "彭妈妈", 70)
            {
                Sub = new List<TestClass>()
            };


            var eddieMotherFriend1 = new TestClass(4001, 0, "陈阿姨（妈妈的朋友）", 68)
            {
                Sub = new List<TestClass>()
            };
            var eddieMotherFriend2 = new TestClass(4002, 0, "李阿姨（妈妈的朋友）", 65);


            var friendChild1 = new TestClass(5001, 0, "儿子小王", 25);
            var friendChild2 = new TestClass(5002, 0, "女儿小张", 23);
            eddieMotherFriend1.Sub.Add(friendChild1);
            eddieMotherFriend1.Sub.Add(friendChild2);

            eddieMother.Sub.Add(eddieMotherFriend1);
            eddieMother.Sub.Add(eddieMotherFriend2);

            var eddieSister = new TestClass(3102, 0, "彭妹妹（姐姐）", 40)
            {
                Sub = new List<TestClass>()
            };
            var niece = new TestClass(3103, 0, "外甥小彭", 12);
            eddieSister.Sub.Add(niece);

            eddiePeng.Sub.Add(eddieMother);
            eddiePeng.Sub.Add(eddieSister);

            return list;
        }

        object GetPageSummaryData(object[] source)
        {
            if (source == null) return null;
            int totalAge = 0;
            foreach (TestClass item in source) totalAge += item.age;
            return new
            {
                name = "平均年龄",
                age = totalAge / source.Length
            };
        }

        void pagination1_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            table1.DataSource = GetPageData(e.Current, e.PageSize);
            SummarySet();
        }
        string pagination1_ShowTotalChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            return $"{e.PageSize} / {e.Total}条 {e.PageTotal}页";
        }

        #endregion

        public class TestColumn : AntdUI.TemplateColumn
        {
            public TestColumn(string id, string title) : base(id, title) { }
            public override AntdUI.ICell GetCellValue(object value) => new AntdUI.CellTag(value.ToString(), AntdUI.TTypeMini.Success);
        }
        public enum EHobbies
        {
            读书 = 0,
            旅游 = 1,
            社交 = 2,
            运动 = 3,
        }
        public class TestClass : AntdUI.NotifyProperty
        {
            public TestClass(int index, int start, string name, int age)
            {
                id = (index + 1);
                if (start == 1) _online = new AntdUI.CellBadge(AntdUI.TState.Success, AntdUI.Localization.Get("Table.Data.Online", "在线"));
                else if (start == 2) _online = new AntdUI.CellBadge(AntdUI.TState.Processing, AntdUI.Localization.Get("Table.Data.Online.Processing", "处置"));
                else if (start == 3) _online = new AntdUI.CellBadge(AntdUI.TState.Error, AntdUI.Localization.Get("Table.Data.Online.Error", "离线"));
                else if (start == 4) _online = new AntdUI.CellBadge(AntdUI.TState.Warn, AntdUI.Localization.Get("Table.Data.Online.Warn", "离线"));
                else _online = new AntdUI.CellBadge(AntdUI.TState.Default, AntdUI.Localization.Get("Table.Data.Online.Default", "常规"));
                _name = name;
                _age = age;
                _date = DateTime.Now.Date.AddYears(-age);
                _hobby = new Random().Next(0, 3);
                _address = AntdUI.Localization.GetLangI("Table.Data.Address" + id, null);
                if (_address == null) _address = AntdUI.Localization.GetLangI("Table.Data.AddressNum", null);
                if (_address == null) _address = (new Random().Next(DateTime.Now.Second) > 5 ? "东湖" : "西湖") + "区湖底公园" + id + "号";
                else _address += id;

                _enable = start % 2 == 0;
                if (start == 1)
                {
                    _imgs = new AntdUI.CellImage[] {
                        new AntdUI.CellImage(Properties.Resources.img1).SetBorder(Color.BlueViolet, 4),
                        new AntdUI.CellImage(Properties.Resources.bg1)
                    };
                }

                if (start == 1)
                {
                    _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("id", null, AntdUI.TTypeMini.Primary).SetIcon("SearchOutlined").SetIconHover(Properties.Resources.icon_like),
                        new AntdUI.CellButton("id", null, AntdUI.TTypeMini.Warn).SetIcon("ArrowDownOutlined"),
                        new AntdUI.CellButton("id", null, AntdUI.TTypeMini.Error).SetArrow()
                    };
                }
                else if (start == 2)
                {
                    _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("id").SetBorder().SetIcon("SearchOutlined").SetIconHover(Properties.Resources.icon_like),
                        new AntdUI.CellButton("id").SetBorder().SetIcon("ArrowDownOutlined"),
                        new AntdUI.CellButton("id").SetBorder().SetArrow()
                    };
                }
                else if (start == 3)
                {
                    _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("id").SetBorder().SetGhost().SetIcon("SearchOutlined").SetIconHover(Properties.Resources.icon_like),
                        new AntdUI.CellButton("id").SetBorder().SetGhost().SetIcon("ArrowDownOutlined"),
                        new AntdUI.CellButton("id").SetBorder().SetGhost().SetArrow()
                    };
                }
                else if (start == 4)
                {
                    _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("edit", "Edit", AntdUI.TTypeMini.Primary),
                        new AntdUI.CellButton("delete", "Delete", AntdUI.TTypeMini.Error)
                    };
                }
                else if (start == 5)
                {
                    _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("edit", "Edit", AntdUI.TTypeMini.Primary).SetBorder().SetGhost(),
                        new AntdUI.CellButton("delete", "Delete", AntdUI.TTypeMini.Error).SetBorder().SetGhost()
                    };
                }
                else if (start == 6)
                {
                    _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("download", "Download", AntdUI.TTypeMini.Success).SetIcon("DownloadOutlined")
                    };
                }
                else _btns = new AntdUI.CellLink[] { new AntdUI.CellLink("delete", "Delete") };
            }

            public int id { get; set; }

            bool _check = false;
            public bool check
            {
                get => _check;
                set
                {
                    if (_check == value) return;
                    _check = value;
                    OnPropertyChanged();
                }
            }

            bool _radio = false;
            public bool radio
            {
                get => _radio;
                set
                {
                    if (_radio == value) return;
                    _radio = value;
                    OnPropertyChanged();
                }
            }

            bool _checkTitle = false;
            public bool checkTitle
            {
                get => _checkTitle;
                set
                {
                    if (_checkTitle == value) return;
                    _checkTitle = value;
                    OnPropertyChanged();
                }
            }

            string _name;
            public string name
            {
                get => _name;
                set
                {
                    if (_name == value) return;
                    _name = value;
                    OnPropertyChanged();
                }
            }

            AntdUI.CellBadge _online;
            public AntdUI.CellBadge online
            {
                get => _online;
                set
                {
                    _online = value;
                    OnPropertyChanged();
                }
            }

            bool _enable = false;
            public bool enable
            {
                get => _enable;
                set
                {
                    if (_enable == value) return;
                    _enable = value;
                    OnPropertyChanged();
                }
            }

            int _age;
            public int age
            {
                get => _age;
                set
                {
                    if (_age == value) return;
                    _age = value;
                    OnPropertyChanged();
                }
            }
            int _hobby;
            public int hobby
            {
                get => _hobby;
                set
                {
                    if (_hobby == value) return;
                    _hobby = value;
                    OnPropertyChanged();
                }
            }
            DateTime _date;
            public DateTime date
            {
                get => _date;
                set
                {
                    if (_date == value) return;
                    _date = value;
                    OnPropertyChanged();
                }
            }

            string _address;
            public string address
            {
                get => _address;
                set
                {
                    if (_address == value) return;
                    _address = value;
                    OnPropertyChanged();
                }
            }

            AntdUI.CellTag[] _tag;
            public AntdUI.CellTag[] tag
            {
                get => _tag;
                set
                {
                    _tag = value;
                    OnPropertyChanged();
                }
            }

            AntdUI.CellImage[] _imgs;
            public AntdUI.CellImage[] imgs
            {
                get => _imgs;
                set
                {
                    _imgs = value;
                    OnPropertyChanged();
                }
            }

            AntdUI.CellLink[] _btns;
            public AntdUI.CellLink[] btns
            {
                get => _btns;
                set
                {
                    _btns = value;
                    OnPropertyChanged();
                }
            }

            List<TestClass> _sub;
            public List<TestClass> Sub
            {
                get => _sub;
                set
                {
                    _sub = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
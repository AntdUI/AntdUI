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
    public partial class TableAOT : UserControl
    {
        Form form;
        public TableAOT(Form _form)
        {
            form = _form;
            InitializeComponent();

            #region Table

            table1.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("check").SetFixed(),
                new AntdUI.Column("name", "姓名").SetFixed().SetLocalizationTitleID("Table.Column."),
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
                new AntdUI.Column("age", "年龄").SetAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("address", "住址").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("date", "日期").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("tag", "Tag"),
                new AntdUI.Column("imgs", "图片").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("btns", "操作").SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
            };

            table1.DataSource = GetPageData(pagination1.Current, pagination1.PageSize);
            pagination1.PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };

            #endregion

            selectEditMode.Items.AddRange(EnumList("Click", "DoubleClick"));

            selectEditStyle.Items.AddRange(EnumList("Full", "Excel"));

            selectFocusedStyle.Items.AddRange(EnumList("Dash", "Solid"));
        }

        AntdUI.SelectItem[] EnumList(params string[] data)
        {
            var lists = new List<AntdUI.SelectItem>(data.Length);
            foreach (var it in data) lists.Add(new AntdUI.SelectItem(it));
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
            if (e.Value is string v)
            {
                switch (v)
                {
                    case "Click":
                        table1.EditMode = AntdUI.TEditMode.Click;
                        break;
                    case "DoubleClick":
                        table1.EditMode = AntdUI.TEditMode.DoubleClick;
                        break;
                }
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
            if (e.Value is string v)
            {
                switch (v)
                {
                    case "Full":
                        table1.EditInputStyle = AntdUI.TEditInputStyle.Full;
                        break;
                    case "Excel":
                        table1.EditInputStyle = AntdUI.TEditInputStyle.Excel;
                        break;
                }
            }
            else table1.EditInputStyle = AntdUI.TEditInputStyle.Default;
        }

        private void selectFocusedStyle_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is string v)
            {
                switch (v)
                {
                    case "Dash":
                        table1.CellFocusedStyle = AntdUI.TableCellFocusedStyle.Dash;
                        break;
                    case "Solid":
                        table1.CellFocusedStyle = AntdUI.TableCellFocusedStyle.Solid;
                        break;
                }
            }
            else table1.CellFocusedStyle = AntdUI.TableCellFocusedStyle.None;
        }

        bool table1_CellBeginEdit(object sender, AntdUI.TableEventArgs e)
        {
            if (e.Column == null) return true;
            if (e.Column.Key == "tag" || e.Column.Key == "btns" || e.Column.Key == "imgs") return false;
            return true;
        }

        #endregion

        #region 事件

        void table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Column == null) return;
            if (e.RowIndex > 0 && e.Record is IList<AntdUI.AntItem> data)
            {
                if (e.Column.Key == "online") AntdUI.Popover.open(new AntdUI.Popover.Config(table1, "演示一下能弹出自定义") { Offset = e.Rect });
                else if (e.Column.Key == "tag")
                {
                    var tag = data[e.ColumnIndex + (checkRowsDragSort.Checked ? 0 : 1)];
                    if (tag.value is AntdUI.CellTag[] tags)
                    {
                        if (tags.Length == 1)
                        {
                            if (tags[0].Text == "ERROR") tag.value = new AntdUI.CellTag[] { new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) };
                            else
                            {
                                tags[0].Type = AntdUI.TTypeMini.Error;
                                tags[0].Text = "ERROR";
                            }
                        }
                        else tag.value = new AntdUI.CellTag[] { new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) };
                    }
                    else tag.value = new AntdUI.CellTag[] { new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) };
                }
            }
        }

        void table1_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {
            if (e.Record is IList<AntdUI.AntItem> data)
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
                        new AntdUI.Modal.TextLine(data[2].value.ToString(),AntdUI.Style.Db.Primary),
                        new AntdUI.Modal.TextLine(data[8].value.ToString(),6,AntdUI.Style.Db.TextSecondary)
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

        #endregion

        #region 分页与数据

        object GetPageData(int current, int pageSize)
        {
            var list = new List<AntdUI.AntItem[]>(pageSize);
            int start = Math.Abs(current - 1) * pageSize;
            DateTime now = DateTime.Now;
            DateTime birthday_TigerHu = new DateTime(1983, 7, 4);//数据来源：https://baike.baidu.com/link?url=7UWQOAPtuaXDjkFQZ92-cNlijS9voNgkQEJSmPLDV73RX1RLogXTLRQdBIQ6KMO7s1nIEZDKjvCJXa_e9fOrrhA9HxRDvRbkgGvPdWYMmP7
            DateTime birthday_DanielWu = new DateTime(1974, 9, 30);//数据来源：https://baike.baidu.com/link?url=zk3KO7qvnfny-fZ2QfgQ2-lZleCeNUaCfketfcE6Ur5p_LowHOhlttu0c4tEXDKN673QcgpSRRRUmymic58Rf5NiUpsMJrctl1SXaR2RXuu

            list.Add(GetItemOne(start, 0, AntdUI.Localization.Get("Table.Data.Name1", "胡彦斌"), (int)Math.Round((now - birthday_TigerHu).TotalDays / 365)));
            list.Add(GetItemOne(start + 1, 1, AntdUI.Localization.Get("Table.Data.Name2", "吴彦祖"), (int)Math.Round((now - birthday_DanielWu).TotalDays / 365),
                new AntdUI.CellTag[] { new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info)
                }));

            for (int i = 2; i < pageSize; i++)
            {
                int index = start + i;
                list.Add(GetItemOne(index, i, AntdUI.Localization.Get("Table.Data.Name3", "胡彦祖"), 20 + index));
            }
            return list;
        }

        void pagination1_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            table1.DataSource = GetPageData(e.Current, e.PageSize);
        }
        string pagination1_ShowTotalChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            return $"{e.PageSize} / {e.Total}条 {e.PageTotal}页";
        }

        AntdUI.AntItem[] GetItemOne(int index, int start, string name, int age, AntdUI.CellTag[] tag = null)
        {
            int id = (index + 1);

            AntdUI.CellBadge online = null;
            AntdUI.CellImage[] imgs = null;
            AntdUI.CellLink[] btns = null;

            if (start == 1) online = new AntdUI.CellBadge(AntdUI.TState.Success, AntdUI.Localization.Get("Table.Data.Online", "在线"));
            else if (start == 2) online = new AntdUI.CellBadge(AntdUI.TState.Processing, AntdUI.Localization.Get("Table.Data.Online.Processing", "处置"));
            else if (start == 3) online = new AntdUI.CellBadge(AntdUI.TState.Error, AntdUI.Localization.Get("Table.Data.Online.Error", "离线"));
            else if (start == 4) online = new AntdUI.CellBadge(AntdUI.TState.Warn, AntdUI.Localization.Get("Table.Data.Online.Warn", "离线"));
            else online = new AntdUI.CellBadge(AntdUI.TState.Default, AntdUI.Localization.Get("Table.Data.Online.Default", "常规"));


            var address = AntdUI.Localization.GetLangI("Table.Data.Address" + id, null);
            if (address == null) address = AntdUI.Localization.GetLangI("Table.Data.AddressNum", null);
            if (address == null) address = (new Random().Next(DateTime.Now.Second) > 5 ? "东湖" : "西湖") + "区湖底公园" + id + "号";
            else address += id;

            if (start == 1)
            {
                imgs = new AntdUI.CellImage[] {
                    new AntdUI.CellImage(Properties.Resources.img1).SetBorder(Color.BlueViolet, 4),
                    new AntdUI.CellImage(Properties.Resources.bg1)
                };
            }

            if (start == 1)
            {
                btns = new AntdUI.CellLink[] {
                    new AntdUI.CellButton("id", null, AntdUI.TTypeMini.Primary).SetIcon("SearchOutlined").SetIconHover(Properties.Resources.icon_like),
                    new AntdUI.CellButton("id", null, AntdUI.TTypeMini.Warn).SetIcon("ArrowDownOutlined"),
                    new AntdUI.CellButton("id", null, AntdUI.TTypeMini.Error).SetArrow()
                };
            }
            else if (start == 2)
            {
                btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("id").SetBorder().SetIcon("SearchOutlined").SetIconHover(Properties.Resources.icon_like),
                        new AntdUI.CellButton("id").SetBorder().SetIcon("ArrowDownOutlined"),
                        new AntdUI.CellButton("id").SetBorder().SetArrow()
                    };
            }
            else if (start == 3)
            {
                btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("id").SetBorder().SetGhost().SetIcon("SearchOutlined").SetIconHover(Properties.Resources.icon_like),
                        new AntdUI.CellButton("id").SetBorder().SetGhost().SetIcon("ArrowDownOutlined"),
                        new AntdUI.CellButton("id").SetBorder().SetGhost().SetArrow()
                    };
            }
            else if (start == 4)
            {
                btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("edit", "Edit", AntdUI.TTypeMini.Primary),
                        new AntdUI.CellButton("delete", "Delete", AntdUI.TTypeMini.Error)
                    };
            }
            else if (start == 5)
            {
                btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("edit", "Edit", AntdUI.TTypeMini.Primary).SetBorder().SetGhost(),
                        new AntdUI.CellButton("delete", "Delete", AntdUI.TTypeMini.Error).SetBorder().SetGhost()
                    };
            }
            else if (start == 6)
            {
                btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("download", "Download", AntdUI.TTypeMini.Success).SetIcon("DownloadOutlined")
                    };
            }
            else btns = new AntdUI.CellLink[] { new AntdUI.CellLink("delete", "Delete") };

            return new AntdUI.AntItem[]{
                new AntdUI.AntItem("no", id),
                new AntdUI.AntItem("check", false),
                new AntdUI.AntItem("name", name),
                new AntdUI.AntItem("checkTitle", false),
                new AntdUI.AntItem("radio", false),
                new AntdUI.AntItem("online", online),
                new AntdUI.AntItem("enable", start % 2 == 0),
                new AntdUI.AntItem("age", DateTime.Now.Date.AddYears(-age)),
                new AntdUI.AntItem("address", address),
                new AntdUI.AntItem("date",DateTime.Now.Date.AddYears(-new System.Random().Next(DateTime.Now.Second))),
                new AntdUI.AntItem("tag",tag),
                new AntdUI.AntItem("imgs", imgs),
                new AntdUI.AntItem("btns", btns),
            };
        }

        #endregion
    }
}
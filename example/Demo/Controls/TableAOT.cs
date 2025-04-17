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
                new AntdUI.Column("name", "����").SetFixed().SetLocalizationTitleID("Table.Column."),
                new AntdUI.ColumnCheck("checkTitle", "��ȫѡ����").SetColAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.ColumnRadio("radio", "��ѡ").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("online", "״̬", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.ColumnSwitch("enable", "����", AntdUI.ColumnAlign.Center)
                {
                    LocalizationTitle ="Table.Column.{id}",
                    Call = (value, record, i_row, i_col) => {
                        System.Threading.Thread.Sleep(2000);
                        return value;
                    }
                },
                new AntdUI.Column("age", "����", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("address", "סַ").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("tag", "Tag"),
                new AntdUI.Column("imgs", "ͼƬ").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("btns", "����").SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
            };

            table1.DataSource = GetPageData(pagination1.Current, pagination1.PageSize);
            pagination1.PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };

            #endregion
        }

        #region ʾ��

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

        #region ��ż��

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
            if (table1.Columns != null) table1.Columns[6].SortOrder = table1.Columns[7].SortOrder = e.Value;
        }

        void checkEnableHeaderResizing_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.EnableHeaderResizing = e.Value;
        }

        void checkVisibleHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.VisibleHeader = e.Value;
        }

        private void checkAddressLineBreak_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value) table1.Columns[7].Width = "120";
            else table1.Columns[7].Width = null;
            table1.Columns[7].LineBreak = e.Value;
        }

        #endregion

        #region ���/˫��

        void table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Record is IList<AntdUI.AntItem> data)
            {
                if (e.RowIndex > 0 && e.ColumnIndex == 6) AntdUI.Popover.open(new AntdUI.Popover.Config(table1, "��ʾһ���ܵ����Զ���") { Offset = e.Rect });
                else if (e.RowIndex > 0 && e.ColumnIndex == 8)
                {
                    var tag = data[9];
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
                if (AntdUI.Modal.open(new AntdUI.Modal.Config(form, "�Ƿ�ɾ��", new AntdUI.Modal.TextLine[] {
                    new AntdUI.Modal.TextLine(data[2].value.ToString(),AntdUI.Style.Db.Primary),
                    new AntdUI.Modal.TextLine(data[8].value.ToString(),6,AntdUI.Style.Db.TextSecondary)
                }, AntdUI.TType.Error)
                {
                    CancelText = null,
                    OkType = AntdUI.TTypeMini.Error,
                    OkText = "ɾ��"
                }) == DialogResult.OK)
                {
                    table1.Spin(AntdUI.Localization.Get("Loading2", "���ڼ�����..."), config =>
                    {
                        System.Threading.Thread.Sleep(1000);
                        for (int i = 0; i < 101; i++)
                        {
                            config.Value = i / 100F;
                            config.Text = AntdUI.Localization.Get("Processing", "������") + " " + i + "%";
                            System.Threading.Thread.Sleep(20);
                        }
                        System.Threading.Thread.Sleep(1000);
                        config.Value = null;
                        config.Text = AntdUI.Localization.Get("PleaseWait", "�����ĵȺ�...");
                        System.Threading.Thread.Sleep(2000);
                    }, () =>
                    {
                        System.Diagnostics.Debug.WriteLine("���ؽ���");
                    });
                }
            }
        }

        #endregion

        #region ��ҳ������

        object GetPageData(int current, int pageSize)
        {
            var list = new List<AntdUI.AntItem[]>(pageSize);
            int start = Math.Abs(current - 1) * pageSize;
            DateTime now = DateTime.Now;
            DateTime birthday_TigerHu = new DateTime(1983, 7, 4);//������Դ��https://baike.baidu.com/link?url=7UWQOAPtuaXDjkFQZ92-cNlijS9voNgkQEJSmPLDV73RX1RLogXTLRQdBIQ6KMO7s1nIEZDKjvCJXa_e9fOrrhA9HxRDvRbkgGvPdWYMmP7
            DateTime birthday_DanielWu = new DateTime(1974, 9, 30);//������Դ��https://baike.baidu.com/link?url=zk3KO7qvnfny-fZ2QfgQ2-lZleCeNUaCfketfcE6Ur5p_LowHOhlttu0c4tEXDKN673QcgpSRRRUmymic58Rf5NiUpsMJrctl1SXaR2RXuu

            list.Add(new AntdUI.AntItem[]{
                new AntdUI.AntItem("no",start),
                new AntdUI.AntItem("check",false),
                new AntdUI.AntItem("name","�����"),
                new AntdUI.AntItem("checkTitle",false),
                new AntdUI.AntItem("radio",false),
                new AntdUI.AntItem("online", new AntdUI.CellBadge(AntdUI.TState.Success, "����")),
                    new AntdUI.AntItem("enable",false),
                    new AntdUI.AntItem("age",(int)Math.Round((now - birthday_TigerHu).TotalDays / 365)),
                    new AntdUI.AntItem("address", "���������׹�԰" + (start+1) + "��"),
                    new AntdUI.AntItem("tag"),
                    new AntdUI.AntItem("imgs",new AntdUI.CellImage[] {
                        new AntdUI.CellImage(Properties.Resources.img1){ BorderWidth=4,BorderColor=Color.BlueViolet},
                        new AntdUI.CellImage(Properties.Resources.bg1)
                    }),
                    new AntdUI.AntItem("btns", new AntdUI.CellLink[] {
                        new AntdUI.CellLink("delete","Delete")
                    }),
            });
            list.Add(new AntdUI.AntItem[]{
                    new AntdUI.AntItem("no",start + 1),
                    new AntdUI.AntItem("check",false),
                    new AntdUI.AntItem("name","������"),
                    new AntdUI.AntItem("checkTitle",false),
                    new AntdUI.AntItem("radio",false),
                    new AntdUI.AntItem("online",new AntdUI.CellBadge(AntdUI.TState.Processing, "����")),
                    new AntdUI.AntItem("enable",false),
                    new AntdUI.AntItem("age",(int)Math.Round((now - birthday_DanielWu).TotalDays / 365)),
                    new AntdUI.AntItem("address", "���������׹�԰" + (start + 1+1) + "��"),
                    new AntdUI.AntItem("tag",new AntdUI.CellTag[]{ new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) }),
                    new AntdUI.AntItem("imgs"),
                    new AntdUI.AntItem("btns", new AntdUI.CellLink[] {
                        new AntdUI.CellButton("b1") { BorderWidth=1, IconSvg="SearchOutlined",IconHoverSvg=Properties.Resources.icon_like,ShowArrow=true},
                        new AntdUI.CellButton("b2") {  ShowArrow=true},
                        new AntdUI.CellButton("b3") { Type= AntdUI.TTypeMini.Primary, IconSvg="SearchOutlined" }
                    }),
                });

            for (int i = 2; i < pageSize; i++)
            {
                int index = start + i;
                list.Add(GetItemOne(index, i, "������", 20 + index));
            }
            return list;
        }

        void pagination1_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            table1.DataSource = GetPageData(e.Current, e.PageSize);
        }
        string pagination1_ShowTotalChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            return $"{e.PageSize} / {e.Total}�� {e.PageTotal}ҳ";
        }

        AntdUI.AntItem[] GetItemOne(int index, int start, string name, int age)
        {
            int id = (index + 1);
            AntdUI.CellBadge _online = null;
            AntdUI.CellImage[] _imgs = null;
            AntdUI.CellLink[] _btns = null;

            if (start == 1) _online = new AntdUI.CellBadge(AntdUI.TState.Success, "����");
            else if (start == 2) _online = new AntdUI.CellBadge(AntdUI.TState.Processing, "����");
            else if (start == 3) _online = new AntdUI.CellBadge(AntdUI.TState.Error, "����");
            else if (start == 4) _online = new AntdUI.CellBadge(AntdUI.TState.Warn, "����");
            else _online = new AntdUI.CellBadge(AntdUI.TState.Default, "����");
            if (start == 1)
            {
                _imgs = new AntdUI.CellImage[] {
                    new AntdUI.CellImage(Properties.Resources.img1) { BorderWidth = 4, BorderColor = Color.BlueViolet },
                    new AntdUI.CellImage(Properties.Resources.bg1)
                };
            }

            if (start == 2)
            {
                _btns = new AntdUI.CellLink[] {
                    new AntdUI.CellButton("b1") {
                        BorderWidth = 1,
                        IconSvg = "SearchOutlined",
                        IconHoverSvg = Properties.Resources.icon_like,
                        ShowArrow = true
                    },
                    new AntdUI.CellButton("b2") { ShowArrow = true },
                    new AntdUI.CellButton("b3") { Type = AntdUI.TTypeMini.Primary, IconSvg = "SearchOutlined" }
                };
            }
            else if (start == 3)
            {
                _btns = new AntdUI.CellLink[] {
                    new AntdUI.CellButton("b1", "Border") { BorderWidth = 1 },
                    new AntdUI.CellButton("b2", "GhostBorder") { Ghost = true, BorderWidth = 1, ShowArrow = true, IsLink = true }
                };
            }
            else if (start == 4)
            {
                _btns = new AntdUI.CellLink[] {
                    new AntdUI.CellButton("edit", "Edit", AntdUI.TTypeMini.Primary) { Ghost = true, BorderWidth = 1 },
                    new AntdUI.CellButton("delete", "Delete", AntdUI.TTypeMini.Error) { Ghost = true, BorderWidth = 1 }
                };
            }
            else if (start == 5)
            {
                _btns = new AntdUI.CellLink[] {
                    new AntdUI.CellButton("edit","Edit",AntdUI.TTypeMini.Primary),
                    new AntdUI.CellButton("delete","Delete",AntdUI.TTypeMini.Error)
                };
            }
            else _btns = new AntdUI.CellLink[] { new AntdUI.CellLink("delete", "Delete") };

            return new AntdUI.AntItem[]{
                new AntdUI.AntItem("no", id),
                new AntdUI.AntItem("check", false),
                new AntdUI.AntItem("name", name),
                new AntdUI.AntItem("checkTitle", false),
                new AntdUI.AntItem("radio", false),
                new AntdUI.AntItem("online", _online),
                new AntdUI.AntItem("enable", start % 2 == 0),
                new AntdUI.AntItem("age", age),
                new AntdUI.AntItem("address", "���������׹�԰" + id + "��"),
                new AntdUI.AntItem("tag"),
                new AntdUI.AntItem("imgs", _imgs),
                new AntdUI.AntItem("btns", _btns),
            };
        }

        #endregion
    }
}
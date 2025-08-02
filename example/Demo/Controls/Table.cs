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
    public partial class Table : UserControl
    {
        Form form;
        public Table(Form _form)
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
                new AntdUI.Column("age", "����").SetAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("address", "סַ").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("date", "����").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("tag", "Tag"),
                new AntdUI.Column("imgs", "ͼƬ").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("btns", "����").SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
            };

            table1.DataSource = GetPageData(pagination1.Current, pagination1.PageSize);
            pagination1.PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };

            #endregion

            Array editMode = Enum.GetValues(typeof(AntdUI.TEditMode));
            var editModes = new List<AntdUI.SelectItem>(editMode.Length);
            foreach (var it in editMode) editModes.Add(new AntdUI.SelectItem(it));
            editModes.RemoveAt(0);
            selectEditMode.Items.AddRange(editModes.ToArray());

            Array editStyle = Enum.GetValues(typeof(AntdUI.TEditInputStyle));
            var editStyles = new List<AntdUI.SelectItem>(editStyle.Length);
            foreach (var it in editStyle) editStyles.Add(new AntdUI.SelectItem(it));
            editStyles.RemoveAt(0);
            selectEditStyle.Items.AddRange(editStyles.ToArray());
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

        bool table1_CellBeginEdit(object sender, AntdUI.TableEventArgs e)
        {
            if (e.Column == null) return true;
            if (e.Column.Key == "tag" || e.Column.Key == "btns" || e.Column.Key == "imgs") return false;
            return true;
        }

        #endregion

        #region �¼�

        void table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Column == null) return;
            if (e.RowIndex > 0 && e.Record is TestClass data)
            {
                if (e.Column.Key == "online") AntdUI.Popover.open(new AntdUI.Popover.Config(table1, "��ʾһ���ܵ����Զ���") { Offset = e.Rect });
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
                    if (AntdUI.Modal.open(new AntdUI.Modal.Config(form, "�Ƿ�ɾ��", new AntdUI.Modal.TextLine[] {
                        new AntdUI.Modal.TextLine(data.name,AntdUI.Style.Db.Primary),
                        new AntdUI.Modal.TextLine(data.address,6,AntdUI.Style.Db.TextSecondary)
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
        }

        int tmpRowIndex = -1;
        float tmpProg = 0F;
        void table1_RowPaint(object sender, AntdUI.TablePaintRowEventArgs e)
        {
            if (tmpRowIndex == e.RowIndex && tmpProg > 0) e.g.Fill(AntdUI.Style.rgba(AntdUI.Style.Db.SuccessBorder, tmpProg), new Rectangle(e.Rect.X, e.Rect.Y, (int)(e.Rect.Width * tmpProg), e.Rect.Height));
        }

        void table1_FilterPopupEnd(object sender, AntdUI.TableFilterPopupEndEventArgs e)
        {
            AntdUI.Notification.info(form, "ɸѡ���", $"��ɸѡ�� {(e.Records == null ? 0 : e.Records.Length)} �������", AntdUI.TAlignFrom.Top);
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

        #endregion

        #region ��ҳ������

        object GetPageData(int current, int pageSize)
        {
            var list = new List<TestClass>(pageSize);
            int start = Math.Abs(current - 1) * pageSize;
            DateTime now = DateTime.Now;
            DateTime birthday_TigerHu = new DateTime(1983, 7, 4);//������Դ��https://baike.baidu.com/link?url=7UWQOAPtuaXDjkFQZ92-cNlijS9voNgkQEJSmPLDV73RX1RLogXTLRQdBIQ6KMO7s1nIEZDKjvCJXa_e9fOrrhA9HxRDvRbkgGvPdWYMmP7
            DateTime birthday_DanielWu = new DateTime(1974, 9, 30);//������Դ��https://baike.baidu.com/link?url=zk3KO7qvnfny-fZ2QfgQ2-lZleCeNUaCfketfcE6Ur5p_LowHOhlttu0c4tEXDKN673QcgpSRRRUmymic58Rf5NiUpsMJrctl1SXaR2RXuu

            list.Add(new TestClass(start, 0, AntdUI.Localization.Get("Table.Data.Name1", "�����"), (int)Math.Round((now - birthday_TigerHu).TotalDays / 365)));
            list.Add(new TestClass(start + 1, 1, AntdUI.Localization.Get("Table.Data.Name2", "������"), (int)Math.Round((now - birthday_DanielWu).TotalDays / 365))
            {
                tag = new AntdUI.CellTag[] { new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) }
            });
            for (int i = 2; i < pageSize; i++)
            {
                int index = start + i;
                list.Add(new TestClass(index, i, AntdUI.Localization.Get("Table.Data.Name3", "������"), 20 + index));
            }
            return list;
        }

        object GetPageSummaryData(object[] source)
        {
            if (source == null) return null;
            int totalAge = 0;
            foreach (TestClass item in source) totalAge += item.age;
            return new
            {
                name = "ƽ������",
                age = totalAge / source.Length
            };
        }

        void pagination1_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            table1.DataSource = GetPageData(e.Current, e.PageSize);
        }
        string pagination1_ShowTotalChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            return $"{e.PageSize} / {e.Total}�� {e.PageTotal}ҳ";
        }

        #endregion

        public class TestColumn : AntdUI.TemplateColumn
        {
            public TestColumn(string id, string title) : base(id, title) { }
            public override AntdUI.ICell GetCellValue(object value) => new AntdUI.CellTag(value.ToString(), AntdUI.TTypeMini.Success);
        }

        public class TestClass : AntdUI.NotifyProperty
        {
            public TestClass(int index, int start, string name, int age)
            {
                id = (index + 1);
                if (start == 1) _online = new AntdUI.CellBadge(AntdUI.TState.Success, AntdUI.Localization.Get("Table.Data.Online", "����"));
                else if (start == 2) _online = new AntdUI.CellBadge(AntdUI.TState.Processing, AntdUI.Localization.Get("Table.Data.Online.Processing", "����"));
                else if (start == 3) _online = new AntdUI.CellBadge(AntdUI.TState.Error, AntdUI.Localization.Get("Table.Data.Online.Error", "����"));
                else if (start == 4) _online = new AntdUI.CellBadge(AntdUI.TState.Warn, AntdUI.Localization.Get("Table.Data.Online.Warn", "����"));
                else _online = new AntdUI.CellBadge(AntdUI.TState.Default, AntdUI.Localization.Get("Table.Data.Online.Default", "����"));
                _name = name;
                _age = age;
                _date = DateTime.Now.Date.AddYears(-age);

                _address = AntdUI.Localization.GetLangI("Table.Data.Address" + id, null);
                if (_address == null) _address = AntdUI.Localization.GetLangI("Table.Data.AddressNum", null);
                if (_address == null) _address = (new Random().Next(DateTime.Now.Second) > 5 ? "����" : "����") + "�����׹�԰" + id + "��";
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
        }
    }
}
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
using System.Drawing;
using System.Windows.Forms;

namespace Overview.Controls
{
    public partial class Table : UserControl
    {
        Form form;
        public Table(Form _form)
        {
            form = _form;
            InitializeComponent();

            #region Table 1

            table1.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("check"){ Fixed=true},
                new AntdUI.Column("name","����"){ Fixed=true},
                new AntdUI.ColumnCheck("checkTitle","��ȫѡ����"){ColAlign=AntdUI.ColumnAlign.Center},
                new AntdUI.ColumnRadio("radio","��ѡ"),
                new AntdUI.Column("online","״̬",AntdUI.ColumnAlign.Center),
                new AntdUI.ColumnSwitch("enable","����",AntdUI.ColumnAlign.Center){ Call=(value,record, i_row, i_col)=>{
                    System.Threading.Thread.Sleep(2000);
                    return value;
                } },
                new AntdUI.Column("age","����",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("address","סַ"){ Width="120", LineBreak=true},
                new AntdUI.Column("tag","Tag"),
                new AntdUI.Column("imgs","ͼƬ"),
                new AntdUI.Column("btns","����"){ Fixed=true,Width="auto"},
            };

            // ��ӱ�ͷ����ģ������
            var list = new List<TestClass>(10) {
                new TestClass(1,"1","�����",32,"���������׹�԰1��"),
                new TestClass(2,"2","������",22,"���������׹�԰1��") {
                    tag=new AntdUI.CellTag[]{ new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) }
                },
            };
            // �������
            for (int i = 2; i < 10; i++) list.Add(new TestClass(i, i.ToString(), "�����", 31 + i, "���������׹�԰" + (i + 10) + "��"));

            table1.DataSource = list;

            #endregion

            #region Table 2

            table2.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("name","����"),
                new AntdUI.Column("age","����",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("address","סַ"),
                new AntdUI.Column("tag","Tag"){ Width="auto"}
            };
            table2.DataSource = GetPageData(pagination1.Current, pagination1.PageSize);
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
            if (e.RowIndex % 2 == 0)
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
            if (table1.Columns != null) table1.Columns[4].SortOrder = table1.Columns[5].SortOrder = e.Value;
        }

        void checkEnableHeaderResizing_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.EnableHeaderResizing = e.Value;
        }

        void checkVisibleHeader_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            table1.VisibleHeader = e.Value;
        }

        #endregion

        #region ���/˫��

        void table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Record is TestClass data)
            {
                if (e.RowIndex > 0 && e.ColumnIndex == 6) AntdUI.Popover.open(new AntdUI.Popover.Config(table1, "��ʾһ���ܵ����Զ���") { Offset = e.Rect });
                else if (e.RowIndex > 0 && e.ColumnIndex == 8)
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
                AntdUI.Modal.open(new AntdUI.Modal.Config(form, "�Ƿ�ɾ��", new AntdUI.Modal.TextLine[] {
                    new AntdUI.Modal.TextLine(data.name,AntdUI.Style.Db.Primary),
                    new AntdUI.Modal.TextLine(data.address,6,AntdUI.Style.Db.TextSecondary)
                }, AntdUI.TType.Error)
                {
                    CancelText = null,
                    OkType = AntdUI.TTypeMini.Error,
                    OkText = "ɾ��"
                });
                table1.Spin("���ڼ�����...", () =>
                {
                    System.Threading.Thread.Sleep(2000);
                }, () =>
                {
                    System.Diagnostics.Debug.WriteLine("���ؽ���");
                });
            }
        }

        #endregion

        #region ��ҳ������

        object GetPageData(int current, int pageSize)
        {
            var list = new List<TestClass2>();
            int start = Math.Abs(current - 1) * pageSize;
            for (int i = 0; i < pageSize; i++)
            {
                int index = start + i;
                list.Add(new TestClass2(index, "������" + index, (index + 20), "���������׹�԰" + (index + 1) + "��"));
            }
            return list;
        }

        void pagination1_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            table2.DataSource = GetPageData(e.Current, e.PageSize);
        }
        string pagination1_ShowTotalChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            return $"{e.PageSize} / {e.Total}�� {e.PageTotal}ҳ";
        }

        #endregion

        public class TestClass : AntdUI.NotifyProperty
        {
            public TestClass(int i, string key, string name, int age, string address)
            {
                _key = key;
                if (i == 1) _online = new AntdUI.CellBadge(AntdUI.TState.Success, "����");
                else if (i == 2) _online = new AntdUI.CellBadge(AntdUI.TState.Processing, "����");
                else if (i == 3) _online = new AntdUI.CellBadge(AntdUI.TState.Error, "����");
                else if (i == 4) _online = new AntdUI.CellBadge(AntdUI.TState.Warn, "����");
                else _online = new AntdUI.CellBadge(AntdUI.TState.Default, "����");
                _name = name;
                _age = age;
                _address = address;
                _enable = i % 2 == 0;
                if (i == 1)
                {
                    _imgs = new AntdUI.CellImage[] {
                        new AntdUI.CellImage(Properties.Resources.img1){ BorderWidth=4,BorderColor=Color.BlueViolet},
                        new AntdUI.CellImage(Properties.Resources.bg1)
                    };
                }

                if (i == 2)
                {
                    _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("b1") { BorderWidth=1, IconSvg=Properties.Resources.icon_search,IconHoverSvg=Properties.Resources.icon_like,ShowArrow=true},
                        new AntdUI.CellButton("b2") {  ShowArrow=true},
                        new AntdUI.CellButton("b3") { Type= AntdUI.TTypeMini.Primary, IconSvg=Properties.Resources.icon_search }
                    };
                }
                else if (i == 3)
                {
                    _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("b1","Border") {  BorderWidth=1},
                        new AntdUI.CellButton("b2","GhostBorder") {  Ghost = true,BorderWidth=1,ShowArrow=true,IsLink=true }
                    };
                }
                else if (i == 4)
                {
                    _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("edit","Edit",AntdUI.TTypeMini.Primary) {  Ghost = true,BorderWidth=1 },
                        new AntdUI.CellButton("delete","Delete",AntdUI.TTypeMini.Error) {  Ghost = true ,BorderWidth=1}
                    };
                }
                else if (i == 5)
                {
                    _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("edit","Edit",AntdUI.TTypeMini.Primary),
                        new AntdUI.CellButton("delete","Delete",AntdUI.TTypeMini.Error)
                    };
                }
                else
                {
                    _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellLink("delete","Delete")
                    };
                }
            }

            bool _check = false;
            public bool check
            {
                get => _check;
                set
                {
                    if (_check == value) return;
                    _check = value;
                    OnPropertyChanged("check");
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
                    OnPropertyChanged("radio");
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
                    OnPropertyChanged("checkTitle");
                }
            }

            string _key;
            public string key
            {
                get => _key;
                set
                {
                    if (_key == value) return;
                    _key = value;
                    OnPropertyChanged("key");
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
                    OnPropertyChanged("name");
                }
            }

            AntdUI.CellBadge _online;
            public AntdUI.CellBadge online
            {
                get => _online;
                set
                {
                    _online = value;
                    OnPropertyChanged("online");
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
                    OnPropertyChanged("enable");
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
                    OnPropertyChanged("age");
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
                    OnPropertyChanged("address");
                }
            }

            AntdUI.CellTag[] _tag;
            public AntdUI.CellTag[] tag
            {
                get => _tag;
                set
                {
                    _tag = value;
                    OnPropertyChanged("tag");
                }
            }

            AntdUI.CellImage[] _imgs;
            public AntdUI.CellImage[] imgs
            {
                get => _imgs;
                set
                {
                    _imgs = value;
                    OnPropertyChanged("imgs");
                }
            }

            AntdUI.CellLink[] _btns;
            public AntdUI.CellLink[] btns
            {
                get => _btns;
                set
                {
                    _btns = value;
                    OnPropertyChanged("btns");
                }
            }
        }

        public class TestClass2 : AntdUI.NotifyProperty
        {
            public TestClass2(int i, string name, int age, string address)
            {
                _name = name;
                _age = age;
                _address = address;
                if (i % 2 == 0) _tag = new AntdUI.CellTag("YES" + i, AntdUI.TTypeMini.Success);
                else _tag = new AntdUI.CellTag("NO" + i, AntdUI.TTypeMini.Error);
            }

            string _name;
            public string name
            {
                get => _name;
                set
                {
                    if (_name == value) return;
                    _name = value;
                    OnPropertyChanged("name");
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
                    OnPropertyChanged("age");
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
                    OnPropertyChanged("address");
                }
            }

            AntdUI.CellTag _tag;
            public AntdUI.CellTag tag
            {
                get => _tag;
                set
                {
                    _tag = value;
                    OnPropertyChanged("tag");
                }
            }
        }
    }
}
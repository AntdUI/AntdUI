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

namespace Demo.Controls
{
    public partial class Table : UserControl
    {
        Form form;
        public Table(Form _form)
        {
            form = _form;
            InitializeComponent();
            tabPage1.Text = AntdUI.Localization.Get("Table.Tab1", "常规");
            tabPage2.Text = AntdUI.Localization.Get("Table.Tab2", "分页");

            #region Table 1

            table1.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("check"){ Fixed=true },
                new AntdUI.Column("name","姓名"){ Fixed=true},
                new AntdUI.ColumnCheck("checkTitle","不全选标题"){ColAlign=AntdUI.ColumnAlign.Center},
                new AntdUI.ColumnRadio("radio","单选"),
                new AntdUI.Column("online","状态",AntdUI.ColumnAlign.Center),
                new AntdUI.ColumnSwitch("enable","启用",AntdUI.ColumnAlign.Center){ Call=(value,record, i_row, i_col)=>{
                    System.Threading.Thread.Sleep(2000);
                    return value;
                } },
                new AntdUI.Column("age","年龄",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("address","住址"){ Width="120", LineBreak=true},
                new AntdUI.Column("tag","Tag"),
                new AntdUI.Column("imgs","图片"),
                new AntdUI.Column("btns","操作"){ Fixed=true,Width="auto"},
            };

            // 添加表头，绑定模型名称
            var list = new List<TestClass>(10) {
                new TestClass(1,"1","胡彦斌",32,"西湖区湖底公园1号"),
                new TestClass(2,"2","胡彦祖",22,"西湖区湖底公园1号") {
                    tag=new AntdUI.CellTag[]{ new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) }
                },
            };
            // 添加数据
            for (int i = 2; i < 10; i++) list.Add(new TestClass(i, i.ToString(), "胡彦斌", 31 + i, "西湖区湖底公园" + (i + 10) + "号"));

            table1.DataSource = list;

            #endregion

            #region Table 2

            table2.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("name","姓名"),
                new AntdUI.Column("age","年龄",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("address","住址"),
                new AntdUI.Column("tag","Tag"){ Width="auto"}
            };
            table2.DataSource = GetPageData(pagination1.Current, pagination1.PageSize);
            pagination1.PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };

            #endregion
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

        #region 奇偶列

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

        #endregion

        #region 点击/双击

        void table1_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Record is TestClass data)
            {
                if (e.RowIndex > 0 && e.ColumnIndex == 6) AntdUI.Popover.open(new AntdUI.Popover.Config(table1, "演示一下能弹出自定义") { Offset = e.Rect });
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
                    table1.Spin("正在加载中...", config =>
                    {
                        System.Threading.Thread.Sleep(1000);
                        for (int i = 0; i < 101; i++)
                        {
                            config.Value = i / 100F;
                            config.Text = "处理中 " + i + "%";
                            System.Threading.Thread.Sleep(20);
                        }
                        System.Threading.Thread.Sleep(1000);
                        config.Value = null;
                        config.Text = "请耐心等候...";
                        System.Threading.Thread.Sleep(2000);
                    }, () =>
                    {
                        System.Diagnostics.Debug.WriteLine("加载结束");
                    });
                }
            }
        }

        #endregion

        #region 分页与数据

        object GetPageData(int current, int pageSize)
        {
            var list = new List<TestClass2>();
            int start = Math.Abs(current - 1) * pageSize;
            for (int i = 0; i < pageSize; i++)
            {
                int index = start + i;
                list.Add(new TestClass2(index, "王健林" + index, (index + 20), "西湖区湖底公园" + (index + 1) + "号"));
            }
            return list;
        }

        void pagination1_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            table2.DataSource = GetPageData(e.Current, e.PageSize);
        }
        string pagination1_ShowTotalChanged(object sender, AntdUI.PagePageEventArgs e)
        {
            return $"{e.PageSize} / {e.Total}条 {e.PageTotal}页";
        }

        #endregion

        public class TestClass : AntdUI.NotifyProperty
        {
            public TestClass(int i, string key, string name, int age, string address)
            {
                _key = key;
                if (i == 1) _online = new AntdUI.CellBadge(AntdUI.TState.Success, "在线");
                else if (i == 2) _online = new AntdUI.CellBadge(AntdUI.TState.Processing, "处置");
                else if (i == 3) _online = new AntdUI.CellBadge(AntdUI.TState.Error, "离线");
                else if (i == 4) _online = new AntdUI.CellBadge(AntdUI.TState.Warn, "离线");
                else _online = new AntdUI.CellBadge(AntdUI.TState.Default, "常规");
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
                        new AntdUI.CellButton("b1") { BorderWidth=1, IconSvg="SearchOutlined",IconHoverSvg=Properties.Resources.icon_like,ShowArrow=true},
                        new AntdUI.CellButton("b2") {  ShowArrow=true},
                        new AntdUI.CellButton("b3") { Type= AntdUI.TTypeMini.Primary, IconSvg="SearchOutlined" }
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
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

namespace Overview.Controls
{
    public partial class Table : UserControl
    {
        public Table()
        {
            InitializeComponent();

            table1.Columns = new AntdUI.Column[] {
                new AntdUI.ColumnCheck("check"){ Fixed=true},
                new AntdUI.Column("name","����"){ Fixed=true},
                new AntdUI.Column("online","״̬",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("age","����",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("address","סַ"),
                new AntdUI.Column("tag","Tag"),
                new AntdUI.Column("imgs","ͼƬ"),
                new AntdUI.Column("btns","����"){ Fixed=true},
            };// ��ӱ�ͷ����ģ������

            var list = new List<TestClass>(10) {
                new TestClass(1,"1","�����",32,"���������׹�԰1��"),
                new TestClass(2,"2","������",43,"���������׹�԰1��") {
                    tag=new AntdUI.CellTag[]{ new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) }
                },
            };// �������
            for (int i = 2; i < 10; i++) list.Add(new TestClass(i, i.ToString(), "�����", 100, "���������׹�԰" + (i + 10) + "��"));

            table1.DataSource = list;


            table2.Columns = new AntdUI.Column[] {
                new AntdUI.Column("name","����"),
                new AntdUI.Column("age","����",AntdUI.ColumnAlign.Center),
                new AntdUI.Column("address","סַ"),
                new AntdUI.Column("tag","Tag")
            };

            var list2 = new List<TestClass2>(100);
            for (int i = 0; i < 100; i++) list2.Add(new TestClass2(i, "������" + i, (i + 20), "���������׹�԰" + (i + 1) + "��"));
            table2.DataSource = GetPageData(pagination1.Current, pagination1.PageSize);
        }
        object GetPageData(int current, int pageSize)
        {
            var list2 = new List<TestClass2>();
            int start = Math.Abs(current - 1) * pageSize;
            for (int i = 0; i < pageSize; i++)
            {
                int index = start + i;
                list2.Add(new TestClass2(index, "������" + index, (index + 20), "���������׹�԰" + (index + 1) + "��"));
            }
            return list2;
        }

        private void pagination1_ValueChanged(int current, int total, int pageSize, int pageTotal)
        {
            var list = new List<TestClass> {
                new TestClass(1,"1","�����",32,"���������׹�԰1��"),
                new TestClass(2,"2","������",43,"���������׹�԰1��") {
                    tag=new AntdUI.CellTag[]{ new AntdUI.CellTag("NICE", AntdUI.TTypeMini.Success), new AntdUI.CellTag("DEVELOPER", AntdUI.TTypeMini.Info) }
                },
            };// �������
            for (int i = 2; i < 10; i++) list.Add(new TestClass(i, i.ToString(), "�����", 100, "���������׹�԰" + (i + 10) + "��"));

            list[0].check = list[4].check = true;
            table1.DataSource = list;

            table2.DataSource = GetPageData(current, pageSize);
        }

        private string pagination1_ShowTotalChanged(int current, int total, int pageSize, int pageTotal)
        {
            return $"{pageSize} / {total}�� {pageTotal}ҳ";
        }

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
                    new AntdUI.CellButton("edit","Edit",AntdUI.TTypeMini.Primary) {  Ghost = true,BorderWidth=1 },
                    new AntdUI.CellButton("delete","Delete",AntdUI.TTypeMini.Error) {  Ghost = true ,BorderWidth=1}
                    };
                }
                else if (i == 3)
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

            AntdUI.CellBadge? _online;
            public AntdUI.CellBadge? online
            {
                get => _online;
                set
                {
                    _online = value;
                    OnPropertyChanged("online");
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

            AntdUI.CellTag[]? _tag;
            public AntdUI.CellTag[]? tag
            {
                get => _tag;
                set
                {
                    _tag = value;
                    OnPropertyChanged("tag");
                }
            }

            AntdUI.CellImage[]? _imgs;
            public AntdUI.CellImage[]? imgs
            {
                get => _imgs;
                set
                {
                    _imgs = value;
                    OnPropertyChanged("imgs");
                }
            }

            AntdUI.CellLink[]? _btns;
            public AntdUI.CellLink[]? btns
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

            AntdUI.CellTag? _tag;
            public AntdUI.CellTag? tag
            {
                get => _tag;
                set
                {
                    _tag = value;
                    OnPropertyChanged("tag");
                }
            }
        }

        private void table1_CellButtonClick(object sender, AntdUI.CellLink btn, MouseEventArgs args, object record, int rowIndex, int columnIndex)
        {
            if (record is TestClass data)
            {
                AntdUI.Modal.open(new AntdUI.Modal.Config((Form)Parent, "�Ƿ�ɾ��", new AntdUI.Modal.TextLine[] {
                new AntdUI.Modal.TextLine(data.name,AntdUI.Style.Db.Primary),
                new AntdUI.Modal.TextLine(data.address,6,AntdUI.Style.Db.TextSecondary)
            }, AntdUI.TType.Error)
                {
                    CancelText = null,
                    OkType = AntdUI.TTypeMini.Error,
                    OkText = "ɾ��"
                });
            }
        }
    }
}
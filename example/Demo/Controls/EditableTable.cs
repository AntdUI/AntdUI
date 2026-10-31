// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public class EditableTable : UserControl
    {
        AntdUI.BaseForm form;
        AntdUI.EditableTable editableTable1;
        BindingList<Person> data;

        public EditableTable(AntdUI.BaseForm _form)
        {
            form = _form;
            data = new BindingList<Person>(new List<Person> {
                new Person(1, "张三", 28) { salary = 6500.5m, city = 110000, address = "北京市海淀区" },
                new Person(2, "李四", 36) { salary = 8800m,   city = 310000, address = "上海市浦东新区" },
                new Person(3, "王五", 45) { salary = 12000m,  city = 440100, address = "广州市天河区" }
            });

            var header = new AntdUI.PageHeader
            {
                Text = "EditableTable 可编辑表格",
                Description = "行级编辑：点新增/编辑后整行一起进入编辑态，操作列切换为保存/取消。"
                    + "文本必填/长度、数字范围、日期、下拉（存编码显名称）编辑器与行级校验；姓名填 test 可试服务端拒绝保存。",
                Dock = DockStyle.Top,
                Font = new Font("Microsoft YaHei UI", 12F),
                ShowBack = true,
                Padding = new Padding(0, 0, 0, 10)
            };

            var btnAdd = new AntdUI.Button
            {
                Text = "新增一行",
                Type = AntdUI.TTypeMini.Primary,
                Size = new Size(100, 34),
                Location = new Point(12, 10)
            };
            btnAdd.Click += (s, e) =>
            {
                editableTable1.AddRecord(new Person(data.Count + 1, "", null));
            };

            var btnEditFirst = new AntdUI.Button
            {
                Text = "编辑首行",
                Size = new Size(100, 34),
                Location = new Point(122, 10)
            };
            btnEditFirst.Click += (s, e) =>
            {
                if (data.Count > 0) editableTable1.BeginRowEdit(data[0]);
            };

            var toolbar = new AntdUI.Panel { Dock = DockStyle.Top, Height = 54 };
            toolbar.Controls.Add(btnAdd);
            toolbar.Controls.Add(btnEditFirst);

            editableTable1 = new AntdUI.EditableTable
            {
                Dock = DockStyle.Fill,
                BuiltinActions = true,
                Padding = new Padding(12, 0, 12, 12)
            };
            editableTable1.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("check").SetFixed(),                                             // 复选列（表头全选）
                new AntdUI.Column("id", "编号", AntdUI.ColumnAlign.Center).SetWidth(70),               // 只读列
                new AntdUI.Column("name", "姓名").SetWidth(120),                                       // 文本编辑 + 必填
                new AntdUI.Column("online", "状态", AntdUI.ColumnAlign.Center).SetWidth(90),           // 徽标 CellBadge
                new AntdUI.ColumnSwitch("enable", "启用", AntdUI.ColumnAlign.Center),                  // 开关列
                new AntdUI.Column("age", "年龄").SetWidth(80).SetAlign(),                              // 数字编辑 + 范围
                new AntdUI.Column("salary", "工资").SetWidth(100).SetAlign(),                          // 数字编辑（小数）
                new AntdUI.ColumnSelect("city", "城市").SetItems(GetCities()).SetWidth(100).SetAlign(), // 下拉编辑（存编码显名称）
                new AntdUI.Column("date", "生日", AntdUI.ColumnAlign.Center).SetWidth(110).SetDisplayFormat("yyyy-MM-dd"), // 日期编辑
                new AntdUI.Column("address", "住址").SetWidth(160).SetEllipsis()                       // 文本编辑 + 最大长度
            };
            editableTable1.SetEditor("name", AntdUI.TEditorType.Text).SetRequired(true);
            editableTable1.SetEditor("age", AntdUI.TEditorType.Number).SetRequired(true).SetRange(1, 120);
            editableTable1.SetEditor("salary", AntdUI.TEditorType.Number).SetRange(0, 999999);
            editableTable1.SetEditor("city", AntdUI.TEditorType.Select).SetItems(GetCities().ToArray());
            editableTable1.SetEditor("date", AntdUI.TEditorType.Date);
            editableTable1.SetEditor("address", AntdUI.TEditorType.Text).SetMaxLength(12);

            editableTable1.Binding(data);

            editableTable1.RowSaving += (s, e) =>
            {
                // 演示拦截保存：姓名为 "test" 时拒绝
                if (e.Record is Person p && p.name == "test")
                {
                    e.Cancel = true;
                    AntdUI.Message.warn(form, "服务端校验失败：姓名不能为 test");
                }
            };
            editableTable1.RowSaved += (s, e) => AntdUI.Message.success(form, "已保存");
            editableTable1.RowDeleted += (s, e) => AntdUI.Message.success(form, "已删除");

            Controls.Add(editableTable1);
            Controls.Add(toolbar);
            Controls.Add(header);
            BackColor = AntdUI.Style.Db.BgLayout;
        }

        static List<AntdUI.SelectItem> GetCities() => new List<AntdUI.SelectItem> {
            new AntdUI.SelectItem("北京", 110000),
            new AntdUI.SelectItem("上海", 310000),
            new AntdUI.SelectItem("广州", 440100),
            new AntdUI.SelectItem("深圳", 440300)
        };

        /// <summary>
        /// 与 Table 演示一致的数据类：继承 NotifyProperty（MVVM 属性变更即刷新单元格），
        /// CellBadge 属性值由 Table 自动渲染为模板单元格
        /// </summary>
        public class Person : AntdUI.NotifyProperty
        {
            public Person(int index, string name, int? age)
            {
                id = index;
                _name = name;
                _age = age;
                _date = DateTime.Now.AddDays(-index * 30);
                _enable = index % 2 == 1;
                _online = (index % 3) switch
                {
                    0 => new AntdUI.CellBadge(AntdUI.TState.Success, "在线"),
                    1 => new AntdUI.CellBadge(AntdUI.TState.Processing, "处置中"),
                    _ => new AntdUI.CellBadge(AntdUI.TState.Default, "常规")
                };
            }

            public int id { get; set; }

            bool _check = false;
            public bool check { get => _check; set { if (_check == value) return; _check = value; OnPropertyChanged(); } }

            string _name;
            public string name { get => _name; set { if (_name == value) return; _name = value; OnPropertyChanged(); } }

            AntdUI.CellBadge _online;
            public AntdUI.CellBadge online { get => _online; set { _online = value; OnPropertyChanged(); } }

            bool _enable = false;
            public bool enable { get => _enable; set { if (_enable == value) return; _enable = value; OnPropertyChanged(); } }

            int? _age;
            public int? age { get => _age; set { if (_age == value) return; _age = value; OnPropertyChanged(); } }

            decimal? _salary;
            public decimal? salary { get => _salary; set { if (_salary == value) return; _salary = value; OnPropertyChanged(); } }

            int? _city;
            public int? city { get => _city; set { if (_city == value) return; _city = value; OnPropertyChanged(); } }

            DateTime? _date;
            public DateTime? date { get => _date; set { if (_date == value) return; _date = value; OnPropertyChanged(); } }

            string _address;
            public string address { get => _address; set { if (_address == value) return; _address = value; OnPropertyChanged(); } }
        }
    }
}

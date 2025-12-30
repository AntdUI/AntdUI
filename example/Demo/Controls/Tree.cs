// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Tree : UserControl
    {
        AntdUI.BaseForm form;
        public Tree(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tree1.PauseLayout = tree2.PauseLayout = tree3.PauseLayout = true;
            var it_loading = new AntdUI.TreeItem().SetText("加载动画，点击暂停", "Tree.Loading").SetExpand().SetLoading().SetIcon("PlayCircleOutlined");
            tree1.Items.Add(it_loading);
            AntdUI.ITask.Run(() =>
            {
                var random = new Random();
                for (int i = 0; i < random.Next(7, 20); i++)
                {
                    var it = new AntdUI.TreeItem("Tree1 " + (i + 1)).SetExpand();
                    AddSub(it, 1, random);
                    tree1.Items.Add(it);
                }
                for (int i = 0; i < random.Next(7, 20); i++)
                {
                    var it = new AntdUI.TreeItem("Tree2 " + (i + 1));
                    AddSub(it, 1, random);
                    tree2.Items.Add(it);
                }
                var basepath = new DirectoryInfo(Environment.CurrentDirectory.Substring(0, 3));
                foreach (var item in basepath.GetDirectories())
                {
                    var it = new AntdUI.TreeItem(item.Name).SetName(item.FullName).SetIcon(Properties.Resources.icon_folder).SetCanExpand();
                    tree3.Items.Add(it);
                }
                foreach (var item in basepath.GetFiles())
                {
                    var it = new AntdUI.TreeItem(item.Name).SetName(item.FullName);
                    tree3.Items.Add(it);
                }
            }).ContinueWith(action =>
            {
                tree1.PauseLayout = tree2.PauseLayout = tree3.PauseLayout = false;
            });
        }

        void AddSub(AntdUI.TreeItem it, int d, Random random)
        {
            if (random.Next(0, 10) > 5 && d < 10)
            {
                var list = new List<AntdUI.TreeItem>();
                for (int i = 0; i < random.Next(3, 9); i++)
                {
                    var its = new AntdUI.TreeItem("Sub_" + d + " " + (i + 1)).SetExpand(d < 4);
                    if (d == 1)
                    {
                        int c = random.Next(0, 10);
                        if (c > 6) its.IconSvg = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M396.96 558.24l129.488 128.384a14.288 14.288 0 0 0 20.128 0L818.4 419.136a336.848 336.848 0 0 1 36.448 152.864C854.848 759.712 701.312 912 512 912S169.12 759.712 169.12 572 322.688 232 512 232a343.424 343.424 0 0 1 225.024 83.424L536.64 512.48l-46.736-46.4a66 66 0 0 0-92.912 0 64.832 64.832 0 0 0 0 92.16zM634.192 126.272a14.4 14.4 0 0 0-14.4-14.288H387.824c-7.936 0-14.4 6.4-14.4 14.288v48.96a14.4 14.4 0 0 0 14.352 14.288h232a14.4 14.4 0 0 0 14.4-14.272V126.288z\" fill=\"#9076F8\" p-id=\"1135\"></path><path d=\"M825.648 322.064a36.672 36.672 0 0 1-0.272 51.824l-276.64 274a14.288 14.288 0 0 1-20.16 0l-108.352-108.4a36.64 36.64 0 0 1 51.84-51.776l66.72 66.8 235.04-232.8a36.64 36.64 0 0 1 51.84 0.224v0.128z\" fill=\"#C7B0FF\" p-id=\"1136\"></path></svg>";
                    }
                    if (random.Next(0, 10) > 6) its.Checked = true;
                    AddSub(its, d + 1, random);
                    list.Add(its);
                }
                it.Sub.AddRange(list);
            }
        }


        private void tree3_BeforeExpand(object sender, AntdUI.TreeExpandEventArgs e)
        {
            if (e.Value)
            {
                if (e.Item.Tag is bool)
                {
                    e.Item.IconSvg = Properties.Resources.icon_folderopened;
                    return;
                }
                e.CanExpand = false;
                tree3.PauseLayout = true;
                AntdUI.ITask.Run(() =>
                {
                    e.Item.Tag = true;
                    int count = 0;
                    try
                    {
                        var basepath = new DirectoryInfo(e.Item.Name);
                        foreach (var item in basepath.GetDirectories())
                        {
                            var it = new AntdUI.TreeItem(item.Name).SetName(item.FullName).SetIcon(Properties.Resources.icon_folder).SetCanExpand();
                            e.Item.Sub.Add(it);
                            count++;
                        }
                        foreach (var item in basepath.GetFiles())
                        {
                            var it = new AntdUI.TreeItem(item.Name).SetName(item.FullName);
                            e.Item.Sub.Add(it);
                            count++;
                        }
                    }
                    catch { }
                    if (count == 0) e.Item.CanExpand = false;
                    else
                    {
                        e.Item.IconSvg = Properties.Resources.icon_folderopened;
                        e.Item.Expand = true;
                    }
                }).ContinueWith(action => tree3.PauseLayout = false);
            }
            else e.Item.IconSvg = Properties.Resources.icon_folder;
        }


        private void tree1_NodeMouseClick(object sender, AntdUI.TreeSelectEventArgs e)
        {
            if (e.Item == tree1.Items.First()) e.Item.Loading = !e.Item.Loading;
        }
    }
}
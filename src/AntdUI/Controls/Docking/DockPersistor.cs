// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace AntdUI
{
    /// <summary>
    /// XML save/load for <see cref="DockPanel"/> layouts. Serializes each pane's position,
    /// size ratio, state (Docked / AutoHide / Float), and per-pane contents (PersistId + active index).
    /// Floating windows capture their screen bounds. Resolver hydrates <see cref="IDockContent"/>
    /// instances by <see cref="IDockContent.PersistId"/>.
    /// </summary>
    public static class DockPersistor
    {
        const string FileVersion = "1.0";
        const string Root = "DockLayout";

        /// <summary>Serialize the current layout to an XML string.</summary>
        public static string Save(DockPanel panel)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = false, Encoding = Encoding.UTF8 };
            using (var sw = new StringWriter(sb, CultureInfo.InvariantCulture))
            using (var w = XmlWriter.Create(sw, settings))
            {
                w.WriteStartDocument();
                w.WriteStartElement(Root);
                w.WriteAttributeString("Version", FileVersion);

                // Docked panes
                w.WriteStartElement("Panes");
                for (int i = 0; i < panel.Panes.Count; i++)
                {
                    var p = panel.Panes[i];
                    w.WriteStartElement("Pane");
                    w.WriteAttributeString("Position", p.Position.ToString());
                    w.WriteAttributeString("State", p.State.ToString());
                    w.WriteAttributeString("SizeRatio", p.SizeRatio.ToString("R", CultureInfo.InvariantCulture));
                    w.WriteAttributeString("SelectedIndex", p.SelectedIndex.ToString(CultureInfo.InvariantCulture));
                    for (int j = 0; j < p.Contents.Count; j++)
                    {
                        var c = p.Contents[j];
                        w.WriteStartElement("Content");
                        w.WriteAttributeString("PersistId", c.PersistId ?? string.Empty);
                        w.WriteEndElement();
                    }
                    w.WriteEndElement();
                }
                w.WriteEndElement();

                w.WriteStartElement("AutoHide");
                foreach (var item in panel.Strips()) WriteStrip(w, item.Item1.ToString(), item.Item2);
                w.WriteEndElement();

                // Floats
                w.WriteStartElement("Floats");
                for (int i = 0; i < panel.FloatWindows.Count; i++)
                {
                    var fw = panel.FloatWindows[i];
                    w.WriteStartElement("Float");
                    w.WriteAttributeString("X", fw.Bounds.X.ToString(CultureInfo.InvariantCulture));
                    w.WriteAttributeString("Y", fw.Bounds.Y.ToString(CultureInfo.InvariantCulture));
                    w.WriteAttributeString("W", fw.Bounds.Width.ToString(CultureInfo.InvariantCulture));
                    w.WriteAttributeString("H", fw.Bounds.Height.ToString(CultureInfo.InvariantCulture));
                    for (int j = 0; j < fw.Pane.Contents.Count; j++)
                    {
                        var c = fw.Pane.Contents[j];
                        w.WriteStartElement("Content");
                        w.WriteAttributeString("PersistId", c.PersistId ?? string.Empty);
                        w.WriteEndElement();
                    }
                    w.WriteEndElement();
                }
                w.WriteEndElement();

                w.WriteEndElement(); // Root
                w.WriteEndDocument();
            }
            return sb.ToString();
        }

        static void WriteStrip(XmlWriter w, string name, DockAutoHideStrip strip)
        {
            w.WriteStartElement(name);
            for (int i = 0; i < strip.Contents.Count; i++)
            {
                w.WriteStartElement("Content");
                w.WriteAttributeString("PersistId", strip.Contents[i].PersistId ?? string.Empty);
                w.WriteEndElement();
            }
            w.WriteEndElement();
        }

        /// <summary>Restore a layout. <paramref name="resolver"/> returns the <see cref="IDockContent"/> for a PersistId (or null to skip).</summary>
        public static void Load(DockPanel panel, string xml, Func<string, IDockContent?> resolver)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            if (string.IsNullOrWhiteSpace(xml)) return;

            panel.BeginBatch();
            try
            {
                var existing = new List<IDockContent>();
                for (int i = 0; i < panel.Panes.Count; i++) for (int j = 0; j < panel.Panes[i].Contents.Count; j++) existing.Add(panel.Panes[i].Contents[j]);
                for (int i = 0; i < existing.Count; i++) panel.HideContent(existing[i]);

                using (var sr = new StringReader(xml))
                using (var r = XmlReader.Create(sr))
                {
                    while (r.Read())
                    {
                        if (r.NodeType != XmlNodeType.Element) continue;
                        switch (r.Name)
                        {
                            case "Pane": ReadPane(r, panel, resolver); break;
                            case "Left": ReadStrip(r, panel, resolver, DockPosition.Left); break;
                            case "Right": ReadStrip(r, panel, resolver, DockPosition.Right); break;
                            case "Top": ReadStrip(r, panel, resolver, DockPosition.Top); break;
                            case "Bottom": ReadStrip(r, panel, resolver, DockPosition.Bottom); break;
                            case "Float": ReadFloat(r, panel, resolver); break;
                        }
                    }
                }
            }
            finally { panel.EndBatch(); }
        }

        static void ReadPane(XmlReader r, DockPanel panel, Func<string, IDockContent?> resolver)
        {
            DockPosition pos = ParseEnum(r.GetAttribute("Position"), DockPosition.Right);
            float size = ParseFloat(r.GetAttribute("SizeRatio"), 0.25f);
            int selected = ParseInt(r.GetAttribute("SelectedIndex"), 0);

            var ids = new List<string>();
            if (!r.IsEmptyElement)
            {
                using (var sub = r.ReadSubtree())
                {
                    while (sub.Read())
                    {
                        if (sub.NodeType == XmlNodeType.Element && sub.Name == "Content")
                        {
                            string pid = sub.GetAttribute("PersistId") ?? string.Empty;
                            if (pid.Length > 0) ids.Add(pid);
                        }
                    }
                }
            }

            DockPane? pane = null;
            for (int i = 0; i < ids.Count; i++)
            {
                var c = resolver(ids[i]);
                if (c == null) continue;
                if (pane == null) { pane = panel.AddContent(c, pos, size, true); }
                else { pane.AddContent(c); }
            }
            if (pane != null && selected >= 0 && selected < pane.Contents.Count)
            {
                pane.SelectedIndex = selected;
            }
        }

        static void ReadStrip(XmlReader r, DockPanel panel, Func<string, IDockContent?> resolver, DockPosition edge)
        {
            if (r.IsEmptyElement) return;
            using (var sub = r.ReadSubtree())
            {
                while (sub.Read())
                {
                    if (sub.NodeType == XmlNodeType.Element && sub.Name == "Content")
                    {
                        string pid = sub.GetAttribute("PersistId") ?? string.Empty;
                        if (pid.Length == 0) continue;
                        var c = resolver(pid);
                        if (c == null) continue;
                        panel.AutoHideContent(c, edge);
                    }
                }
            }
        }

        static void ReadFloat(XmlReader r, DockPanel panel, Func<string, IDockContent?> resolver)
        {
            int x = ParseInt(r.GetAttribute("X"), 100);
            int y = ParseInt(r.GetAttribute("Y"), 100);
            int w = ParseInt(r.GetAttribute("W"), 400);
            int h = ParseInt(r.GetAttribute("H"), 300);
            var rect = new System.Drawing.Rectangle(x, y, w, h);

            DockFloatWindow? fw = null;
            if (r.IsEmptyElement) return;
            using (var sub = r.ReadSubtree())
            {
                while (sub.Read())
                {
                    if (sub.NodeType == XmlNodeType.Element && sub.Name == "Content")
                    {
                        string pid = sub.GetAttribute("PersistId") ?? string.Empty;
                        if (pid.Length == 0) continue;
                        var c = resolver(pid);
                        if (c == null) continue;
                        if (fw == null) fw = panel.FloatContent(c, rect);
                        else fw.Pane.AddContent(c);
                    }
                }
            }
        }

        static int ParseInt(string? s, int dflt)
        {
            int v; return (s != null && int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v)) ? v : dflt;
        }

        static float ParseFloat(string? s, float dflt)
        {
            float v; return (s != null && float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v)) ? v : dflt;
        }

        static T ParseEnum<T>(string? s, T dflt) where T : struct
        {
            T v; return (s != null && Enum.TryParse<T>(s, out v)) ? v : dflt;
        }
    }
}

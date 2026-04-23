// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace AntdUI
{
    /// <summary>
    /// Named-layout manager for <see cref="DockPanel"/>. Wraps <see cref="DockPersistor"/> with a
    /// dictionary keyed by layout name (e.g. "Default", "Debug", "Review") plus Apply / Delete /
    /// Names operations and a combined XML blob for disk persistence of the whole set.
    /// </summary>
    public sealed class DockLayoutStore
    {
        const string Root = "DockLayoutStore";
        const string FileVersion = "1.0";

        readonly Dictionary<string, string> layouts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Fires after a named layout is captured, applied, or removed.</summary>
        public event EventHandler<DockLayoutStoreEventArgs>? Changed;

        /// <summary>All layout names. Enumerates the underlying dictionary keys — no allocation per access.</summary>
        public IEnumerable<string> Names => layouts.Keys;

        /// <summary>Number of stored layouts.</summary>
        public int Count => layouts.Count;

        /// <summary>Whether a layout with <paramref name="name"/> exists.</summary>
        public bool Contains(string name) => !string.IsNullOrEmpty(name) && layouts.ContainsKey(name);

        /// <summary>Captures the current <paramref name="panel"/> layout under <paramref name="name"/>, overwriting any existing entry.</summary>
        public void Capture(string name, DockPanel panel)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required", nameof(name));
            if (panel == null) throw new ArgumentNullException(nameof(panel));
            layouts[name] = DockPersistor.Save(panel);
            Changed?.Invoke(this, new DockLayoutStoreEventArgs(name, DockLayoutStoreAction.Captured));
        }

        /// <summary>Applies the named layout to <paramref name="panel"/>. Returns false if the name is unknown.</summary>
        public bool Apply(string name, DockPanel panel, Func<string, IDockContent?> resolver)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            if (!layouts.TryGetValue(name, out var xml)) return false;
            DockPersistor.Load(panel, xml, resolver);
            Changed?.Invoke(this, new DockLayoutStoreEventArgs(name, DockLayoutStoreAction.Applied));
            return true;
        }

        /// <summary>Removes the named layout. Returns false if not present.</summary>
        public bool Remove(string name)
        {
            if (!layouts.Remove(name)) return false;
            Changed?.Invoke(this, new DockLayoutStoreEventArgs(name, DockLayoutStoreAction.Removed));
            return true;
        }

        /// <summary>Clears all stored layouts.</summary>
        public void Clear()
        {
            if (layouts.Count == 0) return;
            layouts.Clear();
            Changed?.Invoke(this, new DockLayoutStoreEventArgs(string.Empty, DockLayoutStoreAction.Cleared));
        }

        /// <summary>Raw XML blob for a named layout (for inspection/migration). Empty string if unknown.</summary>
        public string GetXml(string name) => layouts.TryGetValue(name, out var xml) ? xml : string.Empty;

        /// <summary>Serialize all named layouts to a single XML blob. Round-trips via <see cref="FromXml"/>.</summary>
        public string ToXml()
        {
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = false, Encoding = Encoding.UTF8 };
            using (var sw = new StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture))
            using (var w = XmlWriter.Create(sw, settings))
            {
                w.WriteStartDocument();
                w.WriteStartElement(Root);
                w.WriteAttributeString("Version", FileVersion);
                foreach (var kv in layouts)
                {
                    w.WriteStartElement("Layout");
                    w.WriteAttributeString("Name", kv.Key);
                    w.WriteCData(kv.Value);
                    w.WriteEndElement();
                }
                w.WriteEndElement();
                w.WriteEndDocument();
            }
            return sb.ToString();
        }

        /// <summary>Replaces the current store with layouts parsed from <paramref name="xml"/>.</summary>
        public void FromXml(string xml)
        {
            layouts.Clear();
            if (string.IsNullOrWhiteSpace(xml)) { Changed?.Invoke(this, new DockLayoutStoreEventArgs(string.Empty, DockLayoutStoreAction.Cleared)); return; }

            using (var sr = new StringReader(xml))
            using (var r = XmlReader.Create(sr))
            {
                string? currentName = null;
                while (r.Read())
                {
                    if (r.NodeType == XmlNodeType.Element && r.Name == "Layout")
                    {
                        currentName = r.GetAttribute("Name");
                        if (r.IsEmptyElement) currentName = null;
                    }
                    else if (r.NodeType == XmlNodeType.CDATA && currentName != null)
                    {
                        layouts[currentName] = r.Value;
                        currentName = null;
                    }
                }
            }
            Changed?.Invoke(this, new DockLayoutStoreEventArgs(string.Empty, DockLayoutStoreAction.Loaded));
        }
    }

    /// <summary>Action that triggered a <see cref="DockLayoutStore.Changed"/> event.</summary>
    public enum DockLayoutStoreAction { Captured, Applied, Removed, Cleared, Loaded }

    /// <summary>Event payload for <see cref="DockLayoutStore.Changed"/>.</summary>
    public sealed class DockLayoutStoreEventArgs : EventArgs
    {
        public string Name { get; }
        public DockLayoutStoreAction Action { get; }
        public DockLayoutStoreEventArgs(string name, DockLayoutStoreAction action) { Name = name; Action = action; }
    }
}

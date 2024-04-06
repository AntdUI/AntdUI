// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    public abstract partial class SvgVisualElement
    {
        /// <summary>
        /// Gets or sets a value to determine whether the element will be rendered.
        /// </summary>
        [SvgAttribute("visibility")]
        public virtual bool Visible
        {
            get { return (Attributes["visibility"] == null) ? true : (bool)Attributes["visibility"]; }
            set { Attributes["visibility"] = value; }
        }

        /// <summary>
        /// Gets or sets a value to determine whether the element will be rendered.
        /// Needed to support SVG attribute display="none"
        /// </summary>
        [SvgAttribute("display")]
        public virtual string Display
        {
            get { return Attributes["display"] as string; }
            set { Attributes["display"] = value; }
        }

        // Displayable - false if attribute display="none", true otherwise
        protected virtual bool Displayable
        {
            get
            {
                string checkForDisplayNone = Attributes["display"] as string;
                if ((!string.IsNullOrEmpty(checkForDisplayNone)) && (checkForDisplayNone == "none"))
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Gets or sets the fill <see cref="SvgPaintServer"/> of this element.
        /// </summary>
        [SvgAttribute("enable-background")]
        public virtual string EnableBackground
        {
            get { return Attributes["enable-background"] as string; }
            set { Attributes["enable-background"] = value; }
        }
    }
}
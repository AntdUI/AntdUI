// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Xml;

namespace AntdUI.Svg
{
    /// <summary>
    /// Represents a list of re-usable SVG components.
    /// </summary>
    [SvgElement("metadata")]
    public class SvgDocumentMetadata : SvgElement
    {
        //	private string _metadata; 


        /// <summary>
        /// Initializes a new instance of the <see cref="SvgDocumentMetadata"/> class.
        /// </summary>
        public SvgDocumentMetadata()
        {
            Content = "";
        }


        //public string Metadata
        //{
        //    get { return _metadata; }
        //    set { _metadata = value; }
        //}


        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            // Do nothing. Children should NOT be rendered.
        }

        protected override void WriteChildren(XmlTextWriter writer)
        {
            writer.WriteRaw(this.Content); //write out metadata as is
        }


        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDocumentMetadata>();
        }

        public override void InitialiseFromXML(XmlTextReader reader, SvgDocument document)
        {
            base.InitialiseFromXML(reader, document);

            //read in the metadata just as a string ready to be written straight back out again
            Content = reader.ReadInnerXml();
        }

    }
}
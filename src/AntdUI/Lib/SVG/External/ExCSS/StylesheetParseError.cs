// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

namespace AntdUI.Svg.ExCSS
{
    public sealed class StylesheetParseError
    {
        public StylesheetParseError(ParserError error, string errorMessage, int line, int column)
        {
            ParserError = error;
            Message = errorMessage;
            Line = line;
            Column = column;
        }

        public ParserError ParserError { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public string Message { get; private set; }

        public override string ToString()
        {
            return string.Format("Line {0}, Column {1}: {2}.", Line, Column, Message);
        }
    }
}
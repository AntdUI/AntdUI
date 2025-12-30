// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace SVGView
{
    public class Localizer : AntdUI.ILocalization
    {
        public string? GetLocalizedString(string key)
        {
            switch (key)
            {
                case "ID":
                    return "en-US";

                case "Cancel":
                    return "Cancel";
                case "OK":
                    return "OK";
                case "Now":
                    return "Now";
                case "ToDay":
                    return "Today";
                case "NoData":
                    return "No data";

                case "ItemsPerPage":
                    return "Per/Page";

                #region SVGView

                case "Title":
                    return "SVG View";
                case "Auto":
                    return "Auto";
                case "Zip":
                    return "Zip";
                case "CopyOK":
                    return "copied";
                case "CopyFailed":
                    return "copy failed";

                #endregion

                default:
                    return null;
            }
        }
    }
}
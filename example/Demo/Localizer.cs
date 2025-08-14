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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

namespace Demo
{
    public class Localizer : AntdUI.ILocalization
    {
        public string GetLocalizedString(string key)
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

                case "Filter.Clean":
                    return "Clean";
                case "Filter.ByView":
                    return "ByView";
                case "Filter.Auto":
                    return "Auto";
                case "Filter.DataSource":
                    return "Source";
                case "Filter.SelectAll":
                    return "(Select All)";
                case "Filter.Blank":
                    return "(Blank)";
                case "Filter.Search":
                    return "Search";
                case "Filter.Width":
                    return "200";

                case "Filter.Equal":
                    return "Equal";
                case "Filter.NotEqual":
                    return "NotEqual";
                case "Filter.Greater":
                    return "Greater";
                case "Filter.Less":
                    return "Less";
                case "Filter.Contain":
                    return "Contain";
                case "Filter.NotContain":
                    return "NotContain";
                case "Filter.None":
                    return "None";

                #region DEMO

                case "Overview.txt_search"://ÊäÈë¹Ø¼ü×ÖËÑË÷...
                    return "Keywords to search";
                case "CopyOK":
                    return "copied";
                case "CopyFailed":
                    return "copy failed";

                case "General":
                    return "General";
                case "Layout":
                    return "Layout";
                case "Navigation":
                    return "Navigation";
                case "DataEntry":
                    return "Data Entry";
                case "DataDisplay":
                    return "Data Display";
                case "Feedback":
                    return "Feedback";
                case "Other":
                    return "Other";

                case "Setting":
                    return "Setting";
                case "AnimationEnabled":
                    return "Animation Enabled";
                case "ShadowEnabled":
                    return "Shadow Enabled";
                case "PopupWindow":
                    return "Popup in the window";
                case "ScrollBarHidden":
                    return "ScrollBar Hidden Style";
                case "TextRenderingHighQuality":
                    return "TextRendering HighQuality";

                case "Previous":
                    return "Previous";
                case "Next":
                    return "Next";
                case "Finish":
                    return "Finish";
                case "Click:":
                    return "Click:";

                //Alert ----------------------------
                case "Alert.Text":
                    return "Alert";
                case "Alert.Description":
                    return "Display warning messages that require attention.";
                case "Alert.divider1":
                    return "More types";
                case "Alert.divider2":
                    return "Description";
                case "Alert.divider3":
                    return "Loop Banner";
                case "Alert.alert14":
                    return "Nike Just Do It";

                //Avatar ----------------------------
                case "Avatar.Text":
                    return "Avatar";
                case "Avatar.Description":
                    return "Used to represent users or things, supporting the display of images, icons, or characters.";
                case "Avatar.avatar5":
                    return "N";

                //Badge ----------------------------
                case "Badge.Text":
                    return "Badge";
                case "Badge.Description":
                    return "Small numerical value or status descriptor for UI elements.";
                case "Badge.divider1":
                    return "Basic";
                case "Badge.divider2":
                    return "More";
                case "Badge.tag1":
                    return "GAMES";

                //Breadcrumb ----------------------------
                case "Breadcrumb.Text":
                    return "Breadcrumb";
                case "Breadcrumb.Description":
                    return "Display the current location within a hierarchy. And allow going back to states higher up in the hierarchy.";

                //Button ----------------------------
                case "Button.Text":
                    return "Button";
                case "Button.Description":
                    return "To trigger an operation.";
                case "Button.Search":
                    return "Search";
                case "Button.divider1":
                    return "Color & Type";
                case "Button.divider2":
                    return "Icon";
                case "Button.divider3":
                    return "Link";
                case "Button.divider4":
                    return "IconPosition";
                case "Button.divider5":
                    return "Multiple Buttons";
                case "Button.divider6":
                    return "Shape";

                //FloatButton ----------------------------
                case "FloatButton.1":
                    return "Search for";
                case "FloatButton.2":
                    return "Don't be ridiculous";
                case "FloatButton.3":
                    return "Help me";
                case "FloatButton.4":
                    return "Hopeless";
                case "FloatButton.Arrow":
                    return "Expand";

                //Carousel ----------------------------
                case "Carousel.Text":
                    return "Carousel";
                case "Carousel.Description":
                    return "A set of carousel areas.";

                //Checkbox ----------------------------
                case "Checkbox.Text":
                    return "Checkbox";
                case "Checkbox.Description":
                    return "Collect user's choices.";
                case "Checkbox.divider1":
                    return "Basic";
                case "Checkbox.divider2":
                    return "Custom color";
                case "Checkbox.divider3":
                    return "Linkage";

                //Collapse ----------------------------
                case "Collapse.Text":
                    return "Collapse";
                case "Collapse.Description":
                    return "A content area which can be collapsed and expanded.";
                case "Collapse.divider1":
                    return "Collapse";
                case "Collapse.divider2":
                    return "Borderless";

                //ColorPicker ----------------------------
                case "ColorPicker.Text":
                    return "ColorPicker";
                case "ColorPicker.Description":
                    return "Used for color selection.";

                //DatePicker ----------------------------
                case "DatePicker.Text":
                    return "DatePicker";
                case "DatePicker.Description":
                    return "To select or input a date.";
                case "DatePicker.divider1":
                    return "Basic";
                case "DatePicker.divider2":
                    return "Range Picker";
                case "DatePicker.divider3":
                    return "Time/Preset";
                case "DatePicker.PlaceholderText":
                    return "Select a date";
                case "DatePicker.PlaceholderS":
                    return "Start date";
                case "DatePicker.PlaceholderE":
                    return "End date";

                //Divider ----------------------------
                case "Divider.Text":
                    return "Divider";
                case "Divider.Description":
                    return "A divider line separates different content.";

                //GridPanel ----------------------------
                case "GridPanel.Text":
                    return "GridPanel";
                case "GridPanel.Description":
                    return "Grid layout container with precise division of areas.";
                case "GridPanel.Prefix":
                    return "Span Attribute";
                case "GridPanel.Describe":
                    return "-The column width attribute is before, and the row height attribute is after; grouping. Each row represents the number of row height attributes corresponding to the number of rows, with spaces separating each number";

                //Splitter ----------------------------
                case "Splitter.Text":
                    return "Splitter";
                case "Splitter.Description":
                    return "Split panels to isolate";

                //Drawer ----------------------------
                case "Drawer.Text":
                    return "Drawer";
                case "Drawer.Description":
                    return "A panel that slides out from the edge of the screen.";
                case "Drawer.divider1":
                    return "Basic";

                //Dropdown ----------------------------
                case "Dropdown.Text":
                    return "Dropdown";
                case "Dropdown.Description":
                    return "A dropdown list.";
                case "Dropdown.divider1":
                    return "Type";
                case "Dropdown.divider2":
                    return "Placement";
                case "Dropdown.dropdown1":
                    return "Subs menu";

                //Icon ----------------------------
                case "Icon.Text":
                    return "Icon";
                case "Icon.Description":
                    return "Semantic vector graphics.";
                case "Icon.PlaceholderText":
                    return "Search icons here, click icon to copy code";
                case "Outlined": return "Outlined";
                case "Filled": return "Filled";
                case "Icon.Directional": return "Directional Icons";
                case "Icon.Suggested": return "Suggested Icons";
                case "Icon.Editor": return "Editor Icons";
                case "Icon.Data": return "Data Icons";
                case "Icon.Logos": return "Brand and Logos";
                case "Icon.Application": return "Application Icons";

                //Input ----------------------------
                case "Input.Text":
                    return "Input";
                case "Input.Description":
                    return "Through mouse or keyboard input content, it is the most basic form field wrapper.";
                case "Input.Search":
                    return "Search";
                case "Input.divider1":
                    return "Basic";
                case "Input.divider2":
                    return "Pre / Post tab";
                case "Input.divider3":
                    return "Variant";
                case "Input.divider4":
                    return "Multiline";
                case "Input.divider5":
                    return "Password";
                case "Input.divider6":
                    return "Combination";
                case "Input.Code":
                    return "Please enter verification code";
                case "Input.Tao":
                    return "Tao, I like it";
                case "Input.input2":
                case "Input.input14":
                    return "Clear button";
                case "Input.input3":
                    return "Round";
                case "Input.input4":
                    return "Bold border";
                case "Input.input5":
                    return "Please input something";
                case "Input.input13":
                    return "Enter your password";
                case "Input.input10":
                case "Input.input18":
                case "Input.input19":
                    return "input search text";

                //InputNumber ----------------------------
                case "InputNumber.Text":
                    return "InputNumber";
                case "InputNumber.Description":
                    return "Enter a number within certain range with the mouse or keyboard.";
                case "InputNumber.divider1":
                    return "Basic";
                case "InputNumber.input3":
                    return "Enter number";

                //Menu ----------------------------
                case "Menu.Text":
                    return "Menu";
                case "Menu.Description":
                    return "A versatile menu for navigation.";
                case "Menu.divider1":
                    return "Top Navigation";
                case "Menu.divider2":
                    return "Inline menu";
                case "Menu.divider3":
                    return "Vertical menu";
                case "Menu.expand":
                    return "Expand";
                case "Menu.collapse":
                    return "Collapse";

                //Message ----------------------------
                case "Message.Text":
                    return "Message";
                case "Message.Description":
                    return "Display global messages as feedback in response to user operations.";
                case "Message.divider1":
                    return "More types";
                case "Message.divider2":
                    return "Message with loading indicator";

                //Modal ----------------------------
                case "Modal.Text":
                    return "Modal";
                case "Modal.Description":
                    return "Display a modal dialog box, providing a title, content area, and action buttons.";
                case "Modal.divider1":
                    return "Basic";

                //Notification ----------------------------
                case "Notification.Text":
                    return "Notification";
                case "Notification.Description":
                    return "Prompt notification message globally.";
                case "Notification.divider1":
                    return "Placement";
                case "Notification.divider2":
                    return "More types";

                //PageHeader ----------------------------
                case "PageHeader.Text":
                    return "PageHeader";
                case "PageHeader.Description":
                    return "A header with common actions and design elements built in.";
                case "PageHeader.Type":
                    return "ShowBack";
                case "PageHeader.divider1":
                    return "Basic";
                case "PageHeader.divider2":
                    return "CloseButton";

                //Pagination ----------------------------
                case "Pagination.Text":
                    return "Pagination";
                case "Pagination.Description":
                    return "A long list can be divided into several pages, and only one page will be loaded at a time.";

                //Panel ----------------------------
                case "Panel.Text":
                    return "Panel";
                case "Panel.Description":
                    return "A container for displaying information.";

                //Popover ----------------------------
                case "Popover.Text":
                    return "Popover";
                case "Popover.Description":
                    return "The floating card pops up when clicking/mouse hovering over an element.";
                case "Popover.divider1":
                    return "Basic";
                case "Popover.divider2":
                    return "Placement";
                case "Popover.button1":
                    return "Normal";
                case "Popover.button2":
                    return "Custom Control Content";

                //Preview ----------------------------
                case "Preview.Text":
                    return "Preview";
                case "Preview.Description":
                    return "Picture preview box.";
                case "Preview.divider1":
                    return "Basic";
                case "Preview.button1":
                    return "Single Image";
                case "Preview.button2":
                    return "Multiple Images";
                case "Preview.button3":
                    return "Dynamic Load Images";

                //Progress ----------------------------
                case "Progress.Text":
                    return "Progress";
                case "Progress.Description":
                    return "Display the current progress of the operation.";
                case "Progress.divider1":
                    return "Standard progress bar";
                case "Progress.divider2":
                    return "Circular progress bar";
                case "Progress.divider3":
                    return "Mini size progress bar";
                case "Progress.divider4":
                    return "Responsive circular progress bar";
                case "Progress.divider5":
                    return "Progress bar with steps";

                //Radio ----------------------------
                case "Radio.Text":
                    return "Radio";
                case "Radio.Description":
                    return "Used to select a single state from multiple options.";
                case "Radio.divider1":
                    return "Basic";
                case "Radio.divider2":
                    return "Custom color";
                case "Radio.divider3":
                    return "Linkage";

                //Rate ----------------------------
                case "Rate.Text":
                    return "Rate";
                case "Rate.Description":
                    return "Used for rating operation on something.";
                case "Rate.rate5":
                    return "A";
                case "Rate.rate6":
                    return "NB";

                //Result ----------------------------
                case "Result.Text":
                    return "Result";
                case "Result.Description":
                    return "Used to feedback the processing results of a series of operations.";

                //Segmented ----------------------------
                case "Segmented.Text":
                    return "Segmented";
                case "Segmented.Description":
                    return "Display multiple options and allow users to select a single option.";

                //Select ----------------------------
                case "Select.Text":
                    return "Select";
                case "Select.Description":
                    return "A dropdown menu for displaying choices.";
                case "Select.divider1":
                    return "Basic";
                case "Select.divider2":
                    return "Combination";
                case "Select.divider3":
                    return "More";
                case "Select.select4":
                    return "No text";
                case "Select.select5":
                    return "Show Arrow";
                case "Select.select6":
                case "Select.select7":
                    return "Enter search freely";
                case "Select.select8":
                    return "(Select)";

                //Slider ----------------------------
                case "Slider.Text":
                    return "Slider";
                case "Slider.Description":
                    return "A Slider component for displaying current value and intervals in range.";
                case "Slider.divider1":
                    return "Basic";
                case "Slider.divider2":
                    return "Mark Dot";

                //Steps ----------------------------
                case "Steps.Text":
                    return "Steps";
                case "Steps.Description":
                    return "A navigation bar that guides users through the steps of a task.";

                //Switch ----------------------------
                case "Switch.Text":
                    return "Switch";
                case "Switch.Description":
                    return "Used to toggle between two states.";

                //Table ----------------------------
                case "Table.Text":
                    return "Table";
                case "Table.Description":
                    return "A table displays rows of data.";
                case "Table.checkFixedHeader":
                    return "FixedHeader";
                case "Table.checkColumnDragSort":
                    return "ColumnDragSort";
                case "Table.checkRowsDragSort":
                    return "RowsDragSort";
                case "Table.checkBordered":
                    return "Bordered";
                case "Table.checkSetRowStyle":
                    return "SetRowStyle";
                case "Table.checkSortOrder":
                    return "SortOrder";
                case "Table.checkEnableHeaderResizing":
                    return "EnableHeaderResizing";
                case "Table.checkVisibleHeader":
                    return "VisibleHeader";
                case "Table.checkAddressLineBreak":
                    return "AddressLineBreak";
                case "Table.checkFilter":
                    return "Filter";
                case "Table.selectEditMode":
                    return "EditMode";
                case "Table.selectEditStyle":
                    return "EditStyle";
                case "Table.Column.name":
                    return "Name";
                case "Table.Column.checkTitle":
                    return "No Title";
                case "Table.Column.radio":
                    return "Radio";
                case "Table.Column.online":
                    return "Online";
                case "Table.Column.enable":
                    return "Enable";
                case "Table.Column.age":
                    return "Age";
                case "Table.Column.address":
                    return "Address";
                case "Table.Column.date":
                    return "Date";
                case "Table.Column.imgs":
                    return "Imgs";
                case "Table.Column.btns":
                    return "Action";
                case "Table.Data.Name1":
                    return "John Brown";
                case "Table.Data.Name2":
                    return "Jim Green";
                case "Table.Data.Name3":
                    return "Joe Black";
                case "Table.Data.Online.Default":
                    return "Default";
                case "Table.Data.Online":
                    return "Online";
                case "Table.Data.Online.Processing":
                    return "Processing";
                case "Table.Data.Online.Error":
                    return "Error";
                case "Table.Data.Online.Warn":
                    return "Warn";
                case "Table.Data.Address1":
                    return "London, Park Lane no.1";
                case "Table.Data.Address2":
                    return "New York No.1 Lake Park";
                case "Table.Data.Address3":
                    return "London No. 1 Lake Park";
                case "Table.Data.Address4":
                    return "Sydney No. 1 Lake Park";
                case "Table.Data.AddressNum":
                    return "London, Park Lane no.";

                //Tabs ----------------------------
                case "Tabs.Text":
                    return "Tabs";
                case "Tabs.Description":
                    return "Tabs make it easy to explore and switch between different views.";
                case "Tabs.divider1":
                    return "Basic";
                case "Tabs.divider2":
                    return "Card style";
                case "Tabs.divider3":
                    return "Center position";

                //Tag ----------------------------
                case "Tag.Text":
                    return "Tag";
                case "Tag.Description":
                    return "Used for marking and categorization.";
                case "Tag.divider1":
                    return "Basic";
                case "Tag.divider2":
                    return "Colorful Tag";
                case "Tag.divider3":
                    return "Icon";
                case "Tag.tag16":
                    return "Custom Icon";

                //Timeline ----------------------------
                case "Timeline.Text":
                    return "Timeline";
                case "Timeline.Description":
                    return "Vertical display timeline.";

                //TimePicker ----------------------------
                case "TimePicker.Text":
                    return "TimePicker";
                case "TimePicker.Description":
                    return "To select/input a time.";
                case "TimePicker.divider1":
                    return "Basic";

                //Tooltip ----------------------------
                case "Tooltip.Text":
                    return "Tooltip";
                case "Tooltip.Description":
                    return "Simple text popup box.";
                case "Tooltip.divider1":
                    return "Basic";
                case "Tooltip.divider2":
                    return "Placement";
                case "Tooltip.label4":
                    return "Simplest usage";

                //Tour ----------------------------
                case "Tour.Text":
                    return "Tour";
                case "Tour.Description":
                    return "A popup component for guiding users through a product.";

                //Tree ----------------------------
                case "Tree.Text":
                    return "Tree";
                case "Tree.Description":
                    return "Multiple-level structure list.";
                case "Tree.Loading":
                    return "Load animation, click pause";

                //VirtualPanel ----------------------------
                case "VirtualPanel.Text":
                    return "VirtualPanel";
                case "VirtualPanel.Description":
                    return "Layout container detached from Winform framework.";
                case "VirtualPanel.checkbox1":
                    return "Waterfall";

                //Calendar ----------------------------
                case "Calendar.Text":
                    return "Calendar";
                case "Calendar.Description":
                    return "A container that displays data in calendar form.";
                case "Calendar.divider1":
                    return "Basic";

                //ContextMenuStrip ----------------------------
                case "ContextMenuStrip.Text":
                    return "ContextMenuStrip";
                case "ContextMenuStrip.Description":
                    return "Right click on the current page at will";

                //Battery ----------------------------
                case "Battery.Text":
                    return "Battery";
                case "Battery.Description":
                    return "Display device battery level.";
                case "Battery.Add":
                    return "Power up";
                case "Battery.Subtract":
                    return "Reduce";
                case "Battery.divider1":
                    return "Basic";
                case "Battery.divider2":
                    return "No text";
                case "Battery.divider3":
                    return "Point size";

                //Signal ----------------------------
                case "Signal.Text":
                    return "Signal";
                case "Signal.Description":
                    return "Display device signals.";
                case "Signal.Add":
                    return "Add";
                case "Signal.Subtract":
                    return "Subtract";
                case "Signal.divider1":
                    return "Basic";
                case "Signal.divider2":
                    return "Line style";
                case "Signal.divider3":
                    return "Loading";

                //Spin ----------------------------
                case "Spin.Text":
                    return "Spin";
                case "Spin.Description":
                    return "Used for the loading status of a page or a block.";
                case "Spin.divider1":
                    return "Direct use";
                case "Spin.divider2":
                    return "Display Text";
                case "Spin.divider3":
                    return "Basic";
                case "Spin.btnPanel":
                    return "Current container";
                case "Spin.btnControl":
                    return "Control above";
                case "Spin.btnWindow":
                    return "Entire window";

                //Shield ----------------------------
                case "Shield.Text":
                    return "Shield";
                case "Shield.Description":
                    return "Concise, consistent, and legible badges.";
                case "Shield.qq":
                    return "QQ Group";

                #endregion

                case "Loading":
                    return "LOADING";
                case "Processing":
                    return "Processing";
                case "Loading2":
                    return "Loading in progress...";
                case "PleaseWait":
                    return "Please be patient and wait";

                default:
                    System.Diagnostics.Debug.WriteLine(key);
                    return null;
            }
        }
    }
}
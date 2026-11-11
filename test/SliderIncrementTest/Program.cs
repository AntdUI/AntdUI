using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using AntdUI;

static class Program
{
    static int checks;
    [STAThread]
    static void Main()
    {
        using (var slider = new Slider())
        {
            Check(slider.Increment == 1, "default increment");
            foreach (int invalid in new[] { 0, -1 })
            {
                try { slider.Increment = invalid; throw new Exception("invalid increment accepted"); }
                catch (ArgumentOutOfRangeException) { checks++; }
            }
            slider.SetMinMax(0, 1000);
            slider.Increment = 100;
            slider.WheelModifyEnabled = true;
            Wheel(slider, 120);
            Check(slider.Value == 100, "wheel up");
            Wheel(slider, -120);
            Check(slider.Value == 0, "wheel down");
            slider.Value = 125;
            Check(Next(slider, true) == 200 && Next(slider, false) == 100, "keyboard shared stepping from off-grid value");
            SetField(slider, "rect_read", new Rectangle(0, 0, 1000, 1000));
            foreach (var align in new[] { TAlignMini.Left, TAlignMini.Right, TAlignMini.Top, TAlignMini.Bottom })
            {
                slider.Align = align;
                bool reverse = align == TAlignMini.Right || align == TAlignMini.Bottom;
                int position = reverse ? 550 : 450;
                Check(Find(slider, position, position) == 500, "drag midpoint " + align);
                Check(Find(slider, reverse ? 0 : 1000, reverse ? 0 : 1000) == 1000, "drag endpoint " + align);
            }
            slider.SetMinMax(-50, 45);
            slider.Increment = 30;
            slider.Value = 40;
            Wheel(slider, 120);
            Check(slider.Value == 45, "non-divisible maximum reachable");
            Wheel(slider, -120);
            Check(slider.Value == 40, "reverse from maximum returns nearest grid");
            Check(Next(slider, false) == 10, "negative minimum grid");
            slider.SetMinMax(0, 1000);
            slider.Value = 0;
            slider.Dots = true;
            slider.Marks.Add(new SliderMarkItem { Value = 17 });
            slider.Marks.Add(new SliderMarkItem { Value = 43 });
            Wheel(slider, 120);
            Check(slider.Value == 17, "marks take priority");
            slider.Dots = false;
            slider.SetMinMax(int.MinValue, int.MaxValue);
            slider.Increment = int.MaxValue;
            slider.Value = int.MaxValue;
            Check(Next(slider, true) == int.MaxValue, "overflow upper");
            Check(Next(slider, false) == int.MaxValue - 1, "full integer range");
            slider.SetMinMax(7, 7);
            Check(Next(slider, true) == 7 && Next(slider, false) == 7, "zero range");
        }
        Console.WriteLine("Passed " + checks + " increment checks.");
    }
    static object Invoke(Slider slider, string name, params object[] args) =>
        typeof(Slider).GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(slider, args);
    static int Next(Slider slider, bool increase) => (int)Invoke(slider, "NextIncrementValue", increase);
    static int Find(Slider slider, int x, int y) => (int)Invoke(slider, "FindIndex", x, y, false);
    static void SetField(Slider slider, string name, object value) =>
        typeof(Slider).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(slider, value);
    static void Wheel(Slider slider, int delta) =>
        Invoke(slider, "OnMouseWheel", new HandledMouseEventArgs(MouseButtons.None, 0, 0, 0, delta));
    static void Check(bool result, string name)
    {
        if (!result) throw new Exception(name);
        checks++;
    }
}

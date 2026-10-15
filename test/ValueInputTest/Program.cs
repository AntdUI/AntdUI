using System;
using System.Reflection;
using System.Windows.Forms;
using AntdUI;
using ShortcutKeys = AntdUI.Input.ShortcutKeys;

internal static class Program
{
    static int checks;

    [STAThread]
    static void Main()
    {
        using (var input = new InputNumber())
        {
            Check(!input.KeyboardModifyEnabled && input.WheelModifyEnabled, "InputNumber defaults");
            input.Value = 10;
            input.ProcessShortcutKeys(ShortcutKeys.Up);
            Check(input.Value == 10, "keyboard disabled");
            input.KeyboardModifyEnabled = true;
            input.Text = "25";
            input.ProcessShortcutKeys(ShortcutKeys.Up);
            Check(input.Value == 26, "commit pending text before increment");
            input.DecimalPlaces = 2;
            input.Increment = 0.25m;
            input.ProcessShortcutKeys(ShortcutKeys.Down);
            Check(input.Value == 25.75m, "decimal increment");
            input.ReadOnly = true;
            input.ProcessShortcutKeys(ShortcutKeys.Up);
            Check(input.Value == 25.75m, "read only keyboard");
            input.ReadOnly = false;
            input.Maximum = 26;
            input.Value = 26;
            int changes = 0;
            input.ValueChanged += (_, __) => changes++;
            input.ProcessShortcutKeys(ShortcutKeys.Up);
            Check(input.Value == 26 && changes == 0, "no event at upper bound");
            input.WheelModifyEnabled = false;
            Wheel(input, -120);
            Check(input.Value == 26, "wheel disabled");
            input.WheelModifyEnabled = true;
            Wheel(input, -120);
            Check(input.Value == 25.75m, "wheel increment");
            Wheel(input, 0);
            Check(input.Value == 25.75m, "zero wheel delta");
            input.Maximum = null;
            input.Value = decimal.MaxValue;
            input.Increment = 1;
            input.ProcessShortcutKeys(ShortcutKeys.Up);
            Check(input.Value == decimal.MaxValue, "decimal overflow saturates");
        }
        using (var slider = new Slider())
        {
            Check(!slider.KeyboardModifyEnabled && !slider.WheelModifyEnabled, "Slider defaults");
            slider.Value = 10;
            Wheel(slider, 120);
            Check(slider.Value == 10, "Slider wheel disabled");
            slider.KeyboardModifyEnabled = true;
            Check(slider.TabStop, "keyboard enables tab navigation");
            slider.KeyboardModifyEnabled = false;
            Check(!slider.TabStop, "keyboard disables tab navigation");
            slider.WheelModifyEnabled = true;
            Wheel(slider, 120);
            Check(slider.Value == 11, "Slider wheel increment");
            slider.Dots = true;
            slider.Marks.Add(new SliderMarkItem { Value = 30 });
            slider.Marks.Add(new SliderMarkItem { Value = 5 });
            slider.Marks.Add(new SliderMarkItem { Value = 20 });
            Wheel(slider, 120);
            Check(slider.Value == 20, "unsorted marks next");
            Wheel(slider, -120);
            Check(slider.Value == 5, "unsorted marks previous");
            slider.Dots = false;
            slider.SetMinMax(int.MinValue, int.MaxValue);
            slider.Value = int.MaxValue;
            Wheel(slider, 120);
            Check(slider.Value == int.MaxValue, "integer overflow saturates");
            slider.Enabled = false;
            Wheel(slider, -120);
            Check(slider.Value == int.MaxValue, "disabled Slider");
        }
        Console.WriteLine($"Passed {checks} checks.");
    }

    static void Wheel(Control control, int delta)
    {
        var args = new HandledMouseEventArgs(MouseButtons.None, 0, 0, 0, delta);
        control.GetType().GetMethod("OnMouseWheel", BindingFlags.Instance | BindingFlags.NonPublic)
            .Invoke(control, new object[] { args });
    }

    static void Check(bool passed, string name)
    {
        if (!passed) throw new Exception(name);
        checks++;
    }
}

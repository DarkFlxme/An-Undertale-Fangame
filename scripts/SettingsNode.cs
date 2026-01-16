using game;
using Godot;
using System;
using System.Diagnostics.Contracts;

public partial class SettingsNode : Node2D
{
    [Signal] public delegate void VSyncChangedEventHandler(bool toggled);
    [Signal] public delegate void FPSDisplayChangedEventHandler(bool toggled);
    public CheckButton vsync;
    public CheckButton fpsdisplay;
    public CheckButton animationbutton;
    public LineEdit fpslimiter;
    public override void _Ready()
    {
        vsync = GetNode<CheckButton>("CheckButtonVSync");
        fpsdisplay = GetNode<CheckButton>("CheckButtonfpslabel");
        fpslimiter = GetNode<LineEdit>("fpslimit");
        animationbutton = GetNode<CheckButton>("CheckButtonAnim");
        vsync.Toggled += OnVsyncToggled;
        fpsdisplay.Toggled += OnFPSDisplayToggled;
        animationbutton.Toggled += OnAnimationToggled;
        fpslimiter.FocusExited += () => OnFPSLimitChanged(fpslimiter.Text);
        fpslimiter.TextSubmitted += OnFPSLimitChanged;
        ApplySettings();
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("X") && Visible)
        {
            var parent = GetParent();
            Hide();
            parent.GetNode<Control>("Control").Show();
            parent.GetParent<Node2D>().SetProcess(true);
            var parentButtons = parent.GetParent<Buttons>();
			foreach (Node2D node in parentButtons.GetChildren())
			{
				node.Show();
			}
			parentButtons.SetProcess(true);
			parentButtons.SetHighlighted(parentButtons.buttons[parentButtons.currentIndex], true);
        }
    }

    private void OnVsyncToggled(bool toggled)
    {
        EmitSignal(SignalName.VSyncChanged, toggled);
    }
    private void OnFPSDisplayToggled(bool toggled)
    {
        EmitSignal(SignalName.FPSDisplayChanged, toggled);
    }
    private void OnFPSLimitChanged(string limit)
    {
        fpslimiter.ReleaseFocus();
        if (int.TryParse(limit, out int fpslimit))
        {
            if (fpslimit < 0)
            {
                fpslimiter.Text = Engine.MaxFps.ToString();
            }
            else
            {
                Engine.MaxFps = fpslimit;
                fpslimiter.Text = Engine.MaxFps.ToString();
            }
        }
        else
        {
            Engine.MaxFps = fpslimit;
            fpslimiter.Text = Engine.MaxFps.ToString();
        }
        SettingsManager.Settings["fpslimit"] = Engine.MaxFps;
        SettingsManager.SaveSettings();
    }
    private void OnAnimationToggled(bool toggled)
    {
        Settings.menuAnimationEnabled = toggled;
        SettingsManager.Settings["menuanimation"] = toggled;
        SettingsManager.SaveSettings();
    }
    public void ApplySettings()
    {
        if (SettingsManager.Settings.TryGetValue("vsync", out Variant vsyncEnabled))
            vsync.ButtonPressed = vsyncEnabled.AsBool();

        if (SettingsManager.Settings.TryGetValue("fpsdisplay", out Variant fpsEnabled))
            fpsdisplay.ButtonPressed = fpsEnabled.AsBool();
        if (SettingsManager.Settings.TryGetValue("mastervolume", out Variant mastervolume))
        {
            GetNode<VolumeControl>("VolumeControl").SetVolume(0, (float)mastervolume.AsDouble());
        }
        if (SettingsManager.Settings.TryGetValue("sfxvolume", out Variant sfxvolume))
        {
            GetNode<VolumeControl>("VolumeControl").SetVolume(1, (float)sfxvolume.AsDouble());
        }
        if (SettingsManager.Settings.TryGetValue("musicvolume", out Variant musicvolume))
        {
            GetNode<VolumeControl>("VolumeControl").SetVolume(2, (float)musicvolume.AsDouble());
        }
        if (SettingsManager.Settings.TryGetValue("fpslimit", out Variant fpslimit))
        {
            fpslimiter.Text = fpslimit.AsInt32().ToString();
            Engine.MaxFps = fpslimit.AsInt32();
        }
        if (SettingsManager.Settings.TryGetValue("menuanimation", out Variant menuanimation))
        {
            animationbutton.ButtonPressed = menuanimation.AsBool();
            Settings.menuAnimationEnabled = menuanimation.AsBool();
        }
    }
}
using Godot;
using System;
using System.Data.Common;

public partial class VolumeControl : Control
{
    [Export] private HSlider masterSlider;
    [Export] private HSlider sfxSlider;
    [Export] private HSlider musicSlider;
    [Export] private Label masterLabel;
    [Export] private Label sfxLabel;
    [Export] private Label musicLabel;
    public override void _Ready()
    {
        masterSlider.DragEnded += (a) =>
        {
            SettingsManager.Settings["mastervolume"] = (float)masterSlider.Value;
        };
        sfxSlider.DragEnded += (a) =>
        {
            SettingsManager.Settings["sfxvolume"] = (float)sfxSlider.Value;
        };
        musicSlider.DragEnded += (a) =>
        {
            SettingsManager.Settings["musicvolume"] = (float)musicSlider.Value;
        };
        masterSlider.ValueChanged += (a) =>
        {
            SetVolume(0, (float)masterSlider.Value);
            masterLabel.Text = $"{masterSlider.Value}%";
        };
        sfxSlider.ValueChanged += (a) =>
        {
            SetVolume(1, (float)sfxSlider.Value);
            sfxLabel.Text = $"{sfxSlider.Value}%";
        };
        musicSlider.ValueChanged += (a) =>
        {
            SetVolume(2, (float)musicSlider.Value);
            musicLabel.Text = $"{musicSlider.Value}%";
        };
    }
    public void SetVolume(int bus, float value)
    {
        switch (bus)
        {
            case 0:
                masterSlider.Value = value;
                AudioServer.SetBusVolumeLinear(0, value / 100);
                break;
            case 1:
                sfxSlider.Value = value;
                AudioServer.SetBusVolumeLinear(1, value / 100);
                break;
            case 2:
                musicSlider.Value = value;
                AudioServer.SetBusVolumeLinear(2, value / 100);
                break;
        }
    }
}

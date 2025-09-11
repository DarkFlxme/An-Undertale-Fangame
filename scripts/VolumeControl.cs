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
        masterSlider.ValueChanged += (value) =>
        {
            AudioServer.SetBusVolumeLinear(0, (float)value / 100);
            masterLabel.Text = $"{value}%";
            SettingsManager.Settings["mastervolume"] = value;
            SettingsManager.SaveSettings();
        };
        sfxSlider.ValueChanged += (value) =>
        {
            AudioServer.SetBusVolumeLinear(1, (float)value / 100);
            sfxLabel.Text = $"{value}%";
            SettingsManager.Settings["sfxvolume"] = value;
            SettingsManager.SaveSettings();
        };
        musicSlider.ValueChanged += (value) =>
        {
            AudioServer.SetBusVolumeLinear(2, (float)value / 100);
            musicLabel.Text = $"{value}%";
            SettingsManager.Settings["musicvolume"] = value;
            SettingsManager.SaveSettings();
        };
    }
    public void SetVolume(int bus, float value)
    {
        switch (bus)
        {
            case 0:
                masterSlider.Value = value;
                SettingsManager.Settings["mastervolume"] = value;
                break;
            case 1:
                sfxSlider.Value = value;
                SettingsManager.Settings["sfxvolume"] = value;
                break;
            case 2:
                musicSlider.Value = value;
                SettingsManager.Settings["musicvolume"] = value;
                break;
        }
    }
}

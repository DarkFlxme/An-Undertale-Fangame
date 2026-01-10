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
            AudioServer.SetBusVolumeLinear(0, (float)masterSlider.Value / 100);
            masterLabel.Text = $"{masterSlider.Value}%";
            SettingsManager.Settings["mastervolume"] = masterSlider.Value;
            SettingsManager.SaveSettings();
        };
        sfxSlider.DragEnded += (a) =>
        {
            AudioServer.SetBusVolumeLinear(1, (float)sfxSlider.Value / 100);
            sfxLabel.Text = $"{sfxSlider.Value}%";
            SettingsManager.Settings["sfxvolume"] = sfxSlider.Value;
            SettingsManager.SaveSettings();
        };
        musicSlider.DragEnded += (a) =>
        {
            AudioServer.SetBusVolumeLinear(2, (float)musicSlider.Value / 100);
            musicLabel.Text = $"{musicSlider.Value}%";
            SettingsManager.Settings["musicvolume"] = musicSlider.Value;
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

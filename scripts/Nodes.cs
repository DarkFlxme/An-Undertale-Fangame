using System;
using Godot;

namespace game
{
    public partial class Nodes : Node2D
    {
        public static Player PlayerNode;
        public static StaticBody2D map1;
        public static StaticBody2D map2;
        public static Label fpslabel;
        public static Timer BulletTimer;
        public static SettingsNode settingsnode;
        public static Node2D shield_green;
        public override void _Ready()
        {
            PlayerNode = GetNode<Player>("Player");
            map1 = GetNode<StaticBody2D>("map1");
            map2 = GetNode<StaticBody2D>("map2");
            shield_green = GetNode<Node2D>("Shield");
            fpslabel = GetNode<Label>("Label");
            BulletTimer = GetNode<Timer>("BulletTimer");
            settingsnode = GetNode<SettingsNode>("Settings");
            settingsnode.Connect(SettingsNode.SignalName.VSyncChanged, Callable.From<bool>(Settings.SetVsync));
            settingsnode.Connect(SettingsNode.SignalName.FPSDisplayChanged, Callable.From<bool>(Settings.SetFPSDisplay));
            Settings.SetVsync(SettingsManager.Settings["vsync"].AsBool());
            Settings.SetFPSDisplay(SettingsManager.Settings["fpsdisplay"].AsBool());
            DisableNode(PlayerNode);
            DisableNode(map1);
            DisableNode(map2);
            DisableNode(settingsnode);
            DisableNode(shield_green);
        }
        public override void _Process(double delta)
        {
            if (Settings.fpsshow)
                fpslabel.Text = "FPS: " + Engine.GetFramesPerSecond();
            else
                fpslabel.Text = "";
            if (Input.IsActionJustPressed("toggle_fullscreen"))
                Settings.SetWindow(!Settings.windowMode);
            if (Input.IsActionJustPressed("quit"))
                GetTree().Quit();
        }
        public static void DisableNode(CanvasItem node)
        {
            node.Hide();
            node.ProcessMode = ProcessModeEnum.Disabled;
        }
        public static void EnableNode(CanvasItem node)
        {
            node.Show();
            node.ProcessMode = ProcessModeEnum.Inherit;
        }
    }
    public static class Settings
    {
        public static bool vsync = true;
        public static bool fpsshow = false;
        public static bool windowMode = true;
        public static void SetWindow(bool mode)
        {
            if (mode == false)
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                windowMode = false;
            }
            else
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                windowMode = true;
            }
        }
        public static void SetVsync(bool mode)
        {
            if (mode == false)
            {
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
                vsync = false;
            }
            else
            {
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
                vsync = true;
            }
            SettingsManager.Settings["vsync"] = vsync;
            SettingsManager.SaveSettings();
        }
        public static void SetFPSDisplay(bool mode)
        {
            fpsshow = mode;
            SettingsManager.Settings["fpsdisplay"] = fpsshow;
            SettingsManager.SaveSettings();
        }
    }
}
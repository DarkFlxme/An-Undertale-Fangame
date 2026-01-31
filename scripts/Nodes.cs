using System;
using Godot;

namespace game
{
    public partial class Nodes : Node2D
    {
        public static Player PlayerNode;
        public static Node2D map1;
        public static StaticBody2D map2;
        public static Label fpslabel;
        public static Timer BulletTimer;
        public static SettingsNode settingsnode;
        public static Node2D shield_green;
        public static Node2D purplelines;
        public static FadeControl FadeRect;
        public static Buttons MenuButtons;
        public static GameUI gameUI;
        float quittimer = 0f;
        public override void _Ready()
        {
            gameUI = GetNode<GameUI>("GameUI");
            PlayerNode = GetNode<Player>("Player");
            map1 = GetNode<Node2D>("map1");
            map2 = GetNode<StaticBody2D>("map2");
            shield_green = GetNode<Node2D>("Shield");
            fpslabel = GetNode<Label>("Label");
            BulletTimer = GetNode<Timer>("BulletTimer");
            settingsnode = GetNode<SettingsNode>("/root/Nodes/MainMenuButtons/SettingsButton/Settings");
            purplelines = GetNode<Node2D>("purpleHeart");
            FadeRect = GetNode<FadeControl>("CanvasLayer/ColorRect");
            MenuButtons = GetNode<Buttons>("MainMenuButtons");
            purplelines.Hide();
            settingsnode.Connect(SettingsNode.SignalName.VSyncChanged, Callable.From<bool>(Settings.SetVsync));
            settingsnode.Connect(SettingsNode.SignalName.FPSDisplayChanged, Callable.From<bool>(Settings.SetFPSDisplay));
            Settings.SetVsync(SettingsManager.Settings["vsync"].AsBool());
            Settings.SetFPSDisplay(SettingsManager.Settings["fpsdisplay"].AsBool());
            DisableNode(PlayerNode);
            DisableNode(map1);
            DisableNode(map2);
            DisableNode(shield_green);
        }
        public override void _Notification(int what)
        {
            if(what == NotificationWMCloseRequest)
            {
                SettingsManager.SaveSettings();
            }
        }
        public override void _Process(double delta)
        {
            if (Settings.fpsshow)
                fpslabel.Text = "FPS: " + Engine.GetFramesPerSecond();
            else
                fpslabel.Text = "";
            if (Input.IsActionJustPressed("toggle_fullscreen"))
                Settings.SetWindow(!Settings.windowMode);
            if (Input.IsActionPressed("quit"))
            {
                quittimer += (float)delta;
                if (quittimer >= 1.5f)
                    GetTree().Quit();
            }
            else if (Input.IsActionJustReleased("quit"))
            {
                Settings.SetWindow(true);
                quittimer = 0f;
            }
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
        public enum Difficulty
        {
            Casual,
            Normal,
            Extreme
        }
        public static Difficulty GameDifficulty;
        public static double BossFightTime = 0.0;
        public static bool vsync = true;
        public static bool fpsshow = false;
        public static bool windowMode = true;
        public static bool menuAnimationEnabled = true;
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
        }
        public static void SetFPSDisplay(bool mode)
        {
            fpsshow = mode;
            SettingsManager.Settings["fpsdisplay"] = fpsshow;
        }
    }
}
using Godot;
using System;
using System.Collections.Generic;

public partial class SettingsManager : Node
{
    public static Dictionary<string, Variant> Settings = new()
    {
        {"vsync", Variant.From(true)},
        {"fpsdisplay", Variant.From(false)},
        {"mastervolume",Variant.From<float>(100)},
        {"sfxvolume",Variant.From<float>(100)},
        {"musicvolume",Variant.From<float>(100)},
    };

    private const string SettingsFilePath = "user://settings.json";

    public override void _Ready()
    {
        LoadSettings();
    }

    public static void LoadSettings()
    {
        if (FileAccess.FileExists(SettingsFilePath))
        {
            using var file = FileAccess.Open(SettingsFilePath, FileAccess.ModeFlags.Read);
            string jsonText = file.GetAsText();
            file.Close();

            var parseResult = Json.ParseString(jsonText);
            if (parseResult.VariantType == Variant.Type.Dictionary)
            {
                var loadedSettings = parseResult.As<Godot.Collections.Dictionary>();
                Settings.Clear();
                foreach (var key in loadedSettings.Keys)
                {
                    Settings[key.AsStringName()] = loadedSettings[key];
                }
            }
        }
        else
        {
            SaveSettings();
        }
    }

    public static void SaveSettings()
    {
        var godotDict = new Godot.Collections.Dictionary();
        foreach (var kvp in Settings)
        {
            godotDict[kvp.Key] = kvp.Value;
        }

        string jsonText = Json.Stringify(godotDict);
        using var file = FileAccess.Open(SettingsFilePath, FileAccess.ModeFlags.Write);
        file.StoreString(jsonText);
        file.Close();
    }
}

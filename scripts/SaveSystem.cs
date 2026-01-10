using Godot;
using System;
using System.Collections.Generic;

public partial class SaveSystem : Node
{
    public static Dictionary<string, Variant> SaveFile = new()
    {
        {"highscore", Variant.From<int>(0)},
    };
    const string SavePath = "user://save.dat";
    public override void _Ready()
    {
        LoadSaveFile();
    }
    public static void LoadSaveFile()
    {
        if (FileAccess.FileExists(SavePath))
        {
            using var file = FileAccess.Open(SavePath,FileAccess.ModeFlags.Read);
            Variant v=file.GetVar();
            if (v.VariantType == Variant.Type.Dictionary)
            {
                var data = v.AsGodotDictionary();
                SaveFile.Clear();
                foreach(var key in data.Keys)
                {
                    SaveFile[key.AsStringName()] = data[key];
                }
            }
        }
        else
        {
            SaveGame();
        }
    }
    public static void SaveGame()
    {
        using var file = FileAccess.Open(SavePath,FileAccess.ModeFlags.Write);
        var godotDict = new Godot.Collections.Dictionary();
        foreach(var kvp in SaveFile)    
        {
            godotDict[kvp.Key] = kvp.Value;
        }
        file.StoreVar(godotDict);
    }
}


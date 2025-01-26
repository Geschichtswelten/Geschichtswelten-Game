using System;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class JsonHandler : MonoBehaviour
{

    public static SettingsClass ReadSettings(string jsonString)
    {
        try
        {
            TextAsset file = Resources.Load(jsonString) as TextAsset;
            return JsonUtility.FromJson<SettingsClass>(file.text);
        }
        catch (NullReferenceException)
        {
            return null;
        }
    }

    public static void WriteSettings(SettingsClass settings)
    {
        string jsonString = JsonUtility.ToJson(settings);
        if (Directory.Exists(Application.dataPath + "/Settings"))
        {
            File.WriteAllText(Application.dataPath + "/Settings/settings.json", jsonString);
        }
        else
        {
            Directory.CreateDirectory(Application.dataPath + "/Settings");
            File.WriteAllText(Application.dataPath + "/Settings/settings.json", jsonString);
        }
        
    }

    public static GameProfile readGameProfile(string jsonString)
    {
        try
        {
            TextAsset file = Resources.Load(jsonString) as TextAsset;
            return JsonUtility.FromJson<GameProfile>(file.text);
        }
        catch (NullReferenceException)
        {
            
            return null;
        }
    }

    public static void WriteGameProfile(GameProfile profile)
    {
        string jsonString = JsonUtility.ToJson(profile);
        if (Directory.Exists(Application.dataPath + "/Profiles"))
        {
            File.WriteAllText(Application.dataPath + "/Profiles/profile.json", jsonString);
        }
        else
        {
            Directory.CreateDirectory(Application.dataPath + "/Profiles");
            File.WriteAllText(Application.dataPath + "/Profiles/profile.json", jsonString);
        }
    }

    public static void DeleteGameProfile()
    {
        Directory.Delete(Application.dataPath + "/Profiles", true);
    }


}

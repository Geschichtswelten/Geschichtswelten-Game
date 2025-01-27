using System;
using System.IO;
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
            Debug.Log("No settings file found");
            return null;
        }
    }

    public static void WriteSettings(SettingsClass settings)
    {
        string jsonString = JsonUtility.ToJson(settings);
        if (Directory.Exists(Application.persistentDataPath + "/Settings"))
        {
            File.WriteAllText(Application.persistentDataPath + "/Settings/settings.json", jsonString);
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Settings");
            File.WriteAllText(Application.persistentDataPath + "/Settings/settings.json", jsonString);
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
        if (Directory.Exists(Application.persistentDataPath + "/Profiles"))
        {
            File.WriteAllText(Application.persistentDataPath + "/Profiles/profile.json", jsonString);
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Profiles");
            File.WriteAllText(Application.persistentDataPath + "/Profiles/profile.json", jsonString);
        }
    }

    public static void DeleteGameProfile()
    {
        Directory.Delete(Application.persistentDataPath + "/Profiles", true);
    }


}

using System;
using System.IO;
using UnityEngine;

public class JsonHandler : MonoBehaviour
{

    public static SettingsClass ReadSettings(string jsonString)
    {
        try
        {
            string settings = File.ReadAllText(jsonString);
            if(settings == "") return null;
            return JsonUtility.FromJson<SettingsClass>(settings);
        }
        catch (Exception)
        {
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
            string file = File.ReadAllText(jsonString);
            if(file == "") return null;
            return JsonUtility.FromJson<GameProfile>(file);
        }
        catch (Exception)
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

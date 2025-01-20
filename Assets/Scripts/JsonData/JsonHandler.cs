using System;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class JsonHandler : MonoBehaviour
{

    public static SettingsClass ReadSettings(string jsonString)
    {
        try
        {
            TextAsset file = AssetDatabase.LoadAssetAtPath(jsonString, typeof(TextAsset)) as TextAsset;
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
        TextAsset file = new TextAsset(jsonString);
        AssetDatabase.DeleteAsset("Assets/Settings/player_settings.asset");
        AssetDatabase.CreateAsset(file, "Assets/Settings/player_settings.asset");
    }

    public static GameProfile readGameProfile(string jsonString)
    {
        try
        {
            TextAsset file = AssetDatabase.LoadAssetAtPath(jsonString, typeof(TextAsset)) as TextAsset;
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
        TextAsset file = new TextAsset(jsonString);
        AssetDatabase.DeleteAsset("Assets/profile.asset");
        AssetDatabase.CreateAsset(file, "Assets/profile.asset");
    }

    public static void DeleteGameProfile()
    {
        AssetDatabase.DeleteAsset("Assets/profile.asset");
    }


}

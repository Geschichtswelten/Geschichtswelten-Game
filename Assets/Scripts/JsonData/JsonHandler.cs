using System;
using UnityEditor;
using UnityEngine;

public class JsonHandler : MonoBehaviour
{

    public static GameProfile readGameProfile(string jsonString)
    {
        try
        {
            TextAsset file = Resources.Load(jsonString) as TextAsset;
            return JsonUtility.FromJson<GameProfile>(file.text);
        }
        catch (NullReferenceException e)
        {
            return null;
        }
    }

    public static void WriteGameProfile(GameProfile profile)
    {
        string jsonString = JsonUtility.ToJson(profile);
        TextAsset file = new TextAsset(jsonString);
        AssetDatabase.DeleteAsset("Assets/profile.json");
        AssetDatabase.CreateAsset(file, "Assets/profile.json");
    }


}

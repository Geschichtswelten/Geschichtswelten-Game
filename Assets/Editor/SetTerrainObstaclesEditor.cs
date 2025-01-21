using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetTerrainObstaclesEditor : EditorWindow
{

    [MenuItem("Tools/Set Terrain Tree Obstacles")]
    
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<SetTerrainObstaclesEditor>("Set Tree Terrain Obstacles");
    }

    private void OnGUI()
    {
        GUILayout.Label("Set Tree Terrain Obstacles", EditorStyles.boldLabel);

        if (GUILayout.Button("Bake"))
        {
            BakeObstacles();
        }

        GUILayout.Label("add component", EditorStyles.boldLabel);

        if (GUILayout.Button("add"))
        {
            AddComp();
        }

        GUILayout.Label("spawn Mushrooms", EditorStyles.boldLabel);
        
        if(GUILayout.Button("add"))
        {
            AddShrooms();
        }

    }

    private void AddShrooms()
    {
        Scene[] scenes = new Scene[SceneManager.loadedSceneCount];
        for (int i = 0; i < SceneManager.loadedSceneCount; i++)
        {
            scenes[i] = SceneManager.GetSceneAt(i);
        }
        List<Terrain> terrains = new List<Terrain>();

        foreach (Scene scene in scenes)
        {
            GameObject[] objects = scene.GetRootGameObjects();
            foreach (GameObject obj in objects)
            {
                if (obj.TryGetComponent<Terrain>(out Terrain t))
                {
                    terrains.Add(t);
                }
            }
        }

        GameObject ob = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mushrooms.prefab") as GameObject;

        // Call the function to set terrain obstacles
        //SetTerrainObstaclesStatic.BakeTreeObstacles(terrains);

        SetTerrainObstaclesStatic.AddMushrooms(terrains, ob);
    }

    private void BakeObstacles()
    {
        Scene[] scenes = new Scene[SceneManager.loadedSceneCount];
        for (int i = 0; i < SceneManager.loadedSceneCount; i++)
        {
            scenes[i] = SceneManager.GetSceneAt(i);
        }
        List<Terrain> terrains = new List<Terrain>();

        foreach (Scene scene in scenes)
        {
            GameObject[] objects = scene.GetRootGameObjects();
            foreach (GameObject obj in objects)
            {
                if (obj.TryGetComponent<Terrain>(out Terrain t))
                {
                    terrains.Add(t);
                }
            }
        }
        Debug.Log(terrains.Count);
        // Call the function to set terrain obstacles
        SetTerrainObstaclesStatic.BakeTreeObstacles(terrains);

        //SetTerrainObstaclesStatic.WriteSavestate(terrains);
    }

    private void AddComp()
    {
        Scene[] scenes = new Scene[SceneManager.loadedSceneCount];
        for (int i = 0; i < SceneManager.loadedSceneCount; i++)
        {
            scenes[i] = SceneManager.GetSceneAt(i);
        }
        List<Terrain> terrains = new List<Terrain>();

        foreach (Scene scene in scenes)
        {
            GameObject[] objects = scene.GetRootGameObjects();
            foreach (GameObject obj in objects)
            {
                if (obj.TryGetComponent<Terrain>(out Terrain t))
                {
                    terrains.Add(t);
                }
            }
        }
        
        // Call the function to set terrain obstacles
        //SetTerrainObstaclesStatic.BakeTreeObstacles(terrains);

        SetTerrainObstaclesStatic.SetScripts(terrains);
    }

}

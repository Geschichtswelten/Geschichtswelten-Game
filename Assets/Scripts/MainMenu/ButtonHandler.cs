using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public static SettingsClass settings = new SettingsClass();
    public static GameProfile profile = null;
    public static event Action OnSettingsChanged;

    private void Awake()
    {
        ButtonHandler[] ls = Resources.FindObjectsOfTypeAll<ButtonHandler>();
        if (ls.Length > 1)
        {
            if (ls[0] == this)
            {
                Destroy(ls[1].gameObject);
            }
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadSettings();
    }

    private void LoadSettings()
    {
        SettingsClass settingsClass = JsonHandler.ReadSettings("Assets/Settings/player_settings.asset");
        settingsClass ??= new SettingsClass(100f, 100f, 100f, 0);
        settings = settingsClass;
        OnSettingsChanged?.Invoke();
    }

    public static void InvokeOnSettingsChanged()
    {
        OnSettingsChanged?.Invoke();
    }

    //Setter for SettingsClass

    /*
     *
     *TODO: Loading Screen and load remaining values
     *
     */
    public void LoadGame()
    {
        StartCoroutine(LoadSceneAsync());
    }

    /*
     *
     *TODO: Loading Screen, load the correct scene, e.g. the cutscenes
     *
     */

    public void CreateNewGame()
    {
        StartCoroutine(LoadNewGameAsync());
    }

    public static IEnumerator LoadNewGameAsync()
    {
        
        //ProgressBar

        
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(392);
        loadScene.allowSceneActivation = false;
        profile = new GameProfile();
        JsonHandler.WriteGameProfile(profile);
        while (!loadScene.isDone)
        {
            Debug.Log(loadScene.progress);
            yield return new WaitForSeconds(0.5f);
            if (loadScene.progress >= 0.9f)
            {
                loadScene.allowSceneActivation = true;
            }
        }

    }


    IEnumerator LoadSceneAsync()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(392);
        loadScene.allowSceneActivation = false;
        //ProgressBar
        profile = JsonHandler.readGameProfile("Assets/profile.asset");

        if (profile == null)
        {
            Debug.Log("No profile found, creating a new one");
            CreateNewGame();
            //Have to test that
            yield break;
        }
        
        while (!loadScene.isDone)
        {
            Debug.Log(loadScene.progress);
            yield return new WaitForSeconds(0.5f);
            if (loadScene.progress >= 0.9f)
            {
                loadScene.allowSceneActivation = true;
            }
        }

        Scene actScene = SceneManager.GetSceneByName("GameScene_0");
        GameObject[] roots = actScene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            if (root.CompareTag("PlayerWrapper"))
            {
                Debug.Log("FoundPlayer");
                root.GetComponent<WrapperScript>().LoadProfile(profile);
                root.GetComponentInChildren<PlayerBehaviour>().freeze = true;
                yield return new WaitForSeconds(2f);
                root.GetComponentInChildren<PlayerBehaviour>().freeze = false;
            }
        }

        

    }

    



}

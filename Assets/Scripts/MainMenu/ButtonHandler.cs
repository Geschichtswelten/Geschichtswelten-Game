using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ButtonHandler : MonoBehaviour
{
    public static SettingsClass settings = new SettingsClass();
    public static GameProfile profile = null;
    public static event Action OnSettingsChanged;

    public GameObject loadingScreen;
    public VideoPlayer videoPlayer;
    public AudioClip vidAudioClip;
    void Start()
    {
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
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

    public GameObject SetLoadingScreenActive()
    {
        loadingScreen.SetActive(true);
        return loadingScreen;
    }

    //Setter for SettingsClass

    /*
     *
     *TODO: Loading Screen and load remaining values
     *
     */
    public void LoadGame()
    {
        SetLoadingScreenActive();
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

    IEnumerator LoadNewGameAsync()
    {
        
        //ProgressBar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        SetLoadingScreenActive();
        yield return new WaitForSeconds(2);
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(393);
        loadScene.allowSceneActivation = false;
        while (!loadScene.isDone)
        {
            yield return new WaitForSeconds(0.01f);
            if (loadScene.progress == 0.9f)
            {
                var canv = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
                foreach (Canvas obj in canv)
                {
                    obj.gameObject.SetActive(false);
                }
                var worldMusicScript = FindAnyObjectByType<WorldMusicScript>();
                worldMusicScript.source.clip = vidAudioClip;
                worldMusicScript.source.volume = settings.masterVolume * settings.dialogueVolume;
                videoPlayer.SetDirectAudioVolume(0, settings.masterVolume * settings.dialogueVolume);
                worldMusicScript.source.Play();
                videoPlayer.Play();
                yield return new WaitUntil(()=>!videoPlayer.isPlaying);
                loadScene.allowSceneActivation = true;
            }
        }

    }


    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(1);
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(392, LoadSceneMode.Additive);
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
        SetLoadingScreenActive();
        while (!loadScene.isDone)
        {
            yield return new WaitForSeconds(0.01f);
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
                yield return new WaitForSeconds(2);
                root.GetComponent<WrapperScript>().LoadPosition(profile);
            }
        }
        
        yield return new WaitForSeconds(5);
        SceneManager.UnloadSceneAsync(0);


    }

    



}

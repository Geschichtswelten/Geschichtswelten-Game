using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public static SettingsClass settings = new SettingsClass();
    public static GameProfile profile = null;
    public static event Action OnSettingsChanged;

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
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(392);
        profile = new GameProfile();
        profile.playerPosY = 55f;
        profile.playerPosZ = 100f;
        profile.playerPosX = 100f;
        profile.playerRotX = 0f;
        profile.playerRotY = 0f;
        profile.playerRotZ = 0f;
        JsonHandler.WriteGameProfile(profile);
    }

    IEnumerator LoadSceneAsync()
    {
         profile = JsonHandler.readGameProfile("Assets/profile.asset");

        if (profile == null)
        {
            Debug.Log("No profile found, creating a new one");
            CreateNewGame();
            //Have to test that
            yield break;
        }
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(392);

        while (!loadScene.isDone)
        {
            yield return new WaitForSeconds(1f);
        }

        Scene actScene = SceneManager.GetSceneByName("GameScene_0");
        GameObject[] roots = actScene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            if (root.CompareTag("Player"))
            {
                Debug.Log("Found a profile");
                root.transform.position = new Vector3(profile.playerPosX, profile.playerPosY, profile.playerPosZ);
                root.transform.rotation = new Quaternion(profile.playerRotX, profile.playerRotY, profile.playerRotZ, 1);
                //load the remaining values once they are implemented
            }
        }

        

    }
}

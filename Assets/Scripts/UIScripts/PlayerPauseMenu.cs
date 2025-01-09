using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerPauseMenu : MonoBehaviour
{
    [Header("Own Children")]
    [SerializeField] private GameObject _canvas;
    [Header("Player Stuff")]
    [SerializeField] private GameObject _player;

    //Player Script for items and camps missing
    private DayNightCycle _cycle;
    private void Start()
    {
        _cycle = FindFirstObjectByType<DayNightCycle>();
        _canvas.SetActive(false);
    }

    public void ActivatePauseMenu()
    {
        while (Time.timeScale != 0f)
        {
            Time.timeScale -= Time.deltaTime;
            if ( Time.timeScale < 0f ) Time.timeScale = 0f;
        }

        _canvas.SetActive( true );
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReturnToGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _canvas.SetActive( false );

        while (Time.timeScale != 1f)
        {
            Time.timeScale += Time.deltaTime;
            if (Time.timeScale > 1f) Time.timeScale = 1f;
        }
    }

    public void SaveGame()
    {
        int[][] arr = new int[0][];
        GameProfile profile = new GameProfile(_player.transform.position.x, _player.transform.position.y, _player.transform.position.z, 
            _player.transform.rotation.x, _player.transform.position.y, _player.transform.position.z, arr, false, false, false, _cycle.time, _cycle.days);
        JsonHandler.WriteGameProfile(profile);
    }

    public void SaveAndQuitToMainMenu()
    {
        SaveGame();
        SceneManager.LoadScene(0);
    }

    public void SaveAndQuitGame()
    {
        SaveGame();
        Application.Quit();
    }

}

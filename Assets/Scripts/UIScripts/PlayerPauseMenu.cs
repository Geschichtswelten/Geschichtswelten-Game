using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerPauseMenu : MonoBehaviour
{
    [Header("Own Children")]
    [SerializeField] private GameObject _canvas;
    [Header("Player Stuff")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Inventory _playerInventory;
    [SerializeField] private Inventory _hotbar;
    [SerializeField] private Inventory _characterSlots;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider dialogueSlider;
    [SerializeField] private Slider mouseSensitivitySlider;

    public bool isTutorial = false;

    //Player Script for items and camps missing
    private DayNightCycle _cycle;
    private void Start()
    {
        _cycle = FindFirstObjectByType<DayNightCycle>();
        _canvas.SetActive(false);
    }

    private void OnEnable()
    {
        ButtonHandler.OnSettingsChanged += HandleSettingsChanged;
    }

    private void OnDisable()
    {
        ButtonHandler.OnSettingsChanged -= HandleSettingsChanged;
    }

    private void HandleSettingsChanged()
    {
        if (masterSlider != null && dialogueSlider != null && musicSlider != null && mouseSensitivitySlider != null)
        {
            masterSlider.value = ButtonHandler.settings.masterVolume;
            dialogueSlider.value = ButtonHandler.settings.dialogueVolume;
            musicSlider.value = ButtonHandler.settings.musicVolume;
            mouseSensitivitySlider.value = ButtonHandler.settings.mouseSensitivity;
            
        }

    }

    public void ActivatePauseMenu()
    {
        _canvas.SetActive(true);
        masterSlider.value = ButtonHandler.settings.masterVolume;
        musicSlider.value = ButtonHandler.settings.musicVolume;
        dialogueSlider.value = ButtonHandler.settings.dialogueVolume;
        mouseSensitivitySlider.value = ButtonHandler.settings.mouseSensitivity;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void ReturnToGame()
    {
        _canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public bool isPaused => _canvas.activeSelf;
    public void tooglePauseMenu()
    {
        if (_canvas.activeSelf)
        {
            ReturnToGame();
        }
        else
        {
            ActivatePauseMenu();
        }
    }

    public void SaveGame()
    {
        if (isTutorial) return;
        _hotbar.updateItemList();
        _characterSlots.updateItemList();
        _playerInventory.updateItemList();
        //4 R�stung, 5 Hotbar, 25 Inventar
        SerializedList<SerializedList<int>> player_items = new SerializedList<SerializedList<int>>(new List<SerializedList<int>>());
        for (int i = 0; i < 34; i++) 
        {
            player_items.list.Add(new SerializedList<int>(new List<int>()));
            if(i < 4)
            {
                Item item = _characterSlots.GetItemForSlot(i);
                if(item != null)
                {
                    player_items.list[i].list.Add(item.itemID);
                    player_items.list[i].list.Add(item.itemValue);
                }
                else
                {
                    player_items.list[i].list.Add(-1);
                    player_items.list[i].list.Add(0);
                }
                
            }else if (i < 9)
            {
                Item item = _hotbar.GetItemForSlot(i-4);
                if (item != null)
                {
                    player_items.list[i].list.Add(item.itemID);
                    player_items.list[i].list.Add(item.itemValue);
                }
                else
                {
                    player_items.list[i].list.Add(-1);
                    player_items.list[i].list.Add(0);
                }
            }
            else
            {
                Item item = _playerInventory.GetItemForSlot(i - 9);
                if (item != null)
                {
                    player_items.list[i].list.Add(item.itemID);
                    player_items.list[i].list.Add(item.itemValue);
                }
                else
                {
                    player_items.list[i].list.Add(-1);
                    player_items.list[i].list.Add(0);
                }
            }
        }

        Vector3 rot = _player.transform.rotation.eulerAngles;

        GameProfile profile = new GameProfile(_player.transform.position.x, _player.transform.position.y, _player.transform.position.z, 
            rot.x, rot.y, rot.z, player_items, false, false, false, _cycle.time, _cycle.days);
        JsonHandler.WriteGameProfile(profile);
    }

    public void SaveAndQuitToMainMenu()
    {
        SaveGame();
        Time.timeScale = 1;
        Destroy(FindAnyObjectByType<ButtonHandler>().gameObject);
        Destroy(FindAnyObjectByType<WorldMusicScript>().gameObject);
        SceneManager.LoadScene(0);
    }

    public void SaveAndQuitGame()
    {
        SaveGame();
        Application.Quit();
    }

    public void UpdateMasterVolume()
    {
        
            ButtonHandler.settings.SetMasterVolume(masterSlider.value);
            ButtonHandler.InvokeOnSettingsChanged();
        
    }

    public void UpdateMusicVolume()
    {
        ButtonHandler.settings.SetMusicVolume(musicSlider.value);
        ButtonHandler.InvokeOnSettingsChanged();
    }

    public void UpdateDialogueVolume()
    {
        ButtonHandler.settings.SetDialogueVolume(dialogueSlider.value);
        ButtonHandler.InvokeOnSettingsChanged();
    }

    public void UpdateMouseSensitivity()
    {
        ButtonHandler.settings.SetMouseSensitivity(mouseSensitivitySlider.value);
        ButtonHandler.InvokeOnSettingsChanged();
    }

    public void SaveSettings()
    {
        JsonHandler.WriteSettings(ButtonHandler.settings);
    }
}

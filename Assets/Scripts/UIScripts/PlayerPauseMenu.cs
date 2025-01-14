using NUnit.Framework;
using System.Collections.Generic;
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
    [SerializeField] private Inventory _playerInventory;
    [SerializeField] private Inventory _hotbar;
    [SerializeField] private Inventory _characterSlots;

    //Player Script for items and camps missing
    private DayNightCycle _cycle;
    private void Start()
    {
        _cycle = FindFirstObjectByType<DayNightCycle>();
        _canvas.SetActive(false);
    }

    public void ActivatePauseMenu()
    {
        _canvas.SetActive(true);
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
        List<Item> armorList = _characterSlots.ItemsInInventory;
        List<Item> hotbarList = _hotbar.ItemsInInventory;
        List<Item> inventoryList = _playerInventory.ItemsInInventory;
        //4 R�stung, 5 Hotbar, 25 Inventar
        int[][] items_inv = new int[34][];
        for (int i = 0; i < items_inv.Length; i++) 
        {
            items_inv[i] = new int[2];
            if(i < 4)
            {
                if(armorList.Count > 0)
                {
                    Item item = armorList[0];
                    armorList.RemoveAt(0);
                    items_inv[i][0] = item.itemID;
                    items_inv[i][1] = item.itemValue;
                }
                else
                {
                    items_inv[i][0] = -1;
                    items_inv[i][1] = 0;
                }
                
            }else if (i < 9)
            {
                if (hotbarList.Count > 0)
                {
                    Item item = hotbarList[0];
                    hotbarList.RemoveAt(0);
                    items_inv[i][0] = item.itemID;
                    items_inv[i][1] = item.itemValue;
                }
                else
                {
                    items_inv[i][0] = -1;
                    items_inv[i][1] = 0;
                }
            }
            else
            {
                if (inventoryList.Count > 0)
                {
                    Item item = inventoryList[0];
                    inventoryList.RemoveAt(0);
                    items_inv[i][0] = item.itemID;
                    items_inv[i][1] = item.itemValue;
                }
                else
                {
                    items_inv[i][0] = -1;
                    items_inv[i][1] = 0;
                }
            }
        }

        Vector3 rot = _player.transform.rotation.eulerAngles;

        GameProfile profile = new GameProfile(_player.transform.position.x, _player.transform.position.y, _player.transform.position.z, 
            rot.x, rot.y, rot.z, items_inv, false, false, false, _cycle.time, _cycle.days);
        JsonHandler.WriteGameProfile(profile);
    }

    public void SaveAndQuitToMainMenu()
    {
        SaveGame();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void SaveAndQuitGame()
    {
        SaveGame();
        Application.Quit();
    }
}

﻿using System;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Linq;

public class Hotbar : MonoBehaviour
{
     [SerializeField] private Inventory items;
     [SerializeField] private PlayerBehaviour player;
     
     public KeyCode[] keyCodesForSlots = new KeyCode[999];
     public int slotsInTotal;
     

#if UNITY_EDITOR
    [MenuItem("Master System/Create/Hotbar")]        //creating the menu item
    public static void menuItemCreateInventory()       //create the inventory at start
    {
        GameObject Canvas = null;
        if (GameObject.FindGameObjectWithTag("Canvas") == null)
        {
            GameObject inventory = new GameObject();
            inventory.name = "Inventories";
            Canvas = (GameObject)Instantiate(Resources.Load("Prefabs/Canvas - Inventory") as GameObject);
            Canvas.transform.SetParent(inventory.transform, true);
            GameObject panel = (GameObject)Instantiate(Resources.Load("Prefabs/Panel - Hotbar") as GameObject);
            panel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            panel.transform.SetParent(Canvas.transform, true);
            GameObject draggingItem = (GameObject)Instantiate(Resources.Load("Prefabs/DraggingItem") as GameObject);
            Instantiate(Resources.Load("Prefabs/EventSystem") as GameObject);
            draggingItem.transform.SetParent(Canvas.transform, true);
            Inventory inv = panel.AddComponent<Inventory>();
            panel.AddComponent<InventoryDesign>();
            panel.AddComponent<Hotbar>();
            inv.getPrefabs();
        }
        else
        {
            GameObject panel = (GameObject)Instantiate(Resources.Load("Prefabs/Panel - Hotbar") as GameObject);
            panel.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, true);
            panel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            Inventory inv = panel.AddComponent<Inventory>();
            panel.AddComponent<Hotbar>();
            DestroyImmediate(GameObject.FindGameObjectWithTag("DraggingItem"));
            GameObject draggingItem = (GameObject)Instantiate(Resources.Load("Prefabs/DraggingItem") as GameObject);
            draggingItem.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, true);
            panel.AddComponent<InventoryDesign>();
            inv.getPrefabs();
        }
    }
#endif

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        items = GetComponent<Inventory>();
    }

    void Update()
    {
        for (int i = 0; i < slotsInTotal; i++)
        {
            if (Input.GetKeyDown(keyCodesForSlots[i]))
            {
                /*if (transform.GetChild(1).GetChild(i).childCount != 0 && transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.itemType != ItemType.UFPS_Ammo)
                {
                    if (transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ConsumeItem>().duplication != null && transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ItemOnObject>().item.maxStack == 1)
                    {
                        Destroy(transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ConsumeItem>().duplication);
                    }
                    transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<ConsumeItem>().consumeIt();
                } //what da hellll*/
                
                /*  TODO!!
                 Hier Hotbarslot umfärben/markieren whatever */
                ushort item_id = (ushort)items.GetItemIdForSlotInHotbar(i);
                //Debug.Log("Trying to equip item " + item_id);
                player.EquipItem(item_id, i);
                //Debug.Log("hotbar contains " + items.getItemList().Count + " items");
            }
        }
    }

    public int getSlotsInTotal()
    {
        Inventory inv = GetComponent<Inventory>();
        if (inv != null) return slotsInTotal = inv.width * inv.height;
        return 0;
    }
}

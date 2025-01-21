using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class StorageInventory : MonoBehaviour
{

    [SerializeField]
    public GameObject inventory;

    [SerializeField]
    public List<Item> storageItems = new List<Item>();

    [SerializeField]
    private ItemDataBaseList itemDatabase;

    [SerializeField]
    public int distanceToOpenStorage;

    public float timeToOpenStorage;

    private InputManager inputManagerDatabase;

    float startTimer;
    float endTimer;
    bool showTimer;

    public int itemAmount;

    Tooltip tooltip;
    public Inventory inv;

    [SerializeField] public GameObject player;

    static Image timerImage;
    static GameObject timer;

    bool closeInv;

    bool showStorage;

    public void addItemToStorage(int id, int value)
    {
        Item item = itemDatabase.getItemByID(id);
        item.itemValue = value;
        storageItems.Add(item);
    }



    void Start()
    {
        if (inputManagerDatabase == null)
            inputManagerDatabase = (InputManager)Resources.Load("InputManager");
        
        inv = inventory.GetComponent<Inventory>();
        ItemDataBaseList inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");

        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
            tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();

    }

    public void setImportantVariables()
    {
        if (itemDatabase == null)
            itemDatabase = (ItemDataBaseList)Resources.Load("ItemDatabase");
    }

    void Update()
    {
        if(player == null || inventory == null) return;

        float distance = Vector3.Distance(this.gameObject.transform.position, player.transform.position);

        if (showTimer)
        {
            if (timerImage != null)
            {
                timer.SetActive(true);
                float fillAmount = (Time.time - startTimer) / timeToOpenStorage;
                timerImage.fillAmount = fillAmount;
            }
        }

        if (distance <= distanceToOpenStorage && Input.GetKeyDown(inputManagerDatabase.StorageKeyCode))
        {
            showStorage = !showStorage;
            StartCoroutine(OpenInventoryWithTimer());
        }

        if (distance > distanceToOpenStorage && showStorage)
        {
            showStorage = false;
            if (inventory.activeSelf)
            {
                storageItems.Clear();
                setListofStorage();
                inventory.SetActive(false);
                inv.deleteAllItems();
            }
            tooltip.deactivateTooltip();
        }
    }

    IEnumerator OpenInventoryWithTimer()
    {
        if (showStorage)
        {
            startTimer = Time.time;
            showTimer = true;
            yield return new WaitForSeconds(timeToOpenStorage);
            if (showStorage)
            {
                inv.ItemsInInventory.Clear();
                inventory.SetActive(true);
                addItemsToInventory();
                showTimer = false;
                if (timer != null)
                    timer.SetActive(false);
            }
        }
        else
        {
            storageItems.Clear();
            setListofStorage();
            inventory.SetActive(false);
            inv.deleteAllItems();
            tooltip.deactivateTooltip();
        }


    }



    void setListofStorage()
    {
        Inventory inv = inventory.GetComponent<Inventory>();
        storageItems = inv.getItemList();
    }

    void addItemsToInventory()
    {
        Inventory iV = inventory.GetComponent<Inventory>();
        for (int i = 0; i < storageItems.Count; i++)
        {
            iV.addItemToInventory(storageItems[i].itemID, storageItems[i].itemValue);
        }
        iV.stackableSettings();
    }






}

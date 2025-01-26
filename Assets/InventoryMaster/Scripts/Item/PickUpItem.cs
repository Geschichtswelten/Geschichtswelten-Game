
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public Item item;
    private Inventory _inventory;
    private GameObject _player;
    [Range(1, float.PositiveInfinity)] [SerializeField] private static float _pickUpRange = 5;

    [SerializeField] private int initItemIdForEditor = 0;
    // Use this for initialization

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null) {
            _inventory = _player.GetComponent<PlayerInventory>().inventory.GetComponent<Inventory>();
            if (initItemIdForEditor > 0 && (item.itemID == 0 || item.itemName.Length == 0)) item = _inventory.GetItemFromId(initItemIdForEditor);
        }
        else
        {
            //Debug.Log("Player is not.");
        }
    }

    void Update()
    {
        if (_inventory != null && Input.GetKeyDown(KeyCode.E))
        {
            var distance = Vector3.Distance(_player.transform.position, transform.position);
            var playerDir = _player.transform.forward;
            playerDir.y = 0;
            
            var playerToItem = transform.position - _player.transform.position;
            playerToItem.y = 0;
            
            var angle = Vector3.Angle(playerDir, playerToItem);
            //Debug.Log(angle + "," + distance);
            
            if (distance < _pickUpRange && angle < 40) PickUpByPlayer();
            /*
            float distance = Vector3.Distance(this.gameObject.transform.position, _player.transform.position);

            if (distance <= 3)
            {
                bool check = _inventory.checkIfItemAllreadyExist(item.itemID, item.itemValue);
                if (check)
                    Destroy(this.gameObject);
                else if (_inventory.ItemsInInventory.Count < (_inventory.width * _inventory.height))
                {
                    _inventory.addItemToInventory(item.itemID, item.itemValue);
                    _inventory.updateItemList();
                    _inventory.stackableSettings();
                    Destroy(this.gameObject);
                }

            }*/
        }
    }

    public void PickUpByPlayer()
    {
        if (!_inventory.TryAddItemsToExistingStack(item.itemID, item.itemValue))
        {
            if (_inventory.ItemsInInventory.Count < (_inventory.width * _inventory.height))
            {
                _inventory.addItemToInventory(item.itemID, item.itemValue);
                _inventory.updateItemList();
                _inventory.stackableSettings();
            }
        }
        Destroy(gameObject);
    }
}

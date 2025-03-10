using UnityEngine;

public class WrapperScript : MonoBehaviour
{
    [Header("Inventorys")]
    [SerializeField] private Inventory hotbar;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Inventory armor;
    [Header("Player")]
    [SerializeField] private PlayerBehaviour playerBehaviour;


    public void LoadPosition(GameProfile profile)
    {
        Vector3 pos = new Vector3(profile.playerPosX, profile.playerPosY, profile.playerPosZ);
        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(profile.playerRotX, profile.playerRotY, profile.playerRotZ);

        playerBehaviour.LoadPosition(pos, rot);
    }
    public void LoadProfile(GameProfile profile)
    {
        inventory.DeleteAllItems();
        //Load Inventory
        int ignore = 0;
        for (int i = 0; i < 34; i++)
        {
            if (i == 4)
            {
                ignore = 0;
            }
            if (i < 4)
            {
                if (profile.playerItems.list[i].list[0] != -1)
                {
                    armor.addItemToInventory(ignore, profile.playerItems.list[i].list[0], profile.playerItems.list[i].list[1]);
                    PlayerBehaviour.Armor arm = new PlayerBehaviour.Armor
                    {
                        ItemId = profile.playerItems.list[i].list[0],
                        Multiplier = armor.GetItemFromId(profile.playerItems.list[i].list[0]).itemAttributes[0].attributeValue / 100f
                    };
                    playerBehaviour.RegisterArmor(arm);

                }
                else
                {
                    ignore += 1;
                }

            }
            else if (i < 9)
            {
                if (profile.playerItems.list[i].list[0] != -1)
                {
                    hotbar.addItemToInventory(ignore, profile.playerItems.list[i].list[0], profile.playerItems.list[i].list[1]);
                }
                else
                {
                    ignore += 1;
                }
            }
            else
            {
                if (profile.playerItems.list[i].list[0] != -1)
                {
                    inventory.addItemToInventory(profile.playerItems.list[i].list[0], profile.playerItems.list[i].list[1]);
                    inventory.updateItemList();
                    inventory.stackableSettings();
                }
            }
        }

        Vector3 pos = new Vector3(profile.playerPosX, profile.playerPosY, profile.playerPosZ);
        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(profile.playerRotX, profile.playerRotY, profile.playerRotZ);

        playerBehaviour.LoadPosition(pos, rot);
        

    }


}

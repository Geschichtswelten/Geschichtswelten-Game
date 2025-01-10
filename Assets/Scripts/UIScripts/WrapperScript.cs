using UnityEngine;

public class WrapperScript : MonoBehaviour
{
    [Header("Inventorys")]
    [SerializeField] private Inventory hotbar;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Inventory armor;
    [Header("Player")]
    [SerializeField] private PlayerBehaviour playerBehaviour;

    public void LoadProfile(GameProfile profile)
    {
        //Load Inventory
        int ignore = 0;
        for (int i = 0; i < 34; i++)
        {
            if (i == 4 || i == 9)
            {
                ignore = 0;
            }
            if (i < 4)
            {
                if (profile.playerItems[i][0] != -1)
                {
                    armor.addItemToInventory(ignore, profile.playerItems[i][0], profile.playerItems[i][1]);
                }
                else
                {
                    ignore += 1;
                }

            }
            else if (i < 9)
            {
                if (profile.playerItems[i][0] != -1)
                {
                    hotbar.addItemToInventory(ignore, profile.playerItems[i][0], profile.playerItems[i][1]);
                }
                else
                {
                    ignore += 1;
                }
            }
            else
            {
                if (profile.playerItems[i][0] != -1)
                {
                    inventory.addItemToInventory(ignore, profile.playerItems[i][0], profile.playerItems[i][1]);
                }
                else
                {
                    ignore += 1;
                }
            }
        }

        Vector3 pos = new Vector3(profile.playerPosX, profile.playerPosY, profile.playerPosZ);
        Quaternion rot = new Quaternion(profile.playerRotX, profile.playerRotY, profile.playerRotZ, 1);

        playerBehaviour.LoadPosition(pos, rot);


    }


}

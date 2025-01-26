using DefaultNamespace;
using UnityEngine;

public class FoodBehaviour : ItemBehaviour
{
    private PlayerBehaviour player;
    [SerializeField] private float foodWorth = 30f;
    private Inventory hotbarInv;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        hotbarInv = player.GetHotbar();
    }

    public override void Action1()
    {
        Debug.Log("I am food");
        player.Eat(foodWorth);
        if (!hotbarInv.ConsumeItemAtSlot(player.activeHotbarSlot))
        {
            player.EquipItem(0);
            Destroy(gameObject);
        }
    }

    public override void Action2()
    {
        return;
    }
}

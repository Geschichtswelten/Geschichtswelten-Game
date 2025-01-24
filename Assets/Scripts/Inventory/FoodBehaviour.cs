using DefaultNamespace;
using UnityEngine;

public class FoodBehaviour : ItemBehaviour
{
    private PlayerBehaviour player;
    [SerializeField] private float foodWorth = 30f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }
    public override void action1()
    {
        Debug.Log("I am food");
        player.Eat(foodWorth);
    }

    public override void action2()
    {
        return;
    }
}

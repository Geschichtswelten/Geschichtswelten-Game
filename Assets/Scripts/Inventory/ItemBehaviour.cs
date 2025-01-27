using System;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(AnimationHandler), typeof(ItemSfxHandler))]
    public abstract class ItemBehaviour : MonoBehaviour
    {
        [SerializeField] protected ushort id;
        [SerializeField] protected ushort amt;
        [SerializeField] protected itemType type;
        [SerializeField] protected string name;
        public float damage;
        
        [SerializeField] protected AnimationHandler animationHandler;
        [SerializeField] protected ItemSfxHandler itemSfxHandler;

        public ItemBehaviour (ushort id, ushort amt, itemType type, string name)
        {
            this.id = id;
            this.amt = amt;
            this.type = type;
            this.name = name;
        }

        public ItemBehaviour()
        {
            id = ushort.MaxValue;
            amt = 1;
            type = itemType.consumable;
            name = "unnamed";
        }
        
        private void Awake()
        {
            if (animationHandler == null) animationHandler = GetComponent<AnimationHandler>();
            if (itemSfxHandler == null) itemSfxHandler = GetComponent<ItemSfxHandler>();
        }

        public abstract void Action1();
        public abstract void Action2();
        
        public ushort Id { get => id; set => id = value; }
        public ushort Amt { get => amt; set => amt = value; }
        public string Name { get => name; set => name = value; }
        public AnimationHandler AnimationHandler { get => animationHandler; set => animationHandler = value; }
        public itemType Type { get => type; set => type = value; }
    }

    public enum itemType
    {
        consumable,
        weapon,
        armor
    }
}
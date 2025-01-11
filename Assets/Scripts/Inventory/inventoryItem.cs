namespace DefaultNamespace
{
    public struct inventoryItem
    {

        public inventoryItem(ushort itemId, ushort quantity)
        {
            id = itemId;
            amount = quantity;
        }
        public ushort id { get;  set; }
        public ushort amount  { get; set; }
    }
}
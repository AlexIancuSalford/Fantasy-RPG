using RPG.UI.Inventory;
using UnityEngine;

namespace RPG.UI.Quest
{
    [System.Serializable]
    public struct Reward
    {
        [Min(1)] public int Number;
        public InventoryItem Item;
    }
}

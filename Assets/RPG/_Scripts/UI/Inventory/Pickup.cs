using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This is a script for a Unity game that allows for the pickup and handling of items in an inventory. It is attached to a
    /// game object that represents an item that can be picked up in the game world.
    /// 
    /// The Pickup class has several properties: Item is an InventoryItem object that represents the item being picked up, Number
    /// is an integer that represents the number of items being picked up (if the item is stackable), and Inventory is a reference
    /// to the player's inventory.
    /// 
    /// The Awake method is a Unity event that is called when the object the script is attached to is created. In this case, the
    /// Awake method is used to find the player game object and retrieve a reference to its Inventory component.
    /// 
    /// The Setup method is used to initialize the Item and Number properties when the item is created. If the item is not stackable,
    /// the number of items is set to 1.
    /// 
    /// The PickupItem method is called when the player picks up the item. It attempts to add the item to the first available empty
    /// slot in the player's inventory. If it is successful, the game object representing the item is destroyed.
    /// 
    /// The CanBePickedUp method returns a boolean value indicating whether the player has enough space in their inventory to pick up the item.
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        /// <summary>
        /// The item being picked up.
        /// </summary>
        public InventoryItem Item { get; private set; } = null;

        /// <summary>
        /// The number of items being picked up (if the item is stackable).
        /// </summary>
        public int Number { get; private set; } = 1;

        /// <summary>
        /// A reference to the player's inventory.
        /// </summary>
        private Inventory Inventory { get; set; } = null;

        /// <summary>
        /// Finds the player object and retrieves a reference to its inventory component.
        /// </summary>
        private void Awake()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Inventory = player.GetComponent<Inventory>();
        }

        /// <summary>
        /// Initializes the item and number properties when the item is created.
        /// </summary>
        /// <param name="item">The item being picked up.</param>
        /// <param name="number">The number of items being picked up.</param>
        public void Setup(InventoryItem item, int number)
        {
            Item = item;

            // If the item is not stackable, set the number to 1.
            if (!item.Stackable) { number = 1; }

            Number = number;
        }

        /// <summary>
        /// Attempts to add the item to the first available empty slot in the player's inventory.
        /// If successful, the game object representing the item is destroyed.
        /// </summary>
        public void PickupItem()
        {
            bool foundSlot = Inventory.AddToFirstEmptySlot(Item, Number);
            if (foundSlot) { Destroy(gameObject); }
        }

        /// <summary>
        /// Returns a boolean value indicating whether the player has enough space in their inventory to pick up the item.
        /// </summary>
        /// <returns>True if the player has enough space, false otherwise.</returns>
        public bool CanBePickedUp() { return Inventory.HasSpaceFor(Item); }
    }
}

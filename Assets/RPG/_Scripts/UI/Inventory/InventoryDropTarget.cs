using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// Handles spawning pickups when item dropped into the world.
    /// 
    /// Must be placed on the root canvas where items can be dragged. Will be
    /// called if dropped over empty space. 
    /// </summary>
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItem>
    {
        /// <summary>
        /// Adds the specified item to the player's inventory.
        /// </summary>
        /// <param name="item">The item to be added to the inventory.</param>
        /// <param name="count">The number of items to add to the inventory.</param>
        public void AddItemsToInventory(InventoryItem item, int count)
        {
            // Find the player object in the scene
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Get the ItemDropper component on the player and use its DropItem method to drop the item
            player.GetComponent<ItemDropper>().DropItem(item, count);
        }

        /// <summary>
        /// Gets the maximum number of the specified item that can be accepted.
        /// </summary>
        /// <param name="item">The item for which to get the maximum acceptable count.</param>
        /// <returns>The maximum number of the specified item that can be accepted.</returns>
        public int GetMaxAcceptableItemCount(InventoryItem item)
        {
            // Return the maximum value for an int to indicate that any number of items can be accepted
            return int.MaxValue;
        }
    }
}
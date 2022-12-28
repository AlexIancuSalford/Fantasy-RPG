using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script is a component for a game object in a Unity project that is designed to be a drop target for inventory items
    ///
    /// It has two functions:
    /// 
    /// AddItemsToInventory: This function is called when an inventory item is dropped on the drop target. It receives the item
    /// and a count of how many of the item should be added to the inventory. The function first finds the game object in the scene
    /// with the "Player" tag, which is assumed to be the player character. It then gets the ItemDropper component attached to that
    /// game object and calls its DropItem function, passing in the item and count as arguments. This likely adds the item to the
    /// player's inventory and updates the inventory UI.
    /// 
    /// GetMaxAcceptableItemCount: This function is called when an inventory item is being dragged over the drop target.
    /// It receives the item being dragged and returns an integer representing the maximum number of that item that the drop
    /// target will accept. In this case, the function returns int.MaxValue, which is the largest possible value for an integer
    /// in C#. This indicates that the drop target will accept any number of the given item.
    /// </summary>
    public class DropTarget : MonoBehaviour, IDragDestination<InventoryItem>
    {
        /// <summary>
        /// Adds the given inventory item to the player's inventory.
        /// </summary>
        /// <param name="item">The inventory item to be added.</param>
        /// <param name="count">The number of items to be added.</param>
        public void AddItemsToInventory(InventoryItem item, int count)
        {
            // Find the game object in the scene with the "Player" tag.
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Get the ItemDropper component attached to the player object and call its DropItem function.
            player.GetComponent<ItemDropper>().DropItem(item, count);
        }

        /// <summary>
        /// Gets the maximum number of the given inventory item that the drop target will accept.
        /// </summary>
        /// <param name="item">The inventory item being dragged over the drop target.</param>
        /// <returns>The maximum number of the given item that the drop target will accept.</returns>
        public int GetMaxAcceptableItemCount(InventoryItem item)
        {
            // Return the maximum possible value for an integer to indicate that the drop target will accept any number of the given item.
            return int.MaxValue;
        }
    }
}

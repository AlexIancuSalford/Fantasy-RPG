using RPG.Helper;
using RPG.Save;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script is a component for an object in a Unity game that handles dropping and spawning of inventory items.
    /// It is designed to be used with a system that has a concept of inventory items, as well as objects called "Pickup"
    /// that represent a specific item that has been dropped in the game world.
    /// 
    /// The component has a list of Pickup objects called "DroppedItems" that keeps track of the Pickup objects that
    /// have been spawned by this component. It has two methods for dropping items: "DropItem" and "SpawnPickup". The
    /// "DropItem" method takes an InventoryItem object and an optional integer representing the number of items to drop.
    /// It then calls the "SpawnPickup" method to spawn a Pickup object for the specified item at the object's current
    /// position, or at a position specified by the "GetDropLocation" method if it has been overridden.
    /// 
    /// The component also implements the "ISaveableEntity" interface, which means it has methods for saving and loading
    /// the state of the component. The "SaveState" method returns an object that contains information about the items
    /// that have been dropped by the component, including their item IDs, positions, and number of items. The "LoadState"
    /// method takes an object as input and uses it to recreate the dropped items by instantiating Pickup objects for the
    /// specified items at the specified positions and with the specified numbers.
    /// 
    /// Finally, the component has a method called "RemoveDestroyedDrops" that removes any Pickup objects from the "DroppedItems"
    /// list that have been destroyed. This is likely used to keep the list up to date and to avoid trying to reference
    /// destroyed objects.
    /// </summary>
    public class ItemDropper : MonoBehaviour, ISaveableEntity
    {
        /// <summary>
        /// A list of Pickup objects spawned by this component.
        /// </summary>
        private List<Pickup> DroppedItems { get; set; } = new List<Pickup>();

        /// <summary>
        /// Drops a specified number of the given inventory item.
        /// </summary>
        /// <param name="item">The inventory item to drop.</param>
        /// <param name="number">The number of items to drop.</param>
        public void DropItem(InventoryItem item, int number)
        {
            SpawnPickup(item, GetDropLocation(), number);
        }

        /// <summary>
        /// Drops a single instance of the given inventory item.
        /// </summary>
        /// <param name="item">The inventory item to drop.</param>
        public void DropItem(InventoryItem item)
        {
            SpawnPickup(item, GetDropLocation(), 1);
        }

        /// <summary>
        /// Gets the location at which to spawn a dropped item.
        /// </summary>
        /// <returns>The drop location.</returns>
        protected virtual Vector3 GetDropLocation()
        {
            // By default, the drop location is the object's current position.
            return transform.position;
        }

        /// <summary>
        /// Spawns a Pickup object for the given inventory item at the specified location.
        /// </summary>
        /// <param name="item">The inventory item to drop.</param>
        /// <param name="spawnLocation">The location at which to spawn the Pickup object.</param>
        /// <param name="number">The number of items in the Pickup object.</param>
        public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
        {
            Pickup pickup = item.SpawnPickup(spawnLocation, number);
            DroppedItems.Add(pickup);
        }

        #region ISaveableEntity implementation

        /// <summary>
        /// Saves the state of the component.
        /// </summary>
        /// <returns>An object containing information about the dropped items.</returns>

        public object SaveState()
        {
            RemoveDestroyedDrops();
            DropRecord[] droppedItemsList = new DropRecord[DroppedItems.Count];
            for (int i = 0; i < droppedItemsList.Length; i++)
            {
                droppedItemsList[i].ItemID = DroppedItems[i].Item.ItemID;
                droppedItemsList[i].Position = new Vector3f(DroppedItems[i].transform.position);
                droppedItemsList[i].Number = DroppedItems[i].Number;
            }
            return droppedItemsList;
        }

        /// <summary>
        /// Loads the state of the component.
        /// </summary>
        /// <param name="state">An object containing information about the dropped items.</param>
        public void LoadState(object state)
        {
            var droppedItemsList = (DropRecord[])state;
            foreach (var item in droppedItemsList)
            {
                var pickupItem = InventoryItem.GetFromID(item.ItemID);
                Vector3 position = item.Position;
                int number = item.Number;
                SpawnPickup(pickupItem, position, number);
            }
        }

        #endregion

        /// <summary>
        /// Removes any Pickup objects from the DroppedItems list that have been destroyed.
        /// </summary>

        private void RemoveDestroyedDrops()
        {
            List<Pickup> newList = new List<Pickup>();
            foreach (var item in DroppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            DroppedItems = newList;
        }
    }
}
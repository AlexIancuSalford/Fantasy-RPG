using RPG.Helper;
using RPG.Save;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        /// A list of records of dropped items from other scenes
        /// </summary>
        private List<DropRecord> OtherSceneDroppedItems = new List<DropRecord>();

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
            // Remove any dropped items that have been destroyed
            RemoveDestroyedDrops();

            // Get the build index of the current scene
            int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

            // Create a list of DropRecord objects for the dropped items in the DroppedItems list
            List<DropRecord> droppedItemsList = DroppedItems
                .Select(pickup => new DropRecord
                {
                    // Set the ItemID, Position, Number, and SceneBuildIndex for the DropRecord object
                    ItemID = pickup.Item.ItemID, 
                    Position = new Vector3f(pickup.transform.position), 
                    Number = pickup.Number, 
                    SceneBuildIndex = currentSceneBuildIndex,
                })
                .ToList();

            // Add the dropped items from other scenes to the list
            droppedItemsList.AddRange(OtherSceneDroppedItems);
            return droppedItemsList;
        }

        /// <summary>
        /// Loads the state of the component.
        /// </summary>
        /// <param name="state">An object containing information about the dropped items.</param>
        public void LoadState(object state)
        {
            // Get the build index of the current scene
            int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            // Create a list of DropRecord objects from the state object
            List<DropRecord> droppedItemsList = (List<DropRecord>)state;
            // Clear the OtherSceneDroppedItems list
            OtherSceneDroppedItems.Clear();

            // Iterate through each DropRecord object in the droppedItemsList
            foreach (DropRecord item in droppedItemsList)
            {
                // If the DropRecord object is from a different scene, add it to the OtherSceneDroppedItems list
                if (item.SceneBuildIndex != currentSceneBuildIndex)
                {
                    OtherSceneDroppedItems.Add(item);
                    continue;
                }

                // Get the InventoryItem object corresponding to the ItemID of the DropRecord object
                InventoryItem pickupItem = InventoryItem.GetFromID(item.ItemID);
                
                // Get the Position and Number values of the DropRecord object
                Vector3 position = item.Position;
                int number = item.Number;
                
                // Spawn the pickup with the SpawnPickup method
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
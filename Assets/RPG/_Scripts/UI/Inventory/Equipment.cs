using RPG.Save;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This is a script for an Equipment component in Unity. The Equipment component is responsible for managing a player character's
    /// equipped items.
    /// 
    /// The Equipment component has a Dictionary called EquippedItems that maps EquipLocation enum values to EquipableItem objects.
    /// The EquipLocation enum represents the different locations on a character where an item can be equipped, such as the head or
    /// the chest. The EquipableItem class likely represents items that can be equipped by the player character, such as weapons or armor.
    /// 
    /// The Equipment component also has an event called EquipmentUpdated which can be subscribed to by other objects in the game to
    /// be notified when the player character's equipped items are updated.
    /// 
    /// The Equipment component has several methods for interacting with the EquippedItems dictionary and the EquipmentUpdated event.
    /// The GetItemInSlot method returns the EquipableItem in a given EquipLocation, the AddItem method adds an EquipableItem to a specific
    /// EquipLocation, the RemoveItem method removes an EquipableItem from a specific EquipLocation, and the GetAllPopulatedSlots method
    /// returns a list of all EquipLocation values that have an EquipableItem equipped in them.
    /// 
    /// The Equipment component also implements the ISaveableEntity interface, which allows it to be saved and loaded as part of the game's
    /// save data. The SaveState method returns an object that represents the Equipment component's state in a form that can be serialized
    /// (converted to a string), and the LoadState method takes in an object that represents the component's state in a serialized form and
    /// uses it to restore the component's state.
    /// </summary>
    public class Equipment : MonoBehaviour, ISaveableEntity
    {
        /// <summary>
        /// A dictionary that maps EquipLocation enum values to EquipableItem objects, representing the items currently equipped by the player character.
        /// </summary>
        protected Dictionary<EquipLocation, EquipableItem> EquippedItems { get; set; } = new Dictionary<EquipLocation, EquipableItem>();

        /// <summary>
        /// An event that is raised when the player character's equipped items are updated.
        /// </summary>
        public event Action EquipmentUpdated;

        /// <summary>
        /// Gets the EquipableItem in the specified EquipLocation.
        /// </summary>
        /// <param name="equipLocation">The EquipLocation to get the equipped item from.</param>
        /// <returns>The EquipableItem in the specified EquipLocation, or null if there is no item equipped in that location.</returns>
        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            return !EquippedItems.ContainsKey(equipLocation) ? null : EquippedItems[equipLocation];
        }

        /// <summary>
        /// Adds an EquipableItem to the specified EquipLocation.
        /// </summary>
        /// <param name="slot">The EquipLocation to equip the item in.</param>
        /// <param name="item">The EquipableItem to equip.</param>
        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            // Assert that the item being equipped is allowed to be equipped in the specified slot
            Debug.Assert(item.AllowedEquipLocation == slot);

            EquippedItems[slot] = item;

            // Raise the EquipmentUpdated event
            EquipmentUpdated?.Invoke();
        }

        /// <summary>
        /// Removes an EquipableItem from the specified EquipLocation.
        /// </summary>
        /// <param name="slot">The EquipLocation to remove the item from.</param>
        public void RemoveItem(EquipLocation slot)
        {
            EquippedItems.Remove(slot);

            // Raise the EquipmentUpdated event
            EquipmentUpdated?.Invoke();
        }

        /// <summary>
        /// Gets a list of all EquipLocation values that have an EquipableItem equipped in them.
        /// </summary>
        /// <returns>A list of all EquipLocation values that have an EquipableItem equipped in them.</returns>
        public IEnumerable<EquipLocation> GetAllPopulatedSlots()
        {
            return EquippedItems.Keys;
        }

        /// <summary>
        /// Gets an object that represents the Equipment component's state in a form that can be serialized.
        /// </summary>
        /// <returns>An object that represents the Equipment component's state in a form that can be serialized.</returns>
        public object SaveState()
        {
            Dictionary<EquipLocation, string> equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
            foreach (KeyValuePair<EquipLocation, EquipableItem> pair in EquippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.ItemID;
            }
            return equippedItemsForSerialization;
        }

        /// <summary>
        /// Retrieve the saved state and store it into the EquipedItems dictionary
        /// </summary>
        public void LoadState(object state)
        {
            EquippedItems = new Dictionary<EquipLocation, EquipableItem>();

            Dictionary<EquipLocation, string> equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

            foreach (KeyValuePair<EquipLocation, string> pair in equippedItemsForSerialization)
            {
                EquipableItem item = (EquipableItem)InventoryItem.GetFromID(pair.Value);
                if (item != null) { EquippedItems[pair.Key] = item; }
            }
        }
    }
}
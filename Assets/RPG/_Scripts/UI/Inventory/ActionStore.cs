using RPG.Save;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script is a script for managing an "action store" in a Unity game. The action store is a collection of
    /// items that can be used by the player.
    /// 
    /// The script has a number of public functions that allow the user to interact with the action store, such as:
    /// 
    /// GetAction(int index): This function retrieves an action item from the store based on its index. If the index is
    /// not found in the store, the function returns null.
    ///
    /// GetNumber(int index): This function returns the number of items at the given index in the store. If the index
    /// is not found in the store, the function returns 0.
    ///
    /// AddAction(InventoryItem item, int index, int number): This function adds a given number of a given action item
    /// to the store at the given index.
    ///
    /// Use(int index, GameObject user): This function uses the action item at the given index in the store. If the item
    /// is consumable (i.e., it is used up when used), it is removed from the store. The function returns true if the item
    /// was successfully used, or false if the item could not be used (e.g., if the index was not found in the store).
    ///
    /// RemoveItems(int index, int number): This function removes a given number of items from the store at the given
    /// index. If the number of items drops to zero or below, the item is removed from the store.
    ///
    /// MaxAcceptable(InventoryItem item, int index): This function returns the maximum number of a given item that can
    /// be added to the store at the given index. If the item cannot be added to the store (e.g., if it is not an action
    /// item or if it is already present in the store at the given index), the function returns 0.
    ///
    /// The script also implements the ISaveableEntity interface, which means it has functions for saving and loading
    /// its state. Specifically, it has SaveState() and LoadState(object state) functions for saving and loading the
    /// state of the action store.
    /// 
    /// The script also has an event called StoreUpdated, which is triggered when the action store is updated
    /// (e.g., when an item is added or removed). Other scripts can subscribe to this event and execute code
    /// when the event is triggered.
    /// </summary>
    public class ActionStore : MonoBehaviour, ISaveableEntity
    {
        /// <summary>
        /// Dictionary storing the docked items in the action store, with the item index as the key.
        /// </summary>
        private Dictionary<int, DockedItemSlot> DockedItems { get; set; } = new Dictionary<int, DockedItemSlot>();

        /// <summary>
        /// Event that is triggered when the action store is updated (e.g., when an item is added or removed).
        /// </summary>
        public event Action StoreUpdated;

        /// <summary>
        /// Retrieves an action item from the store based on its index. If the index is not found in the store, returns null.
        /// </summary>
        /// <param name="index">The index of the action item to retrieve.</param>
        /// <returns>The action item at the given index, or null if the index is not found in the store.</returns>
        public ActionItem GetAction(int index)
        {
            return DockedItems.ContainsKey(index) ? DockedItems[index].Item : null;
        }

        /// <summary>
        /// Returns the number of items at the given index in the store. If the index is not found in the store, returns 0.
        /// </summary>
        /// <param name="index">The index of the items to retrieve the number of.</param>
        /// <returns>The number of items at the given index, or 0 if the index is not found in the store.</returns>
        public int GetNumber(int index)
        {
            return DockedItems.ContainsKey(index) ? DockedItems[index].Number : 0;
        }

        /// <summary>
        /// Adds a given number of a given action item to the store at the given index.
        /// </summary>
        /// <param name="item">The action item to add to the store.</param>
        /// <param name="index">The index to add the item at.</param>
        /// <param name="number">The number of items to add.</param>
        public void AddAction(InventoryItem item, int index, int number)
        {
            // If the index is already present in the store, add the items to the existing stack
            if (DockedItems.ContainsKey(index))
            {
                // Only add the items if they are the same as the item already at the index
                if (ReferenceEquals(item, DockedItems[index].Item))
                {
                    DockedItems[index].Number += number;
                }
            }
            // If the index is not present in the store, add a new item stack
            else
            {
                DockedItemSlot slot = new DockedItemSlot
                {
                    Item = item as ActionItem,
                    Number = number
                };
                DockedItems[index] = slot;
            }

            // Trigger the StoreUpdated event
            StoreUpdated?.Invoke();
        }

        /// <summary>
        /// Uses the action item at the given index in the store. If the item is consumable (i.e., it is used up when used), it is removed from the store.
        /// </summary>
        /// <param name="index">The index of the action item to use.</param>
        /// <param name="user">The GameObject that is using the item.</param>
        /// <returns>True if the item was successfully used, or false if the item could not be used (e.g., if the index was not found in the store).</returns>
        public bool Use(int index, GameObject user)
        {
            // Return false if the index is not present in the store
            if (!DockedItems.ContainsKey(index)) { return false; }

            // Use the item and remove it from the store if it is consumable
            DockedItems[index].Item.Use(user);
            if (DockedItems[index].Item.Consumable)
            {
                RemoveItems(index, 1);
            }
            return true;
        }

        /// <summary>
        /// Removes a given number of items from the store at the given index. If the number of items drops to zero or below, the item is removed from the store.
        /// </summary>
        /// <param name="index">The index of the items to remove.</param>
        /// <param name="number">The number of items to remove.</param>
        public void RemoveItems(int index, int number)
        {
            // Return if the index is not present in the store
            if (!DockedItems.ContainsKey(index)) return;

            // Remove the items and remove the item from the store if the number of items drops to zero or below
            DockedItems[index].Number -= number;
            if (DockedItems[index].Number <= 0)
            {
                DockedItems.Remove(index);
            }

            // Trigger the StoreUpdated event
            StoreUpdated?.Invoke();
        }

        /// <summary>
        /// Returns the maximum number of a given item that can be added to the store at the given index. If the item cannot be added to the store
        /// (e.g., if it is not an action item or if it is already present in the store at the given index), returns 0.
        /// </summary>
        /// <param name="item">The item to add to the store.</param>
        /// <param name="index">The index to add the item at.</param>
        /// <returns>The maximum number of items that can be added to the store at the given index, or 0 if the item cannot be added to the store.</returns>
        public int MaxAcceptable(InventoryItem item, int index)
        {
            // Return 0 if the item is not an action item
            ActionItem actionItem = item as ActionItem;
            if (!actionItem) { return 0; }

            // Return 0 if the item is already present in the store at the given index
            if (DockedItems.ContainsKey(index) && !object.ReferenceEquals(item, DockedItems[index].Item)) { return 0; }

            // Return int.MaxValue if the item is consumable (i.e., it can be stacked)
            if (actionItem.Consumable) { return int.MaxValue; }

            // Return 1 if the item is not consumable (i.e., it cannot be stacked) and is not already present in the store at the given index
            return DockedItems.ContainsKey(index) ? 0 : 1;
        }

        /// <summary>
        /// Saves the state of the action store.
        /// </summary>
        /// <returns>The saved state of the action store.</returns>
        public object SaveState()
        {
            // Create a dictionary to store the saved state of the action store
            Dictionary<int, DockedItemRecord> state = new Dictionary<int, DockedItemRecord>();

            // Iterate through the docked items in the action store and add their saved state to the dictionary
            foreach (KeyValuePair<int, DockedItemSlot> pair in DockedItems)
            {
                DockedItemRecord record = new DockedItemRecord
                {
                    ItemID = pair.Value.Item.ItemID,
                    Number = pair.Value.Number
                };
                state[pair.Key] = record;
            }
            return state;
        }

        /// <summary>
        /// Loads the state of the action store.
        /// </summary>
        /// <param name="state">The state to load into the action store.</param>
        public void LoadState(object state)
        {
            // Convert the state object to a dictionary
            Dictionary<int, DockedItemRecord> stateDict = (Dictionary<int, DockedItemRecord>)state;

            // Iterate through the state dictionary and add the items to the action store
            foreach (var pair in stateDict)
            {
                AddAction(InventoryItem.GetFromID(pair.Value.ItemID), pair.Key, pair.Value.Number);
            }
        }
    }
}
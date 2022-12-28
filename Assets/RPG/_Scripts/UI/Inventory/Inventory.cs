using System;
using RPG.Save;
using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// The Inventory script is a script that is attached to a GameObject and provides functionality for an inventory system in a game.
    /// It has several fields, properties, and methods that allow the player to interact with their inventory, such as adding and removing
    /// items, checking if the inventory has a certain item or if it has space for a certain item, and finding the number of items in a specific slot.
    /// 
    /// The script has a SerializeField called InventorySize, which is an int representing the number of slots in the inventory. It also has an InventorySlot[]
    /// called InventorySlots, which is an array of InventorySlot objects representing the individual slots in the inventory. InventorySlots is
    /// null until the Awake method is called, at which point it is initialized to an array of InventorySlot objects with a length equal to InventorySize.
    /// 
    /// The script has an event called InventoryUpdated, which can be subscribed to by other scripts to be notified when the inventory has been updated.
    /// It also has a static method called GetPlayerInventory, which returns the Inventory component attached to the GameObject with the "Player" tag.
    /// 
    /// The script has several methods that allow the player to interact with their inventory, such as HasSpaceFor, which checks if there is space in the
    /// inventory for a given InventoryItem, and GetSize, which returns the size of the inventory. It also has methods for adding and removing items, such
    /// as AddToFirstEmptySlot, which adds a given InventoryItem to the first empty slot in the inventory, and RemoveFromSlot, which removes a certain
    /// number of items from a specific slot.
    /// 
    /// The script also implements the ISaveableEntity interface, which means it has a SaveState method and a LoadState method for saving and loading
    /// the state of the inventory. The SaveState method returns an object containing information about the items and their quantities in the inventory,
    /// and the LoadState method takes in an object and uses it to restore the state of the inventory.
    /// </summary>
    public class Inventory : MonoBehaviour, ISaveableEntity
    {
        /// <summary>
        /// The number of slots in the inventory.
        /// </summary>
        [field: SerializeField] private int InventorySize { get; set; } = 16;

        /// <summary>
        /// The individual slots in the inventory.
        /// </summary>
        private InventorySlot[] InventorySlots { get; set; } = null;

        /// <summary>
        /// Event that is invoked when the inventory is updated.
        /// </summary>
        public event Action InventoryUpdated;

        /// <summary>
        /// Returns the Inventory component attached to the GameObject with the "Player" tag.
        /// </summary>
        /// <returns>The Inventory component attached to the player GameObject.</returns>
        public static Inventory GetPlayerInventory()
        {
            GameObject player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        /// <summary>
        /// Determines if there is space in the inventory for the specified item.
        /// </summary>
        /// <param name="item">The item to check for space.</param>
        /// <returns>True if there is space for the item, false otherwise.</returns>
        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        /// <summary>
        /// Returns the size of the inventory.
        /// </summary>
        /// <returns>The size of the inventory.</returns>
        public int GetSize()
        {
            return InventorySlots.Length;
        }

        /// <summary>
        /// Adds the specified item to the first empty slot in the inventory.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="number">The number of items to add.</param>
        /// <returns>True if the item was added, false otherwise.</returns>
        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int itemSlot = FindSlot(item);

            if (itemSlot < 0)
            {
                return false;
            }

            InventorySlots[itemSlot].Item = item;
            InventorySlots[itemSlot].Number += number;
            InventoryUpdated?.Invoke();

            return true;
        }

        /// <summary>
        /// Determines if the inventory contains the specified item.
        /// </summary>
        /// <param name="item">The item to check for.</param>
        /// <returns>True if the inventory has the item, false otherwise.</returns>
        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                if (object.ReferenceEquals(InventorySlots[i].Item, item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the item in the specified slot.
        /// </summary>
        /// <param name="slot">The slot to get the item from.</param>
        /// <returns>The item in the specified slot.</returns>
        public InventoryItem GetItemInSlot(int slot)
        {
            return InventorySlots[slot].Item;
        }

        /// <summary>
        /// Returns the number of items in the specified slot.
        /// </summary>
        /// <param name="slot">The slot to get the number of items from.</param>
        /// <returns>The number of items in the specified slot.</returns>
        public int GetNumberInSlot(int slot)
        {
            return InventorySlots[slot].Number;
        }

        /// <summary>
        /// Removes a certain number of items from the specified slot.
        /// </summary>
        /// <param name="slot">The slot to remove the items from.</param>
        /// <param name="number">The number of items to remove.</param>
        public void RemoveFromSlot(int slot, int number)
        {
            InventorySlots[slot].Number -= number;
            if (InventorySlots[slot].Number <= 0)
            {
                InventorySlots[slot].Number = 0;
                InventorySlots[slot].Item = null;
            }

            InventoryUpdated?.Invoke();
        }

        /// <summary>
        /// Adds the specified item and number of items to the specified slot.
        /// </summary>
        /// <param name="slot">The slot to add the items to.</param>
        /// <param name="item">The item to add.</param>
        /// <param name="number">The number of items to add.</param>
        /// <returns>True if the items were added, false otherwise.</returns>
        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (InventorySlots[slot].Item != null)
            {
                return AddToFirstEmptySlot(item, number);
            }

            int itemStack = FindStack(item);
            if (itemStack >= 0)
            {
                slot = itemStack;
            }

            InventorySlots[slot].Item = item;
            InventorySlots[slot].Number += number;
            InventoryUpdated?.Invoke();

            return true;
        }

        /// <summary>
        /// Initializes the InventorySlots array.
        /// </summary>
        private void Awake()
        {
             InventorySlots = new InventorySlot[InventorySize];
        }

        /// <summary>
        /// Finds the slot containing the specified item or an empty slot if the item is not present.
        /// </summary>
        /// <param name="item">The item to find a slot for.</param>
        /// <returns>The index of the slot containing the item or an empty slot, or -1 if no suitable slot was found.</returns>
        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }

            return i;
        }

        /// <summary>
        /// Finds the first empty slot in the inventory.
        /// </summary>
        /// <returns>The index of the first empty slot, or -1 if no empty slot was found.</returns>
        private int FindEmptySlot()
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                if (InventorySlots[i].Item == null)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Finds the slot containing the specified item.
        /// </summary>
        /// <param name="item">The item to find a slot for.</param>
        /// <returns>The index of the slot containing the item, or -1 if no suitable slot was found.</returns>
        private int FindStack(InventoryItem item)
        {
            if (!item.Stackable)
            {
                return -1;
            }

            for (int i = 0; i < InventorySlots.Length; i++)
            {
                if (ReferenceEquals(InventorySlots[i].Item, item))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Saves the state of the inventory.
        /// </summary>
        /// <returns>An object containing information about the items and their quantities in the inventory.</returns>
        public object SaveState()
        {
            InventorySlotRecord[] slotStrings = new InventorySlotRecord[InventorySize];
            for (int i = 0; i < InventorySize; i++)
            {
                if (InventorySlots[i].Item != null)
                {
                    slotStrings[i].ItemID = InventorySlots[i].Item.ItemID;
                    slotStrings[i].Number = InventorySlots[i].Number;
                }
            }

            return slotStrings;
        }

        /// <summary>
        /// Loads the state of the inventory.
        /// </summary>
        /// <param name="state">An object containing information about the items and their quantities in the inventory.</param>
        public void LoadState(object state)
        {
            InventorySlotRecord[] slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < InventorySize; i++)
            {
                //Debug.Log($"Source: {gameObject.name}, Item: {slotStrings[i].ItemID}, Slot: {i}");
                InventorySlots[i].Item = InventoryItem.GetFromID(slotStrings[i].ItemID);
                InventorySlots[i].Number = slotStrings[i].Number;
            }

            InventoryUpdated?.Invoke();
        }
    }
}
using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script appears to define a class called InventorySlotUI that is a MonoBehaviour in the RPG.UI.Inventory namespace.
    /// The class also implements two interfaces: IItemHolder and IDragContainer&lt;InventoryItem&gt;.
    /// 
    /// The InventorySlotUI class has a serialized field called Icon of type InventoryItemIcon, and a number of private fields:
    /// Index, Item, and Inventory. The Index field is an integer initialized to -1, and the Item and Inventory fields are both
    /// of type InventoryItem and are initialized to null.
    /// 
    /// The InventorySlotUI class has a method called Setup that takes an Inventory object and an integer index as arguments.
    /// This method sets the Inventory and Index fields to the values of the arguments passed in, and then calls a method on the
    /// Icon object to set the item and number in the slot using the inventory object's GetItemInSlot and GetNumberInSlot methods.
    /// 
    /// The InventorySlotUI class also has several other methods that are part of the interfaces it implements:
    /// 
    /// GetItem returns the item in the slot using the Inventory object's GetItemInSlot method and the Index field.
    ///
    /// GetNumber returns the number of items in the slot using the Inventory object's GetNumberInSlot method and the Index field.
    ///
    /// RemoveItems removes a specified number of items from the slot using the Inventory object's RemoveFromSlot method and the
    /// Index field.
    ///
    /// GetMaxAcceptableItemCount returns the maximum number of items that can be added to the slot using the Inventory object's
    /// HasSpaceFor method. If there is space for the item, it returns int.MaxValue, otherwise it returns 0.
    ///
    /// AddItemsToInventory adds a specified number of items to the slot using the Inventory object's AddItemToSlot method and the
    /// Index field.
    /// </summary>
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        /// <summary>
        /// The icon that represents the item in the slot.
        /// </summary>
        [field : SerializeField] private InventoryItemIcon Icon { get; set; } = null;

        /// <summary>
        /// The index of the slot in the inventory.
        /// </summary>
        private int Index { get; set; } = -1;

        /// <summary>
        /// The item currently held in the slot.
        /// </summary>
        private InventoryItem Item { get; set; } = null;

        /// <summary>
        /// The inventory that the slot is a part of.
        /// </summary>
        private Inventory Inventory { get; set; } = null;

        /// <summary>
        /// Set up the slot with an inventory and index.
        /// </summary>
        /// <param name="inventory">The inventory that the slot is a part of.</param>
        /// <param name="index">The index of the slot in the inventory.</param>
        public void Setup(Inventory inventory, int index)
        {
            // Set the Inventory and Index fields to the values of the arguments.
            Inventory = inventory;
            Index = index;

            // Set the item and number in the slot using the inventory's GetItemInSlot and GetNumberInSlot methods.
            Icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
        }

        /// <summary>
        /// Get the item currently held in the slot.
        /// </summary>
        /// <returns>The item currently held in the slot.</returns>
        public InventoryItem GetItem()
        {
            // Return the item in the slot using the Inventory object's GetItemInSlot method and the Index field.
            return Inventory.GetItemInSlot(Index);
        }

        /// <summary>
        /// Get the number of items currently held in the slot.
        /// </summary>
        /// <returns>The number of items currently held in the slot.</returns>
        public int GetNumber()
        {
            // Return the number of items in the slot using the Inventory object's GetNumberInSlot method and the Index field.
            return Inventory.GetNumberInSlot(Index);
        }

        /// <summary>
        /// Remove a specified number of items from the slot.
        /// </summary>
        /// <param name="number">The number of items to remove from the slot.</param>
        public void RemoveItems(int number)
        {
            // Remove the specified number of items from the slot using the Inventory object's RemoveFromSlot method and the Index field.
            Inventory.RemoveFromSlot(Index, number);
        }

        /// <summary>
        /// Get the maximum number of items that can be added to the slot.
        /// </summary>
        /// <param name="item">The item that is being added to the slot.</param>
        /// <returns>The maximum number of items that can be added to the slot.</returns>
        public int GetMaxAcceptableItemCount(InventoryItem item)
        {
            // If the inventory has space for the item, return int.MaxValue, otherwise return 0.
            return Inventory.HasSpaceFor(item) ? int.MaxValue : 0;
        }

        /// <summary>
        /// Add a specified number of items to the slot.
        /// </summary>
        /// <param name="item">The item to add to the slot.</param>
        /// <param name="count">The number of items to add to the slot.</param>
        public void AddItemsToInventory(InventoryItem item, int count)
        {
            // Add the specified number of items to the slot using the Inventory object's AddItemToSlot method and the Index field.
            Inventory.AddItemToSlot(Index, item, count);
        }
    }
}
using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script is a Unity component for a UI element that represents an action slot in an inventory system.
    /// The UI element is responsible for displaying an item icon, and it implements several interfaces that allow
    /// it to interact with the inventory system.
    /// 
    /// The ActionSlotUI class extends MonoBehaviour, which is the base class for all Unity scripts. It also implements
    /// the IItemHolder and IDragContainer&lt;InventoryItem&gt; interfaces, which define methods for interacting with an
    /// inventory item.
    /// 
    /// The class has several fields, including a serialized InventoryItemIcon object called Icon, and an int called Index.
    /// The Awake method is a Unity lifecycle method that is called when the component is initialized. In this case, it is
    /// used to set the ActionStore property and register an event handler for the StoreUpdated event.
    /// 
    /// The ActionStore class appears to be responsible for storing and managing the actions in the inventory. The ActionSlotUI
    /// component interacts with the ActionStore to get and set the item in the slot, as well as to add and remove items from
    /// the inventory. The UpdateIcon method is used to update the display of the item icon in the UI element.
    /// 
    /// The IItemHolder interface defines a method for getting the item held by the UI element, and the IDragContainer
    /// interface defines methods for adding and removing items from the container. The GetMaxAcceptableItemCount method appears
    /// to be used to determine the maximum number of a given item that can be added to the container.
    /// </summary>
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [field : SerializeField] public InventoryItemIcon Icon { get; private set; } = null;
        [field : SerializeField] public int Index { get; private set; } = 0;

        private ActionStore ActionStore { get; set; }

        /// <summary>
        /// Unity's Awake method is called when the component is initialized.
        /// It is used to set the ActionStore property and register an event handler for the StoreUpdated event.
        /// </summary>
        private void Awake()
        {
            // Find the action store component on the player game object
            ActionStore = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionStore>();
            // Register the UpdateIcon method as an event handler for the StoreUpdated event
            ActionStore.StoreUpdated += UpdateIcon;
        }

        /// <summary>
        /// GetItem returns the item in the action slot.
        /// </summary>
        /// <returns>The item in the action slot.</returns>
        public InventoryItem GetItem()
        {
            return ActionStore.GetAction(Index);
        }

        /// <summary>
        /// RemoveItems removes a specified number of items from the action slot.
        /// </summary>
        /// <param name="number">The number of items to remove.</param>
        public void RemoveItems(int number)
        {
            ActionStore.RemoveItems(Index, number);
        }

        /// <summary>
        /// UpdateIcon updates the display of the item icon in the UI element.
        /// </summary>
        private void UpdateIcon()
        {
            Icon.SetItem(GetItem(), GetNumber());
        }

        /// <summary>
        /// GetMaxAcceptableItemCount returns the maximum number of a given item that can be added to the action slot.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>The maximum number of the given item that can be added to the action slot.</returns>
        public int GetMaxAcceptableItemCount(InventoryItem item)
        {
            return ActionStore.MaxAcceptable(item, Index);
        }

        /// <summary>
        /// AddItemsToInventory adds a specified number of a given item to the action slot.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="count">The number of items to add.</param>
        public void AddItemsToInventory(InventoryItem item, int count)
        {
            ActionStore.AddAction(item, Index, count);
        }

        /// <summary>
        /// GetNumber returns the number of items in the action slot.
        /// </summary>
        /// <returns>The number of items in the action slot.</returns>
        public int GetNumber()
        {
            return ActionStore.GetNumber(Index);
        }
    }
}
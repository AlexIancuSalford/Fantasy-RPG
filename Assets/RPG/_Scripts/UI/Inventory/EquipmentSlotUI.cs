using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script appears to define a class called "EquipmentSlotUI" in the namespace "RPG.UI.Inventory". The class has the following features:
    /// 
    /// It derives from Unity's "MonoBehaviour" class and implements two interfaces: "IItemHolder" and "IDragContainer&lt;InventoryItem".
    /// 
    /// It has a public field called "Icon", which is an instance of the "InventoryItemIcon" class and is serialized in the Unity editor.
    /// 
    /// It has a public field called "EquipLocation" of the "EquipLocation" type, which is serialized in the Unity editor. The default
    /// value of this field is "EquipLocation.Weapon".
    /// 
    /// It has a private field called "playerEquipment", which is an instance of the "Equipment" class.
    /// 
    /// It has an "Awake" method that is called when the object is initialized. In this method, the script finds a game object with the tag
    /// "Player", gets its "Equipment" component, and assigns it to the "playerEquipment" field. It also subscribes to the "EquipmentUpdated"
    /// event of the "playerEquipment" instance.
    /// 
    /// It has a "Start" method that is called when the object becomes active. In this method, the script calls a "RedrawUI" method.
    /// 
    /// It has a "GetItem" method that returns the item in the equipment slot specified by the "EquipLocation" field for the player's equipment.
    /// 
    /// It has a "GetNumber" method that returns 1 if the equipment slot has an item, or 0 if it does not.
    /// 
    /// It has a "RemoveItems" method that removes the item from the equipment slot specified by the "EquipLocation" field for the player's equipment.
    /// 
    /// It has a "RedrawUI" method that sets the item in the "Icon" field to the item in the equipment slot specified by the "EquipLocation"
    /// field for the player's equipment.
    /// 
    /// It has a "GetMaxAcceptableItemCount" method that takes an "InventoryItem" item as a parameter and returns the maximum number of that item
    /// that can be added to the equipment slot. This method checks if the item is an instance of the "EquipableItem" class and if its allowed equip
    /// location matches the "EquipLocation" field. If both conditions are met, the method returns 1 if the equipment slot is empty, or 0 if it is not.
    /// 
    /// It has an "AddItemsToInventory" method that takes an "InventoryItem" item and an integer count as parameters, and adds the item to the
    /// equipment slot specified by the "EquipLocation" field for the player's equipment.
    /// </summary>
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        /// <summary>
        /// The item icon displayed in the UI.
        /// </summary>
        [field : SerializeField] public InventoryItemIcon Icon { get; private set; } = null;

        /// <summary>
        /// The type of equipment slot represented by this UI element.
        /// </summary>
        [field : SerializeField] public EquipLocation EquipLocation { get; set; } = EquipLocation.Weapon;

        /// <summary>
        /// A reference to the player's equipment component.
        /// </summary>
        private Equipment PlayerEquipment { get; set; } = null;

        /// <summary>
        /// Initializes the equipment slot UI.
        /// </summary>
        private void Awake()
        {
            // Find the player game object and get its Equipment component.
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerEquipment = player.GetComponent<Equipment>();

            // Subscribe to the EquipmentUpdated event of the player's Equipment component.
            PlayerEquipment.EquipmentUpdated += RedrawUI;
        }

        /// <summary>
        /// Redraws the UI when the object becomes active.
        /// </summary>
        private void Start()
        {
            RedrawUI();
        }

        /// <summary>
        /// Returns the item in the equipment slot represented by this UI element.
        /// </summary>
        /// <returns>The item in the equipment slot, or null if the slot is empty.</returns>
        public InventoryItem GetItem()
        {
            return PlayerEquipment.GetItemInSlot(EquipLocation);
        }

        /// <summary>
        /// Returns the number of items in the equipment slot represented by this UI element.
        /// </summary>
        /// <returns>1 if the equipment slot has an item, or 0 if it is empty.</returns>
        public int GetNumber()
        {
            return GetItem() != null ? 1 : 0;
        }

        /// <summary>
        /// Removes the item from the equipment slot represented by this UI element.
        /// </summary>
        /// <param name="number">The number of items to remove (ignored, as there can only be one item in an equipment slot).</param>
        public void RemoveItems(int number)
        {
            PlayerEquipment.RemoveItem(EquipLocation);
        }

        /// <summary>
        /// Redraws the UI by updating the item icon.
        /// </summary>
        private void RedrawUI()
        {
            Icon.SetItem(PlayerEquipment.GetItemInSlot(EquipLocation));
        }

        /// <summary>
        /// Returns the maximum number of the specified item that can be added to the equipment slot represented by this UI element.
        /// </summary>
        /// <param name="item">The item to add to the equipment slot.</param>
        /// <returns>1 if the item can be equipped in the equipment slot and the slot is currently empty, or 0 if the item cannot be equipped or the slot is not empty.</returns>
        public int GetMaxAcceptableItemCount(InventoryItem item)
        {
            // Check if the item is an equipable item and if it can be equipped in the equipment slot represented by this UI element.
            EquipableItem equipableItem = item as EquipableItem;
            if (equipableItem == null) return 0;
            if (equipableItem.AllowedEquipLocation != EquipLocation) { return 0; }

            // Return 1 if the equipment slot is empty, or 0 if it is not.
            return GetItem() != null ? 0 : 1;
        }

        /// <summary>
        /// Adds the specified item to the equipment slot represented by this UI element.
        /// </summary>
        /// <param name="item">The item to add to the equipment slot.</param>
        /// <param name="count">The number of items to add (ignored, as there can only be one item in an equipment slot).</param>
        public void AddItemsToInventory(InventoryItem item, int count)
        {
            PlayerEquipment.AddItem(EquipLocation, (EquipableItem)item);
        }

    }
}

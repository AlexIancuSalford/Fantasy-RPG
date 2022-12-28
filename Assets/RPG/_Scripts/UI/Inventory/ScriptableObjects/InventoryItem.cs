using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script defines an InventoryItem class. The InventoryItem class is derived from ScriptableObject and implements the
    /// ISerializationCallbackReceiver interface.
    /// 
    /// The InventoryItem class has several fields, all of which are marked with the SerializeField attribute, which indicates
    /// that they should be serialized (saved to and loaded from disk). These fields include:
    /// 
    /// ItemID: a string containing a unique identifier for the item. This field has a private setter, which means that it
    /// can only be set within the class and not from outside it.
    ///
    /// DisplayName: a string containing the name of the item to be displayed in the user interface (UI). Like ItemID, this
    /// field has a private setter.
    ///
    /// description: a string containing a description of the item to be displayed in the UI. This field is also annotated with
    /// the TextArea attribute, which is likely used to format the field as a multiline text area in the Unity editor.
    ///
    /// Icon: a Sprite containing the icon to represent the item in the inventory UI. Like ItemID and DisplayName, this field has a private setter.
    ///
    /// Pickup: a Pickup object representing the prefab that should be spawned when the item is dropped in the game world.
    /// Like ItemID, DisplayName, and Icon, this field has a private setter.
    ///
    /// Stackable: a boolean indicating whether multiple items of this type can be stacked in the same inventory slot. Like the
    /// other fields, this field has a private setter.
    ///
    /// The InventoryItem class also includes a ItemLookupCache dictionary field and a GetFromID method, which allows for retrieving
    /// an InventoryItem object from its ItemID. The ItemLookupCache is populated with all InventoryItem objects in the "Resources"
    /// folder when it is first accessed.
    /// 
    /// The InventoryItem class also includes a SpawnPickup method that instantiates a Pickup object at a specified position in the
    /// game world and sets it up with the current InventoryItem and a specified number of items.
    /// 
    /// Finally, the InventoryItem class includes implementations of the OnAfterDeserialize and OnBeforeSerialize methods from the
    /// ISerializationCallbackReceiver interface. The OnAfterDeserialize method is called after the object has been deserialized,
    /// and the OnBeforeSerialize method is called before the object is serialized. In this implementation, the OnAfterDeserialize
    /// method is used to set the ItemID field to a new random identifier if it is not already set, while the OnBeforeSerialize method is empty.
    /// </summary>
    public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.
        /// </summary>
        [field : SerializeField] public string ItemID { get; private set; } = null;

        /// <summary>
        /// Item name to be displayed in UI.
        /// </summary>
        [field : SerializeField] public string DisplayName { get; private set; } = null;

        /// <summary>
        /// Item description to be displayed in UI.
        /// </summary>
        [SerializeField][TextArea] public string description = null;

        /// <summary>
        /// The UI icon to represent this item in the inventory.
        /// </summary>
        [field : SerializeField] public Sprite Icon { get; private set; } = null;

        /// <summary>
        /// The prefab that should be spawned when this item is dropped.
        /// </summary>
        [field : SerializeField] public Pickup Pickup { get; private set; } = null;

        /// <summary>
        /// If true, multiple items of this type can be stacked in the same inventory slot.
        /// </summary>
        [field : SerializeField] public bool Stackable { get; private set; } = false;

        /// <summary>
        /// A dictionary of all InventoryItem objects, indexed by their ItemID.
        /// </summary>
        private static Dictionary<string, InventoryItem> ItemLookupCache { get; set; } =
            new Dictionary<string, InventoryItem>();

        /// <summary>
        /// Returns the InventoryItem object with the specified ItemID.
        /// </summary>
        /// <param name="itemID">The ItemID of the InventoryItem to retrieve.</param>
        /// <returns>The InventoryItem object with the specified ItemID, or null if no such object exists.</returns>
        public static InventoryItem GetFromID(string itemID)
        {
            if (ItemLookupCache == null)
            {
                // If the ItemLookupCache has not yet been initialized, populate it with all InventoryItem objects
                // in the "Resources" folder.
                ItemLookupCache = new Dictionary<string, InventoryItem>();
                InventoryItem[] itemArray = Resources.LoadAll<InventoryItem>("");
                foreach (InventoryItem item in itemArray)
                {
                    if (ItemLookupCache.ContainsKey(item.ItemID))
                    {
                        Debug.LogError(
                            $"Looks like there's a duplicate ID for objects: {ItemLookupCache[item.ItemID]} and {item}");
                        continue;
                    }

                    ItemLookupCache[item.ItemID] = item;
                }
            }

            if (itemID == null || !ItemLookupCache.ContainsKey(itemID)) return null;
            return ItemLookupCache[itemID];
        }

        /// <summary>
        /// Spawns a Pickup object at the specified position in the game world, representing the current InventoryItem object.
        /// </summary>
        /// <param name="position">The position at which to spawn the Pickup object.</param>
        /// <param name="number">The number of items represented by the Pickup object.</param>
        /// <returns>The spawned Pickup object.</returns>
        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickup = Instantiate(Pickup);
            pickup.transform.position = position;
            pickup.Setup(this, number);
            return pickup;
        }

        /// <summary>
        /// This method is called after the object has been deserialized.
        /// </summary>
        public void OnAfterDeserialize()
        {
            // If the ItemID field is not set, generate a new random identifier for the item.
            if (string.IsNullOrWhiteSpace(ItemID))
            {
                ItemID = System.Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// This method is called before the object is serialized.
        /// </summary>
        public void OnBeforeSerialize()
        {
            // This method is currently empty. (on purpose as the interface needs to implement this, but I don't need it)
        }
    }
}

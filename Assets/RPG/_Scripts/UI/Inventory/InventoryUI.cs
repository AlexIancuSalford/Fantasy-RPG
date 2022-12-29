using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script defines a class called InventoryUI that manages the display of an inventory in a Unity game.
    /// It appears to be part of a larger system for displaying and interacting with inventory items in the game.
    /// 
    /// The InventoryUI class has several methods:
    /// 
    /// Awake(): This method is called when the object the script is attached to is created. It gets a reference to
    /// the player's inventory using the GetPlayerInventory() method and subscribes to the InventoryUpdated event of
    /// the player's inventory.
    ///
    /// Start(): This method is called after Awake(), and it calls the Redraw() method.
    ///
    /// Redraw(): This method destroys all the children of the object the script is attached to and then instantiates
    /// a new InventorySlotUI object for each item in the player's inventory, passing it the player's inventory and
    /// the index of the current item as arguments.
    ///
    /// The InventoryUI class also has a SerializeField field called InventoryItemPrefab, which is a prefab
    /// (a reusable game object with a specific configuration) of an InventorySlotUI object. This prefab will be
    /// instantiated for each item in the player's inventory.
    /// 
    /// The InventorySlotUI object is responsible for displaying and interacting with a single inventory item.
    /// It has a Setup() method that takes an inventory and an index as arguments and is called when the object is instantiated.
    /// This method probably sets up the display and behavior of the object for the specific inventory item it represents.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        /// <summary>
        /// A prefab of an InventorySlotUI object, which is responsible for
        /// displaying and interacting with a single inventory item.
        /// </summary>
        [field : SerializeField] private InventorySlotUI InventoryItemPrefab { get; set; } = null;

        /// <summary>
        /// A reference to the player's inventory.
        /// </summary>
        private Inventory PlayerInventory { get; set; } = null;

        /// <summary>
        /// This method is called when the object the script is attached to is created.
        /// </summary>
        private void Awake() 
        {
            // Get a reference to the player's inventory.
            PlayerInventory = Inventory.GetPlayerInventory();
            // Subscribe to the InventoryUpdated event of the player's inventory.
            PlayerInventory.InventoryUpdated += Redraw;
        }

        /// <summary>
        /// This method is called after Awake(), and it calls the Redraw() method.
        /// </summary>
        private void Start()
        {
            Redraw();
        }

        /// <summary>
        /// This method destroys all the children of the object the script is attached to
        /// and then instantiates a new InventorySlotUI object for each item in the player's
        /// inventory, passing it the player's inventory and the index of the current item as arguments.
        /// </summary>
        private void Redraw()
        {
            // Destroy all the children of the object the script is attached to.
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Iterate through all the items in the player's inventory.
            for (int i = 0; i < PlayerInventory.GetSize(); i++)
            {
                // Instantiate an InventorySlotUI object and set it up for the current item.
                InventorySlotUI itemUI = Instantiate(InventoryItemPrefab, transform);
                itemUI.Setup(PlayerInventory, i);
            }
        }
    }
}
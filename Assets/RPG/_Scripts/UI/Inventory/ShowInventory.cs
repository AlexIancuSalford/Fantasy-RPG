using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script is a simple script for an inventory system in Unity. It's attached to a GameObject in the scene,
    /// and it allows the player to toggle the visibility of a UI element (specified by the UIElement field) by pressing a
    /// specific key (specified by the ToggleKey field).
    /// 
    /// The script has a Start method and an Update method. The Start method is called when the script is first
    /// initialized, and it sets the UIElement to be inactive by default. The Update method is called once per frame
    /// and it checks if the ToggleKey has been pressed. If it has, it sets the UIElement to be the opposite o what it is
    /// (e.g. if it's active, set to inactive, if it's inactive, set to active). Furthermore, it sets the HUD element off if
    /// the Inventory is on and vice-versa.
    /// 
    /// The [field : SerializeField] attribute above the ToggleKey, UIElement, and HUDElement fields allows the values of these
    /// fields to be set in the Unity Inspector, rather than hardcoded in the script. This allows you to easily customize
    /// the key that toggles the inventory and the UI element that represents the inventory in the Unity Editor.
    /// </summary>
    public class ShowInventory : MonoBehaviour
    {
        /// <summary>
        /// The key that will toggle the visibility of the inventory UI element.
        /// </summary>
        [field : SerializeField] public KeyCode ToggleKey { get; set; } = KeyCode.None;

        /// <summary>
        /// The UI element that represents the inventory.
        /// </summary>
        [field : SerializeField] public GameObject UIElement { get; set; } = null;

        /// <summary>
        /// The UI element that represents the HUD (heads-up display).
        /// </summary>
        [field: SerializeField] public GameObject HUDElement { get; set; } = null;

        /// <summary>
        /// Set the inventory UI element to be inactive by default.
        /// </summary>
        void Start()
        {
            UIElement.SetActive(false);
        }

        /// <summary>
        /// Check if the toggle key has been pressed. If it has, set the inventory UI element to be active.
        /// Since the HUD will be in the way of the inventory, turn it off while the inventory is on.
        /// </summary>
        void Update()
        {
            if (!Input.GetKeyDown(ToggleKey)) { return; }
            UIElement.SetActive(!UIElement.activeSelf);
            if (HUDElement != null) { HUDElement.SetActive(!HUDElement.activeSelf); }
        }
    }
}

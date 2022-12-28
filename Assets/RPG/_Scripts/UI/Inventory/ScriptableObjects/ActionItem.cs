using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This is a script that defines a scriptable object called "ActionItem" that represents an item in an inventory system.
    /// 
    /// The ActionItem class derives from the InventoryItem class, which is likely a base class that provides some common
    /// functionality for inventory items. The ActionItem class adds a boolean field called consumable and a method called Use.
    /// 
    /// The Use method takes a GameObject called user as a parameter and has a single line of code that prints a message to the console.
    /// This method can be overridden in derived classes to provide custom behavior for using the action item.
    /// 
    /// The [CreateAssetMenu(menuName = ("Scriptable Object/Action Item"))] attribute above the class definition allows the ActionItem
    /// scriptable object to be created in the Unity editor. It will appear in the "Create" menu under the "Scriptable Object/Action Item" submenu.
    /// /// </summary>
        [CreateAssetMenu(menuName = ("Scriptable Object/Action Item"))]
    public class ActionItem : InventoryItem
    {
        /// <summary>
        /// Whether or not the action item is consumable (e.g. a health potion).
        /// </summary>
        [field : SerializeField] public bool Consumable { get; private set; } = false;

        /// <summary>
        /// Use the action item. (e.g. use the potion)
        /// </summary>
        /// <param name="user">The game object using the action item.</param>
        public virtual void Use(GameObject user)
        {
            Debug.Log("Using action: " + this);
        }
    }
}
using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script is a component for a Unity game object that allows it to display a tooltip containing
    /// information about an item when the player hovers their mouse over the game object.
    /// 
    /// The script is derived from a base class called TooltipSpawner, which likely contains basic
    /// functionality for spawning and positioning a tooltip.
    /// 
    /// The ItemTooltipSpawner class has two methods that override corresponding methods in the base class:
    /// CanCreateTooltip and UpdateTooltip. The CanCreateTooltip method returns a boolean indicating whether
    /// a tooltip should be created for this game object. It does this by checking if the game object has an
    /// IItemHolder component, and whether that component has an item currently held. If both of these
    /// conditions are true, the method returns true; otherwise, it returns false.
    /// 
    /// The UpdateTooltip method updates the contents of the tooltip with information about the item.
    /// It does this by getting the ItemTooltip component of the tooltip game object, and then using the Setup
    /// method of that component to pass in the item. If the game object does not have an ItemTooltip component,
    /// the method simply returns without doing anything.
    /// 
    /// Finally, the ItemTooltipSpawner class has a RequireComponent attribute applied to it, which specifies
    /// that it requires an IItemHolder component to be attached to the same game object in order to function properly.
    /// </summary>
    [RequireComponent(typeof(IItemHolder))]
    public class ItemTooltipSpawner : TooltipSpawner
    {
        /// <summary>
        /// Overrides the CanCreateTooltip method of the base class, TooltipSpawner
        /// </summary>
        /// <returns>Returns true if the game object has an IItemHolder component and that component has an item currently held, false otherwise</returns>
        public override bool CanCreateTooltip()
        {
            // Get the IItemHolder component
            IItemHolder itemHolder = GetComponent<IItemHolder>();

            // Return whether the item holder has an item currently held
            return itemHolder.GetItem() != null;
        }

        /// <summary>
        /// Overrides the UpdateTooltip method of the base class, TooltipSpawner
        /// Updates the contents of the tooltip with information about the item held by the game object's IItemHolder component
        /// </summary>
        /// <param name="tooltip"></param>
        public override void UpdateTooltip(GameObject tooltip)
        {
            // Get the ItemTooltip component of the tooltip game object
            ItemTooltip itemTooltip = tooltip.GetComponent<ItemTooltip>();

            // If the tooltip game object does not have an ItemTooltip component, return without doing anything
            if (!itemTooltip) { return; }

            // Get the item held by the game object's IItemHolder component
            InventoryItem item = GetComponent<IItemHolder>().GetItem();

            // Use the ItemTooltip's Setup method to set the contents of the tooltip to display information about the item
            itemTooltip.Setup(item);
        }
    }
}
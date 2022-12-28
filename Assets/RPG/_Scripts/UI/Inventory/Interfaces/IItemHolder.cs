namespace RPG.UI.Inventory
{
    /// <summary>
    /// Allows the `ItemTooltipSpawner` to display the right information.
    /// </summary>
    public interface IItemHolder
    {
        public InventoryItem GetItem();
    }
}

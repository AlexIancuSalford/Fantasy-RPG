namespace RPG.UI.Inventory
{
    /// <summary>
    /// An interface for a drag source in an inventory.
    /// </summary>
    /// <typeparam name="T">The type of item being dragged.</typeparam>
    public interface IDragSource<out T> where T : class
    {
        /// <summary>
        /// Gets the item being dragged.
        /// </summary>
        /// <returns>The item being dragged.</returns>
        T GetItem();

        /// <summary>
        /// Gets the number of items being dragged.
        /// </summary>
        /// <returns>The number of items being dragged.</returns>
        int GetNumber();

        /// <summary>
        /// Removes a specified number of items from the drag source.
        /// </summary>
        /// <param name="number">The number of items to remove.</param>
        void RemoveItems(int number);
    }
}
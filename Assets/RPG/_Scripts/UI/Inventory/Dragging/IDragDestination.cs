namespace RPG.UI.Inventory
{
    /// <summary>
    /// The interface is defined as IInventoryDestination&lt;in T&gt; where T is a class type parameter and is marked with the in keyword.
    /// This means that the T parameter is an input-only type parameter and is considered covariant. This means that the IInventoryDestination
    /// interface can be implemented by any class that takes a class type parameter that is a subtype of the type specified by T.
    /// 
    /// The GetMaxAcceptableItemCount method takes a single parameter of type T and returns an int value. This method is likely used to
    /// determine the maximum number of items of type T that can be accepted by an object implementing this interface.
    /// 
    /// The AddItemsToInventory method also takes a single parameter of type T and an int value. This method is likely used to add a
    /// specified number of items of type T to an object implementing this interface.
    /// </summary>
    /// <typeparam name="T">The type of items that can be accepted by the object.</typeparam>
    public interface IDragDestination<in T> where T : class
    {
        /// <summary>
        /// Gets the maximum number of items of type <typeparamref name="T"/> that can be accepted by the object.
        /// </summary>
        /// <param name="item">The item to get the maximum acceptable count for.</param>
        /// <returns>The maximum number of items of type <typeparamref name="T"/> that can be accepted by the object.</returns>
        int GetMaxAcceptableItemCount(T item);

        /// <summary>
        /// Adds a specified number of items of type <typeparamref name="T"/> to the object's inventory.
        /// </summary>
        /// <param name="item">The item to add to the inventory.</param>
        /// <param name="count">The number of items to add to the inventory.</param>
        void AddItemsToInventory(T item, int count);
    }
}
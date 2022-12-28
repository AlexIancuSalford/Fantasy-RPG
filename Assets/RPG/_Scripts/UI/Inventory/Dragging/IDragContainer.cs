namespace RPG.UI.Inventory
{
    /// <summary>
    /// This is an interface for an object that acts as both the source and the destination for the drag action.
    /// It makes it possible to swap items between containers.
    /// </summary>
    /// <typeparam name="T">The type that represents the item being dragged.</typeparam>
    public interface IDragContainer<T> : IDragDestination<T>, IDragSource<T> where T : class
    {
    }
}
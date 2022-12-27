using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This Unity script implements drag and drop functionality for an inventory system. The script is designed to be used with a
    /// game object that has a CanvasGroup component, and is designed to be used with other game objects that implement the
    /// IDragSource and IDragDestination interfaces.
    /// 
    /// The script itself is a MonoBehaviour that implements the IBeginDragHandler, IDragHandler, and IEndDragHandler interfaces,
    /// which allow it to handle events related to dragging and dropping items in a UI. The script is also constrained to work with a
    /// specific type T, which represents the type of items being dragged and dropped.
    /// 
    /// The script has several fields:
    /// 
    /// StartPosition is a vector that represents the position of the game object at the start of a drag.
    /// OriginalParent is a Transform that represents the parent of the game object at the start of a drag.
    /// DragSource is an IDragSource that represents the container that the item being dragged is coming from.
    /// ParentCanvas is a Canvas that represents the canvas that the game object is a child of.
    /// The script has several methods:
    /// 
    /// Awake is a Unity lifecycle method that is called when the script is initialized. It sets the ParentCanvas
    /// field to the canvas that the game object is a child of, and sets the DragSource field to the IDragSource component
    /// that is a parent of the game object.
    ///
    /// OnBeginDrag is a method that is called when the drag operation starts. It sets the StartPosition field to the
    /// current position of the game object, sets the OriginalParent field to the current parent of the game object, and disables
    /// the CanvasGroup component's blocksRaycasts property so that the game object does not block raycasts during the drag. It also
    /// sets the game object's parent to the ParentCanvas so that it is displayed on top of other UI elements during the drag.
    ///
    /// OnDrag is a method that is called during the drag operation. It sets the position of the game object to the current position of the pointer.
    ///
    /// OnEndDrag is a method that is called when the drag operation ends. It sets the position of the game object back to the
    /// StartPosition, re-enables the CanvasGroup component's blocksRaycasts property, and sets the game object's parent back to
    /// the OriginalParent. It then determines the destination of the drag operation by checking whether the pointer is currently
    /// over a game object with an IDragDestination component. If the pointer is not over such a game object, the method attempts
    /// to transfer the item to the ParentCanvas itself (if it implements IDragDestination). If the pointer is over a game object with an
    /// IDragDestination component, the method attempts to transfer the item to that game object.
    ///
    /// GetContainer is a helper method that returns the IDragDestination component of the game object that the pointer
    /// is currently over, or null if there is no such component.
    ///
    /// DropItemIntoContainer is a method that is called to transfer the item to a specific destination container.
    /// It first checks whether the destination is the same as the source container, and if so, does nothing. It then
    /// checks whether the destination and source containers both implement IDragContainer, and if either does not implement IDragContainer
    /// , it attempts to transfer the item using the AttemptSimpleTransfer method. Otherwise, it attempts to swap the items in the two
    /// containers using the AttemptSwap method.
    /// 
    /// AttemptSwap is a method that attempts to swap the items in the source and destination containers. It first removes the specified number
    /// of items from each container, then calculates how many items should be returned to each container using the CalculateTakeBack method.
    /// If either container is unable to accept the other container's item, it returns the excess items to the original container. Otherwise, it
    /// adds the remaining items to the destination container and removes the original items from the source container.
    ///
    /// CalculateTakeBack is a helper method that calculates how many of a given item should be returned to a container during a swap
    /// operation. It returns the maximum number of items that the container can accept, minus the number of items already present in the container.
    ///
    /// AttemptSimpleTransfer is a method that attempts to transfer the item to a destination container that does not implement IDragContainer.
    /// It first checks whether the destination container can accept the item, and if so, removes the item from the source container and adds
    /// it to the destination container.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler where T : class
    {
        // The position of the game object at the start of the drag
        private Vector3 StartPosition { get; set; } = new Vector3();
        // The parent of the game object at the start of the drag
        private Transform OriginalParent { get; set; } = null;
        // The container that the item being dragged is coming from
        private IDragSource<T> DragSource { get; set; } = null;
        // The canvas that the game object is a child of
        private Canvas ParentCanvas { get; set; } = null;

        /// <summary>
        /// Initialize the canvas and drag source properties
        /// </summary>
        private void Awake()
        {
            ParentCanvas = GetComponentInParent<Canvas>();
            DragSource = GetComponentInParent<IDragSource<T>>();
        }

        /// <summary>
        /// Called when the drag operation starts
        /// </summary>
        /// <param name="eventData"></param>
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            // Save the current position and parent of the game object
            StartPosition = transform.position;
            OriginalParent = transform.parent;

            // Disable raycasts on the game object so it does not block UI interaction
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            // Set the game object's parent to the canvas, so it is displayed on top of other UI elements
            transform.SetParent(ParentCanvas.transform, true);
        }

        /// <summary>
        /// Called during the drag operation
        /// </summary>
        /// <param name="eventData"></param>
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            // Set the position of the game object to the current pointer position
            transform.position = eventData.position;
        }

        /// <summary>
        /// Called when the drag operation ends
        /// </summary>
        /// <param name="eventData"></param>
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            // Reset the position and parent of the game object
            transform.position = StartPosition;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(OriginalParent, true);

            // Determine the destination container for the item
            IDragDestination<T> destinationContainer = !EventSystem.current.IsPointerOverGameObject()
                // If the pointer is not over a game object, try to transfer the item to the canvas itself
                ? ParentCanvas.GetComponent<IDragDestination<T>>()
                // Otherwise, try to transfer the item to the game object the pointer is over
                : GetContainer(eventData);

            // If a valid destination container was found, transfer the item
            if (destinationContainer != null) { DropItemIntoContainer(destinationContainer); }
        }

        /// <summary>
        /// Returns the IDragDestination<T> component of the game object the pointer is currently over, or null if there is no such component
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        private IDragDestination<T> GetContainer(PointerEventData eventData)
        {
            return !eventData.pointerEnter ? null : eventData.pointerEnter.GetComponentInParent<IDragDestination<T>>();
        }

        /// <summary>
        /// Attempts to transfer the item to the specified destination container
        /// </summary>
        /// <param name="destination"></param>
        private void DropItemIntoContainer(IDragDestination<T> destination)
        {
            // If the destination is the same as the source container, do nothing
            if (ReferenceEquals(destination, DragSource)) { return; }

            // Try to cast the destination and source containers to IDragContainer<T>
            IDragContainer<T> destinationContainer = destination as IDragContainer<T>;
            IDragContainer<T> sourceContainer = DragSource as IDragContainer<T>;

            // If either the destination or source container does not implement IDragContainer<T>,
            // or if the destination container already has an item,
            // attempt to transfer the item using the AttemptSimpleTransfer method
            if (destinationContainer == null || 
                sourceContainer == null ||
                destinationContainer.GetItem() == null ||
                ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
            {
                AttemptSimpleTransfer(destination);
                return;
            }

            // If both containers implement IDragContainer<T> and the destination container is empty,
            // attempt to swap the items in the two containers
            AttemptSwap(destinationContainer, sourceContainer);
        }

        /// <summary>
        /// Attempts to swap the items in the source and destination containers
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        private void AttemptSwap(IDragContainer<T> destination, IDragContainer<T> source)
        {
            // Remove the specified number of items from each container
            int removedSourceNumber = source.GetNumber();
            T removedSourceItem = source.GetItem();
            int removedDestinationNumber = destination.GetNumber();
            T removedDestinationItem = destination.GetItem();

            source.RemoveItems(removedSourceNumber);
            destination.RemoveItems(removedDestinationNumber);

            // Calculate how many items should be returned to each container
            int sourceTakeBackNumber = CalculateTakeBack(removedSourceItem, removedSourceNumber, source, destination);
            int destinationTakeBackNumber = CalculateTakeBack(removedDestinationItem, removedDestinationNumber, destination, source);

            // If either container cannot accept the other container's item, return excess items to the original container
            if (sourceTakeBackNumber > 0)
            {
                source.AddItemsToInventory(removedSourceItem, sourceTakeBackNumber);
                removedSourceNumber -= sourceTakeBackNumber;
            }
            if (destinationTakeBackNumber > 0)
            {
                destination.AddItemsToInventory(removedDestinationItem, destinationTakeBackNumber);
                removedDestinationNumber -= destinationTakeBackNumber;
            }

            // If either container cannot accept the other container's item, do nothing
            if (source.GetMaxAcceptableItemCount(removedDestinationItem) < removedDestinationNumber ||
                destination.GetMaxAcceptableItemCount(removedSourceItem) < removedSourceNumber ||
                removedSourceNumber == 0)
            {
                if (removedDestinationNumber > 0)
                {
                    destination.AddItemsToInventory(removedDestinationItem, removedDestinationNumber);
                }
                if (removedSourceNumber > 0)
                {
                    source.AddItemsToInventory(removedSourceItem, removedSourceNumber);
                }
                return;
            }

            // Otherwise, add the remaining items to the destination container and remove the original items from the source container
            if (removedDestinationNumber > 0)
            {
                source.AddItemsToInventory(removedDestinationItem, removedDestinationNumber);
            }
            if (removedSourceNumber > 0)
            {
                destination.AddItemsToInventory(removedSourceItem, removedSourceNumber);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        private bool AttemptSimpleTransfer(IDragDestination<T> destination)
        {
            var draggingItem = DragSource.GetItem();
            var draggingNumber = DragSource.GetNumber();

            var acceptable = destination.GetMaxAcceptableItemCount(draggingItem);
            var toTransfer = Mathf.Min(acceptable, draggingNumber);

            if (toTransfer <= 0) return true;
            DragSource.RemoveItems(toTransfer);
            destination.AddItemsToInventory(draggingItem, toTransfer);
            return false;

        }

        /// <summary>
        /// Returns the maximum number of a given item that a container can accept, minus the number of items already present in the container
        /// </summary>
        /// <param name="removedItem"></param>
        /// <param name="removedNumber"></param>
        /// <param name="removeSource"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        private int CalculateTakeBack(T removedItem, int removedNumber, IDragContainer<T> removeSource, IDragContainer<T> destination)
        {
            int takeBackNumber = 0;
            // Calculate the maximum number of items the container can accept
            int destinationMaxAcceptable = destination.GetMaxAcceptableItemCount(removedItem);

            // If the container can accept all removed items, return all removed items
            if (destinationMaxAcceptable >= removedNumber) { return takeBackNumber; }
            takeBackNumber = removedNumber - destinationMaxAcceptable;

            int sourceTakeBackAcceptable = removeSource.GetMaxAcceptableItemCount(removedItem);

            // Otherwise, return the maximum number of items the container can accept
            return sourceTakeBackAcceptable < takeBackNumber ? 0 : takeBackNumber;
        }
    }
}
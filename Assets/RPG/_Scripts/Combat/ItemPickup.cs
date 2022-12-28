using RPG.Controller;
using RPG.UI.Inventory;
using UnityEngine;
using static RPG.Helper.CursorHelper;

namespace RPG.Combat
{
    /// <summary>
    /// This script is a Unity component that allows an object to be picked up by the player when the player clicks on it.
    /// It is attached to a game object with a Pickup component, which is responsible for handling the actual pickup process.
    /// 
    /// The script has two main responsibilities:
    /// 
    /// Implementing the IRaycastable interface, which allows the object to be highlighted and interactable when the player's
    /// cursor is hovering over it. The GetCursorType method returns a cursor type (an enumeration defined in the CursorHelper class)
    /// that determines which cursor should be displayed when the player's cursor is over the object.
    /// 
    /// Handling player input when the object is clicked on. When the player clicks on the object, the script calls the
    /// PickupItem method on the attached Pickup component, which initiates the pickup process.
    /// 
    /// The script also has a Pickup property that is set to the Pickup component attached to the game object. This property
    /// is initialized in the Awake method, which is called when the component is enabled.
    /// </summary>
    [RequireComponent(typeof(Pickup))]
    public class ItemPickup : MonoBehaviour, IRaycastable
    {
        /// <summary>
        /// Property for the Pickup component attached to this game object.
        /// </summary>
        private Pickup Pickup { get; set; } = null;

        /// <summary>
        /// Initialize the Pickup property when the component is enabled.
        /// </summary>
        private void Awake()
        {
            Pickup = GetComponent<Pickup>();
        }

        /// <summary>
        /// Return the cursor type to display when the player's cursor is over this object.
        /// </summary>
        /// <returns>The pickup cursor type if the item can be picked up, full pickup cursor type otherwise</returns>
        public CursorType GetCursorType()
        {
            return Pickup.CanBePickedUp() ? CursorType.Pickup : CursorType.FullPickup;
        }

        /// <summary>
        /// Handle player input when the object is clicked on.
        /// </summary>
        /// <param name="callingController"></param>
        /// <returns></returns>
        public bool HandleRaycast(PlayerController callingController)
        {
            // If the left mouse button is pressed, initiate the pickup process.
            if (Input.GetMouseButtonDown(0))
            {
                Pickup.PickupItem();
            }
            return true;
        }
    }
}
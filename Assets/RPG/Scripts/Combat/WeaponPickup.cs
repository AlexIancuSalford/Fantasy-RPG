using RPG.Controller;
using static RPG.Helper.CursorHelper;
using UnityEngine;

namespace RPG.Combat
{
    /// <summary>
    /// This is a Unity script that defines a class called "WeaponPickup". The class has a field called "Weapon" which
    /// is a serialized Weapon object. The class also implements the "IRaycastable" interface, which has two methods:
    /// "HandleRaycast" and "GetCursorType".
    ///
    /// The "WeaponPickup" class has a method called "OnTriggerEnter" which is called when a collider enters the trigger
    /// attached to the object. If the collider belongs to an object with the "Player" tag, the "PickupWeapon" method is
    /// called with the collider's "Fighter" component as an argument.
    ///
    /// The "PickupWeapon" method calls the "EquipWeapon" method on the "Fighter" component of the player's game object,
    /// passing the "Weapon" field as an argument. It then destroys the game object that the script is attached to.
    ///
    /// The "HandleRaycast" method is called when the player is using a cursor to interact with objects in the game.
    /// If the left mouse button is pressed and the player is in range of the object, the "PickupWeapon" method is
    /// called with the player's "Fighter" component as an argument. The "HandleRaycast" method always returns true.
    ///
    /// The "GetCursorType" method returns a "CursorType" value indicating the type of cursor that should be displayed
    /// when the player is hovering over the object. In this case, it returns the "Pickup" cursor type.
    /// </summary>
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        // The weapon that will be picked up by the player.
        [field: SerializeField] public Weapon Weapon { get; private set; } = null;

        /// <summary>
        /// This method is called when a collider enters the trigger attached to the game object.
        /// If the collider belongs to an object with the "Player" tag, the PickupWeapon method is called with the collider's Fighter component as an argument.
        /// </summary>
        /// <param name="other">The collider that entered the trigger</param>
        private void OnTriggerEnter(Collider other)
        {
            // Check if the collider that entered the trigger is the player
            if (other.gameObject.tag.Equals("Player"))
            {
                PickupWeapon(other.gameObject.GetComponent<Fighter>());
            }
        }

        /// <summary>
        /// This method picks up the weapon and equips it on the player's Fighter component.
        /// It then destroys the game object that the script is attached to.
        /// </summary>
        /// <param name="fighter">The Fighter component of the player's game object</param>
        private void PickupWeapon(Fighter fighter)
        {
            // Get the fighter component on the player's game object and call the EquipWeapon method
            fighter.EquipWeapon(Weapon);
            // Destroy the weapon pickup game object
            Destroy(gameObject);
        }

        /// <summary>
        /// This method is called when the player is using a cursor to interact with objects in the game.
        /// If the left mouse button is pressed and the player is in range of the object, the PickupWeapon method is called with the player's Fighter component as an argument.
        /// </summary>
        /// <param name="caller">The PlayerController component of the player's game object</param>
        /// <returns>Always returns true</returns>
        public bool HandleRaycast(PlayerController caller)
        {
            // If the cursor is over the item, the mouse button is pressed and the player is in range,
            // pickup the item
            if (Input.GetMouseButtonDown(0) )
            {
                PickupWeapon(caller.GetComponent<Fighter>());
            }

            // Always return true
            return true;
        }

        /// <summary>
        /// This method returns a CursorType value indicating the type of cursor that should be displayed when the player is hovering over the object.
        /// </summary>
        /// <returns>The Pickup cursor type</returns>
        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}

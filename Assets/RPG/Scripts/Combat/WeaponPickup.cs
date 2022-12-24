using RPG.Controller;
using static RPG.Helper.CursorHelper;
using UnityEngine;

namespace RPG.Combat
{
    /// <summary>
    /// This class handles the behavior of weapon pickups in the game.
    /// When the player collides with a weapon pickup, the weapon will be equipped on the player's fighter component.
    /// The weapon pickup object will then be destroyed.
    /// </summary>
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        // The weapon that will be picked up by the player.
        [field: SerializeField] public Weapon Weapon { get; private set; } = null;

        private void OnTriggerEnter(Collider other)
        {
            // Check if the collider that entered the trigger is the player
            if (other.gameObject.tag.Equals("Player"))
            {
                PickupWeapon(other.gameObject.GetComponent<Fighter>());
            }
        }

        private void PickupWeapon(Fighter fighter)
        {
            // Get the fighter component on the player's game object and call the EquipWeapon method
            fighter.EquipWeapon(Weapon);
            // Destroy the weapon pickup game object
            Destroy(gameObject);
        }

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

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}

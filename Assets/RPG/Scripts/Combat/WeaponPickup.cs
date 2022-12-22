using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    /// <summary>
    /// This class handles the behavior of weapon pickups in the game.
    /// When the player collides with a weapon pickup, the weapon will be equipped on the player's fighter component.
    /// The weapon pickup object will then be destroyed.
    /// </summary>
    public class WeaponPickup : MonoBehaviour
    {
        // The weapon that will be picked up by the player.
        [field: SerializeField] public Weapon Weapon { get; private set; } = null;

        private void OnTriggerEnter(Collider other)
        {
            // Check if the collider that entered the trigger is the player
            if (other.gameObject.tag.Equals("Player"))
            {
                // Get the fighter component on the player's game object and call the EquipWeapon method
                other.gameObject.GetComponent<Fighter>().EquipWeapon(Weapon);
                // Destroy the weapon pickup game object
                Destroy(gameObject);
            }
        }
    }
}

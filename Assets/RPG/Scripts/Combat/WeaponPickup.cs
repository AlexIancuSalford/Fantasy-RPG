using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [field: SerializeField] public Weapon Weapon { get; private set; } = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                other.gameObject.GetComponent<Fighter>().EquipWeapon(Weapon);
                Destroy(gameObject);
            }
        }
    }
}

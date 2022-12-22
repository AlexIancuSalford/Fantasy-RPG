/*
 * This script appears to be for a Weapon scriptable object in a Unity game.
 * A scriptable object is a data asset that can contain data but does not
 * have any logic or behavior.
 *  
 * The script has several public fields that can be set in the Unity editor:
 *  
 * "OverrideController": an AnimatorOverrideController object representing
 * the animation controller to use when the weapon is equipped.
 *
 * "WeaponPrefab": a GameObject representing the weapon prefab to spawn when
 * the weapon is equipped.
 *
 * "AttackCooldown": a float representing the time in seconds between attacks
 * with this weapon.
 *
 * "Range": a float representing the range of the weapon's attacks.
 *
 * "Damage": a float representing the amount of damage dealt by the weapon's
 * attacks.
 *
 * "IsRightHanded": a bool representing whether the weapon is equipped in the
 * right hand.
 *
 * "Projectile": a Projectile object representing the projectile to launch
 * when the weapon is used to attack.
 *
 * The script also has several methods that can be called to manipulate the weapon in the game:
 *  
 * "SpawnWeapon": instantiates the weapon prefab and sets the weapon's
 * animation controller on the player character.
 *
 * "HasProjectile": returns a bool indicating whether the weapon has a
 * projectile associated with it.
 *
 * "LaunchProjectile": instantiates the weapon's projectile and sets it to
 * target the specified enemy.
 *
 * "DestroyOldWeapon": destroys the previously equipped weapon.
 *
 * The script is annotated with the "CreateAssetMenu" attribute, which allows
 * a new instance of the scriptable object to be created from the Unity
 * editor's "Create" menu. It is also annotated with the "SerializeField"
 * attribute, which allows the private fields to be visible and editable in
 * the Unity editor.
 */

using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Object/Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [field : SerializeField] private AnimatorOverrideController OverrideController { get; set; } = null;
        [field : SerializeField] private GameObject WeaponPrefab { get; set; } = null;
        [field : SerializeField] public float AttackCooldown { get; set; } = 1f;
        [field : SerializeField] public float Range { get; private set; } = 2f;
        [field : SerializeField] public float Damage { get; private set; } = 5f;
        [field : SerializeField] private bool IsRightHanded { get; set; } = true;
        [field : SerializeField] private Projectile Projectile { get; set; } = null;

        private const string WEAPON_NAME = "Weapon";

        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(leftHand, rightHand);

            if (WeaponPrefab != null)
            {
                GameObject weapon = Instantiate(WeaponPrefab, IsRightHanded ? rightHand : leftHand);
                weapon.name = WEAPON_NAME;
            }

            AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (OverrideController != null)
            {
                animator.runtimeAnimatorController = OverrideController;
            }
            else if(overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        public bool HasProjectile()
        {
            return Projectile != null;
        }

        public void LaunchProjectile(Transform leftHand, Transform rightHand, Health target)
        {
            Projectile projectileInst = Instantiate(
                Projectile, 
                (IsRightHanded ? rightHand : leftHand).position, 
                Quaternion.identity
                );
            projectileInst.SetTarget(target, Damage);
        }

        public void DestroyOldWeapon(Transform leftHand, Transform rightHand)
        {
            Transform oldWeapon = rightHand.Find(WEAPON_NAME);

            if (oldWeapon == null) { oldWeapon = leftHand.Find(WEAPON_NAME); }
            if (oldWeapon == null) { return; }

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
    }
}
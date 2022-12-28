using RPG.Attributes;
using RPG.Stats;
using RPG.UI.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    /// <summary>
    /// This is a scriptable object in Unity that represents a weapon. The scriptable object is serialized,
    /// meaning that its data can be saved and loaded at runtime.
    /// 
    /// The script contains a number of public properties, such as AttackCooldown, Range, and Damage, that describe
    /// the weapon's characteristics. It also has a property called PercentageBonusDamage, which represents a percentage
    /// increase in damage dealt by the weapon.
    /// 
    /// The script also contains a number of private fields that are serialized, such as OverrideController, which is an
    /// AnimatorOverrideController that can be used to override the character's animator controller with the weapon's animator
    /// controller, and WeaponPrefab, which is a prefab (template) for the weapon game object.
    /// 
    /// The script includes several methods that can be used to interact with the weapon. The SpawnWeapon method instantiates
    /// the weapon game object and sets the character's animator controller to the weapon's animator controller. The HasProjectile
    /// method returns a boolean indicating whether the weapon has a projectile. The LaunchProjectile method instantiates and
    /// launches a projectile from the weapon. The DestroyOldWeapon method destroys any existing weapon game object that the
    /// character may have equipped.
    /// 
    /// Finally, the script implements the IStatsProvider interface, which means that it has a method called GetStats that returns
    /// a collection of stats. It is not clear from the code provided what the GetStats method does or how it is used.
    /// </summary>
    [CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Object/Weapon", order = 0)]
    public class Weapon : EquipableItem, IStatsProvider
    {
        [field : SerializeField] private AnimatorOverrideController OverrideController { get; set; } = null;
        [field : SerializeField] private WeaponComponent WeaponPrefab { get; set; } = null;
        [field : SerializeField] public float AttackCooldown { get; set; } = 1f;
        [field : SerializeField] public float Range { get; private set; } = 2f;
        [field : SerializeField] public float Damage { get; private set; } = 5f;
        [field : SerializeField] public float PercentageBonusDamage { get; set; } = 0.0f;
        [field : SerializeField] private bool IsRightHanded { get; set; } = true;
        [field : SerializeField] private Projectile Projectile { get; set; } = null;

        private const string WEAPON_NAME = "Weapon";

        /// <summary>
        /// Spawns the weapon game object and overrides the character's animator controller with the weapon's AnimatorOverrideController.
        /// This method also returns the WeaponComponent that was just instantiated, so it can be used individually.
        /// </summary>
        /// <param name="rightHand">The character's right hand transform.</param>
        /// <param name="leftHand">The character's left hand transform.</param>
        /// <param name="animator">The character's animator component.</param>
        /// <returns>The WeaponComponent that was instantiated</returns>
        public WeaponComponent SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            // Destroy the old weapon, if any
            DestroyOldWeapon(leftHand, rightHand);

            WeaponComponent weaponComponent = null;
            // Spawn the weapon game object
            if (WeaponPrefab != null)
            {
                weaponComponent = Instantiate(WeaponPrefab, IsRightHanded ? rightHand : leftHand);
                weaponComponent.gameObject.name = WEAPON_NAME;
            }

            // Override the character's animator controller with the weapon's AnimatorOverrideController, if any
            AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (OverrideController != null)
            {
                animator.runtimeAnimatorController = OverrideController;
            }
            // Otherwise, restore the character's original animator controller
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            // If the 
            return weaponComponent;
        }

        /// <summary>
        /// Determines whether the weapon has a projectile.
        /// </summary>
        /// <returns>True if the weapon has a projectile, false otherwise.</returns>
        public bool HasProjectile()
        {
            return Projectile != null;
        }

        /// <summary>
        /// Launches a projectile from the weapon.
        /// </summary>
        /// <param name="leftHand">The character's left hand transform.</param>
        /// <param name="rightHand">The character's right hand transform.</param>
        /// <param name="target">The Health component of the target object to be hit by the projectile.</param>
        /// <param name="instigator">The game object that launched the projectile.</param>
        /// <param name="damage">The base damage of the instigator</param>
        public void LaunchProjectile(Transform leftHand, Transform rightHand, Health target, GameObject instigator, float damage)
        {
            // Create an instance of the projectile
            Projectile projectileInst = Instantiate(
                Projectile, 
                (IsRightHanded ? rightHand : leftHand).position, 
                Quaternion.identity
                );
            // Set the target and damage of the projectile
            projectileInst.SetTarget(target, instigator, damage);
        }

        /// <summary>
        /// Destroys the old weapon game object, if any.
        /// </summary>
        /// <param name="leftHand">The character's left hand transform.</param>
        /// <param name="rightHand">The character's right hand transform.</param>
        public void DestroyOldWeapon(Transform leftHand, Transform rightHand)
        {
            // Find the old weapon game object in the right hand
            Transform oldWeapon = rightHand.Find(WEAPON_NAME);

            // If not found, try finding it in the left hand
            if (oldWeapon == null) { oldWeapon = leftHand.Find(WEAPON_NAME); }
            // If still not found, return
            if (oldWeapon == null) { return; }

            // Rename the old weapon game object to avoid conflicts when destroying it
            oldWeapon.gameObject.name = "DESTROYING";
            // Destroy the old weapon game object
            Destroy(oldWeapon.gameObject);
        }

        /// <summary>
        /// This method returns the what the damage value should be based on a Stats attribute passed in.
        /// These values represent the multipliers that will be applied to the base value of a given attribute when calculating the final value.
        /// For example, if the weapon contains an entry for "Damage" with a value of 1.5, it means that any damage done by this fighter
        /// will be increased by the value returned here when calculating the final damage value.
        ///
        /// It is important to note that multiple additive multipliers can be applied if there are more in the list.
        /// </summary>
        /// <returns>A dictionary of attribute names and their corresponding float values</returns>
        public IEnumerable<float> GetModifiers(Stats.Stats stat)
        {
            if (stat == Stats.Stats.BaseDamage)
            {
                yield return Damage;
            }
        }

        /// <summary>
        /// This method returns the value of an attribute based on a Stats attribute passed in.
        /// These values represent the percentage increase or decrease that will be applied to the base value of a given attribute when calculating the final value.
        /// For example, if the weapon contains an entry for "PercentageBonusDamage" with a value of 25, it means that any damage done by this fighter
        /// will be increased by 25% (25) when calculating the final damage value.
        ///
        /// It is important to note that multiple percentage multipliers can be applied, and the percentage can also be negative,
        /// essentially turning it into a debuff instead of a buff.
        /// </summary>
        /// <returns>A dictionary of attribute names and their corresponding float values</returns>
        public IEnumerable<float> GetModifiersPercentage(Stats.Stats stat)
        {
            if (stat == Stats.Stats.BaseDamage)
            {
                yield return PercentageBonusDamage;
            }
        }
    }
}
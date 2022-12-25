using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    /// <summary>
    /// This script appears to be for a Weapon scriptable object in a Unity game.
    /// A scriptable object is a data asset that can contain data but does not
    /// have any logic or behavior. This one has some logic, though, mostly about
    /// weapon behaviour.
    ///  
    /// The script has several public fields that can be set in the Unity editor:
    ///  
    /// "OverrideController": an AnimatorOverrideController object representing
    /// the animation controller to use when the weapon is equipped.
    /// 
    /// "WeaponPrefab": a GameObject representing the weapon prefab to spawn when
    /// the weapon is equipped.
    /// 
    /// "AttackCooldown": a float representing the time in seconds between attacks
    /// with this weapon.
    /// 
    /// "Range": a float representing the range of the weapon's attacks.
    /// 
    /// "Damage": a float representing the amount of damage dealt by the weapon's
    /// attacks.
    /// 
    /// "IsRightHanded": a bool representing whether the weapon is equipped in the
    /// right hand.
    /// 
    /// "Projectile": a Projectile object representing the projectile to launch
    /// when the weapon is used to attack.
    /// 
    /// The script also has several methods that can be called to manipulate the weapon in the game:
    ///  
    /// "SpawnWeapon": instantiates the weapon prefab and sets the weapon's
    /// animation controller on the player character.
    /// 
    /// "HasProjectile": returns a bool indicating whether the weapon has a
    /// projectile associated with it.
    /// 
    /// "LaunchProjectile": instantiates the weapon's projectile and sets it to
    /// target the specified enemy.
    /// 
    /// "DestroyOldWeapon": destroys the previously equipped weapon.
    /// 
    /// The script is annotated with the "CreateAssetMenu" attribute, which allows
    /// a new instance of the scriptable object to be created from the Unity
    /// editor's "Create" menu. It is also annotated with the "SerializeField"
    /// attribute, which allows the private fields to be visible and editable in
    /// the Unity editor.
    /// </summary>
    [CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Object/Weapon", order = 0)]
    public class Weapon : ScriptableObject
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
    }
}
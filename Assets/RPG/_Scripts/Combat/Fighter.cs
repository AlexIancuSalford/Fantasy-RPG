using RPG.Attributes;
using RPG.Core;
using RPG.Helper;
using RPG.Movement;
using RPG.Save;
using RPG.Stats;
using RPG.UI.Inventory;
using UnityEngine;

namespace RPG.Combat
{
    /// <summary>
    /// This script is for a combat system. It is attached to a game object that will be able to attack other game objects.
    /// The script has various references to other components on the same game object, including an ActionManager,
    /// Animator, and Equipment.
    /// 
    /// The script includes an Update method that runs every frame. This method checks if the object has a target and if the
    /// target is within range. If the target is in range, the object will move towards it. If the target is not in range,
    /// the object will handle attack behavior. The script also has a method called Hit that is called when the object attacks
    /// its target. This method will either launch a projectile or deal damage directly to the target, depending on the current
    /// weapon.
    /// 
    /// The script also includes a Cancel method that stops the attack animation, cancels any movement, and removes the target.
    /// It also implements the IAction and ISaveableEntity interfaces, which allow the object to be controlled by the ActionManager
    /// and saved with the SaveSystem, respectively. The script also has a number of private fields and properties for tracking
    /// various values, such as the time since the last attack and the current weapon being used.
    /// </summary>
    public class Fighter : MonoBehaviour, IAction, ISaveableEntity
    {
        // Private properties for references to other components on the same game object
        public Health Target { get; private set; }
        private Mover MoverRef { get; set; }
        private ActionManager ActionManager { get; set; }
        private Animator Animator { get; set; }
        private Equipment Equipment { get; set; }

        // Private fields for tracking the time since the last attack and the current weapon being used
        private float timeSinceLastAttack = Mathf.Infinity;
        [ReadOnly] public Weapon CurrentWeapon = null;
        private CDeferredValue<WeaponComponent> CurrentWeaponComponent { get; set; } = null;

        // Public properties for transforms used to spawn projectiles and the default weapon to be equipped
        [field : SerializeField] private Transform RightHandTransform { get; set; }
        [field : SerializeField] private Transform LeftHandTransform { get; set; }
        [field : SerializeField] public Weapon DefaultWeapon { get; set; } = null;

        // Called when the script is first enabled
        private void Awake()
        {
            CurrentWeapon = DefaultWeapon;
            CurrentWeaponComponent = new CDeferredValue<WeaponComponent>(SetupWeaponComponent);

            // Get references to other components on the same game object
            MoverRef = GetComponent<Mover>();
            ActionManager = GetComponent<ActionManager>();
            Animator = GetComponent<Animator>();
            Equipment = GetComponent<Equipment>();

            // Subscribe to the EquipmentUpdated from the equipment script to update and equip weapon when one is placed in a slot
            if (Equipment != null)
            {
                Equipment.EquipmentUpdated += UpdateEquipment;
            }
        }

        private void Start()
        {
            CurrentWeaponComponent.EnsureInitialized();
        }

        // Update is called once per frame
        void Update()
        {
            // Increment the time since the last attack
            timeSinceLastAttack += Time.deltaTime;

            // If there is no target or the target is dead, do nothing
            if (Target == null) { return; }
            if (Target.IsDead()) { return; }

            // If the target is within range, move towards it
            if (IsTargetInRange(Target.transform))
            {
                MoverRef.MoveTo(Target.transform.position);
            }
            // Otherwise, cancel movement and handle attack behavior
            else
            {
                MoverRef.Cancel();
                HandleAttackBehaviour();
            }
        }

        /// <summary>
        /// This method is called as an animation event when the attack
        /// animation reaches the point where the attack hits the target.
        /// If the current weapon has a projectile, it launches the projectile
        /// towards the target using the LaunchProjectile() method of the
        /// Weapon class. If the weapon does not have a projectile, it deals
        /// damage directly to the target using the TakeDamage() method of the
        /// Health component.
        /// </summary>
        public void Hit()
        {
            // If there is no target, do nothing
            if (Target == null) { return; }

            CurrentWeaponComponent.Value.OnHit();

            float damage = GetComponent<BaseStats>().GetStat(Stats.Stats.BaseDamage);

            // If the current weapon has a projectile, launch it towards the target
            if (CurrentWeapon.HasProjectile())
            {
                CurrentWeapon.LaunchProjectile(RightHandTransform, LeftHandTransform, Target, gameObject, damage);
            }
            // Otherwise, deal damage directly to the target
            else
            {
                Target.TakeDamage(gameObject, damage);
            }
        }

        /// <summary>
        /// Wrapper for the Hit() method animation event.
        /// Projectile animations used in this project do
        /// not have a uniform format for their Hit event,
        /// so every new event (like in thi case, shoot) will
        /// be calling hit to standardize
        /// </summary>
        private void Shoot()
        {
            Hit();
        }

        /// <summary>
        /// Cancels the current attack and stops movement.
        ///
        /// This method is used to cancel the current attack and stop
        /// movement. It stops the attack animation using the
        /// StopAttackAnimation() method, cancels movement using the
        /// Cancel() method of the Mover component, and clears the current
        /// target by setting the Target property to null.
        /// </summary>
        public void Cancel()
        {
            StopAttackAnimation();
            MoverRef.Cancel();
            Target = null;
        }

        /// <summary>
        /// Starts attacking the given target.
        ///
        /// This method is used to start attacking a given target.
        /// It starts the action using the StartAction() method of the
        /// ActionManager component and sets the target by getting the
        /// Health component of the target game object.
        /// </summary>
        /// <param name="target">The target to attack</param>
        public void Attack(GameObject target)
        {
            // Start the action and set the target
            ActionManager.StartAction(this);
            Target = target.GetComponent<Health>();
        }

        /// <summary>
        /// This method is responsible for rotating the fighter to face the
        /// target and starting the attack animation when it is ready to
        /// attack again. It does this by using the LookAt() method to rotate
        /// the fighter towards the target and checking if the time since
        /// the last attack is greater than the attack cooldown of the
        /// current weapon. If it is, it starts the attack animation and
        /// resets the time since the last attack to 0.
        /// </summary>
        public void HandleAttackBehaviour()
        {
            // Rotate the fighter to face the target
            transform.LookAt(Target.transform);

            // If the time since the last attack is greater than the attack cooldown of the current weapon, start the attack animation
            if (timeSinceLastAttack > CurrentWeapon.AttackCooldown)
            {
                StartAttackAnimation();
                timeSinceLastAttack = 0f;
            }
            
        }

        /// <summary>
        /// Determines whether the given target can be attacked by the fighter.
        ///
        /// This method is used to determine whether the given target can be
        /// attacked by the fighter. It returns false if the target is null
        /// or does not have a Health component, and returns true if the
        /// target is alive (i.e. not dead).
        /// </summary>
        /// <param name="target">The target to check.</param>
        /// <returns>True if the target can be attacked, false otherwise.</returns>
        public bool CanAttack(GameObject target)
        {
            // If the target is null or does not have a Health component, return false
            if (target == null) { return false; }

            Health targetHealth = target.GetComponent<Health>();

            // Otherwise, return whether the target is alive and not null
            return targetHealth != null && !targetHealth.IsDead();
        }

        /// <summary>
        /// Determines whether the current target is within the range of the current weapon.
        ///
        /// This method is used to determine whether the current target is
        /// within the range of the current weapon. It does this by using
        /// the Distance() method of the Vector3 class to calculate the
        /// distance between the target and the fighter, and comparing it
        /// to the range of the current weapon. If the distance is within
        /// the range, it returns true, otherwise it returns false.
        /// </summary>
        /// <param name="target">The target to check distance from</param>
        /// <returns>True if the target is within range, false otherwise</returns>
        private bool IsTargetInRange(Transform target)
        {
            // Return whether the distance to the target is within the range of the current weapon
            return Vector3.Distance(target.position, gameObject.transform.position) >= CurrentWeapon.Range;
        }

        /// <summary>
        /// Stops the attack animation.
        ///
        /// This method is used to stop the attack animation. It does this
        /// by resetting the "attack" trigger and setting the "stopAttack"
        /// trigger on the Animator component. These triggers are used to
        /// control the attack animation using animation states and
        /// transitions in the animator controller.
        /// </summary>
        private void StopAttackAnimation()
        {
            // Reset the attack trigger and set the stop attack trigger to stop the attack animation
            Animator.ResetTrigger("attack");
            Animator.SetTrigger("stopAttack");
        }

        /// <summary>
        /// Starts the attack animation.
        ///
        /// This method is used to start the attack animation. It does this
        /// by resetting the "stopAttack" trigger and setting the "attack"
        /// trigger on the Animator component. These triggers are used to
        /// control the attack animation using animation states and
        /// transitions in the animator controller.
        /// </summary>
        private void StartAttackAnimation()
        {
            // Reset the stop attack trigger and set the attack trigger to start the attack animation
            Animator.ResetTrigger("stopAttack");
            Animator.SetTrigger("attack");
        }

        /// <summary>
        /// The EquipWeapon method is responsible for equipping a character with a weapon. It does this by calling the AttachWeapon
        /// method to set the character's CurrentWeapon field to the weapon being equipped and set up the weapon component for the weapon.
        /// 
        /// The EquipWeapon method also updates the animator's weapon type parameter to match the type of the equipped weapon,
        /// and sets the time since the last attack to 0 to reset the attack cooldown.
        ///
        /// The EquipWeapon method is similar to the AttachWeapon method, but it includes an additional line of code to update the animator's weapon
        /// type parameter. This is to allow the AttachWeapon method to be called by other scripts, while the EquipWeapon method is
        /// specifically intended to be called when the character is equipping a new weapon.
        /// </summary>
        /// <param name="weapon"></param>
        public void EquipWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            CurrentWeaponComponent.Value = AttachWeapon(weapon);
        }

        /// <summary>
        /// The AttachWeapon method is responsible for equipping a character with a weapon. It does this by setting the character's
        /// CurrentWeapon field to the weapon being equipped and setting up the weapon component for the weapon.
        /// 
        /// The weapon component is a script attached to the weapon game object that provides functionality specific to that weapon.
        /// The weapon component is set up by creating an instance of the weapon component and setting the character's CurrentWeaponComponent
        /// field to a CDeferredValue object that wraps the weapon component instance.
        /// 
        /// The AttachWeapon method also updates the animator's weapon type parameter to match the type of the equipped weapon,
        /// and sets the time since the last attack to 0 to reset the attack cooldown.
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns>The weapon component that was instantiated</returns>
        private WeaponComponent AttachWeapon(Weapon weapon)
        {
            return weapon.SpawnWeapon(RightHandTransform, LeftHandTransform, GetComponent<Animator>()); ;
        }

        /// <summary>
        /// Saves the state of the entity.
        ///
        /// This method is part of the implementation of the ISaveableEntity
        /// interface. It is used to save the state of the entity
        /// (in this case, the fighter). In this particular case, it returns
        /// the name of the current weapon as the object to be saved.
        /// This allows the save system to save the name of the weapon so it
        /// can be loaded later.
        ///
        /// Note that this method could be expanded to save additional state
        /// information if needed.For example, it could return a custom data
        /// structure containing the name of the current weapon as well as
        /// other information about the fighter.
        /// </summary>
        /// <returns>The state of the entity as an object</returns>
        public object SaveState()
        {
            // Return the name of the current weapon as the object to be saved
            return CurrentWeapon.name;
        }

        /// <summary>
        /// Loads the state of the entity from the given object.
        ///
        /// This method is part of the implementation of the ISaveableEntity
        /// interface. It is used to load the state of the entity
        /// (in this case, the fighter) from the given object.
        /// In this particular case, it expects the object to be a string
        /// containing the name of the weapon to be loaded. It loads the
        /// weapon using the Load() method of the Resources class and equips
        /// it using the EquipWeapon() method.
        ///
        /// Note that this method could be expanded to load additional state
        /// information if needed. For example, it could accept a custom data
        /// structure containing the name of the weapon as well as other
        /// information about the fighter, and use this information to restore
        /// the state of the fighter.
        /// </summary>
        /// <param name="obj"></param>
        public void LoadState(object obj)
        {
            // Load the weapon with the given name and equip it
            Weapon weapon = Resources.Load(obj as string) as Weapon;
            EquipWeapon(weapon);
        }

        /// <summary>
        /// This method is used to deffer the initialization of the WeaponComponent with the help of the helper class CDeferredValue until it is used
        /// to avoid race conditions or null pointer references (caused by race conditions or the order of script execution)
        /// </summary>
        /// <returns>The WeaponComponent that was spawned into the world</returns>
        private WeaponComponent SetupWeaponComponent()
        {
            return AttachWeapon(DefaultWeapon);
        }

        /// <summary>
        /// Method called when the EquipmentUpdated is triggered.
        ///
        /// This method will equip a weapon or other equipment when the weapon is placed into the equipment slot
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void UpdateEquipment()
        {
            // Get the weapon from the equipment weapon slot and cast it to the weapon type
            Weapon weapon = Equipment.GetItemInSlot(EquipLocation.Weapon) as Weapon;
            
            // Equip the weapon if there is a weapon, equip the default weapon if there is none
            // This will cover the case in which the player unequipped a weapon from the slot, as it will equip the unarmed "weapon"
            EquipWeapon(weapon == null ? DefaultWeapon : weapon);
        }
    }
}
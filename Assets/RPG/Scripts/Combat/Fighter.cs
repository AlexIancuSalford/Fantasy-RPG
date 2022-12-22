/*
 * The Fighter class extends MonoBehaviour and implements the IAction and ISaveableEntity interfaces.
 * This means it can be used as an action that can be performed by the ActionManager and can be saved and loaded by the save system.
 * 
 * The Target, MoverRef, ActionManager, and Animator properties are references to other components on the same game object. 
 * The Target property stores the current target of the fighter, MoverRef is a reference to the Mover component,
 * ActionManager is a reference to the ActionManager component, and Animator is a reference to the Animator component.
 *  
 *  The _timeSinceLastAttack field stores the time (in seconds) since the last attack was performed,
 *  and the _currentWeapon field stores the current weapon being used by the fighter.
 *  
 *  The RightHandTransform and LeftHandTransform properties are the transforms used to spawn
 *  projectiles when the current weapon has a projectile.
 *  The DefaultWeapon property is the weapon that will be equipped when the fighter is first initialized.
 *  
 *  The Awake() method is called when the script is first enabled.
 *  It checks if a weapon is currently equipped and, if not, equips the default weapon.
 *  
 *  The Start() method is called on the first frame that the script is enabled.
 *  It gets references to the Mover, ActionManager, and Animator components on the same game object.
 *  
 *  The Update() method is called once per frame. It increments the time since the last attack,
 *  checks if there is a target and if the target is dead, and either moves towards the target or handles attack behavior depending
 */

using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Save;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveableEntity
    {
        // Private properties for references to other components on the same game object
        private Health Target { get; set; }
        private Mover MoverRef { get; set; }
        private ActionManager ActionManager { get; set; }
        private Animator Animator { get; set; }

        // Private fields for tracking the time since the last attack and the current weapon being used
        private float _timeSinceLastAttack = Mathf.Infinity;
        private Weapon _currentWeapon = null;

        // Public properties for transforms used to spawn projectiles and the default weapon to be equipped
        [field : SerializeField] private Transform RightHandTransform { get; set; }
        [field : SerializeField] private Transform LeftHandTransform { get; set; }
        [field : SerializeField] public Weapon DefaultWeapon { get; set; } = null;

        // Called when the script is first enabled
        private void Awake()
        {
            // If no weapon is currently equipped, equip the default weapon
            if (_currentWeapon == null)
            {
                EquipWeapon(DefaultWeapon);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get references to other components on the same game object
            // TODO: Move this to Awake
            MoverRef = GetComponent<Mover>();
            ActionManager = GetComponent<ActionManager>();
            Animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            // Increment the time since the last attack
            _timeSinceLastAttack += Time.deltaTime;

            // If there is no target or the target is dead, do nothing
            if (Target == null) { return; }
            if (Target.IsDead) { return; }

            // If the target is within range, move towards it
            if (IsTargetInRange())
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

            // If the current weapon has a projectile, launch it towards the target
            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(RightHandTransform, LeftHandTransform, Target);
            }
            // Otherwise, deal damage directly to the target
            else
            {
                Target.TakeDamage(_currentWeapon.Damage);
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
            if (_timeSinceLastAttack > _currentWeapon.AttackCooldown)
            {
                StartAttackAnimation();
                _timeSinceLastAttack = 0f;
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
            return targetHealth != null && !targetHealth.IsDead;
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
        /// <returns>True if the target is within range, false otherwise</returns>
        private bool IsTargetInRange()
        {
            // Return whether the distance to the target is within the range of the current weapon
            return Vector3.Distance(Target.transform.position, gameObject.transform.position) >= _currentWeapon.Range;
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
        /// Equips the given weapon.
        ///
        /// This method is used to equip the given weapon. It sets the
        /// _currentWeapon field to the given weapon and calls the
        /// SpawnWeapon() method of the Weapon class to spawn the weapon
        /// on the character using the RightHandTransform and
        /// LeftHandTransform properties and the Animator component.
        /// </summary>
        /// <param name="weapon"></param>
        public void EquipWeapon(Weapon weapon)
        {
            // Set the current weapon to the given weapon and spawn it on the character
            _currentWeapon = weapon;
            _currentWeapon.SpawnWeapon(RightHandTransform, LeftHandTransform, GetComponent<Animator>());
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
            return _currentWeapon.name;
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
    }
}
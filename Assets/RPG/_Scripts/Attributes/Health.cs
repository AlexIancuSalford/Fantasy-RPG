using System;
using RPG.Core;
using RPG.Helper;
using RPG.Save;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    /// <summary>
    /// This is a script for a Health component in a Unity game. The script is
    /// attached to a game object and adds a number of properties and functions
    /// related to managing the health of the object.
    ///  
    /// The Health class has the following fields and properties:
    ///  
    /// CurrentHealth: A float representing the current health of the object.
    /// This field is serialized, which means it will be saved to and loaded from
    /// the game's save data. It has a default value of 100.
    /// 
    /// IsDead: A bool representing whether the object is dead or not. It has a
    /// default value of false.
    /// 
    /// ActionManager: A reference to an ActionManager component attached to the
    /// same game object.
    /// 
    /// Animator: A reference to an Animator component attached to the same game
    /// object.
    /// 
    /// The Health class has the following methods:
    ///  
    /// Awake(): This method is called when the script is first initialized.
    /// It retrieves references to the ActionManager and Animator components
    /// attached to the same game object, and sets the CurrentHealth field to the
    /// value returned by the GetHealth() method of the object's BaseStats
    /// component.
    /// 
    /// TakeDamage(GameObject instigator, float damage): This method is called
    /// when the object takes damage. It subtracts the specified damage from the
    /// CurrentHealth field, and sets IsDead to true if CurrentHealth is less than
    /// or equal to zero. If CurrentHealth is not zero, it returns without doing
    /// anything else. Otherwise, it calls the TriggerDeathAnimation() method with
    /// the false argument and the AwardExperience() method with the instigator
    /// argument.
    /// 
    /// TriggerDeathAnimation(bool isLoading): This method is called to trigger
    /// the death animation of the object. If IsDead is true, it returns without
    /// doing anything. If isLoading is true, it retrieves references to the
    /// Animator and ActionManager components attached to the same game object.
    /// It then sets IsDead to true and triggers the "death" animation of the
    /// Animator component. It also cancels the current action of the ActionManager
    /// component.
    /// 
    /// ToPercentage(): This method returns the CurrentHealth field as a percentage
    /// of the object's maximum health, as returned by the GetHealth() method of
    /// the object's BaseStats component.
    /// 
    /// AwardExperience(GameObject instigator): This method is called to award
    /// experience points to the specified instigator object. It retrieves the
    /// Experience component of the instigator object and calls its
    /// GainExperiencePoints() method with the value returned by the
    /// GetExperiencePoints() method of the object's BaseStats component. If the
    /// Experience component is not found, the method returns without doing
    /// anything.
    /// 
    /// SaveState(): This method is called to save the state of the object for
    /// the game's save data. It returns the CurrentHealth field.
    /// 
    /// LoadState(object obj): This method is called to load the state of the
    /// object from the game's save data. It sets the CurrentHealth field to the
    /// value of the obj argument, which is expected to be a float. If
    /// CurrentHealth is less than or equal to zero, it calls the
    /// TriggerDeathAnimation() method with the true argument.
    /// </summary>
    public class Health : MonoBehaviour, ISaveableEntity
    {
        [ReadOnly, SerializeField] private float CurrentHealth = -1f;
        [field : SerializeField] private UnityEvent<float> TakeDamageEvent { get; set; } = null;
        [field : SerializeField] private UnityEvent<float> SetHealthBarEvent { get; set; } = null;
        [field : SerializeField] private UnityEvent OnDie { get; set; } = null;

        public bool IsDead { get; private set; } = false;

        private ActionManager ActionManager { get; set; }
        private Animator Animator { get; set; }

        private void Awake()
        {
            // Retrieve references to the ActionManager and Animator components.
            ActionManager = GetComponent<ActionManager>();
            Animator = GetComponent<Animator>();

            if (CurrentHealth < 0f)
            {
                // Set the current health to the maximum health of the object.
                CurrentHealth = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            }
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenHealth;
            SetHealthBarEvent.Invoke(ToPercentage() / 100);
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenHealth;
        }

        /// <summary>
        /// Makes the object take damage.
        /// </summary>
        /// <param name="instigator">The game object that caused the damage.</param>
        /// <param name="damage">The amount of damage to be taken.</param>
        public void TakeDamage(GameObject instigator, float damage)
        {
            // Subtract the damage from the current health.
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            TakeDamageEvent.Invoke(damage);
            SetHealthBarEvent.Invoke(ToPercentage()/100);

            // If the object is not dead, return without doing anything else.
            if (CurrentHealth == 0)
            {
                OnDie.Invoke();
                // Otherwise, trigger the death animation and award experience to the instigator.
                TriggerDeathAnimation(false);
                AwardExperience(instigator);
            }
        }

        /// <summary>
        /// Triggers the death animation of the object.
        /// </summary>
        /// <param name="isLoading">Whether the object is being loaded from the save data or not.</param>
        private void TriggerDeathAnimation(bool isLoading)
        {
            // If the object is already dead, return without doing anything.
            if (IsDead) { return; }

            // If the object is being loaded, retrieve references to the Animator and ActionManager components.
            if (isLoading)
            {
                Animator = GetComponent<Animator>();
                ActionManager = GetComponent<ActionManager>();
            }

            // Set IsDead to true, trigger the "death" animation, and cancel the current action.
            IsDead = true;
            Animator.SetTrigger("death");
            ActionManager.CancelAction();
        }

        /// <summary>
        /// Returns the current health as a percentage of the maximum health.
        /// </summary>
        /// <returns>The current health as a percentage of the maximum health.</returns>
        public float ToPercentage()
        {
            return CurrentHealth * 100 / GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
        }

        /// <summary>
        /// Awards experience points to the specified game object.
        /// </summary>
        /// <param name="instigator">The game object to be awarded the experience points.</param>
        public void AwardExperience(GameObject instigator)
        {
            // Retrieve the Experience component of the instigator.
            Experience experience = instigator.GetComponent<Experience>();

            // If the Experience component is not found, return without doing anything.
            if (experience == null) { return; }

            // Otherwise, award the instigator the experience points.
            experience.GainExperiencePoints(GetComponent<BaseStats>().GetStat(Stats.Stats.ExperiencePoints));
        }

        /// <summary>
        /// Saves the state of the object for the game's save data.
        /// </summary>
        /// <returns>The state to be saved.</returns>
        public object SaveState()
        {
            return CurrentHealth;
        }

        /// <summary>
        /// Loads the state of the object from the game's save data.
        /// </summary>
        /// <param name="obj">The state to be loaded.</param>
        public void LoadState(object obj)
        {
            // Set the current health to the value of the obj argument.
            CurrentHealth = (float)obj;

            // If the object is dead, trigger the death animation.
            if (CurrentHealth <= 0)
            {
                TriggerDeathAnimation(true);
            }
        }

        /// <summary>
        /// This method is a getter for the current health
        /// </summary>
        /// <returns>The current health</returns>
        public float GetCurrentHealth()
        {
            return CurrentHealth;
        }

        /// <summary>
        /// This method returns the max health
        /// </summary>
        /// <returns>Return max health</returns>
        public float GetMaxHealth()
        {
            return MathF.Max(CurrentHealth,GetComponent<BaseStats>().GetStat(Stats.Stats.Health));
        }

        /// <summary>
        /// This method sets the current health to the value corresponding to its level
        /// </summary>
        private void RegenHealth()
        {
            CurrentHealth = GetMaxHealth();
        }
    }
}
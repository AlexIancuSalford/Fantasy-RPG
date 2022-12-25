using RPG.Save;
using System;
using UnityEngine;

namespace RPG.Stats
{
    /// <summary>
    /// This script defines the Experience class. It has a single field, ExperiencePoints, which is a float that
    /// represents the current amount of experience points the character has. The class also has an event called
    /// OnGainExperiencePoints that is triggered when the character gains experience points.
    /// 
    /// The Experience class also implements the ISaveableEntity interface, which requires it to have
    /// implementations of the SaveState and LoadState methods. These methods allow the Experience class to
    /// be saved and loaded as part of the game's save data.
    /// 
    /// The Experience class has a single public method, GainExperiencePoints, which adds a specified amount of
    /// experience points to the character's total experience points and triggers the OnGainExperiencePoints event.
    /// </summary>
    public class Experience : MonoBehaviour, ISaveableEntity
    {
        [field : SerializeField] public float ExperiencePoints { get; private set; }

        // Event that is called when the character gains experience points.
        public event Action OnGainExperiencePoints;

        /// <summary>
        /// Adds the specified amount of experience points to the character's experience points.
        /// </summary>
        /// <param name="experiencePoints">The amount of experience points to add.</param>
        public void GainExperiencePoints(float experiencePoints)
        {
            ExperiencePoints += experiencePoints;
            // Invoke the OnGainExperiencePoints event
            OnGainExperiencePoints();
        }

        /// <summary>
        /// Saves the current state of the experience component.
        /// </summary>
        /// <returns>The current state of the experience component.</returns>
        public object SaveState()
        {
            return ExperiencePoints;
        }

        /// <summary>
        /// Loads the specified state of the experience component.
        /// </summary>
        /// <param name="obj">The state to load.</param>
        public void LoadState(object obj)
        {
            ExperiencePoints = (float)obj;
        }
    }
}

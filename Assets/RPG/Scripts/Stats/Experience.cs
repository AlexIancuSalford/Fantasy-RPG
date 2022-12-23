using System;
using RPG.Save;
using UnityEngine;

namespace RPG.Stats
{
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

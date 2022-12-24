using System;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    /// <summary>
    /// The script is a component that can be attached to a game object in a Unity scene, and it
    /// provides functionality related to character stats and leveling up.
    /// 
    /// The script has several fields, which are variables that are serialized and can be set in the Unity inspector.
    /// These include a starting level, a character class, a progression object, a prefab for a level-up effect, and a
    /// boolean flag indicating whether to use modifiers. The script also has several properties, which are methods that
    /// act like variables but can have additional logic. These include the current level and an experience component.
    /// 
    /// The script has several methods, which are functions that perform specific tasks. The Awake method is called when the
    /// script is initialized, and it gets a reference to the experience component. The OnEnable and OnDisable methods
    /// are called when the script component is enabled or disabled, respectively, and they register or unregister a method
    /// for gaining experience points. The Start method is called when the script component is started, and it sets the
    /// current level to the level determined by the experience points. The UpdateLevel method is called when experience points
    /// are gained and it updates the current level if the new level is higher than the current level. The GetStat method returns
    /// a stat value for a given stat type, taking into account the character's level and any stat modifiers.
    /// The GetModifiersPercentage and GetModifiers methods return the sum of all stat modifiers as a percentage or a flat value,
    /// respectively, for a given stat type. The GetLevel method returns the character's level based on the amount of
    /// experience points they have.
    /// 
    /// Finally, the script has an event called OnLevelUp that is triggered when the character levels up. This event can
    /// be subscribed to by other components in order to perform some action when the character levels up.
    /// </summary>
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [field : SerializeField] private CharacterClass CharacterClass { get; set; }
        [field : SerializeField] private Progression Progression { get; set; } = null;
        [field : SerializeField] private GameObject LevelUpEffect { get; set; } = null;
        [field : SerializeField] private bool ShouldUseModifiers { get; set; } = false;

        public int CurrentLevel { get; private set; }
        private Experience Experience { get; set; }

        public event Action OnLevelUp;

        private void Awake()
        {
            // Get a reference to the experience component.
            Experience = GetComponent<Experience>();
        }

        private void OnEnable()
        {
            // Register the UpdateLevel method as a handler for the OnGainExperiencePoints event.
            if (Experience != null)
            {
                Experience.OnGainExperiencePoints += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            // Unregister the UpdateLevel method as a handler for the OnGainExperiencePoints event.
            if (Experience != null)
            {
                Experience.OnGainExperiencePoints -= UpdateLevel;
            }
        }

        private void Start()
        {
            // Set the current level to the level determined by the experience points.
            CurrentLevel = GetLevel();
        }

        /// <summary>
        /// Updates the current level of the character if the new level is higher than the current level.
        /// </summary>
        private void UpdateLevel()
        {
            // Calculate the new level based on the experience points.
            int newLevel = GetLevel();

            // If the new level is higher than the current level, update the current level and trigger the OnLevelUp event.
            if (newLevel > CurrentLevel)
            {
                CurrentLevel = newLevel;
                Instantiate(LevelUpEffect, transform);
                OnLevelUp();
            }
        }

        /// <summary>
        /// Gets the stat value for a given stat type, taking into account the character's level and any stat modifiers.
        /// </summary>
        /// <param name="stat">The stat type to get the value for.</param>
        /// <returns>The stat value for the given stat type.</returns>
        public float GetStat(Stats stat)
        {
            // Calculate the base stat value based on the character's level and the progression object.
            // Add any flat stat modifiers from other components.
            // Add any percentage-based stat modifiers from other components.
            // Return the final stat value by applying the modifiers to the base value.
            return (Progression.GetStat(stat, CharacterClass, GetLevel()) + GetModifiers(stat)) * (1 + GetModifiersPercentage(stat) / 100);
        }

        /// <summary>
        /// Gets the sum of all percentage-based stat modifiers for a given stat type from other components.
        /// </summary>
        /// <param name="stat">The stat type to get the modifiers for.</param>
        /// <returns>The sum of all percentage-based stat modifiers for the given stat type.</returns>
        private float GetModifiersPercentage(Stats stat)
        {
            // Return 0 if stat modifiers should not be used.
            if (!ShouldUseModifiers) { return 0; }

            // Get all components that provide stat modifiers.
            // Return the sum of all percentage-based stat modifiers for the given stat type from all stat modifier providers.
            return GetComponents<IStatsProvider>().Sum(statsProvider => statsProvider.GetModifiersPercentage(stat).Sum());
        }

        /// <summary>
        /// Gets the sum of all flat stat modifiers for a given stat type from other components.
        /// </summary>
        /// <param name="stat">The stat type to get the modifiers for.</param>
        /// <returns>The sum of all flat stat modifiers for the given stat type.</returns>
        private float GetModifiers(Stats stat)
        {
            // Return 0 if stat modifiers should not be used.
            if (!ShouldUseModifiers) { return 0; }

            // Get all components that provide stat modifiers.
            // Return the sum of all flat stat modifiers for the given stat type from all stat modifier providers.
            return GetComponents<IStatsProvider>().Sum(statsProvider => statsProvider.GetModifiers(stat).Sum());
        }

        /// <summary>
        /// Gets the character's level based on the amount of experience points they have.
        /// </summary>
        /// <returns>The character's level.</returns>
        public int GetLevel()
        {
            // Get the experience component.
            Experience experience = GetComponent<Experience>();

            // If the experience component is not present, return the starting level.
            if (experience == null) { return startingLevel; }

            // Get the current amount of experience points.
            float xp = experience.ExperiencePoints;
            // Get the maximum number of levels - 1 from the progression object.
            int pLevel = Progression.GetLevels(Stats.LevelUpXp, CharacterClass);

            // Iterate through all levels, starting from level 1.
            for (int i = 1; i <= pLevel; i++)
            {
                // Get the amount of experience points needed to reach the current level.
                float levelUpXp = Progression.GetStat(Stats.LevelUpXp, CharacterClass, i);

                // If the character has not reached the current level, return the previous level.
                if (levelUpXp > xp) { return i; }
            }

            // If the character has reached or exceeded the maximum number of levels, return the maximum level plus 1 (to reach the next level).
            return pLevel + 1;
        }
    }
}

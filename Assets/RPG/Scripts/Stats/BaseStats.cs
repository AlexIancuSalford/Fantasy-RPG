using System;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        // The starting level for the character. TODO: Remove in the future
        [SerializeField] private int startingLevel = 1;
        [field : SerializeField] private CharacterClass CharacterClass { get; set; }
        [field : SerializeField] private Progression Progression { get; set; } = null;
        [field : SerializeField] private GameObject LevelUpEffect { get; set; } = null;

        public int CurrentLevel { get; private set; }

        public Action OnLevelUp;

        private void Start()
        {
            int newLevel = GetLevel();
            Experience experience = GetComponent<Experience>();

            if (experience != null)
            {
                // Subscribe to the OnGainExperiencePoints event of the experience component
                experience.OnGainExperiencePoints += UpdateLevel;
            }
        }

        /// <summary>
        /// Updates the current level of the character if the new level is greater than the current level.
        /// This method is called whenever the character gains experience points.
        /// </summary>
        private void UpdateLevel()
        {
            int newLevel = GetLevel();
            // If the new level is greater than the current level, set the current level to the new level
            if (newLevel > CurrentLevel)
            {
                CurrentLevel = newLevel;
                // Spawn particle effect on level up
                Instantiate(LevelUpEffect, transform);
                OnLevelUp();
            }
        }

        /// <summary>
        /// Gets the value of the specified stat.
        /// </summary>
        /// <param name="stat">The stat to get the value of.</param>
        /// <returns>The value of the specified stat.</returns>
        public float GetStat(Stats stat)
        {
            // Return the stat value using the Progression data.
            return Progression.GetStat(stat, CharacterClass, GetLevel()) + GetModifiers(stat);
        }

        private float GetModifiers(Stats stat)
        {
            return GetComponents<IStatsProvider>().Sum(statsProvider => statsProvider.GetModifiers(stat).Sum());
        }

        /// <summary>
        /// Gets the current level of the character.
        /// </summary>
        /// <returns>The current level of the character.</returns>
        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();

            // If there is no experience component, return the starting level
            if (experience == null) { return startingLevel; }

            // Get the current experience points of the character.
            float xp = experience.ExperiencePoints;
            // Get the penultimate level of the character.
            int pLevel = Progression.GetLevels(Stats.LevelUpXp, CharacterClass);

            // Iterate through each level up to the penultimate level.
            for (int i = 1; i <= pLevel; i++)
            {
                // Get the experience points required for the current level.
                float levelUpXp = Progression.GetStat(Stats.LevelUpXp, CharacterClass, i);

                // If the required experience points are greater than the current experience points, return the current level.
                if (levelUpXp > xp) { return i; }
            }

            // If the loop completes, return the maximum level, which will be the next level.
            // Meaning the player has enough xp to level up
            return pLevel + 1;
        }
    }
}

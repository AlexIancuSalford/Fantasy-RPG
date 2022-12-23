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

        /// <summary>
        /// Gets the value of the specified stat.
        /// </summary>
        /// <param name="stat">The stat to get the value of.</param>
        /// <returns>The value of the specified stat.</returns>
        public float GetStat(Stats stat)
        {
            // Return the stat value using the Progression data.
            return Progression.GetStat(stat, CharacterClass, startingLevel);
        }

        /// <summary>
        /// Gets the current level of the character.
        /// </summary>
        /// <returns>The current level of the character.</returns>
        public float GetLevel()
        {
            // Get the current experience points of the character.
            float xp = GetComponent<Experience>().ExperiencePoints;
            // Get the penultimate level of the character.
            float pLevel = Progression.GetLevels(Stats.LevelUpXp, CharacterClass);

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

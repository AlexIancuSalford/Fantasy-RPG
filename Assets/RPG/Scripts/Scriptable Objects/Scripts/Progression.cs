/*
 * This script defines a Progression class, which is a scriptable
 * object in Unity.Scriptable objects are asset files that contain
 * data that can be used by other components in your game.
 *  
 * The Progression class has a serialized field called progressionClasses,
 * which is an array of ProgressionClass objects. Each
 * ProgressionClass object has a characterClass field and a stats
 * field. The characterClass field is an enumeration (CharacterClass)
 * that represents a character class in a role-playing game, and the
 * stats field is an array of ProgressionStats objects.
 * Each ProgressionStats object has a stat field, which
 * is another enumeration (Stats) that represents a stat in the game,
 * and a levels field, which is an array of floats.
 *  
 * The Progression class also has a public method called GetStat, which
 * takes three arguments: a Stats enumeration, a CharacterClass enumeration,
 * and an integer (level). This method returns the value of a specific
 * stat for a specific character class at a specific level.
 *  
 * The method works by first filtering the progressionClasses array to
 * find the ProgressionClass object that corresponds to the specified
 * character class. It then selects all the ProgressionStats objects from
 * that ProgressionClass object and filters them again to find the
 * ProgressionStats object that corresponds to the specified stat.
 * The method then returns the value at the specified level in the
 * levels array for that ProgressionStats object. If no matching
 * ProgressionStats object is found, the method returns 0 by default.
 */

using System;
using UnityEngine;
using System.Linq;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Scriptable Object/Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionClass[] progressionClasses = null;

        /// <summary>
        /// Gets the value of a specific stat for a specific character class at a specific level.
        /// </summary>
        /// <param name="stat">The stat to get the value of.</param>
        /// <param name="characterClass">The character class to get the stat value for.</param>
        /// <param name="level">The level to get the stat value at.</param>
        /// <returns>The value of the stat at the specified level for the specified character class, or 0 if no matching data was found.</returns>
        public float GetStat(Stats stat, CharacterClass characterClass, int level)
        {
            return progressionClasses
                // Find the ProgressionClass object for the specified character class
                .Where(progressionClass => progressionClass.characterClass == characterClass)
                // Select all the ProgressionStats objects for that character class
                .SelectMany(progressionClass => progressionClass.stats)
                // Find the ProgressionStats object for the specified stat
                .Where(progressionStats => progressionStats.stat == stat && progressionStats.levels.Length >= level)
                // Return the value at the specified level in the levels array for that stat
                .Select(progressionStats => progressionStats.levels[level - 1])
                // If no matching ProgressionStats object is found, return 0 by default
                .FirstOrDefault();
        }

        [Serializable]
        private class ProgressionClass
        {
            // Public fields are automatically serialized, but I'll leave
            // this here to make it clear
            [SerializeField] public CharacterClass characterClass;
            [SerializeField] public ProgressionStats[] stats;
        }

        [Serializable]
        private class ProgressionStats
        {
            // Public fields are automatically serialized, but I'll leave
            // this here to make it clear
            [SerializeField] public Stats stat;
            [SerializeField] public float[] levels;
        }
    }
}

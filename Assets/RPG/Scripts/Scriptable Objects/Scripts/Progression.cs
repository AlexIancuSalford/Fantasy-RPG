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
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Scriptable Object/Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionClass[] progressionClasses = null;

        private Dictionary<CharacterClass, Dictionary<Stats, float[]>> dictionaryLookup = null;

        /// <summary>
        /// Gets the value of a specific stat for a specific character class at a specific level.
        /// </summary>
        /// <param name="stat">The stat to get the value of.</param>
        /// <param name="characterClass">The character class to get the stat value for.</param>
        /// <param name="level">The level to get the stat value at.</param>
        /// <returns>The value of the stat at the specified level for the specified character class, or 0 if no matching data was found.</returns>
        public float GetStat(Stats stat, CharacterClass characterClass, int level)
        {
            Lookup();

            return dictionaryLookup[characterClass][stat].Length < level 
                ? 0
                : dictionaryLookup[characterClass][stat][level - 1];
        }

        private void Lookup()
        {
            if (dictionaryLookup != null) { return; }

            dictionaryLookup = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();

            foreach (ProgressionClass progressionClass in progressionClasses)
            {
                Dictionary<Stats, float[]> statLookup = new Dictionary<Stats, float[]>();

                foreach (ProgressionStats progressionStat in progressionClass.stats)
                {
                    statLookup[progressionStat.stat] = progressionStat.levels;
                }

                dictionaryLookup[progressionClass.characterClass] = statLookup;
            }
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

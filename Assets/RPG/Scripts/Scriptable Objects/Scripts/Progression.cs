/*
 * This script defines a Unity scriptable object called
 * Progression. A scriptable object is a type of asset
 * that allows you to store data that can be edited in
 * the Unity editor.
 *  
 * The Progression scriptable object has a serialized
 * field called progressionClasses which is an array
 * of ProgressionClass objects. Each ProgressionClass
 * object has two serialized fields: a CharacterClass
 * field and an array of ProgressionStats objects.
 * Each ProgressionStats object has two serialized
 * fields: a Stats field and an array of floats called levels.
 *  
 *  The Progression scriptable object also has a Dictionary
 * field called dictionaryLookup which is used to store
 * the data from the progressionClasses field in a more
 * easily accessible form.
 *  
 *  The Progression scriptable object has a public method
 * called GetStat which takes in three arguments:
 * a Stats enum, a CharacterClass enum, and an
 * integer called level. This method returns the
 * value of the stat specified by the Stats enum
 * for the character class specified by the CharacterClass
 * enum at the level specified by the level argument.
 * If the level specified is greater than the number
 * of levels available for that stat, the method returns 0.
 *  
 *  The Progression scriptable object also has a private
 * method called Lookup which is used to populate the
 * dictionaryLookup field with the data from the
 * progressionClasses field. This method is called each
 * time the GetStat method is called, but only if the
 * dictionaryLookup field is null. This allows the
 * dictionaryLookup field to be initialized only once,
 * improving the performance of the GetStat method.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Scriptable Object/Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionClass[] progressionClasses = null;

        private Dictionary<CharacterClass, Dictionary<Stats, float[]>> dictionaryLookup = null;

        /// <summary>
        /// Returns the value of the stat specified by the Stats enum for the character class specified by the CharacterClass enum at the level specified by the level argument.
        /// If the level specified is greater than the number of levels available for that stat, the method returns 0.
        /// </summary>
        /// <param name="stat">The stat to get the value of.</param>
        /// <param name="characterClass">The character class to get the stat value for.</param>
        /// <param name="level">The level to get the stat value for.</param>
        /// <returns>The value of the stat at the specified level for the specified character class.</returns>
        public float GetStat(Stats stat, CharacterClass characterClass, int level)
        {
            // Initialize the dictionaryLookup field if it hasn't already been initialized.
            Lookup();

            // Return 0 if the level specified is greater than the number of levels available for that stat,
            // otherwise return the value of the stat at the specified level for the specified character class.
            return dictionaryLookup[characterClass][stat].Length < level 
                ? 0
                : dictionaryLookup[characterClass][stat][level - 1];
        }

        /// <summary>
        /// This method checks the number of available levels for the specified stat and character class
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="characterClass"></param>
        /// <returns>The number of available levels</returns>
        public int GetLevels(Stats stat, CharacterClass characterClass)
        {
            // If the dictionary lookup is now initialized yet, initialize it
            if (dictionaryLookup == null) { Lookup(); }

            return dictionaryLookup[characterClass][stat].Length;
        }

        /// <summary>
        /// Populates the dictionaryLookup field with the data from the progressionClasses field.
        /// This method is called each time the GetStat method is called, but only if the dictionaryLookup field is null.
        /// This allows the dictionaryLookup field to be initialized only once, improving the performance of the GetStat method.
        /// </summary>
        private void Lookup()
        {
            // Return if the dictionaryLookup field has already been initialized.
            if (dictionaryLookup != null) { return; }

            // Initialize the dictionaryLookup field.
            dictionaryLookup = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();

            // Loop through each ProgressionClass object in the progressionClasses array.
            foreach (ProgressionClass progressionClass in progressionClasses)
            {
                // Dictionary for storing data from the ProgressionStats objects in a more easily accessible form.
                Dictionary<Stats, float[]> statLookup = new Dictionary<Stats, float[]>();

                // Loop through each ProgressionStats object in the stats array of the current ProgressionClass object.
                foreach (ProgressionStats progressionStat in progressionClass.stats)
                {
                    // Add the stat and levels data from the current ProgressionStats object to the statLookup dictionary.
                    statLookup[progressionStat.stat] = progressionStat.levels;
                }

                // Add the stat and levels data to the main dictionary
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

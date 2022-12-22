using System;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Scriptable Object/Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionClass[] progressionClasses = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (ProgressionClass progressionClass in progressionClasses)
            {
                if (progressionClass.characterClass == characterClass)
                {
                    //return progressionClass.health[level - 1];
                }
            }

            return 0;
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
            [SerializeField] public Stats stats;
            [SerializeField] public float[] levels;
        }
    }
}

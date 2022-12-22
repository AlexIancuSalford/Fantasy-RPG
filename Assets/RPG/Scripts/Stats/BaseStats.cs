using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [field : SerializeField] private CharacterClass CharacterClass { get; set; }
        [field : SerializeField] private Progression Progression { get; set; } = null;

        public float GetStat(Stats stat)
        {
            return Progression.GetStat(stat, CharacterClass, startingLevel);
        }
    }
}

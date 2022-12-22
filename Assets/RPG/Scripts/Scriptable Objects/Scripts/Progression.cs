using System;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Scriptable Object/Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionClass[] progressionClasses = null;

        [Serializable]
        private class ProgressionClass
        {
            [SerializeField] private CharacterClass characterClass;
            [SerializeField] private float[] health;
        }
    }
}

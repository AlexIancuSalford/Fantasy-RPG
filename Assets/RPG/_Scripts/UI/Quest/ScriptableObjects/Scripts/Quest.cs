using UnityEngine;

namespace RPG.UI.Quest
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Object/Quest")]
    public class Quest : ScriptableObject
    {
        [field : SerializeField] public string[] Objectives { get; private set; } = null;

        public string GetQuestTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return Objectives.Length;
        }
    }
}

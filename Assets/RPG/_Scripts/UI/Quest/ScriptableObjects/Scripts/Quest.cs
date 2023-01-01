using UnityEngine;

namespace RPG.UI.Quest
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Object/Quest")]
    public class Quest : ScriptableObject
    {
        [field : SerializeField] private string[] Objectives { get; set; } = null;

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

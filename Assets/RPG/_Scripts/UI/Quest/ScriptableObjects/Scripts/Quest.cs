using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.UI.Quest
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Object/Quest")]
    public class Quest : ScriptableObject
    {
        [field : SerializeField] public List<Objective> Objectives { get; private set; } = new List<Objective>();
        [field: SerializeField] public List<Reward> Rewards { get; private set; } = new List<Reward>();

        public string GetQuestTitle()
        {
            return name;
        }

        public bool HasObjective(string objectiveRef)
        {
            return Objectives.Any(obj => obj.Reference == objectiveRef);
        }

        public static Quest GetQuestByName(string name)
        {
            return Resources.LoadAll<Quest>("").FirstOrDefault(quest => quest.name == name);
        }
    }
}

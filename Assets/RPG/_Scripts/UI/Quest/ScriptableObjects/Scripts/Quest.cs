using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.UI.Quest
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Object/Quest")]
    public class Quest : ScriptableObject
    {
        [field : SerializeField] public List<string> Objectives { get; private set; } = new List<string>();

        public string GetQuestTitle()
        {
            return name;
        }

        public bool HasObjective(string objective)
        {
            return Objectives.Contains(objective);
        }

        public static Quest GetQuestByName(string name)
        {
            return Resources.LoadAll<Quest>("").FirstOrDefault(quest => quest.name == name);
        }
    }
}

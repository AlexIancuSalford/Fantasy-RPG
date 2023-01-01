using System;
using System.Collections.Generic;
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
    }
}

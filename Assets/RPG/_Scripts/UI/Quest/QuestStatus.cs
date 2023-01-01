using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quest
{
    [System.Serializable]
    public class QuestStatus
    {
        [field : SerializeField] public Quest Quest { get; private set; } = null;
        [field: SerializeField] public List<string> CompletedObjective { get; private set; } = null;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestList : MonoBehaviour
    {
        public List<QuestStatus> QuestStatuses { get; private set; } = new List<QuestStatus>();

        public event Action QuestStatusChanged;

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) { return; }

            QuestStatus newStatus = new QuestStatus(quest);
            QuestStatuses.Add(newStatus);
            QuestStatusChanged?.Invoke();
        }

        public bool HasQuest(Quest quest)
        {
            return QuestStatuses.Any(questStatus => questStatus.Quest == quest);
        }
    }
}

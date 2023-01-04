using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Save;
using RPG.UI.Inventory;
using UnityEngine;
using PlayerInventory = RPG.UI.Inventory.Inventory;

namespace RPG.UI.Quest
{
    public class QuestList : MonoBehaviour, ISaveableEntity, IEvaluator
    {
        public List<QuestStatus> QuestStatuses { get; private set; } = new List<QuestStatus>();

        public event Action QuestStatusChanged;

        private void Start()
        {
            QuestStatusChanged?.Invoke();
        }

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

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus questStatus = GetQuestStatus(quest);
            questStatus.CompleteObjective(objective);

            if (questStatus.IsQuestComplete())
            {
                GiveReward(quest);
            }

            QuestStatusChanged?.Invoke();
        }

        public QuestStatus GetQuestStatus(Quest quest)
        {
            return QuestStatuses.FirstOrDefault(questStatus => questStatus.Quest == quest);
        }

        private void GiveReward(Quest quest)
        {
            foreach (Reward reward in quest.Rewards)
            {
                if (!GetComponent<PlayerInventory>().AddToFirstEmptySlot(reward.Item, reward.Number))
                {
                    GetComponent<ItemDropper>().DropItem(reward.Item, reward.Number);
                }
            }
        }

        public object SaveState()
        {
            List<object> state = new List<object>();

            foreach (QuestStatus questStatus in QuestStatuses)
            {
                state.Add(questStatus.SaveState());
            }

            return state;
        }

        public void LoadState(object obj)
        { 
            List<object> objects = obj as List<object>;

            QuestStatuses.Clear();

            foreach (object objectState in objects)
            {
                QuestStatuses.Add(new QuestStatus(objectState));
            }
        }

        public bool? Evaluate(string predicate, string[] args)
        {
            if (predicate != "HasQuest") { return null; }

            return HasQuest(Quest.GetQuestByName(args[0]));
        }
    }
}

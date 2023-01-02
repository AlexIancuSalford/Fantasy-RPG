using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG.UI.Quest
{
    public class QuestStatus
    {
        private object objectState;

        public Quest Quest { get; private set; } = null;
        public List<string> CompletedObjective { get; private set; } = new List<string>();

        public QuestStatus(Quest quest)
        {
            Quest = quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = (QuestStatusRecord)objectState;
            Quest = Quest.GetQuestByName(state.QuestName);
            CompletedObjective = state.CompletedObjectives;
        }

        public void CompleteObjective(string objective)
        {
            if (Quest.HasObjective(objective))
            {
                CompletedObjective.Add(objective);
            }
        }

        public object SaveState()
        {
            QuestStatusRecord state = new QuestStatusRecord
            {
                QuestName = Quest.name,
                CompletedObjectives = CompletedObjective
            };

            return state;
        }

        public bool IsQuestComplete()
        {
            return Quest.Objectives.All(objective => CompletedObjective.Contains(objective.Reference));
        }
    }
}

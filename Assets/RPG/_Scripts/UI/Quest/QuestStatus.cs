using System;
using System.Collections.Generic;

namespace RPG.UI.Quest
{
    public class QuestStatus
    {
        public Quest Quest { get; private set; } = null;
        public List<string> CompletedObjective { get; private set; } = new List<string>();

        public QuestStatus(Quest quest)
        {
            Quest = quest;
        }

        public void CompleteObjective(string objective)
        {
            if (Quest.HasObjective(objective))
            {
                CompletedObjective.Add(objective);
            }
        }
    }
}

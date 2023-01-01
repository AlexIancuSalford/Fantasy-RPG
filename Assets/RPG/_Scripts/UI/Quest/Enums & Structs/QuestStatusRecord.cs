using System.Collections.Generic;

namespace RPG.UI.Quest
{
    [System.Serializable]
    public struct QuestStatusRecord
    {
        public string QuestName;
        public List<string> CompletedObjectives;
    }
}

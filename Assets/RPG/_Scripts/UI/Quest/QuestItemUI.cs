using TMPro;
using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestItemUI : MonoBehaviour
    {
        [field : SerializeField] private TextMeshProUGUI QuestTitle { get; set; } = null;
        [field: SerializeField] private TextMeshProUGUI QuestProgress { get; set; } = null;

        public QuestStatus QuestStatus { get; private set; } = null;

        public void SetupQuest(QuestStatus item)
        {
            QuestStatus = item;
            QuestTitle.text = item.Quest.GetQuestTitle();
            QuestProgress.text = $"{item.CompletedObjective.Count}/{item.Quest.GetObjectiveCount()}";
        }
    }
}

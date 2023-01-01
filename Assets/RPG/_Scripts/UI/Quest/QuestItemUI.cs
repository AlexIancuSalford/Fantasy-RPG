using TMPro;
using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestItemUI : MonoBehaviour
    {
        [field : SerializeField] private TextMeshProUGUI QuestTitle { get; set; } = null;
        [field: SerializeField] private TextMeshProUGUI QuestProgress { get; set; } = null;

        public Quest Quest { get; private set; } = null;

        public void SetupQuest(Quest item)
        {
            Quest = item;
            QuestTitle.text = item.GetQuestTitle();
            QuestProgress.text = $"0/{item.GetObjectiveCount()}";
        }
    }
}

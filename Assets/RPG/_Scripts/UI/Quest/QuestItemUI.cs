using TMPro;
using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestItemUI : MonoBehaviour
    {
        [field : SerializeField] private TextMeshProUGUI QuestTitle { get; set; } = null;
        [field: SerializeField] private TextMeshProUGUI QuestProgress { get; set; } = null;

        public void SetupQuest(Quest item)
        {
            QuestTitle.text = item.GetQuestTitle();
            QuestProgress.text = $"0/{item.GetObjectiveCount()}";
        }
    }
}

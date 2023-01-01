using TMPro;
using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [field : SerializeField] private TextMeshProUGUI Title { get; set; } = null;
        [field: SerializeField] private Transform ObjectiveContainer { get; set; } = null;
        [field: SerializeField] private GameObject Objective { get; set; } = null;
        [field: SerializeField] private GameObject ObjectiveIncomplete { get; set; } = null;

        public void SetupTooltip(QuestStatus item)
        {
            Title.text = item.Quest.GetQuestTitle();

            foreach (Transform obj in ObjectiveContainer)
            {
                Destroy(obj.gameObject);
            }

            foreach (string objective in item.Quest.Objectives)
            {
                GameObject instance = Instantiate(
                    item.CompletedObjective.Contains(objective) ? Objective : ObjectiveIncomplete
                    , ObjectiveContainer);

                instance.GetComponentInChildren<TextMeshProUGUI>().text = objective;
            }
        }
    }

}
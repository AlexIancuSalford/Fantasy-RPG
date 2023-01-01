using TMPro;
using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [field : SerializeField] private TextMeshProUGUI Title { get; set; } = null;
        [field: SerializeField] private Transform ObjectiveContainer { get; set; } = null;
        [field: SerializeField] private GameObject Objective { get; set; } = null;

        public void SetupTooltip(Quest item)
        {
            Title.text = item.GetQuestTitle();

            foreach (Transform obj in ObjectiveContainer)
            {
                Destroy(obj.gameObject);
            }

            foreach (string objective in item.Objectives)
            {
                GameObject instance = Instantiate(Objective, ObjectiveContainer);
                instance.GetComponentInChildren<TextMeshProUGUI>().text = objective;
            }
        }
    }

}
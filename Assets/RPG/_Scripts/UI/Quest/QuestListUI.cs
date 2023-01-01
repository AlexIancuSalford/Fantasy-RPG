using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestListUI : MonoBehaviour
    {
        [field : SerializeField] private QuestItemUI QuestItemUI { get; set; } = null;

        private QuestList QuestList { get; set; } = null;

        private void Awake()
        {
            QuestList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        }

        // Start is called before the first frame update
        void Start()
        {
            QuestList.QuestStatusChanged += UpdateQuestList;
            UpdateQuestList();
        }

        private void UpdateQuestList()
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestStatus questStatus in QuestList.QuestStatuses)
            {
                Instantiate<QuestItemUI>(QuestItemUI, transform).SetupQuest(questStatus);
            }
        }
    }
}

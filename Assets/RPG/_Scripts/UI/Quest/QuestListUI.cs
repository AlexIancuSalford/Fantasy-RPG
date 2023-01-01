using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestListUI : MonoBehaviour
    {
        [field : SerializeField] private QuestItemUI QuestItemUI { get; set; } = null;

        // Start is called before the first frame update
        void Start()
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            foreach (QuestStatus questStatus in questList.QuestStatuses)
            {
                Instantiate<QuestItemUI>(QuestItemUI, transform).SetupQuest(questStatus);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

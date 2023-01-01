using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestListUI : MonoBehaviour
    {
        [field : SerializeField] private List<Quest> TmpList { get; set; } = new List<Quest>();
        [field : SerializeField] private QuestItemUI QuestItemUI { get; set; } = null;

        // Start is called before the first frame update
        void Start()
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Quest quest in TmpList)
            {
                Instantiate<QuestItemUI>(QuestItemUI, transform).SetupQuest(quest);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

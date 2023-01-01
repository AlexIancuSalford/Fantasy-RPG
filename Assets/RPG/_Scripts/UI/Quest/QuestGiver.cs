using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestGiver : MonoBehaviour
    {
        [field : SerializeField] private Quest Quest { get; set; } = null;

        public void GiveQuest()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>().AddQuest(Quest);
        }
    }
}

using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestCompletion : MonoBehaviour
    {
        [field : SerializeField] private Quest Quest { get; set; } = null;
        [field: SerializeField] private string Objective { get; set; } = null;

        public void CompleteObjective()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>().CompleteObjective(Quest, Objective);
        }
    }
}

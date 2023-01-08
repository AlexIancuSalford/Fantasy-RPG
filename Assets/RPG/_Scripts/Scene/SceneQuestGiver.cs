using RPG.Save;
using RPG.UI.Quest;
using UnityEngine;

namespace RPG.Scene
{
    /// <summary>
    /// This script is meant to give the player a quest when the collider is entered
    /// </summary>
    public class SceneQuestGiver : MonoBehaviour, ISaveableEntity
    {
        private bool isQuestGiven = false;

        private void OnTriggerEnter(Collider other)
        {
            if (isQuestGiven) { return; }

            GetComponent<QuestGiver>().GiveQuest();
            isQuestGiven = true;
        }

        public object SaveState()
        {
            return isQuestGiven;
        }

        public void LoadState(object obj)
        {
            isQuestGiven = (bool)obj;
        }
    }
}

using RPG.Save;
using RPG.UI.Inventory;
using UnityEngine;

namespace RPG.Cutscene
{
    public class TriggerAnimation : MonoBehaviour, ISaveableEntity
    {
        [field : SerializeField] private InventoryItem RequiredItem { get; set; } = null;

        private bool wasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (wasTriggered) { return; }

            GameObject player = GameObject.FindWithTag("Player");

            if (player != null && player.GetComponent<Inventory>().HasItem(RequiredItem))
            {
                GetComponent<Animator>().Play("GateAnimation", 0, 0);
                wasTriggered = true;
            }
        }

        public object SaveState()
        {
            return wasTriggered;
        }

        public void LoadState(object obj)
        {
            wasTriggered = (bool)obj;
        }
    }
}

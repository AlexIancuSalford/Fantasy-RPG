using UnityEngine;
using UnityEngine.Events;
using static RPG.Dialogue.DialogueEnums;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [field : SerializeField] private DialogueAction Action { get; set; } = DialogueAction.None;
        [field : SerializeField] private UnityEvent OnTrigger { get; set; } = null;

        public void Trigger(DialogueAction action)
        {
            if (action == Action)
            {
                OnTrigger.Invoke();
            }
        }
    }
}

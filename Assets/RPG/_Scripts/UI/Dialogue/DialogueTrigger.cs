using UnityEngine;
using UnityEngine.Events;
using static RPG.Dialogue.DialogueEnums;

namespace RPG.Dialogue
{
    /// <summary>
    /// The DialogueTrigger script is a simple script that allows you to set a specific DialogueAction and an associated
    /// UnityEvent that will be triggered when the Trigger function is called with the same DialogueAction as the one set
    /// in the script.
    /// 
    /// The DialogueTrigger script has two serialized fields:
    /// 
    /// Action: This is of type DialogueAction and is used to specify which DialogueAction will trigger the OnTrigger event.
    /// 
    /// OnTrigger: This is of type UnityEvent and represents the event that will be triggered when the Trigger function is
    /// called with the same DialogueAction as the one set in the Action field.
    /// 
    /// The Trigger function takes in a DialogueAction parameter and checks if it matches the Action field. If it does,
    /// it invokes the OnTrigger event.
    /// 
    /// DialogueEnums is a static class that contains enumerations related to dialogue, and DialogueAction is one of those
    /// enumerations. MonoBehaviour is the base class for all Unity scripts, and the using static RPG.Dialogue.DialogueEnums;
    /// directive allows you to use the DialogueAction enumeration without having to specify the class it belongs to
    /// (i.e., you can just use DialogueAction instead of DialogueEnums.DialogueAction).
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        /// <summary>
        /// The `DialogueAction` that will trigger the `OnTrigger` event.
        /// </summary>
        [field : SerializeField] private DialogueAction Action { get; set; } = DialogueAction.None;

        /// <summary>
        /// The event that will be triggered when the `Trigger` function is called with the same
        /// `DialogueAction` as the one set in the `Action` field.
        /// </summary>
        [field : SerializeField] private UnityEvent OnTrigger { get; set; } = null;

        /// <summary>
        /// Triggers the `OnTrigger` event if the specified `action` matches the `Action` field.
        /// </summary>
        /// <param name="action">The `DialogueAction` to compare to the `Action` field.</param>
        public void Trigger(DialogueAction action)
        {
            // If the action to trigger is the right action, invoke the event
            if (action == Action)
            {
                OnTrigger.Invoke();
            }
        }
    }
}

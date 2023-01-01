using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RPG.Dialogue.DialogueEnums;
using Random = UnityEngine.Random;

namespace RPG.Dialogue
{
    /// <summary>
    /// This script is used for managing a conversation between two game objects (in this case a player and an NPC). The conversation is
    /// represented as a tree of nodes, where each node represents a piece of dialogue and has a list of child nodes representing the
    /// possible responses to that piece of dialogue.
    /// 
    /// The script has a number of fields, including a CurrentDialogue field that holds the current state of the conversation, a
    /// CurrentDialogueNode field that holds the current node of the conversation, and an AIConversation field that holds a reference to
    /// the AI game object involved in the conversation.
    /// 
    /// The script also has a number of methods that can be called to manipulate the conversation. For example, the Next method advances
    /// the conversation to the next node, the StartDialogue method starts a new conversation, and the QuitDialogue method ends the current
    /// conversation. There are also methods for getting the text of the current node, getting the possible responses to the current node,
    /// and selecting a particular response.
    /// 
    /// There are also a number of private methods in the script that are used to trigger actions when a node is entered or exited, and to
    /// trigger actions based on the value of a DialogueAction field.
    /// </summary>
    public class Conversation : MonoBehaviour
    {
        /// <summary>
        /// The name of the player.
        /// </summary>
        [field : SerializeField] private string PlayerName { get; set; } = string.Empty;

        /// <summary>
        /// The current dialogue being displayed.
        /// </summary>
        private Dialogue CurrentDialogue { get; set; } = null;

        /// <summary>
        /// The current node being displayed in the dialogue.
        /// </summary>
        private Node CurrentDialogueNode { get; set; } = null;

        /// <summary>
        /// The AI character's conversation object.
        /// </summary>
        private AIConversation AIConversation { get; set; } = null;

        /// <summary>
        /// Whether the player is currently choosing a response.
        /// </summary>
        public bool IsChoosing { get; private set; } = false;

        /// <summary>
        /// Event that is triggered when the conversation is updated.
        /// </summary>
        public event Action onConversationUpdated;

        /// <summary>
        /// Returns the text of the current node.
        /// </summary>
        public string GetNodeText()
        {
            return CurrentDialogueNode == null ? string.Empty : CurrentDialogueNode.Text;
        }

        /// <summary>
        /// Advances the conversation to the next node.
        /// </summary>
        public void Next()
        {
            // If the current node has player responses, set IsChoosing to true and trigger the OnExitAction.
            if (CurrentDialogue.GetPlayerNodeChildren(CurrentDialogueNode).Any())
            {
                // Since the player is choosing, set bool flag to true
                IsChoosing = true;
                // After the player chooses the response, trigger the on exit for the node
                TriggerExitAction();
                // Invoke the event since the a choice has been made by the player
                onConversationUpdated?.Invoke();
                return;
            }

            // Otherwise, select a random child node and advance to it.
            Node[] children = CurrentDialogue.GetAINodeChildren(CurrentDialogueNode).ToArray();
            // Trigger the exit action from previous node
            TriggerExitAction();
            // Set the current dialogue node the a next one chose at random
            CurrentDialogueNode = children[Random.Range(0, children.Length)];
            // Trigger the new nodes' enter action
            TriggerEnterAction();
            // Call since the conversation changed
            onConversationUpdated?.Invoke();
        }

        /// <summary>
        /// Returns whether the current node has any children.
        /// </summary>
        public bool NodeHasNext()
        {
            return CurrentDialogue.GetAllNodeChildren(CurrentDialogueNode).Any();
        }

        /// <summary>
        /// Returns a list of choices for the player to select from.
        /// </summary>
        public IEnumerable<Node> GetChoices()
        {
            return CurrentDialogue.GetAllNodeChildren(CurrentDialogueNode);
        }

        /// <summary>
        /// Selects a choice for the player and advances the conversation to the next node.
        /// </summary>
        public void SelectChoice(Node node)
        {
            CurrentDialogueNode = node;
            TriggerEnterAction();
            IsChoosing = false;
            Next();
        }

        /// <summary>
        /// Starts a new dialogue with the specified AIConversation and Dialogue objects.
        /// </summary>
        /// <param name="newAIConversation">The AIConversation object for the AI character.</param>
        /// <param name="newDialogue">The Dialogue object to use for the conversation.</param>
        public void StartDialogue(AIConversation newAIConversation, Dialogue newDialogue)
        {
            AIConversation = newAIConversation;
            CurrentDialogue = newDialogue;
            CurrentDialogueNode = CurrentDialogue.Nodes[0];
            TriggerEnterAction();
            onConversationUpdated?.Invoke();
        }

        /// <summary>
        /// Returns whether a conversation is currently active.
        /// </summary>
        public bool IsActive()
        {
            return CurrentDialogue != null;
        }

        /// <summary>
        /// Quits the current dialogue.
        /// </summary>
        public void QuitDialogue()
        {
            // Set everything to null and call the nodes' exit action and notify that the dialogue has changed
            CurrentDialogue = null;
            TriggerExitAction();
            CurrentDialogueNode = null;
            IsChoosing = false;
            AIConversation = null;
            onConversationUpdated?.Invoke();
        }

        /// <summary>
        /// Triggers the OnEnterAction for the current node.
        /// </summary>
        private void TriggerEnterAction()
        {
            if (CurrentDialogueNode != null)
            {
                TriggerAction(CurrentDialogueNode.OnEnterAction);
            }
        }

        /// <summary>
        /// Triggers the OnExitAction for the current node.
        /// </summary>
        private void TriggerExitAction()
        {
            if (CurrentDialogueNode != null)
            {
                TriggerAction(CurrentDialogueNode.OnExitAction);
            }
        }

        /// <summary>
        /// Triggers a DialogueAction for the current AIConversation.
        /// </summary>
        /// <param name="action">The action to trigger.</param>
        private void TriggerAction(DialogueAction action)
        {
            if (action == DialogueAction.None) { return; }

            foreach (DialogueTrigger trigger in AIConversation.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }

        /// <summary>
        /// Returns the name of the game object speaking, either the player or the AI character.
        /// </summary>
        public string GetGameObjectName()
        {
            return IsChoosing ? PlayerName : AIConversation.Name;
        }
    }
}
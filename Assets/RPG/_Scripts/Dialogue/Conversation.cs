using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static RPG.Dialogue.DialogueEnums;
using Random = UnityEngine.Random;

namespace RPG.Dialogue
{
    public class Conversation : MonoBehaviour
    {
        private Dialogue CurrentDialogue { get; set; } = null;
        private Node CurrentDialogueNode { get; set; } = null;
        private AIConversation AIConversation { get; set; } = null; 
        public bool IsChoosing { get; private set; } = false;

        public event Action onConversationUpdated;

        // Update is called once per frame
        void Update()
        {

        }

        public string GetNodeText()
        {
            return CurrentDialogueNode == null ? string.Empty : CurrentDialogueNode.Text;
        }

        public void Next()
        {
            if (CurrentDialogue.GetPlayerNodeChildren(CurrentDialogueNode).Any())
            {
                IsChoosing = true;
                TriggerExitAction();
                onConversationUpdated?.Invoke();
                return;
            }

            Node[] children = CurrentDialogue.GetAINodeChildren(CurrentDialogueNode).ToArray();
            TriggerExitAction();
            CurrentDialogueNode = children[Random.Range(0, children.Length)];
            TriggerEnterAction();
            onConversationUpdated?.Invoke();
        }

        public bool NodeHasNext()
        {
            return CurrentDialogue.GetAllNodeChildren(CurrentDialogueNode).Any();
        }

        public IEnumerable<Node> GetChoices()
        {
            return CurrentDialogue.GetAllNodeChildren(CurrentDialogueNode);
        }

        public void SelectChoice(Node node)
        {
            CurrentDialogueNode = node;
            TriggerEnterAction();
            IsChoosing = false;
            Next();
        }

        public void StartDialogue(AIConversation newAiConversation, Dialogue newDialogue)
        {
            AIConversation = newAiConversation;
            CurrentDialogue = newDialogue;
            CurrentDialogueNode = CurrentDialogue.Nodes[0];
            TriggerEnterAction();
            onConversationUpdated?.Invoke();
        }

        public bool IsActive()
        {
            return CurrentDialogue != null;
        }

        public void QuitDialogue()
        {
            CurrentDialogue = null;
            TriggerExitAction();
            CurrentDialogueNode = null;
            IsChoosing = false;
            AIConversation = null;
            onConversationUpdated?.Invoke();
        }

        private void TriggerEnterAction()
        {
            if (CurrentDialogueNode != null)
            {
                TriggerAction(CurrentDialogueNode.OnEnterAction);
            }
        }

        private void TriggerExitAction()
        {
            if (CurrentDialogueNode != null)
            {
                TriggerAction(CurrentDialogueNode.OnExitAction);
            }
        }

        private void TriggerAction(DialogueAction action)
        {
            if (action == DialogueAction.None) { return; }

            foreach (DialogueTrigger trigger in AIConversation.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Dialogue
{
    public class Conversation : MonoBehaviour
    {
        private Dialogue CurrentDialogue { get; set; } = null;
        private Node CurrentDialogueNode { get; set; } = null;
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
                onConversationUpdated?.Invoke();
                return;
            }

            Node[] children = CurrentDialogue.GetAINodeChildren(CurrentDialogueNode).ToArray();
            CurrentDialogueNode = children[Random.Range(0, children.Length)];

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
            IsChoosing = false;
            Next();
        }

        public void StartDialogue(Dialogue newDialogue)
        {
            CurrentDialogue = newDialogue;
            CurrentDialogueNode = CurrentDialogue.Nodes[0];
            onConversationUpdated?.Invoke();
        }

        public bool IsActive()
        {
            return CurrentDialogue != null;
        }

        public void QuitDialogue()
        {
            CurrentDialogue = null;
            CurrentDialogueNode = null;
            IsChoosing = false;
            onConversationUpdated?.Invoke();
        }
    }
}

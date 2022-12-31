using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class Conversation : MonoBehaviour
    {
        [field : SerializeField] private Dialogue CurrentDialogue { get; set; } = null;

        private Node CurrentDialogueNode { get; set; } = null;
        public bool IsChoosing { get; private set; } = false;

        private void Awake()
        {
            CurrentDialogueNode = CurrentDialogue.Nodes[0];
        }

        // Start is called before the first frame update
        void Start()
        {

        }

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
                return;
            }

            Node[] children = CurrentDialogue.GetAINodeChildren(CurrentDialogueNode).ToArray();
            CurrentDialogueNode = children[Random.Range(0, children.Length)];
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
    }
}

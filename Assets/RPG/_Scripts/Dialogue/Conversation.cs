using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class Conversation : MonoBehaviour
    {
        [field : SerializeField] private Dialogue CurrentDialogue { get; set; } 

        private Node CurrentDialogueNode { get; set; } = null;

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
            Node[] children = CurrentDialogue.GetAllNodeChildren(CurrentDialogueNode).ToArray();
            CurrentDialogueNode = children[Random.Range(0, children.Length)];
        }

        public bool NodeHasNext()
        {
            return CurrentDialogue.GetAllNodeChildren(CurrentDialogueNode).Any();
        }
    }
}

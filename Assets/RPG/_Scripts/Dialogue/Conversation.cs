using UnityEngine;

namespace RPG.Dialogue
{
    public class Conversation : MonoBehaviour
    {
        [field : SerializeField] private Dialogue CurrentDialogue { get; set; } 

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
            return CurrentDialogue == null ? string.Empty : CurrentDialogue.Nodes[0].Text;
        }
    }
}

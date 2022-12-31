using RPG.Dialogue;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        private Conversation Conversation { get; set; } =  null;
        [field : SerializeField] private TextMeshProUGUI AIText { get; set; } = null;
        [field : SerializeField] private TextMeshProUGUI SpeakerName { get; set; } = null;

        private void Awake()
        {
            Conversation = GameObject.FindGameObjectWithTag("Player").GetComponent<Conversation>();
        }

        // Start is called before the first frame update
        void Start()
        {
            AIText.text = Conversation.GetNodeText();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

using RPG.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        private Conversation Conversation { get; set; } =  null;
        
        [field : SerializeField] private TextMeshProUGUI AIText { get; set; } = null;
        [field : SerializeField] private TextMeshProUGUI SpeakerName { get; set; } = null;
        [field : SerializeField] private Button NextButton { get; set; } = null;
        [field: SerializeField] private Button QuitButton { get; set; } = null;
        [field: SerializeField] private Transform ChoiceRoot { get; set; } = null;
        [field: SerializeField] private GameObject ChoicePrefab { get; set; } = null;
        [field: SerializeField] private GameObject AIResponce { get; set; } = null;

        private void Awake()
        {
            Conversation = GameObject.FindGameObjectWithTag("Player").GetComponent<Conversation>();
        }

        // Start is called before the first frame update
        void Start()
        {
            Conversation.onConversationUpdated += UpdateUI;
            NextButton.onClick.AddListener(() => { Conversation.Next(); });
            QuitButton.onClick.AddListener(() => { Conversation.QuitDialogue(); });
            UpdateUI();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UpdateUI()
        {
            gameObject.SetActive(Conversation.IsActive());
            if (!Conversation.IsActive()) { return; }

            AIResponce.SetActive(!Conversation.IsChoosing);
            ChoiceRoot.gameObject.SetActive(Conversation.IsChoosing);

            if (Conversation.IsChoosing)
            {
                foreach (Transform item in ChoiceRoot)
                {
                    Destroy(item.gameObject);
                }

                foreach (Node choice in Conversation.GetChoices())
                {
                    GameObject instance = Instantiate(ChoicePrefab, ChoiceRoot);
                    instance.GetComponentInChildren<TextMeshProUGUI>().text = choice.Text;
                    instance.GetComponentInChildren<Button>().onClick.AddListener(() =>
                    {
                        Conversation.SelectChoice(choice);
                    });
                }
            }
            else
            {
                AIText.text = Conversation.GetNodeText();
                NextButton.gameObject.SetActive(Conversation.NodeHasNext());
            }
        }
    }
}

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
        [field: SerializeField] private Transform ChoiceRoot { get; set; } = null;
        [field: SerializeField] private GameObject ChoicePrefab { get; set; } = null;

        private void Awake()
        {
            Conversation = GameObject.FindGameObjectWithTag("Player").GetComponent<Conversation>();
        }

        // Start is called before the first frame update
        void Start()
        {
            NextButton.onClick.AddListener(Next);
            UpdateUI();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Next()
        {
            Conversation.Next();
            UpdateUI();
        }

        private void UpdateUI()
        {
            AIText.text = Conversation.GetNodeText();
            NextButton.gameObject.SetActive(Conversation.NodeHasNext());

            foreach (Transform item in ChoiceRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (string choice in Conversation.GetChoices())
            {
                GameObject instance = Instantiate(ChoicePrefab, ChoiceRoot);
                instance.GetComponentInChildren<TextMeshProUGUI>().text = choice;
            }
        }
    }
}

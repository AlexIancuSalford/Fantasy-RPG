using RPG.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    /// <summary>
    /// This script defines a Unity UI element that displays a conversation between a player character and an NPC. The conversation
    /// is managed by a Conversation component attached to the player character, and the UI element consists of a text field for the
    /// NPC's dialogue, a text field for the speaker's name, a button to advance to the next line of dialogue, and a button to exit
    /// the conversation.
    /// 
    /// The UI element also includes a ChoiceRoot transform which is used to display a list of choices to the player as a series of
    /// buttons. When the player selects a choice, the Conversation component is informed of the player's selection through the
    /// SelectChoice method.
    /// 
    /// The UpdateUI method is called to update the UI element whenever the onConversationUpdated event is raised by the Conversation
    /// component. This method enables or disables the UI element based on whether the conversation is active, and also updates the text
    /// fields and buttons based on the current state of the conversation.
    /// </summary>
    public class DialogueUI : MonoBehaviour
    {
        /// <summary>
        /// Reference to the Conversation component attached to the player character
        /// </summary>
        private Conversation Conversation { get; set; } =  null;

        /// <summary>
        /// Text field for displaying NPC dialogue
        /// </summary>
        [field : SerializeField] private TextMeshProUGUI AIText { get; set; } = null;

        /// <summary>
        /// Text field for displaying speaker's name
        /// </summary>
        [field : SerializeField] private TextMeshProUGUI SpeakerName { get; set; } = null;

        /// <summary>
        /// Button for advancing to the next line of dialogue
        /// </summary>
        [field : SerializeField] private Button NextButton { get; set; } = null;

        /// <summary>
        /// Button for exiting the conversation
        /// </summary>
        [field: SerializeField] private Button QuitButton { get; set; } = null;

        /// <summary>
        /// Transform for displaying a list of choices to the player
        /// </summary>
        [field: SerializeField] private Transform ChoiceRoot { get; set; } = null;

        /// <summary>
        /// Prefab for a choice button
        /// </summary>
        [field: SerializeField] private GameObject ChoicePrefab { get; set; } = null;

        /// <summary>
        /// GameObject for the AI's response
        /// </summary>
        [field: SerializeField] private GameObject AIResponce { get; set; } = null;

        private void Awake()
        {
            // Get reference to the Conversation component on the player character
            Conversation = GameObject.FindGameObjectWithTag("Player").GetComponent<Conversation>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Subscribe to the onConversationUpdated event to update the UI when the conversation changes
            Conversation.onConversationUpdated += UpdateUI;
            // Add a listener to the NextButton's onClick event to advance the conversation
            NextButton.onClick.AddListener(() => { Conversation.Next(); });
            // Add a listener to the QuitButton's onClick event to exit the conversation
            QuitButton.onClick.AddListener(() => { Conversation.QuitDialogue(); });
            // Initialize the UI
            UpdateUI();
        }

        /// <summary>
        /// Updates the UI element to reflect the current state of the conversation.
        /// </summary>
        private void UpdateUI()
        {
            // Enable or disable the UI element based on whether the conversation is active
            gameObject.SetActive(Conversation.IsActive());
            if (!Conversation.IsActive()) { return; }
            
            // Set the speaker's name
            SpeakerName.text = Conversation.GetGameObjectName();

            // Enable or disable the AI response based on whether the player is choosing a dialogue option
            AIResponce.SetActive(!Conversation.IsChoosing);
            // Enable or disable the choice root based on whether the player is choosing a dialogue option
            ChoiceRoot.gameObject.SetActive(Conversation.IsChoosing);

            if (Conversation.IsChoosing)
            {
                // Clear any existing choices
                foreach (Transform item in ChoiceRoot)
                {
                    Destroy(item.gameObject);
                }

                // Create a new button for each choice
                foreach (Node choice in Conversation.GetChoices())
                {
                    GameObject instance = Instantiate(ChoicePrefab, ChoiceRoot);
                    instance.GetComponentInChildren<TextMeshProUGUI>().text = choice.Text;
                    instance.GetComponentInChildren<Button>().onClick.AddListener(() =>
                    {
                        // Inform the Conversation component of the player's selection when a choice is clicked
                        Conversation.SelectChoice(choice);
                    });
                }
            }
            else
            {
                // Set the NPC's dialogue text
                AIText.text = Conversation.GetNodeText();
                // Enable or disable the Next button based on whether there is another line of dialogue to display
                NextButton.gameObject.SetActive(Conversation.NodeHasNext());
            }
        }
    }
}

using RPG.Helper;
using RPG.Scene;
using TMPro;
using UnityEngine;

namespace RPG.UI.Menu
{
    /// <summary>
    /// This script appears to be a script for a main menu in a Unity game. It has a serialized field for a TMP_InputField
    /// object called NewGameNameField, which is a Unity UI element for displaying and editing text. It also has two methods:
    /// ContinueGame and StartNewGame.
    /// 
    /// The Awake method is called when the script is first initialized, and it creates a new CDeferredValue object called
    /// saveWrapper that is initialized with a delegate to the GetSaveWrapper method. The CDeferredValue class is a custom
    /// class that appears to be used to delay the evaluation of a value until it is needed.
    /// 
    /// The ContinueGame method calls the ContinueGame method on the saveWrapper object's Value property, which returns the
    /// SaveWrapper object that was retrieved by the GetSaveWrapper method.
    /// 
    /// The StartNewGame method calls the StartNewGame method on the saveWrapper object's Value property, and passes it a
    /// string as an argument. If the NewGameNameField object's text property is empty or null, the string "save" is passed,
    /// otherwise the text property is passed.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [field : SerializeField] public TMP_InputField NewGameNameField { get; set; } = null;

        private CDeferredValue<SaveWrapper> saveWrapper;

        /// <summary>
        /// Initializes the saveWrapper field with a delegate to the GetSaveWrapper method.
        /// This method is called when the script is first initialized.
        /// </summary>
        private void Awake()
        {
            saveWrapper = new CDeferredValue<SaveWrapper>(GetSaveWrapper);
        }

        /// <summary>
        /// Returns the SaveWrapper object in the scene.
        /// </summary>
        /// <returns>The SaveWrapper object in the scene.</returns>
        private SaveWrapper GetSaveWrapper()
        {
            return FindObjectOfType<SaveWrapper>();
        }

        /// <summary>
        /// Continues the game by calling the ContinueGame method on the saveWrapper object's
        /// Value property.
        /// </summary>
        public void ContinueGame()
        {
            saveWrapper.Value.ContinueGame();
        }

        /// <summary>
        /// Starts a new game by calling the StartNewGame method on the saveWrapper object's
        /// Value property and passing it a string as an argument. If the NewGameNameField
        /// object's text property is empty or null, the string "save" is passed, otherwise
        /// the text property is passed.
        /// </summary>
        public void StartNewGame()
        {
            saveWrapper.Value.StartNewGame(string.IsNullOrEmpty(NewGameNameField.text) ? "save" : NewGameNameField.text);
        }
    }
}

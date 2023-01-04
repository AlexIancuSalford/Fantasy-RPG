using RPG.Helper;
using RPG.Scene;
using TMPro;
using UnityEngine;

namespace RPG.UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [field : SerializeField] public TMP_InputField NewGameNameField { get; set; } = null;

        private CDeferredValue<SaveWrapper> saveWrapper;

        private void Awake()
        {
            saveWrapper = new CDeferredValue<SaveWrapper>(GetSaveWrapper);
        }

        private SaveWrapper GetSaveWrapper()
        {
            return FindObjectOfType<SaveWrapper>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ContinueGame()
        {
            saveWrapper.Value.ContinueGame();
        }

        public void StartNewGame()
        {
            saveWrapper.Value.StartNewGame(string.IsNullOrEmpty(NewGameNameField.text) ? "save" : NewGameNameField.text);
        }
    }
}

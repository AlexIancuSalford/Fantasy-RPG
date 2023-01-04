using RPG.Helper;
using RPG.Scene;
using UnityEngine;

namespace RPG.UI.Menu
{
    public class MainMenu : MonoBehaviour
    {
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
    }
}

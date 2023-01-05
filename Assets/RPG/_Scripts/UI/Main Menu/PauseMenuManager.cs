using RPG.Controller;
using RPG.Scene;
using UnityEngine;

namespace RPG.UI.Menu
{
    public class PauseMenuManager : MonoBehaviour
    {
        private PlayerController PlayerController { get; set; } = null;

        private void Awake()
        {
            PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        
        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnEnable()
        {
            Time.timeScale = 0;
            PlayerController.enabled = false;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
            PlayerController.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Save()
        {
            GetSaveWrapper().Save();
        }

        public void SaveAndQuit()
        {
            SaveWrapper saveWrapper = GetSaveWrapper();
            saveWrapper.Save();
            saveWrapper.LoadMenu();
        }

        private SaveWrapper GetSaveWrapper()
        {
            return FindObjectOfType<SaveWrapper>();
        }
    }
}


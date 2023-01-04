using RPG.Save;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
    public class SaveWrapper : MonoBehaviour
    {
        private float _fadeInTime = 2f;

        [field : SerializeField] public int SceneToLoadIndex { get; private set; }

        private FadeEffect FadeEffect { get; set; }

        private void Awake()
        {
            FadeEffect = FindObjectOfType<FadeEffect>();
        }

        public void ContinueGame()
        {
            if (!PlayerPrefs.HasKey("currentSaveFile")) { return;}
            if (!GetComponent<SaveManager>().IsSaveFile(GetCurrentSaveFile())) { return; }
            StartCoroutine(LoadLastScene());
        }
        public void StartNewGame(string fileName)
        {
            SetCurrentSaveFile(fileName);
            StartCoroutine(LoadNewScene());
        }

        private IEnumerator LoadLastScene()
        {
            // Fade out the screen immediately when the game starts
            FadeEffect.FadeOutImmediately();
            // Load the last saved scene using the SaveManager component
            yield return GetComponent<SaveManager>().LoadLastScene(GetCurrentSaveFile());
            // Fade the screen back in
            yield return FadeEffect.FadeIn(_fadeInTime);
            // Load the saved game data again due to unidentified bug
            Load();
        }

        private IEnumerator LoadNewScene()
        {
            SceneToLoadIndex = SceneToLoadIndex == 0 ? SceneManager.GetActiveScene().buildIndex + 1 : SceneToLoadIndex;

            // Fade out the screen immediately when the game starts
            FadeEffect.FadeOutImmediately();
            // Load the last saved scene using the SaveManager component
            yield return SceneManager.LoadSceneAsync(SceneToLoadIndex);
            // Fade the screen back in
            yield return FadeEffect.FadeIn(_fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                Delete();
            }
        }

        /// <summary>
        /// Saves the game data using the SaveManager component and the default save file name.
        /// </summary>
        public void Save()
        {
            GetComponent<SaveManager>().Save(GetCurrentSaveFile());
        }

        /// <summary>
        /// Loads the game data using the SaveManager component and the default save file name.
        /// </summary>
        public void Load()
        {
            GetComponent<SaveManager>().Load(GetCurrentSaveFile());
        }

        /// <summary>
        /// Delete save file for debugging purposes.
        /// </summary>
        public void Delete()
        {
            GetComponent<SaveManager>().Delete(GetCurrentSaveFile());
        }

        private void SetCurrentSaveFile(string fileName)
        {
            PlayerPrefs.SetString("currentSaveFile", fileName);
        }

        private string GetCurrentSaveFile()
        {
            return PlayerPrefs.GetString("currentSaveFile");
        }
    }
}

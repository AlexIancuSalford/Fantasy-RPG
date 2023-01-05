using RPG.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
    public class SaveWrapper : MonoBehaviour
    {
        private const float FadeInTime = 2f;
        private const float FadeOutTime = 1f;

        [field : SerializeField] public int SceneToLoadIndex { get; private set; }
        [field : SerializeField] public int MainMenuIndex { get; private set; } = 0;

        private FadeEffect FadeEffect { get; set; }

        private void Awake()
        {
            FadeEffect = FindObjectOfType<FadeEffect>();
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

        public void LoadGame(string fileName)
        {
            SetCurrentSaveFile(fileName);
            StartCoroutine(LoadLastScene());
        }

        public void LoadMenu()
        {
            StartCoroutine(LoadMainMenu());
        }

        private IEnumerator LoadLastScene()
        {
            // Fade out the screen immediately when the game starts
            FadeEffect.FadeOutImmediately();
            // Load the last saved scene using the SaveManager component
            yield return GetComponent<SaveManager>().LoadLastScene(GetCurrentSaveFile());
            // Fade the screen back in
            yield return FadeEffect.FadeIn(FadeInTime);
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
            yield return FadeEffect.FadeIn(FadeInTime);
        }

        private IEnumerator LoadMainMenu()
        {
            // Fade out the screen when the loading starts
            yield return FadeEffect.FadeOut(1);
            // Load the last saved scene using the SaveManager component
            yield return SceneManager.LoadSceneAsync(MainMenuIndex);
            // Fade the screen back in
            yield return FadeEffect.FadeIn(FadeInTime);
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

        public IEnumerable<string> SaveList()
        {
            return GetComponent<SaveManager>().SaveList();
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

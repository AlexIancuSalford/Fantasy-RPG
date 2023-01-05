using RPG.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
    /// <summary>
    /// This script is a wrapper for saving and loading game data in a Unity game using the SaveManager component.
    /// It has several methods for starting a new game, continuing a previous game, loading a specific save file,
    /// and returning to the main menu. It also has methods for saving, loading, and deleting save data, and for
    /// listing the available save files. The script listens for the keys 'S', 'L', and 'F1' to be pressed and will
    /// save, load, or delete the current save file, respectively. The script also uses a FadeEffect component to fade
    /// the screen in and out when transitioning between scenes.
    /// </summary>
    public class SaveWrapper : MonoBehaviour
    {
        // Constants for fade in and fade out times
        private const float FadeInTime = 2f;
        private const float FadeOutTime = 1f;

        /// <summary>
        /// The index of the scene to load when starting a new game
        /// </summary>
        [field : SerializeField] public int SceneToLoadIndex { get; private set; }

        /// <summary>
        /// The index of the main menu scene
        /// </summary>
        [field : SerializeField] public int MainMenuIndex { get; private set; } = 0;

        /// <summary>
        /// A reference to the FadeEffect component in the scene
        /// </summary>
        private FadeEffect FadeEffect { get; set; }

        private void Awake()
        {
            // Get the FadeEffect component in the scene
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

        /// <summary>
        /// Attempts to continue the previous game.
        /// </summary>
        public void ContinueGame()
        {
            // Check if a current save file exists
            if (!PlayerPrefs.HasKey("currentSaveFile")) { return;}
            // Check if the save file is valid
            if (!GetComponent<SaveManager>().IsSaveFile(GetCurrentSaveFile())) { return; }
            // Start a coroutine to load the last saved scene
            StartCoroutine(LoadLastScene());
        }

        /// <summary>
        /// Starts a new game with the specified save file name.
        /// </summary>
        /// <param name="fileName">The name of the save file to use for the new game.</param>
        public void StartNewGame(string fileName)
        {
            // Set the current save file name
            SetCurrentSaveFile(fileName);
            // Start a coroutine to load the new scene
            StartCoroutine(LoadNewScene());
        }

        /// <summary>
        /// Loads a specific game save file.
        /// </summary>
        /// <param name="fileName">The name of the save file to load.</param>
        public void LoadGame(string fileName)
        {
            // Set the current save file name
            SetCurrentSaveFile(fileName);
            // Start a coroutine to load the last saved scene
            StartCoroutine(LoadLastScene());
        }

        /// <summary>
        /// Loads the main menu scene.
        /// </summary>
        public void LoadMenu()
        {
            // Start a coroutine to load the main menu scene
            StartCoroutine(LoadMainMenu());
        }

        /// <summary>
        /// A coroutine that loads the last saved scene and fades the screen in afterwards.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// A coroutine that loads a new scene and fades the screen in afterwards.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadNewScene()
        {
            // If the SceneToLoadIndex is not set, use the next scene in the build index
            SceneToLoadIndex = SceneToLoadIndex == 0 ? SceneManager.GetActiveScene().buildIndex + 1 : SceneToLoadIndex;

            // Fade out the screen immediately when the game starts
            FadeEffect.FadeOutImmediately();
            // Load the last saved scene using the SaveManager component
            yield return SceneManager.LoadSceneAsync(SceneToLoadIndex);
            // Fade the screen back in
            yield return FadeEffect.FadeIn(FadeInTime);
        }

        /// <summary>
        /// A coroutine that loads the main menu scene and fades the screen in afterwards.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadMainMenu()
        {
            // Fade out the screen when the loading starts
            yield return FadeEffect.FadeOut(FadeOutTime);
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

        /// <summary>
        /// Gets a list of available save files.
        /// </summary>
        /// <returns>An enumerable collection of save file names.</returns>
        public IEnumerable<string> SaveList()
        {
            // Return the list of save files using the SaveManager component
            return GetComponent<SaveManager>().SaveList();
        }

        /// <summary>
        /// Sets the current save file name.
        /// </summary>
        /// <param name="fileName">The name of the save file to use as the current save file.</param>
        private void SetCurrentSaveFile(string fileName)
        {
            // Set the "currentSaveFile" PlayerPref to the specified file name
            PlayerPrefs.SetString("currentSaveFile", fileName);
        }

        /// <summary>
        /// Gets the current save file name.
        /// </summary>
        /// <returns>The name of the current save file.</returns>
        private string GetCurrentSaveFile()
        {
            return PlayerPrefs.GetString("currentSaveFile");
        }
    }
}

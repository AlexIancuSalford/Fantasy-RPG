using System.Collections;
using RPG.Save;
using UnityEngine;

namespace RPG.Scene
{
    /// <summary>
    /// This script is for a SaveWrapper object in a Unity game that is
    /// used to handle saving and loading game data. The script has several
    /// functions that can be called to save or load the game data.
    ///  
    /// The "Awake" function is called when the object is initialized, and it
    /// finds an object in the scene with the "FadeEffect" script and stores it
    /// in the "FadeEffect" field.
    ///  
    /// The "Start" function is a coroutine that is called when the object is
    /// initialized. It immediately fades the screen out, loads the last saved
    /// scene using the "SaveManager" component attached to the object, fades the
    /// screen back in, and then loads the saved game data.
    ///  
    /// The "Update" function is called every frame, and it checks for input from
    /// the player to either save or load the game data.
    ///  
    /// The "Save" function saves the game data using the "SaveManager" component
    /// attached to the object and the default save file name. The "Load" function
    /// loads the game data using the "SaveManager" component and the default save
    /// file name.
    /// </summary>
    public class SaveWrapper : MonoBehaviour
    {
        public const string _defaultSaveFile = "save";
        private float _fadeInTime = 2f;

        private FadeEffect FadeEffect { get; set; }

        private void Awake()
        {
            FadeEffect = FindObjectOfType<FadeEffect>();
        }

        private IEnumerator Start()
        {
            // Fade out the screen immediately when the game starts
            FadeEffect.FadeOutImmediately();
            // Load the last saved scene using the SaveManager component
            yield return GetComponent<SaveManager>().LoadLastScene(_defaultSaveFile);
            // Fade the screen back in
            yield return FadeEffect.FadeIn(_fadeInTime);
            // Load the saved game data again due to unidentified bug
            Load();
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
            GetComponent<SaveManager>().Save(_defaultSaveFile);
        }

        /// <summary>
        /// Loads the game data using the SaveManager component and the default save file name.
        /// </summary>
        public void Load()
        {
            GetComponent<SaveManager>().Load(_defaultSaveFile);
        }

        /// <summary>
        /// Delete save file for debugging purposes.
        /// </summary>
        public void Delete()
        {
            GetComponent<SaveManager>().Delete(_defaultSaveFile);
        }
    }
}

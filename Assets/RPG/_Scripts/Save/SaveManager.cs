using RPG.Helper;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Save
{
    public class SaveManager : MonoBehaviour
    {
        public void Save(string fileName)
        {
            // Read the current game state from the file
            Dictionary<string, object> loadDictionary = CSerializer.ReadFromFile(fileName);

            // Save the current game state to the dictionary
            SaveState(loadDictionary);

            // Write the dictionary to the file
            CSerializer.WriteToFile(fileName, loadDictionary);
        }

        /// <summary>
        /// Loads the game state from the file with the given file name.
        /// </summary>
        /// <param name="fileName">The file name to load the game state from.</param>
        /// <returns>The game state dictionary.</returns>
        public Dictionary<string, object> Load(string fileName)
        {
            // Read the game state from the file and pass it to the LoadState method
            return LoadState(CSerializer.ReadFromFile(fileName));
        }

        /// <summary>
        /// Deserializes the game state from the given state object and applies it to the game objects.
        /// </summary>
        /// <param name="state">The game state object.</param>
        /// <returns>The game state dictionary.</returns>
        private Dictionary<string, object> LoadState(object state)
        {
            Dictionary<string, object> stateDictionary = (Dictionary<string, object>)state;

            // Iterate over all the SaveableEntity components in the game
            foreach (SaveableEntity entity in FindObjectsOfType<SaveableEntity>())
            {
                // Check if the dictionary contains data for the component
                if (stateDictionary.ContainsKey(entity.UUID))
                {
                    // Load the component's data from the dictionary
                    entity.LoadState(stateDictionary[entity.UUID]);
                }
            }

            return stateDictionary;
        }

        /// <summary>
        /// Serializes the current game state and stores it in the given dictionary.
        /// </summary>
        /// <param name="stateDictionary">The dictionary to store the game state in.</param>
        private void SaveState(Dictionary<string, object> stateDictionary)
        {
            // Iterate over all the SaveableEntity components in the game
            foreach (SaveableEntity entity in FindObjectsOfType<SaveableEntity>())
            {
                // Add the component's data to the dictionary using the component's UUID as the key
                stateDictionary[entity.UUID] = entity.SaveState();
            }

            // Add the build index of the current scene to the dictionary
            stateDictionary["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        /// <summary>
        /// Loads the last scene the player was in and applies the game state from the file with the given file name.
        /// </summary>
        /// <param name="fileName">The file name to load the game state from.</param>
        /// <returns>An enumerator for the coroutine.</returns>
        public IEnumerator LoadLastScene(string fileName)
        {
            // Read the game state from the file
            Dictionary<string, object> stateDictionary = CSerializer.ReadFromFile(fileName);

            // Check if the dictionary contains the build index of the last scene the player was in
            if (stateDictionary.ContainsKey("lastSceneBuildIndex"))
            {
                int lastSceneBuildIndex = (int)stateDictionary["lastSceneBuildIndex"];

                // Load the last scene if it is different from the current scene
                if (lastSceneBuildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(lastSceneBuildIndex);
                }
            }

            // Load the game state
            LoadState(stateDictionary);
        }

        /// <summary>
        /// Deletes the file passed in.
        /// </summary>
        /// <param name="fileName">The file to be deleted</param>
        public void Delete(string fileName)
        {
            File.Delete(CSerializer.GetPathFromFile(fileName));
        }

        /// <summary>
        /// This method checks if a save file exists already
        /// </summary>
        /// <param name="fileName">The save file name</param>
        /// <returns>True if the save file exists, false otherwise</returns>
        public bool IsSaveFile(string fileName)
        {
            return File.Exists(CSerializer.GetPathFromFile(fileName));
        }

        public IEnumerable<string> SaveList()
        {
            foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if (Path.GetExtension(path).Equals(".save"))
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }

            Directory.EnumerateFiles(Application.persistentDataPath);
        }
    }
}

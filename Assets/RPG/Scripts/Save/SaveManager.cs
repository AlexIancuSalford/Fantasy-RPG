/*
 * The SaveManager script is a Unity component that allows the game to be
 * saved and loaded. It has four main methods: Save, Load, LoadState, and
 * SaveState. It also has a coroutine method called LoadLastScene.
 *  
 * The Save method takes a file name as an argument and writes the current
 * game state to a file with that name using the CSerializer.WriteToFile
 * method. The game state is obtained by calling the SaveState method, which
 * iterates over all the SaveableEntity components in the game and adds their
 * serialized data to a dictionary using the component's UUID as the key.
 * The dictionary is then passed to the CSerializer.WriteToFile method to be
 * written to the file.
 *  
 * The Load method takes a file name as an argument and reads the game state
 * from the file using the CSerializer.ReadFromFile method. It then passes the
 * state to the LoadState method to be deserialized and applied to the game
 * objects. The LoadState method iterates over all the SaveableEntity
 * components in the game and checks if the state dictionary contains data
 * for that component. If it does, it calls the LoadState method on the
 * component, passing in the data from the dictionary as an argument.
 *  
 * The LoadLastScene coroutine method takes a file name as an argument and
 * reads the game state from the file using the CSerializer.ReadFromFile
 * method. It then checks if the dictionary contains a "lastSceneBuildIndex"
 * key, which represents the build index of the last scene the player was in.
 * If the build index is different from the current scene's build index, the
 * coroutine loads the last scene using the SceneManager.LoadSceneAsync method.
 * Once the scene is finished loading, it calls the LoadState method to apply
 * the game state to the game objects.
 */

using System.Collections;
using System.Collections.Generic;
using RPG.Helper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Save
{
    public class SaveManager : MonoBehaviour
    {
        // <summary>
        /// Saves the current game state to a file with the given file name.
        /// </summary>
        /// <param name="fileName">The file name to save the game state to.</param>
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
    }
}

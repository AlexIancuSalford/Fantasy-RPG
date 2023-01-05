using RPG.Helper;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Save
{
    /// <summary>
    /// This Unity script provides functionality for saving and loading a game's state. It allows you to save the
    /// current state of the game to a file, and to load a previously saved state from a file.
    /// 
    /// The SaveManager class is a MonoBehaviour, which means it can be attached to a GameObject in a Unity scene
    /// and will receive updates from the game engine. It has three public methods: Save, Load, and LoadLastScene.
    /// 
    /// The Save method writes the current state of the game to a file with the given file name. It does this by
    /// creating a dictionary of key-value pairs representing the game state, and then passing it to the WriteToFile
    /// method of the CSerializer class. The CSerializer class is responsible for converting the dictionary to a
    /// string and writing it to a file.
    /// 
    /// The Load method reads the game state from a file with the given file name and returns the game state as a
    /// dictionary of key-value pairs. It does this by calling the ReadFromFile method of the CSerializer class,
    /// which reads the file and converts the string back into a dictionary.
    /// 
    /// The LoadLastScene method loads the last scene the player was in and applies the game state from a file with
    /// the given file name. It does this by reading the game state from the file, and then checking if the dictionary
    /// contains the build index of the last scene the player was in. If it does, it loads the scene with that build
    /// index. It then calls the LoadState method, passing it the dictionary, which deserializes the game state and
    /// applies it to the game objects.
    /// 
    /// The SaveState and LoadState methods are responsible for serializing and deserializing the game state, respectively.
    /// They do this by iterating over all the SaveableEntity components in the game and calling the SaveState and LoadState
    /// methods on them, respectively. The SaveableEntity class is a component that can be attached to a GameObject to make
    /// it serializable. It has a UUID field, which is a unique identifier for the component, and SaveState and LoadState
    /// methods, which are responsible for serializing and deserializing the component's state.
    /// </summary>
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

        /// <summary>
        /// This method gets a list of files with the extension .save from the Unity Application.persistentDataPath folder
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> SaveList()
        {
            // Iterate through all the files in the Application.persistentDataPath directory
            foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                // Check if the file extension is .save
                if (Path.GetExtension(path).Equals(".save"))
                {
                    //If it is, yield return the file name without the .save extension
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }
    }
}

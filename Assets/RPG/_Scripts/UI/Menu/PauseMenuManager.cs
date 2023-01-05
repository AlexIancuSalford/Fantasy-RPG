using RPG.Controller;
using RPG.Scene;
using UnityEngine;

namespace RPG.UI.Menu
{
    /// <summary>
    /// This script defines a PauseMenuManager class in the RPG.UI.Menu namespace. The PauseMenuManager class is attached to a
    /// game object in the game and has several methods for managing the game's pause menu.
    /// 
    /// The Awake method is called when the object is created. In this case, it looks for a game object with the "Player" tag
    /// and gets the PlayerController component attached to it.
    /// 
    /// The OnEnable method is called when the object is enabled. In this case, it sets the game's timescale to 0 (pausing the game)
    /// and disables the PlayerController component.
    /// 
    /// The OnDisable method is called when the object is disabled. In this case, it sets the game's timescale to 1 (unpausing the game)
    /// and enables the PlayerController component.
    /// 
    /// The Save method gets the SaveWrapper component in the scene and calls its Save method to save the game.
    /// 
    /// The SaveAndQuit method gets the SaveWrapper component in the scene, calls its Save method to save the game, and then calls
    /// the LoadMenu method to load the main menu.
    /// 
    /// The GetSaveWrapper method gets the SaveWrapper component in the scene by using the FindObjectOfType method.
    /// </summary>
    public class PauseMenuManager : MonoBehaviour
    {
        /// <summary>
        /// A reference to the PlayerController component attached to the player game object.
        /// </summary>
        private PlayerController PlayerController { get; set; } = null;

        /// <summary>
        /// Finds the player game object with the "Player" tag and gets its PlayerController component.
        /// </summary>
        private void Awake()
        {
            PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        /// <summary>
        /// Pauses the game and disables the player's movement when the object is enabled.
        /// </summary>
        private void OnEnable()
        {
            if (PlayerController == null) { return; }

            Time.timeScale = 0;
            PlayerController.enabled = false;
        }

        /// <summary>
        /// Unpauses the game and enables the player's movement when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            if (PlayerController == null) { return; }

            Time.timeScale = 1;
            PlayerController.enabled = true;
        }

        /// <summary>
        /// Saves the game by calling the Save method of the SaveWrapper component in the scene.
        /// </summary>
        public void Save()
        {
            GetSaveWrapper().Save();
        }

        /// <summary>
        /// Saves the game and loads the main menu by calling the Save and LoadMenu methods of the SaveWrapper component in the scene.
        /// </summary>
        public void SaveAndQuit()
        {
            SaveWrapper saveWrapper = GetSaveWrapper();
            saveWrapper.Save();
            saveWrapper.LoadMenu();
        }

        /// <summary>
        /// Gets the SaveWrapper component in the scene.
        /// </summary>
        /// <returns>The SaveWrapper component in the scene.</returns>
        private SaveWrapper GetSaveWrapper()
        {
            return FindObjectOfType<SaveWrapper>();
        }
    }
}


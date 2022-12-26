using RPG.Save;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cutscene
{
    /// <summary>
    /// This script is a MonoBehaviour in Unity that triggers a cutscene when
    /// the player enters a certain area in the game. It also implements the
    /// ISaveableEntity interface to allow the script's state to be saved and
    /// loaded.
    ///  
    /// The script has a static boolean field called _isPlaying that is used to
    /// keep track of whether the cutscene is currently playing or not. It is set
    /// to true initially.
    ///  
    /// The OnTriggerEnter() method is called when the player collides with a
    /// trigger collider. If the cutscene is already playing or the player game
    /// object does not have the "Player" tag, the method returns without doing
    /// anything. Otherwise, it sets _isPlaying to false, and plays the cutscene
    /// using the Play() method of the PlayableDirector component on the same game
    /// object as this script.
    ///  
    /// The SaveState() method returns the current value of _isPlaying as an object.
    ///  
    /// The LoadState() method takes an object as an argument, and sets
    /// _isPlaying to the value of the object cast to a bool. This allows the
    /// script to restore its state when the game is loaded.
    /// </summary>
    public class TriggerCutscene : MonoBehaviour, ISaveableEntity
    {
        private static bool _isPlaying = true;

        /// <summary>
        /// Triggers the cutscene when the player enters the trigger collider.
        /// </summary>
        /// <param name="other">The collider of the object that entered the
        /// trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            // If the cutscene is already playing or the player game object does not have the "Player" tag, return without doing anything.
            if (_isPlaying != true || !other.CompareTag("Player")) { return; }

            // Set _isPlaying to false and play the cutscene.
            _isPlaying = false;
            GetComponent<PlayableDirector>().Play();
        }

        /// <summary>
        /// Returns the current value of _isPlaying as an object.
        /// </summary>
        /// <returns>The current value of _isPlaying.</returns>
        public object SaveState()
        {
            return _isPlaying;
        }

        /// <summary>
        /// Sets _isPlaying to the value of the object cast to a bool.
        /// </summary>
        /// <param name="obj">The object to be cast to a bool and set as the
        /// value of _isPlaying.</param>
        public void LoadState(object obj)
        {
            _isPlaying = (bool)obj;
        }
    }
}

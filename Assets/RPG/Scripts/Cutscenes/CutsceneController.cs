/*
 * This script is a MonoBehaviour in Unity that controls a cutscene in a game.
 * It uses the Unity Playables system to play and stop a cutscene.
 *  
 * The script has three private properties:
 *  
 * 1. PlayableDirector: a reference to a PlayableDirector component on the same
 * game object as this script.
 *
 * 2. Player: a reference to a GameObject in the game that represents the
 * player character.
 *
 * ActionManager: a reference to an ActionManager script on the player game object.
 * In the Start() method, the script gets references to the PlayableDirector
 * and Player game object, and sets up two event handlers: played and stopped.
 * These events are raised by the PlayableDirector when the cutscene starts
 * playing and stops playing, respectively.
 *  
 * The DisableControl() method is called when the cutscene starts playing,
 * and it disables the player's control by setting the enabled property of
 * the player's PlayerController script to false, and cancels any current
 * action by calling the CancelAction() method on the player's ActionManager
 * script.
 *  
 * The EnableControl() method is called when the cutscene stops playing, and
 * it re-enables the player's control by setting the enabled property of the
 * player's PlayerController script to true.
 *  
 * The GetPlayer() method returns the player game object if it has already
 * been set, or it uses the FindGameObjectWithTag() method to find the game
 * object in the scene with the "Player" tag if it has not been set yet.
 */

using RPG.Controller;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cutscene
{
    public class CutsceneController : MonoBehaviour
    {
        // A reference to the PlayableDirector component on the same game object as this script.
        private PlayableDirector PlayableDirector { get; set; }
        // A reference to the game object in the game that represents the player character.
        private GameObject Player { get; set; }

        private void Start()
        {
            // TODO: Move to Awake

            // Get a reference to the PlayableDirector component on this game object.
            PlayableDirector = GetComponent<PlayableDirector>();
            // Get a reference to the player game object.
            Player = GetPlayer();

            // Set up event handlers for the played and stopped events.
            PlayableDirector.played += DisableControl;
            PlayableDirector.stopped += EnableControl;
        }

        /// <summary>
        /// Disables the player's control by setting the enabled property of
        /// the player's PlayerController script to false,
        /// and cancels any current action by calling the CancelAction()
        /// method on the player's ActionManager script.
        /// </summary>
        /// <param name="playableDirector">The PlayableDirector that raised the played event.</param>
        private void DisableControl(PlayableDirector playableDirector)
        {
            // Cancel the player's current action.
            Player.GetComponent<ActionManager>().CancelAction();
            // Disable the player's control.
            Player.GetComponent<PlayerController>().enabled = false;
        }

        /// <summary>
        /// Re-enables the player's control by setting the enabled property
        /// of the player's PlayerController script to true.
        /// </summary>
        /// <param name="playableDirector">The PlayableDirector that raised the stopped
        /// event.</param>
        private void EnableControl(PlayableDirector playableDirector)
        {
            // Enable the player's control.
            Player.GetComponent<PlayerController>().enabled = true;
        }

        /// <summary>
        /// Returns the player game object if it has already been set, or it uses the FindGameObjectWithTag() method to find
        /// the game object in the scene with the "Player" tag if it has not been set yet.
        /// </summary>
        /// <returns>The player game object.</returns>
        private GameObject GetPlayer()
        {
            return Player == null ? GameObject.FindGameObjectWithTag("Player") : Player;
        }
    }
}

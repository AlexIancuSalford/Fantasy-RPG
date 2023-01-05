using Cinemachine;
using RPG.Attributes;
using RPG.Scene;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Controller
{
    /// <summary>
    /// This script is responsible for respawning the game object (presumably the player character) at a
    /// designated position when the player dies.
    /// 
    /// The script has several serialized fields that can be set in the Unity Editor:
    /// 
    /// RespawnPosition: a Transform component that represents the position where the player should be respawned
    ///
    /// RespawnDelay: the number of seconds to wait before respawning the player
    ///
    /// FadeTime: the number of seconds to fade out and fade in the screen
    ///
    /// HealthRegenPercentage: the percentage of the player's maximum health to restore when respawning
    ///
    /// The script also has a listener for the OnDie event of the Health component attached to the game object.
    /// When this event is triggered, the script starts a coroutine called RespawnRoutine.
    /// 
    /// The RespawnRoutine coroutine does the following:
    /// 
    /// Saves the game
    /// Waits for the RespawnDelay number of seconds
    /// Fades out the screen over FadeTime seconds
    /// Teleports the game object to the RespawnPosition
    /// Restores a percentage of the player's health
    /// Resets all enemy characters in the scene by calling their Reset method and restoring a percentage of their health
    /// Saves the game again
    /// Fades in the screen over FadeTime seconds
    /// Updates the active virtual camera in the scene to follow the respawned player character
    ///
    /// The script also has an Awake method that adds the RespawnPlayer method as a listener for the OnDie event of the Health component, and a Start method that checks if the player is already dead and respawns them if necessary.
    /// </summary>
    public class RespawnController : MonoBehaviour
    {
        /// <summary>
        /// The position where the game object should be respawned.
        /// </summary>
        [field : SerializeField] private Transform RespawnPosition { get; set; } = null;

        /// <summary>
        /// The number of seconds to wait before respawning the game object.
        /// </summary>
        [field: SerializeField] private float RespawnDelay { get; set; }

        /// <summary>
        /// The number of seconds to fade out and fade in the screen during the respawn process.
        /// </summary>
        [field: SerializeField] private float FadeTime { get; set; }

        /// <summary>
        /// The percentage of the game object's maximum health to restore when respawning.
        /// </summary>
        [field: SerializeField] private float HealthRegenPercentage { get; set; }

        private void Awake()
        {
            // Add the RespawnPlayer method as a listener for the OnDie event of the Health component
            GetComponent<Health>().OnDie.AddListener(RespawnPlayer);
        }

        // Start is called before the first frame update
        void Start()
        {
            // If the game object is already dead, start the respawn process
            if (GetComponent<Health>().IsDead())
            {
                RespawnPlayer();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Start the respawn process for the game object.
        /// </summary>
        private void RespawnPlayer()
        {
            StartCoroutine(RespawnRoutine());
        }

        // <summary>
        /// A coroutine that handles the respawn process for the game object.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RespawnRoutine()
        {
            // Find the FadeEffect and SaveWrapper components in the scene
            FadeEffect fadeEffect = FindObjectOfType<FadeEffect>();
            SaveWrapper saveWrapper = FindObjectOfType<SaveWrapper>();

            // Save the game. This is done so the player cannot quit the game just before or shortly after they died
            saveWrapper.Save();
            // Wait for the respawn delay
            yield return new WaitForSeconds(RespawnDelay);
            // Fade out the screen
            yield return fadeEffect.FadeOut(FadeTime);
            // Teleport the game object to the respawn position
            GetComponent<NavMeshAgent>().Warp(RespawnPosition.position);
            // Restore a percentage of the game object's health
            GetComponent<Health>().RegenHealth(HealthRegenPercentage);
            // Reset all enemy characters in the scene
            ResetEnemies();
            // Save the game again
            saveWrapper.Save();
            // Fade in the screen
            yield return fadeEffect.FadeIn(FadeTime);

            // Update the active virtual camera in the scene to follow the respawned game object
            ICinemachineCamera camera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if (camera.Follow == transform)
            {
                camera.OnTargetObjectWarped(transform, RespawnPosition.position - transform.position);
            }
        }

        /// <summary>
        /// Reset all enemy characters in the scene by calling their Reset method and restoring a percentage of their health.
        /// </summary>
        private void ResetEnemies()
        {
            // Iterate through all the game objects in the scene that have an AIController attached
            foreach (AIController enemy in FindObjectsOfType<AIController>())
            {
                // Call the reset method on all enemies
                enemy.Reset();
                // Restore 80% of the enemies' health. This will also make them not dead (respawn)
                enemy.GetComponent<Health>()?.RegenHealth(80);
            }
        }
    }
}

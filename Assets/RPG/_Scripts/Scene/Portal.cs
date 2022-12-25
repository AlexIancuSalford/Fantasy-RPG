using System.Collections;
using RPG.Controller;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
    /// <summary>
    /// This script is for a Portal object in a Unity game that is
    /// used to transport the player character to another scene or location within
    /// the game.When the player character collides with the Portal object
    /// (as determined by the "OnTriggerEnter" function), the script starts a
    /// coroutine called "LoadScene".
    /// 
    /// The "LoadScene" coroutine first finds an object in the scene with the
    /// "FadeEffect" script, and uses it to fade out the screen.It then saves
    /// the game state using a "SaveWrapper" object, loads the scene specified by
    /// the "SceneToLoad" field, and loads the saved game state.It then finds the
    /// other Portal object with the matching "DestinationPortal" field, and warps
    /// the player character to the position and rotation of its child object
    /// (which is likely meant to be the spawn point for the player character in
    /// the destination scene). The script then waits for the specified
    /// "FadeWaitTime" before fading the screen back in, and then destroys the
    /// Portal game object.
    /// 
    /// The script also includes several public fields that can be set in the Unity editor or via script:
    /// 
    /// "SceneToLoad": a string representing the name of the scene to load when
    /// the player character enters the Portal.
    /// 
    /// "DestinationPortal": an enumeration(enum) representing the destination
    /// portal the player character will be transported to.The enum has eight
    /// possible values: A, B, C, D, E, F, G, and H.The player character will be
    /// teleported to the corresponding opposite portal (e.g.Postal A will take the
    /// player character to the corresponding A portal in the selected scene)
    /// 
    /// "FadeOutTime": a float representing the time it takes for the screen to
    ///  * fade out, in seconds.
    /// 
    /// "FadeInTime": a float representing the time it takes for the screen to fade
    /// in, in seconds.
    /// 
    /// "FadeWaitTime": a float representing the time the script should wait before
    /// fading the screen back in, in seconds.
    /// 
    /// The script also includes two private fields:
    /// 
    /// "_spawnPoint": a Transform object representing the child object of the
    /// Portal, which is likely the spawn point for the player character in the
    /// destination scene.
    /// 
    /// "saveWrapper": a SaveWrapper object used to save and load the game state.
    ///  </summary>
    public class Portal : MonoBehaviour
    {
        public enum DestinationId
        {
            A, B, C, D, E, F, G, H
        }

        [field : SerializeField] public string SceneToLoad { get; set; }
        [field: SerializeField] public DestinationId DestinationPortal { get; set; }
        [field: SerializeField] public float FadeOutTime { get; set; } = 1f;
        [field: SerializeField] public float FadeInTime { get; set; } = 2f;
        [field: SerializeField] public float FadeWaitTime { get; set; } = .5f;

        private Transform _spawnPoint;

        private void Awake()
        {
            _spawnPoint = this.transform.GetChild(0);
        }

        /// <summary>
        /// Called when another collider enters the trigger collider attached to this object.
        /// If the collider is tagged as "Player", starts the LoadScene coroutine.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(LoadScene());
            }
        }

        /// <summary>
        /// Coroutine that handles the process of loading a new scene and transporting the player character.
        /// </summary>
        /// <returns>Yield instruction to wait for the coroutine to finish.</returns>
        private IEnumerator LoadScene()
        {
            // Find the FadeEffect object and SaveWrapper object in the scene
            FadeEffect fadeEffect = FindObjectOfType<FadeEffect>();
            SaveWrapper saveWrapper = FindObjectOfType<SaveWrapper>();

            // Make the Portal object persist between scene loads
            DontDestroyOnLoad(gameObject);

            // Grab the player and its PlayerController component so the player has the control disabled while the scene is loading
            // I honestly don't know why it took me so long to find this obvious bug
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            // Fade out the screen
            yield return fadeEffect.FadeOut(FadeOutTime);
            saveWrapper.Save(); // Save the game state
            yield return SceneManager.LoadSceneAsync(SceneToLoad); // Load the new scene

            // Grab the player and its PlayerController component so the player has the control disabled while the scene is loading
            // This has to be done again as this is not the same player as the previous scene.
            PlayerController playerControllerAgain = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerControllerAgain.enabled = false;

            saveWrapper.Load(); // Load the saved game state

            // Find the other Portal object with the matching DestinationPortal field
            Portal otherPortal = GetOtherPortal();
            // Find the player character game object in the scene
            GameObject player = GameObject.FindWithTag("Player");

            // Transport the player character to the position and rotation of the other Portal's spawn point
            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
            player.transform.rotation = otherPortal._spawnPoint.rotation;

            // Save the game state on the other side of the portal
            saveWrapper.Save();

            // Wait before fading the screen back in
            yield return new WaitForSeconds(FadeWaitTime);
            // Fade the screen back in
            yield return fadeEffect.FadeIn(FadeInTime);

            // Give control back to the player after the scene has finished loading
            playerControllerAgain.enabled = true;

            // Destroy the Portal object
            Destroy(gameObject);
        }

        /// <summary>
        /// Finds the other Portal object with the matching DestinationPortal field.
        /// </summary>
        /// <returns>The other Portal object, or null if none is found.</returns>
        private Portal GetOtherPortal()
        {
            // Find all Portal objects in the scene
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                // Skip the current Portal object
                if (portal == this) { continue; }
                // Skip Portal objects with a different DestinationPortal field
                if (portal.DestinationPortal != DestinationPortal) { continue; }

                // Return the matching Portal object
                return portal;
            }

            // Return null if no matching Portal object is found
            return null;
        }
    }
}

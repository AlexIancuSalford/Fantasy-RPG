using UnityEngine;

namespace RPG.Core
{
    /// <summary>
    /// This script is designed to be used in the Unity game engine and it is intended to make an object
    /// (like a character, NPC, or in this case the camera) always face the main camera in the scene.
    /// 
    /// The script has a single method called LateUpdate(), which is called once per frame in the game.
    /// The LateUpdate() method is called after all other updates have been processed, so it is often used
    /// for tasks that require the most up-to-date information.
    /// 
    /// Inside the LateUpdate() method, the script sets the forward direction of the object that the script is
    /// attached to (transform.forward) to be the same as the forward direction of the main camera in the scene
    /// (Camera.main.transform.forward). This causes the object to always face the camera, no matter where the
    /// camera is positioned or where it is looking.
    /// </summary>
    public class FaceCamera : MonoBehaviour
    {
        // Update is called once per frame
        void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}

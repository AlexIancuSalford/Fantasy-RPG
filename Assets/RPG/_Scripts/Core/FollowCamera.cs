using UnityEngine;

namespace RPG.Core
{
    /// <summary>
    /// This script is designed to be used in the Unity game engine and it is intended to make a camera follow a target
    /// (like a character, NPC, or in this case the camera) object in the scene.
    /// 
    /// The script has a single method called LateUpdate(), which is called once per frame in the game.
    /// The LateUpdate() method is called after all other updates have been processed,
    /// so it is often used for tasks that require the most up-to-date information.
    /// 
    /// Inside the LateUpdate() method, the script sets the position of the object that the script is
    /// attached to (transform.position) to be the same as the position of the target object (Target.position).
    /// This causes the camera to follow the target object, no matter where it moves.
    /// 
    /// The script also includes a public property called Target that is annotated with the SerializeField attribute.
    /// This attribute indicates that the Target property should be serialized by Unity, which means that it can be
    /// modified in the Unity editor and its value will be persisted between game sessions. The Target property is
    /// defined using the get; set; syntax, which means that it is a read-write property that can be accessed like a field.
    /// </summary>
    public class FollowCamera : MonoBehaviour
    {
        [field: SerializeField] public Transform Target { get; set; }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = Target.position;
        }
    }
}

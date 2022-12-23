/*
 * This is a script for a Unity game object that allows it to destroy itself
 * when a particle system that is attached to it has finished playing.
 *  
 * The script is written in C# and is attached to a game object in a Unity
 * project. It contains a single function called "Update" which is called
 * once per frame in the game.
 *  
 * The script starts by checking if the particle system attached to the game
 * object is still alive using the "IsAlive()" function. If the particle
 * system is no longer alive, the script will destroy the game object using
 * the "Destroy()" function. This will remove the game object from the game
 * and free up resources.
 *
 * If another game object is provided to the script by assigning a value to the
 * "targetGameObject" in the editor, the script will destroy that game object instead.
 * This is meant to be used in the case the user wants to destroy the parent
 * game object the particle effect sits on along with the particle effect.
 *  
 * This script is used to create temporary visual effects, such as
 * explosions or particle trails, that are only meant to be displayed for a
 * short period of time. Once the effect is finished playing, the script will
 * destroy the game object to clean up resources and avoid cluttering the game.
 */

using UnityEngine;

namespace RPG.Core
{
    public class DestroyEffect : MonoBehaviour
    {
        [field : SerializeField] private GameObject targetGameObject { get; set; } = null;

        // Update is called once per frame
        void Update()
        {
            // Check if the particle system is still alive
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                // If a parent is assigned, destroy that object, along with this one
                // If the particle system is no longer alive, destroy the game object
                Destroy(targetGameObject != null ? targetGameObject : gameObject);
            }
        }
    }
}

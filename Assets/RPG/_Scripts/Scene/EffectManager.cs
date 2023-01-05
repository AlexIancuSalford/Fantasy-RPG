using UnityEngine;

namespace RPG.Scene
{
    /// <summary>
    /// This script is responsible for spawning a persistent GameObject that will not be destroyed when changing
    /// scenes in the game. The hasSpawned field is used to ensure that the object is only spawned once, and the
    /// Awake() method is used to trigger the object spawning at the appropriate time.
    ///
    /// The class has the following features:
    /// 
    /// 1. It has a serialized field called persistentObject, which is a GameObject that will be instantiated and made
    /// persistent across scene changes. The [field: SerializeField] attribute indicates that this field will be serialized,
    /// meaning that it will be visible in the Inspector window in Unity and can be set using the Unity Editor.
    /// 
    /// 2. It has a private static field called hasSpawned, which is a boolean that is used to track whether the persistent
    /// object has already been instantiated.
    /// 
    /// 3. It has an Awake() method, which is a special method in Unity that is called when the component is enabled. In
    /// this method, the script checks if the persistent object has already been spawned by checking the value of hasSpawned.
    /// If the object has not been spawned, it calls the SpawnObjects() method.
    /// 
    /// 4. It has a SpawnObjects() method, which instantiates the persistentObject GameObject and makes it persistent
    /// across scene changes by calling DontDestroyOnLoad() on it. This method is only called if the persistent object
    /// has not already been spawned.
    /// </summary>
    public class EffectManager : MonoBehaviour
    {
        [field : SerializeField] private GameObject PersistentObject { get; set; }

        private static bool hasSpawned = false;

        private void Awake()
        {
            // Check if the persistent object has already been spawned
            if (hasSpawned) { return; }

            // Spawn the persistent object
            SpawnObjects();

            // Set the hasSpawned flag to true to indicate that the object has been spawned
            hasSpawned = true;
        }

        /// <summary>
        /// Instantiates and makes persistent the specified GameObject.
        /// </summary>
        private void SpawnObjects()
        {
            // Instantiate the persistent object
            GameObject persistentObj = Instantiate(PersistentObject);
            // Make the object persistent across scene changes
            DontDestroyOnLoad(persistentObj);
        }
    }
}

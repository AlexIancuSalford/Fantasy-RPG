using RPG.Save;
using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// The PickupSpawner script is a Unity script that is used to manage the spawning and destruction of
    /// pickup objects in a game. It has the following features:
    /// 
    /// It has a serialized InventoryItem field called Item and an int field called Number, which represent the
    /// item to be spawned and the number of instances to spawn, respectively.
    ///
    /// In the Awake function, it calls a SpawnPickup function to instantiate the pickup object.
    ///
    /// It has a GetPickup function that returns the Pickup component attached to a child object of the
    /// PickupSpawner game object.
    ///
    /// It has an isCollected function that returns true if the Pickup component is not found in a child object,
    /// and false otherwise.
    ///
    /// It has a SpawnPickup function that instantiates the pickup object and sets it as a child object of the
    /// PickupSpawner game object.
    ///
    /// It has a DestroyPickup function that destroys the pickup object if it exists.
    ///
    /// It implements the ISaveableEntity interface, which requires it to have SaveState and LoadState functions.
    /// The SaveState function returns a bool indicating whether the pickup object has been collected, and the
    /// LoadState function either destroys or spawns the pickup object based on this bool.
    /// </summary>
    public class PickupSpawner : MonoBehaviour, ISaveableEntity
    {
        /// <summary>
        /// The item to be spawned as a pickup.
        /// </summary>
        [field : SerializeField] private InventoryItem Item { get; set; } = null;

        /// <summary>
        /// The number of instances of the pickup to spawn.
        /// </summary>
        [field : SerializeField] private int Number { get; set; } = 1;

        private void Awake()
        {
            // Spawn in Awake so can be destroyed by save system after.
            SpawnPickup();
        }

        /// <summary>
        /// Returns the Pickup component attached to a child object of the PickupSpawner game object.
        /// </summary>
        /// <returns>The Pickup component attached to a child object of the PickupSpawner game object.</returns>
        public Pickup GetPickup() 
        { 
            return GetComponentInChildren<Pickup>();
        }

        /// <summary>
        /// Returns whether the Pickup component is attached to a child object of the PickupSpawner game object.
        /// </summary>
        /// <returns>True if the Pickup component is not found in a child object, and false otherwise.</returns>
        public bool IsCollected() 
        { 
            return GetPickup() == null;
        }

        /// <summary>
        /// Instantiates the pickup object and sets it as a child object of the PickupSpawner game object.
        /// </summary>
        private void SpawnPickup()
        {
            Pickup spawnedPickup = Item.SpawnPickup(transform.position, Number);
            spawnedPickup.transform.SetParent(transform);
        }

        /// <summary>
        /// Destroys the pickup object if it exists.
        /// </summary>
        private void DestroyPickup()
        {
            if (GetPickup())
            {
                Destroy(GetPickup().gameObject);
            }
        }

        /// <summary>
        /// Returns a bool indicating whether the pickup object has been collected.
        /// </summary>
        /// <returns>True if the Pickup component is not found in a child object, and false otherwise.</returns>
        public object SaveState()
        {
            return IsCollected();
        }

        /// <summary>
        /// Either destroys or spawns the pickup object based on the provided bool.
        /// </summary>
        /// <param name="state">A bool indicating whether the pickup object should be collected.</param>
        public void LoadState(object state)
        {
            bool shouldBeCollected = (bool)state;

            switch (shouldBeCollected)
            {
                case true when !IsCollected():
                    DestroyPickup();
                    break;
                case false when IsCollected():
                    SpawnPickup();
                    break;
            }
        }
    }
}
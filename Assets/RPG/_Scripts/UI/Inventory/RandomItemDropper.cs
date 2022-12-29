using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script defines a class called "RandomItemDropper" that is derived from another class called "ItemDropper".
    /// It is used to randomly drop items in a game using Unity's NavMesh system.
    /// 
    /// The class has several properties:
    /// 
    /// ScatterRadius: A float that represents the radius around the transform of the GameObject that this script is
    /// attached to within which random drop locations will be chosen.
    ///
    /// Attempts: An integer that represents the number of attempts that will be made to find a valid drop location using
    /// the NavMesh.
    ///
    /// DropLibrary: A reference to an ItemDropLibrary object that contains information about the items that can be dropped.
    ///
    /// NumberOfDrops: An integer that represents the number of items that will be dropped.
    ///
    /// The class also overrides a method called "GetDropLocation" from the base class "ItemDropper". This method attempts
    /// to find a valid location to drop an item within the ScatterRadius of the GameObject's transform, using the NavMesh
    /// to check if the location is valid. If a valid location is found, the method returns the position of the location.
    /// If no valid location is found after Attempts number of tries, the method returns the position of the transform.
    /// 
    /// Finally, the class has a method called "RandomDrop" which gets the current level of the GameObject that this script
    /// is attached to using the "BaseStats" component, and uses that level to retrieve a random set of items from the DropLibrary.
    /// It then iterates over the items and calls the "DropItem" method from the base class "ItemDropper" to drop each item at
    /// the location returned by the "GetDropLocation" method.
    /// </summary>
    public class RandomItemDropper : ItemDropper
    {
        /// <summary>
        /// The radius around the transform of the GameObject that this script is attached to within which random drop locations will be chosen.
        /// </summary>
        [field : SerializeField] private float ScatterRadius { get; set; } = 1.0f;

        /// <summary>
        /// The number of attempts that will be made to find a valid drop location using the NavMesh.
        /// </summary>
        [field : SerializeField] private int Attempts { get; set; } = 10;

        /// <summary>
        /// A reference to an ItemDropLibrary object that contains information about the items that can be dropped.
        /// </summary>
        [field : SerializeField] private ItemDropLibrary DropLibrary { get; set; } = null;

        /// <summary>
        /// The number of items that will be dropped.
        /// </summary>
        [field: SerializeField] private int NumberOfDrops { get; set; } = 2;

        /// <summary>
        /// Overrides the base method from ItemDropper to attempt to find a valid location to drop an item within
        /// the ScatterRadius of the GameObject's transform, using the NavMesh to check if the location is valid.
        ///
        /// If a valid location is found, the method returns the position of the location. If no valid location is
        /// found after Attempts number of tries, the method returns the position of the transform.
        /// </summary>
        /// <returns>The position of the drop location, or the position of the transform if no valid location is found.</returns>
        protected override Vector3 GetDropLocation()
        {
            // Iterate through the number of attempts and return at the first successful one
            for (int i = 0; i < Attempts; i++)
            {
                // Get a random point around the game objects' transform within the scatter radius
                Vector3 randomDropPoint = transform.position + Random.insideUnitSphere * ScatterRadius;
                // If the point is not on the navmesh, continue
                // This is so the item is not dropped in the air or underground
                if (!NavMesh.SamplePosition(randomDropPoint, out NavMeshHit hit, 0.1f, NavMesh.AllAreas)) { continue; }
                // Return the first successful random point that is on the navmesh
                return hit.position;
            }

            // If no attempt was successful, use the game objects' transform as the drop point
            return transform.position;
        }

        /// <summary>
        /// Gets the current level of the GameObject that this script is attached to using the "BaseStats" component,
        /// and uses that level to retrieve a random set of items from the DropLibrary.
        /// 
        /// It then iterates over the items and calls the "DropItem" method from the base class "ItemDropper" to drop
        /// each item at the location returned by the "GetDropLocation" method.
        /// </summary>
        public void RandomDrop()
        {
            // Get the base stats component to get the current level
            BaseStats baseStats = GetComponent<BaseStats>();
            // Use the drop library to get the random drops
            IEnumerable<Dropped> items = DropLibrary.GetRandomDrops(baseStats.CurrentLevel);

            // Iterate through the random items and drop them
            foreach (Dropped item in items)
            {
                DropItem(item.Item, item.Number);
            }
        }
    }
}

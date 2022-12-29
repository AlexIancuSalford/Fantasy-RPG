using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// The script is for a "drop library," which is a collection of items that can be randomly "dropped" in the game.
    /// 
    /// The script has several fields that are serialized and can be set in the Unity editor:
    /// 
    /// ItemDropList: an array of DropConfig objects, which contain information about items that can be dropped.
    /// DropChance: an array of floats that represents the chance for an item to be dropped at a certain level.
    /// DropsMin: an array of integers that represents the minimum number of items that can be dropped at a certain level.
    /// DropMax: an array of integers that represents the maximum number of items that can be dropped at a certain level.
    /// There are also several methods in the script:
    /// 
    /// GetRandomDrops(int level): returns a sequence of Dropped objects, which contain information about an item and the
    /// number of that item that was dropped. This method uses other methods in the script to determine whether to drop items
    /// and which items to drop, based on the level provided as an argument.
    ///
    /// SelectRandomItem(int level): returns a DropConfig object that represents a randomly selected item to be dropped,
    /// based on the relative chance of each item being dropped at the given level.
    ///
    /// GetSumChance(int level): returns the sum of the relative chances of all items being dropped at the given level.
    ///
    /// GetNumberOfDrops(int level): returns a random integer between the minimum and maximum number of items that can be
    /// dropped at the given level.
    ///
    /// ShouldRandomDrop(int level): returns a boolean value indicating whether an item should be dropped at the given level,
    /// based on the drop chance at that level.
    ///
    /// GetRandomDropItem(int level): returns a Dropped object that contains information about a randomly selected item and the
    /// number of that item to be dropped, based on the given level.
    ///
    /// The script also has an attribute at the top that specifies that a menu item should be added to the "Create" menu in the
    /// Unity editor, allowing an instance of this scriptable object to be created from the editor.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Object/Item Drop Library")]
    public class ItemDropLibrary : ScriptableObject
    {
        /// <summary>
        /// An array of DropConfig objects, which contain information about items that can be dropped.
        /// </summary>
        [field : SerializeField] private DropConfig[] ItemDropList { get; set; } = null;

        /// <summary>
        /// An array of floats that represents the chance for an item to be dropped at a certain level.
        /// </summary>
        [field : SerializeField] private float[] DropChance { get; set; } = null;

        /// <summary>
        /// An array of integers that represents the minimum number of items that can be dropped at a certain level.
        /// </summary>
        [field: SerializeField] private int[] DropsMin { get; set; } = null;

        /// <summary>
        /// An array of integers that represents the maximum number of items that can be dropped at a certain level.
        /// </summary>
        [field: SerializeField] private int[] DropMax { get; set; } = null;

        /// <summary>
        /// Returns a sequence of Dropped objects, which contain information about items that are randomly dropped at the given level.
        /// </summary>
        /// <param name="level">The level at which the items are being dropped.</param>
        /// <returns>A sequence of Dropped objects.</returns>
        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            // Determine if items should be dropped at this level
            if (!ShouldRandomDrop(level))
            {
                yield break;
            }

            // Get the number of drops for this level and iterate over it
            for (int i = 0; i < GetNumberOfDrops(level); i++)
            {
                // Yield a random drop for each drop that should occur at this level
                yield return GetRandomDropItem(level);
            }
        }

        /// <summary>
        /// Selects a random item to be dropped at the given level, based on the relative chance of each item being dropped.
        /// </summary>
        /// <param name="level">The level at which the item is being dropped.</param>
        /// <returns>A DropConfig object representing the randomly selected item.</returns>
        private DropConfig SelectRandomItem(int level)
        {
            float changeSum = 0f;
            float roll = Random.Range(0, GetSumChance(level));

            // Iterate through the items and select the one with the highest relative chance
            foreach (DropConfig item in ItemDropList)
            {
                changeSum += DropConfig.GetItemByLevel(item.RelativeChance, level);
                if (changeSum > roll)
                {
                    return item;
                }
            }

            // If no item was selected, return null
            return null;
        }

        /// <summary>
        /// Calculates the sum of the relative chances of all items being dropped at the given level.
        /// </summary>
        /// <param name="level">The level at which the items are being dropped.</param>
        /// <returns>The sum of the relative chances of all items being dropped at the given level.</returns>
        private float GetSumChance(int level)
        {
            // Calculate the sum of the relative chances for all items
            return ItemDropList.Sum(item => DropConfig.GetItemByLevel(item.RelativeChance, level));
        }

        /// <summary>
        /// Gets a random integer between the minimum and maximum number of items that can be dropped at the given level.
        /// </summary>
        /// <param name="level">The level at which the items are being dropped.</param>
        /// <returns>A random integer between the minimum and maximum number of items that can be dropped at the given level.</returns>
        private int GetNumberOfDrops(int level)
        {
            // Get a random number of drops between the min and max number of drops at this level
            return Random.Range(DropConfig.GetItemByLevel(DropsMin, level), DropConfig.GetItemByLevel(DropMax, level));
        }

        /// <summary>
        /// Determines whether an item should be dropped at the given level, based on the drop chance at that level.
        /// </summary>
        /// <param name="level">The level at which the item is being dropped.</param>
        /// <returns>True if an item should be dropped at the given level, false otherwise.</returns>
        private bool ShouldRandomDrop(int level)
        {
            // Generate a random number between 0 and 100 and check if it is less than the drop chance at this level
            return Random.Range(0, 100) < DropConfig.GetItemByLevel(DropChance, level);
        }

        /// <summary>
        /// Gets a random drop item and the number of that item to be dropped at the given level.
        /// </summary>
        /// <param name="level">The level at which the item is being dropped.</param>
        /// <returns>A Dropped object containing information about the randomly selected drop item and the number of that item to be dropped.</returns>
        private Dropped GetRandomDropItem(int level)
        {
            // Select a random drop item
            DropConfig drop = SelectRandomItem(level);

            // Return a new Dropped object with the item and a random number of that item to be dropped
            return new Dropped
            {
                Item = drop.Item,
                Number = drop.GetRandomNumber(level)
            };
        }
    }
}

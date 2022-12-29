using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script defines a DropConfig class that is used to specify how an InventoryItem should be dropped in the game.
    /// The DropConfig class has several fields that specify the item to be dropped, the relative chance of the item being dropped,
    /// the maximum and minimum number of items that can be dropped, and the level at which the item is dropped.
    /// 
    /// The GetRandomNumber method generates a random number of items to be dropped based on the minimum and maximum number of
    /// items specified for the given level. If the item is stackable, the method generates a random number between the minimum
    /// and maximum number of items, inclusive. If the item is not stackable, the method returns 1.
    /// 
    /// The GetItemByLevel method is a generic static method that takes an array of items and a level as input, and returns the
    /// item from the array that corresponds to the given level. If the level is outside the range of the array, the default value
    /// for the item type is returned. If the level is invalid, an InvalidOperationException is thrown.
    /// </summary>
    [System.Serializable]
    public class DropConfig
    {
        /// <summary>
        /// The item to be dropped.
        /// </summary>
        public InventoryItem Item;

        /// <summary>
        /// An array of relative chances for the item to be dropped.
        /// The chance for a specific level is calculated by dividing the value at that level's index
        /// by the sum of all the values in the array.
        /// </summary>
        public float[] RelativeChance;

        /// <summary>
        /// An array of maximum numbers of items that can be dropped for each level.
        /// </summary>
        public int[] NumberMax;

        /// <summary>
        /// An array of minimum numbers of items that can be dropped for each level.
        /// </summary>
        public int[] NumberMin;

        /// <summary>
        /// Generates a random number of items to be dropped based on the minimum and maximum number of items specified for the given level.
        /// </summary>
        /// <param name="level">The level at which the item is being dropped.</param>
        /// <returns>A random number of items to be dropped.</returns>
        public int GetRandomNumber(int level)
        {
            int min = GetItemByLevel(NumberMin, level);
            int max = GetItemByLevel(NumberMax, level);

            // If the item is stackable, a random number between the minimum and maximum number of items (inclusive) is generated.
            // If the item is not stackable, 1 is returned.
            return Item.Stackable
                ? Random.Range(min, max + 1)
                : 1;
        }

        /// <summary>
        /// Returns the item from the given array that corresponds to the given level.
        /// </summary>
        /// <typeparam name="T">The type of the items in the array.</typeparam>
        /// <param name="items">The array of items.</param>
        /// <param name="level">The level to retrieve the item for.</param>
        /// <returns>The item from the array that corresponds to the given level.</returns>
        public static T GetItemByLevel<T>(T[] items, int level)
        {
            return level switch
            {
                int n when n > 0 && n <= items.Length => items[level - 1],
                // If the level is outside the range of the array, the default value for the item type is returned.
                int n when n <= 0 || n > items.Length => default(T),
                // If the level is invalid, an InvalidOperationException is thrown.
                _ => throw new InvalidOperationException()
            };
        }
    }
}

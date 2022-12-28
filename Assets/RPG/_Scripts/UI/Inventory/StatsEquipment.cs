using RPG.Stats;
using System.Collections.Generic;
using System.Linq;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script appears to define a class called StatsEquipment that inherits from the Equipment class and implements the
    /// IStatsProvider interface.
    /// 
    /// The StatsEquipment class has two methods: GetModifiers and GetModifiersPercentage. Both of these methods take a Stats
    /// object as an input and return an IEnumerable of floats.
    /// 
    /// The GetModifiers method returns a list of the modifiers for a given stat, as provided by the IStatsProvider interface.
    /// It does this by first getting a list of equipped items using the EquippedItems dictionary and the equippedItem variable.
    /// It then uses Select to transform this list into a list of IStatsProvider objects, using GetItemInSlot to retrieve the
    /// item in each equipped slot. Finally, it uses SelectMany to flatten the list of lists of modifiers into a single list.
    /// 
    /// The GetModifiersPercentage method is similar, but it returns a list of percentage-based modifiers for a given stat
    /// instead of absolute values.
    /// </summary>
    public class StatsEquipment : Equipment, IStatsProvider
    {
        /// <summary>
        /// Gets a list of modifiers for a given stat.
        /// </summary>
        /// <param name="stat">The stat to get modifiers for.</param>
        /// <returns>A list of modifiers for the given stat.</returns>
        public IEnumerable<float> GetModifiers(Stats.Stats stat)
        {
            // Get a list of equipped items and transform the list of equipped items into a list of IStatsProvider objects
            // Then flatten the list of lists of modifiers into a single list
            return EquippedItems.Keys
                .Select(GetItemInSlot)
                .OfType<IStatsProvider>()
                .SelectMany(item => item.GetModifiers(stat));
        }

        /// <summary>
        /// Gets a list of percentage-based modifiers for a given stat.
        /// </summary>
        /// <param name="stat">The stat to get modifiers for.</param>
        /// <returns>A list of percentage-based modifiers for the given stat.</returns>
        public IEnumerable<float> GetModifiersPercentage(Stats.Stats stat)
        {
            // Get a list of equipped items and transform the list of equipped items into a list of IStatsProvider objects
            // Then flatten the list of lists of modifiers into a single list
            return EquippedItems.Keys
                .Select(GetItemInSlot)
                .OfType<IStatsProvider>()
                .SelectMany(item => item.GetModifiersPercentage(stat));
        }
    }
}

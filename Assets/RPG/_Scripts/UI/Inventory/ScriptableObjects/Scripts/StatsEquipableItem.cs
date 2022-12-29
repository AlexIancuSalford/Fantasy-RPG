using RPG.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script defines a class called StatsEquipableItem that is a type of EquipableItem and also implements the
    /// IStatsProvider interface.
    /// 
    /// The StatsEquipableItem class has two serialized fields: Modifiers and PercentageModifiers, both of which are
    /// arrays of Modifier objects. The Modifier class is not shown in this script, but it is likely a simple class
    /// that stores a Stats enum value and a float value.
    /// 
    /// The StatsEquipableItem class also has two methods: GetModifiers and GetModifiersPercentage. Both of these
    /// methods take a Stats enum value as an argument and return an IEnumerable&lt;float&gt; object. The GetModifiers
    /// method returns the values of all Modifier objects in the Modifiers array that have a Stats value matching the
    /// input argument. The GetModifiersPercentage method does the same thing, but for the PercentageModifiers array.
    /// 
    /// Finally, the StatsEquipableItem class has an attribute [CreateAssetMenu(menuName = "Scriptable Object/Stats Eqipable Item")]
    /// which specifies that an instance of this class can be created as a scriptable object in the Unity editor and placed
    /// in the "Scriptable Object" menu.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Object/Stats Eqipable Item")]
    public class StatsEquipableItem : EquipableItem, IStatsProvider
    {
        /// <summary>
        /// An array of Modifier objects representing flat stat bonuses provided by this item.
        /// </summary>
        [field : SerializeField] public Modifier[] Modifiers { get; set; }

        /// <summary>
        /// An array of Modifier objects representing percentage stat bonuses provided by this item.
        /// </summary>
        [field: SerializeField] public Modifier[] PercentageModifiers{ get; set; }

        /// <summary>
        /// Returns the values of all Modifiers in the Modifiers array that have a Stat matching the input argument.
        /// </summary>
        /// <param name="stat">The Stat to filter the Modifiers by.</param>
        /// <returns>An IEnumerable of float values representing the stat bonuses provided by this item.</returns>
        public IEnumerable<float> GetModifiers(Stats.Stats stat)
        {
            return from modifier in Modifiers where modifier.Stat == stat select modifier.Value;
        }

        /// <summary>
        /// Returns the values of all Modifiers in the PercentageModifiers array that have a Stat matching the input argument.
        /// </summary>
        /// <param name="stat">The Stat to filter the Modifiers by.</param>
        /// <returns>An IEnumerable of float values representing the percentage stat bonuses provided by this item.</returns>
        public IEnumerable<float> GetModifiersPercentage(Stats.Stats stat)
        {
            return from modifier in PercentageModifiers where modifier.Stat == stat select modifier.Value;
        }
    }
}

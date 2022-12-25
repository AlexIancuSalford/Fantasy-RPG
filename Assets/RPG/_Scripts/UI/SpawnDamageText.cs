using UnityEngine;

namespace RPG.HUD
{
    /// <summary>
    /// A script for spawning damage text on the HUD.
    /// </summary>
    public class SpawnDamageText : MonoBehaviour
    {
        /// <summary>
        /// The prefab for the damage text to be spawned.
        /// </summary>
        [field : SerializeField] public DamageText DamageTextPrefab { get; private set; } = null;

        /// <summary>
        /// Spawns a new instance of the damage text prefab and sets its text.
        /// </summary>
        /// <param name="damage">The damage value to display in the text.</param>
        public void SpawnText(float damage)
        {
            // Instantiate the damage text prefab and set its text
            DamageText instance = Instantiate<DamageText>(DamageTextPrefab, transform);
            instance.SetText($"{damage:0}");
        }
    }
}

using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.HUD
{
    /// <summary>
    /// A script for displaying the player's current level on the HUD.
    /// </summary>
    public class LevelDisplay : MonoBehaviour
    {
        /// <summary>
        /// The BaseStats component attached to the player game object.
        /// </summary>
        private BaseStats baseStats;

        /// <summary>
        /// The TextMeshProUGUI component that displays the player's level.
        /// </summary>
        private TextMeshProUGUI text;

        /// <summary>
        /// Finds the player game object and gets its BaseStats component.
        /// </summary>
        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        /// <summary>
        /// Gets the TextMeshProUGUI component.
        /// </summary>
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// Updates the player level display.
        /// </summary>
        void Update()
        {
            text.text = $"{baseStats.GetLevel()}";
        }
    }
}

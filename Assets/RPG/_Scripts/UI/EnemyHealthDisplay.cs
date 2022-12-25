using RPG.Combat;
using TMPro;
using UnityEngine;

namespace RPG.HUD
{
    /// <summary>
    /// A script for displaying the current health of the enemy the player is targeting.
    /// </summary>
    public class EnemyHealthDisplay : MonoBehaviour
    {
        /// <summary>
        /// The Fighter component attached to the player game object.
        /// </summary>
        private Fighter fighter;

        /// <summary>
        /// The TextMeshProUGUI component that displays the enemy health.
        /// </summary>
        private TextMeshProUGUI text;

        /// <summary>
        /// Finds the player game object and gets its Fighter component.
        /// </summary>
        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        /// <summary>
        /// Gets the TextMeshProUGUI component.
        /// </summary>
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// Updates the enemy health display.
        /// </summary>
        void Update()
        {
            // If the player has no target, display "N/A"
            // Otherwise, display the current health and max health of the target
            text.text = fighter.Target == null 
                ? "N/A" 
                : $"{fighter.Target.GetCurrentHealth():0}/{fighter.Target.GetMaxHealth():0}";
        }
    }
}

using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.HUD
{
    /// <summary>
    /// A script for displaying the player's current health on the HUD.
    /// </summary>
    public class HealthDisplay : MonoBehaviour
    {
        /// <summary>
        /// The Health component attached to the player game object.
        /// </summary>
        private Health health = null;

        /// <summary>
        /// The TextMeshProUGUI component that displays the player's health.
        /// </summary>
        private TextMeshProUGUI text = null;

        /// <summary>
        /// Finds the player game object and gets its Health component.
        /// </summary>
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        /// <summary>
        /// Gets the TextMeshProUGUI component.
        /// </summary>
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// Updates the player health display.
        /// </summary>
        void Update()
        {
            text.text = $"{health.GetCurrentHealth():0}/{health.GetMaxHealth():0}";
        }
    }
}

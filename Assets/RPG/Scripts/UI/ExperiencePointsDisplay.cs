using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.HUD
{
    /// <summary>
    /// A script for displaying the player's current experience points on the HUD.
    /// </summary>
    public class ExperiencePointsDisplay : MonoBehaviour
    {
        /// <summary>
        /// The Experience component attached to the player game object.
        /// </summary>
        private Experience experience;

        /// <summary>
        /// The TextMeshProUGUI component that displays the experience points.
        /// </summary>
        private TextMeshProUGUI text;

        /// <summary>
        /// Finds the player game object and gets its Experience component.
        /// </summary>
        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        /// <summary>
        /// Gets the TextMeshProUGUI component.
        /// </summary>
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// Updates the experience points display.
        /// </summary>
        void Update()
        {
            text.text = $"{experience.ExperiencePoints:0}";
        }
    }
}

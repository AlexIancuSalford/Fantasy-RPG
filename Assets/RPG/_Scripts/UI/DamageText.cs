using TMPro;
using UnityEngine;

namespace RPG.HUD
{
    /// <summary>
    /// A script for displaying damage text on the HUD.
    /// </summary>
    public class DamageText : MonoBehaviour
    {
        /// <summary>
        /// The TextMeshProUGUI component that displays the damage text.
        /// </summary>
        [field : SerializeField] private TextMeshProUGUI Text { get; set; } = null;

        /// <summary>
        /// Destroys the game object this script is attached to.
        /// </summary>
        public void DestroyText()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Sets the text displayed by the TextMeshProUGUI component.
        /// </summary>
        /// <param name="text">The text to display.</param>
        public void SetText(string text)
        {
            Text.text = text;
        }
    }
}

using UnityEngine;

namespace RPG.HUD
{
    /// <summary>
    /// A script for displaying a health bar on the HUD.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        /// <summary>
        /// The RectTransform component that represents the foreground of the health bar.
        /// </summary>
        [field : SerializeField] private RectTransform Foreground { get; set; } = null;

        /// <summary>
        /// The Canvas component that the health bar is a part of.
        /// </summary>
        [field : SerializeField] private Canvas RootCanvas { get; set; } = null;

        /// <summary>
        /// Sets the fill amount of the health bar.
        /// </summary>
        /// <param name="value">The fill amount, from 0 to 1.</param>
        public void SetHealthBar(float value)
        {
            // If the value is 0 or 1, disable the canvas (which will hide the health bar)
            if (Mathf.Approximately(value, 0) || Mathf.Approximately(value, 1))
            {
                RootCanvas.enabled = false;
                return;
            }

            // Otherwise, enable the canvas and set the local scale of the foreground to the value
            RootCanvas.enabled = true;
            Foreground.localScale = new Vector3(value, 1, 1);
        }
    }
}

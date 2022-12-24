using UnityEngine;

namespace RPG.HUD
{
    public class HealthBar : MonoBehaviour
    {
        [field : SerializeField] private RectTransform Foreground { get; set; } = null;
        [field : SerializeField] private Canvas RootCanvas { get; set; } = null;

        public void SetHealthBar(float value)
        {
            if (Mathf.Approximately(value, 0) || Mathf.Approximately(value, 1))
            {
                RootCanvas.enabled = false;
                return;
            }

            RootCanvas.enabled = true;
            Foreground.localScale = new Vector3(value, 1, 1);
        }
    }
}

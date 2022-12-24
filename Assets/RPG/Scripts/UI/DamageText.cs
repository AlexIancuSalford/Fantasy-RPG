using TMPro;
using UnityEngine;

namespace RPG.HUD
{
    public class DamageText : MonoBehaviour
    {
        [field : SerializeField] private TextMeshProUGUI Text { get; set; } = null;

        public void DestroyText()
        {
            Destroy(gameObject);
        }

        public void SetText(string text)
        {
            Text.text = text;
        }
    }
}

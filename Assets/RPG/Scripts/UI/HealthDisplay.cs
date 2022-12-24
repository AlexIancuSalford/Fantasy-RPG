using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.HUD
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health health;
        private TextMeshProUGUI text;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            text.text = $"{health.GetCurrentHealth():0}/{health.GetMaxHealth():0}";
        }
    }
}

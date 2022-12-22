using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.HUD
{
    public class ExperiencePointsDisplay : MonoBehaviour
    {
        private Experience experience;
        private TextMeshProUGUI text;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            text.text = $"{experience.ExperiencePoints:0}";
        }
    }
}

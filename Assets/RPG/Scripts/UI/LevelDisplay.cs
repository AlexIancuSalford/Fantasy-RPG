using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.HUD
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats baseStats;
        private TextMeshProUGUI text;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            text.text = $"{baseStats.GetLevel()}";
        }
    }
}

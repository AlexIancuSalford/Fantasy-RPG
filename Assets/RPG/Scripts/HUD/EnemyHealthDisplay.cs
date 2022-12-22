using RPG.Combat;
using TMPro;
using UnityEngine;

namespace RPG.HUD
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter fighter;
        private TextMeshProUGUI text;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            text.text = fighter.Target == null ? "N/A" : $"{fighter.Target.ToPercentage():0}%";
        }
    }
}

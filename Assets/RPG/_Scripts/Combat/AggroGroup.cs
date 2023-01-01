using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [field : SerializeField] private Fighter[] Fighters { get; set; } = null;
        [field : SerializeField] private bool IsActiveOnStart { get; set; } = false;

        private void Start()
        {
            Activate(IsActiveOnStart);
        }

        public void Activate(bool shouldActivate)
        {
            foreach (Fighter fighter in Fighters)
            {
                Target target = fighter.GetComponent<Target>();

                if (target != null)
                {
                    target.enabled = shouldActivate;
                }
                fighter.enabled = shouldActivate;
            }
        }
    }
}

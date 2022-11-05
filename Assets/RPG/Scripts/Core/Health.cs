using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [field : SerializeField] public float CurrentHealth { get; private set; } = 100f;

        public bool IsDead { get; private set; } = false;

        private ActionManager ActionManager { get; set; }
        private Animator Animator { get; set; }

        private void Start()
        {
            ActionManager = GetComponent<ActionManager>();
            Animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            Debug.Log(CurrentHealth);

            if (CurrentHealth == 0)
            {
                TriggerDeathAnimation();
            }
        }

        private void TriggerDeathAnimation()
        {
            if (IsDead) { return; }

            IsDead = true;
            Animator.SetTrigger("death");
            ActionManager.CancelAction();
        }
    }
}

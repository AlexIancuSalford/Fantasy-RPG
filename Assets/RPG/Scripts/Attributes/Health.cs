using RPG.Core;
using RPG.Save;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveableEntity
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
                TriggerDeathAnimation(false);
            }
        }

        private void TriggerDeathAnimation(bool isLoading)
        {
            if (IsDead) { return; }

            if (isLoading)
            {
                Animator = GetComponent<Animator>();
                ActionManager = GetComponent<ActionManager>();
            }

            IsDead = true;
            Animator.SetTrigger("death");
            ActionManager.CancelAction();
        }

        public object SaveState()
        {
            return CurrentHealth;
        }

        public void LoadState(object obj)
        {
            CurrentHealth = (float)obj;

            if (CurrentHealth <= 0)
            {
                TriggerDeathAnimation(true);
            }
        }
    }
}

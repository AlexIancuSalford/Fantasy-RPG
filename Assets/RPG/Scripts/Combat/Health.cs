using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [field : SerializeField] public float CurrentHealth { get; private set; } = 100f;

        public bool IsDead { get; private set; } = false;

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
            GetComponent<Animator>().SetTrigger("death");
        }
    }
}

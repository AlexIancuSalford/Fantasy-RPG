using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [field : SerializeField] public float CurrentHealth { get; private set; } = 100f;

        public void TakeDamage(float damage)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            Debug.Log(CurrentHealth);
        }
    }
}

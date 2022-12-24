using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.HUD
{
    public class SpawnDamageText : MonoBehaviour
    {
        [field : SerializeField] public DamageText DamageTextPrefab { get; private set; } = null;

        public void SpawnText(float damage)
        {
            DamageText instance = Instantiate<DamageText>(DamageTextPrefab, transform);
        }
    }
}

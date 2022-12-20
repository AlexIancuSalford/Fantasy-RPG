using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Object/Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [field : SerializeField] private AnimatorOverrideController OverrideController { get; set; } = null;
        [field : SerializeField] private GameObject WeaponPrefab { get; set; } = null;
        [field : SerializeField] public float AttackCooldown { get; set; } = 1f;
        [field : SerializeField] public float Range { get; private set; } = 2f;
        [field : SerializeField] public float Damage { get; private set; } = 5f;

        public void SpawnWeapon(Transform position, Animator animator)
        {
            if (WeaponPrefab != null) { Instantiate(WeaponPrefab, position); }
            if (OverrideController != null) { animator.runtimeAnimatorController = OverrideController; }
        }
    }
}
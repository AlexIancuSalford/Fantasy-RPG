using RPG.Core;
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
        [field : SerializeField] private bool IsRightHanded { get; set; } = true;
        [field : SerializeField] private Projectile Projectile { get; set; } = null;

        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (WeaponPrefab != null)
            {
                Instantiate(WeaponPrefab, IsRightHanded ? rightHand : leftHand);
            }
            if (OverrideController != null) { animator.runtimeAnimatorController = OverrideController; }
        }

        public bool HasProjectile()
        {
            return Projectile != null;
        }

        public void LaunchProjectile(Transform leftHand, Transform rightHand, Health target)
        {
            Projectile projectileInst = Instantiate(
                Projectile, 
                (IsRightHanded ? rightHand : leftHand).position, 
                Quaternion.identity
                );
            projectileInst.SetTarget(target);
        }
    }
}
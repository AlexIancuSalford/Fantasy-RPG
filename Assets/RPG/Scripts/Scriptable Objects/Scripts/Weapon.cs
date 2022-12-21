using System.Runtime.InteropServices.WindowsRuntime;
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

        private const string WEAPON_NAME = "Weapon";

        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(leftHand, rightHand);

            if (WeaponPrefab != null)
            {
                GameObject weapon = Instantiate(WeaponPrefab, IsRightHanded ? rightHand : leftHand);
                weapon.name = WEAPON_NAME;
            }

            AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (OverrideController != null)
            {
                animator.runtimeAnimatorController = OverrideController;
            }
            else if(overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
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
            projectileInst.SetTarget(target, Damage);
        }

        public void DestroyOldWeapon(Transform leftHand, Transform rightHand)
        {
            Transform oldWeapon = rightHand.Find(WEAPON_NAME);

            if (oldWeapon == null) { oldWeapon = leftHand.Find(WEAPON_NAME); }
            if (oldWeapon == null) { return; }

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
    }
}
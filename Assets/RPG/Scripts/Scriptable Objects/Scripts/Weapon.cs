using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Object/Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [field: SerializeField] private AnimatorOverrideController OverrideController { get; set; } = null;
        [field: SerializeField] private GameObject WeaponPrefab { get; set; } = null;

        public void SpawnWeapon(Transform position, Animator animator)
        {
            Instantiate(WeaponPrefab, position);
            animator.runtimeAnimatorController = OverrideController;
        }
    }
}
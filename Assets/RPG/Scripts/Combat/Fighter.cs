using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private Health Target { get; set; }
        private Mover MoverRef { get; set; }
        private ActionManager ActionManager { get; set; }
        private Animator Animator { get; set; }

        private float _timeSinceLastAttack = Mathf.Infinity;
        private Weapon _currentWeapon = null;

        [field : SerializeField] private Transform RightHandTransform { get; set; }
        [field : SerializeField] private Transform LeftHandTransform { get; set; }
        [field : SerializeField] public string DefaultWeapon = "Unarmed";

        // Start is called before the first frame update
        void Start()
        {
            MoverRef = GetComponent<Mover>();
            ActionManager = GetComponent<ActionManager>();
            Animator = GetComponent<Animator>();

            Weapon weapon = Resources.Load(DefaultWeapon) as Weapon;
            EquipWeapon(weapon);
        }

        // Update is called once per frame
        void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (Target == null) { return; }
            if (Target.IsDead) { return; }

            if (IsTargetInRange())
            {
                MoverRef.MoveTo(Target.transform.position);
            }
            else
            {
                MoverRef.Cancel();
                HandleAttackBehaviour();
            }
        }

        /// <summary>
        /// Animation hit event
        /// To be used to trigger on hit events or particle effects
        /// </summary>
        public void Hit()
        {
            if (Target == null) { return; }

            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(RightHandTransform, LeftHandTransform, Target);
            }
            else
            {
                Target.TakeDamage(_currentWeapon.Damage);
            }
        }

        private void Shoot()
        {
            Hit();
        }

        public void Cancel()
        {
            StopAttackAnimation();
            MoverRef.Cancel();
            Target = null;
        }

        public void Attack(GameObject target)
        {
            ActionManager.StartAction(this);
            Target = target.GetComponent<Health>();
        }

        public void HandleAttackBehaviour()
        {
            transform.LookAt(Target.transform);
            if (_timeSinceLastAttack > _currentWeapon.AttackCooldown)
            {
                StartAttackAnimation();
                _timeSinceLastAttack = 0f;
            }
            
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null) { return false; }

            Health targetHealth = target.GetComponent<Health>();

            return targetHealth != null && !targetHealth.IsDead;
        }

        private bool IsTargetInRange()
        {
            return Vector3.Distance(Target.transform.position, gameObject.transform.position) >= _currentWeapon.Range;
        }

        private void StopAttackAnimation()
        {
            Animator.ResetTrigger("attack");
            Animator.SetTrigger("stopAttack");
        }

        private void StartAttackAnimation()
        {
            Animator.ResetTrigger("stopAttack");
            Animator.SetTrigger("attack");
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            _currentWeapon.SpawnWeapon(RightHandTransform, LeftHandTransform, GetComponent<Animator>());
        }
    }
}
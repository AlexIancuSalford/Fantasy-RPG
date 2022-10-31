using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private Transform TargetTransform { get; set; }
        private Mover MoverRef { get; set; }
        private ActionManager ActionManager { get; set; }
        private Animator Animator { get; set; }

        private float _timeSinceLastAttack = 0f; 

        [field: SerializeField] public float BasicAttackCooldown { get; set; } = 1f;
        [field : SerializeField] public float WeaponRange { get; set; }
        [field : SerializeField] public float WeaponDamage { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            MoverRef = GetComponent<Mover>();
            ActionManager = GetComponent<ActionManager>();
            Animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (TargetTransform == null) return;
            if (IsTargetInRange())
            {
                MoverRef.MoveTo(TargetTransform.position);
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
            TargetTransform.GetComponent<Health>().TakeDamage(WeaponDamage);
        }

        public void Cancel()
        {
            TargetTransform = null;
        }

        public void Attack(Target target)
        {
            ActionManager.StartAction(this);
            TargetTransform = target.transform;
        }

        public void HandleAttackBehaviour()
        {
            if (_timeSinceLastAttack > BasicAttackCooldown)
            {
                Animator.SetTrigger("attack");
                _timeSinceLastAttack = 0f;
            }
            
        }

        private bool IsTargetInRange()
        {
            return Vector3.Distance(TargetTransform.position, gameObject.transform.position) >= WeaponRange;
        }
    }
}
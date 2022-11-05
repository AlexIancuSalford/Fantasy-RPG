using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        [field : SerializeField] public float ChaseRange { get; set; } = 6.0f;

        private GameObject _player;

        private Fighter Fighter { get; set; }
        private Health Health { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            Fighter = GetComponent<Fighter>();
            Health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Health.IsDead) { return; }

            if (IsPlayerInRange() && Fighter.CanAttack(_player))
            {
                Fighter.Attack(_player);
            }
            else
            {
                Fighter.Cancel();
            }
        }

        private bool IsPlayerInRange()
        {
            if (_player != null)
            {
                return Vector3.Distance(_player.transform.position, gameObject.transform.position) <= ChaseRange;
            }

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position, ChaseRange);
        }
    }
}

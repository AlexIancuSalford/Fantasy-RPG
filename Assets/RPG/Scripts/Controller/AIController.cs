using System.Security.Cryptography.X509Certificates;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        [field : SerializeField] public float ChaseRange { get; set; } = 4.0f;
        [field : SerializeField] public float SuspicionTime { get; set; } = 4.0f;
        [field : SerializeField] public PatrolController PatrolRoute { get; set; } 

        private GameObject _player;
        private Vector3 _guardLocation;
        private float _timeSincePlayerSpotted = Mathf.Infinity;
        private float _waypointMarginOfError = 1f;
        private int _currentWaypointIndex = 0;

        private Fighter Fighter { get; set; }
        private Health Health { get; set; }
        private Mover Mover { get; set; }
        private ActionManager ActionManager { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            Fighter = GetComponent<Fighter>();
            Health = GetComponent<Health>();
            Mover = GetComponent<Mover>();
            ActionManager = GetComponent<ActionManager>();

            _guardLocation = gameObject.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            switch (true)
            {
                case bool x when Health.IsDead:
                    return;
                case bool x when IsPlayerInRange() && Fighter.CanAttack(_player):
                    _timeSincePlayerSpotted = 0;
                    AttackingBehaviour();
                    break;
                case bool x when (_timeSincePlayerSpotted < SuspicionTime):
                    SuspicionBehaviour();
                    break;
                default:
                    PatrolBehaviour();
                    break;
            }

            _timeSincePlayerSpotted += Time.deltaTime;
        }

        private void AttackingBehaviour()
        {
            Fighter.Attack(_player);
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardLocation;

            if (PatrolRoute != null)
            {
                if (IsAtWaypoint())
                {
                    GetNextWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            Mover.StartMoveAction(nextPosition);
        }

        private bool IsAtWaypoint()
        {
            return Vector3.Distance(gameObject.transform.position, GetCurrentWaypoint()) < _waypointMarginOfError;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return PatrolRoute.GetWaypointAtIndex(_currentWaypointIndex);
        }

        private void GetNextWaypoint()
        {
            _currentWaypointIndex = PatrolRoute.GetNextWaypointIndex(_currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            ActionManager.CancelAction();
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

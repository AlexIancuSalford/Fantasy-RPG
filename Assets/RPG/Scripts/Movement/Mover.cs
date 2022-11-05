using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent Agent { get; set; }
        private Animator Animator { get; set; }
        private ActionManager ActionManager { get; set; }
        private Health Health { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            ActionManager = GetComponent<ActionManager>();
            Health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            Agent.enabled = !Health.IsDead;

            Animator.SetFloat("forwardSpeed", transform.InverseTransformDirection(Agent.velocity).z);
        }

        public void MoveTo(Vector3 destination)
        {
            Agent.isStopped = false;
            Agent.destination = destination;
        }

        /// <summary>
        /// This is just for PlayerController use.
        /// This will stop the attacking move when a move command is issued
        /// </summary>
        /// <param name="destination"></param>
        public void StartMoveAction(Vector3 destination)
        {
            ActionManager.StartAction(this);
            MoveTo(destination);
        }

        public void Cancel()
        {
            Agent.isStopped = true;
        }
    }
}

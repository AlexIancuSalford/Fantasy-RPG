using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent Agent { get; set; }
        private Animator Animator { get; set; }
        private Fighter Fighter { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            Fighter = GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
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
            Fighter.Cancel();
            MoveTo(destination);
        }

        public void Stop()
        {
            Agent.isStopped = true;
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent Agent { get; set; }
        private Animator Animator { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
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

        public void Stop()
        {
            Agent.isStopped = true;
        }
    }
}

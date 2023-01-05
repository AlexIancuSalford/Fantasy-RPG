using RPG.Attributes;
using RPG.Core;
using RPG.Helper;
using RPG.Save;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    /// <summary>
    /// This script defines a Unity component called "Mover" that can be added to
    /// a game object. The Mover component allows the game object to be moved
    /// around the game world using the Unity NavMesh system.
    ///  
    /// The Mover component has several fields that are set when the component
    /// is initialized:
    ///  
    ///  1. NavMeshAgent: This field is used to store a reference to the
    /// NavMeshAgent component attached to the same game object. The NavMeshAgent
    /// is used to move the game object around the game world using the NavMesh.
    /// 
    ///  2. Animator: This field is used to store a reference to the Animator
    /// component attached to the same game object. The Animator is used to
    /// control the animation of the game object.
    /// 
    ///  3. ActionManager: This field is used to store a reference to the
    /// ActionManager component attached to the same game object. The ActionManager
    /// is used to manage actions that can be performed by the game object.
    /// 
    ///  4. Health: This field is used to store a reference to the Health
    /// component attached to the same game object. The Health component is used
    /// to track the health of the game object.
    /// 
    ///  The Mover component has several methods:
    ///  
    /// Awake(): This method is called when the component is initialized.
    /// It sets the values of the fields described above by getting references
    /// to the corresponding components attached to the same game object.
    /// 
    /// Update(): This method is called once per frame. It disables the
    /// NavMeshAgent if the game object is dead (as determined by the Health
    /// component) and sets the "forwardSpeed" parameter of the Animator based on
    /// the forward velocity of the NavMeshAgent.
    /// 
    /// MoveTo(Vector3 destination): This method sets the destination of the
    /// NavMeshAgent to the specified location and starts the NavMeshAgent moving
    /// towards it.
    /// 
    /// StartMoveAction(Vector3 destination): This method starts a move action by
    /// calling the StartAction() method of the ActionManager and then calling the
    /// 
    /// MoveTo() method to move the game object to the specified destination.
    /// 
    /// Cancel(): This method stops the NavMeshAgent from moving.
    /// 
    /// SaveState(): This method returns an object that represents the current
    /// state of the Mover component and can be used to restore the component to
    /// this state later.
    /// 
    /// LoadState(object obj): This method restores the Mover component to a
    /// previously saved state. It does this by setting the position of the game
    /// object to the position stored in the saved state, and then stopping the
    /// current action by calling the CancelAction() method of the ActionManager.
    /// 
    /// The Mover component also implements two interfaces: IAction and
    /// ISaveableEntity. The IAction interface is used to indicate that the
    /// Mover component represents an action that can be performed by the game
    /// object. The ISaveableEntity interface is used to indicate that the Mover
    /// component should be included in the game's save data, and provides methods
    /// for saving and loading the component's state.
    /// </summary>
    public class Mover : MonoBehaviour, IAction, ISaveableEntity
    {
        private NavMeshAgent Agent { get; set; }
        private Animator Animator { get; set; }
        private ActionManager ActionManager { get; set; }
        private Health Health { get; set; }

        // Start is called before the first frame update
        void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            ActionManager = GetComponent<ActionManager>();
            Health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            // Disable the NavMeshAgent if the game object is dead
            Agent.enabled = !Health.IsDead();

            // Set the "forwardSpeed" parameter of the Animator based on the forward velocity of the NavMeshAgent
            Animator.SetFloat("forwardSpeed", transform.InverseTransformDirection(Agent.velocity).z);
        }

        /// <summary>
        /// Moves the game object to the specified destination using the NavMeshAgent.
        /// </summary>
        /// <param name="destination">The destination to move to.</param>
        public void MoveTo(Vector3 destination)
        {
            // Set the destination of the NavMeshAgent and start it moving
            Agent.isStopped = false;
            Agent.destination = destination;
        }

        /// <summary>
        /// Starts a move action by calling the StartAction() method of the ActionManager and then
        /// calling the MoveTo() method to move the game object to the specified destination.
        /// This method is intended to be used by the PlayerController.
        /// </summary>
        /// <param name="destination">The destination to move to.</param>
        public void StartMoveAction(Vector3 destination)
        {
            // Start a move action
            ActionManager.StartAction(this);
            MoveTo(destination);
        }

        /// <summary>
        /// Stops the NavMeshAgent from moving.
        /// </summary>
        public void Cancel()
        {
            Agent.isStopped = true;
        }

        /// <summary>
        /// Returns an object that represents the current state of the Mover component and can be
        /// used to restore the component to this state later.
        /// </summary>
        /// <returns>An object representing the current state of the Mover component.</returns>
        public object SaveState()
        {
            // Return the current position of the game object as a Vector3f object
            return new Vector3f(transform.position);
        }

        /// <summary>
        /// Restores the Mover component to a previously saved state.
        /// </summary>
        /// <param name="obj">An object representing the saved state of the Mover component.</param>
        public void LoadState(object obj)
        {
            // Disable the NavMeshAgent as it can compete when changing the position attribute
            GetComponent<NavMeshAgent>().enabled = false;

            // Set the position of the game object to the position stored in the saved state
            Vector3f position = (Vector3f)obj;
            transform.position = position;

            // Re-enable the NavMeshAgent and cancel the current action
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionManager>().CancelAction();
        }
    }
}

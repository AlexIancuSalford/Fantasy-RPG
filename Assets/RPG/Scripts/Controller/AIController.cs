/* This script defines an AIController class in the RPG.Controller namespace.
 * The class is derived from MonoBehaviour and is designed to be attached to
 * a game object in a Unity project. The class is designed to control the
 * behavior of an NPC in a role-playing game.
 *
 * The AIController class has several serialized fields that can be edited
 * in the Unity editor. The ChaseRange field is a float value that determines
 * the range at which the NPC will start chasing the player.
 * The SuspicionTime field is a float value that determines the amount of
 * time that the NPC will be in a suspicious state after the player has left
 * their chase range. The PatrolRoute field is a reference to a
 * PatrolController object that specifies the patrol route that the NPC will
 * follow when not attacking or suspicious of the player. The WaypointStopTime
 * field is a float value that determines the amount of time that the NPC will stay at each waypoint in their patrol route.
 *
 * The AIController class has several private fields that are used to store
 * state information. The _player field is a reference to the player game
 * object. The _guardLocation field is a vector that stores the initial
 * location of the NPC. The _timeSincePlayerSpotted field is a float value
 * that tracks the amount of time that has passed since the player was last
 * spotted. The _timeAtWaypoint field is a float value that tracks the amount
 * of time that the NPC has spent at their current waypoint.
 * The _waypointMarginOfError field is a float value that determines how
 * close the NPC needs to be to a waypoint in order to be considered "at"
 * the waypoint. The _currentWaypointIndex field is an integer value that
 * tracks the index of the NPC's current waypoint in their patrol route.
 *
 * The AIController class also has several properties that allow other
 * scripts to access components attached to the NPC game object. The Fighter
 * property is a reference to the NPC's Fighter component, which handles
 * their combat behavior. The Health property is a reference to the NPC's
 * Health component, which tracks their health points. The Mover property
 * is a reference to the NPC's Mover component, which handles their movement.
 * The ActionManager property is a reference to the NPC's ActionManager
 * component, which manages the NPC's actions.
 *
 * The Start method is a special Unity method that is called when the script
 * is first enabled. In this method, the NPC finds the player game object
 * using its tag, and then gets references to its Fighter, Health, Mover,
 * and ActionManager components. The Start method also stores the NPC's
 * initial location in the _guardLocation field.
 *
 * The Update method is a special Unity method that is called once per frame.
 * In this method, the NPC's behavior is determined based on a series of
 * conditions. If the NPC is dead, it does nothing. If the player is within
 * range and the NPC is able to attack them, it starts attacking. If the
 * player was recently spotted and the NPC is still in a suspicious state,
 * it acts suspicious. If none of these conditions are met, the NPC patrols.
 * The Update method also updates the NPC's timers.
 *
 * The AttackingBehaviour method handles the NPC's behavior when it is
 * attacking the player. It calls the Attack method on the NPC's Fighter
 * component, passing the player game object as an argument
*/

using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Controller
{
    /// <summary>
    /// This class serves as the AI controller for an NPC in a role-playing game (I mean this one, of course)
    /// </summary>
    public class AIController : MonoBehaviour
    {
        // The range at which the NPC will start chasing the player.
        [field : SerializeField] public float ChaseRange { get; set; } = 4.0f;
        // The amount of time that the NPC will be in a suspicious state after the player has left their chase range.
        [field : SerializeField] public float SuspicionTime { get; set; } = 4.0f;
        // The patrol route that the NPC will follow when not attacking or suspicious of the player.
        [field : SerializeField] public PatrolController PatrolRoute { get; set; }
        // The amount of time that the NPC will stay at each waypoint in their patrol route.
        [field: SerializeField] public float WaypointStopTime { get; set; } = 4f;

        private GameObject _player;
        private Vector3 _guardLocation;
        private float _timeSincePlayerSpotted = Mathf.Infinity;
        private float _timeAtWaypoint = Mathf.Infinity;
        private float _waypointMarginOfError = 1f;
        private int _currentWaypointIndex = 0;

        private Fighter Fighter { get; set; }
        private Health Health { get; set; }
        private Mover Mover { get; set; }
        private ActionManager ActionManager { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            // TODO: Move to Awake

            // Find the player GameObject using its tag.
            _player = GameObject.FindGameObjectWithTag("Player");

            // Get the Fighter, Health, Mover, and ActionManager components of the NPC.
            Fighter = GetComponent<Fighter>();
            Health = GetComponent<Health>();
            Mover = GetComponent<Mover>();
            ActionManager = GetComponent<ActionManager>();

            // Set the initial location of the NPC.
            _guardLocation = gameObject.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            // Determine the NPC's behavior based on a series of conditions.
            switch (true)
            {
                // If the NPC is dead, do nothing.
                case bool x when Health.IsDead:
                    return;
                // If the player is within range and the NPC is able to attack them, start attacking.
                case bool x when IsPlayerInRange() && Fighter.CanAttack(_player):
                    _timeSincePlayerSpotted = 0;
                    AttackingBehaviour();
                    break;
                // If the player was recently spotted and the NPC is still in a suspicious state, act suspicious.
                case bool x when (_timeSincePlayerSpotted < SuspicionTime):
                    SuspicionBehaviour();
                    break;
                // If none of the above conditions are met, patrol.
                default:
                    PatrolBehaviour();
                    break;
            }

            // Update the NPC's timers.
            UpdateTimers();
        }

        /// <summary>
        /// The NPC's behavior when attacking the player.
        /// </summary>
        private void AttackingBehaviour()
        {
            Fighter.Attack(_player);
        }

        /// <summary>
        /// The NPC's behavior when patrolling.
        /// </summary>
        private void PatrolBehaviour()
        {
            // The destination for the NPC's next move.
            Vector3 nextPosition = _guardLocation;

            // If the NPC has a patrol route, use it to determine their next destination.
            if (PatrolRoute != null)
            {
                // If the NPC has reached a waypoint, move on to the next one.
                if (IsAtWaypoint())
                {
                    _timeAtWaypoint = 0f;
                    GetNextWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            // If the NPC has spent enough time at their current waypoint (or their initial position), start moving towards the next destination.
            if (_timeAtWaypoint > WaypointStopTime)
            {
                Mover.StartMoveAction(nextPosition);
            }
        }

        /// <summary>
        /// This method checks if the NPC reached the waypoint
        /// </summary>
        /// <returns>Returns true if the NPC has reached their current waypoint.</returns>
        private bool IsAtWaypoint()
        {
            return Vector3.Distance(gameObject.transform.position, GetCurrentWaypoint()) < _waypointMarginOfError;
        }

        /// <summary>
        /// This method gets the NPC's current waypoint
        /// </summary>
        /// <returns>Returns the NPC's current waypoint.</returns>
        private Vector3 GetCurrentWaypoint()
        {
            return PatrolRoute.GetWaypointAtIndex(_currentWaypointIndex);
        }

        /// <summary>
        /// Moves the NPC to the next waypoint in their patrol route.
        /// </summary>
        private void GetNextWaypoint()
        {
            _currentWaypointIndex = PatrolRoute.GetNextWaypointIndex(_currentWaypointIndex);
        }

        /// <summary>
        /// The NPC's behavior when suspicious of the player.
        /// </summary>
        private void SuspicionBehaviour()
        {
            // Cancel the NPC's current action.
            ActionManager.CancelAction();
        }

        /// <summary>
        /// This methods checks if the player is in the range of the NPCs attack
        /// </summary>
        /// <returns>Returns true if the player is within the NPC's chase range.</returns>
        private bool IsPlayerInRange()
        {
            if (_player != null)
            {
                return Vector3.Distance(_player.transform.position, gameObject.transform.position) <= ChaseRange;
            }

            // If the player GameObject can't be found, return false.
            return false;
        }

        /// <summary>
        /// Updates the NPC's timers.
        /// </summary>
        private void UpdateTimers()
        {
            _timeSincePlayerSpotted += Time.deltaTime;
            _timeAtWaypoint += Time.deltaTime;
        }

        /// <summary>
        /// This method is called when the NPC's GameObject is selected in the editor.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position, ChaseRange);
        }
    }
}

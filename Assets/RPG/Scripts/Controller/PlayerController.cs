/* 
 * The script has three private properties: MoveToTarget, Fighter, and Health.
 * MoveToTarget is a component for moving the player character to a target location,
 * Fighter is a component for handling combat, and Health is a component for
 * tracking the player's health.
 *  
 * In the Start method, these properties are assigned to the corresponding
 * components attached to the player character game object.
 *  
 * In the Update method, a switch statement is used to determine what action
 * the player should take based on whether they are in combat, can move to the
 * cursor, or are dead. If the player is dead, the method returns without
 * doing anything. If the player is in combat, it allows the player to attack
 * the target if the left mouse button is pressed. If the player can move to
 * the cursor, it allows the player to move to the location indicated by the
 * cursor if the left mouse button is pressed.
 *  
 * The CanMoveToCursor method uses a raycast to determine if the player can
 * move to the location indicated by the cursor. If the left mouse button is
 * pressed and the player can move to the cursor, the StartMoveAction method
 * of the MoveToTarget component is called with the hit point as an argument.
 *  
 *  The IsInCombat method uses a raycast to check if the player is targeting
 * an enemy. If an enemy is targeted and the left mouse button is pressed,
 * the Attack method of the Fighter component is called with the enemy game
 * object as an argument.
 *  
 *  The GetRayFromScreenPoint method returns a ray going from the main camera
 * through the mouse cursor position on the screen. This ray is used to check
 * for objects that the player is targeting.
 */

using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private Mover MoveToTarget { get; set; }
        private Fighter Fighter { get; set; }
        private Health Health { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            // TODO: Move to Awake
            MoveToTarget = GetComponent<Mover>();
            Fighter = GetComponent<Fighter>();
            Health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            // Determine what action the player should take based on whether they are in combat, can move to the cursor, or are dead
            switch (true)
            {
                // If the player is dead, return without doing anything
                case bool x when Health.IsDead:
                    return;
                // If the player is in combat, allow the player to attack the target if the left mouse button is pressed
                case bool x when IsInCombat():
                    break;
                // If the player can move to the cursor, allow the player to move to the location indicated by the cursor if the left mouse button is pressed
                case bool x when CanMoveToCursor():
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Determines if the player can move to the location indicated by the cursor.
        /// </summary>
        /// <returns>True if the player can move to the cursor, false otherwise.</returns>
        private bool CanMoveToCursor()
        {
            // Use a raycast to check if the player can move to the cursor
            bool hasHit = Physics.Raycast(GetRayFromScreenPoint(), out RaycastHit hit);

            // If the raycast didn't hit anything, return false
            if (!hasHit) { return false; }

            // If the left mouse button is pressed, call the StartMoveAction method of the Mover component with the hit point as an argument
            if (Input.GetMouseButton(0))
            {
                MoveToTarget.StartMoveAction(hit.point);
            }

            // Return true since the player can move to the cursor
            return true;
        }

        /// <summary>
        /// Determines if the player is in combat.
        /// </summary>
        /// <returns>True if the player is in combat, false otherwise.</returns>
        private bool IsInCombat()
        {
            // Use a raycast to check if the player is targeting an enemy
            RaycastHit[] raycastHits = Physics.RaycastAll(GetRayFromScreenPoint());

            foreach (RaycastHit raycastHit in raycastHits)
            {
                // Get the Target component of the object that was hit by the raycast
                Target target = raycastHit.transform.GetComponent<Target>();

                // If the object doesn't have a Target component, continue to the next object
                if (target == null) { continue; }

                // If the player can't attack the target, continue to the next object
                if (!Fighter.CanAttack(target.gameObject)) { continue; }

                // If the player can't attack the target, continue to the next object
                if (Input.GetMouseButton(0))
                {
                    Fighter.Attack(target.gameObject);
                }

                // return true since the player is in combat
                return true;
            }

            // If no enemies were targeted or the player can't attack any of them, return false
            return false;
        }

        /// <summary>
        /// Gets a ray going from the main camera through the mouse cursor position on the screen.
        /// </summary>
        /// <returns>The ray from the main camera through the mouse cursor position on the screen.</returns>
        private Ray GetRayFromScreenPoint()
        {
            // Return the ray from the main camera through the mouse cursor position on the screen
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

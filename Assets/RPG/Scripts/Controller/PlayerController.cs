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
 * The IsInCombat method uses a raycast to check if the player is targeting
 * an enemy. If an enemy is targeted and the left mouse button is pressed,
 * the Attack method of the Fighter component is called with the enemy game
 * object as an argument.
 *  
 * The GetRayFromScreenPoint method returns a ray going from the main camera
 * through the mouse cursor position on the screen. This ray is used to check
 * for objects that the player is targeting.
 *
 * The SetCursor method is used to change the mouse cursor in the game to a
 * custom cursor defined by the CursorType enum. The CursorType enum is
 * defined in the Helper namespace and contains a list of different cursor
 * types such as Attack, Walk, Talk, and None.
 *  
 * The GetCursorMapping method is used to retrieve the CursorMapping
 * object that corresponds to a specific CursorType. The CursorMapping
 * object contains information about the custom cursor such as its texture,
 * hotspot, and priority. This information is used to set the custom
 * cursor when the SetCursor method is called.
 *  
 * In the Update method, the SetCursor method is called with a CursorType
 * argument based on the type of action that is possible when raycasting
 * over a target. For example, if the player is targeting an enemy and the
 * left mouse button is pressed, the Attack cursor is set. If the player
 * can move to the location indicated by the cursor, the Walk cursor is set.
 * If no action is possible, the None cursor is set.
 */

using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using CursorType = RPG.Helper.CursorHelper.CursorType;
using CursorMapping = RPG.Helper.CursorHelper.CursorMapping;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private Mover MoveToTarget { get; set; }
        private Fighter Fighter { get; set; }
        private Health Health { get; set; }

        [field : SerializeField] private CursorMapping[] CursorMappings { get; set; } = null;

        private void Awake()
        {
            MoveToTarget = GetComponent<Mover>();
            Fighter = GetComponent<Fighter>();
            Health = GetComponent<Health>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // I'll leave the Start here just in case
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
                // If the default is reached, then the player can neither move to location, nor attack, so set the none cursor
                default:
                    SetCursor(CursorType.None);
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

            // Set the cursor to move, indicating the ability to move to location
            SetCursor(CursorType.Move);
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

                SetCursor(CursorType.Attack);
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

        /// <summary>
        /// Sets the mouse cursor to a custom cursor defined by the CursorType enum.
        /// </summary>
        /// <param name="cursorType">The type of custom cursor to set</param>
        private void SetCursor(CursorType cursorType)
        {
            // Get the CursorMapping object that corresponds to the specified CursorType
            CursorMapping cursorMapping = GetCursorMapping(cursorType);
            // Set the custom cursor using the information in the CursorMapping object
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }

        /// <summary>
        /// Returns the CursorMapping object that corresponds to the specified CursorType.
        /// </summary>
        /// <param name="cursorType">The type of cursor to get the mapping for</param>
        /// <returns>The CursorMapping object for the specified cursor type</returns>
        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            // Iterate through the CursorMappings array to find the CursorMapping object with a matching CursorType
            foreach (CursorMapping mapping in CursorMappings)
            {
                // Return the CursorMapping object if a match is found
                if (mapping.type == cursorType)
                {
                    return mapping;
                }
            }

            // If no match is found, just return the first element
            // TODO: This should never be the case, but should add protection against it
            return CursorMappings[0];
        }
    }
}

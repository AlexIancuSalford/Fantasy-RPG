/*
 * The PlayerController script is a script that controls the behavior of a player
 * character in a game. It is attached to a game object in a Unity project and is
 * responsible for handling player input and determining the appropriate action
 * for the player to take based on that input.
 *  
 * The script is using several other scripts as dependencies, which are imported
 * at the top:
 *  
 * RPG.Attributes: This script contains functionality related to character
 * attributes, such as health, damage, and other statistics.
 *
 * RPG.Combat: This script contains functionality related to combat,
 * such as attacking enemies and taking damage.
 *
 * RPG.Movement: This script contains functionality related to movement,
 * such as moving the player character to a specified location.
 *
 *
 * The PlayerController script has several private fields:
 *  
 * MoveToTarget: This is a Mover component that is used to move the player character
 * to a specified location.
 *
 * Fighter: This is a Fighter component that is used to handle combat-related
 * actions, such as attacking enemies.
 *
 * Health: This is a Health component that is used to track the player
 * character's health and determine if they are dead.
 *
 * CursorMappings: This is an array of CursorMapping objects that are
 * used to map a CursorType enum value to a cursor texture.
 *
 * In the Awake method, the MoveToTarget, Fighter, and Health fields are
 * assigned the corresponding components on the game object that the script is attached to.
 *  
 * The Update method is called once per frame and is used to determine
 * the appropriate action for the player to take based on their current
 * state. It does this by using a switch statement with several case
 * blocks, each of which checks for a different condition.
 *  
 * The first case block checks if the player has clicked on an UI element by
 * calling the CanInteractWithUI method. If this is true, the SetCursor
 * method is called with the CursorType.UI enum value to set the mouse
 * cursor to the UI cursor.
 *  
 * The second case block checks if the player is dead by checking the
 * IsDead property of the Health component. If this is true, the SetCursor
 * method is called with the CursorType.None enum value to set the mouse
 * cursor to the default cursor.
 *  
 * The third case block checks if the player can interact with an object
 * by calling the CanInteractWithObject method. If this is true, no action is taken.
 *  
 * The fourth case block checks if the player can move to the location
 * indicated by the mouse cursor by calling the CanMoveToCursor method. If
 * this is true and the left mouse button is pressed, the StartMoveAction
 * method of the Mover component is called with the hit point as an argument.
 * The SetCursor method is then called with the CursorType.Move enum value
 * to set the mouse cursor to the move cursor.
 *  
 * The default case block is reached if none of the previous conditions are
 * met, in which case the SetCursor method is called with the CursorType.
 * None enum value to set the mouse cursor to the default cursor.
 *  
 * The CanMoveToCursor method is used to determine if the player can move
 * to the location indicated by the mouse cursor. It does this by using a
 * raycast to check if the player can move to the cursor, and if the left mouse
 */

using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.EventSystems;
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
                // If the player clicks on an UI element, set the cursor to none and return
                case bool x when CanInteractWithUI():
                    SetCursor(CursorType.UI);
                    break;
                // If the player is dead, return without doing anything
                case bool x when Health.IsDead:
                    SetCursor(CursorType.None);
                    return;
                case bool x when CanInteractWithObject():
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

        /// <summary>
        /// This method checks if the cursor is over an UI element using the Unity Engine EventSystem
        /// </summary>
        /// <returns>True if the cursor is over an UI element, false otherwise</returns>
        private bool CanInteractWithUI()
        {
            // Returns true if the cursor is over an UI element
            return EventSystem.current.IsPointerOverGameObject();
        }

        /// <summary>
        /// Determines if the player can interact with an object at the location indicated by the cursor.
        /// </summary>
        /// <returns>True if the player can interact with an object, false otherwise.</returns>
        private bool CanInteractWithObject()
        {
            // Use a raycast to check if the player can interact with an object
            RaycastHit[] raycastHits = SortedRaycastAll();

            // If the raycast didn't hit anything, return false
            foreach (RaycastHit raycastHit in raycastHits)
            {
                // Get all the game objects that implement the IRaycastable interface
                IRaycastable[] raycastables = raycastHit.transform.GetComponents<IRaycastable>();

                // Check if the raycasting hit anything
                if (raycastables.Length <= 0) { continue; }
                // Call the HandleRequest method on the first object hit implementing IRaycastable
                if (raycastables[0].HandleRaycast(this))
                {
                    SetCursor(raycastables[0].GetCursorType());
                    return true;
                }

                // I'll leave this here since in the future I may want to handle the raycasting on all 
                // objects hit, but as of now, just handling the first object is fine.
                //foreach (IRaycastable raycastable in raycastables)
                //{
                //    // Call the HandleRequest method on the object implementing IRaycastable
                //    if (raycastable.HandleRaycast(this))
                //    {
                //        SetCursor(raycastable.GetCursorType());
                //        return true;
                //    }
                //}
            }

            // If no enemies were targeted or the player can't attack any of them, return false
            return false;
        }

        /// <summary>
        /// This method sorts an array of raycast hits based on their distance from the cursor.
        /// The method uses Array.Sort to sort the array of raycast hits in place.
        ///
        /// Using Array.Sort to sort the RaycastAll array is likely to be more performant
        /// than using the OrderBy method from the System.Linq namespace. This is because
        /// Array.Sort is a native method implemented in C#, while OrderBy is implemented
        /// in C# and makes use of LINQ (Language Integrated Query) which is a higher
        /// level abstraction.
        /// </summary>
        /// <returns>The sorted array of raycast hits</returns>
        private RaycastHit[] SortedRaycastAll()
        {
            // Raycast from a given position and direction
            RaycastHit[] hits = Physics.RaycastAll(GetRayFromScreenPoint());

            // Sort the array based on distance
            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

            return hits;
        }
    }
}

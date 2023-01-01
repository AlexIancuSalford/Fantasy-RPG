using RPG.Attributes;
using RPG.Controller;
using UnityEngine;
using static RPG.Helper.CursorHelper;

namespace RPG.Combat
{
    /// <summary>
    /// The script is a component for a Unity game object that allows the object to be targeted and attacked by a player character.
    /// The component requires a Health component to be attached to the same game object.
    /// 
    /// The script implements the IRaycastable interface, which means it has two methods that need to be implemented:
    /// 
    /// HandleRaycast and GetCursorType. The HandleRaycast method is called when the object is being "raycasted"
    /// (that is, when the player is pointing at the object with the cursor and interacting with it). The method
    /// receives a reference to the PlayerController object that is calling the method.
    /// 
    /// The HandleRaycast method first checks if the player character can attack the target object by calling the CanAttack
    /// method on the Fighter component attached to the player character. If the player can't attack the target, the method
    /// returns false. Otherwise, if the left mouse button is being held down, the player character attacks the target
    /// by calling the Attack method on the Fighter component. Finally, the method returns true to indicate that the player
    /// is in combat with the target.
    /// 
    /// The GetCursorType method returns the CursorType.Attack value, which is a cursor that indicates that the player can attack the target.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class Target : MonoBehaviour, IRaycastable
    {
        /// <summary>
        /// This method is called when the object is being "raycasted" (that is, when the player is pointing at the object with the cursor and interacting with it).
        /// The method receives a reference to the PlayerController object that is calling the method.
        /// </summary>
        /// <param name="caller">The PlayerController that called this method, usually the player</param>
        /// <returns>True or False, depending on the users needs</returns>
        public bool HandleRaycast(PlayerController caller)
        {
            if (!enabled) { return false; }

            // If the player can't attack the target, move along
            if (!caller.GetComponent<Fighter>().CanAttack(gameObject)) { return false; }

            // If the player can attack the target, attack the target if the left mouse button is being held down
            if (Input.GetMouseButton(0)) { caller.GetComponent<Fighter>().Attack(gameObject); }

            // Return true since the player is in combat
            return true;
        }

        /// <summary>
        /// This method returns the CursorType value that indicates the type of cursor to use when the player is pointing at the object with the cursor.
        /// </summary>
        /// <returns>Return the attack cursor, since this is a target object</returns>
        public CursorType GetCursorType()
        {
            return CursorType.Attack;
        }
    }
}

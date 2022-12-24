using RPG.Attributes;
using RPG.Controller;
using UnityEngine;
using static RPG.Helper.CursorHelper;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class Target : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController caller)
        {
            // If the player can't attack the target, move along
            if (!caller.GetComponent<Fighter>().CanAttack(gameObject)) { return false; }

            // If the player can attack the target, attack the target
            if (Input.GetMouseButton(0)) { caller.GetComponent<Fighter>().Attack(gameObject); }

            // Return true since the player is in combat
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Attack;
        }
    }
}

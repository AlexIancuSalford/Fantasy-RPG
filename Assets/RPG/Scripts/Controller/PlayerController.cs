using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPGCharacterAnims.Actions;
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
            MoveToTarget = GetComponent<Mover>();
            Fighter = GetComponent<Fighter>();
            Health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (true)
            {
                case bool x when Health.IsDead:
                    return;
                case bool x when IsInCombat():
                    break;
                case bool x when CanMoveToCursor():
                    break;
                default:
                    break;
            }
        }

        private bool CanMoveToCursor()
        {
            bool hasHit = Physics.Raycast(GetRayFromScreenPoint(), out RaycastHit hit);

            if (!hasHit) { return false; }

            if (Input.GetMouseButton(0))
            {
                MoveToTarget.StartMoveAction(hit.point);
            }
            
            return true;
        }

        private bool IsInCombat()
        {
            RaycastHit[] raycastHits = Physics.RaycastAll(GetRayFromScreenPoint());

            foreach (RaycastHit raycastHit in raycastHits)
            {
                Target target = raycastHit.transform.GetComponent<Target>();

                if (target == null) { continue; }

                if (!Fighter.CanAttack(target.gameObject)) { continue; }

                if (Input.GetMouseButtonDown(0))
                {
                    Fighter.Attack(target.gameObject);
                }

                return true;
            }

            return false;
        }

        private Ray GetRayFromScreenPoint()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

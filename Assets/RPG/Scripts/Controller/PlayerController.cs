using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private Mover MoveToTarget { get; set; }
        private Fighter Fighter { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            MoveToTarget = GetComponent<Mover>();
            Fighter = GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (true)
            {
                case bool x when IsInCombat():
                    break;
                case bool y when CanMoveToCursor():
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
                MoveToTarget.MoveTo(hit.point);
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

                if (Input.GetMouseButtonDown(0))
                {
                    Fighter.Attack(target);
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

using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.Assertions;

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

            Assert.IsNotNull(Fighter);
            Assert.IsNotNull(MoveToTarget);
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovement();
            HandleCombat();
        }

        private void HandleMovement()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        private void HandleCombat()
        {
            RaycastHit[] raycastHits = Physics.RaycastAll(GetRayFromScreenPoint());

            foreach (RaycastHit raycastHit in raycastHits)
            {
                Target target = raycastHit.transform.GetComponent<Target>();

                if (target != null && Input.GetMouseButtonDown(0))
                {
                    Fighter.Attack(target);
                }
            }
        }

        private void MoveToCursor()
        {
            bool hasHit = Physics.Raycast(GetRayFromScreenPoint(), out RaycastHit hit);

            if (hasHit)
            {
                MoveToTarget.MoveTo(hit.point);
            }
        }

        private Ray GetRayFromScreenPoint()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

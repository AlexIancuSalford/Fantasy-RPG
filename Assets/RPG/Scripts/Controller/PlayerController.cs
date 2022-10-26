using UnityEngine;
using RPG.Movement;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private Mover MoveToTarget { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            MoveToTarget = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        private void MoveToCursor()
        {
            bool hasHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

            if (hasHit)
            {
                MoveToTarget.MoveTo(hit.point);
            }
        }
    }
}

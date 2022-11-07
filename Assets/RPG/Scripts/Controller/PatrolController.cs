using UnityEngine;

namespace RPG.Controller
{
    public class PatrolController : MonoBehaviour
    {
        [field: SerializeField, Range(0f, 0.5f)] public float WaypointSize { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextWaypointIndex(i);

                Gizmos.DrawSphere(transform.GetChild(i).position, WaypointSize);
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(j).position);
            }
        }

        public int GetNextWaypointIndex(int index)
        {
            return (index + 1) % transform.childCount;
        }

        public Vector3 GetWaypointAtIndex(int index)
        {
            return transform.GetChild(index).position;
        }
    }
}

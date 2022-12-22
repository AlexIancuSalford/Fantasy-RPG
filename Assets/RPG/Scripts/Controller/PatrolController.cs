/* This script defines a PatrolController class in the RPG.Controller
 * namespace. The class is derived from MonoBehaviour and is designed to be
 * attached to a game object in a Unity project.
 *  
 * The PatrolController class has a serialized field called WaypointSize,
 * which is a float value that can be edited in the Unity editor. The Range
 * attribute specifies that this value should be between 0 and 0.5. There is
 * also a getter and setter for WaypointSize that allow the value to be
 * accessed and modified from other scripts.
 *  
 * The OnDrawGizmos method is a special Unity method that is called when the
 * gizmos for a game object are drawn in the editor. This method is used to
 * draw a series of spheres and lines that represent the waypoints in the
 * patrol path. The method iterates over all the children of the PatrolController game
 * object, draws a sphere at each child's position with a size specified by
 * WaypointSize, and then draws a line connecting that child to the next
 * waypoint in the path.
 *  
 * The GetNextWaypointIndex method takes an index as an argument and returns
 * the index of the next waypoint in the path. If the provided index is the
 * last waypoint in the path, the method returns the index of the first
 * waypoint. This allows the path to be treated as a loop.
 *  
 * The GetWaypointAtIndex method takes an index as an argument and returns
 * the position of the waypoint at that index. This allows other scripts to
 * easily access the positions of the waypoints in the patrol path.*/

using UnityEngine;

namespace RPG.Controller
{
    public class PatrolController : MonoBehaviour
    {
        [field: SerializeField, Range(0f, 0.5f)] public float WaypointSize { get; set; }

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

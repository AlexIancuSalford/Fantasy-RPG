using UnityEngine;

namespace RPG.Controller
{

    /// <summary>
    /// This script defines a PatrolController class in the RPG.Controller
    /// namespace. The class is derived from MonoBehaviour and is designed to be
    /// attached to a game object in a Unity project.
    ///  
    /// The PatrolController class has a serialized field called WaypointSize,
    /// which is a float value that can be edited in the Unity editor. The Range
    /// attribute specifies that this value should be between 0 and 0.5. There is
    /// also a getter and setter for WaypointSize that allow the value to be
    /// accessed and modified from other scripts.
    ///  
    /// The OnDrawGizmos method is a special Unity method that is called when the
    /// gizmos for a game object are drawn in the editor. This method is used to
    /// draw a series of spheres and lines that represent the waypoints in the
    /// patrol path. The method iterates over all the children of the PatrolController game
    /// object, draws a sphere at each child's position with a size specified by
    /// WaypointSize, and then draws a line connecting that child to the next
    /// waypoint in the path.
    ///  
    /// The GetNextWaypointIndex method takes an index as an argument and returns
    /// the index of the next waypoint in the path. If the provided index is the
    /// last waypoint in the path, the method returns the index of the first
    /// waypoint. This allows the path to be treated as a loop.
    ///  
    /// The GetWaypointAtIndex method takes an index as an argument and returns
    /// the position of the waypoint at that index. This allows other scripts to
    /// easily access the positions of the waypoints in the patrol path.
    /// </summary>
    public class PatrolController : MonoBehaviour
    {
        [field: SerializeField, Range(0f, 0.5f)] public float WaypointSize { get; set; }

        /// <summary>
        /// This method is called by Unity at certain times to draw visualizations of objects in the scene view.
        /// It iterates through all the child objects of the patrol object and draws a sphere at the position of each child object.
        /// It also draws a line between the current child object and the next child object in the list of children.
        /// </summary>
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                // Get the index of the next waypoint in the list
                int j = GetNextWaypointIndex(i);

                // Draw a sphere at the current waypoint's position
                Gizmos.DrawSphere(transform.GetChild(i).position, WaypointSize);
                // Draw a line between the current and next waypoints
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(j).position);
            }
        }

        /// <summary>
        /// Returns the index of the next waypoint in the list of child waypoints.
        /// If the provided index is the last waypoint in the list, the method wraps around to the beginning of the list.
        /// </summary>
        /// <param name="index">The current index of the waypoint in the list.</param>
        /// <returns>The index of the next waypoint in the list.</returns>
        public int GetNextWaypointIndex(int index)
        {
            return (index + 1) % transform.childCount;
        }

        /// <summary>
        /// Returns the position of the child waypoint at the provided index.
        /// </summary>
        /// <param name="index">The index of the waypoint in the list of child waypoints.</param>
        /// <returns>The position of the waypoint at the provided index.</returns>
        public Vector3 GetWaypointAtIndex(int index)
        {
            return transform.GetChild(index).position;
        }
    }
}

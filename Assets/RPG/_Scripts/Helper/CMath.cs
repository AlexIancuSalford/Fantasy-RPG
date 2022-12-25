using UnityEngine;
using UnityEngine.AI;

namespace RPG.Helper
{
    /// <summary>
    /// This script defines a static class called CMath with a single method called CalculatePathLength.
    /// The method takes a NavMeshPath object as a parameter and returns a float value representing the length of the path.
    /// 
    /// The NavMeshPath object represents a path that can be followed by a character on a NavMesh, which is a
    /// virtual mesh that is used to represent the walkable surface of a game level. NavMesh paths are typically
    /// used for AI characters to navigate through a game level.
    /// 
    /// The CalculatePathLength method iterates over the elements of the path.corners array, which represents a series of
    /// points that make up the path. It calculates the distance between each consecutive pair of points using the
    /// Vector3.Distance method, and adds up these distances to get the total length of the path. Finally, it returns the path length.
    /// </summary>
    public static class CMath
    {
        /// <summary>
        /// This method calculates length of a NavMeshMath path.
        ///
        /// This method assumes that the path has a status of NavMeshPathStatus.PathComplete.
        /// Make sure the NavMeshPaths' status is NavMeshPathStatus.PathComplete before using this
        /// method to calculates its length
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static float CalculatePathLength(NavMeshPath path)
        {
            float pathLength = 0f;

            // The length of the path is calculated by iterating over the elements of the array and
            // adding up the distances between the points using the Vector3.Distance method
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                pathLength += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return pathLength;
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

namespace RPG.Helper
{
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

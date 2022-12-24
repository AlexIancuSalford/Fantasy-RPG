using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [field: SerializeField] public Transform Target { get; set; }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = Target.position;
        }
    }
}

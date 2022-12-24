using UnityEngine;

namespace RPG.Core
{
    public class FaceCamera : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}

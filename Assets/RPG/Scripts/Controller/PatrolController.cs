using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Controller
{
    [ExecuteInEditMode]
    public class PatrolController : MonoBehaviour
    {
        [field: SerializeField, Range(0f, 0.5f)] public float WaypointSize { get; set; }

        void Awake()
        {
            Debug.Log(WaypointSize);
        }

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
                Gizmos.DrawSphere(transform.GetChild(i).position, WaypointSize);
            }
        }
    }
}

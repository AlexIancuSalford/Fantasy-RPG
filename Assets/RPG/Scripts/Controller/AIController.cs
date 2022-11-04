using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        [field: SerializeField] public float ChaseRange { get; set; } = 6.0f;

        private GameObject _player;

        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (IsPlayerInRange())
            {
                Debug.Log($"Player is in range of {gameObject.name}");
            }
        }

        private bool IsPlayerInRange()
        {
            if (_player != null)
            {
                return Vector3.Distance(_player.transform.position, gameObject.transform.position) <= ChaseRange;
            }

            return false;
        }
    }
}

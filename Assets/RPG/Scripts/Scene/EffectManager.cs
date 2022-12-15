using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Scene
{
    public class EffectManager : MonoBehaviour
    {
        [field: SerializeField] private GameObject persistentObject { get; set; }

        private static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned)
            {
                return;
            }

            SpawnObjects();

            hasSpawned = true;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void SpawnObjects()
        {
            GameObject persistentObj = Instantiate(persistentObject);
            DontDestroyOnLoad(persistentObj);
        }
    }
}

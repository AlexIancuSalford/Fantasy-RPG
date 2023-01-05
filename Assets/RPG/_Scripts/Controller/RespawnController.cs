using System;
using System.Collections;
using Cinemachine;
using RPG.Attributes;
using RPG.Scene;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Controller
{
    public class RespawnController : MonoBehaviour
    {
        [field : SerializeField] private Transform RespawnPosition { get; set; } = null;
        [field: SerializeField] private float RespawnDelay { get; set; }
        [field: SerializeField] private float FadeTime { get; set; }
        [field: SerializeField] private float HealthRegenPercentage { get; set; }

        private void Awake()
        {
            GetComponent<Health>().OnDie.AddListener(RespawnPlayer);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (GetComponent<Health>().IsDead())
            {
                RespawnPlayer();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void RespawnPlayer()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            FadeEffect fadeEffect = FindObjectOfType<FadeEffect>();
            SaveWrapper saveWrapper = FindObjectOfType<SaveWrapper>();

            saveWrapper.Save();
            yield return new WaitForSeconds(RespawnDelay);
            yield return fadeEffect.FadeOut(FadeTime);
            GetComponent<NavMeshAgent>().Warp(RespawnPosition.position);
            GetComponent<Health>().RegenHealth(HealthRegenPercentage);
            ResetEnemies();
            saveWrapper.Save();
            yield return fadeEffect.FadeIn(FadeTime);

            ICinemachineCamera camera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if (camera.Follow == transform)
            {
                camera.OnTargetObjectWarped(transform, RespawnPosition.position - transform.position);
            }
        }

        private void ResetEnemies()
        {
            foreach (AIController enemy in FindObjectsOfType<AIController>())
            {
                enemy.Reset();
                enemy.GetComponent<Health>()?.RegenHealth(80);
            }
        }
    }
}

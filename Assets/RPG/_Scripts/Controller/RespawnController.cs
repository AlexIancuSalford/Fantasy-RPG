using System.Collections;
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

            yield return new WaitForSeconds(RespawnDelay);
            yield return fadeEffect.FadeOut(FadeTime);
            GetComponent<NavMeshAgent>().Warp(RespawnPosition.position);
            GetComponent<Health>().RegenHealth(HealthRegenPercentage);
            yield return fadeEffect.FadeIn(FadeTime);
        }
    }
}

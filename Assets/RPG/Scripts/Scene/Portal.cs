using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
    public class Portal : MonoBehaviour
    {
        public enum DestinationId
        {
            A, B, C, D, E, F, G, H
        }

        [field : SerializeField] public string SceneToLoad { get; set; }
        [field: SerializeField] public DestinationId DestinationPortal { get; set; }
        [field: SerializeField] public float FadeOutTime { get; set; } = 1f;
        [field: SerializeField] public float FadeInTime { get; set; } = 2f;
        [field: SerializeField] public float FadeWaitTime { get; set; } = .5f;

        private Transform _spawnPoint;

        private void Awake()
        {
            _spawnPoint = this.transform.GetChild(0);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(LoadScene());
            }
        }

        private IEnumerator LoadScene()
        {
            FadeEffect fadeEffect = FindObjectOfType<FadeEffect>();

            DontDestroyOnLoad(gameObject);

            yield return fadeEffect.FadeOut(FadeOutTime);
            yield return SceneManager.LoadSceneAsync(SceneToLoad);

            Portal otherPortal = GetOtherPortal();
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
            player.transform.rotation = otherPortal._spawnPoint.rotation;

            yield return new WaitForSeconds(FadeWaitTime);
            yield return fadeEffect.FadeIn(FadeInTime);

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) { continue; }
                if (portal.DestinationPortal != DestinationPortal) { continue; }

                return portal;
            }

            return null;
        }
    }
}

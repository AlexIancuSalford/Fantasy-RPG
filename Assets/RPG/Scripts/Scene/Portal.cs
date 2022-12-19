using System.Collections;
using RPG.Save;
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
            SaveWrapper saveWrapper = FindObjectOfType<SaveWrapper>();

            DontDestroyOnLoad(gameObject);

            yield return fadeEffect.FadeOut(FadeOutTime);
            saveWrapper.Save();
            yield return SceneManager.LoadSceneAsync(SceneToLoad);
            saveWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
            player.transform.rotation = otherPortal._spawnPoint.rotation;

            saveWrapper.Save();

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

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
    public class Portal : MonoBehaviour
    {
        [field : SerializeField] public string SceneToLoad { get; set; }

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
            DontDestroyOnLoad(this);
            yield return SceneManager.LoadSceneAsync(SceneToLoad);

            Portal otherPortal = GetOtherPortal();
            GameObject player = GameObject.FindWithTag("Player");

            player.transform.position = otherPortal._spawnPoint.position;
            player.transform.rotation = otherPortal._spawnPoint.rotation;

            Destroy(this);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) { continue; }

                return portal;
            }

            return null;
        }
    }
}

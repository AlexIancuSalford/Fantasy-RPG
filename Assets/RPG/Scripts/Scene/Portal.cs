using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
    public class Portal : MonoBehaviour
    {
        [field: SerializeField] public string SceneToLoad { get; set; }

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
            if (other.tag.Equals("Player"))
            {
                StartCoroutine(LoadScene());
            }
        }

        private IEnumerator LoadScene()
        {
            DontDestroyOnLoad(this);
            yield return SceneManager.LoadSceneAsync(SceneToLoad);
            Debug.Log("Scene Loaded");
            Destroy(this);
        }
    }
}

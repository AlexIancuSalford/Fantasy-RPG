using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
    public class Portal : MonoBehaviour
    {
        [field : SerializeField] public string SceneToLoad { get; set; }

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
            Debug.Log("Collision");

            if (other.tag.Equals("Player"))
            {
                SceneManager.LoadScene(SceneToLoad);
            }
        }
    }
}

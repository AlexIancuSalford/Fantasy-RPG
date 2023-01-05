using UnityEngine;

namespace RPG.UI.Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        [field : SerializeField] public GameObject Entry { get; set; } = null;

        private void Start()
        {
            SwitchTo(Entry);
        }

        public void SwitchTo(GameObject gameObject)
        {
            if (gameObject.transform.parent != transform) { return; }

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(child.gameObject == gameObject);
            }
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}

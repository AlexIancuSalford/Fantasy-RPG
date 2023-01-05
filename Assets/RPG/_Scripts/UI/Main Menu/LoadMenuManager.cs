using UnityEngine;

namespace RPG.UI.Menu
{
    public class LoadMenuManager : MonoBehaviour
    {
        [field : SerializeField] public Transform Root { get; private set; } = null;
        [field : SerializeField] public GameObject ButtonPrefab { get; private set; } = null;

        private void OnEnable()
        {
            foreach (Transform child in Root)
            {
                Destroy(child.gameObject);
            }

            Instantiate(ButtonPrefab, Root);
            Instantiate(ButtonPrefab, Root);
            Instantiate(ButtonPrefab, Root);
            Instantiate(ButtonPrefab, Root);
        }
    }
}

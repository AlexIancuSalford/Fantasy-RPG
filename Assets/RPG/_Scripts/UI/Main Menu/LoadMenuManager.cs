using RPG.Scene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menu
{
    public class LoadMenuManager : MonoBehaviour
    {
        [field : SerializeField] public Transform Root { get; private set; } = null;
        [field : SerializeField] public GameObject ButtonPrefab { get; private set; } = null;

        private void OnEnable()
        {
            SaveWrapper saveWrapper = FindObjectOfType<SaveWrapper>();
            if (saveWrapper == null) { return; }

            foreach (Transform child in Root)
            {
                Destroy(child.gameObject);
            }

            foreach (string save in saveWrapper.SaveList())
            {
                GameObject instance = Instantiate(ButtonPrefab, Root);
                instance.GetComponentInChildren<TextMeshProUGUI>().text = save;
                instance.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    saveWrapper.LoadGame(save);
                });
            }
        }
    }
}

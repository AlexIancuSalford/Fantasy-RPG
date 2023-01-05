using RPG.Scene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menu
{
    /// <summary>
    /// This script is a simple script that is used to manage a load menu in Unity. The script is attached to a GameObject,
    /// and it has three serialized fields:
    /// 
    /// Root: This is a Transform component that represents the root of the menu. It is likely that this Transform has several
    /// child GameObjects that represent the individual save game buttons.
    ///
    /// ButtonPrefab: This is a GameObject that serves as a prefab for the individual save game buttons.
    ///
    /// saveWrapper: This is a reference to the SaveWrapper script in the scene.
    ///
    /// The script has a single method, OnEnable, which is called when the GameObject that this script is attached to is enabled
    /// (e.g., when the load menu is displayed). The method first checks if a SaveWrapper component exists in the scene. If it
    /// does not, the method exits early. If a SaveWrapper component does exist, the method first clears the menu by destroying
    /// all the child GameObjects of the Root Transform. Next, the method iterates over all the save games in the SaveWrapper's
    /// save list, instantiates a new button for each save game, sets the text of the button to the name of the save game, and
    /// adds a listener to the button's onClick event that calls the LoadGame method of the SaveWrapper and passes in the name
    /// of the save game.
    /// </summary>
    public class LoadMenuManager : MonoBehaviour
    {
        /// <summary>
        /// The root of the menu.
        /// </summary>
        [field : SerializeField] public Transform Root { get; private set; } = null;

        /// <summary>
        /// A prefab for the individual save game buttons.
        /// </summary>
        [field : SerializeField] public GameObject ButtonPrefab { get; private set; } = null;

        private void OnEnable()
        {
            // Get the SaveWrapper script in the scene.
            SaveWrapper saveWrapper = FindObjectOfType<SaveWrapper>();
            if (saveWrapper == null) { return; }

            // Clear the menu by destroying all child GameObjects of the Root Transform.
            foreach (Transform child in Root)
            {
                Destroy(child.gameObject);
            }

            // Iterate over all save games in the SaveWrapper's save list.
            foreach (string save in saveWrapper.SaveList())
            {
                // Instantiate a new button for the save game.
                GameObject instance = Instantiate(ButtonPrefab, Root);
                // Set the text of the button to the name of the save game.
                instance.GetComponentInChildren<TextMeshProUGUI>().text = save;
                // Add a listener to the button's onClick event that calls the LoadGame method of the SaveWrapper and passes in the name of the save game.
                instance.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    saveWrapper.LoadGame(save);
                });
            }
        }
    }
}

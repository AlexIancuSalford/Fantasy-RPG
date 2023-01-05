using UnityEngine;

namespace RPG.UI.Menu
{
    /// <summary>
    /// This script defines a class called MainMenuManager that extends MonoBehaviour which is a base class for
    /// components that need to be updated every frame.
    /// 
    /// The MainMenuManager class has a single field called Entry that is serialized and has a public getter and
    /// setter. The Entry field is of type GameObject and is set to null by default.
    /// 
    /// The Start method is a special method in Unity that is called when the script component is enabled, after
    /// all the other components have been enabled. In this case, the Start method calls the SwitchTo method and
    /// passes the Entry field as an argument.
    /// 
    /// The SwitchTo method takes a GameObject as an argument and sets the active property of all child objects
    /// of the transform object to false, except for the child object that is passed as an argument. This has the
    /// effect of deactivating all child objects of the transform object except for the one that is passed to the
    /// SwitchTo method.
    /// 
    /// The QuitGame method is called when the player wants to quit the game. In the editor, it sets the isPlaying
    /// property of the EditorApplication to false, which stops the game in the editor. In a build, it calls the
    /// Application.Quit() method, which quits the game.
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        /// <summary>
        /// Entry is a serialized field of type GameObject that specifies the initial menu to display.
        /// </summary>
        [field : SerializeField] public GameObject Entry { get; set; } = null;

        /// <summary>
        /// Start is a special method in Unity that is called when the script component is enabled,
        /// after all the other components have been enabled.
        /// It calls the SwitchTo method and passes the Entry field as an argument.
        /// </summary>
        private void Start()
        {
            SwitchTo(Entry);
        }

        /// <summary>
        /// SwitchTo is a method that takes a GameObject as an argument and sets the active property
        /// of all child objects of the transform object to false, except for the child object that is
        /// passed as an argument. This has the effect of deactivating all child objects of the transform
        /// object except for the one that is passed to the SwitchTo method.
        /// </summary>
        /// <param name="gameObject">The GameObject to activate.</param>
        public void SwitchTo(GameObject gameObject)
        {
            if (gameObject.transform.parent != transform) { return; }

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(child.gameObject == gameObject);
            }
        }

        /// <summary>
        /// QuitGame is a method that is called when the player wants to quit the game.
        /// In the editor, it sets the isPlaying property of the EditorApplication to false,
        /// which stops the game in the editor. In a build, it calls the Application.Quit() method,
        /// which quits the game.
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}

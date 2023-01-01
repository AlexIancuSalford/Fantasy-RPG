using RPG.Attributes;
using RPG.Controller;
using RPG.Helper;
using UnityEngine;

namespace RPG.Dialogue
{
    /// <summary>
    /// This script is a Unity component that adds dialogue functionality to a game object (an NPC in this case). When the object is clicked,
    /// the script starts a dialogue with the object. The dialogue is determined by the Dialogue property of the script, which should be set
    /// in the Unity editor.
    /// 
    /// The script also implements the IRaycastable interface, which means that it has a method HandleRaycast that takes a PlayerController
    /// as an argument and returns a boolean. This method is called when the object the script is attached to is clicked, and the boolean
    /// returned indicates whether the click was successfully handled or not.
    /// 
    /// The script also has a Name property which is a string representing the name of the object, and a GetCursorType method which returns a
    /// CursorType enum value. The GetCursorType method is likely used to set the cursor icon when the mouse hovers over the object.
    /// 
    /// The script also has a Health component which is used to check if the object is dead. If the object is dead, the HandleRaycast method
    /// returns false, indicating that the click was not successfully handled or that another cursor should be set instead.
    /// </summary>
    public class AIConversation : MonoBehaviour, IRaycastable
    {
        /// <summary>
        /// The name of the object. This should be set in the Unity editor.
        /// </summary>
        [field : SerializeField] public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// The dialogue object for this object. This should be set in the Unity editor.
        /// </summary>
        [field : SerializeField] private Dialogue Dialogue { get; set; } = null;

        /// <summary>
        /// The Health component of this object.
        /// </summary>
        private Health health = null;

        private void Awake()
        {
            // Get the Health component of this object
            health = gameObject.GetComponent<Health>();
        }

        /// <summary>
        /// Gets the cursor type for this object.
        /// </summary>
        /// <returns>The cursor type for this object.</returns>
        public CursorHelper.CursorType GetCursorType()
        {
            return CursorHelper.CursorType.Dialogue;
        }

        /// <summary>
        /// Handles a raycast from the player. If the left mouse button is clicked and the object is not dead,
        /// starts a dialogue with the object.
        /// </summary>
        /// <param name="caller">The PlayerController that initiated the raycast.</param>
        /// <returns>True if the raycast was successfully handled, false otherwise.</returns>
        public bool HandleRaycast(PlayerController caller)
        {
            // Return false if the object (NPC) is dead
            if (health.IsDead) { return false; }

            if (Dialogue == null)
            {
                Debug.LogError("The AI does not have a Dialogue Object");
                return false;
            }

            // If the left mouse button is clicked, start the dialogue
            if (Input.GetMouseButtonDown(0))
            {
                caller.GetComponent<Conversation>().StartDialogue(this, Dialogue);
            }

            return true;
        }
    }
}

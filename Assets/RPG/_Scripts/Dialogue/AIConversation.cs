using RPG.Controller;
using RPG.Helper;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversation : MonoBehaviour, IRaycastable
    {
        [field : SerializeField] private Dialogue Dialogue { get; set; } = null;

        public CursorHelper.CursorType GetCursorType()
        {
            return CursorHelper.CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController caller)
        {
            if (Dialogue == null)
            {
                Debug.LogError("The AI does not have a Dialogue Object");
                return false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                caller.GetComponent<Conversation>().StartDialogue(this, Dialogue);
            }

            return true;
        }
    }
}

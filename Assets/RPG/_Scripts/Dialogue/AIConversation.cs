using RPG.Attributes;
using RPG.Controller;
using RPG.Helper;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversation : MonoBehaviour, IRaycastable
    {
        [field : SerializeField] public string Name { get; private set; } = string.Empty;
        [field : SerializeField] private Dialogue Dialogue { get; set; } = null;

        private Health health = null;

        private void Awake()
        {
            health = gameObject.GetComponent<Health>();
        }

        public CursorHelper.CursorType GetCursorType()
        {
            return CursorHelper.CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController caller)
        {
            if (health.IsDead) { return false; }

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

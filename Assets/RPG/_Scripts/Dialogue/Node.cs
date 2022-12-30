using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class Node : ScriptableObject
    {
        [field : SerializeField] private List<string> NodeChildren { get; set; } = new List<string>();
        [field: SerializeField] public string Text { get; private set; } = string.Empty;

        [SerializeField] private Rect rectPosition = new Rect(0, 0, 200, 100);

        public Rect GetRect()
        {
            return rectPosition;
        }

        public void SetRectPosition(Vector2 newPosition)
        {
#if UNITY_EDITOR
            // Record an undo operation for moving the node
            Undo.RecordObject(this, "Move Dialogue Node");
#endif
            rectPosition.position = newPosition;
        }

        public void SetText(string newText)
        {
            if (newText != Text)
            {

#if UNITY_EDITOR
                // Record undo operation
                Undo.RecordObject(this, "Update Dialog Text");
#endif
                Text = newText;
            }
        }

        public List<string> GetNodeChildren()
        {
            return NodeChildren;
        }

        public void AddNodeChild(string childID)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Add Dialogue Link");
#endif
            NodeChildren.Add(childID);
        }

        public void RemoveNodeChild(string childID)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Remove Dialogue Link");
#endif
            NodeChildren.Remove(childID);
        }

        public enum ActionType
        {
            Add,
            Delete,
        }
    }
}
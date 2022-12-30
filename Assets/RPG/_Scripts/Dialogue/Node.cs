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
            rectPosition.position = newPosition;
            EditorUtility.SetDirty(this);
#endif
        }

        public void SetText(string newText)
        {
            if (newText != Text)
            {

#if UNITY_EDITOR
                // Record undo operation
                Undo.RecordObject(this, "Update Dialog Text");
                Text = newText;
                EditorUtility.SetDirty(this);
#endif
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
            NodeChildren.Add(childID);
            EditorUtility.SetDirty(this);
#endif
        }

        public void RemoveNodeChild(string childID)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Remove Dialogue Link");
            NodeChildren.Remove(childID);
            EditorUtility.SetDirty(this);
#endif
        }

        public enum ActionType
        {
            Add,
            Delete,
        }
    }
}
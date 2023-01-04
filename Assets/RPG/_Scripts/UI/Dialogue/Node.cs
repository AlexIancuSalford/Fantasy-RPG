using RPG.UI.Quest;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static RPG.Dialogue.DialogueEnums;

namespace RPG.Dialogue
{
    public class Node : ScriptableObject
    {
        /// <summary>
        /// List of child node IDs that are connected to this node.
        /// </summary>
        [field : SerializeField] private List<string> NodeChildren { get; set; } = new List<string>();

        /// <summary>
        /// Dialogue text displayed when this node is reached in the dialogue tree.
        /// </summary>
        [field : SerializeField] public string Text { get; private set; } = string.Empty;

        /// <summary>
        /// Enum indicating whether the player character or another character is speaking the dialogue in this node.
        /// </summary>
        [field : SerializeField] public Speaker CurrentSpeaker { get; set; } = Speaker.Other;

        /// <summary>
        /// Rectangle representing the position and size of a graphical element that represents the node in the dialogue tree editor.
        /// </summary>
        [SerializeField] private Rect rectPosition = new Rect(0, 0, 200, 100);

        [field : SerializeField] public DialogueAction OnEnterAction { get; private set; } = DialogueAction.None;
        [field : SerializeField] public DialogueAction OnExitAction { get; private set; } = DialogueAction.None;
        [field : SerializeField] public Condition Condition { get; private set; } = null;

        /// <summary>
        /// Returns the rectPosition field.
        /// </summary>
        public Rect GetRect()
        {
            return rectPosition;
        }

        /// <summary>
        /// Sets the position of the rectPosition field. 
        /// This method also records an undo operation for moving the node when running in the Unity editor.
        /// </summary>
        public void SetRectPosition(Vector2 newPosition)
        {
#if UNITY_EDITOR
            // Record an undo operation for moving the node
            Undo.RecordObject(this, "Move Dialogue Node");
            rectPosition.position = newPosition;
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Sets the Text property. 
        /// This method also records an undo operation for updating the text when running in the Unity editor.
        /// </summary>
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

        /// <summary>
        /// Returns the NodeChildren property.
        /// </summary>
        public List<string> GetNodeChildren()
        {
            return NodeChildren;
        }

        /// <summary>
        /// Adds a child node ID to the NodeChildren list.
        /// This method also records an undo operation for adding a link when running in the Unity editor.
        /// </summary>
        public void AddNodeChild(string childID)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Add Dialogue Link");
            NodeChildren.Add(childID);
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Removes a child node ID from the NodeChildren list.
        /// This method also records an undo operation for removing a link when running in the Unity editor.
        /// </summary>
        public void RemoveNodeChild(string childID)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Remove Dialogue Link");
            NodeChildren.Remove(childID);
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Sets the CurrentSpeaker property.
        /// This method also records an undo operation for changing the speaker when running in the Unity editor.
        /// </summary>
        public void SetSpeaker(Speaker newSpeaker)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Change Dialogue Speaker");
            CurrentSpeaker = newSpeaker;
            EditorUtility.SetDirty(this);
#endif
        }

        public bool CheckCondition(IEnumerable<IEvaluator> enumerable)
        {
            return Condition.CheckEvaluators(enumerable);
        }
    }
}
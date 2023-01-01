using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static RPG.Dialogue.DialogueEnums;

namespace RPG.Dialogue
{
    /// <summary>
    /// This script defines a class called "Node" that is used in the dialogue system. The class is marked as a ScriptableObject,
    /// which means it can be saved as an asset file in Unity and accessed from other scripts.
    /// 
    /// The Node class has several properties and methods related to managing a node in the dialogue tree.
    /// 
    /// The NodeChildren property is a list of strings that represent the child nodes that are connected to this node.
    ///
    /// The Text property is a string that holds the dialogue text that is displayed when this node is reached in the dialogue tree.
    ///
    /// The CurrentSpeaker property is an enumeration (a set of named constants) that indicates whether the player character or
    /// another character is speaking the dialogue in this node.
    ///
    /// The rectPosition field is a Unity Rect object that represents the position and size of a graphical element that represents
    /// the node in the dialogue tree editor.
    ///
    /// The Node class also has several methods for modifying its properties:
    /// 
    /// The GetRect method returns the rectPosition field.
    ///
    /// The SetRectPosition method allows the position of the rectPosition field to be set. This method also contains a #if UNITY_EDITOR block,
    /// which means it will only be executed when the code is running in the Unity editor (not when the game is built and run). This block uses
    /// the Undo class to record an undo operation for moving the node, and sets the rectPosition field to the new position.
    ///
    /// The SetText method allows the Text property to be set, and also contains an #if UNITY_EDITOR block that uses the Undo class to record an
    /// undo operation when the text is updated.
    ///
    /// The GetNodeChildren method returns the NodeChildren property.
    ///
    /// The AddNodeChild and RemoveNodeChild methods allow child nodes to be added or removed from the NodeChildren list, and both contain
    /// #if UNITY_EDITOR blocks that use the Undo class to record undo operations for adding or removing links.
    ///
    /// The SetSpeaker method allows the CurrentSpeaker property to be set, and contains an #if UNITY_EDITOR block that uses the Undo class to
    /// record an undo operation when the speaker is changed.
    /// </summary>
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
    }
}
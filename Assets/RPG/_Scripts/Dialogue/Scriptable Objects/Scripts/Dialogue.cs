using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace RPG.Dialogue
{
    /// <summary>
    /// This is a script for a dialogue system. It is a scriptable object that can be used to create dialogue
    /// between characters in a game.
    /// 
    /// The script has a list of nodes called "Nodes" which represent the different lines of dialogue. It also
    /// has a couple of Vector2 variables, "DefaultCanvasSize" and "NewNodeOffset", which are used to set the
    /// size of the canvas that the dialogue nodes are displayed on and the offset between newly created nodes,
    /// respectively.
    /// 
    /// The script also has a dictionary called "nodesByUUID" which stores nodes by their unique identifier (UUID).
    /// It has a method called "BuildDictionary" which builds this dictionary by iterating through the list of nodes
    /// and adding them to the dictionary using their UUID as the key.
    /// 
    /// There are also methods for creating and deleting nodes, as well as a method for retrieving all the children
    /// of a given node.
    /// 
    /// The script implements the "ISerializationCallbackReceiver" interface, which allows it to perform certain
    /// actions before and after serialization (the process of converting the object's data into a format that can be
    /// stored or transmitted). In this case, the "OnBeforeSerialize" method is called before serialization and the
    /// "OnAfterDeserialize" method is called after deserialization (the process of converting serialized data back
    /// into an object).
    /// 
    /// There is also a "#if UNITY_EDITOR" directive at the beginning of the script, which means that the code within
    /// it will only be compiled when the script is being run in the Unity editor. This is useful for code that is
    /// only needed in the editor and not when the game is actually being played.
    /// </summary>
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Object/Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        /// <summary>
        /// A list of dialogue nodes.
        /// </summary>
        [field : SerializeField] public List<Node> Nodes { get; private set; } = new List<Node>();

        /// <summary>
        /// The default size of the canvas on which the dialogue nodes are displayed.
        /// </summary>
        [field : SerializeField] public Vector2 DefaultCanvasSize { get; private set; } = new Vector2(4000, 4000);

        /// <summary>
        /// The offset between newly created nodes.
        /// </summary>
        [field: SerializeField] public Vector2 NewNodeOffset { get; private set; } = new Vector2(250, 0);

        /// <summary>
        /// Add a field to store the dictionary
        /// </summary>
        private Dictionary<string, Node> nodesByUUID = new Dictionary<string, Node>();

        /// <summary>
        /// Use OnValidate to build the dictionary when the scriptable object is changed in the Unity editor
        /// </summary>
        private void OnValidate()
        {
            BuildDictionary();
        }

        /// <summary>
        /// Returns an enumerable collection of all the children of the specified parent node.
        /// </summary>
        /// <param name="parentNode">The parent node whose children are to be retrieved.</param>
        /// <returns>An enumerable collection of all the children of the specified parent node.</returns>
        public IEnumerable<Node> GetAllNodeChildren(Node parentNode)
        {
            // Check if the dictionary has been initialized
            if (nodesByUUID == null)
            {
                // If not, build the dictionary
                BuildDictionary();
            }

            if (parentNode.GetNodeChildren() == null)
            {
                yield break;
            }

            // Use the dictionary to look up the nodes by their name (UUID)
            foreach (string childID in parentNode.GetNodeChildren())
            {
                if (nodesByUUID.TryGetValue(childID, out var childNode))
                {
                    // Yield return each child node
                    yield return childNode;
                }
            }
        }

        /// <summary>
        /// Builds the dictionary of nodes by UUID.
        /// </summary>
        private void BuildDictionary()
        {
            // Clear the dictionary
            if (nodesByUUID != null) { nodesByUUID.Clear(); }
            else { nodesByUUID = new Dictionary<string, Node>(); }

            foreach (var node in Nodes.Where(node => node != null))
            {
                nodesByUUID[node.name] = node;
            }
        }

        /// <summary>
        /// Creates a new dialogue node and adds it to the list of nodes.
        /// </summary>
        /// <param name="parent">The parent node of the new node. Can be null if the new node has no parent.</param>
        public void CreateNode(Node parent)
        {
            Node node = CreateInstance<Node>();
            node.name = Guid.NewGuid().ToString();
#if UNITY_EDITOR
            // Record the creation of the new node as an undo operation
            Undo.RegisterCreatedObjectUndo(node, "Created Dialogue Node");
#endif

            if (parent != null)
            {
                // Set the new node as a child of the parent node
                parent.AddNodeChild(node.name);

                // Set the speaker of the new node to be the opposite of the parent node's speaker
                Node.Speaker newSpeaker = parent.CurrentSpeaker switch
                {
                    Node.Speaker.Player => Node.Speaker.Other,
                    Node.Speaker.Other => Node.Speaker.Player,
                    _ => Node.Speaker.Other
                };
                node.SetSpeaker(newSpeaker);

                // Set the position of the new node to be offset from the parent node's position
                node.SetRectPosition(parent.GetRect().position + NewNodeOffset);
            }

#if UNITY_EDITOR
            // Record an undo operation, so the add or delete operation can be undone by CTRL-Z
            Undo.RecordObject(this, "Add/Delete Dialog Node");
#endif
            // Add the new node to the list of nodes
            Nodes.Add(node);
            // Rebuild the dictionary of nodes by UUID
            OnValidate();
        }

        /// <summary>
        /// Deletes a dialogue node from the list of nodes.
        /// </summary>
        /// <param name="node">The node to be deleted.</param>
        public void DeleteNode(Node node)
        {
#if UNITY_EDITOR
            // Record the deletion of the node as an undo operation
            Undo.RecordObject(this, "Delete Dialogue node");
#endif
            // Remove the node from the list of nodes
            Nodes.Remove(node);
            // Rebuild the dictionary of nodes by UUID
            OnValidate();

            // Remove the node from the list of children for all other nodes
            foreach (Node listNode in Nodes)
            {
                listNode.GetNodeChildren().Remove(node.name);
            }
#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(node);
#endif
        }

        /// <summary>
        /// Called before serialization.
        /// </summary>
        public void OnBeforeSerialize()
        {
            // If the list of nodes is empty, create a new node
            if (Nodes.Count == 0)
            {
                Node newNode = CreateInstance<Node>();
                newNode.name = Guid.NewGuid().ToString();
                Nodes.Add(newNode);
                OnValidate();
            }

#if UNITY_EDITOR
            // If the script is being run in the Unity editor, refresh the asset database
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (Node node in Nodes)
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        /// <summary>
        /// Called after deserialization.
        /// </summary>
        public void OnAfterDeserialize()
        {
            
        }
    }
}

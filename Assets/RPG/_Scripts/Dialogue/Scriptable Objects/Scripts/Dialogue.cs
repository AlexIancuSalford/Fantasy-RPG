using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Object/Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [field : SerializeField] public List<Node> Nodes { get; private set; } = new List<Node>();
        [field : SerializeField] public Vector2 DefaultCanvasSize { get; private set; } = new Vector2(4000, 4000);
        [field: SerializeField] public Vector2 NewNodeOffset { get; private set; } = new Vector2(250, 0);

        // Add a field to store the dictionary
        private Dictionary<string, Node> nodesByUUID = new Dictionary<string, Node>();

        // Use OnValidate to build the dictionary when the scriptable object is changed in the Unity editor
        private void OnValidate()
        {
            BuildDictionary();
        }

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

            // Use the dictionary to look up the nodes by their UUID
            foreach (string childID in parentNode.GetNodeChildren())
            {
                Node childNode;
                if (nodesByUUID.TryGetValue(childID, out childNode))
                {
                    // Yield return each child node
                    yield return childNode;
                }
            }
        }

        // Add a method to build the dictionary
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

        public void CreateNode(Node parent)
        {
            Node node = CreateInstance<Node>();
            node.name = Guid.NewGuid().ToString();
#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(node, "Created Dialogue Node");
#endif

            if (parent != null)
            {
                parent.AddNodeChild(node.name);
                Node.Speaker newSpeaker = parent.CurrentSpeaker switch
                {
                    Node.Speaker.Player => Node.Speaker.Other,
                    Node.Speaker.Other => Node.Speaker.Player,
                    _ => Node.Speaker.Other
                };
                node.SetSpeaker(newSpeaker);
                node.SetRectPosition(parent.GetRect().position + NewNodeOffset);
            }

#if UNITY_EDITOR
            // Record an undo operation, so the add or delete operation can be undone by CTRL-Z
            Undo.RecordObject(this, "Add/Delete Dialog Node");
#endif

            Nodes.Add(node);
            OnValidate();
        }

        public void DeleteNode(Node node)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Delete Dialogue node");
#endif
            Nodes.Remove(node);
            OnValidate();

            foreach (Node listNode in Nodes)
            {
                listNode.GetNodeChildren().Remove(node.name);
            }
#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(node);
#endif
        }

        public void OnBeforeSerialize()
        {
            if (Nodes.Count == 0)
            {
                Node newNode = CreateInstance<Node>();
                newNode.name = Guid.NewGuid().ToString();
                Nodes.Add(newNode);
                OnValidate();
            }

#if UNITY_EDITOR
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

        public void OnAfterDeserialize()
        {
            
        }
    }
}

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

        // Add a field to store the dictionary
        private Dictionary<string, Node> nodesByUUID = new Dictionary<string, Node>();

#if UNITY_EDITOR
        private void Awake()
        {
        }
#endif

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

            if (parentNode.NodeChildren == null)
            {
                yield break;
            }

            // Use the dictionary to look up the nodes by their UUID
            foreach (string childID in parentNode.NodeChildren)
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
            Undo.RegisterCreatedObjectUndo(node, "Created Dialogue Node");

            if (parent != null)
            {
                parent.NodeChildren.Add(node.name);
            }

            Nodes.Add(node);
            OnValidate();
        }

        public void DeleteNode(Node node)
        {
            Nodes.Remove(node);
            OnValidate();

            foreach (Node listNode in Nodes)
            {
                listNode.NodeChildren.Remove(node.name);
            }

            Undo.DestroyObjectImmediate(node);
        }

        public void OnBeforeSerialize()
        {
            if (Nodes.Count == 0)
            {
                CreateNode(null);
            }

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
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}

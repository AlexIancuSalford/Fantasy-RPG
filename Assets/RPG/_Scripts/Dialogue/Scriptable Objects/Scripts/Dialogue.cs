using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Object/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [field : SerializeField] public List<Node> Nodes { get; private set; } = new List<Node>();

        // Add a field to store the dictionary
        private Dictionary<string, Node> nodesByUUID = null;

#if UNITY_EDITOR
        private void Awake()
        {
            if (Nodes.Count == 0)
            {
                Nodes.Add(new Node());
            }

            OnValidate();
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

            foreach (Node node in Nodes)
            {
                nodesByUUID[node.UUID] = node;
            }
        }
    }
}

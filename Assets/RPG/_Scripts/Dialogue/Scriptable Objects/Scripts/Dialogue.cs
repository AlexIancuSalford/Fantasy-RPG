using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Object/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [field : SerializeField] public List<Node> Nodes { get; private set; } = new List<Node>();

#if UNITY_EDITOR
        private void Awake()
        {
            if (Nodes.Count == 0)
            {
                Nodes.Add(new Node());
            }
        }
#endif
    }
}

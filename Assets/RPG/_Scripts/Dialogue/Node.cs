using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class Node : ScriptableObject
    {
        [field : SerializeField] public string Text { get; set; } = string.Empty;
        [field : SerializeField] public List<string> NodeChildren { get; private set; } = new List<string>();
        
        [field: SerializeField] public Rect RectPosition = new Rect(0, 0, 200, 100);

        public enum ActionType
        {
            Add,
            Delete,
        }
    }
}
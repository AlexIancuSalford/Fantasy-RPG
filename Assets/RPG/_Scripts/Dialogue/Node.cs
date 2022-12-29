using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class Node
    {
        [field : SerializeField] public string UUID { get; set; } = string.Empty;
        [field : SerializeField] public string Text { get; set; } = string.Empty;
        [field : SerializeField] public string[] NodeChildren { get; private set; } = null;
        
        [field: SerializeField] public Rect RectPosition = new Rect(0, 0, 200, 100);
    }
}
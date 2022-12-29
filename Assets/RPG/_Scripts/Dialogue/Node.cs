using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class Node
    {
        [field : SerializeField] public string UUID { get; private set; } = string.Empty;
        [field: SerializeField] public string Text { get; set; } = string.Empty;
        [field: SerializeField] public string[] NodeChildren { get; private set; } = null;
    }
}
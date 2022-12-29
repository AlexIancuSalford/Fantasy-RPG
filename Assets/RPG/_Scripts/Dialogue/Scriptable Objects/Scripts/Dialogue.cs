using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Object/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [field : SerializeField] private Node[] Nodes { get; set; } = null;
    }
}

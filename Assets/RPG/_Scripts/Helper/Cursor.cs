using UnityEngine;

namespace RPG.Helper
{
    public static class CursorHelper
    {
        public enum CursorType
        {
            None,
            Move,
            Attack,
            UI,
            Pickup,
        }

        [System.Serializable]
        public struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
    }
}

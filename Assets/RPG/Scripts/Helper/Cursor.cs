using System.Collections;
using System.Collections.Generic;
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
        }

        [System.Serializable]
        public struct CursorMapping
        {
            public CursorType type;
        }
    }
}

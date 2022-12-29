using RPG.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    [System.Serializable]
    public struct DropRecord
    {
        public string ItemID;
        public Vector3f Position;
        public int Number;
    }
}

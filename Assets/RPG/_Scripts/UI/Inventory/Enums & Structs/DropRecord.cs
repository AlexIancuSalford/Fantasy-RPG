using RPG.Helper;

namespace RPG.UI.Inventory
{
    [System.Serializable]
    public struct DropRecord
    {
        public string ItemID;
        public Vector3f Position;
        public int Number;
        public int SceneBuildIndex;
    }
}

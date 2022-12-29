using UnityEngine;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script defines a class called EquipableItem that derives from a class called InventoryItem. EquipableItem
    /// is marked with the CreateAssetMenu attribute, which allows instances of this class to be created as assets in the Unity editor.
    /// 
    /// The EquipableItem class has a single serialized field called allowedEquipLocation, which is of type EquipLocation.
    /// EquipLocation is an enumeration (enum) that represents different locations where an item can be equipped (e.g. weapon, armor, etc.).
    /// 
    /// The EquipableItem class also has a public method called GetAllowedEquipLocation that returns the value of the
    /// allowedEquipLocation field. This method can be called to retrieve the allowed equip location for a given EquipableItem instance.
    /// </summary>
    [CreateAssetMenu(menuName = ("Scriptable Object/Eqipable Item"))]
    public class EquipableItem : InventoryItem
    {
        [field : SerializeField] public EquipLocation AllowedEquipLocation { get; private set; } = EquipLocation.Weapon;
    }
}
using UnityEngine;
using TMPro;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This script is a Unity component called ItemTooltip that is used to display information about an InventoryItem in
    /// a user interface. It has two serialized fields, TitleText and BodyText, which are both instances of the TextMeshProUGUI
    /// class and represent UI text elements. The TitleText and BodyText fields are both marked with the
    /// [field : SerializeField] attribute, which allows them to be visible and editable in the Unity inspector but not
    /// accessible from other scripts.
    /// 
    /// The ItemTooltip class has a single public method called Setup that takes an InventoryItem as a parameter.
    /// The Setup method sets the text property of the TitleText and BodyText fields to the DisplayName and description
    /// properties of the InventoryItem, respectively. This causes the DisplayName and description of the InventoryItem to
    /// be displayed in the UI when the ItemTooltip is shown.
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        /// <summary>
        /// The UI text element that displays the DisplayName of the InventoryItem.
        /// </summary>
        [field : SerializeField] private TextMeshProUGUI TitleText { get; set; } = null;

        /// <summary>
        /// The UI text element that displays the description of the InventoryItem.
        /// </summary>
        [field : SerializeField] private TextMeshProUGUI BodyText { get; set; } = null;

        /// <summary>
        /// Sets up the ItemTooltip to display the DisplayName and description of the given InventoryItem.
        /// </summary>
        /// <param name="item">The InventoryItem to display information for.</param>
        public void Setup(InventoryItem item)
        {
            // Set the text of the TitleText UI element to the DisplayName of the InventoryItem
            TitleText.text = item.DisplayName;
            // Set the text of the BodyText UI element to the description of the InventoryItem
            BodyText.text = item.description;
        }
    }
}

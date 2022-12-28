using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// The InventoryItemIcon script is a component that can be added to a Unity GameObject. It is intended to display
    /// an icon representing an InventoryItem object and, optionally, a number indicating how many of the item are
    /// being represented.
    /// 
    /// The script has a couple of serialized fields: TextContainer and ItemNumber. TextContainer is a GameObject that
    /// is used to hold the ItemNumber text element, and ItemNumber is a TextMeshProUGUI component that displays the
    /// number of items being represented.
    /// 
    /// The script also has two public methods: SetItem and SetItem(InventoryItem item, int number).
    /// 
    /// The SetItem method takes an InventoryItem object as an argument and displays the item's icon on the Image
    /// component attached to the GameObject. If the InventoryItem object is null, it disables the Image component.
    /// The method also sets the TextContainer GameObject to be inactive, effectively hiding the ItemNumber text element.
    /// 
    /// The SetItem(InventoryItem item, int number) method is similar to the first SetItem method, but it also takes an
    /// int argument that represents the number of items being represented. If the number is greater than 1, the method
    /// activates the TextContainer GameObject and sets the text of the ItemNumber component to the value of the number
    /// argument. If the number is 1 or less, the method deactivates the TextContainer GameObject, effectively hiding
    /// the ItemNumber text element.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        /// <summary>
        /// A GameObject that is used to hold the ItemNumber text element.
        /// </summary>
        [field : SerializeField] private GameObject TextContainer { get; set; } = null;

        /// <summary>
        /// A TextMeshProUGUI component that displays the number of items being represented.
        /// </summary>
        [field : SerializeField] private TextMeshProUGUI ItemNumber { get; set; } = null;

        /// <summary>
        /// Display the icon for an InventoryItem object and set the TextContainer GameObject to be inactive.
        /// </summary>
        /// <param name="item">The InventoryItem object to be displayed</param>
        public void SetItem(InventoryItem item)
        {
            SetItem(item, 0);
        }

        /// <summary>
        /// Display the icon for an InventoryItem object and, if the number of items is greater than 1, 
        /// display the number and activate the TextContainer GameObject. Otherwise, set the TextContainer
        /// GameObject to be inactive.
        /// </summary>
        /// <param name="item">The InventoryItem object to be displayed</param>
        /// <param name="number">The number of items being represented</param>
        public void SetItem(InventoryItem item, int number)
        {
            Image iconImage = GetComponent<Image>();

            // If the item is null, disable the icon image
            if (item == null)
            {
                iconImage.enabled = false;
            }
            // Otherwise, enable the icon image and set the sprite to the item's icon
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = item.Icon;
            }

            // If the ItemNumber component does not exist, return early
            if (!ItemNumber) { return; }

            // If the number of items is 1 or less, set the TextContainer GameObject to be inactive
            if (number <= 1)
            {
                TextContainer.SetActive(false);
            }
            // Otherwise, activate the TextContainer GameObject and set the ItemNumber text to the number of items
            else
            {
                TextContainer.SetActive(true);
                ItemNumber.text = number.ToString();
            }
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Inventory
{
    /// <summary>
    /// This is a script for spawning a tooltip in the Unity game engine. The script implements the MonoBehaviour,
    /// IPointerEnterHandler, and IPointerExitHandler interfaces, which allow it to receive events related to the
    /// mouse pointer entering or exiting the object that this script is attached to.
    /// 
    /// The script has a serialized field called TooltipPrefab of type GameObject, which is a reference to the
    /// prefab (a template object) for the tooltip. The script also has a private field called tooltip of type GameObject,
    /// which is a reference to the current instance of the tooltip that has been spawned.
    /// 
    /// The script has an abstract method called UpdateTooltip, which takes in a GameObject and is meant to update
    /// the content of the tooltip with specific information. The script also has an abstract method called CanCreateTooltip,
    /// which returns a bool indicating whether it is currently possible to create a tooltip for the object.
    /// 
    /// The script has an event handler for when the object is destroyed or disabled, which calls a ClearTooltip method to
    /// destroy the current tooltip instance.
    /// 
    /// The script also has event handlers for when the mouse pointer enters or exits the object. When the mouse pointer
    /// enters the object, the script checks whether it is possible to create a tooltip, and if so, it instantiates a new
    /// instance of the TooltipPrefab and assigns it to the tooltip field. It then calls the UpdateTooltip method to update
    /// the content of the tooltip, and a PositionTooltip method to reposition the tooltip so that it is properly aligned with
    /// the object. When the mouse pointer exits the object, the script calls the ClearTooltip method to destroy the current
    /// tooltip instance.
    /// </summary>
    public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// The prefab for the tooltip.
        /// </summary>
        [field : SerializeField] private GameObject TooltipPrefab { get; set; } = null;

        /// <summary>
        /// The current instance of the tooltip.
        /// </summary>
        private GameObject Tooltip { get; set; } = null;

        /// <summary>
        /// Abstract method for updating the content of the tooltip.
        /// </summary>
        /// <param name="tooltip">The tooltip GameObject.</param>
        public abstract void UpdateTooltip(GameObject tooltip);

        /// <summary>
        /// Abstract method for checking whether it is currently possible to create a tooltip for the object.
        /// </summary>
        /// <returns>A bool indicating whether a tooltip can be created.</returns>
        public abstract bool CanCreateTooltip();

        /// <summary>
        /// Event handler for when the object is destroyed. Destroys the current tooltip instance.
        /// </summary>
        private void OnDestroy()
        {
            ClearTooltip();
        }

        /// <summary>
        /// Event handler for when the object is disabled. Destroys the current tooltip instance.
        /// </summary>
        private void OnDisable()
        {
            ClearTooltip();
        }

        /// <summary>
        /// Event handler for when the mouse pointer enters the object. Creates a new tooltip instance if possible, and updates and positions the tooltip.
        /// </summary>
        /// <param name="eventData">Data for the pointer enter event.</param>
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            // Get the parent canvas of the object
            Canvas parentCanvas = GetComponentInParent<Canvas>();

            // If a tooltip is already present and it is not possible to create a new one, destroy the current tooltip
            if (Tooltip && !CanCreateTooltip())
            {
                ClearTooltip();
            }

            // If no tooltip is present and it is possible to create one, instantiate a new tooltip
            if (!Tooltip && CanCreateTooltip())
            {
                Tooltip = Instantiate(TooltipPrefab, parentCanvas.transform);
            }

            // If a tooltip is present, update its content and position it
            if (!Tooltip) { return; }
            UpdateTooltip(Tooltip);
            PositionTooltip();
        }

        /// <summary>
        /// Repositions the tooltip so that it is properly aligned with the object.
        /// </summary>
        private void PositionTooltip()
        {
            // Force update of canvas transforms to get accurate world corners for the tooltip and object
            Canvas.ForceUpdateCanvases();

            // Get the world corners of the tooltip and object
            Vector3[] tooltipCorners = new Vector3[4];
            Tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
            Vector3[] slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);

            // Determine which corner of the object the tooltip should be aligned with
            bool below = transform.position.y > Screen.height / 2f;
            bool right = transform.position.x < Screen.width / 2f;

            // Reposition the tooltip
            int slotCorner = GetCornerIndex(below, right);
            int tooltipCorner = GetCornerIndex(!below, !right);

            Tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + Tooltip.transform.position;
        }

        /// <summary>
        /// Gets the index of the corner of the object or tooltip to align with.
        /// </summary>
        /// <param name="below">A bool indicating whether the object or tooltip is below the center of the screen.</param>
        /// <param name="right">A bool indicating whether the object or tooltip is to the right of the center of the screen.</param>
        /// <returns>The index of the corner to align with.</returns>
        private int GetCornerIndex(bool below, bool right)
        {
            return below switch
            {
                // Object or tooltip is below and to the left of the center of the screen
                true when !right => 0,
                // Object or tooltip is above and to the left of the center of the screen
                false when !right => 1,
                // Object or tooltip is above and to the right of the center of the screen
                false when right => 2,
                // Object or tooltip is below and to the right of the center of the screen
                _ => 3
            };
        }

        /// <summary>
        /// Event handler for when the mouse pointer exits the object. Destroys the current tooltip instance.
        /// </summary>
        /// <param name="eventData">Data for the pointer exit event.</param>
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            ClearTooltip();
        }

        /// <summary>
        /// Destroys the current tooltip instance.
        /// </summary>
        private void ClearTooltip()
        {
            if (Tooltip) { Destroy(Tooltip.gameObject); }
        }
    }
}
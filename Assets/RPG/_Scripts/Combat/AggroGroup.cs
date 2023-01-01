using UnityEngine;

namespace RPG.Combat
{
    /// <summary>
    /// This script is intended to manage a group of "Fighter" objects. The Fighter objects are serialized fields
    /// (meaning they can be set in the Unity Editor) and the script also has a boolean serialized field called
    /// "IsActiveOnStart" which determines whether or not the Fighters should be active when the game starts.
    ///
    /// In the Start() method, the script calls the Activate() method with the value of IsActiveOnStart as an argument.
    /// The Activate() method takes a boolean as an argument and iterates over all of the Fighters in the Fighters array.
    /// For each Fighter, it gets the Target component and sets its "enabled" property to the value of the shouldActivate argument.
    /// It then sets the Fighter's "enabled" property to the same value. This means that if shouldActivate is true, both
    /// the Target component and the Fighter component of each Fighter object will be enabled. If shouldActivate is false,
    /// both components will be disabled.
    /// </summary>
    public class AggroGroup : MonoBehaviour
    {
        /// <summary>
        /// An array of Fighter objects that this AggroGroup manages.
        /// </summary>
        [field : SerializeField] private Fighter[] Fighters { get; set; } = null;

        /// <summary>
        /// Determines whether or not the Fighters should be active when the game starts.
        /// </summary>
        [field : SerializeField] private bool IsActiveOnStart { get; set; } = false;

        /// <summary>
        /// Calls the Activate() method with the value of IsActiveOnStart as an argument when the game starts.
        /// </summary>
        private void Start()
        {
            Activate(IsActiveOnStart);
        }

        /// <summary>
        /// Enables or disables the Target and Fighter components of all Fighters in the Fighters array.
        /// </summary>
        /// <param name="shouldActivate">If true, the Target and Fighter components will be enabled. If false, they will be disabled.</param>
        public void Activate(bool shouldActivate)
        {
            // Iterate over all Fighters in the Fighters array
            foreach (Fighter fighter in Fighters)
            {
                // Get the Target component of the current Fighter
                Target target = fighter.GetComponent<Target>();

                // If the Fighter has a Target component, set its enabled property to the value of shouldActivate
                if (target != null)
                {
                    target.enabled = shouldActivate;
                }
                // Set the enabled property of the Fighter component to the value of shouldActivate
                fighter.enabled = shouldActivate;
            }
        }
    }
}

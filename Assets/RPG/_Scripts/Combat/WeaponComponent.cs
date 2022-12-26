using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    /// <summary>
    /// This script defines a class called "WeaponComponent" that is derived from the "MonoBehaviour" class,
    /// which is a base class for scripts that need to be attached to GameObjects in the Unity engine.
    /// 
    /// The "WeaponComponent" class has a single field, "OnHitEvent", which is serialized and marked with the
    /// "UnityEvent" attribute. This field is a UnityEvent, which is a type of object that can hold a list of
    /// functions that can be invoked or "triggered" by calling the "Invoke" method.
    /// 
    /// The "WeaponComponent" class also has a single method called "OnHit", which simply calls the "Invoke"
    /// method on the "OnHitEvent" field. This means that when the "OnHit" method is called, it will trigger
    /// all of the functions that are registered with the "OnHitEvent" UnityEvent.
    /// 
    /// This script could be used to allow other scripts to register functions that should be called whenever the
    /// "OnHit" method is called, for example to play a sound or apply a force to a GameObject when a weapon hits
    /// something in the game.
    /// </summary>
    public class WeaponComponent : MonoBehaviour
    {
        /// <summary>
        /// An event that is triggered when the weapon hits something.
        /// </summary>
        [field: SerializeField] private UnityEvent OnHitEvent = null;

        /// <summary>
        /// Triggers the OnHitEvent, causing any registered functions to be called.
        /// </summary>
        public void OnHit()
        {
            // Invoke the OnHitEvent, triggering any registered functions
            OnHitEvent.Invoke();
        }
    }
}

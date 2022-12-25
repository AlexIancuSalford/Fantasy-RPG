using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class WeaponComponent : MonoBehaviour
    {
        [field: SerializeField] private UnityEvent OnHitEvent = null; 

        public void OnHit()
        {
            OnHitEvent.Invoke();
        }
    }
}

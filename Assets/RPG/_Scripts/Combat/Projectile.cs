using RPG.Attributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    /// <summary>
    /// The projectile has several serialized fields that can be set in the Unity
    /// Inspector: a speed, whether it is homing (meaning it will follow its target),
    /// and a hit effect GameObject to instantiate upon impact.
    ///  
    /// The projectile also has a private field for a target, which is a Health
    /// component representing the object that the projectile is intended to hit.
    /// It also has a private field for damage, which is the amount of damage that
    /// the projectile will do to the target when it hits. There is also a private
    /// field for a destroy time, which is the amount of time that the projectile
    /// will exist before being destroyed if it misses its target.
    ///  
    /// The Start and Update methods are called by Unity at the beginning and
    /// during each frame of the game, respectively. The Start method sets the
    /// projectile's rotation to face the location that it should aim for, and the
    /// Update method handles the projectile's movement and destruction. If the
    /// projectile has a target and is homing, it will rotate towards the target's
    /// location. It will also continuously move forward at its speed. If the target's
    /// Health component indicates that it is dead, the projectile will be destroyed
    /// after a certain amount of time specified by the destroy time field.
    ///  
    /// The OnTriggerEnter method is called by Unity when the projectile's
    /// collider (a component that determines if it is colliding with other objects)
    /// enters the trigger area of another collider. If the other collider belongs
    /// to an object with a Health component, the projectile will stop moving,
    /// instantiate the hit effect if it has one, and cause the target to take
    /// damage equal to the damage field. The projectile will then destroy itself
    /// after a short delay.
    /// 
    /// The Projectile game object also holds a reference to the game object that
    /// shot the projectile, and will be used in order to determine experience point
    /// awarded to the player character or other applicable effects
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        private Health _target = null;
        private float _damage = 0f;
        private float _destroyTime = 3.0f;
        private GameObject _instigator = null;

        [field : SerializeField] private float Speed { get; set; }
        [field : SerializeField] bool IsHoming { get; set; } = true;
        [field : SerializeField] private GameObject HitEffect { get; set; } = null;
        [field : SerializeField] private UnityEvent OnHit { get; set; } = null;

        // Start is called before the first frame update
        void Start()
        {
            // Set the projectile to aim at the target location
            transform.LookAt(AimLocation());
        }

        void Update()
        {
            // Return if there is no target
            if (_target == null) { return; }

            // If the projectile is homing and the target is not dead, aim at the target
            if (IsHoming && !_target.IsDead) { transform.LookAt(AimLocation()); }

            // Move the projectile forward
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);

            // Cleanup
            // Destroy all projectiles that missed their target
            StartCoroutine(DelayDestroy(_destroyTime));
        }

        /// <summary>
        /// Calculates the aim location for the projectile based on the target's capsule collider.
        /// </summary>
        /// <returns>The aim location for the projectile</returns>
        private Vector3 AimLocation()
        {
            CapsuleCollider targetCapsuleCollider = _target.GetComponent<CapsuleCollider>();
            if (targetCapsuleCollider == null) { return _target.transform.position; }

            // Aim at the center of the target's capsule collider
            return _target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;
        }

        /// <summary>
        /// Set the target and damage for the projectile.
        /// </summary>
        /// <param name="target">The target for the projectile</param>
        /// <param name="damage">The amount of damage the projectile should deal</param>
        /// <param name="instigator">The reference to the game object instantiating the projectile</param>
        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            _target = target;
            _damage = damage;
            _instigator = instigator;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Return if the other object does not have a Health component
            if (other.gameObject.GetComponent<Health>() == null) { return; }
            // Return if the target is already dead
            if (_target.IsDead) { return; }

            // Invoke the event that will trigger a sound on hit
            OnHit.Invoke();

            // Stop the projectile from moving
            Speed = 0;

            // Instantiate the hit effect if it is not null
            if (HitEffect != null)
            {
                Instantiate(HitEffect, AimLocation(), transform.rotation);
            }

            // Deal damage to the target
            _target.TakeDamage(_instigator, _damage);
            // Destroy the projectile after a delay
            Destroy(gameObject, 0.2f);
        }

        /// <summary>
        /// Coroutine that delays the destruction of the projectile by a specified time.
        /// </summary>
        /// <param name="time">The time to wait before destroying the projectile<</param>
        /// <returns></returns>
        private IEnumerator DelayDestroy(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}
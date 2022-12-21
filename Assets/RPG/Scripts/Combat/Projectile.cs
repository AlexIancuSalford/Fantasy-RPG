using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        private Health _target = null;
        private float _damage = 0f;
        private float _destroyTime = 3.0f;

        [field: SerializeField] private float Speed { get; set; }
        [field: SerializeField] bool IsHoming { get; set; } = true;
        [field: SerializeField] private GameObject HitEffect { get; set; } = null;

        // Start is called before the first frame update
        void Start()
        {
            transform.LookAt(AimLocation());
        }

        // Update is called once per frame
        void Update()
        {
            if (_target == null) { return; }

            if (IsHoming && !_target.IsDead) { transform.LookAt(AimLocation()); }
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);

            // Cleanup
            // Destroy all projectiles that missed their target
            StartCoroutine(DelayDestroy(_destroyTime));
        }

        private Vector3 AimLocation()
        {
            CapsuleCollider targetCapsuleCollider = _target.GetComponent<CapsuleCollider>();
            if (targetCapsuleCollider == null) { return _target.transform.position; }

            return _target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;
        }

        public void SetTarget(Health target, float damage)
        {
            _target = target;
            _damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Health>() == null) { return; }
            if (_target.IsDead) { return; }

            Speed = 0;

            if (HitEffect != null)
            {
                Instantiate(HitEffect, AimLocation(), transform.rotation);
            }

            _target.TakeDamage(_damage);
            Destroy(gameObject, 0.2f);
        }

        private IEnumerator DelayDestroy(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}
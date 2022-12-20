using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Health _target = null;
    private float _damage = 0f;
    
    [field : SerializeField] private float Speed { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null) { return;}
        transform.LookAt(AimLocation());
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);
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

        _target.TakeDamage(_damage);
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}

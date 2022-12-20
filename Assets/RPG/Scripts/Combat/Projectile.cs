using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Health _target { get; set; } = null;
    
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

    public void SetTarget(Health target)
    {
        _target = target;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [field : SerializeField] private Transform Target { get; set; } = null;
    [field : SerializeField] private float Speed { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null) { return;}
        transform.LookAt(AimLocation(Target));
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);
    }

    private Vector3 AimLocation(Transform target)
    {
        CapsuleCollider targetCapsuleCollider = target.GetComponent<CapsuleCollider>();
        if (targetCapsuleCollider == null) { return target.position; }

        return target.position + Vector3.up * targetCapsuleCollider.height / 2;
    }
}

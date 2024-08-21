using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamageForce : AOEDamage
{
    [SerializeField] protected float force;
    [SerializeField] protected bool scaleForce = true;
    protected override void ApplyDamage(Collider[] _objects)
    {
        base.ApplyDamage(_objects);
        
        ApplyForce(_objects);
    }

    protected virtual void ApplyForce(Collider[] _objects)
    {
        
        foreach (Collider col in _objects)
        {
            if(col.gameObject == gameObject) continue;
            if (col.attachedRigidbody)
            {
                if (!scaleForce)
                {
                    col.attachedRigidbody.AddForce((col.transform.position - transform.position).normalized * force, ForceMode.Impulse);
                    continue;
                }

                float dist = Vector3.Distance(col.transform.position, transform.position);
                float scale = dist / radius;
                col.attachedRigidbody.AddForce((col.transform.position - transform.position).normalized * (force * scale), ForceMode.Impulse);
            }
        }
    }
    
}

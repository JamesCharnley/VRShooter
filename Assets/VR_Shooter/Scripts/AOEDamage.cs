using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{

    [SerializeField] protected float radius;

    [SerializeField] protected float damage;

    [SerializeField] protected bool scaleDamage;

    public void InitAoe()
    {
        CollectObjects();
    }

    protected virtual void CollectObjects()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);
        ApplyDamage(cols);
    }

    protected virtual void ApplyDamage(Collider[] _objects)
    {
        foreach (Collider col in _objects)
        {
            if(col.gameObject == gameObject) continue;
            if (col.TryGetComponent(out ITakeDamage takeDamage))
            {
                if (!scaleDamage)
                {
                    takeDamage.TakeDamage(damage);
                    continue;
                }

                float dist = Vector3.Distance(col.transform.position, transform.position);
                float scale = dist / radius;
                takeDamage.TakeDamage(damage * scale);
            }
        }
    }
}

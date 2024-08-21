using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class EjectCasing : MonoBehaviour
{
    [SerializeField] private Transform ejectTransform;
    [SerializeField] private GameObject casingPrefab;
    [SerializeField] private float ejectForceMin = 4;
    [SerializeField] private float ejectForceMax = 5;
    public void CasingRelease()
    {
        GameObject casing = Instantiate(casingPrefab, ejectTransform.position, ejectTransform.rotation);
        if (casing.TryGetComponent(out Rigidbody rb))
        {
            float randForce = Random.Range(ejectForceMin, ejectForceMax);
            rb.AddForce((ejectTransform.right + Vector3.up * 0.2f) * randForce, ForceMode.Impulse);
        }
    }

    public void Shoot()
    {
        
    }
}

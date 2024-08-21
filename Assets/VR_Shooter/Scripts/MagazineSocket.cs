using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MagazineSocket : MonoBehaviour
{
    [SerializeField] private GunMagazineType[] compatibleMagazines;
    [SerializeField] private Gun gun;
    [SerializeField] private int maxMagazines;
    private List<Magazine> magazines = new();
    private Magazine currentMagazine;
    [SerializeField] private bool autoRemoveMagazineOnEmpty = false;

    private void Start()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.TryGetComponent(out Magazine mag))
                {
                    magazines.Add(mag);
                }
            }
        }

        if (magazines.Count > 0)
        {
            currentMagazine = magazines[0];
        }
    }

    private bool IsCompatible(GunMagazineType _type)
    {
        foreach (GunMagazineType magType in compatibleMagazines)
        {
            if (magType == _type) return true;
        }

        return false;
    }

    public void TakeMagazine(Magazine _magazine)
    {
        Debug.LogError("MagazineSocket: Take Magazine");
        Collider[] cols = _magazine.transform.GetComponents<Collider>();
        foreach (Collider col in cols)
        {
            col.enabled = false;
        }
        _magazine.ReleaseFromHand();
        magazines.Add(_magazine);
        currentMagazine = magazines[0];
        if (_magazine.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
        _magazine.transform.SetParent(transform);
        _magazine.transform.localPosition = Vector3.zero;
        _magazine.transform.localRotation = quaternion.identity;
        
    }
    
    public bool UseAmmo()
    {
        if (!currentMagazine.IsEmpty())
        {
            currentMagazine.RemoveAmmo(1);
        }
        else
        {
            return false;
        }

        if (currentMagazine.IsEmpty())
        {
            if (autoRemoveMagazineOnEmpty)
            {
                // eject mag
                EjectMagazine();
            }
        }

        return true;
    }

    public void EjectMagazine()
    {
        magazines.Remove(currentMagazine);
        currentMagazine.Eject();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Magazine mag))
        {
            if (IsCompatible(mag.GetType()))
            {
                if (magazines.Count < maxMagazines)
                {
                    TakeMagazine(mag);
                }
            }
        }
    }
}

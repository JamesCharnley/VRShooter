using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Magazine : GrabableObject
{
    [SerializeField] private GunMagazine magazineData;

    private AmmoHolder ammoHolder;

    private MagazineSocket currentSocket;

    [SerializeField] private bool destroyOnEject = false;
    public void SetMagazineData(GunMagazine _magazineData)
    {
        magazineData = _magazineData;
    }

    public void SetAmmoHolder(AmmoHolder _ammoHolder)
    {
        ammoHolder = _ammoHolder;
    }

    public GunMagazineType GetType()
    {
        return magazineData.currentMagazineType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MagazineSocket socket))
        {
            currentSocket = socket;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MagazineSocket socket))
        {
            currentSocket = null;
        }
    }

    public void RemoveAmmo(int _amount)
    {
        magazineData.currentAmmo.count -= 1;
    }

    public bool IsEmpty()
    {
        if (magazineData.currentAmmo.count == 0)
        {
            return true;
        }

        return false;
    }

    public void Eject()
    {
        Debug.LogError("Magazine: Eject");
        if (destroyOnEject)
        {
            Destroy(gameObject);
            return;
        }

        if (TryGetComponent(out Rigidbody rb))
        {
            Debug.LogError("Magazine: Eject.. Rigidbody found");
            transform.SetParent(null);
            rb.isKinematic = false;
        }

        StartCoroutine(EnableCollidersAfterSeconds());
    }

    IEnumerator EnableCollidersAfterSeconds()
    {
        yield return new WaitForSeconds(1);
        Collider[] cols = GetComponents<Collider>();
        foreach (Collider col in cols)
        {
            col.enabled = true;
        }
    }
    
    public override FingerGroup GetFingerGroup(Hand _handSide, Transform _xrOrigin, Transform _handBone)
    {
        float originAngle =
            Vector3.Angle(Vector3.up, transform.up);
        Debug.Log($"Angle: {originAngle}");
        FingerGroup optimalFingerGroup = null;
        float lowestAngleDiff = 9999;
        foreach (FingerGroup fingerGroup in fingerGroups)
        {
            if (fingerGroup.GetHandType == _handSide)
            {
                float angleDiff = fingerGroup.GetOptimalAngleOffset - originAngle;
                if (Mathf.Abs(angleDiff) < lowestAngleDiff)
                {
                    optimalFingerGroup = fingerGroup;
                    lowestAngleDiff = Mathf.Abs(angleDiff);
                }
            }
        }

        if (optimalFingerGroup)
        {
            return optimalFingerGroup;
        }
        return fingerGroups[0];
    }
}

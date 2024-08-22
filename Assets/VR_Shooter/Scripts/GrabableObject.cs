using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GrabableObject : MonoBehaviour
{
    public bool isGrabbed = false;

    public Action OnGrabbed;
    public Action OnDropped;

    protected HandActions ownerHand = null;
    

    public virtual void Grab(GameObject _hand)
    {
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
        //rb.Move(_hand.transform.TransformVector(Vector3.zero), _hand.transform.rotation);
        transform.SetParent(_hand.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = quaternion.identity;
        
        
        
        isGrabbed = true;

        ownerHand = _hand.transform.GetComponent<HandActions>();

        if (_hand.TryGetComponent(out GrabHandler grabHandler))
        {
            grabHandler.SetGrabbedObject(this);
        }
        else
        {
            Debug.LogError("Grab: failed to get component GrabHandler");
        }
        
        OnGrabbed?.Invoke();
    }

    public virtual void ReleaseFromHand(bool _forced = false, bool _enableRigidbody = true)
    {
        transform.SetParent(null);
        if (_enableRigidbody)
        {
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
            }
        }

        if (_forced)
        {
            if (ownerHand.transform.TryGetComponent(out GrabHandler _grabHandler))
            {
                _grabHandler.DropGrabable();
            }
        }

        isGrabbed = false;

        OnDropped?.Invoke();

        ownerHand = null;
    }
}

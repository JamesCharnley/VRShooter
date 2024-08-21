using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
    private Vector3 startPosition;

    private Quaternion startRotation;

    private Rigidbody rb;

    private ShootingRangeControl control;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = rb.position;
        startRotation = rb.rotation;

        control = FindObjectOfType<ShootingRangeControl>();
        if (control != null)
        {
            control.OnResetTargets += ResetTarget;
        }
    }

    void ResetTarget()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = startPosition;
        rb.rotation = startRotation;
    }

    private void OnDestroy()
    {
        if (control != null)
        {
            control.OnResetTargets -= ResetTarget;
        }
    }
}

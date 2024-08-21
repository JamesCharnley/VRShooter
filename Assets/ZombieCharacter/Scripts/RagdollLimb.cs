using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollLimb : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private RagdollLimb reactiveOtherLimb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyForce(Vector3 _force, ForceMode _forceMode, Vector3 _atPosition)
    {
        if (_atPosition != Vector3.zero)
        {
            rb.AddForceAtPosition(_force, _atPosition, _forceMode);
        }
        else
        {
            rb.AddForce(_force, _forceMode);
        }
        

        if (reactiveOtherLimb != null)
        {
            reactiveOtherLimb.ApplyForce(_force * -1, _forceMode, Vector3.zero);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour
{
    [SerializeField] private Transform xrOrigin;

    [SerializeField] private Transform pistol;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Vector3.SignedAngle(xrOrigin.forward, pistol.forward, xrOrigin.forward);
        Debug.Log($"Angle: {angle}");
    }
}

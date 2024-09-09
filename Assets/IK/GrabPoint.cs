using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPoint : MonoBehaviour
{
    [SerializeField] private FingerType fingerType;
    public FingerType GetFingerType => fingerType;

    [SerializeField] private Transform fingerRoot;
    public Transform GetFingerRoot => fingerRoot;
}

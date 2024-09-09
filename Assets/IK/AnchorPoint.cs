using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
    [SerializeField] private AnchorType anchorType;
    public AnchorType GetAnchorType => anchorType;
}

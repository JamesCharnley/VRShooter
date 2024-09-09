using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistChainTransforms : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Transform tip;
    [SerializeField] private Transform rootTarget;
    [SerializeField] private Transform tipTarget;

    public Transform GetRoot => root;
    public Transform GetTip => tip;
    public Transform GetRootTarget => rootTarget;
    public Transform GetTipTarget => tipTarget;
}

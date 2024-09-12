using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FingerGroup : MonoBehaviour
{
    public enum TwistChainType
    {
        Root,
        Secondary
    }
    [System.Serializable]
    public struct TwistChainSettings
    {
        public float weight;
        public FingerType finger;
        public TwistChainType type;
        public Vector3 rootRotation;
        public Vector3 tipRotation;
    }
    [SerializeField] private AnchorType anchorType;
    public AnchorType GetAnchorType => anchorType;

    [SerializeField] private GrabPoint[] fingers;

    public GrabPoint[] GetFingers => fingers;

    [SerializeField] private Transform handAlignmentPoint;

    public Transform GetHandAlignmentPoint => handAlignmentPoint;

    [SerializeField] private float optimalAngleOffset;
    public float GetOptimalAngleOffset => optimalAngleOffset;

    [SerializeField] private Hand hand;
    public Hand GetHandType => hand;

    [SerializeField] private TwistChainSettings[] twistChainSettings;

    public void SetFingerTwistChain(Transform _fingerRoot, FingerType _fingerType)
    {
        TwistChainSettings settings = new();
        bool settingsExist = false;
        foreach (TwistChainSettings chainSettings in twistChainSettings)
        {
            if (chainSettings.finger == _fingerType)
            {
                settings = chainSettings;
                settingsExist = true;
                break;
            }
        }

        if (!settingsExist) return;

        TwistChainConstraint twistChain = null;
        TwistChainTransforms twistChainTransforms = null;
        switch (settings.type)
        {
            case TwistChainType.Root:
                twistChain = _fingerRoot.GetComponent<TwistChainConstraint>();
                twistChainTransforms = _fingerRoot.GetComponent<TwistChainTransforms>();
                break;
            case TwistChainType.Secondary:
                twistChain = _fingerRoot.GetChild(0).GetComponent<TwistChainConstraint>();
                twistChainTransforms = _fingerRoot.GetChild(0).GetComponent<TwistChainTransforms>();
                break;
            default:
                break;
        }

        if (!twistChain || !twistChainTransforms) return;
        
        twistChainTransforms.GetRootTarget.localEulerAngles = settings.rootRotation;
        twistChainTransforms.GetTipTarget.localEulerAngles = settings.tipRotation;

        twistChain.weight = settings.weight;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public enum FingerType
{
    Index,
    Middle,
    Ring,
    Pinky,
    Thumb
}

public class GrabHandler : MonoBehaviour
{
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private InputActionProperty gripAction;
    [SerializeField] private Transform[] raycastOrigins;
    [SerializeField] private Transform raycastOriginTransform;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private GrabPoint[] fingers;
    [SerializeField] private ChainIKConstraint[] fingerIkWeights;
    [SerializeField] private Transform handIkTarget;
    [SerializeField] private Transform handBone;
    private Transform handIkTargetParent;
    [SerializeField] private AnchorPoint[] anchorPoints;
    public AnchorPoint[] GetAnchorPoints => anchorPoints;
    [HideInInspector] public GrabableObject grabbedObject = null;

    public Action<GrabableObject> OnGrabbedObject;
    public Action OnReleasedObject;

    private FingerGroup currentFingerGroup = null;

    private GrabableObject currentObjectBeingGrabbed = null;

    private Vector3 handIkOriginPosition;
    private Quaternion handIkOriginRotation;
    [SerializeField] private HandActions handActions;
    // Start is called before the first frame update
    void Start()
    {
        
        gripAction.action.performed += ActivateGrab;
        gripAction.action.canceled += DeactivateGrab;
        handIkOriginPosition = handIkTarget.transform.localPosition;
        handIkOriginRotation = handIkTarget.transform.localRotation;
        handIkTargetParent = handIkTarget.parent;
    }

    private void Update()
    {
        if (currentFingerGroup)
        {
            if (!handAligned)
            {
                AlignHand();
            }
            //AlignHand();
            AlignFingers();
        }

        //return;
        if (handAligned && fingersAligned)
        {
            if (currentObjectBeingGrabbed)
            {
                FinishGrab(currentObjectBeingGrabbed);
                currentObjectBeingGrabbed = null;
            }
            ReturnHandToOrigin();
            if (grabbedObject)
            {
                if (grabbedObject.transform.localPosition != Vector3.zero)
                {
                    grabbedObject.transform.localPosition = Vector3.Lerp(grabbedObject.transform.localPosition,
                        Vector3.zero, 5 * Time.deltaTime);
                }

                if (grabbedObject.transform.localRotation != Quaternion.identity)
                {
                    grabbedObject.transform.localRotation = Quaternion.Lerp(grabbedObject.transform.localRotation, Quaternion.identity, 5 * Time.deltaTime);
                }
            }
        }
    }

    private bool handAligned = false;
    void AlignHand()
    {
        if (handIkTarget.parent)
        {
            handIkTarget.transform.SetParent(null);
        }
        
        handIkTarget.transform.position = Vector3.Lerp(handIkTarget.position,
            currentFingerGroup.GetHandAlignmentPoint.position, 5 * Time.deltaTime);
        handIkTarget.transform.rotation = Quaternion.Lerp(handIkTarget.rotation,
            currentFingerGroup.GetHandAlignmentPoint.rotation, 5 * Time.deltaTime);
        if (Vector3.Distance(handIkTarget.position, currentFingerGroup.GetHandAlignmentPoint.position) < 0.005f)
        {
            if (Quaternion.Angle(handIkTarget.rotation, currentFingerGroup.GetHandAlignmentPoint.rotation) < 1)
            {
                handAligned = true;
            }
        }
    }

    void ReturnHandToOrigin()
    {
        if (!handIkTarget.transform.parent)
        {
            handIkTarget.transform.SetParent(handIkTargetParent);
        }
        
        handIkTarget.transform.localPosition = Vector3.Lerp(handIkTarget.localPosition,
            handIkOriginPosition, 5 * Time.deltaTime);
        handIkTarget.transform.localRotation = Quaternion.Lerp(handIkTarget.localRotation,
            handIkOriginRotation, 5 * Time.deltaTime);
    }

    private bool fingersAligned = false;
    void AlignFingers()
    {
        foreach (GrabPoint finger in fingers)
        {
            foreach (GrabPoint grabPoint in currentFingerGroup.GetFingers)
            {
                if (finger.GetFingerType == grabPoint.GetFingerType)
                {
                    finger.transform.position = grabPoint.transform.position;
                }
            }
        }
        SetFingerIkWeights(1);
        
        foreach (GrabPoint finger in fingers)
        {
            currentFingerGroup.SetFingerTwistChain(finger.GetFingerRoot, finger.GetFingerType);
        }
        
        fingersAligned = true;
    }

    void SetFingerIkWeights(float _weight)
    {
        foreach (ChainIKConstraint fingerIkWeight in fingerIkWeights)
        {
            fingerIkWeight.weight = _weight;
        }
    }
    private void DeactivateGrab(InputAction.CallbackContext obj)
    {
        Debug.Log("DeactivateGrab");
        if (grabbedObject != null)
        {
            grabbedObject.ReleaseFromHand();
            DropGrabable();
        }
    }

    public void DropGrabable()
    {
        currentFingerGroup = null;
        handAligned = false;
        fingersAligned = false;
        SetFingerIkWeights(0);
        OnReleasedObject?.Invoke();
        grabbedObject = null;
    }

    

    private void ActivateGrab(InputAction.CallbackContext obj)
    {
        List<GrabableObject> grabableObjects = GrabOverlapSphere();
        
        if (grabableObjects.Count == 0) return;
        
        GrabableObject closestObject = null;
        foreach (GrabableObject grabableObject in grabableObjects)
        {
            if(grabableObject == null) continue;
            if(grabableObject.isGrabbed) continue;
            
            if (closestObject == null)
            {
                closestObject = grabableObject;
                continue;
            }

            float closestObjectDist = Vector3.Distance(raycastOriginTransform.position, closestObject.transform.position);
            float objectDist = Vector3.Distance(raycastOriginTransform.position, grabableObject.transform.position);

            if (objectDist < closestObjectDist) closestObject = grabableObject;
            
        }

        if (closestObject != null)
        {
            currentFingerGroup = closestObject.GetFingerGroup(handActions.GetHand, xrOrigin, handBone);
            currentObjectBeingGrabbed = closestObject;
            grabableObjects.Clear();
        }
    }

    void FinishGrab(GrabableObject _object)
    {
        foreach (AnchorPoint anchorPoint in anchorPoints)
        {
            if (anchorPoint.GetAnchorType == currentFingerGroup.GetAnchorType)
            {
                _object.Grab(transform.gameObject, anchorPoint);
            }
        }
        OnGrabbedObject?.Invoke(grabbedObject);
        
    }
    List<GrabableObject> GrabRaycast2()
    {
        List<GrabableObject> interactables = new List<GrabableObject>();
        RaycastHit hit;
        foreach (Transform origin in raycastOrigins)
        {
            if (Physics.Raycast(origin.position, raycastOriginTransform.forward, out hit, 0.1f,
                    interactableLayerMask))
            {
                if(hit.collider.TryGetComponent(out GrabableObject _object))
                {
                    if (!interactables.Contains(_object))
                    {
                        interactables.Add(_object);
                    }
                }
            }
            Debug.DrawLine(origin.position, origin.position + raycastOriginTransform.forward * 0.1f, Color.green);
        }
        return interactables;
    }
    List<GrabableObject> GrabOverlapSphere()
    {
        List<GrabableObject> interactables = new List<GrabableObject>();
        Collider[] cols = Physics.OverlapSphere(raycastOriginTransform.position, 0.1f, interactableLayerMask);
        foreach (Collider col in cols)
        {
            if (!col.TryGetComponent(out GrabableObject _object)) continue;
            if (interactables.Contains(_object)) continue;
            interactables.Add(_object);
        }
        return interactables;
    }
    List<GrabableObject> GrabRaycast()
    {
        List<GrabableObject> interactables = new List<GrabableObject>();
        RaycastHit hit;
        // center
        if (Physics.Raycast(raycastOriginTransform.position, raycastOriginTransform.forward, out hit, 0.1f,
                interactableLayerMask))
        {
            if(hit.collider.TryGetComponent(out GrabableObject _object))
            {
                if (!interactables.Contains(_object))
                {
                    interactables.Add(_object);
                }
            }
        }
        // right
        if (Physics.Raycast(raycastOriginTransform.position + raycastOriginTransform.right * 0.05f, raycastOriginTransform.forward, out hit, 0.1f,
                interactableLayerMask))
        {
            if(hit.collider.TryGetComponent(out GrabableObject _object))
            {
                if (!interactables.Contains(_object))
                {
                    interactables.Add(_object);
                }
            }
        }
        // left
        if (Physics.Raycast(raycastOriginTransform.position + -raycastOriginTransform.right * 0.05f, raycastOriginTransform.forward, out hit, 0.1f,
                interactableLayerMask))
        {
            if(hit.collider.TryGetComponent(out GrabableObject _object))
            {
                if (!interactables.Contains(_object))
                {
                    interactables.Add(_object);
                }
            }
        }
        // right up
        if (Physics.Raycast(raycastOriginTransform.position + raycastOriginTransform.right * 0.05f + raycastOriginTransform.up * 0.05f, raycastOriginTransform.forward, out hit, 0.1f,
                interactableLayerMask))
        {
            if(hit.collider.TryGetComponent(out GrabableObject _object))
            {
                if (!interactables.Contains(_object))
                {
                    interactables.Add(_object);
                }
            }
        }
        // right down
        if (Physics.Raycast(raycastOriginTransform.position + raycastOriginTransform.right * 0.05f + -raycastOriginTransform.up * 0.05f, raycastOriginTransform.forward, out hit, 0.1f,
                interactableLayerMask))
        {
            if(hit.collider.TryGetComponent(out GrabableObject _object))
            {
                if (!interactables.Contains(_object))
                {
                    interactables.Add(_object);
                }
            }
        }
        // left up 
        if (Physics.Raycast(raycastOriginTransform.position + -raycastOriginTransform.right * 0.05f + raycastOriginTransform.up * 0.05f, raycastOriginTransform.forward, out hit, 0.1f,
                interactableLayerMask))
        {
            if(hit.collider.TryGetComponent(out GrabableObject _object))
            {
                if (!interactables.Contains(_object))
                {
                    interactables.Add(_object);
                }
            }
        }
        // left down
        if (Physics.Raycast(raycastOriginTransform.position + -raycastOriginTransform.right * 0.05f + -raycastOriginTransform.up * 0.05f, raycastOriginTransform.forward, out hit, 0.1f,
                interactableLayerMask))
        {
            if(hit.collider.TryGetComponent(out GrabableObject _object))
            {
                if (!interactables.Contains(_object))
                {
                    interactables.Add(_object);
                }
            }
        }
        // down
        if (Physics.Raycast(raycastOriginTransform.position + -raycastOriginTransform.up * 0.05f, raycastOriginTransform.forward, out hit, 0.1f,
                interactableLayerMask))
        {
            if(hit.collider.TryGetComponent(out GrabableObject _object))
            {
                if (!interactables.Contains(_object))
                {
                    interactables.Add(_object);
                }
            }
        }
        // up
        if (Physics.Raycast(raycastOriginTransform.position + raycastOriginTransform.up * 0.05f, raycastOriginTransform.forward, out hit, 0.1f,
                interactableLayerMask))
        {
            if(hit.collider.TryGetComponent(out GrabableObject _object))
            {
                if (!interactables.Contains(_object))
                {
                    interactables.Add(_object);
                }
            }
        }
        return interactables;
    }

    public GrabableObject GetGrabbedObject()
    {
        return grabbedObject;
    }

    public void SetGrabbedObject(GrabableObject _object)
    {
        grabbedObject = _object;
    }

    private void OnDestroy()
    {
        gripAction.action.performed -= ActivateGrab;
        gripAction.action.canceled -= DeactivateGrab;
    }
}

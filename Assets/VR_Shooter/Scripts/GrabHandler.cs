using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabHandler : MonoBehaviour
{
    [SerializeField] private InputActionProperty gripAction;
    [SerializeField] private Transform[] raycastOrigins;
    [SerializeField] private Transform raycastOriginTransform;
    [SerializeField] private LayerMask interactableLayerMask;

    [HideInInspector] public GrabableObject grabbedObject = null;

    public Action<GrabableObject> OnGrabbedObject;
    public Action OnReleasedObject;
    // Start is called before the first frame update
    void Start()
    {
        gripAction.action.performed += ActivateGrab;
        gripAction.action.canceled += DeactivateGrab;
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
        OnReleasedObject?.Invoke();
        grabbedObject = null;
    }

    

    private void ActivateGrab(InputAction.CallbackContext obj)
    {
        Debug.Log("ActivateGrab");
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
            closestObject.Grab(gameObject);
            //grabbedObject = closestObject;
            OnGrabbedObject?.Invoke(grabbedObject);
            grabableObjects.Clear();
        }
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabHandler : MonoBehaviour
{
    [SerializeField] private InputActionProperty gripAction;
    private List<GrabableObject> grabableObjects = new();

    [HideInInspector] public GrabableObject grabbedObject = null;
    [HideInInspector] public GrabableObject droppedObject = null;
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
            OnReleasedObject?.Invoke();
        }
    }

    private void ActivateGrab(InputAction.CallbackContext obj)
    {
        Debug.Log("ActivateGrab");
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

            float closestObjectDist = Vector3.Distance(transform.position, closestObject.transform.position);
            float objectDist = Vector3.Distance(transform.position, grabableObject.transform.position);

            if (objectDist < closestObjectDist)
            {
                closestObject = grabableObject;
            }
        }

        if (closestObject != null)
        {
            closestObject.Grab(gameObject);
            //grabbedObject = closestObject;
            OnGrabbedObject?.Invoke(grabbedObject);
            grabableObjects.Clear();
        }
    }

    public GrabableObject GetGrabbedObject()
    {
        return grabbedObject;
    }

    public void SetGrabbedObject(GrabableObject _object)
    {
        grabbedObject = _object;
    }

    public void ForceDropItem()
    {
        Debug.Log("ForceDrop()");
        droppedObject = grabbedObject;
        //RemoveGrabable(grabbedObject);
        //grabbedObject = null;
    }

    void AddGrabable(GrabableObject _object)
    {
        Debug.Log("Add Grabable");
        if (!grabableObjects.Contains(_object))
        {
            grabableObjects.Add(_object);
        }
        else
        {
            Debug.Log("grabable already added");
        }
    }
    void RemoveGrabable(GrabableObject _object)
    {
        Debug.Log("Remove grabable");
        if (grabableObjects.Contains(_object))
        {
            Debug.Log("Removed grabale");
            grabableObjects.Remove(_object);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if (droppedObject != null)
       {
           if (droppedObject.gameObject == other.gameObject)
           {
               Debug.Log("Rejected OnTriggerEnter: dropped == other");
               return;
           }
           
       }

       if (grabbedObject != null)
       {
           if (grabbedObject.gameObject == other.gameObject)
           {
               Debug.Log("Rejected OnTriggerEnter: grabbed == other");
               return;
           }
       }
        
       if (other.TryGetComponent(out GrabableObject _grabable))
       {
           Debug.Log($"OntriggerEnter grabable {_grabable.gameObject.name}");
           AddGrabable(_grabable);
       }
    }
    private void OnTriggerExit(Collider other)
    {
       
        if (other.TryGetComponent(out GrabableObject _grabable))
        {
            Debug.Log("OnTriggerExit: " + _grabable.gameObject.name);
            if (droppedObject && grabbedObject)
            {
                if (grabbedObject == droppedObject)
                {
                    if (_grabable.gameObject == grabbedObject.gameObject)
                    {
                        RemoveGrabable(_grabable);
                        grabbedObject = null;
                        droppedObject = null;
                        return;
                    }
                }
            }

            if (grabbedObject)
            {
                if (grabbedObject.gameObject != other.gameObject)
                {
                    RemoveGrabable(_grabable);
                }
            }
            else
            {
                RemoveGrabable(_grabable);
            }
        }
    }

    private void OnDestroy()
    {
        gripAction.action.performed -= ActivateGrab;
        gripAction.action.canceled -= DeactivateGrab;
    }
}

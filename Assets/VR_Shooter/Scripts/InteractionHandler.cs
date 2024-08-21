using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{

    [SerializeField] private InputActionProperty triggerAction;

    private List<IInteractable> interactables = new();
    // Start is called before the first frame update
    void Start()
    {
        triggerAction.action.performed += TriggerActivated;
    }

    private void OnDestroy()
    {
        triggerAction.action.performed -= TriggerActivated;
    }

    private void TriggerActivated(InputAction.CallbackContext _ctx)
    {
        Debug.Log("Activate trigger");
        IInteractable closestObject = null;
        foreach (IInteractable interactableObject in interactables)
        {
            
            if (closestObject == null)
            {
                closestObject = interactableObject;
                continue;
            }
            MonoBehaviour closestAsMono = closestObject as MonoBehaviour;
            MonoBehaviour currentAsMono = interactableObject as MonoBehaviour;
            
            float closestObjectDist = Vector3.Distance(transform.position, closestAsMono.transform.position);
            float objectDist = Vector3.Distance(transform.position, currentAsMono.transform.position);

            if (objectDist < closestObjectDist)
            {
                closestObject = interactableObject;
            }
        }

        if (closestObject != null)
        {
            closestObject.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactables.Add(interactable);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactables.Remove(interactable);
        }
    }
}

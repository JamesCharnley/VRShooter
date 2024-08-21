using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Hand
{
    Left,
    Right
}
public class HandActions : MonoBehaviour
{
    [SerializeField] private Hand hand;
    public Hand GetHand { get => hand; private set => hand = value; }
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private InputActionProperty gripAction;
    [SerializeField] private InputActionProperty southButtonAction;

    public Action OnTriggerActivate;
    public Action OnTriggerRelease;

    public Action OnGripActivate;
    public Action OnGripRelease;

    public Action OnPrimaryActivate;
    public Action OnPrimaryReleased;
    
    
    // Start is called before the first frame update
    void Start()
    {
        triggerAction.action.performed += TriggerActivated;
        triggerAction.action.canceled += TriggerCanceled;

        gripAction.action.performed += GripActivated;
        gripAction.action.canceled += GripCanceled;

        southButtonAction.action.performed += ButtonSouthPressed;
        southButtonAction.action.canceled += ButtonSouthReleased;
    }

    private void ButtonSouthPressed(InputAction.CallbackContext obj)
    {
        OnPrimaryActivate?.Invoke();
    }
    private void ButtonSouthReleased(InputAction.CallbackContext obj)
    {
        OnPrimaryReleased?.Invoke();
    }

    private void GripCanceled(InputAction.CallbackContext obj)
    {
        OnGripRelease?.Invoke();
    }

    private void GripActivated(InputAction.CallbackContext obj)
    {
        Debug.Log("grip");
        OnGripActivate?.Invoke();
    }

    private void TriggerCanceled(InputAction.CallbackContext obj)
    {
        OnTriggerRelease?.Invoke();
    }

    private void TriggerActivated(InputAction.CallbackContext obj)
    {
        OnTriggerActivate?.Invoke();
    }

    private void OnDestroy()
    {
        triggerAction.action.performed -= TriggerActivated;
        triggerAction.action.canceled -= TriggerCanceled;

        gripAction.action.performed -= GripActivated;
        gripAction.action.canceled -= GripCanceled;
        
        southButtonAction.action.performed -= ButtonSouthPressed;
        southButtonAction.action.canceled -= ButtonSouthReleased;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class HandAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private InputActionProperty gripAction;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float triggerAxis = triggerAction.action.ReadValue<float>();
        float gripAxis = gripAction.action.ReadValue<float>();
        
        anim.SetFloat("Trigger", triggerAxis);
        anim.SetFloat("Grip", gripAxis);
    }
}

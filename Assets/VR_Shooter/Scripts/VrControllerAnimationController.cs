using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrControllerAnimationController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private GrabHandler grabHandler;
    private string triggerAnimation;
    private GrabableObject grabbedObject;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        grabHandler.OnGrabbedObject += GrabbedNewObject;
        grabHandler.OnReleasedObject += ReleasedObject;
    }

    private void ReleasedObject()
    {
        if (grabbedObject != null)
        {
            if (grabbedObject is IAnimateController animateController)
            {
                GetComponent<HandRecoil>().enabled = true;
                if (grabbedObject is Gun gun)
                {
                    gun.OnShoot -= TriggerAction;
                }
            
            }
        }
        grabbedObject = null;
    }

    private void TriggerAction()
    {
        anim.SetTrigger(triggerAnimation);
    }

    private void GrabbedNewObject(GrabableObject _object)
    {
        grabbedObject = _object;
        if (grabbedObject is IAnimateController animateController)
        {
            GetComponent<HandRecoil>().enabled = false;
            triggerAnimation = animateController.TriggerAnimation;
            if (grabbedObject is Gun gun)
            {
                gun.OnShoot += TriggerAction;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        grabHandler.OnGrabbedObject -= GrabbedNewObject;
        grabHandler.OnReleasedObject -= ReleasedObject;
    }
}

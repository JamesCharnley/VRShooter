using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[System.Serializable]
public struct RecoilData
{
    public float maxRecoilX;
    public float maxRecoilY;
    public float recoilSpeed;
    public float correctionSpeed;
    
}
public class HandRecoil : MonoBehaviour
{
    [SerializeField] private GrabHandler grabHandler;
    private GrabableObject grabbedObject;
    private RecoilData recoilData;
    
    float maxRecoilX = -20f;
    float maxRecoilY = 10f;
    float recoilSpeed = 5f;
    float correctionSpeed = 2f;

    private Quaternion originalRotation;
    private Quaternion recoilRotation;
    private bool isRecoiling = false;

    private void Start()
    {
        grabHandler.OnGrabbedObject += GrabbedNewObject;
        grabHandler.OnReleasedObject += ReleasedObject;
        originalRotation = transform.localRotation;
    }

    private float recoilT = 0;
    private float decreseT = 0;
    private float currentRecoilSpeed;
    float speedFade = 1;
    
    void Update()
    {
        if (grabbedObject is null) return;
        if (isRecoiling)
        {
            currentRecoilSpeed -= speedFade * Time.deltaTime;
            recoilT += currentRecoilSpeed * Time.deltaTime;
            // Rotate the object back to its original rotation gradually
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation * recoilRotation, recoilT);
            if (recoilT >= 1)
            {
                isRecoiling = false;
            }
        }
        if(!isRecoiling)
        {
            decreseT += recoilData.correctionSpeed * Time.deltaTime;
            // Rotate the object back to its original rotation gradually
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, decreseT);

            //// If rotation is very close to original rotation, stop recoiling
            //if (decreseT >= 1)
            //{
            //    transform.localRotation = originalRotation;
            //}
        }
        
    }

    private int xDir = 1;
    void Recoil()
    {
        currentRecoilSpeed = recoilData.recoilSpeed;
        xDir *= -1;
        // Calculate random recoil rotation
        float recoilX = Random.Range(recoilData.maxRecoilX / 2, recoilData.maxRecoilX);
        float recoilY = Random.Range(recoilData.maxRecoilY / 2, recoilData.maxRecoilY);
        recoilX *= xDir;

        recoilRotation = Quaternion.Euler(recoilX, recoilY, 0f);
        recoilT = 0;
        decreseT = 0;
        // Set recoiling flag
        isRecoiling = true;
    }
    
    private void ReleasedObject()
    {
        if (grabbedObject == null) return;
        if (grabbedObject is ILocomotionHand locoHand)
        {
            if (grabbedObject is Gun gun)
            {
                gun.OnShoot -= TriggerAction;
            }

            transform.localRotation = originalRotation;

            GetComponent<Animator>().enabled = true;
            GetComponent<VrControllerAnimationController>().enabled = true;
        }
    }

    private void GrabbedNewObject(GrabableObject _object)
    {
        grabbedObject = _object;
        if (grabbedObject is ILocomotionHand locoHand)
        {
            recoilData = locoHand.RecoilValues;
            if (grabbedObject is Gun gun)
            {
                gun.OnShoot += TriggerAction;
            }
            GetComponent<Animator>().enabled = false;
            GetComponent<VrControllerAnimationController>().enabled = false;
        }
    }

   // private Vector3 recoilTargetVector;
    private void TriggerAction()
    {
        Recoil();
    }

    private void OnDestroy()
    {
        grabHandler.OnGrabbedObject -= GrabbedNewObject;
        grabHandler.OnReleasedObject -= ReleasedObject;
    }

 
    
    
}

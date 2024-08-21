using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    // Variables for controlling recoil
    public float maxRecoilX = -20f;
    public float maxRecoilY = 10f;
    public float recoilSpeed = 5f;
    public float recoilDecrease = 2f;
    public float maxRotationLimitX = 90f;
    public float maxRotationLimitY = 90f;

    private Quaternion originalRotation;
    private Quaternion recoilRotation;
    private bool isRecoiling = false;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        Recoil();
        if (isRecoiling)
        {
            // Rotate the object back to its original rotation gradually
            transform.localRotation = Quaternion.Lerp(transform.localRotation, recoilRotation, Time.deltaTime * recoilSpeed);
            if (Quaternion.Angle(transform.localRotation, recoilRotation) < 1.0f)
            {
                isRecoiling = false;
            }
        }
        else
        {
            // Rotate the object back to its original rotation gradually
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.deltaTime * recoilDecrease);

            // If rotation is very close to original rotation, stop recoiling
            if (Quaternion.Angle(transform.localRotation, originalRotation) < 1f)
            {
                transform.localRotation = originalRotation;
            }
        }
        
    }

    void Recoil()
    {
        // Calculate random recoil rotation
        float recoilX = Random.Range(0, maxRecoilX);
        float recoilY = Random.Range(-maxRecoilY, maxRecoilY);

        // Clamp the recoil values to stay within the limits
        recoilX = Mathf.Clamp(recoilX, -maxRotationLimitX, maxRotationLimitX);
        recoilY = Mathf.Clamp(recoilY, -maxRotationLimitY, maxRotationLimitY);

        recoilRotation = Quaternion.Euler(recoilX, recoilY, 0f);

        // Set recoiling flag
        isRecoiling = true;
    }
}

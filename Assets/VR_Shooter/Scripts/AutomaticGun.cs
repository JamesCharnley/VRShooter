using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutomaticGun : Gun, ILocomotionHand
{
    [SerializeField] private float shootInterval = 0.5f;

    private bool canShoot = true;
    private bool triggerPulled = false;
    private bool isFirstShot = false;
    private bool isLastShot = false;
    private float timeLastShot;
    protected override void TriggerPulled()
    {
        isFirstShot = true;
        triggerPulled = true;
        
    }
    
    protected override void TriggerReleased()
    {
        isLastShot = true;
        triggerPulled = false;
        canShoot = true;
    }
    private void Update()
    {
        if (triggerPulled || isLastShot)
        {
            TryShoot();
        }
    }

    protected override void TryShoot()
    {
        if (canShoot)
        {
            if (magazineSocket.UseAmmo())
            {
                float timeSinceLastShot = Time.time - timeLastShot;
                canShoot = false;
                if (isFirstShot)
                {
                    Shoot(true, false);
                    isFirstShot = false;
                }
                else if (isLastShot)
                {
                    if (timeSinceLastShot > 0.1f)
                    {
                        Shoot(false, true);
                    }
                    else
                    {
                        {
                            Shoot(false, true, true);
                        }
                    }
                    isLastShot = false;
                }
                else
                {
                    Shoot();
                }
                timeLastShot = Time.time;
                StartCoroutine(ShootCooldown());
            }
            else
            {
                DryFire();
                canShoot = false;
            }
            
        }
        
    }

   
    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
    }

    [SerializeField] private RecoilData recoilValues;
    public RecoilData RecoilValues { get => recoilValues; set => recoilValues = value; }
}

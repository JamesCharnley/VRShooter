using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    None,
    M1911,
    Shotgun,
    AK
}

public class Gun : GrabableObject
{
    public Action OnShoot;
    [SerializeField] private Weapon weapon;
    public Weapon Weapon { get => weapon; private set => weapon = value; }
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletSoundPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private GameObject muzzleSmokePrefab;
    [SerializeField] private Transform bulletSpawn;

    [SerializeField] protected MagazineSocket magazineSocket;
    public bool IsReloading { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        OnGrabbed += ConnectHand;
        OnDropped += DisconnectHand;
    }
    
    public override FingerGroup GetFingerGroup(Hand _handSide, Transform _xrOrigin, Transform _handBone)
    {
        float originAngle =
            Vector3.SignedAngle(_xrOrigin.forward, transform.forward, _xrOrigin.forward);
        FingerGroup optimalFingerGroup = null;
        float lowestAngleDiff = 9999;
        foreach (FingerGroup fingerGroup in fingerGroups)
        {
            if (fingerGroup.GetHandType == _handSide)
            {
                float angleDiff = fingerGroup.GetOptimalAngleOffset - originAngle;
                if (Mathf.Abs(angleDiff) < lowestAngleDiff)
                {
                    optimalFingerGroup = fingerGroup;
                    lowestAngleDiff = Mathf.Abs(angleDiff);
                }
            }
        }

        if (optimalFingerGroup)
        {
            return optimalFingerGroup;
        }
        return fingerGroups[0];
    }

    protected virtual void ConnectHand()
    {
        ownerHand.OnTriggerActivate += TriggerPulled;
        ownerHand.OnTriggerRelease += TriggerReleased;
        ownerHand.OnPrimaryActivate += StartReload;
        ownerHand.OnPrimaryReleased += StopReload;
    }

    protected virtual void StopReload()
    {
        IsReloading = false;
    }

    protected virtual void StartReload()
    {
        IsReloading = true;
        Reload();
    }

    protected virtual void TriggerPulled()
    {
        TryShoot();
    }
    protected virtual void TriggerReleased()
    {
        
    }

    protected virtual void TryShoot()
    {
        if (magazineSocket.UseAmmo())
        {
            Shoot();
        }
        else
        {
            DryFire();
        }
       
    }

    protected virtual void DryFire()
    {
        Transform spawnTransform = bulletSpawn.transform;
        GameObject bulletSound = Instantiate(bulletSoundPrefab, spawnTransform.position, spawnTransform.rotation);
        if (bulletSound.TryGetComponent(out GunshotSoundFx _soundfx))
        {
            _soundfx.DryFire(weapon);
        }
    }

    protected virtual void Shoot(bool _isFirst = false, bool _isLast = false, bool _tailOnly = false)
    {
        OnShoot?.Invoke();
        Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Transform spawnTransform = bulletSpawn.transform;
        GameObject bulletSound = Instantiate(bulletSoundPrefab, spawnTransform.position, spawnTransform.rotation);
        if (bulletSound.TryGetComponent(out GunshotSoundFx _soundfx))
        {
            _soundfx.Fire(weapon, _isFirst, _isLast, _tailOnly);
        }
        Instantiate(muzzleFlashPrefab, spawnTransform.position, spawnTransform.rotation);
        Instantiate(muzzleSmokePrefab, spawnTransform.position, spawnTransform.rotation);
    }

    public void Reload()
    {
        Debug.LogError("Gun: Reload");
        magazineSocket.EjectMagazine();
    }

    protected virtual void DisconnectHand()
    {
        TriggerReleased();
        ownerHand.OnTriggerActivate -= TriggerPulled;
        ownerHand.OnTriggerRelease -= TriggerReleased;
    }

    private void OnDestroy()
    {
        OnGrabbed -= ConnectHand;
        OnDropped -= DisconnectHand;
    }
    
}

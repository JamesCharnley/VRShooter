using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum GunMagazineType
{
    M1911,
    AK47,
    BoltAction
}

public enum AmmoType
{
    BuckShot,
    pistol9mm,
    Rifle762
    
}
[System.Serializable]
public struct GunMagazine
{
    public int maxAmmo;
    public AmmoCount currentAmmo;
    public GunMagazineType currentMagazineType;
    public AmmoType[] compatibleAmmoTypes;
    public Weapon[] compatibleWeapons;
}
public class Ammunition : MonoBehaviour
{
    private void Start()
    {
        AmmoHolder ammoHolder = FindObjectOfType<AmmoHolder>();
        if (ammoHolder is not null)
        {
            ammoHolder.AddMagazine(currentMagazine);
        }
    }

    [SerializeField] protected GunMagazine currentMagazine;
    public GunMagazineType[] compatibleMagazineTypes;
    public AmmoType[] compatibleAmmoTypes;
    public bool UseAmmo()
    {
        if (currentMagazine.currentAmmo.count == 0) return false;
        currentMagazine.currentAmmo.count -= 1;
        return true;
    }

    public void Reload(GunMagazine _magazine)
    {
        Debug.LogError("Ammunition: Reload");
        currentMagazine = _magazine;
    }
}

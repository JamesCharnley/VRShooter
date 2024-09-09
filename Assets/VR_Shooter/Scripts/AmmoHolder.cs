using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct AmmoCount
{
    public AmmoType ammoType;
    public int count;

}
public class AmmoHolder : GrabableObject
{
    [SerializeField] private GrabHandler leftGrabHandler;
    [SerializeField] private GrabHandler rightGrabHandler;
    [SerializeField] private AmmoCount[] startAmmo;

    private Dictionary<AmmoType, int> availableAmmo = new();

    [SerializeField] private GunMagazine[] startMagazines;

    private Dictionary<GunMagazineType, GunMagazine> currentMagazines = new();
    // Start is called before the first frame update
    void Start()
    {
        foreach (AmmoCount ammoCount in startAmmo)
        {
            availableAmmo.Add(ammoCount.ammoType, ammoCount.count);
        }

        foreach (GunMagazine startMagazine in startMagazines)
        {
            AddMagazine(startMagazine);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Grab(GameObject _hand, AnchorPoint _anchorPoint)
    {
        Debug.LogError("AmmoHolder: Grab");
        Hand handSide = Hand.Left;
        if (_hand.TryGetComponent(out HandActions handActions))
        {
            if (handActions.GetHand == Hand.Right)
            {
                handSide = Hand.Right;
            }
        }

        if (handSide == Hand.Left)
        {
            TryGrabMagazine(rightGrabHandler, leftGrabHandler);
        }
        else
        {
            TryGrabMagazine(leftGrabHandler, rightGrabHandler);
        }
        
    }

    bool TryGrabMagazine(GrabHandler _gunHand, GrabHandler _reloadingHand)
    {
        Debug.LogError("AmmoHolder: TryGrabMagazine");
        GrabableObject grabbedObject = _gunHand.GetGrabbedObject();
        if (grabbedObject != null)
        {
            if (grabbedObject is Gun gun)
            {
                foreach (KeyValuePair<GunMagazineType,GunMagazine> mag in currentMagazines)
                {
                    bool isCompatible = false;
                    foreach (Weapon weapon in mag.Value.compatibleWeapons)
                    {
                        if (weapon == gun.Weapon)
                        {
                            isCompatible = true;
                        }
                    }

                    if (isCompatible)
                    {
                        GameObject prefab = PrefabDirectory.instance.GetMagazinePrefab(mag.Value.currentMagazineType);
                        if (prefab is not null)
                        {
                            Debug.LogError("TryGrabMagazine: Spawned prefab");
                            SpawnMagazine(prefab, _reloadingHand, mag.Value);
                            return true;
                        }
                        else
                        {
                            Debug.LogError("TryGrabMagazine: Prefab null");
                            return false;
                        }
                    }
                    
                }
                Debug.LogError("TryGrabMagazine: No compatible magazine found");
                return false;
            }
            Debug.LogError("TryGrabMagazine: GrabbedObject is not gun");
            return false;
        }
        Debug.LogError("TryGrabMagazine: GrabbedObject is null");
        return false;
    }

    void SpawnMagazine(GameObject _magazinePrefab, GrabHandler _hand, GunMagazine _magazineData)
    {
        Debug.LogError("AmmoHolder: SpawnMagazine");
        GameObject magazineGameObject = Instantiate(_magazinePrefab);
        if(magazineGameObject.TryGetComponent(out GrabableObject grabable))
        {
            grabable.Grab(_hand.gameObject, null);
        }

        if (magazineGameObject.TryGetComponent(out Magazine mag))
        {
            mag.SetAmmoHolder(this);
            AmmoType ammoType = _magazineData.currentAmmo.ammoType;
            if (availableAmmo.ContainsKey(ammoType))
            {
                if (availableAmmo[ammoType] >= _magazineData.maxAmmo)
                {
                    _magazineData.currentAmmo.count = _magazineData.maxAmmo;
                    availableAmmo[ammoType] -= _magazineData.maxAmmo;
                }
                else
                {
                    _magazineData.currentAmmo.count = availableAmmo[ammoType];
                    availableAmmo[ammoType] = 0;
                }
            }
            mag.SetMagazineData(_magazineData);
        }
    }

    public void AddAmmo(AmmoCount _ammo)
    {
        if (availableAmmo.ContainsKey(_ammo.ammoType))
        {
            availableAmmo[_ammo.ammoType] += _ammo.count;
            return;
        }
        availableAmmo.Add(_ammo.ammoType, _ammo.count);
    }

    public void RemoveAmmo(AmmoCount _ammo)
    {
        if (availableAmmo.ContainsKey(_ammo.ammoType))
        {
            availableAmmo[_ammo.ammoType] -= _ammo.count;
        }
    }

    public void AddMagazine(GunMagazine _magazine)
    {
        //Debug.LogError("AmmoHolder: AddMagazine");
        if (currentMagazines.ContainsKey(_magazine.currentMagazineType))
        {
            AddAmmo(_magazine.currentAmmo);
            return;
        }
        currentMagazines.Add(_magazine.currentMagazineType, _magazine);
    }

    
    
}

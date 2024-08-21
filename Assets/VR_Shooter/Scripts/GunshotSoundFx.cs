using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunshotSoundFx : MonoBehaviour
{
    [SerializeField] private AudioSource fire;
    [SerializeField] private AudioSource dryFire;

    public void Fire(Weapon _weapon, bool _isFirst = false, bool _isLast = false, bool _isTail = false)
    {
        if (_isFirst)
        {
            fire.clip = SoundDirectory.instance.GetFirstGunShotSound(_weapon);
        }
        else if (_isLast)
        {
            if (_isTail)
            {
                fire.clip = SoundDirectory.instance.GetTailGunShotSound(_weapon);
            }
            else
            {
                fire.clip = SoundDirectory.instance.GetLastGunShotSound(_weapon);
            }
            
        }
        else
        {
            fire.clip = SoundDirectory.instance.GetGunShotSound(_weapon);
        }
        
        dryFire.clip = SoundDirectory.instance.GetGunDryFireSound(_weapon);
        
        dryFire.Play();
        fire.Play();
    }
    public void DryFire(Weapon _weapon)
    {
        dryFire.clip = SoundDirectory.instance.GetGunDryFireSound(_weapon);
        dryFire.Play();
    }
}

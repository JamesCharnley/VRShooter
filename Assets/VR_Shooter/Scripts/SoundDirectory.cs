using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDirectory : MonoBehaviour
{
    public static SoundDirectory instance;

    [SerializeField] private AudioClip[] metalSolidImpacts;
    [SerializeField] private AudioClip[] concreteImpacts;
    [SerializeField] private AudioClip[] fleshImpacts;
    [SerializeField] private AudioClip[] m1911PistolShots;
    [SerializeField] private AudioClip[] m1911PistolDryFire;
    [SerializeField] private AudioClip[] shotgunShots;
    [SerializeField] private AudioClip akFirstShot;
    [SerializeField] private AudioClip akLastShot;
    [SerializeField] private AudioClip akTailShot;
    [SerializeField] private AudioClip[] akGunShots;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            Destroy(this);
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public AudioClip GetBulletImpactSound(SurfaceType _surfaceType)
    {
        switch (_surfaceType)
        {

            case SurfaceType.MetalSolid:
                return GetRandomSoundClip(metalSolidImpacts);
            case SurfaceType.Concrete:
                return GetRandomSoundClip(concreteImpacts);
            case SurfaceType.Flesh:
                return GetRandomSoundClip(fleshImpacts);
            default:
                return metalSolidImpacts[0];
        }
    }

    public AudioClip GetGunShotSound(Weapon _weapon)
    {
        switch (_weapon)
        {
            case Weapon.M1911:
                return GetRandomSoundClip(m1911PistolShots);
            case Weapon.Shotgun:
                return GetRandomSoundClip(shotgunShots);
            case Weapon.AK:
                return GetRandomSoundClip(akGunShots);
            default:
                return m1911PistolShots[0];
        }
    }

    public AudioClip GetFirstGunShotSound(Weapon _weapon)
    {
        switch (_weapon)
        {
            case Weapon.M1911:
                return GetRandomSoundClip(m1911PistolShots);
            case Weapon.Shotgun:
                return GetRandomSoundClip(shotgunShots);
            case Weapon.AK:
                return akFirstShot;
            default:
                return m1911PistolShots[0];
        }
    }
    public AudioClip GetLastGunShotSound(Weapon _weapon)
    {
        switch (_weapon)
        {
            case Weapon.M1911:
                return GetRandomSoundClip(m1911PistolShots);
            case Weapon.Shotgun:
                return GetRandomSoundClip(shotgunShots);
            case Weapon.AK:
                return akLastShot;
            default:
                return m1911PistolShots[0];
        }
    }
    
    public AudioClip GetTailGunShotSound(Weapon _weapon)
    {
        switch (_weapon)
        {
            case Weapon.M1911:
                return akTailShot;
            case Weapon.Shotgun:
                return akTailShot;
            case Weapon.AK:
                return akTailShot;
            default:
                return akTailShot;
        }
    }
    
    public AudioClip GetGunDryFireSound(Weapon _weapon)
    {
        switch (_weapon)
        {
            case Weapon.M1911:
                return GetRandomSoundClip(m1911PistolDryFire);
            default:
                return m1911PistolDryFire[0];
        }
    }
    AudioClip GetRandomSoundClip(AudioClip[] _clips)
    {
        int rand = Random.Range(0, _clips.Length);
        return _clips[rand];
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeSpeaker : MonoBehaviour
{
    private ShootingRangeControl control;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        control = FindObjectOfType<ShootingRangeControl>();
        control.OnResetTargets += ResetRangeSound;
        audioSource = GetComponent<AudioSource>();
    }

    private void ResetRangeSound()
    {
        audioSource.Play();
    }

    private void OnDestroy()
    {
        control.OnResetTargets -= ResetRangeSound;
    }
}

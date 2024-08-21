using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : AOEDamageForce, IDestructable, ITakeDamage
{
    [SerializeField] private float totalHealth = 50;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform explosionSpawn;

    public void Destruct()
    {
        Vector3 spawnPosition = transform.position;
        if (explosionSpawn is not null)
        {
            spawnPosition = explosionSpawn.position;
        }

        Instantiate(explosionPrefab, spawnPosition, transform.rotation);
        InitAoe();
        Destroy(gameObject);
    }

    public void TakeDamage(float _amount)
    {
        totalHealth -= _amount;
        if (totalHealth <= 0)
        {
            Destruct();
        }
    }
}

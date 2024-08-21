using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [SerializeField] private Transform[] points;
    
    private Action OnSpawn;
    // Start is called before the first frame update
    void Start()
    {
        OnSpawn += NewSpawn;
        StartCoroutine(WaitForSpawn());
    }

    private void NewSpawn()
    {
        StartCoroutine(WaitForSpawn());
    }

    IEnumerator WaitForSpawn()
    {
        yield return new WaitForSeconds(5);
        int ran = Random.Range(0, points.Length);
        for (int i = 0; i < ran; i++)
        {
            Instantiate(prefab, points[i].position, points[ran].rotation);
        }
        

        OnSpawn?.Invoke();
    }

    private void OnDestroy()
    {
        OnSpawn -= NewSpawn;
    }
}

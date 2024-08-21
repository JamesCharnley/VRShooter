using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct MagazinePrefab
{
    public GunMagazineType magazine;
    public GameObject prefab;
}
public class PrefabDirectory : MonoBehaviour
{
    public static PrefabDirectory instance;
    [SerializeField] private MagazinePrefab[] magazinePrefabs;

    private Dictionary<GunMagazineType, GameObject> magazinePrefabsDirectory = new();
    // Start is called before the first frame update
    void Start()
    {
        if (instance is not null)
        {
            Destroy(this);
        }

        instance = this;
        DontDestroyOnLoad(this);
        foreach (MagazinePrefab magazinePrefab in magazinePrefabs)
        {
            magazinePrefabsDirectory.Add(magazinePrefab.magazine, magazinePrefab.prefab);
        }
    }

    public GameObject GetMagazinePrefab(GunMagazineType _magazine)
    {
        if (magazinePrefabsDirectory.ContainsKey(_magazine))
        {
            return magazinePrefabsDirectory[_magazine];
        }

        return null;
    }
}

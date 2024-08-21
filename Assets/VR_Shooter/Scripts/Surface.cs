using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SurfaceType
{
    None,
    MetalSolid,
    MetalHollow,
    Wood,
    Concrete,
    Dirt,
    Flesh
}
public class Surface : MonoBehaviour
{
    [SerializeField] private SurfaceType type;

    public SurfaceType GetSurfaceType()
    {
        return type;
    }
}

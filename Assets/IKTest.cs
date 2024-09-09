using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IKPairs
{
    public Transform Point;
    public Transform Target;
}
public class IKTest : MonoBehaviour
{
    [SerializeField] private IKPairs[] pairs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (IKPairs ikPairs in pairs)
        {
            ikPairs.Point.transform.position = ikPairs.Target.transform.position;
            ikPairs.Point.transform.rotation = ikPairs.Target.transform.rotation;
        }
    }
}

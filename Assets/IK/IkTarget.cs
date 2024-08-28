using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class IkTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles =
            new Vector3(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}

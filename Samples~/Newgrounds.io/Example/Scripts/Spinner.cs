using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple script to make the "please wait" spinner rotate
public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * -240);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // keep the session from expiring while we play
        StartCoroutine(NGIO.KeepSessionAlive());
    }
}

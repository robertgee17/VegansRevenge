using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamera : MonoBehaviour
{
    Camera mainCam;
    Camera uiCam;
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        uiCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        uiCam.fieldOfView = mainCam.fieldOfView;
    }
}

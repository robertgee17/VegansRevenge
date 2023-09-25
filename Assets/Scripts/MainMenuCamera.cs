using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public float rotateAngle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = rotateAngle * Time.deltaTime;
        transform.RotateAround(Vector3.zero, Vector3.up, angle);
    }
}

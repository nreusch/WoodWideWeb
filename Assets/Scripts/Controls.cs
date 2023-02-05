using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private float minFov = 20;
    [SerializeField] private float maxFov = 120;
    [SerializeField] private float camSpeed = 3;

    void Start(){
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if(scrollInput > 0)
            UpdateFov(cam.fieldOfView - 1 * camSpeed);
        else if(scrollInput < 0)
            UpdateFov(cam.fieldOfView + 1 * camSpeed);    
    }

    void UpdateFov(float newFov)
    {
        cam.fieldOfView = Mathf.Clamp(newFov, minFov, maxFov);
    }
}

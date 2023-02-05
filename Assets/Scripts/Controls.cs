using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private float minFov = 20;
    [SerializeField] private float maxFov = 120;
    [SerializeField] private float camSpeed = 3;
	public float mouseSensitivity = 0.01f;
 	[SerializeField] private Vector3 lastPosition;

    void Start(){
    }

    void Update()
    {
		UpdateCamZoom();
		UpdateCanPan();
    }

    void UpdateCamZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if(scrollInput > 0)
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - 1 * camSpeed, minFov, maxFov);
        else if(scrollInput < 0)
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView + 1 * camSpeed, minFov, maxFov);
    }

	void UpdateCanPan()
	{
		if (Input.GetMouseButtonDown(2))
		{
			lastPosition = Input.mousePosition;
		}
	
		if (Input.GetMouseButton(2))
		{
			Vector3 delta = Input.mousePosition - lastPosition;
			cam.transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
			lastPosition = Input.mousePosition;
		}
	}
}

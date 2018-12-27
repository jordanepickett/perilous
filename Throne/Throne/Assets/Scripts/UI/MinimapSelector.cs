using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSelector : MonoBehaviour {

    Camera minimapCamera;

    private Camera gCamera;

    public Camera GetGCamera()
    {
        return gCamera;
    }

    public void SetGCamera(Camera camera)
    {
        gCamera = camera;
    }

	// Use this for initialization
	void Start () {
        minimapCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        InputListen();
	}

    void InputListen()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            RaycastHit hit;
            Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)  && hit.transform.tag=="MinimapPlane")
            {
                if(gCamera)
                {
                    gCamera.transform.position = new Vector3(hit.point.x, gCamera.transform.position.y, hit.point.z - 7);
                    // hit.point contains the point where the ray hits the
                    // object named "MinimapBackground"
                    Debug.Log(hit.point);
                }
            }
            //itsMainCamera.transform.position = Vector3.Lerp(itsMainCamera.transform.position, hit.point, 0.1f);
        }
    }
}

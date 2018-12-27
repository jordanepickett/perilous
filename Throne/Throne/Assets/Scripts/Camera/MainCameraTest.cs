using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainCameraTest : MonoBehaviour {

    //outside of the update loop
    float cameraMovementSpeed = 35f;
    int scrollBorder = 10; // instead of the gui-rect-components
    Vector3 movement = Vector3.zero;
    bool staticCamera = false;

    public int teamId;

    public PlayerController player;

    void Awake()
    {
        //transform.position = new Vector3(player.transform.position.x, 12, player.transform.position.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if(staticCamera == true)
            {
                staticCamera = false;
            }
            else
            {
                staticCamera = true;
            }

        }
        if(!staticCamera)
        {

            //inside the update loop:
            Vector3 currentPosition = Camera.main.transform.position;
            Vector3 targetPosition = currentPosition + movement;
            // bounds left and right
            if (Input.mousePosition.x < scrollBorder) movement.x = -1; // because the screen x left bound is anyway 0 the screen.width isnt needed
            else if (Input.mousePosition.x > Screen.width - scrollBorder) movement.x = 1;
            else movement.x = 0;
            // bounds top and bottom
            if (Input.mousePosition.y < scrollBorder) movement.z = -1;// same as above: screen.width isnt needed
            else if (Input.mousePosition.y > Screen.height - scrollBorder) movement.z = 1;
            else movement.z = 0;
            if (currentPosition != targetPosition)
            {
                Camera.main.transform.position =
                Vector3.MoveTowards(currentPosition, targetPosition, cameraMovementSpeed * Time.deltaTime);
            }
        }
    
    }
}

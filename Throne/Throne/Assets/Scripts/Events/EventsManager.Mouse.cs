using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EventsManager
{
    bool isSelecting = false;
    bool leftClickDown = false;
    Vector3 mousePosition1;

    public void Start()
    {
        gameCamera = Camera.main.GetComponent<Camera>();
    }
    public void CheckMouseClicks()
    {
        // If we press the left mouse button, save mouse location and begin selection
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition1 = Input.mousePosition;
            leftClickDown = true;

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {

                LeftMouseClick(hit);
            }
        }

        if(leftClickDown == true)
        {
            if (Vector3.Distance(mousePosition1, Input.mousePosition) > 25f)
            {
                isSelecting = true;
            }
        }
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0))
        {
            leftClickDown = false;
            if(isSelecting)
            {
                isSelecting = false;
                SelectionComplete();
            }
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8 | 1 << 9 | 1 << 12 | 1 << 13))
        //    {
        //        //Right Clicked on unit or building
        //        if (MouseClick != null)
        //        {
        //            MouseClick(this, new RightButton_Handler((int)Input.mousePosition.x, (int)Input.mousePosition.y, 1, hit.collider.gameObject.GetComponent<RTSObject>()));
        //        }
        //    }
        //    else if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 11 | 1 << 18))
        //    {
        //        //Right clicked on terrain
        //        if (MouseClick != null)
        //        {
        //            MouseClick(this, new RightButton_Handler((int)Input.mousePosition.x, (int)Input.mousePosition.y, 1, hit.point));
        //        }
        //    }
        //}

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.gameObject.tag == "Ground")
                {

                    RightMouseClick(hit.point);
                }
                else
                {
                    if(hit.collider.gameObject.GetComponent<GatherNode>())
                    {
                        RightMouseClickNode(hit);
                    }
                    else if(hit.collider.gameObject.GetComponent<RtsObject>())
                    {
                        RightMouseClickUnit(hit);
                    }
                }
            }
        }
    }

    public void SelectionComplete()
    {
        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);
        MouseSelection(viewportBounds);
    }
}

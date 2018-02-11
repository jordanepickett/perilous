using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EventsManager
{
    bool isSelecting = false;
    Vector3 mousePosition1;

    public void CheckMouseClicks()
    {
        // If we press the left mouse button, save mouse location and begin selection
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;

            if (LeftMouseClick != null)
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {

                    LeftMouseClick(hit.point);
                }
            }
        }
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            SelectionComplete();
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
                RightMouseClick(hit.point);
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

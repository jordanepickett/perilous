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
        }
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            SelectionComplete();
        }
    }

    public void SelectionComplete()
    {
        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);
        MouseSelection(viewportBounds);
    }
}

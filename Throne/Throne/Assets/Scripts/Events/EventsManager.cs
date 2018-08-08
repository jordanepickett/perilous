using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MouseActions(object sender);
public delegate void MouseSelection(Bounds viewPortBounds);
public delegate void RightMouseClick(Vector3 hit);
public delegate void RightMouseClickObject(RaycastHit hit);
public delegate void LeftMouseClick(Vector3 hit);

public delegate void ScreenEdgeActions(object sender, ScreenEdgeEventArgs e);

public partial class EventsManager : MonoBehaviour, IEventsManager {

    public event MouseActions MouseClick;
    public event MouseSelection MouseSelection;
    public event RightMouseClick RightMouseClick;
    public event RightMouseClickObject RightMouseClickObject;
    public event LeftMouseClick LeftMouseClick;
    public event ScreenEdgeActions ScreenEdgeMousePosition;
    public Camera gameCamera;

    public static EventsManager main;

    void Awake()
    {
        main = this;
        StartCoroutine(FindCamera());
    }

    private IEnumerator FindCamera()
    {
        yield return new WaitForSeconds(2);
        Start();
    }

    private void LateUpdate()
    {
        CheckMouseClicks();
        CheckScreenEdgeEvents();
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

}

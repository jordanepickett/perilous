using UnityEngine;
using System.Collections;

public interface IEventsManager
{

    event MouseActions MouseClick;
    event ScreenEdgeActions ScreenEdgeMousePosition;
}

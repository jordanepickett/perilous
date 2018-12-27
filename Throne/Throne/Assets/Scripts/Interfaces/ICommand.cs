using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Icommand {

    void InitializeCommand();
    UnitCommands GetUnitCommand();
    KeyCode GetKeyBind();
    Vector3 GetPoint();
    void SetPoint(Vector3 point);
    GameObject GetUnit();
    void SetUnit(GameObject newUnit);
    void KeyBindCommand();
    int GetBuildTime();
}

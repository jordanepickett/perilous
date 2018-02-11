using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Icommand {

    void InitializeCommand();
    UnitCommands GetUnitCommand();
    KeyCode GetKeyBind();
    Vector3 GetPoint();
    GameObject GetUnit();
    void KeyBindCommand();
}

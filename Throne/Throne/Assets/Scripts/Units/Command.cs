﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command {

    protected UnitCommands unitCommand;

    protected KeyCode keyBind;

    protected GameObject Unit;

    public void SetUnit(GameObject newUnit)
    {
        Unit = newUnit;
    }

    public KeyCode GetKeyBind()
    {
        return keyBind;
    }

    public void SetKeyBind(KeyCode newKeyBind)
    {
        keyBind = newKeyBind;
    }

    protected Vector3 point;

    public Vector3 GetPoint()
    {
        return point;
    }
}

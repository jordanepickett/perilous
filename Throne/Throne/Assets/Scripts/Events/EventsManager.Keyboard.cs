using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EventsManager {

    void CheckKeyBoardPresses()
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isShiftKeyDown)
            {
                SelectionManager.main.AddToBoundedUnits(1);
            }
            else
            {
                SelectionManager.main.SelectBoundUnits(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isShiftKeyDown)
            {
                SelectionManager.main.AddToBoundedUnits(2);
            }
            else
            {
                SelectionManager.main.SelectBoundUnits(2);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (isShiftKeyDown)
            {
                SelectionManager.main.AddToBoundedUnits(3);
            }
            else
            {
                SelectionManager.main.SelectBoundUnits(3);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (isShiftKeyDown)
            {
                SelectionManager.main.AddToBoundedUnits(4);
            }
            else
            {
                SelectionManager.main.SelectBoundUnits(4);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (isShiftKeyDown)
            {
                SelectionManager.main.AddToBoundedUnits(5);
            }
            else
            {
                SelectionManager.main.SelectBoundUnits(5);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (isShiftKeyDown)
            {
                SelectionManager.main.AddToBoundedUnits(6);
            }
            else
            {
                SelectionManager.main.SelectBoundUnits(6);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (isShiftKeyDown)
            {
                SelectionManager.main.AddToBoundedUnits(7);
            }
            else
            {
                SelectionManager.main.SelectBoundUnits(7);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (isShiftKeyDown)
            {
                SelectionManager.main.AddToBoundedUnits(8);
            }
            else
            {
                SelectionManager.main.SelectBoundUnits(8);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (isShiftKeyDown)
            {
                SelectionManager.main.AddToBoundedUnits(9);
            }
            else
            {
                SelectionManager.main.SelectBoundUnits(9);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (isShiftKeyDown)
            {
                SelectionManager.main.AddToBoundedUnits(0);
            }
            else
            {
                SelectionManager.main.SelectBoundUnits(0);
            }
        }
    }
}

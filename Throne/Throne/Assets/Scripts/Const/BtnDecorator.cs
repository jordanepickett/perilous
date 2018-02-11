using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class BtnDecorator {

	public static GameObject Decorate(GameObject obj)
    {
        UnitCommands command = obj.GetComponent<CommandUiSender>().GetCommand().GetUnitCommand();
        switch (command)
        {
            case (UnitCommands.Move):
                obj.GetComponent<Button>().GetComponentInChildren<Text>().text = "Move";
                break;
            case (UnitCommands.Hold):
                obj.GetComponent<Button>().GetComponentInChildren<Text>().text = "Stop";
                break;
        }

        return obj;
    }

    public static GameObject BuildDecorate(GameObject obj)
    {
        obj.GetComponent<Button>().GetComponentInChildren<Text>().text = "Build";
        return obj;
    }
}

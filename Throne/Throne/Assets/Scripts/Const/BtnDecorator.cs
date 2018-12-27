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
            case (UnitCommands.Attack):
                obj.GetComponent<Button>().GetComponentInChildren<Text>().text = "Attack";
                break;
            case (UnitCommands.Gather):
                obj.GetComponent<Button>().GetComponentInChildren<Text>().text = "Gather";
                break;
            default:
                obj.GetComponent<Button>().GetComponentInChildren<Text>().text = "NI";
                break;

        }

        return obj;
    }

    public static GameObject BuildDecorate(GameObject obj)
    {
        obj.GetComponent<Button>().GetComponentInChildren<Text>().text = "Build";
        return obj;
    }

    public static GameObject KeyBindDecorate(GameObject obj)
    {
        string keybind = obj.GetComponent<CommandUiSender>().GetCommand().GetKeyBind().ToString();
        obj.GetComponent<Button>().GetComponentInChildren<Text>().color = Color.white;
        obj.GetComponent<Button>().GetComponentInChildren<Text>().text = keybind;
        return obj;
    }
}

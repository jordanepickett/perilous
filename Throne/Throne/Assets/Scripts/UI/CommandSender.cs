using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSender : MonoBehaviour {

    private Command command;
    private bool isReadyForLocation = false;

    public Command GetCommand()
    {
        return command;
    }

    public void SetCommand(Command newCommand)
    {
        command = newCommand;
    }

    private Button button;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(ReadyUpCommand);
        EventsManager.main.LeftMouseClick += SendCommand;
	}

    void ReadyUpCommand()
    {
        if(isReadyForLocation == true)
        {
            isReadyForLocation = false;
        }
        else
        {
            isReadyForLocation = true;
        }
    }

    void SendCommand(Vector3 point)
    {
        if(isReadyForLocation)
        {
            SetCommand(Commands.CreateMoveOrder(point));
            foreach (SelectableUnit unit in SelectionManager.main.GetSelectedUnits())
            {
                MovementManager.main.GiveMovementCommand(command.Location);
            }
            isReadyForLocation = false;
        }
    }

}

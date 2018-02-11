using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUiSender : MonoBehaviour {

    private Icommand command;
    private bool isReadyForLocation = false;

    public Icommand GetCommand()
    {
        return command;
    }

    public void SetCommand(Icommand newCommand)
    {
        command = newCommand;
    }

    private Button button;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetUpCommand);
	}

    void Update()
    {
        if(Input.GetKeyDown(GetCommand().GetKeyBind()))
        {
            GetCommand().KeyBindCommand();
        }
    }

    void SetUpCommand()
    {
        command.InitializeCommand();
    }
}
